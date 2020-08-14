USE [ms_gateways]
GO

/****** Object:  Index [IX_device_id_unique]    Script Date: 8/14/2020 18:22:00 ******/
CREATE UNIQUE NONCLUSTERED INDEX [IX_device_id_unique] ON [dbo].[device_status]
(
	[device_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO


