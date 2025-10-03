using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MotorcycleRental.Application.DTOs.Motorcycles
{
    public record MotorcycleDto
    {
        [JsonPropertyName("identificador")]
        public string Id { get; init; }

        [JsonPropertyName("ano")]
        public int Year { get; init; }

        [JsonPropertyName("modelo")]
        public string Model { get; init; }

        [JsonPropertyName("placa")]
        public string LicensePlate { get; init; }
    }
}
