namespace Domain.Interfaces;

using Domain.Entities;

/// <summary>
/// Контракт для операций чтения данных об устройствах.
/// </summary>
public interface IDeviceReader
{
    /// <summary>
    /// Получает устройство по уникальному идентификатору.
    /// </summary>
    /// <param name="id">Уникальный идентификатор устройства.</param>
    /// <returns>Устройство или null, если не найдено.</returns>
    Task<Device?> GetByIdAsync(Guid id);

    /// <summary>
    /// Получает список всех зарегистрированных устройств.
    /// </summary>
    /// <returns>Список всех устройств.</returns>
    Task<List<Device>> GetAllAsync();

    /// <summary>
    /// Ищет устройство по имени.
    /// </summary>
    /// <param name="name">Имя устройства для поиска.</param>
    /// <returns>Устройство или null, если не найдено.</returns>
    Task<Device?> GetByNameAsync(string name);
}
