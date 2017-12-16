using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Vehicles.Api.Tests.Helpers;
using Vehicles.Entities.DomainEntities;
using Vehicles.Entities.HelperEntities;
using Xunit;

namespace Vehicles.Api.IntegrationTest
{
    public class CustomersApiTest : BaseHttpTest
    {
        protected string baseController = "customers";
        public class GetCustomersTest : CustomersApiTest
        {
            IEnumerable<Customer> customers = new Customer[]
                            {
                                new Customer { AddressLn1 = "Cementvägen 8", AddressLn2 = "111 11 Södertälje", AddressLn3 = "Sweden", Id = 18, Name = "Kalles Grustransporter AB", Phone="111222333"},
                                new Customer { AddressLn1 = "Balkvägen 12", AddressLn2 = "222 22 Stockholm", AddressLn3 = "Sweden", Id = 19, Name = "Johans Bulk AB", Phone="111222444" },
                                new Customer { AddressLn1 = "Budgetvägen 1", AddressLn2 = "333 33 Uppsala", AddressLn3 = "Sweden", Id = 20, Name = "Haralds Värdetransporter AB", Phone="111222555" }
                            };

            protected override void ConfigureServices(IServiceCollection services)
            {
                services.AddSingleton(new Page<Customer> { Items = customers, TotalSize = customers.Count() });
            }

            [Fact(DisplayName = "[INTEGRATION TEST] [Customers API] -> Get All Customers")]
            public async Task GetCustomers()
            {
                // Arrange

                // Act
                var result = await Client.GetAsync(baseController);

                // Assert
                result.EnsureSuccessStatusCode();
                var resultCustomers = (await ParseResponse<Page<Customer>>(result)).Items;
                Assert.True(resultCustomers.Count() > 0);
            }
        }
        public class AddCustomerTest: CustomersApiTest
        {            
            [Fact(DisplayName = "[INTEGRATION TEST] [Customers API] -> Add Customer")]
            public async Task AddCustomer()
            {
                // Arrange
                Customer customer = new Customer { Name = "Test1", AddressLn1 = "Test Address", Phone = "123456789", Vehicles = new Vehicle[] { new Vehicle { RegNo = "TestRegNo1", VIN = "TestVIN1" } } };
                
                // Act
                var result = await Client.PostAsync(baseController, new StringContent(JsonConvert.SerializeObject(customer),Encoding.Unicode, "application/json"));

                // Assert
                result.EnsureSuccessStatusCode();
                var addedCustomer = await GetCustomerByName(customer.Name);
                customer.Id = addedCustomer.Id;
                Assert.True(EqualityHelper.SameCustomers(customer, addedCustomer));

                // Purge
                result = await Client.DeleteAsync(baseController + "/" + addedCustomer.Id);
                result.EnsureSuccessStatusCode();
            }

            [Fact(DisplayName = "[INTEGRATION TEST] [Customers API] -> Add Invalid Customer")]
            public async Task AddInvalidCustomer()
            {
                // Arrange

                // Act
                var result = await Client.PostAsync(baseController, new StringContent(JsonConvert.SerializeObject(string.Empty), Encoding.Unicode, "application/json"));

                // Assert
                Assert.False(result.IsSuccessStatusCode);
                Assert.True(result.StatusCode == System.Net.HttpStatusCode.BadRequest);
            }

            [Fact(DisplayName = "[INTEGRATION TEST] [Customers API] -> Add Customer With Invalid Vehicles")]
            public async Task AddCustomerWithInvalidVehicles()
            {
                // Arrange
                Customer customer = new Customer { Name = "Test2", AddressLn1 = "Test Address", Phone = "123456789", Vehicles = new Vehicle[] { new Vehicle { RegNo = "TestRegNo2", VIN = "TestVIN 2" }, new Vehicle { RegNo = "NewRegNo 2", VIN = "TestVIN 2" } } };

                // Act
                var result = await Client.PostAsync(baseController, new StringContent(JsonConvert.SerializeObject(customer), Encoding.Unicode, "application/json"));

                // Assert
                Assert.False(result.IsSuccessStatusCode);
                Assert.Null(await GetCustomerByName(customer.Name));
            }
        }

        public class UpdateCustomerTest : CustomersApiTest
        {
            [Fact(DisplayName = "[INTEGRATION TEST] [Customers API] -> Update Customer Data")]
            public async Task UpdateCustomer()
            {
                // Arrange
                Customer customer = new Customer { Name = "Test3", AddressLn1 = "Test Address 2", Phone = "111111", Vehicles = new Vehicle[] { new Vehicle { RegNo = "TestRegNo3", VIN = "TestVIN 3" } } };
                await AddCustomer(customer);

                // Act
                customer.AddressLn1 = "Test Address 3"; customer.AddressLn2 = "Test Address 4"; customer.AddressLn3 = "Test Address 5"; customer.Phone = "12121212";
                var result = await Client.PutAsync("customers/"+ customer.Id, new StringContent(JsonConvert.SerializeObject(customer), Encoding.Unicode, "application/json"));

                // Assert
                result.EnsureSuccessStatusCode();
                var updatedCustomer = await GetCustomerByName(customer.Name);
                Assert.True(EqualityHelper.SameCustomers(customer, updatedCustomer));

                // Purge
                result = await Client.DeleteAsync(baseController + "/" + updatedCustomer.Id);
                result.EnsureSuccessStatusCode();
            }

            [Fact(DisplayName = "[INTEGRATION TEST] [Customers API] -> Update Not Existing Customer")]
            public async Task UpdateNotExistingCustomer()
            {
                // Arrange
                Customer customer = new Customer { Name = "Test33", AddressLn1 = "Test Address 2", Phone = "111111", Vehicles = new Vehicle[] { new Vehicle { RegNo = "TestRegNo9", VIN = "TestVIN 99" } } };

                // Act
                customer.AddressLn1 = "Test Address 3"; customer.AddressLn2 = "Test Address 4"; customer.AddressLn3 = "Test Address 5"; customer.Phone = "12121212";
                var result = await Client.PutAsync("customers/" + customer.Id, new StringContent(JsonConvert.SerializeObject(customer), Encoding.Unicode, "application/json"));

                // Assert
                Assert.False(result.IsSuccessStatusCode);
                Assert.True(result.StatusCode == System.Net.HttpStatusCode.NotFound);
            }
        }

        public class DeleteCustomerTest : CustomersApiTest
        {
            [Fact(DisplayName = "[INTEGRATION TEST] [Customers API] -> Delete Customer")]
            public async Task DeleteCustomer()
            {
                // Arrange
                Customer customer = new Customer { Name = "Test9", AddressLn1 = "Test Address 2", Phone = "111111", Vehicles = new Vehicle[] { new Vehicle { RegNo = "TestRegNo4", VIN = "TestVIN9" } } };
                await AddCustomer(customer);

                // Act
                var result = await Client.DeleteAsync(baseController+"/" + customer.Id);

                // Assert
                result.EnsureSuccessStatusCode();
                customer = await GetCustomerByName(customer.Name);
                Assert.Null(customer);
            }

            [Fact(DisplayName = "[INTEGRATION TEST] [Customers API] -> Delete Not Existing Customer")]
            public async Task DeleteNotExistingCustomer()
            {
                // Arrange
                Customer customer = new Customer { Id=80, Name = "Test4", AddressLn1 = "Test Address 2", Phone = "111111", Vehicles = new Vehicle[] { new Vehicle { RegNo = "TestRegNo4", VIN = "TestVIN3" } } };

                // Act
                var result = await Client.DeleteAsync(baseController + "/" + customer.Id);

                // Assert
                Assert.False(result.IsSuccessStatusCode);
                Assert.True(result.StatusCode == System.Net.HttpStatusCode.NotFound);
            }
        }
    }
}
