using MediatR;
using MotorcycleRental.Application.DTOs.Motorcycles;
using MotorcycleRental.Application.Mappers.Motorcycles;
using MotorcycleRental.Domain.Exceptions;
using MotorcycleRental.Domain.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotorcycleRental.Application.UseCases.Motorcycles.GetMotorcycleById
{
    public class GetMotorcycleByIdQueryHandler : IRequestHandler<GetMotorcycleByIdQuery, MotorcycleDto>
    {
        private readonly IMotorcycleRepository _repository;

        public GetMotorcycleByIdQueryHandler(IMotorcycleRepository repository)
        {
            _repository = repository;
        }

        public async Task<MotorcycleDto> Handle(GetMotorcycleByIdQuery request, CancellationToken cancellationToken)
        {
            var motorcycle = await _repository.GetByIdAsync(request.Id);
            
            if (motorcycle is null)
                throw new NotFoundException("Moto não encontrada");

            return motorcycle.MapToDto();
        }
    }
}
