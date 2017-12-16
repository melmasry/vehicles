USE Vehicles
Go
INSERT INTO [dbo].[Customers]
           ([Name]
           ,[AddressLn1]
           ,[AddressLn2]
           ,[AddressLn3]
           ,[Phone])
     VALUES
           ('Kalles Grustransporter AB','Cementvägen 8','111 11 Södertälje','Sweden','111222333')
INSERT INTO [dbo].[Customers]
           ([Name]
           ,[AddressLn1]
           ,[AddressLn2]
           ,[AddressLn3]
           ,[Phone])
     VALUES
           ('Johans Bulk AB','Balkvägen 12','222 22 Stockholm','Sweden','111222444')
INSERT INTO [dbo].[Customers]
           ([Name]
           ,[AddressLn1]
           ,[AddressLn2]
           ,[AddressLn3]
           ,[Phone])
     VALUES
           ('Haralds Värdetransporter AB','Budgetvägen 1','333 33 Uppsala','Sweden','111222555')


INSERT INTO [dbo].[CustomerVehicles]
           ([CustomerId]
           ,[VIN]
           ,[RegNo])
     VALUES
           (1
           ,'YS2R4X20005399401'
           ,'ABC123')
INSERT INTO [dbo].[CustomerVehicles]
           ([CustomerId]
           ,[VIN]
           ,[RegNo])
     VALUES
           (1
           ,'VLUR4X20009093588'
           ,'DEF456')
INSERT INTO [dbo].[CustomerVehicles]
           ([CustomerId]
           ,[VIN]
           ,[RegNo])
     VALUES
           (1
           ,'VLUR4X20009048066'
           ,'GHI789')
INSERT INTO [dbo].[CustomerVehicles]
           ([CustomerId]
           ,[VIN]
           ,[RegNo])
     VALUES
           (2
           ,'YS2R4X20005388011'
           ,'JKL012')
INSERT INTO [dbo].[CustomerVehicles]
           ([CustomerId]
           ,[VIN]
           ,[RegNo])
     VALUES
           (2
           ,'YS2R4X20005387949'
           ,'MNO345')
INSERT INTO [dbo].[CustomerVehicles]
           ([CustomerId]
           ,[VIN]
           ,[RegNo])
     VALUES
           (3
           ,'YS2R4X20005387765'
           ,'PQR678')
INSERT INTO [dbo].[CustomerVehicles]
           ([CustomerId]
           ,[VIN]
           ,[RegNo])
     VALUES
           (3
           ,'YS2R4X20005387055'
           ,'STU901')