using System.Threading.Tasks;
using Vehicles.Entities.DomainEntities;
using Vehicles.Entities.Enums;
using Vehicles.Entities.HelperEntities;

namespace Vehicles.Services
{
    public interface ICustomersService
    {
        Task<Customer> GetAsync(int id);
        Task<Page<Customer>> GetAsyncByName(string name, PagingOptions pagingOptions, bool getVehicles, SearchVehicleStatus vehicleStatus);
        Task<Page<Customer>> GetAllAsync(PagingOptions pagingOptions, bool getVehicles, SearchVehicleStatus vehicleStatus);
        Task<bool> AddAsync(Customer customer);
        Task<bool> UpdateAsync(int id, Customer customer);
        Task<bool> DeleteAsync(int id);
    }
}
