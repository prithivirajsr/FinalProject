create procedure SP_InsertMemberData @fullname nvarchar(50),@profession nvarchar(50),@gender nvarchar(50),@dateofbirth date,
@mobileno nvarchar(50),@city nvarchar(50),@pincode int,@email nvarchar(50)
as
insert into T_Members(FullName,Profession,Gender,DateOfBirth,MobileNo,City,Pincode,Email)
values(@fullname,@profession,@gender,@dateofbirth,@mobileno,@city,@pincode,@email)
go