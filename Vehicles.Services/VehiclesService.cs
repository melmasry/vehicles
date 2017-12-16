using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Vehicles.Entities.DomainEntities;
using Vehicles.Entities.Enums;
using Vehicles.Entities.HelperEntities;
using Vehicles.Repositories.DomainRepositories;

namespace Vehicles.Services
{
    public class VehiclesService : IVehiclesService
    {
        private readonly IVehiclesRepository _vehiclesRepository;
        public VehiclesService(IVehiclesRepository vehiclesRepository)
        {
            _vehiclesRepository = vehiclesRepository;
        }

        public async Task<Page<Vehicle>> GetAllAsync(PagingOptions pagingOptions)
        {
            IQueryable<Vehicle> query;
            using (IDbConnection connection = _vehiclesRepository.Connection)
            {
                query = (await _vehiclesRepository.GetAllAsync()).AsQueryable();
            }

            var size = query.Count();

            var items = query
                .Skip(pagingOptions.Offset.Value)
                .Take(pagingOptions.Limit.Value)
                .ToArray();

            return new Page<Vehicle>
            {
                Items = items,
                TotalSize = size
            };
        }

        public async Task<Vehicle> GetAsync(int id)
        {
            using (IDbConnection connection = _vehiclesRepository.Connection)
            {
                return await _vehiclesRepository.GetAsync(id);
            }
        }

        public async Task<Page<Vehicle>> GetByCustomerId(int customerId, SearchVehicleStatus vehicleStatus, PagingOptions pagingOptions)
        {
            IQueryable<Vehicle> query;
            using (IDbConnection connection = _vehiclesRepository.Connection)
            {
                query = (await _vehiclesRepository.GetByCustomerId(customerId, vehicleStatus)).AsQueryable();
            }

            var size = query.Count();

            var items = query
                .Skip(pagingOptions.Offset.Value)
                .Take(pagingOptions.Limit.Value)
                .ToArray();

            return new Page<Vehicle>
            {
                Items = items,
                TotalSize = size
            };
        }

        public async Task<Page<Vehicle>> GetByStatus(SearchVehicleStatus vehicleStatus, PagingOptions pagingOptions)
        {
            IQueryable<Vehicle> query;
            using (IDbConnection connection = _vehiclesRepository.Connection)
            {
                query = (await _vehiclesRepository.GetByStatus(vehicleStatus)).AsQueryable();
            }

            var size = query.Count();

            var items = query
                .Skip(pagingOptions.Offset.Value)
                .Take(pagingOptions.Limit.Value)
                .ToArray();

            return new Page<Vehicle>
            {
                Items = items,
                TotalSize = size
            };
        }
        public async Task<Vehicle> GetByRegNo(string regNo)
        {
            using (IDbConnection connection = _vehiclesRepository.Connection)
            {
                return await _vehiclesRepository.GetByRegNo(regNo);
            }
        }

        public async Task<Vehicle> GetByVehicleId(string vehicleId)
        {
            using (IDbConnection connection = _vehiclesRepository.Connection)
            {
                return await _vehiclesRepository.GetByVehicleId(vehicleId);
            }
        }

        public async Task<bool> AddAsync(Vehicle vehicle)
        {
            using (IDbConnection connection = _vehiclesRepository.Connection)
            {
                return await _vehiclesRepository.AddAsync(vehicle);
            }
        }

        public async Task<bool> UpdateAsync(int id, Vehicle vehicle)
        {
            using (IDbConnection connection = _vehiclesRepository.Connection)
            {
                return await _vehiclesRepository.UpdateAsync(id, vehicle);
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            using (IDbConnection connection = _vehiclesRepository.Connection)
            {
                return await _vehiclesRepository.DeleteAsync(id);
            }
        }

        public async Task<bool> PingAsync(int id)
        {
            using (IDbConnection connection = _vehiclesRepository.Connection)
            {
                return await _vehiclesRepository.PingAsync(id);
            }
        }
    }
}
