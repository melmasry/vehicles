Welcome to Vehicles project to handle customers with connected vehicles

Technologies used in the project are as following:
- .NET Core 1.1 WebAPI with Visual Studio 2017
- ASP.NET Core 1.1 interface to monitor vehicles status
- xUnit and Moq for unit testing and integration testing
- SQL Server 2016 for saving customers/vehicles data into database
- Dapper as fast ORM
- Swagger for view/expose Web APIs

Architectures and Patterns
- Full architecture with responsibility separation concerns, SOLID and Clean Code
- Microservices architecure
- .NET Core Native DI
- Repository Pattern
- Unit of Work pattern

How To Run:
- Install Database
	* Make sure that SQL Server Agent is up and running (system is adding a job to update vehicle status according to received ping requests)
	* Inside [Database] folder, run CreateDatabase.bat
	* Check [Output.txt] file created beside batch file
- Start WebAPI
	* Run project [Vehicles.Api]
- Start Simulator
	* Run console project [Simulator\Vehicles.Simulator] and enter URL of WebAPI [http://localhost:9711/api]
- Start Monitoring Web Site
	* Run project [UI\Vehicles.Web]
	* Monitor status using URL (http://localhost:16729/VehiclesStatus/ViewReport)
- (optional) Add/Update/Delete Customers/Vehicles using Swagger.UI
	* http://localhost:9711/swagger/

Future Work:
- Use Azure or AWS for hosting
- Replace SQL Server database with NoSQL option like MongoDB or Azure Table Storage
- Convert Ping service into serverless function to can handle enormous amount of calls from vehicles
- Handle authentication and authorization on the system
- Apply token based technique in communication between service and vehicle using JWT
- Create better user interface for updating data, monitoring status and displaying reports
- Use CI 