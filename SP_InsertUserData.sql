create procedure SP_InsertUserData @name nvarchar(50),@dateofbirth date,
@mobileno nvarchar(50),@email nvarchar(50),@password nvarchar(50),@confirmpassword nvarchar(50)
as
insert into T_Users(Name,DateOfBirth,MobileNo,Email,Password,ConfirmPassword)
values(@name,@dateofbirth,@mobileno,@email,@password,@confirmpassword)
go