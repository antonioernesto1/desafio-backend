using MediatR;
using MotorcycleRental.Application.Interfaces;
using MotorcycleRental.Domain.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotorcycleRental.Application.UseCases.Motorcycles.CreateMotorcycle
{
    public class CreateMotorcycleEventHandler : INotificationHandler<MotorcycleCreatedEvent>
    {
        private const int YEAR_TO_PUBLISH = 2024;
        private readonly IMessageBrokerService _messageBrokerService;

        public CreateMotorcycleEventHandler(IMessageBrokerService messageBrokerService)
        {
            _messageBrokerService = messageBrokerService;
        }

        public async Task Handle(MotorcycleCreatedEvent notification, CancellationToken cancellationToken)
        {
            if (notification.Year == YEAR_TO_PUBLISH)
            {
                var messagePayload = new { notification.MotorcycleId, notification.Year };
                await _messageBrokerService.Publish("motorcycle.created", messagePayload);
            }
        }
    }
}
