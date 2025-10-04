using Microsoft.Extensions.Configuration;
using MotorcycleRental.Application.Interfaces;
using RabbitMQ.Client;
using RabbitMQ.Client.Core.DependencyInjection.Configuration;
using RabbitMQ.Client.Core.DependencyInjection.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MotorcycleRental.Infrastructure.Messaging
{
    public class RabbitMqService : IMessageBrokerService, IDisposable
    {
        private readonly IConnection _connection;
        private const string EXCHANGE_NAME = "motorcycle-rental-exchange";
        public RabbitMqService(IConfiguration configuration)
        {
            var factory = new ConnectionFactory
            {
                HostName = configuration["RabbitMq:Host"],
                UserName = configuration["RabbitMq:User"],
                Password = configuration["RabbitMq:Password"]
            };

            _connection = factory.CreateConnectionAsync().GetAwaiter().GetResult();

            using var channel = _connection.CreateChannelAsync().GetAwaiter().GetResult();

            channel.ExchangeDeclareAsync(EXCHANGE_NAME, ExchangeType.Topic, durable: true)
                .GetAwaiter().GetResult();
        }
        public async Task Publish(string topic, object message)
        {
            using var channel = await _connection.CreateChannelAsync();

            var properties = new BasicProperties
            {
                Persistent = true,
                ContentType = "application/json",
                Timestamp = new AmqpTimestamp(DateTimeOffset.UtcNow.ToUnixTimeSeconds())
            };

            var jsonMessage = JsonSerializer.Serialize(message);
            var body = Encoding.UTF8.GetBytes(jsonMessage);

            await channel.BasicPublishAsync(
               exchange: EXCHANGE_NAME,
               routingKey: topic,
               mandatory: false,
               basicProperties: properties,
               body: body
           );
        }

        public void Dispose()
        {
            _connection?.Dispose();
        }
    }
}
