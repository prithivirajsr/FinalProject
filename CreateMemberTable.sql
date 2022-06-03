USE [DB_FinalProject]
GO

/****** Object:  Table [dbo].[T_Members]    Script Date: 01-06-2022 07:25:49 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[T_Members](
	[MemberId] [int] IDENTITY(1,1) NOT NULL,
	[FullName] [nvarchar](50) NOT NULL,
	[Profession] [nvarchar](50) NOT NULL,
	[Gender] [nvarchar](50) NOT NULL,
	[DateOfBirth] [date] NOT NULL,
	[MobileNo] [nvarchar](50) NOT NULL,
	[City] [nvarchar](50) NOT NULL,
	[Pincode] [int] NOT NULL,
	[Email] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_T_Members] PRIMARY KEY CLUSTERED 
(
	[MemberId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

