USE ms_gateways;
GO

CREATE PROCEDURE dbo.set_device_status
	@DeviceId bigint,
	@Status nvarchar(50)
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE dbo.device_status SET status = @Status WHERE device_id = @DeviceId;
END
GO

DROP PROCEDURE dbo.set_device_status;
GO
