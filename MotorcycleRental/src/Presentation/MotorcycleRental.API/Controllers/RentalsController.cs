
using MediatR;
using Microsoft.AspNetCore.Mvc;
using MotorcycleRental.Application.UseCases.Rental.CreateRental;
using MotorcycleRental.Application.UseCases.Rental.GetRentalById;
using System.Threading.Tasks;

namespace MotorcycleRental.API.Controllers
{
    [Route("locacoes")]
    [ApiController]
    public class RentalsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public RentalsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> CreateRental(CreateRentalCommand request)
        {
            var id = await _mediator.Send(request);

            return Created();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetRentalById(string id)
        {
            var query = new GetRentalByIdQuery { Id = id };
            var rental = await _mediator.Send(query);
            return Ok(rental);
        }
    }
}
