using MediatR;
using MotorcycleRental.Domain.Exceptions;
using MotorcycleRental.Domain.Interfaces;
using MotorcycleRental.Domain.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotorcycleRental.Application.UseCases.Motorcycles.DeleteMotorcycle
{
    public class DeleteMotorcycleCommandHandler : IRequestHandler<DeleteMotorcycleCommand>
    {
        private readonly IMotorcycleRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteMotorcycleCommandHandler(IMotorcycleRepository repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }
        public async Task Handle(DeleteMotorcycleCommand request, CancellationToken cancellationToken)
        {
            var motorcycle = await _repository.GetByIdAsync(request.Id);

            if (motorcycle == null)
                throw new NotFoundException("Moto não encontrada");

            var anyRentals = await _repository.HasActiveRentals(motorcycle.Id);

            if (anyRentals)
                throw new DomainException("Cannot delete a motorcycle with active rentals");

            _repository.Delete(motorcycle);

            await _unitOfWork.SaveChangesAsync();
        }
    }
}
