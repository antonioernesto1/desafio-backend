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
        public async Task<bool> PlateExists(string licensePlate) => await _dbSet.AnyAsync(m => m.LicensePlate == licensePlate);

        public async Task<List<Motorcycle>> GetMotorcyclesAsync(string? licensePlate = null)
        {
            var query = _dbSet.AsQueryable();
            
            if(!string.IsNullOrEmpty(licensePlate))
                query = query.Where(m => m.LicensePlate == licensePlate);

            var motorcycles = await query.ToListAsync();
            
            return motorcycles;
        }

        public async Task<Motorcycle> GetByIdAsync(string id) => await _dbSet.FirstOrDefaultAsync(m => m.Id == id);
    }
}
