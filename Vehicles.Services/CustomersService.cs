using System;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Vehicles.Entities.DomainEntities;
using Vehicles.Entities.Enums;
using Vehicles.Entities.HelperEntities;
using Vehicles.Helpers.UnitOfWork;
using Vehicles.Repositories.DomainRepositories;

namespace Vehicles.Services
{
    public class CustomersService : ICustomersService
    {
        private readonly ICustomersRepository _customersRepository;
        private readonly IUnitOfWorkFactory _unitOfWorkFactory;
        public CustomersService(ICustomersRepository customersRepository, IUnitOfWorkFactory unitOfWorkFactory)
        {
            _customersRepository = customersRepository;
            _unitOfWorkFactory = unitOfWorkFactory;
        }

        public async Task<Page<Customer>> GetAllAsync(PagingOptions pagingOptions, bool getVehicles = true, SearchVehicleStatus vehicleStatus = SearchVehicleStatus.Any)
        {
            return await GetAsyncByName(string.Empty, pagingOptions, getVehicles, vehicleStatus);
        }

        public async Task<Customer> GetAsync(int id)
        {
            using (IDbConnection connection = _customersRepository.Connection)
            {
                return await _customersRepository.GetAsync(id);
            }
        }

        public async Task<Page<Customer>> GetAsyncByName(string name, PagingOptions pagingOptions, bool getVehicles = true, SearchVehicleStatus vehicleStatus = SearchVehicleStatus.Any)
        {
            IQueryable<Customer> query;
            using (IDbConnection connection = _customersRepository.Connection)
            {
                query = (await _customersRepository.GetAsyncByName(name)).AsQueryable();
            }

            var size = query.Count();
            var items = query.ToArray();
            
            if (getVehicles)
            {
                using (IUnitOfWork uow = _unitOfWorkFactory.GetInstance())
                {
                    foreach (var customer in items)
                    {
                        customer.Vehicles = await uow.VehiclesRepository.GetByCustomerId(customer.Id, vehicleStatus);
                    }
                }
                if (vehicleStatus != SearchVehicleStatus.Any)
                {
                    items = items.Where(c => c.Vehicles != null && c.Vehicles.Count() > 0).ToArray();
                }
            }

            items = items
                .Skip(pagingOptions.Offset.Value)
                .Take(pagingOptions.Limit.Value)
                .ToArray();

            return new Page<Customer>
            {
                Items = items,
                TotalSize = items.Count()
            };
        }

        public async Task<bool> AddAsync(Customer customer)
        {
            using (IUnitOfWork uow = _unitOfWorkFactory.GetInstance())
            {
                try
                {
                    if(!await uow.CustomersRepository.AddAsync(customer))
                        return false;
                    foreach (var vehicle in customer.Vehicles)
                    {
                        vehicle.CustomerId = customer.Id;
                        await uow.VehiclesRepository.AddAsync(vehicle);
                    }
                    uow.Commit();
                    return true;
                }
                catch(Exception ex)
                {
                    uow.Rollback();
                    return false;
                }
                
            }
        }

        public async Task<bool> UpdateAsync(int id, Customer customer)
        {
            using (IDbConnection connection = _customersRepository.Connection)
            {
                return await _customersRepository.UpdateAsync(id, customer);
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            using (IDbConnection connection = _customersRepository.Connection)
            {
                return await _customersRepository.DeleteAsync(id);
            }
        }
    }
}
