
using MediatR;
using MotorcycleRental.Application.UseCases.Rental.CreateRental;
using MotorcycleRental.Domain.Exceptions;
using MotorcycleRental.Domain.Interfaces;
using MotorcycleRental.Domain.Interfaces.Repositories;
using System.Threading;
using System.Threading.Tasks;

namespace MotorcycleRental.Application.UseCases.Rental.CreateRental
{
    public class CreateRentalCommandHandler : IRequestHandler<CreateRentalCommand, string>
    {
        private readonly IRentalRepository _rentalRepository;
        private readonly IDeliveryDriverRepository _deliveryDriverRepository;
        private readonly IMotorcycleRepository _motorcycleRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CreateRentalCommandHandler(IRentalRepository rentalRepository, IDeliveryDriverRepository deliveryDriverRepository, IMotorcycleRepository motorcycleRepository, IUnitOfWork unitOfWork)
        {
            _rentalRepository = rentalRepository;
            _deliveryDriverRepository = deliveryDriverRepository;
            _motorcycleRepository = motorcycleRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<string> Handle(CreateRentalCommand request, CancellationToken cancellationToken)
        {
            var deliveryDriver = await _deliveryDriverRepository.GetByIdAsync(request.DeliveryDriverId);
            if (deliveryDriver == null)
            {
                throw new NotFoundException("Delivery driver not found.");
            }

            if (deliveryDriver.Cnh.Type != "A")
            {
                throw new DomainException("Delivery driver must have a type 'A' CNH to rent a motorcycle.");
            }

            var motorcycle = await _motorcycleRepository.GetByIdAsync(request.MotorcycleId);
            if (motorcycle == null)
            {
                throw new NotFoundException("Motorcycle not found.");
            }

            var motorcycleHasActiveRental = await _motorcycleRepository.HasActiveRentals(request.MotorcycleId);
            if (motorcycleHasActiveRental)
            {
                throw new DomainException("Motorcycle is already rented.");
            }

            var rental = new Domain.Aggregates.Rentals.Rental(
                request.DeliveryDriverId,
                request.MotorcycleId,
                request.InitialDate,
                request.EndDate,
                request.ExpectedEndDate,
                request.Plan
            );

            await _rentalRepository.AddAsync(rental);
            await _unitOfWork.SaveChangesAsync();

            return rental.Id.ToString();
        }
    }
}
