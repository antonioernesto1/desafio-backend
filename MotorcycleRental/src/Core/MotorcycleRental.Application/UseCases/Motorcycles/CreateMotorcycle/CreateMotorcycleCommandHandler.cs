using MediatR;
using MotorcycleRental.Domain.Aggregates.Motorcycles;
using MotorcycleRental.Domain.Events;
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
        private readonly IMediator _mediator;
        public CreateMotorcycleCommandHandler(IMotorcycleRepository repository, IUnitOfWork unitOfWork, 
            IMediator mediator)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
            _mediator = mediator;
        }

        public async Task<string> Handle(CreateMotorcycleCommand request, CancellationToken cancellationToken)
        {
            var plateExists = await _repository.PlateExists(request.LicensePlate);
            if (plateExists)
                throw new DomainException("The provided license plate is used by another motorcycle");

            var motorcycle = new Motorcycle(request.Id, request.Year, request.Model, request.LicensePlate);

            await _repository.AddAsync(motorcycle);
            await _unitOfWork.SaveChangesAsync();

            await _mediator.Publish(new MotorcycleCreatedEvent(motorcycle.Id, motorcycle.Year));
            
            return motorcycle.Id;
        }
    }
}
