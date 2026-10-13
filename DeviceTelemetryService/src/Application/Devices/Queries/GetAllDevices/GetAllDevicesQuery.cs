namespace Application.Devices.Queries.GetAllDevices;

using MediatR;
using Application.Devices.DTOs;

/// <summary>
/// Запрос для получения списка всех устройств.
/// </summary>
public record GetAllDevicesQuery : IRequest<List<DeviceDto>>;
