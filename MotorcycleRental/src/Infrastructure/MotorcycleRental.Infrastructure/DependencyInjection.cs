using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MotorcycleRental.Application.Interfaces;
using MotorcycleRental.Domain.Interfaces;
using MotorcycleRental.Domain.Interfaces.Repositories;
using MotorcycleRental.Infrastructure.Messaging;
using MotorcycleRental.Infrastructure.Persistence;
using MotorcycleRental.Infrastructure.Persistence.Repositories;

namespace MotorcycleRental.Infrastructure
{
    public static class DependencyInjection
    {
        public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("Default");
            services.AddDbContext<AppDbContext>(x 
                => x.UseNpgsql(connectionString)
                    .UseSnakeCaseNamingConvention());

            services.AddScoped<IUnitOfWork, UnitOfWork>();

            #region Repositories
            services.AddScoped<IMotorcycleRepository, MotorcycleRepository>();
            #endregion

            services.AddSingleton<IMessageBrokerService, RabbitMqService>();
        }
    }
}
