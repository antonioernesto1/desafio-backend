using MediatR;
using MotorcycleRental.Domain.Exceptions;
using MotorcycleRental.Domain.Interfaces;
using MotorcycleRental.Domain.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotorcycleRental.Application.UseCases.Motorcycles.UpdateLicensePlate
{
    public class UpdateLicensePlateCommandHandler : IRequestHandler<UpdateLicensePlateCommand>
    {
        private readonly IMotorcycleRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        public UpdateLicensePlateCommandHandler(IMotorcycleRepository repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }
        public async Task Handle(UpdateLicensePlateCommand request, CancellationToken cancellationToken)
        {
            var licensePlateExists = await _repository.PlateExists(request.LicensePlate);

            if (licensePlateExists)
                throw new DomainException($"The license plate '{request.LicensePlate}' is already assigned to another motorcycle.");

            var motorcycle = await _repository.GetByIdAsync(request.Id);

            if (request.LicensePlate == motorcycle.LicensePlate)
                throw new NotFoundException("Moto não encontrada");

            if (motorcycle is null)
                throw new DomainException("The new license plate value cannot be the same as the currently registered value.");

            motorcycle.UpdateLicensePlate(request.LicensePlate);

            await _unitOfWork.SaveChangesAsync();
        }
    }
}
