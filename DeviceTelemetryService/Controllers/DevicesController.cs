using Microsoft.AspNetCore.Mvc;
using MediatR;
using Application.Devices.Commands.CreateDevice;
using Application.Devices.Commands.UpdateDevice;
using Application.Devices.Commands.DeleteDevice;
using Application.Devices.Queries.GetDeviceById;
using Application.Devices.Queries.GetAllDevices;
using Application.Devices.DTOs;

namespace DeviceTelemetryService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DevicesController : ControllerBase
{
    private readonly IMediator _mediator;

    public DevicesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    // GET: api/devices
    [HttpGet]
    public async Task<ActionResult<List<DeviceDto>>> GetAll()
    {
        var query = new GetAllDevicesQuery();
        var devices = await _mediator.Send(query);

        return Ok(devices);
    }

    // GET: api/devices/{id}
    [HttpGet("{id}")]
    public async Task<ActionResult<DeviceDto>> GetById(Guid id)
    {
        var query = new GetDeviceByIdQuery(id);
        var device = await _mediator.Send(query);

        if (device == null)
            return NotFound();

        return Ok(device);
    }

    // POST: api/devices
    [HttpPost]
    public async Task<ActionResult<Guid>> Create([FromBody] CreateDeviceDto dto)
    {
        var command = new CreateDeviceCommand(dto);
        var deviceId = await _mediator.Send(command);

        return CreatedAtAction(nameof(GetById), new { id = deviceId }, deviceId);
    }

    // PUT: api/devices/{id}
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateDeviceDto dto)
    {
        var command = new UpdateDeviceCommand(id, dto);
        await _mediator.Send(command);

        return NoContent();
    }

    // DELETE: api/devices/{id}
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var command = new DeleteDeviceCommand(id);
        await _mediator.Send(command);

        return NoContent();
    }
}
