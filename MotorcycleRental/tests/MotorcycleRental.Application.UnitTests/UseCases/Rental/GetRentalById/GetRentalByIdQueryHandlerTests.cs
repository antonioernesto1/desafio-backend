
using FluentAssertions;
using MotorcycleRental.Application.UseCases.Rental.GetRentalById;
using MotorcycleRental.Domain.Exceptions;
using MotorcycleRental.Domain.Interfaces.Repositories;
using NSubstitute;
using System.Threading.Tasks;
using Xunit;

namespace MotorcycleRental.Application.UnitTests.UseCases.Rental.GetRentalById
{
    public class GetRentalByIdQueryHandlerTests
    {
        private readonly IRentalRepository _rentalRepository;
        private readonly GetRentalByIdQueryHandler _sut;

        public GetRentalByIdQueryHandlerTests()
        {
            _rentalRepository = Substitute.For<IRentalRepository>();
            _sut = new GetRentalByIdQueryHandler(_rentalRepository);
        }

        [Fact]
        public async Task Handle_Should_ReturnRentalDto_WhenRentalExists()
        {
            // Arrange
            var rentalId = System.Guid.NewGuid();
            var query = new GetRentalByIdQuery { Id = rentalId.ToString() };
            var rental = new Domain.Aggregates.Rentals.Rental("driver-id", "moto-id", System.DateTime.UtcNow, System.DateTime.UtcNow.AddDays(7), System.DateTime.UtcNow.AddDays(7), 7);

            _rentalRepository.GetByIdAsync(query.Id).Returns(rental);

            // Act
            var result = await _sut.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Id.Should().Be(rental.Id.ToString());
            result.MotorcycleId.Should().Be(rental.MotorcycleId);
            result.DeliveryDriverId.Should().Be(rental.DeliveryDriverId);
        }

        [Fact]
        public async Task Handle_Should_ThrowNotFoundException_WhenRentalDoesNotExist()
        {
            // Arrange
            var query = new GetRentalByIdQuery { Id = System.Guid.NewGuid().ToString() };

            _rentalRepository.GetByIdAsync(query.Id).Returns((Domain.Aggregates.Rentals.Rental)null);

            // Act
            Func<Task> act = async () => await _sut.Handle(query, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<NotFoundException>().WithMessage("Locação não encontrada");
        }
    }
}
