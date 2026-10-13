namespace Application.Devices.Commands.DeleteDevice;

using MediatR;
using Domain.Interfaces;

/// <summary>
/// Обработчик команды удаления устройства.
/// </summary>
public class DeleteDeviceCommandHandler : IRequestHandler<DeleteDeviceCommand, Unit>
{
    private readonly IDeviceWriter _deviceWriter;

    public DeleteDeviceCommandHandler(IDeviceWriter deviceWriter)
    {
        _deviceWriter = deviceWriter;
    }

    public async Task<Unit> Handle(DeleteDeviceCommand request, CancellationToken cancellationToken)
    {
        // Удаляем устройство по ID
        await _deviceWriter.DeleteAsync(request.Id);

        return Unit.Value;
    }
}
