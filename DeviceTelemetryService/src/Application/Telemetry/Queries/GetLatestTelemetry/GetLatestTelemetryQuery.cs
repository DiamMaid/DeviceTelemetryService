namespace Application.Telemetry.Queries.GetLatestTelemetry;

using MediatR;
using Domain.Interfaces;
using Domain.Entities;

public record GetLatestTelemetryQuery(Guid DeviceId, int Count = 10) : IRequest<List<TelemetryData>>;

public class GetLatestTelemetryQueryHandler : IRequestHandler<GetLatestTelemetryQuery, List<TelemetryData>>
{
    private readonly ITelemetryReader _telemetryReader;

    public GetLatestTelemetryQueryHandler(ITelemetryReader telemetryReader)
    {
        _telemetryReader = telemetryReader;
    }

    public async Task<List<TelemetryData>> Handle(GetLatestTelemetryQuery request, CancellationToken cancellationToken)
    {
        return await _telemetryReader.GetLatestForDeviceAsync(request.DeviceId, request.Count);
    }
}
