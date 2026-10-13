using Application;
using Infrastructure;
public partial class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Регистрируем Application слой (MediatR, валидаторы)
        builder.Services.AddApplication();

        // Регистрируем Infrastructure слой (DbContext, репозитории)
        builder.Services.AddInfrastructure(
            builder.Configuration.GetConnectionString("DefaultConnection"));

        // Добавляем контроллеры
        builder.Services.AddControllers();

        var app = builder.Build();
    }
}