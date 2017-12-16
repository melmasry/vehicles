using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Vehicles.Entities.DomainEntities;
using Vehicles.Entities.Enums;

namespace Vehicles.Repositories.DomainRepositories
{
    public interface IVehiclesRepository
    {
        IDbConnection Connection { get; set; }
        IDbTransaction Transaction { get; set; }
        Task<Vehicle> GetAsync(int id);
        Task<Vehicle> GetByVehicleId(string vehicleId);
        Task<Vehicle> GetByRegNo(string regNo);
        Task<IEnumerable<Vehicle>> GetByCustomerId(int customerId, SearchVehicleStatus vehicleStatus);
        Task<IEnumerable<Vehicle>> GetByStatus(SearchVehicleStatus searchStatus);
        Task<IEnumerable<Vehicle>> GetAllAsync();
        Task<bool> AddAsync(Vehicle vehicle);
        Task<bool> UpdateAsync(int id, Vehicle vehicle);
        Task<bool> DeleteAsync(int id);

        Task<bool> PingAsync(int id);
    }
}
