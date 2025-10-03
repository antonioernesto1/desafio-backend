using MediatR;
using MotorcycleRental.Application.DTOs.Motorcycles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotorcycleRental.Application.UseCases.Motorcycles.GetMotorcycleById
{
    public class GetMotorcycleByIdQuery : IRequest<MotorcycleDto>
    {
        public string Id { get; set; }
    }
}
