namespace Application.Devices.DTOs;

/// <summary>
/// DTO для представления информации об IoT-устройстве.
/// </summary>
public class DeviceDto
{
    /// <summary>
    /// Уникальный идентификатор устройства.
    /// </summary>
    public Guid Id { get; init; }

    /// <summary>
    /// Имя устройства.
    /// </summary>
    public string Name { get; init; } = string.Empty;

    /// <summary>
    /// Тип датчика (Temperature, Pressure, Vibration и т.д.).
    /// </summary>
    public string Type { get; init; } = string.Empty;

    /// <summary>
    /// Текущий статус устройства (Online, Offline, Error, Maintenance).
    /// </summary>
    public string Status { get; init; } = string.Empty;

    /// <summary>
    /// Местоположение устройства.
    /// Может быть null, если не указано.
    /// </summary>
    public string? Location { get; init; }

    /// <summary>
    /// Дата и время регистрации устройства.
    /// </summary>
    public DateTime CreatedAt { get; init; }

    /// <summary>
    /// Дата и время последнего heartbeat-сигнала.
    /// Null, если устройство еще не отправляло сигналы.
    /// </summary>
    public DateTime? LastHeartbeat { get; init; }
}
