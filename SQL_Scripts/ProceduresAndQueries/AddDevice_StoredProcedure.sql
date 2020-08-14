USE ms_gateways;
GO

CREATE PROCEDURE dbo.add_device
	@GatewaySerialNumber nvarchar(50),
	@Vendor nvarchar(50),
	@DateCreated datetime,
	@Status nvarchar(50),
	@DeviceId int OUTPUT
AS
BEGIN
	DECLARE @GatewayId int;
	
	SELECT  @GatewayId = id
	FROM dbo.gateway
	WHERE serial_number = @GatewaySerialNumber;

	PRINT 'GatewayId ' + convert(varchar(10),@GatewayId);

	SET @DeviceId = NEXT VALUE FOR dbo.device_id;

	PRINT 'DeviceID ' + convert(varchar(10),@DeviceId);

	INSERT INTO dbo.device (id, vendor, date_created, gateway_id) VALUES(@DeviceId, @Vendor, @DateCreated, @GatewayId);
	INSERT INTO dbo.device_status(status,device_id) VALUES (@Status,@DeviceId);
END
GO

DROP PROCEDURE dbo.add_device; 
GO 