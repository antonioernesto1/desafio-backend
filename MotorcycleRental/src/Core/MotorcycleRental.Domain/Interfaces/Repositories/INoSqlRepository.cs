using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotorcycleRental.Domain.Interfaces.Repositories
{
    public interface INoSqlRepository
    {
        Task SaveAsync<T>(string collectionName, T data) where T : class;
    }
}
