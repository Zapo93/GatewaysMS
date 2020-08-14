USE ms_gateways;
GO 

CREATE PROCEDURE dbo.get_gateway
	@SerialNumber nvarchar(50)
AS
BEGIN
	SET NOCOUNT ON;
	DECLARE @GatewayID bigint;

	SET @GatewayID = (SELECT id 
					FROM gateway
					WHERE serial_number = @SerialNumber);

	SELECT *
	FROM gateway
	WHERE id = @GatewayID;

	SELECT device.id, gateway_id,vendor, date_created, status
	FROM device RIGHT JOIN dbo.device_status ON device_status.device_id = device.id
	WHERE device.gateway_id = @GatewayID;
END
GO

DROP PROCEDURE dbo.get_gateway;
GO
