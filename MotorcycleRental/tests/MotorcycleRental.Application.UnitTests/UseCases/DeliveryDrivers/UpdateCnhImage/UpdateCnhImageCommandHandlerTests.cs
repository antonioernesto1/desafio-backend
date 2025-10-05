
using FluentAssertions;
using MotorcycleRental.Application.Interfaces;
using MotorcycleRental.Application.UseCases.DeliveryDrivers.UpdateCnhImage;
using MotorcycleRental.Domain.Aggregates.DeliveryDrivers;
using MotorcycleRental.Domain.Aggregates.DeliveryDrivers.ValueObjects;
using MotorcycleRental.Domain.Exceptions;
using MotorcycleRental.Domain.Interfaces;
using MotorcycleRental.Domain.Interfaces.Repositories;
using NSubstitute;
using System.Threading.Tasks;
using Xunit;

namespace MotorcycleRental.Application.UnitTests.UseCases.DeliveryDrivers.UpdateCnhImage
{
    public class UpdateCnhImageCommandHandlerTests
    {
        private readonly IDeliveryDriverRepository _deliveryDriverRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IStorageService _storageService;
        private readonly UpdateCnhImageCommandHandler _sut;

        private const string ValidBase64Image = "iVBORw0KGgoAAAANSUhEUgAAAAEAAAABCAQAAAC1HAwCAAAAC0lEQVR42mNkYAAAAAYAAjCB0C8AAAAASUVORK5CYII=";

        public UpdateCnhImageCommandHandlerTests()
        {
            _deliveryDriverRepository = Substitute.For<IDeliveryDriverRepository>();
            _unitOfWork = Substitute.For<IUnitOfWork>();
            _storageService = Substitute.For<IStorageService>();
            _sut = new UpdateCnhImageCommandHandler(_deliveryDriverRepository, _unitOfWork, _storageService);
        }

        [Fact]
        public async Task Handle_Should_UpdateCnhImage_WhenCommandIsValid()
        {
            // Arrange
            var command = new UpdateCnhImageCommand { Id = "driver-1", CnhImage = ValidBase64Image };
            var cnh = new Cnh("A", "123456789");
            var deliveryDriver = new DeliveryDriver(command.Id, "Test Name", "12345678901234", new System.DateTime(1990, 5, 5), cnh);
            deliveryDriver.UpdateCnhImage("cnh_images/old_path.png");

            _deliveryDriverRepository.GetByIdAsync(command.Id).Returns(deliveryDriver);
            _storageService.SaveBase64FileAsync(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>()).Returns("cnh_images/new_path.png");

            // Act
            await _sut.Handle(command, CancellationToken.None);

            // Assert
            await _storageService.Received(1).DeleteAsync("cnh_images/old_path.png");
            await _storageService.Received(1).SaveBase64FileAsync("cnh", command.CnhImage, "cnh_images");
            deliveryDriver.Cnh.ImagePath.Should().Be("cnh_images/new_path.png");
            await _unitOfWork.Received(1).SaveChangesAsync();
        }

        [Fact]
        public async Task Handle_Should_ThrowNotFoundException_WhenDriverDoesNotExist()
        {
            // Arrange
            var command = new UpdateCnhImageCommand { Id = "non-existent-driver", CnhImage = ValidBase64Image };
            _deliveryDriverRepository.GetByIdAsync(command.Id).Returns((DeliveryDriver)null);

            // Act
            Func<Task> act = async () => await _sut.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<NotFoundException>().WithMessage("Entregador não encontrado");
        }

        [Fact]
        public async Task Handle_Should_ThrowDomainException_WhenImageIsInvalid()
        {
            // Arrange
            var command = new UpdateCnhImageCommand { Id = "driver-1", CnhImage = "invalid-image-data" };

            // Act
            Func<Task> act = async () => await _sut.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<DomainException>().WithMessage("Base64 inválido");
        }
    }
}
