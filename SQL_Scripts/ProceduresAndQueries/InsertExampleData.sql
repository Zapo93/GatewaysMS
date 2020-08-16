USE ms_gateways;
GO

INSERT INTO dbo.gateway (serial_number, name, ipv4) VALUES ('12324289723', 'Goshko', '123.123.32.12');
INSERT INTO dbo.gateway (serial_number, name, ipv4) VALUES ('fasdf9723', 'Peshko', '123.103.32.12');
INSERT INTO dbo.gateway (serial_number, name, ipv4) VALUES ('1232lskdasf', 'Borko', '123.22.32.12');
INSERT INTO dbo.gateway (serial_number, name, ipv4) VALUES ('126454359723', 'Niki', '90.123.54.12');

INSERT INTO dbo.device (id, vendor, date_created, gateway_id) VALUES(1, 'SONY', GETDATE(), 10); --DATETIMEFROMPARTS(1,2,3,4,5,6,7)
INSERT INTO dbo.device (id, vendor, date_created, gateway_id) VALUES(2, 'PHILIPS', GETDATE(), 10);
INSERT INTO dbo.device (id, vendor, date_created, gateway_id) VALUES(3, 'LG', GETDATE(), 10);
INSERT INTO dbo.device (id, vendor, date_created, gateway_id) VALUES(5, 'SONY', GETDATE(), 2);

SELECT * FROM dbo.gateway;

SELECT * FROM dbo.device;

SELECT * FROM dbo.device_status;

SELECT * FROM dbo.device INNER JOIN dbo.gateway ON device.gateway_id = gateway.id;

SELECT * FROM dbo.device RIGHT JOIN dbo.gateway ON device.gateway_id = gateway.id;

SELECT * FROM dbo.device RIGHT JOIN dbo.gateway ON device.gateway_id = gateway.id LEFT JOIN dbo.device_status ON device_status.device_id = device.id;

EXEC dbo.TestProcedure 20;
GO

DECLARE @DeviceId int, @DeviceCreated datetime;
SET @DeviceCreated = GETDATE();
EXEC dbo.add_device '12324289723', 'SAMSUNG', @DeviceCreated, 'Online', @DeviceId OUT;
PRINT 'DeviceId ' + convert(varchar(10),@DeviceId);  
GO

EXEC dbo.add_gateway '123456789', 'Kole', '192.168.22.10';
GO

EXEC dbo.delete_device 0;
GO      

EXEC dbo.delete_gateway 'fasdf9723';
GO

EXEC dbo.get_all_gateways;
GO

EXEC dbo.get_gateway 'fasdf9723';
GO

EXEC dbo.set_device_status 3, 'Offline';
GO