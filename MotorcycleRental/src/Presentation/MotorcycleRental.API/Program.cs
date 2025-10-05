using MotorcycleRental.Infrastructure;
using MotorcycleRental.Application;
using Microsoft.EntityFrameworkCore;
using MotorcycleRental.Infrastructure.Persistence;
using MotorcycleRental.API.Middleware;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.AddFilter("LuckyPennySoftware.MediatR.License", LogLevel.None);

builder.Services.AddControllers();

builder.Services.AddDatabase(builder.Configuration);
builder.Services.AddMessageBroker(builder.Configuration);
builder.Services.AddStorageService(builder.Configuration);
builder.Services.AddCoreServices();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    try
    {
        Console.WriteLine("Applying migrations...");
        var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        dbContext.Database.Migrate();
        Console.WriteLine("Migrations applied.");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"An error occurred while applying migrations: {ex.Message}");
    }
}


app.UseExceptionHandler(opt => { });

app.UseAuthorization();

app.MapControllers();

app.Run();
