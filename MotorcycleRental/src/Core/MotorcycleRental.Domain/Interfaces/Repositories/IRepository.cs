using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotorcycleRental.Domain.Interfaces.Repositories
{
    public interface IRepository<T> where T : class
    {
        Task AddAsync(T entity);
        void Delete(T entity);
        List<T> GetAll();
    }
}
