using Microsoft.AspNetCore.Mvc;
using MediatR;
using Application.Telemetry.Commands.SaveTelemetryBatch;

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

    // POST: api/telemetry
    [HttpPost]
    public async Task<IActionResult> SaveBatch([FromBody] List<TelemetryRecordDto> records)
    {
        var command = new SaveTelemetryBatchCommand(records);
        await _mediator.Send(command);

        return Accepted();
    }
}
