namespace Application.Devices.Commands.UpdateDevice;

using MediatR;

/// <summary>
/// DTO для обновления устройства.
/// </summary>
public record UpdateDeviceDto(
    string Name,
    string? Location
);

/// <summary>
/// Команда для обновления устройства.
/// </summary>
public record UpdateDeviceCommand(Guid Id, UpdateDeviceDto DeviceDto) : IRequest<Unit>;
