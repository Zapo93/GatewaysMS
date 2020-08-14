USE ms_gateways;
GO

CREATE PROCEDURE dbo.delete_device
	@DeviceId bigint
AS
BEGIN
	SET NOCOUNT ON;

	DELETE FROM dbo.device WHERE device.id = @DeviceId;
END
GO

DROP PROCEDURE dbo.delete_device;
GO