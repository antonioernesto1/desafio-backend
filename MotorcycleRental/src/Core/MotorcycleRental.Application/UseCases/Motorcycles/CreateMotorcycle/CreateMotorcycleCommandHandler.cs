using MediatR;
using MotorcycleRental.Domain.Aggregates.Motorcycles;
using MotorcycleRental.Domain.Exceptions;
using MotorcycleRental.Domain.Interfaces;
using MotorcycleRental.Domain.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotorcycleRental.Application.UseCases.Motorcycles.CreateMotorcycle
{
    public class CreateMotorcycleCommandHandler : IRequestHandler<CreateMotorcycleCommand, string>
    {
        private readonly IMotorcycleRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        public CreateMotorcycleCommandHandler(IMotorcycleRepository repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<string> Handle(CreateMotorcycleCommand request, CancellationToken cancellationToken)
        {
            var plateExists = await _repository.PlateExists(request.LicensePlate);
            if (plateExists)
                throw new DomainException("The provided license plate is used by another motorcycle");

            var motorcycle = new Motorcycle(request.Id, request.Year, request.Model, request.LicensePlate);

            await _repository.AddAsync(motorcycle);
            await _unitOfWork.SaveChangesAsync();

            //TODO: Implement motorcycle created event. Need to publish it using MediatR
            
            return motorcycle.Id;
        }
    }
}
