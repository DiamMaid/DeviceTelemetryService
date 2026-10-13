using Domain.Interfaces;
using Domain.Entities;
using Microsoft.Extensions.Configuration;
using Infrastructure.Persistence;
using Dapper;
using Npgsql;

namespace Infrastructure.Persistence.Repositories;

public class TelemetryRepository : ITelemetryReader, ITelemetryWriter
{
    private readonly string _connectionString;

    public TelemetryRepository(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found");

        // Настраиваем маппинг Dapper для snake_case колонок
        DefaultTypeMap.MatchNamesWithUnderscores = true;
    }

    public async Task<TelemetryData?> GetByIdAsync(Guid id)
    {
        using var connection = new NpgsqlConnection(_connectionString);
        const string sql = "SELECT * FROM telemetry_data WHERE id = @Id LIMIT 1";
        return await connection.QueryFirstOrDefaultAsync<TelemetryData>(sql, new { Id = id });
    }

    public async Task<List<TelemetryData>> GetLatestForDeviceAsync(Guid deviceId, int count)
    {
        if (deviceId == Guid.Empty)
            throw new ArgumentException("Device ID cannot be empty", nameof(deviceId));
        if (count <= 0)
            throw new ArgumentOutOfRangeException(nameof(count), "Count must be positive");

        const int maxCount = 1000;
        if (count > maxCount)
            throw new ArgumentOutOfRangeException(nameof(count), $"Count cannot exceed {maxCount}");

        using var connection = new NpgsqlConnection(_connectionString);
        const string sql = @"
            SELECT 
                device_id::uuid AS DeviceId,
                time AS Timestamp,
                metric_name AS MetricName,
                metric_value AS MetricValue,
                unit AS Unit,
                tags AS Metadata,
                created_at AS ReceivedAt
            FROM telemetry_data 
            WHERE device_id = @DeviceId::text 
            ORDER BY time DESC 
            LIMIT @Count";
        
        return (await connection.QueryAsync<TelemetryData>(sql, new { DeviceId = deviceId.ToString(), Count = count })).ToList();
    }

    public async Task<List<TelemetryData>> GetByTimeRangeAsync(
        Guid deviceId, 
        DateTime startTime, 
        DateTime endTime)
    {
        if (deviceId == Guid.Empty)
            throw new ArgumentException("Device ID cannot be empty", nameof(deviceId));
        if (startTime > endTime)
            throw new ArgumentException("Start time must be before end time", nameof(startTime));

        using var connection = new NpgsqlConnection(_connectionString);
        const string sql = @"
            SELECT 
                device_id::uuid AS DeviceId,
                time AS Timestamp,
                metric_name AS MetricName,
                metric_value AS MetricValue,
                unit AS Unit,
                tags AS Metadata,
                created_at AS ReceivedAt
            FROM telemetry_data 
            WHERE device_id = @DeviceId::text AND time >= @StartTime AND time <= @EndTime 
            ORDER BY time ASC";
        
        return (await connection.QueryAsync<TelemetryData>(sql, new { DeviceId = deviceId.ToString(), StartTime = startTime, EndTime = endTime })).ToList();
    }

    public async Task<Dictionary<string, (double Avg, double Min, double Max)>> GetAggregatedAsync(
        Guid deviceId,
        DateTime startTime,
        DateTime endTime)
    {
        if (deviceId == Guid.Empty)
            throw new ArgumentException("Device ID cannot be empty", nameof(deviceId));
        if (startTime > endTime)
            throw new ArgumentException("Start time must be before end time", nameof(startTime));

        using var connection = new NpgsqlConnection(_connectionString);
        const string sql = @"
            SELECT metric_name, 
                   AVG(metric_value) AS avg, 
                   MIN(metric_value) AS min, 
                   MAX(metric_value) AS max 
            FROM telemetry_data 
            WHERE device_id = @DeviceId::text AND time >= @StartTime AND time <= @EndTime 
            GROUP BY metric_name";

        var results = await connection.QueryAsync<dynamic>(sql, new { DeviceId = deviceId.ToString(), StartTime = startTime, EndTime = endTime });
        
        return results.ToDictionary(
            r => (string)r.metric_name,
            r => ((double)r.avg, (double)r.min, (double)r.max),
            StringComparer.OrdinalIgnoreCase
        );
    }

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

    public async Task DeleteOlderThanAsync(DateTime cutoffDate)
    {
        using var connection = new NpgsqlConnection(_connectionString);
        await connection.OpenAsync();

        const string sql = "DELETE FROM telemetry_data WHERE time < @CutoffDate";
        await connection.ExecuteAsync(sql, new { CutoffDate = cutoffDate });
    }
}
