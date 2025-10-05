using MediatR;
using MotorcycleRental.Application.Interfaces;
using MotorcycleRental.Domain.Exceptions;
using MotorcycleRental.Domain.Interfaces;
using MotorcycleRental.Domain.Interfaces.Repositories;
using MotorcycleRental.Domain.Validators;

namespace MotorcycleRental.Application.UseCases.DeliveryDrivers.UpdateCnhImage
{
    public class UpdateCnhImageCommandHandler : IRequestHandler<UpdateCnhImageCommand>
    {
        private readonly IDeliveryDriverRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IStorageService _storageService;

        public UpdateCnhImageCommandHandler(IDeliveryDriverRepository repository, 
            IUnitOfWork unitOfWork, IStorageService storageService)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
            _storageService = storageService;
        }

        public async Task Handle(UpdateCnhImageCommand request, CancellationToken cancellationToken)
        {
            ImageFormatValidator.IsValidImageFormat(request.CnhImage, out string? errorMessage);

            if (!string.IsNullOrEmpty(errorMessage))
            {
                throw new DomainException(errorMessage);
            }

            var deliveryDriver = await _repository.GetByIdAsync(request.Id);

            if (deliveryDriver is null)
            {
                throw new NotFoundException("Entregador não encontrado"); // Corresponds to a 404 Not Found
            }

            await _storageService.DeleteAsync(deliveryDriver.Cnh.ImagePath);
            var path = await _storageService.SaveBase64FileAsync("cnh", request.CnhImage, "cnh_images");

            deliveryDriver.UpdateCnhImage(path);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
