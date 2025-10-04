using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotorcycleRental.Application.UseCases.Motorcycles.DeleteMotorcycle
{
    public class DeleteMotorcycleCommand : IRequest
    {
        public string Id { get; set; }
    }
}
