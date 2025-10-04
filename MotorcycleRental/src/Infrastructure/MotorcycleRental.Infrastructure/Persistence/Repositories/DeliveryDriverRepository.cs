using Microsoft.EntityFrameworkCore;
using MotorcycleRental.Domain.Aggregates.DeliveryDrivers;
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

        public async Task<bool> CnpjExists(string cnpj) => await _dbSet.AnyAsync(x => x.Cnpj == cnpj);
    }
}
