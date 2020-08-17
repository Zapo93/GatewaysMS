USE ms_gateways;
GO

--//
EXEC dbo.add_gateway '123456789', 'Home', '192.168.22.10';
GO

DECLARE @DeviceCreated datetime, @DeviceUniqueId int;
SET @DeviceCreated = GETDATE();
EXEC dbo.add_device '123456789', 'SAMSONG', @DeviceCreated, 'Online', @DeviceUniqueId OUTPUT;
GO

DECLARE @DeviceCreated datetime, @DeviceUniqueId int;
SET @DeviceCreated = GETDATE();
EXEC dbo.add_device '123456789', 'SUNY', @DeviceCreated, 'Offline', @DeviceUniqueId OUTPUT;
GO

DECLARE @DeviceCreated datetime, @DeviceUniqueId int;
SET @DeviceCreated = GETDATE();
EXEC dbo.add_device '123456789', 'LC', @DeviceCreated, 'Online', @DeviceUniqueId OUTPUT;
GO
--//
EXEC dbo.add_gateway 'fasd1234231', 'Office', '192.90.50.11';
GO

DECLARE @DeviceCreated datetime, @DeviceUniqueId int;
SET @DeviceCreated = GETDATE();
EXEC dbo.add_device 'fasd1234231', 'DILL', @DeviceCreated, 'Online', @DeviceUniqueId OUTPUT;
GO

DECLARE @DeviceCreated datetime, @DeviceUniqueId int;
SET @DeviceCreated = GETDATE();
EXEC dbo.add_device 'fasd1234231', 'Embassador', @DeviceCreated, 'Offline', @DeviceUniqueId OUTPUT;
GO

--//
EXEC dbo.add_gateway 'udsadppasd', 'Villa', '52.100.43.19';
GO

DECLARE @DeviceCreated datetime, @DeviceUniqueId int;
SET @DeviceCreated = GETDATE();
EXEC dbo.add_device 'udsadppasd', 'Katrin', @DeviceCreated, 'Online', @DeviceUniqueId OUTPUT;
GO

SELECT * FROM dbo.device RIGHT JOIN dbo.gateway ON device.gateway_id = gateway.id LEFT JOIN dbo.device_status ON device_status.device_id = device.id;