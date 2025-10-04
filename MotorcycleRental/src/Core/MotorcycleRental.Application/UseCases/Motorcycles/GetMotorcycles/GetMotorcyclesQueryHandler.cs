using MediatR;
using MotorcycleRental.Application.DTOs.Motorcycles;
using MotorcycleRental.Application.Mappers.Motorcycles;
using MotorcycleRental.Domain.Interfaces;
using MotorcycleRental.Domain.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotorcycleRental.Application.UseCases.Motorcycles.GetMotorcycles
{
    public class GetMotorcyclesQueryHandler : IRequestHandler<GetMotorcyclesQuery, List<MotorcycleDto>>
    {
        private readonly IMotorcycleRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        public GetMotorcyclesQueryHandler(IMotorcycleRepository repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<List<MotorcycleDto>> Handle(GetMotorcyclesQuery request, CancellationToken cancellationToken)
        {
            var motorcycles = await _repository.GetMotorcyclesAsync(request.LicensePlate);
            
            var response = motorcycles.Select(
                    m => m.MapToDto()
                ).ToList();

            return response;
        }
    }
}
