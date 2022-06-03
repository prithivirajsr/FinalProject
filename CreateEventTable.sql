USE [DB_FinalProject]
GO

/****** Object:  Table [dbo].[T_Events]    Script Date: 01-06-2022 18:19:54 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[T_Events](
	[EventId] [int] IDENTITY(1,1) NOT NULL,
	[MemberId] [int] NOT NULL,
	[MemberName] [nvarchar] (50) NOT NULL,
	[Venue] [nvarchar](50) NOT NULL,
	[Date] [date] NOT NULL,
	[ECO] [nvarchar](50) NOT NULL,
	[GDC] [nvarchar](50) NOT NULL,
	[NSS] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_T_Events] PRIMARY KEY CLUSTERED 
(
	[EventId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[T_Events]  WITH CHECK ADD  CONSTRAINT [FK_T_Events_T_Members] FOREIGN KEY([MemberId])
REFERENCES [dbo].[T_Members] ([MemberId])
GO

ALTER TABLE [dbo].[T_Events] CHECK CONSTRAINT [FK_T_Events_T_Members]
GO

