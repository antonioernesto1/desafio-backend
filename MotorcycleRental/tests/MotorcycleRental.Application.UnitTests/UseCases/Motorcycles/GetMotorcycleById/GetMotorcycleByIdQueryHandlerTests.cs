
using FluentAssertions;
using MotorcycleRental.Application.DTOs.Motorcycles;
using MotorcycleRental.Application.Mappers.Motorcycles;
using MotorcycleRental.Application.UseCases.Motorcycles.GetMotorcycleById;
using MotorcycleRental.Domain.Aggregates.Motorcycles;
using MotorcycleRental.Domain.Exceptions;
using MotorcycleRental.Domain.Interfaces.Repositories;
using NSubstitute;
using System.Threading.Tasks;
using Xunit;

namespace MotorcycleRental.Application.UnitTests.UseCases.Motorcycles.GetMotorcycleById
{
    public class GetMotorcycleByIdQueryHandlerTests
    {
        private readonly IMotorcycleRepository _motorcycleRepository;
        private readonly GetMotorcycleByIdQueryHandler _sut;

        public GetMotorcycleByIdQueryHandlerTests()
        {
            _motorcycleRepository = Substitute.For<IMotorcycleRepository>();
            _sut = new GetMotorcycleByIdQueryHandler(_motorcycleRepository);
        }

        [Fact]
        public async Task Handle_Should_ReturnMotorcycleDto_WhenMotorcycleExists()
        {
            // Arrange
            var query = new GetMotorcycleByIdQuery { Id = "test-id" };
            var motorcycle = new Motorcycle(query.Id, 2023, "Model S", "QWE-1234");

            _motorcycleRepository.GetByIdAsync(query.Id).Returns(motorcycle);

            // Act
            var result = await _sut.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Id.Should().Be(motorcycle.Id);
            result.Year.Should().Be(motorcycle.Year);
            result.Model.Should().Be(motorcycle.Model);
            result.LicensePlate.Should().Be(motorcycle.LicensePlate);
        }

        [Fact]
        public async Task Handle_Should_ThrowNotFoundException_WhenMotorcycleDoesNotExist()
        {
            // Arrange
            var query = new GetMotorcycleByIdQuery { Id = "non-existent-id" };

            _motorcycleRepository.GetByIdAsync(query.Id).Returns((Motorcycle)null);

            // Act
            Func<Task> act = async () => await _sut.Handle(query, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<NotFoundException>().WithMessage("Moto não encontrada");
        }
    }
}
