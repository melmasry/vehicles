using System.Threading.Tasks;
using Vehicles.Entities.DomainEntities;
using Vehicles.Entities.Enums;
using Vehicles.Entities.HelperEntities;

namespace Vehicles.Services
{
    public interface IVehiclesService
    {
        Task<Vehicle> GetAsync(int id);
        Task<Vehicle> GetByVehicleId(string vehicleId);
        Task<Vehicle> GetByRegNo(string regNo);
        Task<Page<Vehicle>> GetByCustomerId(int customerId, SearchVehicleStatus vehicleStatus, PagingOptions pagingOptions);
        Task<Page<Vehicle>> GetByStatus(SearchVehicleStatus vehicleStatus, PagingOptions pagingOptions);
        Task<Page<Vehicle>> GetAllAsync(PagingOptions pagingOptions);
        Task<bool> AddAsync(Vehicle vehicle);
        Task<bool> UpdateAsync(int id, Vehicle vehicle);
        Task<bool> DeleteAsync(int id);

        Task<bool> PingAsync(int id);
    }
}
