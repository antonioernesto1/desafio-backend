using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotorcycleRental.Domain.Events
{
    public record MotorcycleCreatedEvent(string MotorcycleId, int Year) : INotification;
}
