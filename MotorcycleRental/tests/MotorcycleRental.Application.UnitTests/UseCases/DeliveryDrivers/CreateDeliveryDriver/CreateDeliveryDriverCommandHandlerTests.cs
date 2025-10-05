
using FluentAssertions;
using MotorcycleRental.Application.Interfaces;
using MotorcycleRental.Application.UseCases.DeliveryDrivers.CreateDeliveryDriver;
using MotorcycleRental.Domain.Aggregates.DeliveryDrivers;
using MotorcycleRental.Domain.Exceptions;
using MotorcycleRental.Domain.Interfaces;
using MotorcycleRental.Domain.Interfaces.Repositories;
using NSubstitute;
using System.Threading.Tasks;
using Xunit;

namespace MotorcycleRental.Application.UnitTests.UseCases.DeliveryDrivers.CreateDeliveryDriver
{
    public class CreateDeliveryDriverCommandHandlerTests
    {
        private readonly IDeliveryDriverRepository _deliveryDriverRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IStorageService _storageService;
        private readonly CreateDeliveryDriverCommandHandler _sut;

        // A valid 1x1 PNG in base64 format
        private const string ValidBase64Image = "iVBORw0KGgoAAAANSUhEUgAAAAEAAAABCAQAAAC1HAwCAAAAC0lEQVR42mNkYAAAAAYAAjCB0C8AAAAASUVORK5CYII=";

        public CreateDeliveryDriverCommandHandlerTests()
        {
            _deliveryDriverRepository = Substitute.For<IDeliveryDriverRepository>();
            _unitOfWork = Substitute.For<IUnitOfWork>();
            _storageService = Substitute.For<IStorageService>();
            _sut = new CreateDeliveryDriverCommandHandler(_deliveryDriverRepository, _unitOfWork, _storageService);
        }

        [Fact]
        public async Task Handle_Should_CreateDeliveryDriver_WhenCommandIsValid()
        {
            // Arrange
            var command = new CreateDeliveryDriverCommand
            {
                Id = "driver-1",
                Name = "Test Driver",
                Cnpj = "12345678901234",
                BirthDate = new System.DateTime(1990, 1, 1),
                CnhNumber = "123456789",
                CnhType = "A",
                CnhImage = ValidBase64Image
            };

            _deliveryDriverRepository.CnpjExists(command.Cnpj).Returns(false);
            _storageService.SaveBase64FileAsync(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>()).Returns("cnh_images/some-path.png");

            // Act
            await _sut.Handle(command, CancellationToken.None);

            // Assert
            await _deliveryDriverRepository.Received(1).AddAsync(Arg.Is<DeliveryDriver>(d => d.Id == command.Id && d.Cnpj == command.Cnpj));
            await _storageService.Received(1).SaveBase64FileAsync("cnh", command.CnhImage, "cnh_images");
            await _unitOfWork.Received(1).SaveChangesAsync();
        }

        [Fact]
        public async Task Handle_Should_ThrowDomainException_WhenCnpjAlreadyExists()
        {
            // Arrange
            var command = new CreateDeliveryDriverCommand
            {
                Id = "driver-1",
                Name = "Test Driver",
                Cnpj = "12345678901234",
                BirthDate = new System.DateTime(1990, 1, 1),
                CnhNumber = "123456789",
                CnhType = "A",
                CnhImage = ValidBase64Image
            };

            _deliveryDriverRepository.CnpjExists(command.Cnpj).Returns(true);

            // Act
            Func<Task> act = async () => await _sut.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<DomainException>().WithMessage("Cnpj is already used by another delivery driver");
        }

        [Fact]
        public async Task Handle_Should_ThrowDomainException_WhenImageFormatIsInvalid()
        {
            // Arrange
            var command = new CreateDeliveryDriverCommand
            {
                CnhImage = "this-is-not-a-valid-base64-image"
            };

            // Act
            Func<Task> act = async () => await _sut.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<DomainException>().WithMessage("Base64 inválido");
        }
    }
}
