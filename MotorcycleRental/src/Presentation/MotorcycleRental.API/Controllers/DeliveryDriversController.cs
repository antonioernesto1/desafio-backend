using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MotorcycleRental.Application.UseCases.DeliveryDrivers.CreateDeliveryDriver;

namespace MotorcycleRental.API.Controllers
{
    [Route("entregadores")]
    [ApiController]
    public class DeliveryDriversController : ControllerBase
    {
        private readonly IMediator _mediator;
        public DeliveryDriversController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> CreateDeliveryDriver(CreateDeliveryDriverCommand request)
        {
            await _mediator.Send(request);
            return Created();
        }
    }
}
