
using MediatR;
using MotorcycleRental.Application.UseCases.Motorcycles.CreateMotorcycle;
using MotorcycleRental.Domain.Aggregates.Motorcycles;
using MotorcycleRental.Domain.Events;
using MotorcycleRental.Domain.Exceptions;
using MotorcycleRental.Domain.Interfaces;
using MotorcycleRental.Domain.Interfaces.Repositories;
using NSubstitute;
using FluentAssertions;

namespace MotorcycleRental.Application.UnitTests.UseCases.Motorcycles.CreateMotorcycle
{
    public class CreateMotorcycleCommandHandlerTests
    {
        private readonly IMotorcycleRepository _motorcycleRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMediator _mediator;
        private readonly CreateMotorcycleCommandHandler _sut;

        public CreateMotorcycleCommandHandlerTests()
        {
            _motorcycleRepository = Substitute.For<IMotorcycleRepository>();
            _unitOfWork = Substitute.For<IUnitOfWork>();
            _mediator = Substitute.For<IMediator>();
            _sut = new CreateMotorcycleCommandHandler(_motorcycleRepository, _unitOfWork, _mediator);
        }

        [Fact]
        public async Task Handle_Should_CreateMotorcycle_WhenLicensePlateIsUnique()
        {
            // Arrange
            var command = new CreateMotorcycleCommand
            {
                Id = "test-id",
                Year = 2023,
                Model = "Test Model",
                LicensePlate = "TEST-123"
            };

            _motorcycleRepository.PlateExists(command.LicensePlate).Returns(false);

            // Act
            var motorcycleId = await _sut.Handle(command, CancellationToken.None);

            // Assert
            motorcycleId.Should().Be(command.Id);
            await _motorcycleRepository.Received(1).AddAsync(Arg.Is<Motorcycle>(m => m.Id == command.Id));
            await _unitOfWork.Received(1).SaveChangesAsync();
            await _mediator.Received(1).Publish(Arg.Any<MotorcycleCreatedEvent>(), Arg.Any<CancellationToken>());
        }

        [Fact]
        public async Task Handle_Should_ThrowDomainException_WhenLicensePlateExists()
        {
            // Arrange
            var command = new CreateMotorcycleCommand
            {
                Id = "test-id",
                Year = 2023,
                Model = "Test Model",
                LicensePlate = "EXIST-123"
            };

            _motorcycleRepository.PlateExists(command.LicensePlate).Returns(true);

            // Act
            Func<Task> act = async () => await _sut.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<DomainException>().WithMessage("The provided license plate is used by another motorcycle");
            await _motorcycleRepository.DidNotReceive().AddAsync(Arg.Any<Motorcycle>());
            await _unitOfWork.DidNotReceive().SaveChangesAsync();
            await _mediator.DidNotReceive().Publish(Arg.Any<MotorcycleCreatedEvent>());
        }
    }
}
