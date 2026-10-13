using Application;
using Infrastructure;

public partial class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Регистрируем Application слой (MediatR, валидаторы)
        builder.Services.AddApplication();

        // Регистрируем Infrastructure слой (DbContext, репозитории)
        builder.Services.AddInfrastructure(
            builder.Configuration.GetConnectionString("DefaultConnection"));

        // Добавляем контроллеры
        builder.Services.AddControllers();

        // Настраиваем Swagger
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new() 
            { 
                Title = "Device Telemetry Service API", 
                Version = "v1",
                Description = "API для управления IoT устройствами и сбора телеметрии"
            });
        });

        var app = builder.Build();

        // Middleware
        if (app.Environment.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            
            // Включаем Swagger только в Development режиме
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Device Telemetry Service API v1");
            });
        }

        app.UseHttpsRedirection();
        app.UseAuthorization();

        // Map controllers
        app.MapControllers();

        await app.RunAsync();
    }
}