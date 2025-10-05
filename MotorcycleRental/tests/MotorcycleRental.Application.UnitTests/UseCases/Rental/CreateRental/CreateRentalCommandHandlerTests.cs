
using FluentAssertions;
using MotorcycleRental.Application.UseCases.Rental.CreateRental;
using MotorcycleRental.Domain.Aggregates.DeliveryDrivers;
using MotorcycleRental.Domain.Aggregates.DeliveryDrivers.ValueObjects;
using MotorcycleRental.Domain.Aggregates.Motorcycles;
using MotorcycleRental.Domain.Exceptions;
using MotorcycleRental.Domain.Interfaces;
using MotorcycleRental.Domain.Interfaces.Repositories;
using NSubstitute;
using System.Threading.Tasks;
using Xunit;

namespace MotorcycleRental.Application.UnitTests.UseCases.Rental.CreateRental
{
    public class CreateRentalCommandHandlerTests
    {
        private readonly IRentalRepository _rentalRepository;
        private readonly IDeliveryDriverRepository _deliveryDriverRepository;
        private readonly IMotorcycleRepository _motorcycleRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly CreateRentalCommandHandler _sut;

        public CreateRentalCommandHandlerTests()
        {
            _rentalRepository = Substitute.For<IRentalRepository>();
            _deliveryDriverRepository = Substitute.For<IDeliveryDriverRepository>();
            _motorcycleRepository = Substitute.For<IMotorcycleRepository>();
            _unitOfWork = Substitute.For<IUnitOfWork>();
            _sut = new CreateRentalCommandHandler(_rentalRepository, _deliveryDriverRepository, _motorcycleRepository, _unitOfWork);
        }

        private DeliveryDriver CreateValidDriver(string cnhType = "A")
        {
            var cnh = new Cnh(cnhType, "123456789");
            return new DeliveryDriver("driver-id", "Test Driver", "12345678901234", new System.DateTime(1990, 1, 1), cnh);
        }

        private Motorcycle CreateValidMotorcycle()
        {
            return new Motorcycle("moto-id", 2022, "Model Test", "ABC-1234");
        }

        [Fact]
        public async Task Handle_Should_CreateRental_WhenCommandIsValid()
        {
            // Arrange
            var command = new CreateRentalCommand { DeliveryDriverId = "driver-id", MotorcycleId = "moto-id" };
            var driver = CreateValidDriver();
            var motorcycle = CreateValidMotorcycle();

            _deliveryDriverRepository.GetByIdAsync(command.DeliveryDriverId).Returns(driver);
            _motorcycleRepository.GetByIdAsync(command.MotorcycleId).Returns(motorcycle);
            _motorcycleRepository.HasActiveRentals(command.MotorcycleId).Returns(false);

            // Act
            var rentalId = await _sut.Handle(command, CancellationToken.None);

            // Assert
            rentalId.Should().NotBeNullOrEmpty();
            await _rentalRepository.Received(1).AddAsync(Arg.Is<Domain.Aggregates.Rentals.Rental>(r => r.DeliveryDriverId == command.DeliveryDriverId));
            await _unitOfWork.Received(1).SaveChangesAsync();
        }

        [Fact]
        public async Task Handle_Should_ThrowDomainException_WhenDriverNotFound()
        {
            // Arrange
            var command = new CreateRentalCommand { DeliveryDriverId = "non-existent-driver" };
            _deliveryDriverRepository.GetByIdAsync(command.DeliveryDriverId).Returns((DeliveryDriver)null);

            // Act
            Func<Task> act = async () => await _sut.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<DomainException>().WithMessage("Dados inválidos");
        }

        [Fact]
        public async Task Handle_Should_ThrowDomainException_WhenDriverCnhIsNotTypeA()
        {
            // Arrange
            var command = new CreateRentalCommand { DeliveryDriverId = "driver-id" };
            var driver = CreateValidDriver(cnhType: "B"); // Invalid CNH type

            _deliveryDriverRepository.GetByIdAsync(command.DeliveryDriverId).Returns(driver);

            // Act
            Func<Task> act = async () => await _sut.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<DomainException>().WithMessage("Delivery driver must have a type 'A' CNH to rent a motorcycle.");
        }

        [Fact]
        public async Task Handle_Should_ThrowDomainException_WhenMotorcycleNotFound()
        {
            // Arrange
            var command = new CreateRentalCommand { DeliveryDriverId = "driver-id", MotorcycleId = "non-existent-moto" };
            var driver = CreateValidDriver();

            _deliveryDriverRepository.GetByIdAsync(command.DeliveryDriverId).Returns(driver);
            _motorcycleRepository.GetByIdAsync(command.MotorcycleId).Returns((Motorcycle)null);

            // Act
            Func<Task> act = async () => await _sut.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<DomainException>().WithMessage("Dados inválidos");
        }

        [Fact]
        public async Task Handle_Should_ThrowDomainException_WhenMotorcycleIsAlreadyRented()
        {
            // Arrange
            var command = new CreateRentalCommand { DeliveryDriverId = "driver-id", MotorcycleId = "moto-id" };
            var driver = CreateValidDriver();
            var motorcycle = CreateValidMotorcycle();

            _deliveryDriverRepository.GetByIdAsync(command.DeliveryDriverId).Returns(driver);
            _motorcycleRepository.GetByIdAsync(command.MotorcycleId).Returns(motorcycle);
            _motorcycleRepository.HasActiveRentals(command.MotorcycleId).Returns(true); // Motorcycle is busy

            // Act
            Func<Task> act = async () => await _sut.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<DomainException>().WithMessage("Motorcycle is already rented.");
        }
    }
}
