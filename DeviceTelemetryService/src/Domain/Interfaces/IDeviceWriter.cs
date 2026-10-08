namespace Domain.Interfaces;

using Domain.Entities;

/// <summary>
/// Контракт для операций записи данных об устройствах.
/// </summary>
public interface IDeviceWriter
{
    /// <summary>
    /// Регистрирует новое устройство в системе.
    /// </summary>
    /// <param name="device">Данные устройства для регистрации.</param>
    /// <returns>Уникальный идентификатор созданного устройства.</returns>
    Task<Guid> CreateAsync(Device device);

    /// <summary>
    /// Обновляет информацию о существующем устройстве.
    /// </summary>
    /// <param name="device">Обновленные данные устройства.</param>
    Task UpdateAsync(Device device);

    /// <summary>
    /// Удаляет устройство из системы.
    /// </summary>
    /// <param name="id">Уникальный идентификатор устройства для удаления.</param>
    Task DeleteAsync(Guid id);
}
