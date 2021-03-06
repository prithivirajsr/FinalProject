USE [DB_FinalProject]
GO
/****** Object:  StoredProcedure [dbo].[SP_InsertEventData]    Script Date: 02-06-2022 15:45:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER procedure [dbo].[SP_InsertEventData]
@memberid int,@membername nvarchar(50),@venue nvarchar(50),
@date date,@eco nvarchar(50),@gdc nvarchar(50),@nss nvarchar(50)
as
insert into T_Events(MemberId,MemberName,Venue,Date,ECO,GDC,NSS)
values(@memberid,@membername,@venue,@date,@eco,@gdc,@nss)
