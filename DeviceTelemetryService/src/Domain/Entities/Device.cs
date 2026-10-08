namespace Domain.Entities;

using Domain.Enums;

/// <summary>
/// Сущность IoT-устройства в системе телеметрии.
/// </summary>
public class Device
{
    /// <summary>
    /// Уникальный идентификатор устройства.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Имя устройства (например, "Temperature Sensor #42").
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Тип датчика устройства.
    /// </summary>
    public SensorType Type { get; set; }

    /// <summary>
    /// Текущий статус устройства.
    /// </summary>
    public DeviceStatus Status { get; set; }

    /// <summary>
    /// Местоположение устройства (цех, этаж, координаты).
    /// Может быть null, если не указано.
    /// </summary>
    public string? Location { get; set; }

    /// <summary>
    /// Дата и время регистрации устройства в системе.
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Дата и время последнего heartbeat-сигнала от устройства.
    /// Null, если устройство еще не отправляло heartbeat.
    /// </summary>
    public DateTime? LastHeartbeat { get; set; }
}