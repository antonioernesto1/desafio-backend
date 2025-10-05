
using FluentAssertions;
using MotorcycleRental.Application.UseCases.Rental.ReturnRental;
using MotorcycleRental.Domain.Exceptions;
using MotorcycleRental.Domain.Interfaces;
using MotorcycleRental.Domain.Interfaces.Repositories;
using NSubstitute;
using System;
using System.Threading.Tasks;
using Xunit;

namespace MotorcycleRental.Application.UnitTests.UseCases.Rental.ReturnRental
{
    public class ReturnRentalCommandHandlerTests
    {
        private readonly IRentalRepository _rentalRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ReturnRentalCommandHandler _sut;

        public ReturnRentalCommandHandlerTests()
        {
            _rentalRepository = Substitute.For<IRentalRepository>();
            _unitOfWork = Substitute.For<IUnitOfWork>();
            _sut = new ReturnRentalCommandHandler(_rentalRepository, _unitOfWork);
        }

        [Fact]
        public async Task Handle_Should_UpdateReturnDate_WhenRentalExistsAndDateIsValid()
        {
            // Arrange
            var rentalId = Guid.NewGuid().ToString();
            var initialDate = DateTime.UtcNow.AddDays(-5);
            var returnDate = DateTime.UtcNow;
            var command = new ReturnRentalCommand { Id = rentalId, ReturnDate = returnDate };
            var rental = new Domain.Aggregates.Rentals.Rental("driver-id", "moto-id", initialDate, initialDate.AddDays(7), initialDate.AddDays(7), 7);

            _rentalRepository.GetByIdAsync(rentalId).Returns(rental);

            // Act
            await _sut.Handle(command, CancellationToken.None);

            // Assert
            rental.ReturnDate.Should().Be(returnDate);
            await _unitOfWork.Received(1).SaveChangesAsync();
        }

        [Fact]
        public async Task Handle_Should_ThrowNotFoundException_WhenRentalDoesNotExist()
        {
            // Arrange
            var command = new ReturnRentalCommand { Id = Guid.NewGuid().ToString(), ReturnDate = DateTime.UtcNow };
            _rentalRepository.GetByIdAsync(command.Id).Returns((Domain.Aggregates.Rentals.Rental)null);

            // Act
            Func<Task> act = async () => await _sut.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<NotFoundException>().WithMessage("Locação não encontrada");
        }

        [Fact]
        public async Task Handle_Should_ThrowDomainException_WhenReturnDateIsBeforeInitialDate()
        {
            // Arrange
            var rentalId = Guid.NewGuid().ToString();
            var initialDate = DateTime.UtcNow.AddDays(-5);
            var invalidReturnDate = initialDate.AddDays(-1); // Date before rental started
            var command = new ReturnRentalCommand { Id = rentalId, ReturnDate = invalidReturnDate };
            var rental = new Domain.Aggregates.Rentals.Rental("driver-id", "moto-id", initialDate, initialDate.AddDays(7), initialDate.AddDays(7), 7);

            _rentalRepository.GetByIdAsync(rentalId).Returns(rental);

            // Act
            Func<Task> act = async () => await _sut.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<DomainException>().WithMessage("Return date cannot be before the initial date");
        }
    }
}
