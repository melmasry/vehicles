using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Vehicles.Entities.DomainEntities;
using Vehicles.Entities.HelperEntities;

namespace Vehicles.Api.IntegrationTest
{
    public abstract class BaseHttpTest : IDisposable
    {
        protected TestServer Server { get; }
        protected HttpClient Client { get; }

        protected virtual Uri BaseAddress => new Uri("http://localhost/api/");
        protected virtual string Environment => "Development";

        protected string customersBaseController = "customers";
        protected string vehiclesBaseController = "vehicles";

        public BaseHttpTest()
        {
            var builder = new WebHostBuilder()
                .UseStartup<Startup>()
                .UseEnvironment(Environment)
                .ConfigureServices(ConfigureServices);

            Server = new TestServer(builder);
            Client = Server.CreateClient();
            Client.BaseAddress = BaseAddress;
        }

        public async Task<T> ParseResponse<T>(HttpResponseMessage result)
        {
            var responseBody = await result.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(responseBody);
        }

        public async Task<Customer> GetCustomerByName(string name)
        {
            var result = await Client.GetAsync(customersBaseController + "/name?name=" + name);

            result.EnsureSuccessStatusCode();
            return (await ParseResponse<Page<Customer>>(result)).Items.FirstOrDefault();
        }

        public async Task AddCustomer(Customer customer)
        {
            var result = await Client.PostAsync(customersBaseController, new StringContent(JsonConvert.SerializeObject(customer), Encoding.Unicode, "application/json"));
            var resultCustomer = await GetCustomerByName(customer.Name);
            customer.Id = resultCustomer.Id;
        }

        public async Task<Vehicle> GetVehicleByVIN(string VIN)
        {
            var result = await Client.GetAsync(vehiclesBaseController + "/vin/" + VIN);

            result.EnsureSuccessStatusCode();
            return (await ParseResponse<Vehicle>(result));
        }

        public async Task AddVehicle(Vehicle vehicle)
        {
            var result = await Client.PostAsync(vehiclesBaseController, new StringContent(JsonConvert.SerializeObject(vehicle), Encoding.Unicode, "application/json"));
            var resultVehicle = await GetVehicleByVIN(vehicle.VIN);
            vehicle.Id = resultVehicle.Id;
        }

        protected virtual void ConfigureServices(IServiceCollection services)
        {
        }

        #region IDisposable Support

        private bool disposedValue = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    Client.Dispose();
                    Server.Dispose();
                }
                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }

        #endregion
    }
}
