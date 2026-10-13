namespace Application.Devices.Queries.GetAllDevices;

using MediatR;
using Domain.Interfaces;
using Application.Devices.DTOs;

/// <summary>
/// Обработчик запроса получения всех устройств.
/// </summary>
public class GetAllDevicesQueryHandler : IRequestHandler<GetAllDevicesQuery, List<DeviceDto>>
{
    private readonly IDeviceReader _deviceReader;

    public GetAllDevicesQueryHandler(IDeviceReader deviceReader)
    {
        _deviceReader = deviceReader;
    }

    public async Task<List<DeviceDto>> Handle(GetAllDevicesQuery request, CancellationToken cancellationToken)
    {
        // Получаем все устройства из репозитория
        var devices = await _deviceReader.GetAllAsync();

        // Преобразуем сущности в DTO
        return devices.Select(device => new DeviceDto
        {
            Id = device.Id,
            Name = device.Name,
            Type = device.Type.ToString(),
            Status = device.Status.ToString(),
            Location = device.Location,
            CreatedAt = device.CreatedAt,
            LastHeartbeat = device.LastHeartbeat
        }).ToList();
    }
}
