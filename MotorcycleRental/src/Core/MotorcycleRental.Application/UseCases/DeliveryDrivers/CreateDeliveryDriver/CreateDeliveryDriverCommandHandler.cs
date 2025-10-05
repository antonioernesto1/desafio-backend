using MediatR;
using MotorcycleRental.Application.Interfaces;
using MotorcycleRental.Domain.Aggregates.DeliveryDrivers;
using MotorcycleRental.Domain.Aggregates.DeliveryDrivers.ValueObjects;
using MotorcycleRental.Domain.Exceptions;
using MotorcycleRental.Domain.Interfaces;
using MotorcycleRental.Domain.Interfaces.Repositories;
using MotorcycleRental.Domain.Validators;

namespace MotorcycleRental.Application.UseCases.DeliveryDrivers.CreateDeliveryDriver
{
    public class CreateDeliveryDriverCommandHandler : IRequestHandler<CreateDeliveryDriverCommand>
    {
        private readonly IDeliveryDriverRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IStorageService _storageService;
        public CreateDeliveryDriverCommandHandler(IDeliveryDriverRepository repository, IUnitOfWork unitOfWork, IStorageService storageService)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
            _storageService = storageService;
        }
        public async Task Handle(CreateDeliveryDriverCommand request, CancellationToken cancellationToken)
        {
            ImageFormatValidator.IsValidImageFormat(request.CnhImage, out string? errorMessage);

            if (!string.IsNullOrEmpty(errorMessage))
                throw new DomainException(errorMessage);

            var cnpjExists = await _repository.CnpjExists(request.Cnpj);

            if (cnpjExists)
                throw new DomainException("Cnpj is already used by another delivery driver");


            var cnh = new Cnh(request.CnhType, request.CnhNumber);
            
            var deliveryDriver = new DeliveryDriver(request.Id, request.Name, request.Cnpj, request.BirthDate, cnh);

            var path = await _storageService.SaveBase64FileAsync("cnh", request.CnhImage, "cnh_images");
            deliveryDriver.UpdateCnhImage(path);

            await _repository.AddAsync(deliveryDriver);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
