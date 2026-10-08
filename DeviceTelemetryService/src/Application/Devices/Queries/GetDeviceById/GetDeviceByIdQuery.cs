namespace Application.Devices.Queries.GetDeviceById;

using MediatR;
using Domain.Interfaces;
using Application.Devices.DTOs;

/// <summary>
/// Запрос для получения устройства по уникальному идентификатору.
/// </summary>
public record GetDeviceByIdQuery(Guid Id) : IRequest<DeviceDto?>;

/// <summary>
/// Обработчик запроса получения устройства по ID.
/// </summary>
public class GetDeviceByIdQueryHandler : IRequestHandler<GetDeviceByIdQuery, DeviceDto?>
{
    private readonly IDeviceReader _deviceReader;

    public GetDeviceByIdQueryHandler(IDeviceReader deviceReader)
    {
        _deviceReader = deviceReader;
    }

    public async Task<DeviceDto?> Handle(GetDeviceByIdQuery request, CancellationToken cancellationToken)
    {
        // Получаем устройство из репозитория
        var device = await _deviceReader.GetByIdAsync(request.Id);

        // Если устройство не найдено — возвращаем null
        if (device == null)
        {
            return null;
        }

        // Преобразуем сущность в DTO
        return new DeviceDto
        {
            Id = device.Id,
            Name = device.Name,
            Type = device.Type.ToString(),
            Status = device.Status.ToString(),
            Location = device.Location,
            CreatedAt = device.CreatedAt,
            LastHeartbeat = device.LastHeartbeat
        };
    }
}