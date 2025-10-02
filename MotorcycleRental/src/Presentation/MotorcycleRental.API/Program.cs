using MotorcycleRental.Infrastructure;
using MotorcycleRental.Application;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.AddFilter("LuckyPennySoftware.MediatR.License", LogLevel.None);

builder.Services.AddControllers();
builder.Services.AddOpenApi();

builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddCoreServices();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
