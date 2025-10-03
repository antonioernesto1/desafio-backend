using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MotorcycleRental.Application.UseCases.Motorcycles.CreateMotorcycle;
using MotorcycleRental.Application.UseCases.Motorcycles.GetMotorcycleById;
using MotorcycleRental.Application.UseCases.Motorcycles.GetMotorcycles;
using MotorcycleRental.Application.UseCases.Motorcycles.UpdateLicensePlate;

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

        [HttpGet]
        public async Task<IActionResult> GetMotorcycles([FromQuery(Name = "placa")] string? licensePlate = null)
        {
            var query = new GetMotorcyclesQuery
            {
                LicensePlate = licensePlate
            };

            var motorcycles = await _mediator.Send(query);
            return Ok(motorcycles);
        }

        [HttpPut("{id}/placa")]
        public async Task<IActionResult> UpdateLicensePlate(string id, [FromBody] UpdateLicensePlateCommand request)
        {
            request.Id = id;
            await _mediator.Send(request);

            return Ok("Placa modificada com sucesso");
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetMotorcycleById(string id)
        {
            var query = new GetMotorcycleByIdQuery
            {
                Id = id
            };

            var motorcycle = await _mediator.Send(query);

            return Ok(motorcycle);
        }
    }
}
