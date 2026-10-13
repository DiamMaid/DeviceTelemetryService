namespace Application.Devices.Commands.CreateDevice;

using MediatR;
using Application.Devices.DTOs;

/// <summary>
/// Команда для создания нового IoT-устройства.
/// </summary>
public record CreateDeviceCommand(CreateDeviceDto DeviceDto) : IRequest<Guid>;
