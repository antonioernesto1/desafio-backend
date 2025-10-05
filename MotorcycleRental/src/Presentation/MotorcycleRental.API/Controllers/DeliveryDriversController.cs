using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MotorcycleRental.Application.UseCases.DeliveryDrivers.CreateDeliveryDriver;
using MotorcycleRental.Application.UseCases.DeliveryDrivers.UpdateCnhImage;

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

        [HttpPost("{id}/cnh")]
        public async Task<IActionResult> UpdateCnhImage(UpdateCnhImageCommand request, string id)
        {
            request.Id = id;
            await _mediator.Send(request);
            return Created();
        }
    }
}
