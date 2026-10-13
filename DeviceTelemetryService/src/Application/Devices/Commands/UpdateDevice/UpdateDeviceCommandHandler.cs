namespace Application.Devices.Commands.UpdateDevice;

using MediatR;
using Domain.Interfaces;

/// <summary>
/// Обработчик команды обновления устройства.
/// </summary>
public class UpdateDeviceCommandHandler : IRequestHandler<UpdateDeviceCommand, Unit>
{
    private readonly IDeviceReader _deviceReader;
    private readonly IDeviceWriter _deviceWriter;

    public UpdateDeviceCommandHandler(IDeviceReader deviceReader, IDeviceWriter deviceWriter)
    {
        _deviceReader = deviceReader;
        _deviceWriter = deviceWriter;
    }

    public async Task<Unit> Handle(UpdateDeviceCommand request, CancellationToken cancellationToken)
    {
        // Получаем существующее устройство
        var device = await _deviceReader.GetByIdAsync(request.Id);

        if (device == null)
        {
            throw new KeyNotFoundException($"Device with ID {request.Id} not found");
        }

        // Обновляем поля
        device.Name = request.DeviceDto.Name;
        device.Location = request.DeviceDto.Location;

        // Сохраняем изменения
        await _deviceWriter.UpdateAsync(device);

        return Unit.Value;
    }
}
