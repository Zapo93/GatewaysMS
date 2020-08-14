USE ms_gateways;
GO

CREATE PROCEDURE dbo.delete_gateway
	@GatewaySerialNumber nvarchar(50)
AS
BEGIN
	SET NOCOUNT ON;

	DELETE FROM dbo.gateway WHERE gateway.serial_number = @GatewaySerialNumber;
END
GO

DROP PROCEDURE dbo.delete_gateway;
GO
