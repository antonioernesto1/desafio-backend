using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MotorcycleRental.Application.Interfaces;
using MotorcycleRental.Domain.Interfaces;
using MotorcycleRental.Domain.Interfaces.Repositories;
using MotorcycleRental.Infrastructure.Messaging;
using MotorcycleRental.Infrastructure.Persistence;
using MotorcycleRental.Infrastructure.Persistence.Repositories;
using MotorcycleRental.Infrastructure.Persistence.Repositories.NoSql;

namespace MotorcycleRental.Infrastructure
{
    public static class DependencyInjection
    {
        public static void AddDatabase(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("Default");
            services.AddDbContext<AppDbContext>(x
                => x.UseNpgsql(connectionString)
                    .UseSnakeCaseNamingConvention());

            services.AddScoped<IUnitOfWork, UnitOfWork>();

            #region Repositories
            services.AddScoped<IMotorcycleRepository, MotorcycleRepository>();
            #endregion
        }

        public static void AddNoSql(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<INoSqlRepository, MongoNoSqlRepository>();
        }

        public static void AddMessageBroker(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IMessageBrokerService, RabbitMqService>();
        }
    }
}
