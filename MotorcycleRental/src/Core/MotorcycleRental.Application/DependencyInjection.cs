using Microsoft.Extensions.DependencyInjection;
using MotorcycleRental.Application.UseCases.Motorcycles.CreateMotorcycle;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotorcycleRental.Application
{
    public static class DependencyInjection
    {
        public static void AddCoreServices(this IServiceCollection services)
        {
            services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssembly(typeof(CreateMotorcycleCommandHandler).Assembly);
            });
        }
    }
}
