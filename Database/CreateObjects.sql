USE [master]
GO

/* For security reasons the login is created disabled and with a random password. */
/****** Object:  Login [vecuser]    Script Date: 12/15/2017 9:12:18 AM ******/
CREATE LOGIN [vecuser] WITH PASSWORD=N'P@ssw0rd', DEFAULT_DATABASE=[Vehicles], DEFAULT_LANGUAGE=[us_english], CHECK_EXPIRATION=OFF, CHECK_POLICY=OFF
GO

USE [Vehicles]
GO
/****** Object:  User [vecuser]    Script Date: 12/15/2017 9:11:38 AM ******/
CREATE USER [vecuser] FOR LOGIN [vecuser] WITH DEFAULT_SCHEMA=[dbo]
GO
ALTER ROLE [db_owner] ADD MEMBER [vecuser]
GO
/****** Object:  Table [dbo].[Customers]    Script Date: 12/15/2017 9:11:38 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Customers](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](max) NOT NULL,
	[AddressLn1] [nvarchar](max) NOT NULL,
	[AddressLn2] [nvarchar](max) NULL,
	[AddressLn3] [nvarchar](max) NULL,
	[Phone] [nvarchar](20) NULL,
 CONSTRAINT [PK_Customers] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[CustomerVehicles]    Script Date: 12/15/2017 9:11:38 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CustomerVehicles](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[CustomerId] [int] NOT NULL,
	[VIN] [nvarchar](50) NOT NULL,
	[RegNo] [nvarchar](10) NOT NULL,
	[LastPingTime] [datetime] NULL,
	[IsActive] [bit] NOT NULL,
 CONSTRAINT [PK_CustomerVehicles] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
ALTER TABLE [dbo].[CustomerVehicles] ADD  CONSTRAINT [DF_CustomerVehicles_IsActive]  DEFAULT ((0)) FOR [IsActive]
GO
ALTER TABLE [dbo].[CustomerVehicles]  WITH CHECK ADD  CONSTRAINT [FK_CustomerVehicles_Customers] FOREIGN KEY([CustomerId])
REFERENCES [dbo].[Customers] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[CustomerVehicles] CHECK CONSTRAINT [FK_CustomerVehicles_Customers]
GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_CustomerVehicles_VIN] ON [dbo].[CustomerVehicles]
(
	[VIN] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO

/****** Object:  StoredProcedure [dbo].[GetCustomers]    Script Date: 12/15/2017 9:11:38 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetCustomers] (@CustomerId int =-1, @Name nvarchar(max)='')
AS
BEGIN
	SELECT [Id]
      ,[Name]
      ,[AddressLn1]
      ,[AddressLn2]
      ,[AddressLn3]
      ,[Phone]
  FROM [dbo].[Customers]
  WHERE (@CustomerId=-1 OR @CustomerId = [Id]) and
		(@Name='' OR @Name IS NULL OR [Name] like '%'+@Name+'%')
END

GO
/****** Object:  StoredProcedure [dbo].[GetCustomerVehicles]    Script Date: 12/15/2017 9:11:38 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetCustomerVehicles] (@VehicleId int = -1, @CustomerId int =-1, @VIN nvarchar(50)='', @RegNo nvarchar(10) = '', @IsActive int = -1)
AS
BEGIN
	SELECT [Id]
      ,[CustomerId]
      ,[VIN]
      ,[RegNo]
	  ,[LastPingTime]
	  ,[IsActive]
  FROM [dbo].[CustomerVehicles]
  WHERE (@VehicleId=-1 OR @VehicleId = [Id])			and
		(@CustomerId = -1 OR @CustomerId = [CustomerId])	and
		(@VIN='' OR @VIN = [VIN]) and
		(@RegNo='' OR @RegNo = [RegNo]) and 
		(@IsActive=-1 OR @IsActive = [IsActive])
END

GO


USE [msdb]
GO

/****** Object:  Job [VehiclesStatus]    Script Date: 12/15/2017 9:13:55 AM ******/
BEGIN TRANSACTION
DECLARE @ReturnCode INT
SELECT @ReturnCode = 0
/****** Object:  JobCategory [[Uncategorized (Local)]]    Script Date: 12/15/2017 9:13:55 AM ******/
IF NOT EXISTS (SELECT name FROM msdb.dbo.syscategories WHERE name=N'[Uncategorized (Local)]' AND category_class=1)
BEGIN
EXEC @ReturnCode = msdb.dbo.sp_add_category @class=N'JOB', @type=N'LOCAL', @name=N'[Uncategorized (Local)]'
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback

END

DECLARE @jobId BINARY(16)
EXEC @ReturnCode =  msdb.dbo.sp_add_job @job_name=N'VehiclesStatus', 
		@enabled=1, 
		@notify_level_eventlog=0, 
		@notify_level_email=0, 
		@notify_level_netsend=0, 
		@notify_level_page=0, 
		@delete_level=0, 
		@description=N'Update vehicles status for any vehicle to disconnected if system does not receive any ping request for more than one minute', 
		@category_name=N'[Uncategorized (Local)]', 
		@owner_login_name=N'vecuser', @job_id = @jobId OUTPUT
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
/****** Object:  Step [UpdateStatus]    Script Date: 12/15/2017 9:13:55 AM ******/
EXEC @ReturnCode = msdb.dbo.sp_add_jobstep @job_id=@jobId, @step_name=N'UpdateStatus', 
		@step_id=1, 
		@cmdexec_success_code=0, 
		@on_success_action=1, 
		@on_success_step_id=0, 
		@on_fail_action=2, 
		@on_fail_step_id=0, 
		@retry_attempts=0, 
		@retry_interval=0, 
		@os_run_priority=0, @subsystem=N'TSQL', 
		@command=N'UPDATE CustomerVehicles SET IsActive=0 WHERE LastPingTime<getdate()-''00:01:00''', 
		@database_name=N'Vehicles', 
		@flags=0
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
EXEC @ReturnCode = msdb.dbo.sp_update_job @job_id = @jobId, @start_step_id = 1
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
EXEC @ReturnCode = msdb.dbo.sp_add_jobschedule @job_id=@jobId, @name=N'Schedule', 
		@enabled=1, 
		@freq_type=4, 
		@freq_interval=1, 
		@freq_subday_type=4, 
		@freq_subday_interval=2, 
		@freq_relative_interval=0, 
		@freq_recurrence_factor=0, 
		@active_start_date=20171214, 
		@active_end_date=99991231, 
		@active_start_time=0, 
		@active_end_time=235959, 
		@schedule_uid=N'a134499c-d438-4b74-83f0-9969acc90d30'
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
EXEC @ReturnCode = msdb.dbo.sp_add_jobserver @job_id = @jobId, @server_name = N'(local)'
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
COMMIT TRANSACTION
GOTO EndSave
QuitWithRollback:
    IF (@@TRANCOUNT > 0) ROLLBACK TRANSACTION
EndSave:

GO


