create procedure SP_UpdateMember 
@fullname nvarchar(50),@profession nvarchar(50),@gender nvarchar(50),
@dateofbirth date,@mobileno nvarchar(50),@city nvarchar(50),@pincode int,@email nvarchar(50),@id int
as
update T_Members set FullName=@fullname,Profession=@profession,Gender=@gender,
DateOfBirth=@dateofbirth,MobileNo=@mobileno,City=@city,Pincode=@pincode,Email=@email where MemberId=@id
go