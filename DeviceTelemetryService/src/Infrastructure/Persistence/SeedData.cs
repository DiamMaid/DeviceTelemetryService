using Domain.Entities;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence;

public static class SeedData
{
    public static async Task InitializeAsync(ApplicationDbContext context)
    {
        await context.Database.EnsureCreatedAsync();

        if (!context.Devices.Any())
        {
            var devices = new List<Device>
            {
                new()
                {
                    Id = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                    Name = "Temperature Sensor #1",
                    Type = SensorType.Temperature,
                    Status = DeviceStatus.Online,
                    Location = "Building A, Floor 1",
                    CreatedAt = DateTime.UtcNow,
                    LastHeartbeat = DateTime.UtcNow
                },
                new()
                {
                    Id = Guid.Parse("22222222-2222-2222-2222-222222222222"),
                    Name = "Pressure Sensor #1",
                    Type = SensorType.Pressure,
                    Status = DeviceStatus.Online,
                    Location = "Building A, Floor 2",
                    CreatedAt = DateTime.UtcNow,
                    LastHeartbeat = DateTime.UtcNow
                },
                new()
                {
                    Id = Guid.Parse("33333333-3333-3333-3333-333333333333"),
                    Name = "Vibration Sensor #1",
                    Type = SensorType.Vibration,
                    Status = DeviceStatus.Offline,
                    Location = "Building B, Floor 1",
                    CreatedAt = DateTime.UtcNow,
                    LastHeartbeat = null
                }
            };

            context.Devices.AddRange(devices);
            await context.SaveChangesAsync();
        }
    }
}
