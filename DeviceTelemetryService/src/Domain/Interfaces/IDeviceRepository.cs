namespace Domain.Interfaces;

/// <summary>
/// Полный контракт репозитория устройств, объединяющий чтение и запись.
/// </summary>
public interface IDeviceRepository : IDeviceReader, IDeviceWriter
{
}