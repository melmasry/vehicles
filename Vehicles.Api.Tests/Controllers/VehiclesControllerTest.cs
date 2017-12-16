using Microsoft.Extensions.Options;
using Moq;
using Vehicles.Api.Controllers;
using Vehicles.Entities.DomainEntities;
using Vehicles.Entities.Enums;
using Vehicles.Entities.HelperEntities;
using Vehicles.Services;
using Xunit;

namespace Vehicles.Api.Tests.Controllers
{
    public class VehiclesControllerTest
    {
        protected VehiclesController ControllerUnderTest { get; }
        protected Mock<IVehiclesService> VehiclesServiceMock { get; }
        protected PagingOptions pagingOptions { get; }

        public VehiclesControllerTest()
        {
            VehiclesServiceMock = new Mock<IVehiclesService>();
            var pagingOptionsMock = new Mock<IOptions<PagingOptions>>();
            ControllerUnderTest = new VehiclesController(VehiclesServiceMock.Object, pagingOptionsMock.Object);
            pagingOptions = new PagingOptions { Offset = 0, Limit = 5 };
        }


        public class Get : VehiclesControllerTest
        {
            private Vehicle[] vehicles;
            public Get()
            {
                vehicles = new Vehicle[]
                            {
                                new Vehicle{Id = 1, CustomerId = 1, VIN="111", RegNo = "1111111", IsActive = true},
                                new Vehicle{Id = 2, CustomerId = 1, VIN="222", RegNo = "2222222", IsActive = false},
                                new Vehicle{Id = 3, CustomerId = 2, VIN="333", RegNo = "3333333", IsActive = false},
                                new Vehicle{Id = 4, CustomerId = 2, VIN="444", RegNo = "4444444", IsActive = true},
                                new Vehicle{Id = 5, CustomerId = 3, VIN="555", RegNo = "5555555", IsActive = true},
                                new Vehicle{Id = 6, CustomerId = 4, VIN="666", RegNo = "6666666", IsActive = false}
                            };
            }

            [Fact(DisplayName = "[Vehicles.Controller] -> Get All Vehicles")]
            public async void GetVehicles()
            {
                var pagingOptions = new PagingOptions { Offset = 1, Limit = 1 };
                // Arrange
                var expectedVehicles = new Page<Vehicle>
                {
                    Items = new Vehicle[] { new Vehicle { Id = 2, CustomerId = 1, VIN = "222", RegNo = "2222222", IsActive = false } },
                    TotalSize = 1
                };
                VehiclesServiceMock
                    .Setup(x => x.GetAllAsync(pagingOptions))
                    .ReturnsAsync(expectedVehicles);

                // Act
                var result = await ControllerUnderTest.Get(pagingOptions);

                // Assert
                Assert.Same(expectedVehicles, result);
            }

            [Fact(DisplayName = "[Vehicles.Controller] -> Get Vehicle By Existing Id")]
            public async void GetVehiclesById()
            {
                // Arrange
                var expectedVehicle = new Vehicle { Id = 5, CustomerId = 3, VIN = "555", RegNo = "5555555", IsActive = true };
                VehiclesServiceMock
                    .Setup(x => x.GetAsync(5))
                    .ReturnsAsync(expectedVehicle);

                // Act
                var result = await ControllerUnderTest.Get(5);

                // Assert
                Assert.Same(expectedVehicle, result);
            }

            [Fact(DisplayName = "[Vehicles.Controller] -> Get Vehicle By Invalid Id")]
            public async void GetVehiclesByInvalidId()
            {
                Vehicle expectedVehicle = null;
                // Arrange
                VehiclesServiceMock
                    .Setup(x => x.GetAsync(80))
                    .ReturnsAsync(expectedVehicle);

                // Act
                var result = await ControllerUnderTest.Get(80);

                // Assert
                Assert.Same(expectedVehicle, result);
            }

            [Fact(DisplayName = "[Vehicles.Controller] -> Get Vehicle By Vehicle Id")]
            public async void GetVehiclesByVehicleId()
            {
                // Arrange
                var expectedVehicle = new Vehicle { Id = 4, CustomerId = 2, VIN = "444", RegNo = "4444444", IsActive = true };
                VehiclesServiceMock
                    .Setup(x => x.GetByVehicleId("4"))
                    .ReturnsAsync(expectedVehicle);

                // Act
                var result = await ControllerUnderTest.GetByVehicleId("4");

                // Assert
                Assert.Same(expectedVehicle, result);
            }

            [Fact(DisplayName = "[Vehicles.Controller] -> Get Vehicle By Existing RegNo")]
            public async void GetVehiclesByRegNo()
            {
                // Arrange
                var expectedVehicle = new Vehicle { Id = 3, CustomerId = 2, VIN = "333", RegNo = "3333333", IsActive = false };
                VehiclesServiceMock
                    .Setup(x => x.GetByRegNo("3333333"))
                    .ReturnsAsync(expectedVehicle);

                // Act
                var result = await ControllerUnderTest.GetByRegNo("3333333");

                // Assert
                Assert.Same(expectedVehicle, result);
            }

            [Fact(DisplayName = "[Vehicles.Controller] -> Get All Vehicles Related To Existing Customer By Id")]
            public async void GetVehiclesByCustomerId()
            {
                // Arrange
                var expectedVehicles = new Page<Vehicle>
                {
                    Items = new Vehicle[] {new Vehicle { Id = 1, CustomerId = 1, VIN = "111", RegNo = "1111111", IsActive = true },
                            new Vehicle { Id = 2, CustomerId = 1, VIN = "222", RegNo = "2222222", IsActive = false } },
                    TotalSize = 2
                };
                VehiclesServiceMock
                    .Setup(x => x.GetByCustomerId(1, SearchVehicleStatus.Any, pagingOptions))
                    .ReturnsAsync(expectedVehicles);

                // Act
                var result = await ControllerUnderTest.GetByCustomerId(1, SearchVehicleStatus.Any, pagingOptions);

                // Assert
                Assert.Same(expectedVehicles, result);
            }

            [Fact(DisplayName = "[Vehicles.Controller] -> Get Empty Vehicles Related To Invalid Customer By Id")]
            public async void GetVehiclesByInvalidCustomerId()
            {
                // Arrange
                var expectedVehicles = new Page<Vehicle>
                {
                    Items = new Vehicle[] { },
                    TotalSize = 2
                };
                VehiclesServiceMock
                    .Setup(x => x.GetByCustomerId(80, SearchVehicleStatus.Any, pagingOptions))
                    .ReturnsAsync(expectedVehicles);

                // Act
                var result = await ControllerUnderTest.GetByCustomerId(80, SearchVehicleStatus.Any, pagingOptions);

                // Assert
                Assert.Same(expectedVehicles, result);
            }

            [Fact(DisplayName = "[Vehicles.Controller] -> Get Vehicle By Status")]
            public async void GetVehiclesByStatus()
            {
                // Arrange
                var expectedVehicles = new Page<Vehicle>
                {
                    Items = new Vehicle[] {
                                new Vehicle{Id = 1, CustomerId = 1, VIN="111", RegNo = "1111111", IsActive = true},
                                new Vehicle{Id = 4, CustomerId = 2, VIN="444", RegNo = "4444444", IsActive = true},
                                new Vehicle{Id = 5, CustomerId = 3, VIN="555", RegNo = "5555555", IsActive = true}
                            },
                    TotalSize = 3
                };
                VehiclesServiceMock
                    .Setup(x => x.GetByStatus(SearchVehicleStatus.Active, pagingOptions))
                    .ReturnsAsync(expectedVehicles);

                // Act
                var result = await ControllerUnderTest.GetByStatus(SearchVehicleStatus.Active, pagingOptions);

                // Assert
                Assert.Same(expectedVehicles, result);
            }
        }
    }
}
