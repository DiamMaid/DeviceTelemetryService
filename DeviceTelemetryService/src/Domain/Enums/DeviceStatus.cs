namespace Domain.Enums;

/// <summary>
/// Статус IoT-устройства в системе телеметрии.
/// </summary>
public enum DeviceStatus
{
    /// <summary>
    /// Устройство активно и регулярно отправляет heartbeat-сигналы.
    /// </summary>
    Online = 1,

    /// <summary>
    /// Устройство не отвечает на heartbeat-сигналы.
    /// </summary>
    Offline = 2,

    /// <summary>
    /// Устройство сообщает о внутренней ошибке или неисправности.
    /// </summary>
    Error = 3,

    /// <summary>
    /// Устройство временно отключено для технического обслуживания.
    /// </summary>
    Maintenance = 4
}
