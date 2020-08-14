USE ms_gateways;
GO

CREATE PROCEDURE get_all_gateways
AS
BEGIN
	SET NOCOUNT ON;

	SELECT *
	FROM dbo.gateway;

	SELECT device.id, gateway_id, vendor, date_created, status
	FROM dbo.device RIGHT JOIN dbo.device_status ON device_status.device_id = device.id; 
END
GO

DROP PROCEDURE get_all_gateways;
GO
