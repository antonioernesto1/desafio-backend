using Microsoft.Extensions.Configuration;
using MotorcycleRental.Application.Interfaces;
using RabbitMQ.Client;
using RabbitMQ.Client.Core.DependencyInjection.Configuration;
using RabbitMQ.Client.Core.DependencyInjection.Services;
using RabbitMQ.Client.Events;
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

            channel.ExchangeDeclareAsync(EXCHANGE_NAME, ExchangeType.Topic, durable: true, autoDelete: false)
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

        public async Task StartConsuming(string queueName, string routingKey, Func<string, Task> messageHandler,
            CancellationToken cancellationToken)
        {
            var channel = await _connection.CreateChannelAsync();

            await channel.ExchangeDeclareAsync(
                EXCHANGE_NAME,
                ExchangeType.Topic,
                durable: true
            );

            await channel.QueueDeclareAsync(
                queue: queueName,
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null
            );

            await channel.QueueBindAsync(
                queue: queueName,
                exchange: EXCHANGE_NAME,
                routingKey: routingKey
            );

            await channel.BasicQosAsync(
                prefetchSize: 0,
                prefetchCount: 1,
                global: false
            );

            var consumer = new AsyncEventingBasicConsumer(channel);

            consumer.ReceivedAsync += async (sender, eventArgs) =>
            {
                try
                {
                    var body = eventArgs.Body.ToArray();
                    var message = Encoding.UTF8.GetString(body);

                    await messageHandler(message);

                    await channel.BasicAckAsync(
                        deliveryTag: eventArgs.DeliveryTag,
                        multiple: false
                    );
                }
                catch (Exception ex)
                {
                    await channel.BasicNackAsync(
                        deliveryTag: eventArgs.DeliveryTag,
                        multiple: false,
                        requeue: true
                    );
                }
            };

            await channel.BasicConsumeAsync(
                queue: queueName,
                autoAck: false,
                consumer: consumer
            );

            await Task.Delay(Timeout.Infinite, cancellationToken);
        }

        public void Dispose()
        {
            _connection?.Dispose();
        }
    }
}
