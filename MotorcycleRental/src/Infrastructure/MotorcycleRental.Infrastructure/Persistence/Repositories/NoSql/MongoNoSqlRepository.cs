using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MotorcycleRental.Domain.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotorcycleRental.Infrastructure.Persistence.Repositories.NoSql
{
    public class MongoNoSqlRepository : INoSqlRepository
    {
        private readonly IMongoDatabase _database;
        public MongoNoSqlRepository(IConfiguration configuration)
        {
            var client = new MongoClient(configuration.GetConnectionString("MongoDB"));
            _database = client.GetDatabase(configuration["MongoDB:Database"]);
        }
        public async Task SaveAsync<T>(string collectionName, T data) where T : class
        {
            var collection = _database.GetCollection<T>(collectionName);
            await collection.InsertOneAsync(data);
        }
    }
}
