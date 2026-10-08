namespace Application.Devices.Commands.CreateDevice;

using MediatR;
using Domain.Interfaces;
using Domain.Events;
using Application.Devices.DTOs;

/// <summary>
/// Команда для создания нового IoT-устройства.
/// </summary>
public record CreateDeviceCommand(CreateDeviceDto DeviceDto) : IRequest<Guid>;

/// <summary>
/// Обработчик команды создания устройства.
/// </summary>
public class CreateDeviceCommandHandler : IRequestHandler<CreateDeviceCommand, Guid>
{
    private readonly IDeviceWriter _deviceWriter;
    private readonly IPublisher _mediator;

    public CreateDeviceCommandHandler(IDeviceWriter deviceWriter, IPublisher mediator)
    {
        _deviceWriter = deviceWriter;
        _mediator = mediator;
    }

    public async Task<Guid> Handle(CreateDeviceCommand request, CancellationToken cancellationToken)
    {
        // Создаем сущность Device из DTO
        var device = new Domain.Entities.Device
        {
            Id = Guid.NewGuid(),
            Name = request.DeviceDto.Name,
            Type = request.DeviceDto.Type,
            Status = Domain.Enums.DeviceStatus.Online,
            Location = request.DeviceDto.Location,
            CreatedAt = DateTime.UtcNow,
            LastHeartbeat = null
        };

        // Сохраняем через репозиторий
        var deviceId = await _deviceWriter.CreateAsync(device);

        // Публикуем доменное событие
        var deviceRegisteredEvent = new DeviceRegisteredEvent(device);
        await _mediator.Publish(deviceRegisteredEvent, cancellationToken);

        return deviceId;
    }
}
