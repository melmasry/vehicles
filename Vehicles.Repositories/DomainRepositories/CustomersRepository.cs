using Dapper;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Vehicles.Entities.DomainEntities;
using Vehicles.Entities.HelperEntities;

namespace Vehicles.Repositories.DomainRepositories
{
    public class CustomersRepository : BaseRepository, ICustomersRepository
    {
        public CustomersRepository(IOptions<DatabaseOptions> databaseOptions) : base(databaseOptions)
        { }

        public async Task<IEnumerable<Customer>> GetAllAsync()
        {
            return await Connection.QueryAsync<Customer>("GetCustomers", commandType: CommandType.StoredProcedure, transaction: Transaction);
        }

        public async Task<Customer> GetAsync(int id)
        {
            using (IDbConnection dbConnection = Connection)
            {
                return (await dbConnection.QueryAsync<Customer>("GetCustomers", new { CustomerId = id }, commandType: CommandType.StoredProcedure)).FirstOrDefault();
            }
        }

        public async Task<IEnumerable<Customer>> GetAsyncByName(string name)
        {
            return (await Connection.QueryAsync<Customer>("GetCustomers", new { Name = name }, commandType: CommandType.StoredProcedure, transaction: Transaction));
        }

        public async Task<bool> AddAsync(Customer customer)
        {
            customer.Id = (int)await Connection.ExecuteScalarAsync(@"INSERT INTO [dbo].[Customers]([Name],[AddressLn1],[AddressLn2],[AddressLn3],[Phone])
                                        OUTPUT INSERTED.ID
                                        VALUES (@Name, @AddressLn1, @AddressLn2, @AddressLn3, @Phone)", new
            {
                Id = customer.Id,
                Name = customer.Name,
                AddressLn1 = customer.AddressLn1,
                AddressLn2 = customer.AddressLn2,
                AddressLn3 = customer.AddressLn3,
                Phone = customer.Phone
            }, transaction: Transaction);

            return true;
        }

        public async Task<bool> UpdateAsync(int id, Customer customer)
        {
            int rowsAffected = await Connection.ExecuteAsync(@"UPDATE [dbo].[Customers]
                                        SET [Name] = @Name, [AddressLn1] = @AddressLn1, [AddressLn2] = @AddressLn2, [AddressLn3] = @AddressLn3, [Phone] = @Phone WHERE [Id] = @Id", new
            {
                Id = id,
                Name = customer.Name,
                AddressLn1 = customer.AddressLn1,
                AddressLn2 = customer.AddressLn2,
                AddressLn3 = customer.AddressLn3,
                Phone = customer.Phone
            }, transaction: Transaction);

            if (rowsAffected > 0)
            {
                return true;
            }
            return false;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            int rowsAffected = await Connection.ExecuteAsync(@"DELETE FROM [Customers] WHERE Id = @CustomerID", new { CustomerID = id }, transaction: Transaction);
            if (rowsAffected > 0)
            {
                return true;
            }
            return false;
        }
    }
}
