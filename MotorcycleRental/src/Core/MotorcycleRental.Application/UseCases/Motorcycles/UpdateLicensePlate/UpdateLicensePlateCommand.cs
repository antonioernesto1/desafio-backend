using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MotorcycleRental.Application.UseCases.Motorcycles.UpdateLicensePlate
{
    public class UpdateLicensePlateCommand : IRequest
    {
        [JsonPropertyName("placa")]
        public string LicensePlate { get; set; }
        [JsonIgnore]
        public string? Id { get; set; }
    }
}
