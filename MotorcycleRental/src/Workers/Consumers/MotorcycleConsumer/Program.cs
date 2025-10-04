using MotorcycleRental.Application;
using MotorcycleRental.Infrastructure;
using MotorcycleRental.MotorcycleConsumer;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddNoSql(builder.Configuration);
builder.Services.AddMessageBroker(builder.Configuration);
builder.Services.AddHostedService<MotorcycleCreatedConsumerWorker>();

var host = builder.Build();
host.Run();
