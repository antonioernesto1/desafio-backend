using Microsoft.EntityFrameworkCore;
using MotorcycleRental.Domain.Aggregates.Rentals;
using MotorcycleRental.Domain.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotorcycleRental.Infrastructure.Persistence.Repositories
{
    public class RentalRepository : Repository<Rental>, IRentalRepository
    {
        public RentalRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<Rental> GetByIdAsync(string id)  => await _dbSet.FirstOrDefaultAsync(x => x.Id == new Guid(id));
    }
}
