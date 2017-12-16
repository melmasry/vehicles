using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using Vehicles.Api.Tests.Helpers;
using Vehicles.Entities.DomainEntities;
using Vehicles.Entities.Enums;
using Vehicles.Entities.HelperEntities;
using Vehicles.Helpers.UnitOfWork;
using Vehicles.Repositories.DomainRepositories;
using Xunit;

namespace Vehicles.Services.Tests
{
    public class VehiclesServiceTest
    {
        protected VehiclesService ServiceUnderTest { get; }
        protected Mock<ICustomersRepository> CustomersRepositoryMock { get; }
        protected Mock<IVehiclesRepository> VehiclesRepositoryMock { get; }
        protected Mock<IUnitOfWorkFactory> UnitOfWorkFactoryMock { get; }
        protected Mock<IUnitOfWork> UnitOfWorkMock { get; }
        protected PagingOptions pagingOptions { get; }

        public VehiclesServiceTest()
        {
            CustomersRepositoryMock = new Mock<ICustomersRepository>();
            VehiclesRepositoryMock = new Mock<IVehiclesRepository>();
            UnitOfWorkFactoryMock = new Mock<IUnitOfWorkFactory>();
            UnitOfWorkMock = new Mock<IUnitOfWork>();

            ServiceUnderTest = new VehiclesService(VehiclesRepositoryMock.Object);

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

        public class Get : VehiclesServiceTest
        {
            private Vehicle[] vehicles;
            public Get()
            {
                vehicles = new Vehicle[] { new Vehicle { VIN = "444", RegNo = "4444", IsActive = true },
                                                                                                        new Vehicle { VIN = "555", RegNo = "5555", IsActive = false },
                                                                                                        new Vehicle { VIN = "666", RegNo = "6666", IsActive = true }
                            };
            }

            [Fact(DisplayName = "[Vehicles.Service] -> Get All Vehicles")]
            public async void GetAllAsync()
            {
                // Arrange
                var expectedVehicles = new Page<Vehicle>
                {
                    Items = new Vehicle[] { vehicles[0], vehicles[1], vehicles[2] },
                    TotalSize = 3
                };

                VehiclesRepositoryMock
                        .Setup(x => x.GetAllAsync())
                        .ReturnsAsync(vehicles);

                // Act
                var result = await ServiceUnderTest.GetAllAsync(pagingOptions);

                // Assert
                Assert.True(EqualityHelper.SamePageOfVehicles(expectedVehicles, result));
            }
        }
    }
}
