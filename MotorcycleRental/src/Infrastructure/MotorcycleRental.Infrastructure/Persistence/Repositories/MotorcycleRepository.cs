using Microsoft.EntityFrameworkCore;
using MotorcycleRental.Domain.Aggregates.Motorcycles;
using MotorcycleRental.Domain.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotorcycleRental.Infrastructure.Persistence.Repositories
{
    public class MotorcycleRepository : Repository<Motorcycle>, IMotorcycleRepository
    {
        public MotorcycleRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<bool> PlateExists(string licensePlate) => await _dbSet.AnyAsync(x => x.LicensePlate == licensePlate);
    }
}
