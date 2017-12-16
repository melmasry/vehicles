Welcome to Vehicles project to handle customers with connected vehicles
Web APIs help user to setup environment of customers and each customer has number of connected vehicles. The apis are used to do all required CRUD operations for two main entities of the system Cutomer and Vehicle taking into consideration all refrential integrity and validation rules. Swagger UI are used to expose all these apis. System includes a web interface to display all customers with vehicles and filter by customer and vehicle's connection status.
Ping is one of the apis that will be exposed by the vehicle once connected and call this method every one minute, this api stores last ping time on the database for this vehicle and on the database side there is a job running every minute to check if there is any vehicle system does not receive any ping pulse from time more than one minute then it will update vehicle's status into InActive until new pulse comes.

Technologies used in the project are as following:
- .NET Core 1.1 WebAPI with Visual Studio 2017
- ASP.NET Core 1.1 interface to monitor vehicles status
- xUnit and Moq for unit testing and integration testing
- SQL Server 2016 for saving customers/vehicles data into database
- Dapper as very fast ORM
- Swagger for view/expose Web APIs

Architectures and Patterns
- Full architecture with responsibility separation concerns, SOLID and Clean Code
- Microservices architecure
- .NET Core Native DI
- Repository Pattern
- Unit of Work pattern

![alt text](https://github.com/melmasry/vehicles/blob/master/Design.jpg)

How To Run:
- Install Database
	* Make sure that SQL Server Agent is up and running (system is adding a job to update vehicle status according to received ping requests)
	* Inside \Database folder, run CreateDatabase.bat
	* Check [Output.txt] file created beside batch file
- Start WebAPI
	* Run project \Vehicles.Api
- Start Simulator
	* Run console project \Simulator\Vehicles.Simulator and enter URL of WebAPI [http://localhost:9711/api]
- Start Monitoring Web Site
	* Run project \UI\Vehicles.Web
	* Monitor status using URL (http://localhost:16729/VehiclesStatus/ViewReport)
- (optional) Add/Update/Delete Customers/Vehicles using Swagger.UI
	* http://localhost:9711/swagger/


Future Work:
- Use Azure or AWS for hosting
- Replace SQL Server database with NoSQL option like MongoDB or Azure Table Storage
- Replace SQL Job with Azure WebJob
- Convert Ping service that is used to keep vehicles alive into a serverless function since it will be heavily called by large amount of vehicles
- Use CI 
- Handle authentication and authorization on the system
- Apply token based technique in communication between service and vehicle using JWT
- Create better user interface for updating data, monitoring status and displaying reports