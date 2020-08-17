USE ms_gateways;
GO 

CREATE PROCEDURE dbo.get_gateway
	@SerialNumber nvarchar(50)
AS
BEGIN
	SET NOCOUNT ON;
	DECLARE @GatewayID int;

	SET @GatewayID = (SELECT id 
					FROM gateway
					WHERE serial_number = @SerialNumber);

	SELECT serial_number, name, ipv4
	FROM gateway
	WHERE id = @GatewayID;

	SELECT device.id, vendor, date_created, status, gateway.serial_number
	FROM device LEFT JOIN dbo.device_status ON device_status.device_id = device.id INNER JOIN dbo.gateway ON dbo.device.gateway_id = dbo.gateway.id
	WHERE device.gateway_id = @GatewayID;
END
GO
