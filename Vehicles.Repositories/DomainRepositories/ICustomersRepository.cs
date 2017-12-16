using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Vehicles.Entities.DomainEntities;

namespace Vehicles.Repositories.DomainRepositories
{
    public interface ICustomersRepository
    {
        IDbConnection Connection { get; set; }
        IDbTransaction Transaction { get; set; }
        Task<Customer> GetAsync(int id);
        Task<IEnumerable<Customer>> GetAsyncByName(string name);
        Task<IEnumerable<Customer>> GetAllAsync();
        Task<bool> AddAsync(Customer customer);
        Task<bool> UpdateAsync(int id, Customer customer);
        Task<bool> DeleteAsync(int id);
    }
}
