USE [ms_gateways]
GO

/****** Object:  Table [dbo].[gateway]    Script Date: 8/14/2020 18:21:08 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[gateway](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[serial_number] [nvarchar](50) NOT NULL,
	[name] [nvarchar](50) NOT NULL,
	[ipv4] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_gateway] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO


