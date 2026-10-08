using Microsoft.Extensions.DependencyInjection;
using FluentValidation;
using MediatR;
using System.Reflection;
using Application.Common.Behaviors;

namespace Application;

/// <summary>
/// Расширения для регистрации сервисов Application слоя.
/// </summary>
public static class DependencyInjection
{
    /// <summary>
    /// Регистрирует все сервисы Application слоя в DI контейнере.
    /// </summary>
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        // Регистрируем MediatR
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
        });

        // Регистрируем валидаторы FluentValidation
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

        // Регистрируем pipeline behaviors
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

        return services;
    }
}
