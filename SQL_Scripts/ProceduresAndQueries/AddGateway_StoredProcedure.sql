USE ms_gateways;
GO

CREATE PROCEDURE dbo.add_gateway
	@SerialNumber nvarchar(50),
	@Name nvarchar(50),
	@IPv4 nvarchar(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	INSERT INTO dbo.gateway (serial_number, name, ipv4) VALUES (@SerialNumber, @Name, @IPv4);
END
GO

DROP PROCEDURE dbo.add_gateway;
GO