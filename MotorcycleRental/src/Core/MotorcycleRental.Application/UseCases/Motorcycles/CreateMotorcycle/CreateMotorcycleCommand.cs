using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MotorcycleRental.Application.UseCases.Motorcycles.CreateMotorcycle
{
    public class CreateMotorcycleCommand : IRequest<string>
    {
        [JsonPropertyName("identificador")]
        public string Id { get; set; }
        [JsonPropertyName("ano")]
        public int Year { get; set; }
        [JsonPropertyName("modelo")]
        public string Model { get; set; }
        [JsonPropertyName("placa")]
        public string LicensePlate { get; set; }
    }
}
