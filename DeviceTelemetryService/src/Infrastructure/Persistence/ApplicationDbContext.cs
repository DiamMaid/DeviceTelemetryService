using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence;

/// <summary>
/// EF Core контекст для работы с базой данных TimescaleDB.
/// </summary>
public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    /// <summary>
    /// Набор устройств в базе данных.
    /// </summary>
    public DbSet<Device> Devices { get; set; }

    /// <summary>
    /// Набор данных телеметрии в базе данных.
    /// </summary>
    public DbSet<TelemetryData> TelemetryData { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }
}