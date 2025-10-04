using MotorcycleRental.Application.DTOs.Motorcycles;
using MotorcycleRental.Domain.Aggregates.Motorcycles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotorcycleRental.Application.Mappers.Motorcycles
{
    internal static class MotorcycleDtoMapper
    {
        public static MotorcycleDto MapToDto(this Motorcycle motorcycle)
        {
            return new MotorcycleDto
            {
                Id = motorcycle.Id,
                LicensePlate = motorcycle.LicensePlate,
                Model = motorcycle.Model,
                Year = motorcycle.Year,
            };
        }
    }
}
