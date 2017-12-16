using Microsoft.Extensions.DependencyInjection;
using Vehicles.Helpers.UnitOfWork;
using Vehicles.Repositories.DomainRepositories;
using Vehicles.Services;

namespace Vehicles.DependencyInjection
{
    public static class DependencyInjector
    {
        public static void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<IUnitOfWorkFactory, UnitOfWorkFactory>();

            services.AddTransient<IVehiclesRepository, VehiclesRepository>();
            services.AddTransient<ICustomersRepository, CustomersRepository>();
            services.AddTransient<IVehiclesService, VehiclesService>();
            services.AddTransient<ICustomersService, CustomersService>();
        }
    }
}
