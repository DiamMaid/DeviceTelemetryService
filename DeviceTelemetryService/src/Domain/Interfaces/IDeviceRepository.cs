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

/// <summary>
/// Полный контракт репозитория устройств, объединяющий чтение и запись.
/// </summary>
public interface IDeviceRepository : IDeviceReader, IDeviceWriter
{
}