using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MotorcycleRental.Application.UseCases.Motorcycles.CreateMotorcycle;

namespace MotorcycleRental.API.Controllers
{
    [Route("motos")]
    [ApiController]
    public class MotorcyclesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public MotorcyclesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> CreateMotorcycle(CreateMotorcycleCommand request)
        {
            var id = await _mediator.Send(request);

            return Created();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetMotorcycleById(string id)
        {
            return Ok();
        }
    }
}
