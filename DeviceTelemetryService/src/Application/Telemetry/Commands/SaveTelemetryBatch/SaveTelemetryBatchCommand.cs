namespace Application.Telemetry.Commands.SaveTelemetryBatch;

using MediatR;
using Domain.Interfaces;
using Domain.Entities;

/// <summary>
/// DTO для одной записи телеметрии в пакете.
/// </summary>
public record TelemetryRecordDto(
    Guid DeviceId,
    DateTime Timestamp,
    string MetricName,
    double MetricValue,
    string? Unit,
    string? Metadata
);

/// <summary>
/// Команда для пакетного сохранения данных телеметрии.
/// </summary>
public record SaveTelemetryBatchCommand(IEnumerable<TelemetryRecordDto> Records) : IRequest<Unit>;

/// <summary>
/// Обработчик команды пакетного сохранения телеметрии.
/// </summary>
public class SaveTelemetryBatchCommandHandler : IRequestHandler<SaveTelemetryBatchCommand, Unit>
{
    private readonly ITelemetryWriter _telemetryWriter;

    public SaveTelemetryBatchCommandHandler(ITelemetryWriter telemetryWriter)
    {
        _telemetryWriter = telemetryWriter;
    }

    public async Task<Unit> Handle(SaveTelemetryBatchCommand request, CancellationToken cancellationToken)
    {
        // Преобразуем DTO в сущности TelemetryData
        var telemetryDataList = request.Records.Select(record => new TelemetryData
        {
            Id = Guid.NewGuid(),
            DeviceId = record.DeviceId,
            Timestamp = record.Timestamp,
            MetricName = record.MetricName,
            MetricValue = record.MetricValue,
            Unit = record.Unit,
            Metadata = record.Metadata,
            ReceivedAt = DateTime.UtcNow
        });

        // Сохраняем пакетом (быстрее, чем по одной записи)
        await _telemetryWriter.SaveBatchAsync(telemetryDataList);

        return Unit.Value;
    }
}
