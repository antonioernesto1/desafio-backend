using MotorcycleConsumer;
using MotorcycleRental.Application;
using MotorcycleRental.Infrastructure;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddMessageBroker(builder.Configuration);
builder.Services.AddHostedService<MotorcycleCreatedConsumerWorker>();

var host = builder.Build();
host.Run();
