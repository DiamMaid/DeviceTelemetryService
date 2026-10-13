using Microsoft.AspNetCore.Mvc;
using MediatR;
using Application.Telemetry.Commands.SaveTelemetryBatch;
using Application.Telemetry.Queries.GetLatestTelemetry;
using Application.Telemetry.Queries.GetTelemetryByTimeRange;
using Domain.Entities;

namespace DeviceTelemetryService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TelemetryController : ControllerBase
{
    private readonly IMediator _mediator;

    public TelemetryController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("{deviceId}/latest")]
    public async Task<ActionResult<List<TelemetryData>>> GetLatest(Guid deviceId, [FromQuery] int count = 10)
    {
        var query = new GetLatestTelemetryQuery(deviceId, count);
        var data = await _mediator.Send(query);

        return Ok(data);
    }

    [HttpGet("{deviceId}/range")]
    public async Task<ActionResult<List<TelemetryData>>> GetByTimeRange(
        Guid deviceId, 
        [FromQuery] DateTime startTime, 
        [FromQuery] DateTime endTime)
    {
        var query = new GetTelemetryByTimeRangeQuery(deviceId, startTime, endTime);
        var data = await _mediator.Send(query);

        return Ok(data);
    }

    [HttpPost]
    public async Task<IActionResult> SaveBatch([FromBody] List<TelemetryRecordDto> records)
    {
        var command = new SaveTelemetryBatchCommand(records);
        await _mediator.Send(command);

        return Accepted();
    }
}
