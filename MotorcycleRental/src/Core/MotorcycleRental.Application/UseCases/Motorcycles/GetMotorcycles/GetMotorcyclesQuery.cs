using MediatR;
using MotorcycleRental.Application.DTOs.Motorcycles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotorcycleRental.Application.UseCases.Motorcycles.GetMotorcycles
{
    public class GetMotorcyclesQuery : IRequest<List<MotorcycleDto>>
    {
        public string? LicensePlate { get; set; }
    }
}
