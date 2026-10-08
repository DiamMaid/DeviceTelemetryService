namespace Domain.Interfaces;

using Domain.Entities;

/// <summary>
/// Контракт для операций чтения данных телеметрии.
/// </summary>
public interface ITelemetryReader
{
    /// <summary>
    /// Получает данные телеметрии по уникальному идентификатору.
    /// </summary>
    Task<TelemetryData?> GetByIdAsync(Guid id);

    /// <summary>
    /// Получает последние N записей телеметрии для устройства.
    /// </summary>
    Task<List<TelemetryData>> GetLatestForDeviceAsync(Guid deviceId, int count);

    /// <summary>
    /// Получает данные телеметрии за указанный период для устройства.
    /// </summary>
    Task<List<TelemetryData>> GetByTimeRangeAsync(
        Guid deviceId, 
        DateTime startTime, 
        DateTime endTime);

    /// <summary>
    /// Получает агрегированные данные (среднее, мин, макс) за период.
    /// </summary>
    Task<Dictionary<string, (double Avg, double Min, double Max)>> GetAggregatedAsync(
        Guid deviceId,
        DateTime startTime,
        DateTime endTime);
}
