
using FluentAssertions;
using MotorcycleRental.Application.UseCases.Motorcycles.GetMotorcycles;
using MotorcycleRental.Domain.Aggregates.Motorcycles;
using MotorcycleRental.Domain.Interfaces;
using MotorcycleRental.Domain.Interfaces.Repositories;
using NSubstitute;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace MotorcycleRental.Application.UnitTests.UseCases.Motorcycles.GetMotorcycles
{
    public class GetMotorcyclesQueryHandlerTests
    {
        private readonly IMotorcycleRepository _motorcycleRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly GetMotorcyclesQueryHandler _sut;

        public GetMotorcyclesQueryHandlerTests()
        {
            _motorcycleRepository = Substitute.For<IMotorcycleRepository>();
            _unitOfWork = Substitute.For<IUnitOfWork>();
            _sut = new GetMotorcyclesQueryHandler(_motorcycleRepository, _unitOfWork);
        }

        [Fact]
        public async Task Handle_Should_ReturnAllMotorcycles_WhenLicensePlateIsNull()
        {
            // Arrange
            var query = new GetMotorcyclesQuery { LicensePlate = null };
            var motorcycles = new List<Motorcycle>
            {
                new Motorcycle("id-1", 2022, "Model A", "AAA-1111"),
                new Motorcycle("id-2", 2023, "Model B", "BBB-2222")
            };

            _motorcycleRepository.GetMotorcyclesAsync(null).Returns(motorcycles);

            // Act
            var result = await _sut.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(2);
            result.First().LicensePlate.Should().Be("AAA-1111");
        }

        [Fact]
        public async Task Handle_Should_ReturnFilteredMotorcycles_WhenLicensePlateIsProvided()
        {
            // Arrange
            var licensePlate = "BBB-2222";
            var query = new GetMotorcyclesQuery { LicensePlate = licensePlate };
            var motorcycles = new List<Motorcycle>
            {
                new Motorcycle("id-2", 2023, "Model B", licensePlate)
            };

            _motorcycleRepository.GetMotorcyclesAsync(licensePlate).Returns(motorcycles);

            // Act
            var result = await _sut.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(1);
            result.Single().LicensePlate.Should().Be(licensePlate);
        }

        [Fact]
        public async Task Handle_Should_ReturnEmptyList_WhenNoMotorcyclesFound()
        {
            // Arrange
            var query = new GetMotorcyclesQuery { LicensePlate = "ZZZ-9999" };
            var motorcycles = new List<Motorcycle>();

            _motorcycleRepository.GetMotorcyclesAsync(query.LicensePlate).Returns(motorcycles);

            // Act
            var result = await _sut.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEmpty();
        }
    }
}
