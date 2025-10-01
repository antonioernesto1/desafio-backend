using MotorcycleRental.Domain.Aggregates.Rentals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotorcycleRental.Domain.Interfaces.Repositories
{
    public interface IRentalRepository : IRepository<Rental>
    {
    }
}
