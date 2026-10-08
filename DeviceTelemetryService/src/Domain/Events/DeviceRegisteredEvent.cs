namespace Domain.Events;

using Domain.Entities;

/// <summary>
/// Доменное событие, возникающее при регистрации нового устройства.
/// </summary>
public class DeviceRegisteredEvent
{
    /// <summary>
    /// Зарегистрированное устройство.
    /// </summary>
    public Device Device { get; }

    /// <summary>
    /// Временная метка события.
    /// </summary>
    public DateTime OccurredAt { get; }

    /// <summary>
    /// Создает новое событие регистрации устройства.
    /// </summary>
    /// <param name="device">Зарегистрированное устройство.</param>
    public DeviceRegisteredEvent(Device device)
    {
        Device = device;
        OccurredAt = DateTime.UtcNow;
    }
}