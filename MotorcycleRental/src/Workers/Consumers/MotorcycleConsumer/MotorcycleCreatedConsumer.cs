using MotorcycleRental.Application.Interfaces;
using MotorcycleRental.Domain.Events;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MotorcycleConsumer
{
    public class MotorcycleCreatedConsumerWorker : BackgroundService
    {
        private readonly IMessageBrokerService _messageBrokerService;
        private readonly IServiceProvider _serviceProvider;

        public MotorcycleCreatedConsumerWorker(
            IMessageBrokerService messageBrokerService,
            IServiceProvider serviceProvider)
        {
            _messageBrokerService = messageBrokerService;
            _serviceProvider = serviceProvider;
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
                var payload = JsonSerializer.Deserialize<MotorcycleCreatedEvent>(message);
               
                //TODO: Save to MongoDB events schema

                using var scope = _serviceProvider.CreateScope();
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
