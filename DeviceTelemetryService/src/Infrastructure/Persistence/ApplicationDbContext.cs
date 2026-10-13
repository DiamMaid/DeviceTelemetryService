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

        modelBuilder.Entity<TelemetryData>(entity =>
        {
            entity.ToTable("telemetry_data");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.DeviceId).HasColumnName("device_id");
            entity.Property(e => e.Timestamp).HasColumnName("time");
            entity.Property(e => e.MetricName).HasColumnName("metric_name");
            entity.Property(e => e.MetricValue).HasColumnName("metric_value");
            entity.Property(e => e.Unit).HasColumnName("unit");
            entity.Property(e => e.Metadata).HasColumnName("tags");
            entity.Property(e => e.ReceivedAt).HasColumnName("created_at");
        });

        modelBuilder.Entity<Device>(entity =>
        {
            entity.ToTable("Devices");
        });
    }
}