namespace Domain.Enums;

/// <summary>
/// Тип IoT-датчика.
/// </summary>
public enum SensorType
{
    /// <summary>
    /// Датчик температуры (измеряет в °C или °F).
    /// </summary>
    Temperature = 1,

    /// <summary>
    /// Датчик давления (измеряет в барах или PSI).
    /// </summary>
    Pressure = 2,

    /// <summary>
    /// Датчик вибрации (измеряет ускорение в м/с²).
    /// </summary>
    Vibration = 3,

    /// <summary>
    /// Датчик влажности (измеряет в процентах).
    /// </summary>
    Humidity = 4,

    /// <summary>
    /// Датчик освещенности (измеряет в люксах).
    /// </summary>
    Light = 5,

    /// <summary>
    /// Датчик движения (бинарный: есть движение / нет).
    /// </summary>
    Motion = 6
}