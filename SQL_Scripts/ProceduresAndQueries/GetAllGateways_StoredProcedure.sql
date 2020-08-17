USE ms_gateways;
GO

CREATE PROCEDURE get_all_gateways
AS
BEGIN
	SET NOCOUNT ON;

	SELECT serial_number, name, ipv4
	FROM dbo.gateway;

	SELECT device.id, gateway.serial_number, vendor, date_created, device_status.status
	FROM dbo.device LEFT JOIN dbo.device_status ON device_status.device_id = device.id INNER JOIN dbo.gateway ON gateway.id = device.gateway_id;
END
GO
