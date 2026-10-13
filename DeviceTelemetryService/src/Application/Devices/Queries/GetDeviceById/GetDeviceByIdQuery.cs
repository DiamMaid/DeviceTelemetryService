namespace Application.Devices.Queries.GetDeviceById;

using MediatR;
using Application.Devices.DTOs;

/// <summary>
/// Запрос для получения устройства по уникальному идентификатору.
/// </summary>
public record GetDeviceByIdQuery(Guid Id) : IRequest<DeviceDto?>;
