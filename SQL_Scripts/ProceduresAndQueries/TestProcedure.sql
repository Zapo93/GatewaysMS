USE ms_gateways;
GO

CREATE PROCEDURE dbo.TestProcedure
	-- Add the parameters for the stored procedure here
	@PrinableVar int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	--SET @PrinableVar = 10;

	PRINT 'TEST MESSAGE VALUE ' + convert(varchar(10),@PrinableVar);  
	SELECT * FROM dbo.device RIGHT JOIN ms_gateways.dbo.gateway ON device.gateway_id = gateway.id;
END
GO

DROP PROCEDURE dbo.TestProcedure; 
GO 