namespace Application.Telemetry.Queries.GetTelemetryByTimeRange;

using MediatR;
using Domain.Interfaces;
using Domain.Entities;

public record GetTelemetryByTimeRangeQuery(Guid DeviceId, DateTime StartTime, DateTime EndTime) : IRequest<List<TelemetryData>>;

public class GetTelemetryByTimeRangeQueryHandler : IRequestHandler<GetTelemetryByTimeRangeQuery, List<TelemetryData>>
{
    private readonly ITelemetryReader _telemetryReader;

    public GetTelemetryByTimeRangeQueryHandler(ITelemetryReader telemetryReader)
    {
        _telemetryReader = telemetryReader;
    }

    public async Task<List<TelemetryData>> Handle(GetTelemetryByTimeRangeQuery request, CancellationToken cancellationToken)
    {
        return await _telemetryReader.GetByTimeRangeAsync(request.DeviceId, request.StartTime, request.EndTime);
    }
}
