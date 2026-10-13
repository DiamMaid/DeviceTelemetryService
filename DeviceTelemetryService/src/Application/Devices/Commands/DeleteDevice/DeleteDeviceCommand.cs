namespace Application.Devices.Commands.DeleteDevice;

using MediatR;

/// <summary>
/// Команда для удаления устройства.
/// </summary>
public record DeleteDeviceCommand(Guid Id) : IRequest<Unit>;
