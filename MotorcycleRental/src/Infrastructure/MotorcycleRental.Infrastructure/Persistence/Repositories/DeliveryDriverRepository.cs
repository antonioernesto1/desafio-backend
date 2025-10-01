using MotorcycleRental.Domain.Aggregates.DeliveryDriver;
using MotorcycleRental.Domain.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotorcycleRental.Infrastructure.Persistence.Repositories
{
    public class DeliveryDriverRepository : Repository<DeliveryDriver>, IDeliveryDriverRepository
    {
        public DeliveryDriverRepository(AppDbContext context) : base(context)
        {
        }
    }
}
