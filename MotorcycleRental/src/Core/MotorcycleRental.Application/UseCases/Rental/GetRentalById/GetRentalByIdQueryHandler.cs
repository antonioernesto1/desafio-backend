
using MediatR;
using MotorcycleRental.Application.UseCases.Rental.DTOs;
using MotorcycleRental.Domain.Exceptions;
using MotorcycleRental.Domain.Interfaces.Repositories;
using System.Threading;
using System.Threading.Tasks;

namespace MotorcycleRental.Application.UseCases.Rental.GetRentalById
{
    public class GetRentalByIdQueryHandler : IRequestHandler<GetRentalByIdQuery, RentalDto>
    {
        private readonly IRentalRepository _rentalRepository;

        public GetRentalByIdQueryHandler(IRentalRepository rentalRepository)
        {
            _rentalRepository = rentalRepository;
        }

        public async Task<RentalDto> Handle(GetRentalByIdQuery request, CancellationToken cancellationToken)
        {
            var rental = await _rentalRepository.GetByIdAsync(request.Id);

            if (rental == null)
            {
                throw new NotFoundException("Locação não encontrada");
            }

            var rentalDto = new RentalDto
            {
                Id = rental.Id.ToString(),
                DailyRate = rental.GetDailyRate(),
                DeliveryDriverId = rental.DeliveryDriverId,
                MotorcycleId = rental.MotorcycleId,
                InitialDate = rental.InitialDate,
                EndDate = rental.EndDate,
                ExpectedEndDate = rental.ExpectedEndDate,
                ReturnDate = rental.ReturnDate
            };

            return rentalDto;
        }
    }
}
