using Domain.Interfaces;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Infrastructure.Persistence;
using Dapper;
using Npgsql;

namespace Infrastructure.Persistence.Repositories;

/// <summary>
/// Реализация репозитория телеметрии через EF Core и Dapper.
/// </summary>
public class TelemetryRepository : ITelemetryReader, ITelemetryWriter
{
    private readonly ApplicationDbContext _context;
    private readonly string _connectionString;

    /// <summary>
    /// Создает новый экземпляр репозитория телеметрии.
    /// </summary>
    /// <param name="context">EF Core контекст для операций чтения.</param>
    /// <param name="configuration">Конфигурация для получения строки подключения.</param>
    public TelemetryRepository(ApplicationDbContext context, IConfiguration configuration)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _connectionString = configuration.GetConnectionString("DefaultConnection") 
            ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found");
    }

    // === ITelemetryReader методы (EF Core) ===

    /// <summary>
    /// Получает данные телеметрии по уникальному идентификатору.
    /// </summary>
    public async Task<TelemetryData?> GetByIdAsync(Guid id)
    {
        return await _context.TelemetryData
            .AsNoTracking()
            .FirstOrDefaultAsync(t => t.Id == id);
    }

    /// <summary>
    /// Получает последние N записей телеметрии для устройства.
    /// </summary>
    public async Task<List<TelemetryData>> GetLatestForDeviceAsync(Guid deviceId, int count)
    {
        if (deviceId == Guid.Empty)
            throw new ArgumentException("Device ID cannot be empty", nameof(deviceId));
        if (count <= 0)
            throw new ArgumentOutOfRangeException(nameof(count), "Count must be positive");

        const int maxCount = 1000;
        if (count > maxCount)
            throw new ArgumentOutOfRangeException(nameof(count), $"Count cannot exceed {maxCount}");

        return await _context.TelemetryData
            .AsNoTracking()
            .Where(t => t.DeviceId == deviceId)
            .OrderByDescending(t => t.Timestamp)
            .Take(count)
            .ToListAsync();
    }

    /// <summary>
    /// Получает данные телеметрии за указанный период для устройства.
    /// </summary>
    public async Task<List<TelemetryData>> GetByTimeRangeAsync(
        Guid deviceId, 
        DateTime startTime, 
        DateTime endTime)
    {
        if (deviceId == Guid.Empty)
            throw new ArgumentException("Device ID cannot be empty", nameof(deviceId));
        if (startTime > endTime)
            throw new ArgumentException("Start time must be before end time", nameof(startTime));

        return await _context.TelemetryData
            .AsNoTracking()
            .Where(t => t.DeviceId == deviceId && 
                       t.Timestamp >= startTime && 
                       t.Timestamp <= endTime)
            .OrderBy(t => t.Timestamp)
            .ToListAsync();
    }

    /// <summary>
    /// Получает агрегированные данные (среднее, мин, макс) за период.
    /// </summary>
    public async Task<Dictionary<string, (double Avg, double Min, double Max)>> GetAggregatedAsync(
        Guid deviceId,
        DateTime startTime,
        DateTime endTime)
    {
        if (deviceId == Guid.Empty)
            throw new ArgumentException("Device ID cannot be empty", nameof(deviceId));
        if (startTime > endTime)
            throw new ArgumentException("Start time must be before end time", nameof(startTime));

        var results = await _context.TelemetryData
            .AsNoTracking()
            .Where(t => t.DeviceId == deviceId && 
                       t.Timestamp >= startTime && 
                       t.Timestamp <= endTime)
            .GroupBy(t => t.MetricName)
            .Select(g => new 
            {
                MetricName = g.Key,
                Avg = g.Average(x => x.MetricValue),
                Min = g.Min(x => x.MetricValue),
                Max = g.Max(x => x.MetricValue)
            })
            .ToListAsync();

        return results.ToDictionary(
            r => r.MetricName,
            r => (r.Avg, r.Min, r.Max),
            StringComparer.OrdinalIgnoreCase
        );
    }

    // === ITelemetryWriter методы (Dapper) ===

    /// <summary>
    /// Сохраняет пакет данных телеметрии (батчинг через Dapper).
    /// </summary>
    public async Task SaveBatchAsync(IEnumerable<TelemetryData> telemetryDataList)
    {
        if (telemetryDataList == null)
            throw new ArgumentNullException(nameof(telemetryDataList));

        var list = telemetryDataList.ToList();
        if (!list.Any())
            return;

        using var connection = new NpgsqlConnection(_connectionString);
        await connection.OpenAsync();

        using var transaction = await connection.BeginTransactionAsync();
        try
        {
            const string sql = @"
                INSERT INTO telemetry_data (time, device_id, metric_name, metric_value, unit, tags, created_at)
                VALUES (@Timestamp, @DeviceId, @MetricName, @MetricValue, @Unit, CAST(@Metadata AS jsonb), @ReceivedAt)";

            await connection.ExecuteAsync(sql, list, transaction);
            await transaction.CommitAsync();
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    /// <summary>
    /// Сохраняет одну запись телеметрии.
    /// </summary>
    public async Task SaveAsync(TelemetryData telemetryData)
    {
        if (telemetryData == null)
            throw new ArgumentNullException(nameof(telemetryData));

        using var connection = new NpgsqlConnection(_connectionString);
        await connection.OpenAsync();

        const string sql = @"
            INSERT INTO telemetry_data (time, device_id, metric_name, metric_value, unit, tags, created_at)
            VALUES (@Timestamp, @DeviceId, @MetricName, @MetricValue, @Unit, CAST(@Metadata AS jsonb), @ReceivedAt)";

        await connection.ExecuteAsync(sql, telemetryData);
    }

    /// <summary>
    /// Удаляет старые данные телеметрии (политика хранения).
    /// </summary>
    public async Task DeleteOlderThanAsync(DateTime cutoffDate)
    {
        using var connection = new NpgsqlConnection(_connectionString);
        await connection.OpenAsync();

        const string sql = "DELETE FROM telemetry_data WHERE time < @CutoffDate";
        await connection.ExecuteAsync(sql, new { CutoffDate = cutoffDate });
    }
}
