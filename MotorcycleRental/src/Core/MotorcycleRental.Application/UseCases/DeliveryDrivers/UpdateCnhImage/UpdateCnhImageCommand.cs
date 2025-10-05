using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MotorcycleRental.Application.UseCases.DeliveryDrivers.UpdateCnhImage
{
    public class UpdateCnhImageCommand : IRequest
    {
        [JsonPropertyName("imagem_cnh")]
        public string CnhImage { get; set; }
        [JsonIgnore]
        public string? Id { get; set; }
    }
}
