
using MediatR;
using System.Text.Json.Serialization;

namespace MotorcycleRental.Application.UseCases.Rental.ReturnRental
{
    public class ReturnRentalCommand : IRequest
    {
        [JsonIgnore]
        public string Id { get; set; }

        [JsonPropertyName("data_devolucao")]
        public DateTime ReturnDate { get; set; }
    }
}
