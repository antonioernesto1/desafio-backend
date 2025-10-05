using MediatR;
using MotorcycleRental.Application.Interfaces;
using MotorcycleRental.Application.UseCases.DeliveryDrivers.CreateDeliveryDriver;
using MotorcycleRental.Domain.Exceptions;
using MotorcycleRental.Domain.Interfaces;
using MotorcycleRental.Domain.Interfaces.Repositories;
using MotorcycleRental.Domain.Validators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            var isValid = ImageFormatValidator.IsValidImageFormat(request.CnhImage, out string? errorMessage);

            if (!string.IsNullOrEmpty(errorMessage))
                throw new DomainException(errorMessage);

            var deliveryDriver = await _repository.GetByIdAsync(request.Id);

            await _storageService.DeleteAsync(deliveryDriver.Cnh.ImagePath);
            var path = await _storageService.SaveBase64FileAsync("cnh", request.CnhImage, "cnh_images");

            deliveryDriver.UpdateCnhImage(path);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
