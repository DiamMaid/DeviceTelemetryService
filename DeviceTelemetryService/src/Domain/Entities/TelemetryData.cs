namespace Domain.Entities;

/// <summary>
/// Сущность данных телеметрии от IoT-устройства.
/// </summary>
public class TelemetryData
{
    /// <summary>
    /// Уникальный идентификатор записи телеметрии.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Идентификатор устройства, отправившего данные.
    /// </summary>
    public Guid DeviceId { get; set; }

    /// <summary>
    /// Временная метка измерения (когда датчик снял показания).
    /// </summary>
    public DateTime Timestamp { get; set; }

    /// <summary>
    /// Тип метрики (температура, давление, вибрация и т.д.).
    /// </summary>
    public string MetricName { get; set; } = string.Empty;

    /// <summary>
    /// Значение метрики.
    /// </summary>
    public double MetricValue { get; set; }

    /// <summary>
    /// Единица измерения (°C, бары, м/с², % и т.д.).
    /// </summary>
    public string? Unit { get; set; }

    /// <summary>
    /// Дополнительные данные в формате JSON (координаты, качество сигнала...).
    /// </summary>
    public string? Metadata { get; set; }

    /// <summary>
    /// Дата и время получения данных сервером.
    /// </summary>
    public DateTime ReceivedAt { get; set; }
}