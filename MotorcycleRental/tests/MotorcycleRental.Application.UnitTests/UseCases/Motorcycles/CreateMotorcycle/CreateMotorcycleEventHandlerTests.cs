
using MediatR;
using MotorcycleRental.Application.Interfaces;
using MotorcycleRental.Application.UseCases.Motorcycles.CreateMotorcycle;
using MotorcycleRental.Domain.Events;
using NSubstitute;
using System.Threading.Tasks;
using Xunit;

namespace MotorcycleRental.Application.UnitTests.UseCases.Motorcycles.CreateMotorcycle
{
    public class CreateMotorcycleEventHandlerTests
    {
        private readonly IMessageBrokerService _messageBrokerService;
        private readonly CreateMotorcycleEventHandler _sut;

        public CreateMotorcycleEventHandlerTests()
        {
            _messageBrokerService = Substitute.For<IMessageBrokerService>();
            _sut = new CreateMotorcycleEventHandler(_messageBrokerService);
        }

        [Fact]
        public async Task Handle_Should_PublishMessage_WhenYearIs2024()
        {
            // Arrange
            var notification = new MotorcycleCreatedEvent("test-id", 2024);

            // Act
            await _sut.Handle(notification, CancellationToken.None);

            // Assert
            await _messageBrokerService.Received(1).Publish(Arg.Is("motorcycle.created"), Arg.Any<object>());
        }

        [Fact]
        public async Task Handle_Should_NotPublishMessage_WhenYearIsNot2024()
        {
            // Arrange
            var notification = new MotorcycleCreatedEvent("test-id", 2023);

            // Act
            await _sut.Handle(notification, CancellationToken.None);

            // Assert
            await _messageBrokerService.DidNotReceive().Publish(Arg.Any<string>(), Arg.Any<object>());
        }
    }
}
