namespace Domain.Interfaces;

using Domain.Entities;

/// <summary>
/// Контракт для операций записи данных телеметрии.
/// </summary>
public interface ITelemetryWriter
{
    /// <summary>
    /// Сохраняет пакет данных телеметрии (батчинг).
    /// </summary>
    Task SaveBatchAsync(IEnumerable<TelemetryData> telemetryDataList);

    /// <summary>
    /// Сохраняет одну запись телеметрии.
    /// </summary>
    Task SaveAsync(TelemetryData telemetryData);

    /// <summary>
    /// Удаляет старые данные телеметрии (политика хранения).
    /// </summary>
    Task DeleteOlderThanAsync(DateTime cutoffDate);
}
