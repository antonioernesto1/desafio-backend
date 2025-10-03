using MotorcycleRental.Domain.Aggregates.Motorcycles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotorcycleRental.Domain.Interfaces.Repositories
{
    public interface IMotorcycleRepository : IRepository<Motorcycle>
    {
        public Task<bool> PlateExists(string licensePlate);
        public Task<Motorcycle> GetByIdAsync(string id);
        public Task<List<Motorcycle>> GetMotorcyclesAsync(string? licensePlate = null);
    }
}
