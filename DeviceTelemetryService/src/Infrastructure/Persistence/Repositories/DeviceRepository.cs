using Domain.Interfaces;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Infrastructure.Persistence;

namespace Infrastructure.Persistence.Repositories;

/// <summary>
/// Реализация репозитория устройств через EF Core.
/// </summary>
public class DeviceRepository : IDeviceReader, IDeviceWriter
{
    private readonly ApplicationDbContext _context;

    public DeviceRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Получает устройство по уникальному идентификатору.
    /// </summary>
    public async Task<Device?> GetByIdAsync(Guid id)
    {
        return await _context.Devices.FindAsync(id);
    }

    /// <summary>
    /// Получает список всех зарегистрированных устройств.
    /// </summary>
    public async Task<List<Device>> GetAllAsync()
    {
        return await _context.Devices.ToListAsync();
    }

    /// <summary>
    /// Ищет устройство по имени.
    /// </summary>
    public async Task<Device?> GetByNameAsync(string name)
    {
        return await _context.Devices
            .FirstOrDefaultAsync(d => d.Name == name);
    }

    /// <summary>
    /// Регистрирует новое устройство в системе.
    /// </summary>
    public async Task<Guid> CreateAsync(Device device)
    {
        _context.Devices.Add(device);
        await _context.SaveChangesAsync();
        return device.Id;
    }

    /// <summary>
    /// Обновляет информацию о существующем устройстве.
    /// </summary>
    public async Task UpdateAsync(Device device)
    {
        _context.Devices.Update(device);
        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// Удаляет устройство из системы.
    /// </summary>
    public async Task DeleteAsync(Guid id)
    {
        var device = await _context.Devices.FindAsync(id);
        if (device != null)
        {
            _context.Devices.Remove(device);
            await _context.SaveChangesAsync();
        }
    }
}
