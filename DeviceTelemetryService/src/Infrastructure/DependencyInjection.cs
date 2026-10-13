using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Domain.Interfaces;
using Infrastructure.Persistence;
using Infrastructure.Persistence.Repositories;

namespace Infrastructure;

/// <summary>
/// Расширения для регистрации сервисов Infrastructure слоя.
/// </summary>
public static class DependencyInjection
{
    /// <summary>
    /// Регистрирует все сервисы Infrastructure слоя в DI контейнере.
    /// </summary>
    /// <param name="services">Коллекция сервисов для регистрации.</param>
    /// <param name="connectionString">Строка подключения к базе данных TimescaleDB.</param>
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services, 
        string connectionString)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseNpgsql(connectionString));

        services.AddScoped<IDeviceReader, DeviceRepository>();
        services.AddScoped<IDeviceWriter, DeviceRepository>();
        services.AddScoped<ITelemetryReader, TelemetryRepository>();
        services.AddScoped<ITelemetryWriter, TelemetryRepository>();

        return services;
    }
}
