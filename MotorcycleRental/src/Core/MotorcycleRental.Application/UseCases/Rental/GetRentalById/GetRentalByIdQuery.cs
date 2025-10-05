
using MediatR;
using MotorcycleRental.Application.UseCases.Rental.DTOs;

namespace MotorcycleRental.Application.UseCases.Rental.GetRentalById
{
    public class GetRentalByIdQuery : IRequest<RentalDto>
    {
        public string Id { get; set; }
    }
}
