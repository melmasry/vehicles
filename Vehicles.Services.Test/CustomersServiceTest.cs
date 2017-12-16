using Moq;
using System.Linq;
using Vehicles.Api.Tests.Helpers;
using Vehicles.Entities.DomainEntities;
using Vehicles.Entities.Enums;
using Vehicles.Entities.HelperEntities;
using Vehicles.Helpers.UnitOfWork;
using Vehicles.Repositories.DomainRepositories;
using Xunit;

namespace Vehicles.Services.Tests
{
    public class CustomersServiceTest
    {
        protected CustomersService ServiceUnderTest { get; }
        protected Mock<ICustomersRepository> CustomersRepositoryMock { get; }
        protected Mock<IVehiclesRepository> VehiclesRepositoryMock { get; }
        protected Mock<IUnitOfWorkFactory> UnitOfWorkFactoryMock { get; }
        protected Mock<IUnitOfWork> UnitOfWorkMock { get; }
        protected PagingOptions pagingOptions { get; }

        public CustomersServiceTest()
        {
            CustomersRepositoryMock = new Mock<ICustomersRepository>();
            VehiclesRepositoryMock = new Mock<IVehiclesRepository>();
            UnitOfWorkFactoryMock = new Mock<IUnitOfWorkFactory>();
            UnitOfWorkMock = new Mock<IUnitOfWork>();

            ServiceUnderTest = new CustomersService(CustomersRepositoryMock.Object, UnitOfWorkFactoryMock.Object);

            UnitOfWorkFactoryMock
                    .Setup(x => x.GetInstance())
                    .Returns(UnitOfWorkMock.Object);

            UnitOfWorkMock
                .Setup(x => x.CustomersRepository)
                .Returns(CustomersRepositoryMock.Object);

            UnitOfWorkMock
                .Setup(x => x.VehiclesRepository)
                .Returns(VehiclesRepositoryMock.Object);

            pagingOptions = new PagingOptions { Offset = 0, Limit = 5 };
        }

        public class Get : CustomersServiceTest
        {
            private Customer[] customers;
            public Get()
            {
                customers = new Customer[]
                            {
                                new Customer { Id=1, Name = "Customer 1", AddressLn1 = "Address 1" },
                                new Customer { Id=2, Name = "Customer 2", AddressLn1 = "Address 2", Vehicles = new Vehicle[] { new Vehicle { VIN = "222", RegNo = "2222", IsActive = false } } },
                                new Customer { Id=3, Name = "Customer 3", AddressLn1 = "Address 3", Vehicles = new Vehicle[] { new Vehicle { VIN = "333", RegNo = "3333", IsActive = true } } },
                                new Customer { Id=4, Name = "Customer 4", AddressLn1 = "Address 4" },
                                new Customer { Id=5, Name = "Customer 5", AddressLn1 = "Address 5" },
                                new Customer { Id=6, Name = "Customer 6", AddressLn1 = "Address 6", Vehicles = new Vehicle[] { new Vehicle { VIN = "444", RegNo = "4444", IsActive = true },
                                                                                                        new Vehicle { VIN = "555", RegNo = "5555", IsActive = false },
                                                                                                        new Vehicle { VIN = "666", RegNo = "6666", IsActive = true }} }
                            };
            }

            [Fact(DisplayName = "[Customers.Service] -> Get All Customers")]
            public async void GetAllAsync()
            {
                // Arrange
                var expectedCustomers = new Page<Customer>
                {
                    Items = new Customer[] { customers[0],customers[1],customers[2],customers[3],customers[4]},
                    TotalSize = 5
                };
                
                CustomersRepositoryMock
                    .Setup(x => x.GetAsyncByName(string.Empty))
                    .ReturnsAsync(expectedCustomers.Items);

                foreach(Customer customer in customers)
                {
                    VehiclesRepositoryMock
                        .Setup(x => x.GetByCustomerId(customer.Id, SearchVehicleStatus.Any))
                        .ReturnsAsync(customer.Vehicles);
                }
                // Act
                var result = await ServiceUnderTest.GetAllAsync(pagingOptions);

                // Assert
                Assert.True(EqualityHelper.SamePageOfCustomers(expectedCustomers, result));
            }

            [Fact(DisplayName = "[Customers.Service] -> Get Customer By Id")]
            public async void GetById()
            {
                // Arrange
                var expectedCustomer = customers[2];

                CustomersRepositoryMock
                    .Setup(x => x.GetAsync(3))
                    .ReturnsAsync(expectedCustomer);

                // Act
                var result = await ServiceUnderTest.GetAsync(3);

                // Assert
                Assert.True(EqualityHelper.SameCustomers(expectedCustomer, result));
            }

            [Fact(DisplayName = "[Customers.Service] -> Get Customer By Invalid Id")]
            public async void GetByInvalidId()
            {
                // Arrange
                Customer expectedCustomer = null;

                CustomersRepositoryMock
                    .Setup(x => x.GetAsync(80))
                    .ReturnsAsync(expectedCustomer);

                // Act
                var result = await ServiceUnderTest.GetAsync(80);

                // Assert
                Assert.Null(result);
            }

            [Fact(DisplayName = "[Customers.Service] -> Get Customers By Name")]
            public async void GetAsyncByName()
            {
                // Arrange
                var expectedCustomers = new Page<Customer>
                {
                    Items = new Customer[] { customers[5] },
                    TotalSize = 1
                };

                CustomersRepositoryMock
                    .Setup(x => x.GetAsyncByName("6"))
                    .ReturnsAsync(expectedCustomers.Items);

                foreach (Customer customer in customers)
                {
                    VehiclesRepositoryMock
                        .Setup(x => x.GetByCustomerId(customer.Id, SearchVehicleStatus.Any))
                        .ReturnsAsync(customer.Vehicles);
                }
                // Act
                var result = await ServiceUnderTest.GetAsyncByName("6", pagingOptions);

                // Assert
                Assert.True(EqualityHelper.SamePageOfCustomers(expectedCustomers, result));
            }

            [Fact(DisplayName = "[Customers.Service] -> Get Customers By Not Existing Name")]
            public async void GetAsyncByInvalidName()
            {
                // Arrange
                var expectedCustomers = new Page<Customer>
                {
                    Items = new Customer[] { },
                    TotalSize = 0
                };

                CustomersRepositoryMock
                    .Setup(x => x.GetAsyncByName("Invalid Name"))
                    .ReturnsAsync(expectedCustomers.Items);
                
                // Act
                var result = await ServiceUnderTest.GetAsyncByName("Invalid Name", pagingOptions);

                // Assert
                Assert.True(EqualityHelper.SamePageOfCustomers(expectedCustomers, result));
            }

            [Fact(DisplayName = "[Customers.Service] -> Get Customers' Vehicles By Status")]
            public async void GetByStatus()
            {
                // Arrange
                var expectedCustomers = new Page<Customer>
                {
                    Items = new Customer[] { 
                                new Customer { Id=3, Name = "Customer 3", AddressLn1 = "Address 3", Vehicles = new Vehicle[] { new Vehicle { VIN = "333", RegNo = "3333", IsActive = true } } },
                                new Customer { Id=6, Name = "Customer 6", AddressLn1 = "Address 6", Vehicles = new Vehicle[] { new Vehicle { VIN = "444", RegNo = "4444", IsActive = true },
                                                                                                        new Vehicle { VIN = "666", RegNo = "6666", IsActive = true } } } },
                    TotalSize = 2
                };

                CustomersRepositoryMock
                    .Setup(x => x.GetAsyncByName(string.Empty))
                    .ReturnsAsync(customers);

                foreach (Customer customer in customers)
                {
                    VehiclesRepositoryMock
                        .Setup(x => x.GetByCustomerId(customer.Id, SearchVehicleStatus.Active))
                        .ReturnsAsync(customer.Vehicles==null? null : customer.Vehicles.Where(v=> v.IsActive).ToArray());
                }
                // Act
                var result = await ServiceUnderTest.GetAsyncByName(string.Empty, pagingOptions, true, SearchVehicleStatus.Active);

                // Assert
                Assert.True(EqualityHelper.SamePageOfCustomers(expectedCustomers, result));
            }
        }
    }
}
