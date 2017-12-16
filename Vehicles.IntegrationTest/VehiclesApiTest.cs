using Newtonsoft.Json;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Vehicles.Entities.DomainEntities;
using Vehicles.Entities.HelperEntities;
using Xunit;

namespace Vehicles.Api.IntegrationTest
{
    public class VehiclesApiTest : BaseHttpTest
    {
        public string baseController = "vehicles";
        public class GetVehiclesTest : VehiclesApiTest
        {
            [Fact(DisplayName = "[INTEGRATION TEST] [Vehicles API] -> Get All Vehicles")]
            public async Task GetVehicles()
            {
                // Arrange

                // Act
                var result = await Client.GetAsync("vehicles");

                // Assert
                result.EnsureSuccessStatusCode();
                var responseBody = await result.Content.ReadAsStringAsync();
                var resultVehicles = JsonConvert.DeserializeObject<Page<Vehicle>>(responseBody).Items;
                Assert.True(resultVehicles.Count()>0);
            }
        }

        public class PingVehicleTest : VehiclesApiTest
        {
            [Fact(DisplayName = "[INTEGRATION TEST] [Vehicles API] -> Ping Vehicle")]
            public async Task PingVehicle()
            {
                // Arrange
                Customer customer = new Customer { Name = "Test40", AddressLn1 = "Test Address 2", Phone = "111111", Vehicles = new Vehicle[] { new Vehicle { RegNo = "TestRegNo4", VIN = "VIN40" } } };
                await AddCustomer(customer);
                var vehicle = await GetVehicleByVIN("VIN40");

                // Act
                var result = await Client.PostAsync(baseController + "/ping/" + vehicle.Id, new StringContent(string.Empty, Encoding.Unicode, "application/json"));

                // Assert
                result.EnsureSuccessStatusCode();
                var updatedVehicle = await GetVehicleByVIN(vehicle.VIN);
                Assert.True(updatedVehicle.IsActive);

                // Purge
                result = await Client.DeleteAsync(customersBaseController + "/" + customer.Id);
                result.EnsureSuccessStatusCode();
            }

            [Fact(DisplayName = "[INTEGRATION TEST] [Vehicles API] -> Ping Not Existing Vehicle")]
            public async Task PingNotExistingVehicle()
            {
                // Arrange
                Vehicle vehicle = new Vehicle { Id = 80, RegNo = "TestRegNo6", VIN = "TestVIN 6" };

                // Act
                var result = await Client.PostAsync(baseController + "/ping/" + vehicle.Id, new StringContent(string.Empty, Encoding.Unicode, "application/json"));

                // Assert
                Assert.False(result.IsSuccessStatusCode);
            }
        }
    }
}
