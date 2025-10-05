
using FluentAssertions;
using MotorcycleRental.Application.UseCases.Motorcycles.DeleteMotorcycle;
using MotorcycleRental.Domain.Aggregates.Motorcycles;
using MotorcycleRental.Domain.Exceptions;
using MotorcycleRental.Domain.Interfaces;
using MotorcycleRental.Domain.Interfaces.Repositories;
using NSubstitute;
using System.Threading.Tasks;
using Xunit;

namespace MotorcycleRental.Application.UnitTests.UseCases.Motorcycles.DeleteMotorcycle
{
    public class DeleteMotorcycleCommandHandlerTests
    {
        private readonly IMotorcycleRepository _motorcycleRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly DeleteMotorcycleCommandHandler _sut;

        public DeleteMotorcycleCommandHandlerTests()
        {
            _motorcycleRepository = Substitute.For<IMotorcycleRepository>();
            _unitOfWork = Substitute.For<IUnitOfWork>();
            _sut = new DeleteMotorcycleCommandHandler(_motorcycleRepository, _unitOfWork);
        }

        [Fact]
        public async Task Handle_Should_DeleteMotorcycle_WhenMotorcycleExistsAndHasNoActiveRentals()
        {
            // Arrange
            var command = new DeleteMotorcycleCommand { Id = "test-id" };
            var motorcycle = new Motorcycle(command.Id, 2022, "Model X", "ABC-1234");

            _motorcycleRepository.GetByIdAsync(command.Id).Returns(motorcycle);
            _motorcycleRepository.HasActiveRentals(command.Id).Returns(false);

            // Act
            await _sut.Handle(command, CancellationToken.None);

            // Assert
            _motorcycleRepository.Received(1).Delete(motorcycle);
            await _unitOfWork.Received(1).SaveChangesAsync();
        }

        [Fact]
        public async Task Handle_Should_ThrowNotFoundException_WhenMotorcycleDoesNotExist()
        {
            // Arrange
            var command = new DeleteMotorcycleCommand { Id = "non-existent-id" };

            _motorcycleRepository.GetByIdAsync(command.Id).Returns((Motorcycle)null);

            // Act
            Func<Task> act = async () => await _sut.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<NotFoundException>().WithMessage("Moto não encontrada");
            _motorcycleRepository.DidNotReceive().Delete(Arg.Any<Motorcycle>());
            await _unitOfWork.DidNotReceive().SaveChangesAsync();
        }

        [Fact]
        public async Task Handle_Should_ThrowDomainException_WhenMotorcycleHasActiveRentals()
        {
            // Arrange
            var command = new DeleteMotorcycleCommand { Id = "test-id" };
            var motorcycle = new Motorcycle(command.Id, 2022, "Model Y", "XYZ-7890");

            _motorcycleRepository.GetByIdAsync(command.Id).Returns(motorcycle);
            _motorcycleRepository.HasActiveRentals(command.Id).Returns(true);

            // Act
            Func<Task> act = async () => await _sut.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<DomainException>().WithMessage("Cannot delete a motorcycle with active rentals");
            _motorcycleRepository.DidNotReceive().Delete(Arg.Any<Motorcycle>());
            await _unitOfWork.DidNotReceive().SaveChangesAsync();
        }
    }
}
