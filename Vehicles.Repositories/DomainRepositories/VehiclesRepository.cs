using Dapper;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Vehicles.Entities.DomainEntities;
using Vehicles.Entities.Enums;
using Vehicles.Entities.HelperEntities;

namespace Vehicles.Repositories.DomainRepositories
{
    public class VehiclesRepository : BaseRepository, IVehiclesRepository
    {
        public VehiclesRepository(IOptions<DatabaseOptions> databaseOptions) : base(databaseOptions)
        { }

        public async Task<IEnumerable<Vehicle>> GetAllAsync()
        {
            return await Connection.QueryAsync<Vehicle>("GetCustomerVehicles", commandType: CommandType.StoredProcedure, transaction: Transaction);
        }

        public async Task<Vehicle> GetAsync(int id)
        {
            return (await Connection.QueryAsync<Vehicle>("GetCustomerVehicles", new { VehicleId = id }, commandType: CommandType.StoredProcedure, transaction: Transaction)).FirstOrDefault();
        }

        public async Task<IEnumerable<Vehicle>> GetByCustomerId(int customerId, SearchVehicleStatus vehicleStatus)
        {
            return await Connection.QueryAsync<Vehicle>("GetCustomerVehicles", new { CustomerId = customerId, IsActive = vehicleStatus }, commandType: CommandType.StoredProcedure, transaction: Transaction);
        }

        public async Task<IEnumerable<Vehicle>> GetByStatus(SearchVehicleStatus searchStatus)
        {
            return await Connection.QueryAsync<Vehicle>("GetCustomerVehicles", new { IsActive = (int)searchStatus }, commandType: CommandType.StoredProcedure, transaction: Transaction);
        }

        public async Task<Vehicle> GetByRegNo(string regNo)
        {
            return (await Connection.QueryAsync<Vehicle>("GetCustomerVehicles", new { RegNo = regNo }, commandType: CommandType.StoredProcedure, transaction: Transaction)).FirstOrDefault();
        }

        public async Task<Vehicle> GetByVehicleId(string vehicleId)
        {
            return (await Connection.QueryAsync<Vehicle>("GetCustomerVehicles", new { VIN = vehicleId }, commandType: CommandType.StoredProcedure, transaction: Transaction)).FirstOrDefault();
        }

        public async Task<bool> AddAsync(Vehicle vehicle)
        {
            int rowsAffected = await Connection.ExecuteAsync(@"INSERT INTO [dbo].[CustomerVehicles]([CustomerId], [VIN], [RegNo])
                                        VALUES ( @CustomerId, @VIN, @RegNo)", new
            {
                CustomerId = vehicle.CustomerId,
                VIN = vehicle.VIN,
                RegNo = vehicle.RegNo
            }, transaction: Transaction);

            if (rowsAffected > 0)
            {
                return true;
            }
            return false;
        }

        public async Task<bool> UpdateAsync(int id, Vehicle vehicle)
        {
            int rowsAffected = await Connection.ExecuteAsync(@"UPDATE [dbo].[CustomerVehicles]
                                        SET [CustomerId] = @CustomerId, [VIN] = @VIN, [RegNo] = @RegNo WHERE [Id] = @Id", new
            {
                Id = id,
                CustomerId = vehicle.CustomerId,
                VIN = vehicle.VIN,
                RegNo = vehicle.RegNo
            }, transaction: Transaction);

            if (rowsAffected > 0)
            {
                return true;
            }
            return false;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            int rowsAffected = await Connection.ExecuteAsync(@"DELETE FROM [CustomerVehicles] WHERE Id = @VehicleID", new { VehicleID = id }, transaction: Transaction);
            if (rowsAffected > 0)
            {
                return true;
            }
            return false;
        }

        public async Task<bool> PingAsync(int id)
        {
            int rowsAffected = await Connection.ExecuteAsync(@"UPDATE [CustomerVehicles] SET [LASTPINGTIME] = @Time, [IsActive] = 1 WHERE [Id] = @VehicleID", new { Time=DateTime.Now, VehicleID = id }, transaction: Transaction);
            if (rowsAffected > 0)
            {
                return true;
            }
            return false;
        }
    }
}
