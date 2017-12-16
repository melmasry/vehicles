using Microsoft.Extensions.Options;
using Vehicles.Entities.HelperEntities;
using Vehicles.Repositories.DomainRepositories;

namespace Vehicles.Helpers.UnitOfWork
{
    public class UnitOfWorkFactory : IUnitOfWorkFactory
    {
        private readonly ICustomersRepository _customersRepository;
        private readonly IVehiclesRepository _vehiclesRepository;
        private readonly IOptions<DatabaseOptions> _databaseOptions;

        public UnitOfWorkFactory(IOptions<DatabaseOptions> databaseOptions, ICustomersRepository customersRepository, IVehiclesRepository vehiclesRepository)
        {
            _customersRepository = customersRepository;
            _vehiclesRepository = vehiclesRepository;
            _databaseOptions = databaseOptions;
        }
        public IUnitOfWork GetInstance()
        {
            return new UnitOfWork(_databaseOptions, _customersRepository, _vehiclesRepository);
        }
    }
}
