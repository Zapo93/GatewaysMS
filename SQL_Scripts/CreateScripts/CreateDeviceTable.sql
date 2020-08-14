USE [ms_gateways]
GO

/****** Object:  Table [dbo].[device]    Script Date: 8/14/2020 18:20:32 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[device](
	[id] [bigint] NOT NULL,
	[vendor] [nvarchar](50) NOT NULL,
	[date_created] [datetime] NOT NULL,
	[gateway_id] [int] NOT NULL,
 CONSTRAINT [PK_device] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[device]  WITH CHECK ADD  CONSTRAINT [FK_device_gateway] FOREIGN KEY([gateway_id])
REFERENCES [dbo].[gateway] ([id])
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[device] CHECK CONSTRAINT [FK_device_gateway]
GO


