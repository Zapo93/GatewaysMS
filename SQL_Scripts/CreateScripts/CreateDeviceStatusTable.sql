USE [ms_gateways]
GO

CREATE TABLE [dbo].[device_status](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[status] [nvarchar](50) NOT NULL,
	[device_id] [int] NOT NULL,
 CONSTRAINT [PK_device_status] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[device_status]  WITH CHECK ADD  CONSTRAINT [FK_device_status_device] FOREIGN KEY([device_id])
REFERENCES [dbo].[device] ([id])
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[device_status] CHECK CONSTRAINT [FK_device_status_device]
GO


