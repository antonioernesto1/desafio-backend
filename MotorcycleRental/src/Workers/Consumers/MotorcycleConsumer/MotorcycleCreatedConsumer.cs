using MotorcycleRental.Application.Interfaces;
using MotorcycleRental.Domain.Events;
using MotorcycleRental.Domain.Interfaces.Repositories;
using System.Text.Json;

namespace MotorcycleRental.MotorcycleConsumer
{
    public class MotorcycleCreatedConsumerWorker : BackgroundService
    {
        private readonly IMessageBrokerService _messageBrokerService;
        private readonly IServiceProvider _serviceProvider;
        private readonly INoSqlRepository _noSqlRepository;
        public MotorcycleCreatedConsumerWorker(
            IMessageBrokerService messageBrokerService,
            IServiceProvider serviceProvider,
            INoSqlRepository noSqlRepository)
        {
            _messageBrokerService = messageBrokerService;
            _serviceProvider = serviceProvider;
            _noSqlRepository = noSqlRepository;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await _messageBrokerService.StartConsuming(
                queueName: "motorcycle.created.notifications",
                routingKey: "motorcycle.created",
                messageHandler: ProcessMessageAsync,
                cancellationToken: stoppingToken
            );
        }

        private async Task ProcessMessageAsync(string message)
        {
            try
            {
                using var scope = _serviceProvider.CreateScope();

                var payload = JsonSerializer.Deserialize<MotorcycleCreatedEvent>(message);
                var obj = new { id = payload.MotorcycleId, year =  payload.Year };
                
                await _noSqlRepository.SaveAsync("motorcycles_2024", obj);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
