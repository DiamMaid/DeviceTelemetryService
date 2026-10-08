namespace Application.Devices.DTOs;

using Domain.Enums;

/// <summary>
/// DTO для создания нового IoT-устройства.
/// </summary>
public record CreateDeviceDto(
    string Name,
    SensorType Type,
    string? Location
);
