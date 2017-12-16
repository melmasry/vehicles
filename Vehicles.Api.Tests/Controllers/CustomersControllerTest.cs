using Microsoft.Extensions.Options;
using Moq;
using Vehicles.Api.Controllers;
using Vehicles.Entities.DomainEntities;
using Vehicles.Entities.Enums;
using Vehicles.Entities.HelperEntities;
using Xunit;

namespace Vehicles.Services.Tests
{
    public class CustomersControllerTest
    {
        protected CustomersController ControllerUnderTest { get; }
        protected Mock<ICustomersService> CustomersServiceMock { get; }
        protected PagingOptions pagingOptions { get; }

        public CustomersControllerTest()
        {
            CustomersServiceMock = new Mock<ICustomersService>();
            var pagingOptionsMock = new Mock<IOptions<PagingOptions>>();
            ControllerUnderTest = new CustomersController(CustomersServiceMock.Object, pagingOptionsMock.Object);
            pagingOptions = new PagingOptions { Offset = 0, Limit = 5 };
        }


        public class Get : CustomersControllerTest
        {
            private Customer[] customers;
            public Get()
            {
                customers = new Customer[]
                            {
                                new Customer { Name = "Customer 1", AddressLn1 = "Address 1" },
                                new Customer { Name = "Customer 2", AddressLn1 = "Address 2" },
                                new Customer { Name = "Customer 3", AddressLn1 = "Address 3" },
                                new Customer { Name = "Customer 4", AddressLn1 = "Address 4" },
                                new Customer { Name = "Customer 5", AddressLn1 = "Address 5" },
                                new Customer { Name = "Customer 6", AddressLn1 = "Address 6" }
                            };
            }
            /// <summary>
            /// Get All customers
            /// </summary>
            [Fact(DisplayName = "[Customers.Controller] -> Get All Customers")]
            public async void GetAllCustomers()
            {
                // Arrange
                var expectedCustomers = new Page<Customer>
                {
                    Items = customers,
                    TotalSize = 6
                };
                CustomersServiceMock
                    .Setup(x => x.GetAllAsync(pagingOptions, true, SearchVehicleStatus.Any))
                    .ReturnsAsync(expectedCustomers);

                // Act
                var result = await ControllerUnderTest.Get(pagingOptions);

                // Assert
                Assert.Same(expectedCustomers, result);
            }
            
            [Fact(DisplayName = "[Customers.Controller] -> Get All Customers By Name")]
            public async void GetCustomersByName()
            {
                // Arrange
                var expectedCustomers = new Page<Customer>
                {
                    Items = new Customer[]
                            {
                                new Customer { Name = "GetCustomersByName Customer", AddressLn1 = "Address 1" }
                            },
                    TotalSize = 1
                };
                CustomersServiceMock
                    .Setup(x => x.GetAsyncByName("GetCustomersByName", pagingOptions, true, SearchVehicleStatus.Any))
                    .ReturnsAsync(expectedCustomers);

                // Act
                var result = await ControllerUnderTest.Get("GetCustomersByName", pagingOptions);

                // Assert
                Assert.Same(expectedCustomers, result);
            }

            [Fact(DisplayName = "[Customers.Controller] -> Get Customers By Not Existing Name")]
            public async void GetCustomersByNotExistingName()
            {
                // Arrange
                var expectedCustomers = new Page<Customer>
                {
                    Items = new Customer[] { },
                    TotalSize = 1
                };
                CustomersServiceMock
                    .Setup(x => x.GetAsyncByName("N/A", pagingOptions, true, SearchVehicleStatus.Any))
                    .ReturnsAsync(expectedCustomers);

                // Act
                var result = await ControllerUnderTest.Get("N/A", pagingOptions);

                // Assert
                Assert.Same(expectedCustomers, result);
            }

            [Fact(DisplayName = "[Customers.Controller] -> Get Customers with paging")]
            public async void GetCustomersPaging()
            {
                // Arrange
                var expectedCustomers = new Page<Customer>
                {
                    Items = new Customer[]
                            {
                                new Customer { Name = "Customer 1", AddressLn1 = "Address 1" },
                                new Customer { Name = "Customer 2", AddressLn1 = "Address 2" },
                                new Customer { Name = "Customer 3", AddressLn1 = "Address 3" },
                                new Customer { Name = "Customer 4", AddressLn1 = "Address 4" },
                                new Customer { Name = "Customer 5", AddressLn1 = "Address 5" }
                            },
                    TotalSize = 5
                };
                CustomersServiceMock
                    .Setup(x => x.GetAsyncByName(string.Empty, pagingOptions, true, SearchVehicleStatus.Any))
                    .ReturnsAsync(expectedCustomers);

                // Act
                var result = await ControllerUnderTest.Get(string.Empty, pagingOptions);

                // Assert
                Assert.Same(expectedCustomers, result);
            }
        }
    }
}
