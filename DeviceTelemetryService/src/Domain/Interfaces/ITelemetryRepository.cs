namespace Domain.Interfaces;

/// <summary>
/// Полный контракт репозитория телеметрии, объединяющий чтение и запись.
/// </summary>
public interface ITelemetryRepository : ITelemetryReader, ITelemetryWriter
{
}