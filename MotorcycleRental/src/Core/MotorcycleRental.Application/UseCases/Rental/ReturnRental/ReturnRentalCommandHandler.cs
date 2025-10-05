
using MediatR;
using MotorcycleRental.Domain.Exceptions;
using MotorcycleRental.Domain.Interfaces;
using MotorcycleRental.Domain.Interfaces.Repositories;

namespace MotorcycleRental.Application.UseCases.Rental.ReturnRental
{
    public class ReturnRentalCommandHandler : IRequestHandler<ReturnRentalCommand>
    {
        private readonly IRentalRepository _rentalRepository;
        private readonly IUnitOfWork _unitOfWork;

        public ReturnRentalCommandHandler(IRentalRepository rentalRepository, IUnitOfWork unitOfWork)
        {
            _rentalRepository = rentalRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(ReturnRentalCommand request, CancellationToken cancellationToken)
        {
            var rental = await _rentalRepository.GetByIdAsync(request.Id);

            if (rental is null)
            {
                throw new NotFoundException("Locação não encontrada");
            }

            rental.ReturnMotorcycle(request.ReturnDate);

            await _unitOfWork.SaveChangesAsync();
        }
    }
}
