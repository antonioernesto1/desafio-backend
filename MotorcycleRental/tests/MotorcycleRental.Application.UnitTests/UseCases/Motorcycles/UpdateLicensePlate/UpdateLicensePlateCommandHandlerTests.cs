
using FluentAssertions;
using MotorcycleRental.Application.UseCases.Motorcycles.UpdateLicensePlate;
using MotorcycleRental.Domain.Aggregates.Motorcycles;
using MotorcycleRental.Domain.Exceptions;
using MotorcycleRental.Domain.Interfaces;
using MotorcycleRental.Domain.Interfaces.Repositories;
using NSubstitute;
using System.Threading.Tasks;
using Xunit;

namespace MotorcycleRental.Application.UnitTests.UseCases.Motorcycles.UpdateLicensePlate
{
    public class UpdateLicensePlateCommandHandlerTests
    {
        private readonly IMotorcycleRepository _motorcycleRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly UpdateLicensePlateCommandHandler _sut;

        public UpdateLicensePlateCommandHandlerTests()
        {
            _motorcycleRepository = Substitute.For<IMotorcycleRepository>();
            _unitOfWork = Substitute.For<IUnitOfWork>();
            _sut = new UpdateLicensePlateCommandHandler(_motorcycleRepository, _unitOfWork);
        }

        [Fact]
        public async Task Handle_Should_UpdateLicensePlate_WhenRequestIsValid()
        {
            // Arrange
            var command = new UpdateLicensePlateCommand { Id = "test-id", LicensePlate = "NEW-5678" };
            var motorcycle = new Motorcycle(command.Id, 2022, "Model Z", "OLD-1234");

            _motorcycleRepository.GetByIdAsync(command.Id).Returns(motorcycle);
            _motorcycleRepository.PlateExists(command.LicensePlate).Returns(false);

            // Act
            await _sut.Handle(command, CancellationToken.None);

            // Assert
            motorcycle.LicensePlate.Should().Be(command.LicensePlate);
            await _unitOfWork.Received(1).SaveChangesAsync();
        }

        [Fact]
        public async Task Handle_Should_ThrowNotFoundException_WhenMotorcycleDoesNotExist()
        {
            // Arrange
            var command = new UpdateLicensePlateCommand { Id = "non-existent-id", LicensePlate = "NEW-5678" };

            _motorcycleRepository.GetByIdAsync(command.Id).Returns((Motorcycle)null);

            // Act
            Func<Task> act = async () => await _sut.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<NotFoundException>().WithMessage("Moto não encontrada");
        }

        [Fact]
        public async Task Handle_Should_ThrowDomainException_WhenNewLicensePlateIsSameAsOldOne()
        {
            // Arrange
            var command = new UpdateLicensePlateCommand { Id = "test-id", LicensePlate = "OLD-1234" };
            var motorcycle = new Motorcycle(command.Id, 2022, "Model Z", "OLD-1234");

            _motorcycleRepository.GetByIdAsync(command.Id).Returns(motorcycle);

            // Act
            Func<Task> act = async () => await _sut.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<DomainException>().WithMessage("The new license plate cannot be the same as the current one.");
        }

        [Fact]
        public async Task Handle_Should_ThrowDomainException_WhenNewLicensePlateAlreadyExists()
        {
            // Arrange
            var command = new UpdateLicensePlateCommand { Id = "test-id", LicensePlate = "EXISTING-9999" };
            var motorcycle = new Motorcycle(command.Id, 2022, "Model Z", "OLD-1234");

            _motorcycleRepository.GetByIdAsync(command.Id).Returns(motorcycle);
            _motorcycleRepository.PlateExists(command.LicensePlate).Returns(true);

            // Act
            Func<Task> act = async () => await _sut.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<DomainException>().WithMessage($"The license plate '{command.LicensePlate}' is already assigned to another motorcycle.");
        }
    }
}
