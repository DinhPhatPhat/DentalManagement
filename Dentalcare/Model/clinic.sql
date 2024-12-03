/*

use master
go
drop database clinic

*/
/*

create database clinic
go
use clinic
go

*/

--function tu tao id 

create function autoid(@NAME Char(2), @QUAN int)
returns VARCHAR(10)
as 
begin
	declare @ID VARCHAR(10)
	SELECT @ID = @NAME + RIGHT(REPLICATE(0,8), 8 - LEN(CAST (@QUAN AS VARCHAR(8)))) +  CAST(@QUAN AS VARCHAR(8))
	RETURN @ID
END
GO


CREATE TABLE Account
(
  id VARCHAR(10) NOT NULL,
  username VARCHAR(50) NOT NULL,
  password VARCHAR(50) NOT NULL,
  meta VARCHAR(MAX) NOT NULL,
  hide BIT NOT NULL DEFAULT 0,
  [order] INT NOT NULL IDENTITY(1,1),
  datebegin DATETIME NOT NULL DEFAULT GETDATE(),
  able BIT NOT NULL DEFAULT 1,
  PRIMARY KEY (id),
  UNIQUE (username)
);

CREATE TABLE Person
(
  meta VARCHAR(MAX) NOT NULL,
  hide BIT NOT NULL DEFAULT 0,
  [order] INT NOT NULL IDENTITY(1,1),
  datebegin DATETIME NOT NULL DEFAULT GETDATE(),
  name NVARCHAR(100) NOT NULL,
  phoneNumber VARCHAR(10) NOT NULL,
  email VARCHAR(100) NOT NULL,
  salary INT NOT NULL check(salary>= 0),
  address NVARCHAR(MAX) NOT NULL,
  gender BIT NOT NULL,
  birthday DATETIME NOT NULL,
  nation NVARCHAR(MAX) NOT NULL,
  role INT NOT NULL,
  img VARCHAR(MAX) NOT NULL,
  id VARCHAR(10) NOT NULL,
  PRIMARY KEY (id),
  FOREIGN KEY (id) REFERENCES Account(id),
  UNIQUE (phoneNumber),
  UNIQUE (email)
);

CREATE TABLE Avatar
(
    personId VARCHAR(10) NOT NULL,
    imgPath VARCHAR(MAX) NOT NULL,
    uploadDate DATETIME NOT NULL DEFAULT GETDATE(),
    PRIMARY KEY (personId),
    FOREIGN KEY (personId) REFERENCES Person(id)
);

CREATE TABLE Admin
(
  hide BIT NOT NULL DEFAULT 0,
  meta VARCHAR(MAX) NOT NULL,
  [order] INT NOT NULL IDENTITY(1,1),
  datebegin DATETIME NOT NULL DEFAULT GETDATE(),
  id VARCHAR(10) NOT NULL,
  PRIMARY KEY (id),
  FOREIGN KEY (id) REFERENCES Person(id)
);

CREATE TABLE Receptionist
(
  hide BIT NOT NULL DEFAULT 0,
  meta VARCHAR(MAX) NOT NULL,
  [order] INT NOT NULL IDENTITY(1,1),
  datebegin DATETIME NOT NULL DEFAULT GETDATE(),
  id VARCHAR(10) NOT NULL,
  PRIMARY KEY (id),
  FOREIGN KEY (id) REFERENCES Person(id)
);

CREATE TABLE Assisstant
(
  hide BIT NOT NULL DEFAULT 0,
  meta VARCHAR(MAX) NOT NULL,
  [order] INT NOT NULL IDENTITY(1,1),
  datebegin DATETIME NOT NULL DEFAULT GETDATE(),
  id VARCHAR(10) NOT NULL,
  PRIMARY KEY (id),
  FOREIGN KEY (id) REFERENCES Person(id)
);

CREATE TABLE Patient
(
  isVip INT NOT NULL,
  hide BIT NOT NULL DEFAULT 0,
  meta VARCHAR(MAX) NOT NULL,
  [order] INT NOT NULL IDENTITY(1,1),
  datebegin DATETIME NOT NULL DEFAULT GETDATE(),
  id VARCHAR(10) NOT NULL,
  PRIMARY KEY (id),
  FOREIGN KEY (id) REFERENCES Person(id)
);

CREATE TABLE Bill
(
  id VARCHAR(10) NOT NULL,
  totalPrice INT NOT NULL DEFAULT 0 check(totalPrice>=0),
  ServicesPrice INT NOT NULL DEFAULT 0 check(ServicesPrice>=0),
  PrescriptionPrice INT NOT NULL DEFAULT 0 check(PrescriptionPrice>=0),
  hide BIT NOT NULL DEFAULT 0,
  able BIT NOT NULL DEFAULT 1,
  meta VARCHAR(MAX) NOT NULL,
  [order] INT NOT NULL IDENTITY(1,1),
  datebegin DATETIME NOT NULL DEFAULT GETDATE(),
  isPayed BIT NOT NULL DEFAULT 0,
  patid VARCHAR(10) NOT NULL,
  PRIMARY KEY (id),
  FOREIGN KEY (patID) REFERENCES Patient(id)
);

CREATE TABLE Service_Category
(
  id VARCHAR(10) NOT NULL,
  name NVARCHAR(100) NOT NULL UNIQUE,
  descrip NVARCHAR(MAX) NOT NULL,
  note NVARCHAR(MAX) NOT NULL,
  meta VARCHAR(MAX) NOT NULL,
  hide BIT NOT NULL DEFAULT 0,
  [order] INT NOT NULL IDENTITY(1,1),
  datebegin DATETIME NOT NULL DEFAULT GETDATE(),
  PRIMARY KEY (id)
);

CREATE TABLE Service
(
  id VARCHAR(10) NOT NULL,
  cateId VARCHAR(10) NOT NUll,
  name NVARCHAR(100) NOT NULL,
  price INT NOT NULL check(price>=0),
  note NVARCHAR(MAX) NOT NULL,
  descrip NVARCHAR(MAX) NOT NULL,
  caredActor NVARCHAR(50) NOT NUll,
  meta VARCHAR(MAX) NOT NULL,
  hide BIT NOT NULL DEFAULT 0,
  [order] INT NOT NULL IDENTITY(1,1),
  datebegin DATETIME NOT NULL DEFAULT GETDATE(),
  calUnit NVARCHAR(50) NOT NULL,
  img VARCHAR(MAX) NOT NULL,
  PRIMARY KEY (id),
  FOREIGN KEY (cateId) REFERENCES Service_Category(id)
);
SELECT * FROM Service_Category WHERE meta = 'chua-tuy' AND hide = 0;

SELECT * FROM Service WHERE cateId = 'SC00000006' AND hide = 0;
CREATE TABLE Material_Category
(
  id VARCHAR(10) NOT NULL,
  name NVARCHAR(100) NOT NULL UNIQUE,
  descrip NVARCHAR(MAX) NOT NULL,
  note NVARCHAR(MAX) NOT NULL,
  meta VARCHAR(MAX) NOT NULL,
  hide BIT NOT NULL DEFAULT 0,
  able BIT NOT NULL DEFAULT 1,
  [order] INT NOT NULL IDENTITY(1,1),
  datebegin DATETIME NOT NULL DEFAULT GETDATE(),
  PRIMARY KEY (id)
);

CREATE TABLE Material
(
  id VARCHAR(10) NOT NULL,
  cateId VARCHAR(10) NOT NULL,
  name NVARCHAR(100) NOT NULL,
  quantity INT NOT NULL check(quantity>=0),
  calUnit NVARCHAR(50) NOT NULL,
  func NVARCHAR(MAX) NOT NULL,
  mfgDate DATETIME NOT NULL,
  able BIT NOT NULL DEFAULT 1,
  hide BIT NOT NULL DEFAULT 0,
  meta VARCHAR(MAX) NOT NULL,
  [order] INT NOT NULL IDENTITY(1,1),
  datebegin DATETIME NOT NULL DEFAULT GETDATE(),
  img VARCHAR(MAX) NOT NULL,
  PRIMARY KEY (id),
  UNIQUE (name),
  FOREIGN KEY (cateId) REFERENCES Material_Category(id)
);

CREATE TABLE FixedMaterial
(
  hide BIT NOT NULL DEFAULT 0,
  meta VARCHAR(MAX) NOT NULL,
  [order] INT NOT NULL IDENTITY(1,1),
  datebegin DATETIME NOT NULL DEFAULT GETDATE(),
  able BIT NOT NULL DEFAULT 1,
  id VARCHAR(10) NOT NULL,
  PRIMARY KEY (id),
  FOREIGN KEY (id) REFERENCES Material(id)
);

CREATE TABLE Ingredient
(
  id VARCHAR(10) NOT NULL,
  name NVARCHAR(100) NOT NULL UNIQUE,
  hide BIT NOT NULL DEFAULT 0,
  meta VARCHAR(MAX) NOT NULL,
  [order] INT NOT NULL IDENTITY(1,1),
  datebegin DATETIME NOT NULL DEFAULT GETDATE(),
  able BIT NOT NULL DEFAULT 1,
  PRIMARY KEY (id),
);

CREATE TABLE ConsumableMaterial
(
  id VARCHAR(10) NOT NULL,
  expDate DATETIME NOT NULL,
  hide BIT NOT NULL DEFAULT 0,
  meta VARCHAR(MAX) NOT NULL,
  [order] INT NOT NULL IDENTITY(1,1),
  datebegin DATETIME NOT NULL DEFAULT GETDATE(),
  able BIT NOT NULL DEFAULT 1,
  PRIMARY KEY (id),
  FOREIGN KEY (id) REFERENCES Material(id)
);

CREATE TABLE Ingredient_ConsumableMaterial
(
  able int not null DEFAULT 1,
  ingreId VARCHAR(10) NOT NULL,
  consumId VARCHAR(10) NOT NULL,
  PRIMARY KEY (ingreId,consumId),
  FOREIGN KEY (ingreId) REFERENCES Ingredient(id),
  FOREIGN KEY (consumId) REFERENCES ConsumableMaterial(id)
);

CREATE TABLE Medicine
(
  caredACtor NVARCHAR(MAX) NOT NULL,
  price INT NOT NULL check(price>=0),
  hide BIT NOT NULL DEFAULT 0,
  meta VARCHAR(MAX) NOT NULL,
  instruction NVARCHAR(MAX) NOT NULL,
  [order] INT NOT NULL IDENTITY(1,1),
  datebegin DATETIME NOT NULL DEFAULT GETDATE(),
  able BIT NOT NULL DEFAULT 1,
  id VARCHAR(10) NOT NULL,
  PRIMARY KEY (id),
  FOREIGN KEY (id) REFERENCES ConsumableMaterial(id)
);

CREATE TABLE Bill_Service
(
  quantity int not null,
  hide BIT NOT NULL DEFAULT 0,
  meta VARCHAR(MAX) NOT NULL,
  [order] INT NOT NULL IDENTITY(1,1),
  datebegin DATETIME NOT NULL DEFAULT GETDATE(),
  billID VARCHAR(10) NOT NULL,
  SerID VARCHAR(10) NOT NULL,
  able BIT NOT NULL DEFAULT 1, 
  PRIMARY KEY (billID,SerID),
  FOREIGN KEY (billID) REFERENCES Bill(id),
  FOREIGN KEY (SerID) REFERENCES Service(id)
);

CREATE TABLE Faculty
(
  id VARCHAR(10) NOT NULL,
  name NVARCHAR(MAX) NOT NULL,
  descrip NVARCHAR(MAX) NOT NULL DEFAULT('none'),
  able BIT NOT NULL DEFAULT 1,
  hide BIT NOT NULL DEFAULT 0,
  meta VARCHAR(MAX) NOT NULL,
  [order] INT NOT NULL IDENTITY(1,1),
  datebegin DATETIME NOT NULL DEFAULT GETDATE(),
  PRIMARY KEY (id)
);

CREATE TABLE Calendar
(
  id VARCHAR(10) NOT NULL,
  timeStart DATETIME NOT NULL ,
  timeEnd DATETIME NOT NULL ,
  able BIT NOT NULL DEFAULT 1,
  hide BIT NOT NULL DEFAULT 0,
  meta VARCHAR(MAX) NOT NULL,
  [order] INT NOT NULL IDENTITY(1,1),
  datebegin DATETIME NOT NULL DEFAULT GETDATE(),
  Personid VARCHAR(10) NOT NULL,
  PRIMARY KEY (id),
  FOREIGN KEY (PersonID) REFERENCES Person(id)
);

CREATE TABLE Comment
(
  id VARCHAR(10) NOT NULL,
  msg NVARCHAR(MAX) NOT NULL,
  title NVARCHAR(MAX) NOT NULL,
  img VARCHAR(MAX) NOT NULL,
  hide BIT NOT NULL DEFAULT 0,
  meta VARCHAR(MAX) NOT NULL,
  [order] INT NOT NULL IDENTITY(1,1),
  datebegin DATETIME NOT NULL DEFAULT GETDATE(),
  patid VARCHAR(10) NOT NULL,
  PRIMARY KEY (id),
  FOREIGN KEY (patID) REFERENCES Patient(id)
);

CREATE TABLE Advertisement
(
  id VARCHAR(10) NOT NULL,
  msg NVARCHAR(MAX) NOT NULL,
  title NVARCHAR(MAX) NOT NULL,
  img VARCHAR(MAX) NOT NULL,
  hide BIT NOT NULL DEFAULT 0,
  meta VARCHAR(MAX) NOT NULL,
  [order] INT NOT NULL IDENTITY(1,1),
  datebegin DATETIME NOT NULL DEFAULT GETDATE(),
  PRIMARY KEY (id)
);

CREATE TABLE Clinic
(
  id VARCHAR(10) NOT NULL,
  name NVARCHAR(100) NOT NULL,
  phoneNumber VARCHAR(10) NOT NULL,
  address NVARCHAR(MAX) NOT NULL,
  img VARCHAR(MAX) NOT NULL,
  email VARCHAR(100) NOT NULL,
  facebook VARCHAR(MAX) NOT NULL,
  zalo VARCHAR(MAX) NOT NULL,
  instagram VARCHAR(MAX) NOT NULL,
  youtube VARCHAR(MAX) NOT NULL,
  hide BIT NOT NULL DEFAULT 0,
  title NVARCHAR(MAX) NOT NULL,
  msg NVARCHAR(MAX) NOT NULL,
  meta VARCHAR(MAX) NOT NULL,
  dateStartClinic DATETIME NOT NULL,
  [order] INT NOT NULL IDENTITY(1,1),
  datebegin DATETIME NOT NULL DEFAULT GETDATE(),
  PRIMARY KEY (id)
);

CREATE TABLE Menu
(
  id VARCHAR(10) NOT NULL,
  name NVARCHAR(100) NOT NULL,
  link NVARCHAR(MAX) NOT NULL,
  hide BIT NOT NULL DEFAULT 0,
  meta VARCHAR(MAX) NOT NULL,
  [order] INT NOT NULL IDENTITY(1,1),
  datebegin DATETIME NOT NULL  DEFAULT GETDATE(),
  PRIMARY KEY (id)
);
CREATE TABLE Footer
(
  id VARCHAR(10) NOT NULL,
  name NVARCHAR(100) NOT NULL,
  link NVARCHAR(MAX),
  hide BIT NOT NULL DEFAULT 0,
  meta VARCHAR(MAX) NOT NULL,
  [order] INT NOT NULL IDENTITY(1,1),
  datebegin DATETIME NOT NULL  DEFAULT GETDATE(),
  PRIMARY KEY (id)
);
CREATE TABLE Dentist
(
  title NVARCHAR(50) NOT NULL,
  hide BIT NOT NULL DEFAULT 0,
  meta VARCHAR(MAX) NOT NULL,
  [order] INT NOT NULL IDENTITY(1,1),
  datebegin DATETIME NOT NULL DEFAULT GETDATE(),
  id VARCHAR(10) NOT NULL,
  falid VARCHAR(10) NOT NULL,
  descrip NVARCHAR(MAX) NOT NULL DEFAULT('none'),
  PRIMARY KEY (id),
  FOREIGN KEY (id) REFERENCES Person(id),
  FOREIGN KEY (falID) REFERENCES Faculty(id)
);

CREATE TABLE Prescription
(
  id VARCHAR(10) NOT NULL,
  note NVARCHAR(MAX) NOT NULL,
  price INT NOT NULL DEFAULT 0 check(price>=0),
  able BIT NOT NULL DEFAULT 1,
  hide BIT NOT NULL DEFAULT 0,
  meta VARCHAR(MAX) NOT NULL,
  [order] INT NOT NULL IDENTITY(1,1),
  datebegin DATETIME NOT NULL DEFAULT GETDATE(),
  denid VARCHAR(10) NOT NULL,
  patid VARCHAR(10) NOT NULL,
  billid VARCHAR(10) NOT NULL  DEFAULT 'BI00000001',
  PRIMARY KEY (id),
  FOREIGN KEY (denID) REFERENCES Dentist(id),
  FOREIGN KEY (patID) REFERENCES Patient(id),
  FOREIGN KEY (billID) REFERENCES Bill(id)
);

CREATE TABLE Prescription_Medicine
(
  preId VARCHAR(10) NOT NULL,
  medId VARCHAR(10) NOT NULL,
  quantityMedicine INT NOT NULL,
  meta VARCHAR(MAX) NOT NULL,
  hide BIT NOT NULL DEFAULT 0,
  able BIT NOT NULL DEFAULT 1,
  [order] INT NOT NULL IDENTITY(1,1),
  datebegin DATETIME NOT NULL DEFAULT GETDATE(),
  PRIMARY KEY (preId,medID),
  FOREIGN KEY (preId) REFERENCES Prescription(id),
  FOREIGN KEY (medId) REFERENCES Medicine(id)
);

CREATE TABLE Appointment
(
  able BIT NOT NULL DEFAULT 1,
  id VARCHAR(10) NOT NULL,
  symptom NVARCHAR(MAX) NOT NULL,
  state NVARCHAR(50) NOT NULL,
  timeStart DATETIME NOT NULL,
  timeEnd DATETIME NOT NULL,
  note NVARCHAR(MAX) NOT NULL,
  hide BIT NOT NULL DEFAULT 0,
  meta VARCHAR(MAX) NOT NULL,
  [order] INT NOT NULL IDENTITY(1,1),
  datebegin DATETIME NOT NULL DEFAULT GETDATE(),
  denid VARCHAR(10) NOT NULL,
  patid VARCHAR(10) NOT NULL,
  PRIMARY KEY (id),
  FOREIGN KEY (denID) REFERENCES Dentist(id),
  FOREIGN KEY (patID) REFERENCES Patient(id)
);

CREATE TABLE NEWS
(
  id VARCHAR(10) NOT NULL,
  img VARCHAR(MAX) NOT NULL,
  title NVARCHAR(MAX) NOT NULL,
  descrip NVARCHAR(MAX) NOT NULL,
  msg NVARCHAR(MAX) NOT NULL,
  meta VARCHAR(MAX) NOT NULL,
  hide BIT NOT NULL DEFAULT 0,
  [order] INT NOT NULL IDENTITY(1,1),
  datebegin DATETIME NOT NULL DEFAULT GETDATE(),
  PRIMARY KEY (id),
);

GO
--Tao Trigger
/*
--Cap nhat tien cua bill khi them dong trong Bill_Service
CREATE TRIGGER ADD_OR_UPDATE_Bill_Service
ON Bill_Service
AFTER INSERT,UPDATE
AS
BEGIN
  -- lay tong gia cua cac service theo billid cua bill_service duoc cap nhat
	-- chi co gia cua service(chua tinh gia cua don thuoc)
  (select bs.billID,'Total' = SUM(s.price*bs.quantity) into T
	from Bill_Service bs
	join service s
	on bs.billID = (select billID from inserted) AND bs.able = 1 AND bs.SerID = s.id
	group by bs.billID)

  -- cap nhat gia tri servicesPrice bill
	UPDATE Bill  
	SET servicesPrice = (select total from T)
	WHERE bill.id in (select billID from T) 
	drop table T
END
GO

--Cap nhat tien cua bill khi thay doi trong Prescription
CREATE TRIGGER ADD_OR_UPDATE_Prescription
ON Prescription 
AFTER INSERT,UPDATE 
AS
BEGIN
	UPDATE Bill 
	SET PrescriptionPrice = 
  case
    when (select able from inserted) = 1 then 
         (select price from inserted) 
    else 0
  end
  WHERE Bill.id = (select billid from inserted) 
END
GO
--Cap nhat tien cua Prescription khi them dong trong Prescription_Medicine
CREATE TRIGGER ADD_OR_UPDATE_Prescription_Medicine
ON Prescription_Medicine 
AFTER INSERT,UPDATE
AS
BEGIN
	UPDATE Prescription 
	SET price =
  case
    when (select able from inserted) = 1 then 
          price + ((select price from Medicine m WHERE m.id = (select medId from inserted) )*(select quantityMedicine from inserted)) 
    else  price - ((select price from Medicine m WHERE m.id = (select medId from inserted) )*(select quantityMedicine from inserted)) 
  end  
	WHERE Prescription.denid = (select denid from inserted) 
  AND Prescription.patid = (select patid from inserted) 
  AND Prescription.billid = (select billid from inserted) 
END
GO

--cap nhat tien cua bill khi servicesPrice hoac Precripsionprice thay doi
CREATE TRIGGER UPDATE_BILL
ON Bill
AFTER UPDATE
AS
BEGIN
  if(UPDATE(PrescriptionPrice) OR UPDATE(servicesPrice))
  BEGIN
    UPDATE Bill 
    SET totalPrice =  PrescriptionPrice + servicesPrice
    WHERE Bill.id = (select id from inserted) 
  END
END
GO

*/

--STORE PROCEDURE
--Them du lieu
--Account,Person,Manager,Receptionist,Dentist,Assisstant,Patient
create proc procAddAccountAndPerson
	--thuoc tinh cua account
	@username NVARCHAR(50),
	@password NVARCHAR(50),

	--thuoc tinh cua person
	@name NVARCHAR(100),
	@phoneNumber VARCHAR(10),
	@email VARCHAR(100),
	@salary INT,
	@address NVARCHAR(MAX),
	@gender BIT,
	@birthday DATETIME,
	@nation NVARCHAR(MAX),
	@role INT,
	@img VARCHAR(MAX),

	--thuoc tinh cua dentist
	@falID VARCHAR(10),
	@title NVARCHAR(50),

	--Cac the meta
	@MetaAccount VARCHAR(MAX),
	@MetaPerson VARCHAR(MAX),
	@MetaPersonDetail VARCHAR(MAX),

	--thuoc tinh descrip
	@descrip NVARCHAR(MAX)
as
begin
	--insert vao account
	declare @Quan INT
	set @Quan = (select count(*) from Account)
	if (TRIM(@MetaAccount) = '')
		begin
			set @MetaAccount = 'tai-khoan-' + CONVERT(varchar(MAX), @Quan+1)
		end
	declare @AccID VARCHAR(10)
	set @AccID = dbo.autoid('AC', @Quan+1)
	insert into Account(id, username, password,meta)
	values (@AccID,@username,@password,@MetaAccount)

	--insert vao person
	
	if(TRIM(@MetaPerson) ='')
		begin
			set @MetaPerson = 'nguoi-dung-' + CONVERT(varchar(MAX), @Quan+1) --So luong nguoi dung va tai khoan phai bang nhau
		end
	insert into Person(id, name, phoneNumber,email,salary,address,gender,birthday,nation,role,img,meta)
	values (@AccID,@name,@phoneNumber,@email,@salary,@address,@gender,@birthday,@nation,@role,@img,@MetaPerson)
	
	--Chia theo vai tro cua role
	--1 la quan li
	--2 la le tan
	--3 la nha si
	--4 la phu ta
	--5 la benh nhan

	--insert vao quan li
	if(@role = 1)
		begin
			declare @QuanAdmin INT
			set @QuanAdmin = (select count(*) from Admin)
			if(TRIM(@MetaPersonDetail)='') 
				begin
					set @MetaPersonDetail = 'quan-li-' + CONVERT(varchar(MAX), @QuanAdmin+1)
				end
			insert into Admin(id,meta)
			values (@AccID,@MetaPersonDetail)
		end
	--insert vao le tan
	else if (@role = 2)
		begin
			declare @QuanRe INT
			set @QuanRe = (select count(*) from Receptionist)
			if(TRIM(@MetaPersonDetail)='') 
				begin
					set @MetaPersonDetail = 'le-tan-' + CONVERT(varchar(MAX), @QuanRe+1)
				end
			insert into Receptionist(id,meta)
			values (@AccID,@MetaPersonDetail)
		end
	--insert vao nha si
	else if (@role = 3)
		begin
			declare @QuanDen INT
			set @QuanDen = (select count(*) from Dentist)
			if(TRIM(@MetaPersonDetail)='') 
				begin
					set @MetaPersonDetail = 'nha-si-' + CONVERT(varchar(MAX), @QuanDen+1)
				end
			insert into Dentist(id,title,falid,meta,descrip)
			values (@AccID,@title,@falID,@MetaPersonDetail,@descrip)
		end
	--insert vao phu ta
	else if (@role = 4)
		begin
			declare @QuanAs INT
			set @QuanAs = (select count(*) from Assisstant)
			if(TRIM(@MetaPersonDetail)='') 
				begin
					set @MetaPersonDetail = 'phu-ta-' + CONVERT(varchar(MAX), @QuanAs+1)
				end
			insert into Assisstant(id,meta)
			values (@AccID,@MetaPersonDetail)
		end
	--insert vao benh nhan
	else if (@role = 5)
		begin
			declare @QuanPat INT
			set @QuanPat = (select count(*) from Patient)
			if(TRIM(@MetaPersonDetail)='') 
				begin
					set @MetaPersonDetail = 'benh-nhan-' + CONVERT(varchar(MAX), @QuanPat+1)
				end
			insert into Patient(id,meta,isVip)
			values (@AccID,@MetaPersonDetail,0)
		end
end 
go

--select * from Account
--select * from Person


--them quan li
exec procAddAccountAndPerson 'admin','123',N'Tạ Triết','0908823743','tatriet16@gmail.com',23000000
,N'23 Lê Hồng Phong, Châu Thành, Trà Vinh',1,'2004-09-21',N'Việt Nam',1,'','','','tai-khoan-thu-nhat', 'nguoi-dung-dau-tien', 'ta-triet',''
--select * from Admin

--them le tan
exec procAddAccountAndPerson 'recep1','234',N'Đinh Phát Phát','0908832142','phatphat@gmail.com',23000000
,N'154 Phạm Ngũ Lão, Thành Thái, Gò Vấp',1,'2004-11-28',N'Việt Nam',2,'','','','tai-khoan-thu-2', 'nguoi-dung-sau', 'dinh-phat-phat',''
exec procAddAccountAndPerson 'recep2','11',N'Đăng Văn Trọng','0893842173','dvv@gmail.com',22000000
,N'182 Tạ Quang Bữu, Phường 9, Quận 10',1,'2004-01-02',N'Việt Nam',2,'','','','', 'nguoi-dung-tam', '',''
go
--select * from receptionist

--FUNCTION TÌM ROLE DỰA VÀO PERSONID
CREATE FUNCTION funcGetRoleByPersonId(@PersonID VARCHAR(10))
RETURNS INT
AS
BEGIN
    DECLARE @Role INT;
    IF EXISTS (SELECT 1 FROM Admin WHERE id = @PersonID)
        SET @Role = 1;  -- Quản lý
    ELSE IF EXISTS (SELECT 1 FROM Receptionist WHERE id = @PersonID)
        SET @Role = 2;  -- Lễ tân
    ELSE IF EXISTS (SELECT 1 FROM Dentist WHERE id = @PersonID)
        SET @Role = 3;  -- Nha sĩ
    ELSE IF EXISTS (SELECT 1 FROM Assisstant WHERE id = @PersonID)
        SET @Role = 4;  -- Phụ tá
    ELSE IF EXISTS (SELECT 1 FROM Patient WHERE id = @PersonID)
        SET @Role = 5;  -- Bệnh nhân
    ELSE
        SET @Role = 0;  -- Không tìm thấy vai trò

    RETURN @Role;
END
GO

--THEM KHOA
create proc procAddFaculty
	@name NVARCHAR(MAX),
	@descrip NVARCHAR(MAX),
	@meta VARCHAR(MAX)
as
begin

	declare @QuanFal INT
	set @QuanFal = (select count(*) from Faculty)
	if(TRIM(@meta)='') 
		begin
			set @meta = 'khoa-' + CONVERT(varchar(MAX), @QuanFal+1)
		end
	declare @id VARCHAR(10)
	set @id = dbo.autoid('FA', @QuanFal+1)
	insert into Faculty(id,descrip,meta,name)
	values (@id,@descrip,@meta,@name)
end
go

--select * from Faculty

--them khoa
exec procAddFaculty N'Nha chu', N'Khoa chuyên về chăm sóc và điều trị các bệnh lý liên quan đến nướu và các mô mềm quanh răng.', 'khoa-nha-chu'
exec procAddFaculty N'Phục Hình', N'Khoa chuyên thực hiện phục hồi chức năng nhai cho bệnh nhân thông qua các biện pháp như hàm giả, cầu răng.', 'khoa-phuc-hinh'
exec procAddFaculty N'Răng trẻ em', N'Khoa chuyên điều trị và chăm sóc răng miệng cho trẻ em, bao gồm cả việc phòng ngừa các bệnh lý.', 'khoa-rang-tre-em'
exec procAddFaculty N'Nhổ răng và tiểu phẩu', N'Khoa chuyên thực hiện các phẫu thuật nhổ răng và các tiểu phẫu liên quan đến răng miệng.', 'khoa-nho-rang-va-tieu-phau'
exec procAddFaculty N'Chữa răng và nội nha', N'Khoa chuyên điều trị các bệnh lý về răng, đặc biệt là các vấn đề nội nha như tủy răng.', 'khoa-chu-rang-va-noi-nha'
exec procAddFaculty N'Tổng quát', N'Khoa cung cấp các dịch vụ khám và điều trị tổng quát cho bệnh nhân, bao gồm cả tư vấn và chăm sóc răng miệng.', 'khoa-tong-quat'

GO

--them nha si
exec procAddAccountAndPerson 'dentist1','123',N'Lâm Đình Kiêm','09023511284',  'lamvak@gmail.com', 9000000,N'Tây Ninh'
,  1,'2003-01-02',N'Việt Nam',3,'', 'FA00000001',N'Thạc sĩ','','','',''
exec procAddAccountAndPerson 'dentist2','123',N'Hoa Hồ Quốc Đại', '09092815226', 'hoadai@gmail.com', 8700000,N'Kiên Giang'
,  1,'1998-07-02',N'Việt Nam',3,'', 'FA00000002',N'Tiến sĩ','','','',''
exec procAddAccountAndPerson 'dentist3','123',N'Bùi Xuân Huấn', '09062715226', 'buixhuan@gmail.com', 8000000,N'Trà Vinh'
,  1,'2010-11-11',N'Việt Nam',3,'', 'FA00000003',N'Thạc sĩ','','','',''
exec procAddAccountAndPerson 'dentist4','123',N'Linh Văn Sơn', '09028365226', 'linhnui@gmail.com', 8000000,N'Thanh Hoá'
,  1,'2014-03-14',N'Việt Nam',3,'', 'FA00000004',N'Tiến sĩ','','','',''
exec procAddAccountAndPerson 'dentist5','123',N'Tạ Minh Triết', '09024635226', 'taminhtriet16@gmail.com', 9300000,N'An Giang'
,  1,'1980-01-21',N'Việt Nam',3,'', 'FA00000005',N'Thạc sĩ','','','',''
exec procAddAccountAndPerson 'dentist6','123',N'Hoa Yến Anh', '09022915226', 'anhvippro@gmail.com', 9400000,N'Tân Biên'
,  0,'1990-01-01',N'Việt Nam',3,'', 'FA00000006',N'Tiến sĩ','','','',''
GO
--select * from dentist

--them mo ta cho nha si
UPDATE Dentist
SET [descrip] = CASE 
    WHEN id = 'AC00000004' THEN N'Nha sĩ có hơn 10 năm kinh nghiệm trong lĩnh vực nha khoa tổng quát, chuyên về chăm sóc răng miệng cho trẻ em.'
    WHEN id = 'AC00000005' THEN N'Chuyên gia hàng đầu về chỉnh nha và cấy ghép implant, đã thực hiện hàng trăm ca phẫu thuật thành công.'
    WHEN id = 'AC00000006' THEN N'Nha sĩ tận tâm, chuyên về phục hình răng sứ và các dịch vụ thẩm mỹ nha khoa hiện đại.'
    WHEN id = 'AC00000007' THEN N'Giảng viên đại học chuyên về nghiên cứu các phương pháp điều trị viêm nướu và bệnh lý nha chu.'
    WHEN id = 'AC00000008' THEN N'Kỹ năng xuất sắc trong điều trị tủy và chữa trị các bệnh lý răng miệng phức tạp.'
    WHEN id = 'AC00000009' THEN N'Chuyên gia tư vấn các giải pháp chăm sóc răng miệng cho người cao tuổi, với nhiều năm kinh nghiệm trong lĩnh vực này.'
END
WHERE id IN ('AC00000004', 'AC00000005', 'AC00000006', 'AC00000007', 'AC00000008', 'AC00000009');
go

--them phu ta 
exec procAddAccountAndPerson 'assisstant1','12',N'Thiết Kiến Công Chúa','0893320123','congchuaslay@gmail.com',3000000
,N'291 Phố Đi Bộ, Nguyễn Huệ, Quận 1',1,'2004-04-12',N'Việt Nam',4,'','','','   ','   ','  ',''
exec procAddAccountAndPerson 'assisstant2','13',N'Cô Văn Nan','0892930213','shinichi@gmail.com',2400000
,N'19 Phố Baker, Phường An Lạc, Bình Dương',1,'2004-05-12',N'Nhật Bản',4,'','','','','','',''
exec procAddAccountAndPerson 'assisstant3','14',N'Đặc Vụ Ngầm','0902931823','aibiet@gmail.com',2200000
,N'12 Lạc long quân, Hàng Chài, Phú Thọ',0,'2001-01-02',N'Anh',4,'','','','','','',''
exec procAddAccountAndPerson 'assisstant4','15',N'Mai Quốc Khánh','0902839141','khanhvippro@gmail.com',2800000
,N'20 Nguyễn Du, Phường Đồng Khánh, Quận 3',0,'2004-09-02',N'Việt Nam',4,'','','','','','',''
--select * from assisstant

--them benh nhan
exec procAddAccountAndPerson 'patient1','12',N'Thân Quốc Thịnh','0908827372','thinhthinh@gmail.com',0
,N'12 Phạm Văn Đồng, Phường Tân Khải, Quận 5',1,'1998-05-30',N'Việt Nam',5,'','','','','','',''
exec procAddAccountAndPerson 'patient2','13',N'Châu Nguyễn Khánh Trình','0908829374','trinhchicken@gmail.com',0
,N'19 Văn Thành, Phường Khởi Định, Quận 3',1,'1998-03-25',N'Việt Nam',5,'','','','','','',''
exec procAddAccountAndPerson 'patient3','14',N'Trịnh Thị Hồng Phúc','0908861419','phuccute@gmail.com',0
,N'12 Long Thành Bắc, Khu Phố 3, Hòa Thành, Tây Ninh',0,'2004-02-02',N'Việt Nam',5,'','','','','','',''
exec procAddAccountAndPerson 'patient4','15',N'Hồ Minh Thư','0908829131','thuho@gmail.com',0
,N'2 Hà Thanh, Khu Phố 2, Châu Thành, An GIang',0,'2001-10-12',N'Việt Nam',5,'','','','','','',''
go
--select * from patient


--THÊM AVATAR
create proc procAddAvatar
    @personId VARCHAR(10)
AS
BEGIN
	DECLARE @role INT;
	DECLARE @imgPath VARCHAR(MAX)
    -- Kiểm tra xem personId đã tồn tại trong bảng Person chưa
    IF EXISTS (SELECT 1 FROM Person WHERE id = @personId)
    BEGIN
		SET @role = dbo.funcGetRoleByPersonId(@personId)
        -- Thêm ảnh đại diện vào bảng Avatar
		SET @imgPath = CONCAT('Content/images/', CASE 
			WHEN @role = 1 THEN 'Admin' 
			WHEN @role = 2 THEN 'Receptionist' 
			WHEN @role = 3 THEN 'Dentist' 
			WHEN @role = 4 THEN 'Assistant' 
			WHEN @role = 5 THEN 'Patient' 
		END, '/', @personId, '/avatar.jpg');

	INSERT INTO Avatar(personId, imgPath) VALUES (@personId, @imgPath);
			PRINT 'Avatar added successfully.';
    END
    ELSE
    BEGIN
        PRINT 'Person ID does not exist.';
    END
END
GO

EXEC procAddAvatar @personId = 'AC00000004';
EXEC procAddAvatar @personId = 'AC00000005';
EXEC procAddAvatar @personId = 'AC00000006';
EXEC procAddAvatar @personId = 'AC00000007';
EXEC procAddAvatar @personId = 'AC00000008';
EXEC procAddAvatar @personId = 'AC00000009';

--patient
EXEC procAddAvatar @personId = 'AC00000014';
EXEC procAddAvatar @personId = 'AC00000015';
EXEC procAddAvatar @personId = 'AC00000016';
EXEC procAddAvatar @personId = 'AC00000017';
go
--Select * from Avatar

--Service_Category
create proc procAddService_Category
	@name NVARCHAR(100),
	@descip NVARCHAR(MAX),
	@note NVARCHAR(MAX),
	@meta NVARCHAR(MAX)
as
begin
	declare @QuanSC INT
	set @QuanSC = (select count(*) from Service_Category)
	if(TRIM(@meta)='') 
		begin
			set @meta = 'loai-dich-vu-' + CONVERT(varchar(MAX), @QuanSC+1)
		end
	declare @id varchar(10)
	set @id = dbo.autoid('SC', @QuanSC+1)
	insert into Service_Category(id,name,descrip,note,meta)
	values (@id,@name,@descip,@note,@meta)
end
go

--them Service_Category
exec procAddService_Category N'Khám tổng quát', N'Tìm kiếm các răng bị bệnh', '', 'kham-tong-quat'
exec procAddService_Category N'Nhổ răng', N'Nhổ răng bị bệnh', '', ''
exec procAddService_Category N'Tiểu phẫu thuật', N'Tiểu phẩu', '', ''
exec procAddService_Category N'Nha chu', N'', '', ''
exec procAddService_Category N'Chữa răng-Nội nha', N'Tái tạo và trám răng', '', ''
exec procAddService_Category N'Chữa tủy', N'Chữa tủy của răng', '', ''
exec procAddService_Category N'Phục hình tháo lắp', N'tháo/lắp răng mới', '', ''
exec procAddService_Category N'Sửa chữa hàm', N'Làm mới hàm răng', '', ''
exec procAddService_Category N'Điều trị tiền phục hình', N'', '', ''
exec procAddService_Category N'Phục hình cố định', N'', '', ''
exec procAddService_Category N'Điều trị răng sữa', N'', '', ''
exec procAddService_Category N'Chỉnh hình răng mặt', N'', '', ''
exec procAddService_Category N'Nha công cộng', N'', '', ''
exec procAddService_Category N'Điều trị loạn năng hệ thống nhai', N'', '', ''
exec procAddService_Category N'X-quang răng', N'', '', ''
exec procAddService_Category N'Răng sứ dán', N'', '', ''
exec procAddService_Category N'Hàm phủ tháo lắp', N'', '', ''
exec procAddService_Category N'Hàm cố định bắt vít', N'', '', ''
exec procAddService_Category N'Phục hình tạm tức thì toàn hàm', N'', '', ''
exec procAddService_Category N'Cấy ghép 1 trụ Implantt', N'', '', ''
exec procAddService_Category N'Màng, xương tổng hợp', N'', '', ''
exec procAddService_Category N'Màng, xương tự thân', N'', '', ''
exec procAddService_Category N'Nâng xoang', N'', '', ''
exec procAddService_Category N'Chụp CT CONE BEAM', N'', '', ''
exec procAddService_Category N'Máng Hướng Dẫn / in Sọ Mặt', N'', '', ''
go
UPDATE Service_Category SET meta = 'nho-rang' WHERE name = N'Nhổ răng';
UPDATE Service_Category SET meta = 'tieu-phau' WHERE name = N'Tiểu phẫu thuật';
UPDATE Service_Category SET meta = 'nha-chu' WHERE name = N'Nha chu';
UPDATE Service_Category SET meta = 'chua-rang-noi-nha' WHERE name = N'Chữa răng-Nội nha';
UPDATE Service_Category SET meta = 'chua-tuy' WHERE name = N'Chữa tủy';
UPDATE Service_Category SET meta = 'phuc-hinh-thao-lap' WHERE name = N'Phục hình tháo lắp';
UPDATE Service_Category SET meta = 'sua-chua-ham' WHERE name = N'Sửa chữa hàm';
UPDATE Service_Category SET meta = 'dieu-tri-tien-phuc-hinh' WHERE name = N'Điều trị tiền phục hình';
UPDATE Service_Category SET meta = 'phuc-hinh-co-dinh' WHERE name = N'Phục hình cố định';
UPDATE Service_Category SET meta = 'dieu-tri-rang-sua' WHERE name = N'Điều trị răng sữa';
UPDATE Service_Category SET meta = 'chinh-hinh-rang-mat' WHERE name = N'Chỉnh hình răng mặt';
UPDATE Service_Category SET meta = 'nha-cong-cong' WHERE name = N'Nha công cộng';
UPDATE Service_Category SET meta = 'dieu-tri-loan-nang-he-thong-nhai' WHERE name = N'Điều trị loạn năng hệ thống nhai';
UPDATE Service_Category SET meta = 'x-quang-rang' WHERE name = N'X-quang răng';
UPDATE Service_Category SET meta = 'rang-su-dan' WHERE name = N'Răng sứ dán';
UPDATE Service_Category SET meta = 'ham-phu-thao-lap' WHERE name = N'Hàm phủ tháo lắp';
UPDATE Service_Category SET meta = 'ham-co-dinh-bat-vit' WHERE name = N'Hàm cố định bắt vít';
UPDATE Service_Category SET meta = 'phuc-hinh-tam-tuc-thi-toan-ham' WHERE name = N'Phục hình tạm tức thì toàn hàm';
UPDATE Service_Category SET meta = 'cay-ghep-implant' WHERE name = N'Cấy ghép 1 trụ Implantt';
UPDATE Service_Category SET meta = 'mang-xuong-tong-hop' WHERE name = N'Màng, xương tổng hợp';
UPDATE Service_Category SET meta = 'mang-xuong-tu-than' WHERE name = N'Màng, xương tự thân';
UPDATE Service_Category SET meta = 'nang-xoang' WHERE name = N'Nâng xoang';
UPDATE Service_Category SET meta = 'chup-ct-cone-beam' WHERE name = N'Chụp CT CONE BEAM';
UPDATE Service_Category SET meta = 'mang-huong-dan-in-so-mat' WHERE name = N'Máng Hướng Dẫn / in Sọ Mặt';
go

--Service
create proc procAddService
	@name nvarchar(100),
	@price int,
	@calUnit nvarchar(50),
	@note nvarchar(max),
	@meta varchar(max),
	@img varchar(max),
	@cateId varchar(10),
	@descrip nvarchar(max),
	@caredActor nvarchar(50)
as
begin
	declare @QuanSe INT
	set @QuanSe = (select count(*) from Service)
	if(TRIM(@meta)='') 
		begin
			set @meta = 'dich-vu-' + CONVERT(varchar(MAX), @QuanSe+1)
		end
	declare @id varchar(10)
	set @id = dbo.autoid('SE', @QuanSe+1)
	insert into Service(id,name,price,note,meta,calUnit,img,cateId,descrip,caredActor)
	values (@id,@name,@price,@note,@meta,@calUnit,@img,@cateId,@descrip,@caredActor)
end
go

--them dich vu
--1.Khám tổng quát
exec procAddService N'Khám tổng quát',500000,N'Lượt','','','','SC00000001',N'Khám tổng quát',N'Mọi bệnh nhân'
--2.Nhổ răng
exec procAddService N'Nhổ răng cửa, răng nanh', 500000,N'Cái','','','','SC00000002',N'Lấy răng cửa bị hỏng',N'Mọi bệnh nhân'
exec procAddService N'Nhổ răng cối nhỏ',600000,N'Cái','','','','SC00000002',N'Lấy răng cối bị hỏng',N'Mọi bệnh nhân'
exec procAddService N'Nhổ răng cối lớn trên',700000,N'Cái','','','','SC00000002',N'Lấy răng cối lớn trên  bị hỏng',N'Mọi bệnh nhân'
exec procAddService N'Nhổ răng cối lớn dưới',900000,N'Cái','','','','SC00000002',N'Lấy răng cối lớn dưới bị hỏng',N'Mọi bệnh nhân'
exec procAddService N'Nhổ răng cối lung lay',500000,N'Cái','','','','SC00000002',N'Lấy răng cối lung lay',N'Mọi bệnh nhân'
exec procAddService N'Nhổ chân răng vĩnh viễn',600000,N'Cái','','','','SC00000002',N'Nhổ chân răng vĩnh viễn',N'Mọi bệnh nhân'
exec procAddService N'Khâu ổ răng',500000,N'Cái','','','','SC00000002',N'Nhổ chân răng vĩnh viễn',N'Mọi bệnh nhân'
--3Tiểu phẫu thuật
exec procAddService N'Rằng khôn mọc lệch nhổ tiểu phẩu',300000,N'Cái','','','','SC00000003',N'Nhổ răng để cân bằng hàm',N'Mọi bệnh nhân'
exec procAddService N'Phẫu thuật điều chỉnh xương ổ răng',200000,N'Cái','','','','SC00000003',N'điều chỉnh xương ổ răng',N'Trên 13 tuổi'
exec procAddService N'Phẩu thuật cắt chóp',300000,N'Cái','','','','SC00000003',N'cắt chóp để cân bằng hàm',N'Trên 13 tuổi'
--4.Nha chu
exec procAddService N'Cạo vôi răng',50000,N'2 hàm','','','','SC00000004',N'Cạo các vôi trên răng',N'Mọi bệnh nhân'
exec procAddService N'Điều trị viêm nha chu không phẫu thuật',100000,N'Vùng hàm','','','','SC00000004',N'Dùng thuốc cho răng viêm nha',N'Mọi bệnh nhân'
exec procAddService N'Phẫu thuật lật vạt làm sạch',100000,N'Lần','','','','SC00000004',N'lật vạt làm sạch',N'Mọi bệnh nhân'
exec procAddService N'Cắt thắng',100000,N'Lần','','','','SC00000004',N'cắt thẳng',N'Mọi bệnh nhân'
exec procAddService N'Phẫu thuật nướu',500000,N'Răng','','','','SC00000004',N'phẩu thuật nướu bị viêm',N'Mọi bệnh nhân'
--5.Chữa răng-Nội nha
exec procAddService N'Tái tạo thân răng', 150000, N'Xoang','','','','SC00000005',N'tái tạo thân răng',N'Mọi bệnh nhân'
exec procAddService N'Trám composite xoang I, III', 2500000, 'Xoang','','','','SC00000005',N'trám composite loại 1/3',N'Mọi bệnh nhân'
exec procAddService N'Trám composite xoang II, IV, V', 1200000, 'Xoang','','','','SC00000005',N'trám composite loại 2/4',N'Mọi bệnh nhân'
exec procAddService N'Trám GIC', 1000000, N'Xoang','','','','SC00000005',N'trám GIC',N'Mọi bệnh nhân'
exec procAddService N'Trám đắp mặt, hở kẽ', 2000000, N'Cái','','','','SC00000005',N'trám đắp mặt',N'Mọi bệnh nhân'
exec procAddService N'Trám phòng ngừa', 800000, N'Cái','','','','SC00000005',N'trám phòng răng bị viêm',N'Mọi bệnh nhân'
--6Chữa tủy
exec procAddService N'Răng cửa, răng nanh', 250000,N'Cái','','','','SC00000006',N'chữa tủy răng cửa, răng nanh',N'Mọi bệnh nhân'
exec procAddService N'Răng cối nhỏ',300000,N'Cái','','','','SC00000006',N'chữa tủy răng cối nhỏ',N'Mọi bệnh nhân'
exec procAddService N'Răng cối lớn',60000,N'Cái','','','','SC00000006',N'chữa tủy răng cối lớn',N'Mọi bệnh nhân'
exec procAddService N'Chữa tủy lại(đóng thêm)',60000,N'Ống tủy','','','','SC00000006',N'chữa tủy thêm theo yêu cầu',N'Mọi bệnh nhân'
--7Phục hình tháo lắp
exec procAddService N'Phục hình tháo lắp 1 răng', 100000, N'Răng','','','','SC00000007',N'phục hình tháo lắp 1 răng',N'Mọi bệnh nhân'
exec procAddService N'Phục hình tháo lắp 1 hàm toàn hàm', 1500000, N'Hàm','','','','SC00000007',N'phục hình tháo lắp 1 hàm',N'Mọi bệnh nhân'
exec procAddService N'Phục hình tháo lắp 2 hàm toàn hàm', 3000000, N'Hàm','','','','SC00000007',N'phuc hình tháo lắp 2 hàm toàn hàm',N'Mọi bệnh nhân'
--8Sữa chữa hàm
exec procAddService N'Vá hàm', 100000, N'Hàm','','','','SC00000008',N'vá hàm',N'Mọi bệnh nhân'
exec procAddService N'Thay nền', 300000, N'Hàm','','','','SC00000008',N'thay nền cho răng hàm',N'Mọi bệnh nhân'
exec procAddService N'Đệm hàm nhựa nấu', 250000, N'Hàm','','','','SC00000008',N'đệm hàm',N'Mọi bệnh nhân'
exec procAddService N'Thêm, thay móc', 50000, N'Cái','','','','SC00000008',N'thay/thêm móc',N'Mọi bệnh nhân'
exec procAddService N'Thêm, thay răng', 50000, N'Cái','','','','SC00000008',N'thay/thêm răng giả',N'Mọi bệnh nhân'
exec procAddService N'Chữa đau', 50000, N'Lần','','','','SC00000008',N'chữa đau /buốt răng',N'Mọi bệnh nhân'
exec procAddService N'Móc dẻo', 200000, N'Cái','','','','SC00000008',N'móc dẻo',N'Mọi bệnh nhân'
exec procAddService N'Hàm dẻo', 700000, N'Cái','','','','SC00000008',N'hàm dẻo',N'Mọi bệnh nhân'
exec procAddService N'Hàm khung bộ ', 750000, N'Cái','','','','SC00000008',N'hàm khung bộ',N'Mọi bệnh nhân'
--9Điều trị tiền phục hình
exec procAddService N'Đệm hàm nhựa tự cứng (Hàm cũ)', 100000, N'Hàm','','','','SC00000009',N'đệm bằng nhữa cho hàm cũ',N'Mọi bệnh nhân'
exec procAddService N'Điều chỉnh khớp cắn (Hàm cũ)', 100000, N'Hàm','','','','SC00000009',N'điều chỉnh khớp của hàm cũ',N'Mọi bệnh nhân'

--10.Phục hình cố định
exec procAddService N'Tái tạo cùi răng (có chốt)', 1500000, N'Răng','','','','SC00000010',N'tái tạo cùi có chốt cho răng',N'Mọi bệnh nhân'
exec procAddService N'Mão, cầu răng kim loại toàn diện', 3500000, N'Răng','','','','SC00000010',N'mão, cầu răng kim loại',N'Mọi bệnh nhân'
exec procAddService N'Mão, cầu răng kim loại-sứ', 5000000, N'Răng','','','','SC00000010',N'mão, cầu răng kim loại và sứ',N'Mọi bệnh nhân'
exec procAddService N'Sứ titan', 3500000, N'Hàm','','','','SC00000010',N'sứ titan phụ thêm',N'Mọi bệnh nhân'
exec procAddService N'Hàm khung Ti (chưa bao gồm răng)', 1500000, N'hàm','','','','SC00000010',N'hàm khung titan thêm',N'Mọi bệnh nhân'
exec procAddService N'Sứ zirconia', 2500000, N'Hàm','','','','SC00000010',N'sứ zirconia thêm',N'Mọi bệnh nhân'
exec procAddService N'Sứ cercon', 3500000, N'Hàm','','','','SC00000010',N'sứ cercon thêm',N'Mọi bệnh nhân'
exec procAddService N'Điều chỉnh, gắn lại, tháo PHCĐ', 1000000, N'Răng','','','','SC00000010',N'thay đổi tích cực bằng các biện pháp phục hồi cố định',N'Mọi bệnh nhân'
exec procAddService N'Hàm tạm', 500000, N'Hàm','','','','SC00000010',N'hàm tạm thêm',N'Mọi bệnh nhân'
exec procAddService N'Mão tạm', 150000, N'Răng','','','','SC00000010',N'mão tạm thêm',N'Mọi bệnh nhân'
exec procAddService N'Cầu răng tạm', 150000, N'Răng','','','','SC00000010',N'cầu răng tạm',N'Mọi bệnh nhân'
exec procAddService N'Cùi giá đúc', 1000000, N'Cái','','','','SC00000010',N'cùi giả',N'Mọi bệnh nhân'

--11.Điều trị răng sữa
exec procAddService N'Nhổ răng sữa(tê bôi)',200000, N'Răng','','','','SC00000011',N'nhổ răng sữa bằng bôi thuốc',N'Mọi bệnh nhân'
exec procAddService N'Nhổ răng sữa(tê chích)',500000, N'Răng','','','','SC00000011',N'nhổ răng sữa bằng tiêm thuốc',N'Mọi bệnh nhân'
exec procAddService N'Trám răng sữa bằng GIC',500000, N'Răng','','','','SC00000011',N'trám răng sữa bằng GIC',N'Mọi bệnh nhân'
exec procAddService N'Trám răng sữa bằng composite',800000,N'Răng','','','','SC00000011',N'trám răng sữa bằng composite',N'Mọi bệnh nhân'
exec procAddService N'Trám dự phòng hố rãnh mặt nhai',800000, N'Răng','','','','SC00000011',N'trám dự phòng răng bị hố rãnh mặt nhai',N'Mọi bệnh nhân'
exec procAddService N'Đặt gel Fluor phòng ngừa',500000, N'Hàm','','','','SC00000011',N'Thêm gel Flour cho răng',N'Mọi bệnh nhân'
exec procAddService N'Lấy tủy chân răng sữa',200000, N'Răng','','','','SC00000011',N'lấy tủy chân của răng sữa',N'Mọi bệnh nhân'
exec procAddService N'Mão nhựa răng cửa (Strip crown)',200000, N'Răng','','','','SC00000011',N'mão nhựa Strip crown cho răng cửa',N'Mọi bệnh nhân'
exec procAddService N'Mão kim loại làm sẵn',2500000, N'Răng','','','','SC00000011',N'mão kim loại cho răng',N'Mọi bệnh nhân'
exec procAddService N'Bộ giữ khoảng tháo lắp',2500000, N'Hàm','','','','SC00000011',N'bộ giữ khung hàm',N'Mọi bệnh nhân'
exec procAddService N'Giữ khoảng cố định 1 bên',400000, N'Cái','','','','SC00000011',N'bộ giữ khung cố định 1 bên',N'Mọi bệnh nhân'
exec procAddService N'Bộ giữ khoảng cố định 2 bên',800000, N'Bộ','','','','SC00000011',N'bộ giữ khung cố định 2 bên',N'Mọi bệnh nhân'
exec procAddService N'Mặt phẳng nghiêng',500000, N'Cái','','','','SC00000011',N'mặt phẳng nghiêng',N'Mọi bệnh nhân'
exec procAddService N'Tấm chặn môi',500000, N'Cái','','','','SC00000011',N'tấm chặn môi',N'Mọi bệnh nhân'
exec procAddService N'Khí cụ đẩy môi (lip bumper)',1000000, N'Cái','','','','SC00000011',N'dụng cụ đẩy môi lip bumper',N'Mọi bệnh nhân'
exec procAddService N'Khí cụ tháo lắp điều trị cắn chéo l răng',1000000, N'Cái','','','','SC00000011',N'dụng cụ thóa lắp điều trị cần chéo 1 răng',N'Mọi bệnh nhân'
exec procAddService N'Khí cụ Quad Helix',1000000, N'Cái','','','','SC00000011',N'dụng cụ Quad Helix',N'Mọi bệnh nhân'
exec procAddService N'Tiểu phẫu',250000, N'Lần','','','','SC00000011',N'Tiểu phẩu',N'Mọi bệnh nhân'

--12.Chỉnh hình răng mặt
--Khí cụ tháo lắp
exec procAddService N'Khí cụ tháo lắp 1 hàm',1500000, N'Hàm','','','','SC00000012',N'khí cụ thóa lắp 1 hàm',N'Mọi bệnh nhân'
exec procAddService N'Khí cụ tháo lắp 2 hàm',3000000, N'Hàm','','','','SC00000012',N'khí cụ thóa lắp 2 hàm',N'Mọi bệnh nhân'
exec procAddService N'Làm lại khí cụ tháo lắp 1 hàm',300000, N'Hàm','','','','SC00000012',N'làm lại khí cụ tháo lắp 1 hàm',N'Mọi bệnh nhân'
exec procAddService N'Làm lại khí cụ tháo lắp 2 hàm',600000, N'Hàm','','','','SC00000012',N'làm lại khí cụ tháo lắp 2 hàm',N'Mọi bệnh nhân'
exec procAddService N'Khí cụ duy trì kết quả 1 hàm',300000, N'Hàm','','','','SC00000012',N'khí cụ giữ lại kết quả của 1 hàm',N'Mọi bệnh nhân'
--Khí cụ cố định
exec procAddService N'Khí cụ cố định 1 hàm',10000000, N'Hàm','','','','SC00000012',N'',N'Mọi bệnh nhân'
exec procAddService N'Khí cụ cố định 2 hàm',20000000, N'Hàm','','','','SC00000012',N'',N'Mọi bệnh nhân'
exec procAddService N'Khí cụ cố định 2 hàm trên 2 năm',26000000, N'Hàm','','','','SC00000012',N'',N'Mọi bệnh nhân'
exec procAddService N'Khí cụ cố định 2 hàm sử dụng mắc cài thế hệ mới',28000000, N'Hàm','','','','SC00000012',N'',N'Mọi bệnh nhân'
exec procAddService N'Khí cụ cố định 2 hàm sử dụng mắc cài sứ',15000000, N'Hàm','','','','SC00000012',N'',N'Mọi bệnh nhân'

--13.Nha công cộng
exec procAddService N'Máng Fluor không thuốc', 100000, N'Máng','','','','SC00000013',N'',N'Mọi bệnh nhân'

--14.Điều trị loạn năng hệ thống nhai
exec procAddService N'1 máng nhai', 500000, N'Máng','','','','SC00000014',N'',N'Mọi bệnh nhân'
exec procAddService N'Mài chỉnh khớp cắn đơn giản', 150000, N'Lần','','','','SC00000014',N'',N'Mọi bệnh nhân'
exec procAddService N'Mài chỉnh khớp cắn phức tạp', 300000, N'Lần','','','','SC00000014' ,N'',N'Mọi bệnh nhân'
--15.X-quang răng
exec procAddService N'Phim quanh chóp', 300000, N'Phim','','','','SC00000015',N'',N'Mọi bệnh nhân'
exec procAddService N'Phim toàn cảnh', 100000, N'Phim','','','','SC00000015',N'',N'Mọi bệnh nhân'


--Phục hình đơn lẻ
--{
--16Răng sứ dán
exec procAddService N'Răng sứ dán kim loại', 1500000,N'Hàm','','','','SC00000016',N'',N'Mọi bệnh nhân'
exec procAddService N'Răng sứ dán Titan', 3000000,N'Hàm','','','','SC00000016',N'',N'Mọi bệnh nhân'
exec procAddService N'Răng sứ dán Zirco', 6000000,N'Hàm','','','','SC00000016',N'',N'Mọi bệnh nhân'
exec procAddService N'Răng sứ dán Bio HPP', 8000000,N'Hàm','','','','SC00000016',N'',N'Mọi bệnh nhân'
--Giá răng sứ bắt vít trên Multi Unit gồm 2 khoản sau cộng lại:***,'',''
exec procAddService N'Trụ phục hình Multi Unit', 1500000,N'Hàm','','','','SC00000016',N'phục hình bằng trụ Multi Unit',N'Mọi bệnh nhân'
exec procAddService N'Răng sứ(Kim loại, Titan, Zirco, Bio HPP)', 1500000,N'Hàm','','','phục hình bằng Răng sứ(Kim loại, Titan, Zirco, Bio HPP)','SC00000016',N'',N'Mọi bệnh nhân'
--}

--Phục Hình bắt vít 
--{
--17Hàm phủ tháo lắp
exec procAddService N'Hàm phủ tháo lắp 2 Implant', 7000000,N'Hàm','','','','SC00000017',N'Răng composite nền nhựa cường lực 2 Implant',N'Mọi bệnh nhân'
exec procAddService N'Hàm phủ tháo lắp 3 Implant thanh bar', 7500000,N'Hàm','','','','SC00000017',N'Răng composite nền nhựa cường lực 3 Implant thanh bar',N'Mọi bệnh nhân'
exec procAddService N'Hàm phủ tháo lắp 4 Implant thanh bar', 8000000,N'Hàm','','','','SC00000017',N'Răng composite nền nhựa cường lực 4 Implant thanh bar',N'Mọi bệnh nhân'
exec procAddService N'Hàm phủ tháo lắp 5 Implant thanh bar', 9000000,N'Hàm','','','','SC00000017',N'Răng composite nền nhựa cường lực 5 Implant thanh bar',N'Mọi bệnh nhân'
exec procAddService N'Hàm phủ tháo lắp 6 Implant thanh bar', 10000000,N'Hàm','','','','SC00000017',N'Răng composite nền nhựa cường lực 6 Implant thanh bar',N'Mọi bệnh nhân'
exec procAddService N'Hàm phủ tháo lắp 3 Implant nút bấm', 7500000,N'Hàm','','','','SC00000017',N'Răng composite nền nhựa cường lực 3 Implant nút bấm',N'Mọi bệnh nhân'
exec procAddService N'Hàm phủ tháo lắp 4 Implant nút bấm', 8000000,N'Hàm','','','','SC00000017',N'Răng composite nền nhựa cường lực 4 Implant nút bấm',N'Mọi bệnh nhân'
exec procAddService N'Hàm phủ tháo lắp 5 Implant nút bấm', 9000000,N'Hàm','','','','SC00000017',N'Răng composite nền nhựa cường lực 5 Implant nút bấm',N'Mọi bệnh nhân'
exec procAddService N'Hàm phủ tháo lắp 6 Implant nút bấm', 10000000,N'Hàm','','','','SC00000017',N'Răng composite nền nhựa cường lực 6 Implant nút bấm',N'Mọi bệnh nhân'

--18Hàm cố định bắt vít
exec procAddService N'Hàm cố định bắt vít 4 Implant Sườn Titan răng nhựa', 7000000,N'Hàm','','','','SC00000018',N'Hàm cố định bắt vít HYBRID 4 Implant Sườn Titan răng nhựa',N'Mọi bệnh nhân'
exec procAddService N'Hàm cố định bắt vít 4 Implant Sườn Titan răng sứa', 15000000,N'Hàm','','','','SC00000018',N'Hàm cố định bắt vít HYBRID 4 Implant Sườn Titan răng sứ',N'Mọi bệnh nhân'
exec procAddService N'Hàm cố định bắt vít 4 Implant Sườn Zirco răng nhựa', 11000000,N'Hàm','','','','SC00000018',N'Hàm cố định bắt vít HYBRID 4 Implant Sườn Zirco răng nhựa',N'Mọi bệnh nhân'
exec procAddService N'Hàm cố định bắt vít 4 Implant Sườn Zirco răng sứa', 19000000,N'Hàm','','','','SC00000018',N'Hàm cố định bắt vít HYBRID 4 Implant Sườn Zirco răng sứ',N'Mọi bệnh nhân'
exec procAddService N'Hàm cố định bắt vít 4 Implant Sườn Bio HPP răng nhựa', 11000000,N'Hàm','','','','SC00000018',N'Hàm cố định bắt vít HYBRID 4 Implant Sườn Bio HPP răng nhựa',N'Mọi bệnh nhân'
exec procAddService N'Hàm cố định bắt vít 4 Implant Sườn Bio HPP răng sứ', 19000000,N'Hàm','','','','SC00000018',N'Hàm cố định bắt vít HYBRID 4 Implant Sườn Bio HPP răng sứ',N'Mọi bệnh nhân'
exec procAddService N'Hàm cố định bắt vít 5 Implant', 1400000,N'Hàm','','','','SC00000018',N'Hàm cố định bắt vít HYBRID 5 Implant',N'Mọi bệnh nhân'
exec procAddService N'Hàm cố định bắT vít 6 Implant Sườn Titan răng nhựa', 9000000,N'Hàm','','','','SC00000018',N'Hàm cố định bắt vít HYBRID 6 Implant Sườn Titan răng nhựa',N'Mọi bệnh nhân'
exec procAddService N'Hàm cố định bắt vít 6 Implant Sườn Titan răng sứ', 17000000,N'Hàm','','','','SC00000018',N'Hàm cố định bắt vít HYBRID 6 Implant Sườn Titan răng sứ',N'Mọi bệnh nhân'
exec procAddService N'Hàm cố định bắt vít 6 Implant Sườn Zirco răng nhựa', 13000000,N'Hàm','','','','SC00000018',N'Hàm cố định bắt vít HYBRID 6 Implant Sườn Zirco răng nhựa',N'Mọi bệnh nhân'
exec procAddService N'Hàm cố định bắt vít 6 Implant Sườn Zirco răng sứ', 21000000,N'Hàm','','','','SC00000018',N'Hàm cố định bắt vít HYBRID 6 Implant Sườn Zirco răng sứ',N'Mọi bệnh nhân'
exec procAddService N'Hàm cố định bắt vít 6 Implant Sườn Bio HPP răng nhựa', 13000000,N'Hàm','','','','SC00000018',N'Hàm cố định bắt vít HYBRID 6 Implant Sườn Bio HPP răng nhựa',N'Mọi bệnh nhân'
exec procAddService N'Hàm cố định bắt vít 6 Implant Sườn Bio HPP răng sứ', 21000000,N'Hàm','','','','SC00000018',N'Hàm cố định bắt vít HYBRID 6 Implant Sườn Bio HPP răng sứ',N'Mọi bệnh nhân'
--}
--19Phục hình tạm tức thì toàn hàm
exec procAddService N'Răng tạm toàn hàm', 2000000,N'Hàm','','','','SC00000019',N'phục hình tạm tức thì toàn hàm',N'Mọi bệnh nhân'
exec procAddService N'Cylinder', 4000000,N'Hàm','','','','SC00000019',N'phục hình tạm tức thì toàn hàm Cylinder',N'Mọi bệnh nhân'

--20Cấy ghép 1 trụ Implant
exec procAddService N'OSSTEM(KOREA)', 1500000,N'trụ Implant','','','','SC00000020',N'Cấy ghép 1 trụ Implant OSSTEM(KOREA)',N'Mọi bệnh nhân'
exec procAddService N'RITTER', 2000000,N'trụ Implant','','','','SC00000020',N'Cấy ghép 1 trụ Implant RITTER',N'Mọi bệnh nhân'
exec procAddService N'SIC(SWISS/GERMANY)', 2000000,N'trụ Implant','','','','SC00000020',N'Cấy ghép 1 trụ Implant SIC(SWISS/GERMANY)',N'Mọi bệnh nhân'
exec procAddService N'NOBEL BIOCARE (USA/SWEDEN)', 2400000,N'trụ Implant','','','','SC00000020',N'Cấy ghép 1 trụ Implant NOBEL BIOCARE (USA/SWEDEN)',N'Mọi bệnh nhân'
exec procAddService N'NOBEL ACTIVE/PARALLEL (USA/SWEDEN)', 2900000,N'trụ Implant','','','','SC00000020',N'Cấy ghép 1 trụ Implant NOBEL ACTIVE/PARALLEL (USA/SWEDEN)',N'Mọi bệnh nhân'
exec procAddService N'STRAUMANN SLActive (SWISS)', 3200000,N'trụ Implant','','','','SC00000020',N'Cấy ghép 1 trụ Implant STRAUMANN SLActive (SWISS)',N'Mọi bệnh nhân'
exec procAddService N'NOBEL TiUltra-ACTIVE/PARALLEL-(USA/SWEDEN)', 3200000,N'trụ Implant','','','','SC00000020',N'Cấy ghép 1 trụ Implant NOBEL TiUltra-ACTIVE/PARALLEL-(USA/SWEDEN)',N'Mọi bệnh nhân'
exec procAddService N'STRAUMANN BLX (SWISS)', 3500000,N'trụ Implant','','','','SC00000020',N'Cấy ghép 1 trụ Implant STRAUMANN BLX (SWISS)',N'Mọi bệnh nhân'
exec procAddService N'Implant Zygoma NOBEL (USA)', 5000000,N'trụ Implant','','','','SC00000020',N'Cấy ghép 1 trụ Implant Zygoma NOBEL (USA)',N'Mọi bệnh nhân'

-- 21Màng, xương tổng hợp
exec procAddService N'Xương khử khoáng', 500000, N'Răng','','','','SC00000021',N'Xương khử khoáng',N'Mọi bệnh nhân'
exec procAddService N'Màng Collagen 15x20mm', 400000, N'Răng','','','','SC00000021',N'Màng Collagen 15x20mm',N'Mọi bệnh nhân'
exec procAddService N'Màng Collagen 20x30mm', 500000, N'Răng','','','','SC00000021',N'Màng Collagen 20x30mm',N'Mọi bệnh nhân'
exec procAddService N'Màng Collagen 30x40mm', 700000, N'Răng','','','','SC00000021',N'Màng Collagen 30x40mm',N'Mọi bệnh nhân'
exec procAddService N'Màng Titan, PTFE', 600000, N'Răng','','','','SC00000021',N'Màng Titan',N'Mọi bệnh nhân'
exec procAddService N'Vít (Tack) neo xương, màng', 600000, N'Con','','','','SC00000021',N'Vít (Tack) neo xương, màng',N'Mọi bệnh nhân'

-- 22Màng, xương tự thân
exec procAddService N'Ghép xương tự thân', 6000000, N'Hàm','','','','SC00000022',N'Ghép xương tự thâ',N'Mọi bệnh nhân'
exec procAddService N'Mô liên kết', 5000000, N'Hàm','','','','SC00000022',N'Mô liên kết',N'Mọi bệnh nhân'
exec procAddService N'Màng PRP (tự thân)', 5000000, N'Hàm','','','','SC00000022',N'Màng PRP (tự thân)',N'Mọi bệnh nhân'
exec procAddService N'Ghép xương mào chậu', 2000000, N'Hàm','','','','SC00000022',N'Ghép xương mào chậu',N'Mọi bệnh nhân'

-- 23Nâng xoang
exec procAddService N'Nâng xoang kín', 4000000, N'Hàm','','','','SC00000023',N'Nâng xoang kín Không bao gồm xương và màng',N'Mọi bệnh nhân'
exec procAddService N'Nâng xoang hở', 6000000, N'Hàm','','','','SC00000023',N'Nâng xoang kín Không bao gồm xương và màng',N'Mọi bệnh nhân'

-- 24Chụp CT CONE BEAM
exec procAddService N'Chụp CT Cone Beam 1 hàm', 550000, N'Hàm','','','','SC00000024',N'Chụp CT Cone Beam 1 hàm',N'Mọi bệnh nhân'
exec procAddService N'Chụp CT Cone Beam 2 hàm', 900000, N'Hàm','','','','SC00000024',N'Chụp CT Cone Beam 1 hàm',N'Mọi bệnh nhân'

--25Máng Hướng Dẫn / in Sọ Mặt
exec procAddService N'Máng hướng dẫn', 3300000, N'Hàm','','','','SC00000025',N'Máng hướng dẫn',N'Mọi bệnh nhân'
exec procAddService N'In sọ mặt', 700000, N'Lần','','','','SC00000025',N'In sọ mặt',N'Mọi bệnh nhân'
go

--Material_Category
create proc procAddMaterial_Category
	@name NVARCHAR(100),
	@descrip NVARCHAR(MAX),
	@note NVARCHAR(MAX),
	@meta VARCHAR(MAX)
as
begin
	declare @QuanMC INT
	set @QuanMC = (select count(*) from Material_Category)
	if(TRIM(@meta)='') 
		begin
			set @meta = 'loai-vat-lieu-' + CONVERT(varchar(MAX), @QuanMC+1)
		end
	declare @id varchar(10)
	set @id = dbo.autoid('MC', @QuanMC+1)
	insert into Material_Category(id,name,descrip,note,meta)
	values (@id,@name,@descrip,@note,@meta)
end
go

--them Material_Category
--loại của vật liệu cố định(1)
exec procAddMaterial_Category N'Mũi cạo', N'Mũi cạo vôi', '', 'mui-cao-voi'
exec procAddMaterial_Category N'Kềm', N'Kềm nha khoa', '', 'kem'
exec procAddMaterial_Category N'Nạy', N'Nạy', '', 'nay'
exec procAddMaterial_Category N'Gương nha khoa', N'gương nha khoa', '', 'guong nha khoa'
exec procAddMaterial_Category N'Thám trâm', N'cây thám trâm', '', 'tham-tram'
exec procAddMaterial_Category N'Nạo ngà', N'Nạo ngà', '', 'nao-nga'
exec procAddMaterial_Category N'Kẹp', N'Kẹp', '', 'kep'
exec procAddMaterial_Category N'Bay trám', N'Bay trám', '', 'bay-tram'
exec procAddMaterial_Category N'Cây đo túi nú', N'Cây đo túi nú', '', 'cay-do-tui-nu'
exec procAddMaterial_Category N'Đèn', N'Đèn', '', 'den'
exec procAddMaterial_Category N'Ống chích', N'ống chích', '', 'ong-chich'

--loại vật liệu tiêu hao
	--không là thuốc
			--Chữa răng (12)
exec procAddMaterial_Category N'Composite đặc', N'composite đặc', '', 'composite-dac'
exec procAddMaterial_Category N'Composite lỏng', N'composite lỏng', '', 'composite-long'
exec procAddMaterial_Category N'Bonding', N'bonding', '', 'bonding'
exec procAddMaterial_Category N'Cọ Bond', N'Cọ Bond', '', 'co-bond'
exec procAddMaterial_Category N'Bông gòn', N'Bông gòn', '', 'bong-gon'
exec procAddMaterial_Category N'Mũi khoan', N'Mũi khoan', '', 'mui-khoan'

			--Nội nha(18)
exec procAddMaterial_Category N'Sealer trám bít', N'sealer trám bít', '', 'sealer-tram-bit'
exec procAddMaterial_Category N'Cone', N'cone', '', 'cone'
exec procAddMaterial_Category N'Chất làm sach và khử khuẩn', N'Chất làm sach và khử khuẩn', '', 'chat-lam-sach-va-khu-khuan'
exec procAddMaterial_Category N'Trâm', N'Trâm', '', 'tram'

			--Nha chu(22)
exec procAddMaterial_Category N'Spongel', N'Spongel cầm máu', '', 'spongel'
exec procAddMaterial_Category N'Mũi đánh bóng', N'mũi đánh bóng', '', 'mui-danh-bong'
exec procAddMaterial_Category N'Chổi đánh bóng', N'chổi đánh bóng ', '', 'choi-danh-bong'
exec procAddMaterial_Category N'Sò đánh bóng', N'sò đánh bóng', '', 'so-danh-bong'

			--Nhổ răng(26)
exec procAddMaterial_Category N'Chỉ khẩu', N'Chỉ khẩu', '', 'chi-khau'
exec procAddMaterial_Category N'Mũi tê', N'Mũi tê', '', 'mui-te'

			--Răng trẻ em(28)
exec procAddMaterial_Category N'Chất trám', N'chất trám', '', 'chat-tram'

	--là thuốc(29)
exec procAddMaterial_Category N'Kháng sinh', N'kháng sinh', '', 'khang-sinh'
exec procAddMaterial_Category N'Giảm đau - Hạ sốt', N'giảm đau - hạ sốt', '', 'giam-dau-ha-sot'
exec procAddMaterial_Category N'Chống viêm', N'chống viêm', '', 'chong-viem'
go

--Material
create proc procAddMaterial
	@name NVARCHAR(MAX),
	@cateId VARCHAR(10),
	@calUnit NVARCHAR(50),
	@quantity int,
	@func NVARCHAR(MAX),
	@mfgDate DATETIME,
	@meta VARCHAR(MAX),
	@img VARCHAR(MAX)
as
begin
	declare @QuanMa INT
	set @QuanMa = (select count(*) from Material)
	if(TRIM(@meta)='') 
		begin
			set @meta = 'vat-lieu-' + CONVERT(varchar(MAX), @QuanMa+1)
		end
	declare @id varchar(10)
	set @id = dbo.autoid('MA', @QuanMa+1)
	insert into Material(id,cateId,name,quantity,calUnit,func,mfgDate,meta,img)
	values (@id,@cateId,@name,@quantity,@calUnit,@func,@mfgDate,@meta,@img)
end
go

--them material
--select * from material
--Vật liệu cố định 
exec procAddMaterial N'Mũi cạo vôi', 'MC00000001', N'Cái', 10, N'Cạo vôi răng', '2024-09-01','mui-cao-voi',''
exec procAddMaterial N'Kềm 150', 'MC00000002', N'Cái', 10, N'Nhổ răng', '2024-09-01','kiem-150',''
exec procAddMaterial N'Kềm 151', 'MC00000002', N'Cái', 10, N'Nhổ răng', '2024-09-01','kiem-151',''
exec procAddMaterial N'Kềm 50', 'MC00000002', N'Cái', 10, N'Nhổ răng', '2024-09-01','kiem-50',''
exec procAddMaterial N'Kềm 51', 'MC00000002', N'Cái', 10, N'Nhổ răng', '2024-09-01','kiem-51',''
exec procAddMaterial N'Nạy 1', 'MC00000003', N'Cái', 12, N'Nạy răng', '2024-09-01','nay-1',''
exec procAddMaterial N'Nạy 2', 'MC00000003', N'Cái', 12, N'Nạy răng', '2024-09-01','nay-2',''
exec procAddMaterial N'Nạy 3', 'MC00000003', N'Cái', 12, N'Nạy răng', '2024-09-01','nay-3',''
exec procAddMaterial N'Gương nha khoa', 'MC00000004', N'Cái', 6, N'Soi răng', '2024-09-01','guong-nha-khoa',''
exec procAddMaterial N'Thám trâm', 'MC00000005', N'Cái', 9, N'Định vị tủy răng', '2024-09-01','tham-tram',''
exec procAddMaterial N'Nạo ngà', 'MC00000006', N'Cái', 21, N'lấy mô ngà sâu, tủy răng và xi-măng trám tạm', '2024-09-01','nao-nga',''
exec procAddMaterial N'Kẹp', 'MC00000007', N'Cái', 8, N'gắp các vật liệu nha khoa hỗ trợ trong các thủ thuật', '2024-09-01','kep',''
exec procAddMaterial N'Bay trám', 'MC00000008', N'Cái', 8, N'công cụ sử dụng để thực hiện quá trình trám răng hoặc điều trị nha khoa', '2024-09-01','kep',''
exec procAddMaterial N'Cây đo túi nú', 'MC00000009', N'Cái', 8, N'dùng trong quá trình thăm khám nha chu, đo túi nướu', '2024-09-01','bay-tram',''
exec procAddMaterial N'Đèn quang trùng hợp', 'MC00000010', N'Cái', 8, N'hỗ trợ các quá trình như: trám răng, tạo cầu, hay lấp đầy khoảng trống giữa các răng', '2024-09-01','cay-do-tui-nu',''
exec procAddMaterial N'Ống chích sắt', 'MC00000011', N'Cái', 8, N'Dùng để hút chất lỏng, nước bọt', '2024-09-01','ong-chich-sat',''

--Vật liệu tiêu hao
	--Không phải thuốc
		--Chữa răng
exec procAddMaterial N'Composite đặc màu A2', 'MC00000012', N'liều', 2, N'tái tạo và phục hình răng', '2024-09-01','composite-dac-mau-a2',''
exec procAddMaterial N'Composite đặc màu A3', 'MC00000012', N'liều', 2, N'tái tạo và phục hình răng', '2024-09-01','composite-dac-mau-a3',''
exec procAddMaterial N'Composite đặc màu A3.5', 'MC00000012', N'liều', 2, N'tái tạo và phục hình răng', '2024-09-01','composite-dac-mau-a3.5',''
exec procAddMaterial N'Composite đặc màu A4', 'MC00000012', N'liều', 2, N'tái tạo và phục hình răng', '2024-09-01','composite-dac-mau-a4',''
exec procAddMaterial N'Composite đặc màu ngà', 'MC00000012', N'liều', 2, N'tái tạo và phục hình răng', '2024-09-01','composite-dac-mau-nga',''
exec procAddMaterial N'Composite đặc màu men tự nhiên', 'MC00000012', N'liều', 2, N'tái tạo và phục hình răng', '2024-09-01','composite-dac-mau-men-tu-nhien',''
exec procAddMaterial N'Composite lỏng màu A2', 'MC00000013', N'liều', 2, N'cải thiện các khuyết điểm trên men răng hoặc trám lót', '2024-09-01','composite-long-mau-a2',''
exec procAddMaterial N'Composite lỏng màu A3', 'MC00000013', N'liều', 2, N'cải thiện các khuyết điểm trên men răng hoặc trám lót', '2024-09-01','composite-long-mau-a3',''
exec procAddMaterial N'Composite lỏng màu A3.5', 'MC00000013', N'liều', 2, N'cải thiện các khuyết điểm trên men răng hoặc trám lót', '2024-09-01','composite-long-mau-a3.5',''
exec procAddMaterial N'Composite lỏng màu A4', 'MC00000013', N'liều', 2, N'cải thiện các khuyết điểm trên men răng hoặc trám lót', '2024-09-01','composite-long-mau-a4',''
exec procAddMaterial N'Composite lỏng màu ngà', 'MC00000013', N'liều', 2, N'cải thiện các khuyết điểm trên men răng hoặc trám lót', '2024-09-01','composite-long-mau-nga',''
exec procAddMaterial N'Composite lỏng màu men tự nhiên', 'MC00000013', N'liều', 2, N'cải thiện các khuyết điểm trên men răng hoặc trám lót', '2024-09-01','composite-long-mau-men-tu-nhien',''
exec procAddMaterial N'Bonding', 'MC00000014', N'chai', 5, N'Dán tuyệt vời trên men và ngà', '2024-09-01','boding',''
exec procAddMaterial N'Cọ bond', 'MC00000015', N'hộp', 5, N'dùng để bôi keo gắn composit, axit etching, thuốc tẩy trắng răng', '2024-09-01','co-bond',''
exec procAddMaterial N'Bông gòn 1kg', 'MC00000016', N'cuộn', 50, N'cô lập răng khi trám, gắn răng hoặc tẩy trắng hoặc cách ly cả cung răng', '2024-09-01','bong-gon-1-kg',''
exec procAddMaterial N'Mũi khoan kim cương', 'MC00000017', N'mũi', 5, N'trám răng, chữa tủy, phục hình', '2024-09-01','mui-khoan-kim-cuong',''
		
		--Nội nha 
exec procAddMaterial N'Sealer trám bít', 'MC00000018', N'mũi', 5, N'phóng thích flour, hỗ trợ phòng ngừa sâu răng', '2024-09-01','sealer-tram-bit',''
exec procAddMaterial N'Cone giấy', 'MC00000019', N'hộp', 15, N'Thấm hút hoàn toàn đến hết chiều dài ống tủy', '2024-09-01','cone-giay',''
exec procAddMaterial N'Cone chính', 'MC00000019', N'hộp', 15, N' dùng trám bít ống Vạch đánh dấu chiều dài giúp dễ dàng kiểm soát chóp chân răng', '2024-09-01','cone-chinh',''
exec procAddMaterial N'NaOCL', 'MC00000020', N'hộp', 15, N'làm sach và khử khuẩn', '2024-09-01','naocl',''
exec procAddMaterial N'Nacl', 'MC00000020', N'hộp', 15, N'làm sach và khử khuẩn', '2024-09-01','nacl',''
exec procAddMaterial N'EDTA', 'MC00000020', N'hộp', 15, N'làm sach và khử khuẩn', '2024-09-01','edta',''
exec procAddMaterial N'Trâm tay', 'MC00000021', N'cây', 5, N'giúp nâng cao hiệu quả làm sạch và tạo hình ống tủy', '2024-09-01','tram-tay',''
exec procAddMaterial N'Trâm máy', 'MC00000021', N'cây', 5, N'giúp nâng cao hiệu quả làm sạch và tạo hình ống tủy', '2024-09-01','tram-may',''
		
		--Nha chu
exec procAddMaterial N'Spongel', 'MC00000022', N'hộp', 5, N'tăng cường khả năng làm đông máu của cơ thể, lấp đầy và che kín lỗ răng sau khi nhổ, hỗ trợ sát trùng, tiêu diệt vi khuẩn ', '2024-09-01','spongel',''
exec procAddMaterial N'Mũi đánh bóng', 'MC00000023', N'cây', 40, N'đánh bóng bề mặt men răng và các phục hồi, tái tạo nét thẩm mỹ và chức năng cho răng', '2024-09-01','mui-danh-bong',''
exec procAddMaterial N'Chổi đánh bóng', 'MC00000024', N'cây', 45, N'đánh và mài bóng răng cho bệnh nhân sau trám', '2024-09-01','choi-danh-bong',''
exec procAddMaterial N'Sò đánh bóng', 'MC00000025', N'hộp', 5, N'hỗ trợ đánh bóng sau trám răng cao cấp', '2024-09-01','so-danh-bong',''

		--Nhổ răng
exec procAddMaterial N'Chỉ khâu', 'MC00000026', N'hộp', 45, N'đảm bảo vết thương được đóng chặt chẽ, ngăn vi khuẩn và tác nhân gây nhiễm trùng xâm nhập', '2024-09-01','chi-khau',''
exec procAddMaterial N'Thuốc tê', 'MC00000027', N'hộp', 65, N'Gây tê tại chỗ và gây tê vùng cho người lớn và trẻ em', '2024-09-01','thuoc-te',''

		--Răng trẻ em
exec procAddMaterial N'formocresol', 'MC00000028', N'hộp', 15, N' sử dụng chủ yếu trong quá trình điều trị tủy để làm sạch và khử trùng ống tủy', '2024-09-01','formocresol',''
exec procAddMaterial N'ZnO', 'MC00000028', N'hộp', 6, N'xi măng trám bít tạm thời', '2024-09-01','zno',''
exec procAddMaterial N'MTA', 'MC00000028', N'hộp', 5, N'xi măng sinh học có khả năng kích thích sự hình thành xương và mô răng', '2024-09-01','mta',''
exec procAddMaterial N'GIC', 'MC00000028', N'hộp', 5, N'xi măng có khả năng giải phóng fluoride, giúp bảo vệ răng khỏi sâu răng', '2024-09-01','gic',''



--Là thuốc
exec procAddMaterial N'Amoxicillin 500mg', 'MC00000029',N'viên', 500, N'thuốc kháng sinh tạm thời', '2024-09-01','amoxicillin-500-mg',''
exec procAddMaterial N'Amoxicillin 625mg', 'MC00000029',N'viên', 350, N'thuốc kháng sinh tạm thời', '2024-09-01','amoxicillin-625-mg',''
exec procAddMaterial N'Amoxicillin 875mg', 'MC00000029',N'viên', 500, N'thuốc kháng sinh tạm thời', '2024-09-01','amoxicillin-875-mg',''
exec procAddMaterial N'Amoxicillin 1000mg', 'MC00000029',N'viên', 300, N'thuốc kháng sinh tạm thời', '2024-09-01','amoxicillin-1000-mg',''
exec procAddMaterial N'Metronidazole 200mg', 'MC00000029',N'viên', 500, N'thuốc kháng sinh tạm thời', '2024-09-01','metronidazole-200-mg',''
exec procAddMaterial N'Metronidazole 500mg', 'MC00000029',N'viên', 400, N'thuốc kháng sinh tạm thời', '2024-09-01','metronidazole-500-mg',''
exec procAddMaterial N'Cephalexin 500mg', 'MC00000029',N'viên', 500, N'thuốc kháng sinh tạm thời', '2024-09-01','cephalexin-500-mg',''
exec procAddMaterial N'Paracetamol 500mg', 'MC00000030',N'viên', 800, N'thuốc giảm đau,hạ sốt tạm thời', '2024-09-01','paracetamol-500-mg',''
exec procAddMaterial N'Aspirin 100mg', 'MC00000030',N'viên', 500, N'thuốc giảm đau,hạ sốt tạm thời', '2024-09-01','aspirin-100-mg',''
exec procAddMaterial N'Ibuprofen 400mg', 'MC00000030',N'viên', 600, N'thuốc giảm đau,hạ sốt tạm thời', '2024-09-01','ibuprofen-500-mg',''
exec procAddMaterial N'Naproxen 500mg', 'MC00000030',N'viên', 500, N'thuốc giảm đau,hạ sốt tạm thời', '2024-09-01','naproxem-500-mg',''
exec procAddMaterial N'Dexamethason 0.5mg', 'MC00000031',N'ống', 500, N'thuốc chống viêm tạm thời', '2024-09-01','dexametahson-0.5-mg',''
go

--ConsumableMaterial
create proc procAddConsumableMaterial
  @expDate DATETIME,
  @meta NVARCHAR(MAX),
  @id VARCHAR(10)
AS
begin
  declare @QuanCO INT
	set @QuanCO = (select count(*) from ConsumableMaterial)
	if(TRIM(@meta)='') 
		begin
			set @meta = 'vat-lieu-tieu-hao-' + CONVERT(varchar(MAX), @QuanCO+1)
		end
  insert into ConsumableMaterial(id,expDate,meta)
  VALUES (@id,@expDate,@meta)
end
go

--them vat lieu tieu hao
--Không phải thuốc
		--Chữa răng
exec procAddConsumableMaterial '2027-09-01', '', 'MA00000017'
exec procAddConsumableMaterial '2027-09-01', '', 'MA00000018'
exec procAddConsumableMaterial '2027-09-01', '', 'MA00000019'
exec procAddConsumableMaterial '2027-09-01', '', 'MA00000020'
exec procAddConsumableMaterial '2027-09-01', '', 'MA00000021'
exec procAddConsumableMaterial '2027-09-01', '', 'MA00000022'
exec procAddConsumableMaterial '2027-09-01', '', 'MA00000023'
exec procAddConsumableMaterial '2027-09-01', '', 'MA00000024'
exec procAddConsumableMaterial '2027-09-01', '', 'MA00000025'
exec procAddConsumableMaterial '2027-09-01', '', 'MA00000026'
exec procAddConsumableMaterial '2027-09-01', '', 'MA00000027'
exec procAddConsumableMaterial '2027-09-01', '', 'MA00000028'
exec procAddConsumableMaterial '2027-09-01', '', 'MA00000029'
exec procAddConsumableMaterial '2026-09-01', '', 'MA00000030'
exec procAddConsumableMaterial '2027-09-01', '', 'MA00000031'
exec procAddConsumableMaterial '2027-09-01', '', 'MA00000032'

--Nội nha 
exec procAddConsumableMaterial '2027-09-01', '', 'MA00000033'
exec procAddConsumableMaterial '2025-03-01', '', 'MA00000034'
exec procAddConsumableMaterial '2025-03-01', '', 'MA00000035'
exec procAddConsumableMaterial '2024-10-01', '', 'MA00000036'
exec procAddConsumableMaterial '2024-10-01', '', 'MA00000037'
exec procAddConsumableMaterial '2024-10-01', '', 'MA00000038'
exec procAddConsumableMaterial '2032-09-01', '', 'MA00000039'
exec procAddConsumableMaterial '2029-09-01', '', 'MA00000040'

--Nha chu
exec procAddConsumableMaterial '2026-03-01', '', 'MA00000041'
exec procAddConsumableMaterial '2029-09-01', '', 'MA00000042'
exec procAddConsumableMaterial '2029-09-01', '', 'MA00000043'
exec procAddConsumableMaterial '2029-09-01', '', 'MA00000044'

--Nhổ răng
exec procAddConsumableMaterial '2027-09-01', '', 'MA00000045'
exec procAddConsumableMaterial '2029-09-01', '', 'MA00000046'

--Răng trẻ em
exec procAddConsumableMaterial '2029-09-01', '', 'MA00000047'
exec procAddConsumableMaterial '2029-09-01', '', 'MA00000048'
exec procAddConsumableMaterial '2029-09-01', '', 'MA00000049'
exec procAddConsumableMaterial '2029-09-01', '', 'MA00000050'

--Là thuốc
exec procAddConsumableMaterial '2029-09-01', '', 'MA00000051'
exec procAddConsumableMaterial '2029-09-01', '', 'MA00000052'
exec procAddConsumableMaterial '2029-09-01', '', 'MA00000053'
exec procAddConsumableMaterial '2029-09-01', '', 'MA00000054'
exec procAddConsumableMaterial '2029-09-01', '', 'MA00000055'
exec procAddConsumableMaterial '2029-09-01', '', 'MA00000056'
exec procAddConsumableMaterial '2029-09-01', '', 'MA00000057'
exec procAddConsumableMaterial '2029-09-01', '', 'MA00000058'
exec procAddConsumableMaterial '2029-09-01', '', 'MA00000059'
exec procAddConsumableMaterial '2029-09-01', '', 'MA00000060'
exec procAddConsumableMaterial '2029-09-01', '', 'MA00000061'
exec procAddConsumableMaterial '2029-09-01', '', 'MA00000062'
go

--Ingredient và Ingredient_ConsumableMaterial
create proc procAddIngredient
  @name NVARCHAR(100),
  @meta NVARCHAR(MAX)
AS
BEGIN
  declare @QuanIN INT
	set @QuanIN = (select count(*) from Ingredient)
	if(TRIM(@meta)='') 
		begin
			set @meta = 'thanh-phan-' + CONVERT(varchar(MAX), @QuanIN+1)
		end
  declare @id varchar(10)
	set @id = dbo.autoid('IN', @QuanIN+1)
  insert into Ingredient(id,name,meta)
  VALUES (@id,@name,@meta)
end
go

--Ingredient_ConsumableMaterial
create proc procAddIngredient_ConsumableMaterial
  @ingreID VARCHAR(10),
  @consumID VARCHAR(10)
AS
BEGIN
  INSERT into Ingredient_ConsumableMaterial(ingreId,consumId)
  VALUES (@ingreID,@consumID)
END
GO

--them ingredient va Ingredient_ConsumableMaterial
------------------------------------------------------01
exec procAddIngredient N'Bisphenol A glycidyl methacrylate (BISGMA)',''
exec procAddIngredient N'urethane dimethacrylate (UDMA)',''
exec procAddIngredient N'polyceram bán tinh thể (PEX)',''
exec procAddIngredient N'nhựa silica',''

exec procAddIngredient_ConsumableMaterial 'IN00000001','MA00000017'
exec procAddIngredient_ConsumableMaterial 'IN00000001','MA00000018'
exec procAddIngredient_ConsumableMaterial 'IN00000001','MA00000019'
exec procAddIngredient_ConsumableMaterial 'IN00000001','MA00000020'
exec procAddIngredient_ConsumableMaterial 'IN00000001','MA00000021'
exec procAddIngredient_ConsumableMaterial 'IN00000001','MA00000022'

exec procAddIngredient_ConsumableMaterial 'IN00000002','MA00000017'
exec procAddIngredient_ConsumableMaterial 'IN00000002','MA00000018'
exec procAddIngredient_ConsumableMaterial 'IN00000002','MA00000019'
exec procAddIngredient_ConsumableMaterial 'IN00000002','MA00000020'
exec procAddIngredient_ConsumableMaterial 'IN00000002','MA00000021'
exec procAddIngredient_ConsumableMaterial 'IN00000002','MA00000022'

exec procAddIngredient_ConsumableMaterial 'IN00000003','MA00000017'
exec procAddIngredient_ConsumableMaterial 'IN00000003','MA00000018'
exec procAddIngredient_ConsumableMaterial 'IN00000003','MA00000019'
exec procAddIngredient_ConsumableMaterial 'IN00000003','MA00000020'
exec procAddIngredient_ConsumableMaterial 'IN00000003','MA00000021'
exec procAddIngredient_ConsumableMaterial 'IN00000003','MA00000022'

exec procAddIngredient_ConsumableMaterial 'IN00000004','MA00000017'
exec procAddIngredient_ConsumableMaterial 'IN00000004','MA00000018'
exec procAddIngredient_ConsumableMaterial 'IN00000004','MA00000019'
exec procAddIngredient_ConsumableMaterial 'IN00000004','MA00000020'
exec procAddIngredient_ConsumableMaterial 'IN00000004','MA00000021'
exec procAddIngredient_ConsumableMaterial 'IN00000004','MA00000022'

------------------------------------------------------05

exec procAddIngredient 'silicon dioxide',''
exec procAddIngredient 'aluminium oxide',''
exec procAddIngredient 'barium',''
exec procAddIngredient 'zirconium oxide',''
exec procAddIngredient 'borosilicate',''
exec procAddIngredient 'barium aluminium silicate glasses',''
exec procAddIngredient N'chất nối (silane)',''

exec procAddIngredient_ConsumableMaterial 'IN00000002','MA00000023'
exec procAddIngredient_ConsumableMaterial 'IN00000002','MA00000024'
exec procAddIngredient_ConsumableMaterial 'IN00000002','MA00000025'
exec procAddIngredient_ConsumableMaterial 'IN00000002','MA00000026'
exec procAddIngredient_ConsumableMaterial 'IN00000002','MA00000027'
exec procAddIngredient_ConsumableMaterial 'IN00000002','MA00000028'

exec procAddIngredient_ConsumableMaterial 'IN00000005','MA00000023'
exec procAddIngredient_ConsumableMaterial 'IN00000005','MA00000024'
exec procAddIngredient_ConsumableMaterial 'IN00000005','MA00000025'
exec procAddIngredient_ConsumableMaterial 'IN00000005','MA00000026'
exec procAddIngredient_ConsumableMaterial 'IN00000005','MA00000027'
exec procAddIngredient_ConsumableMaterial 'IN00000005','MA00000028'

exec procAddIngredient_ConsumableMaterial 'IN00000006','MA00000023'
exec procAddIngredient_ConsumableMaterial 'IN00000006','MA00000024'
exec procAddIngredient_ConsumableMaterial 'IN00000006','MA00000025'
exec procAddIngredient_ConsumableMaterial 'IN00000006','MA00000026'
exec procAddIngredient_ConsumableMaterial 'IN00000006','MA00000027'
exec procAddIngredient_ConsumableMaterial 'IN00000006','MA00000028'

exec procAddIngredient_ConsumableMaterial 'IN00000007','MA00000023'
exec procAddIngredient_ConsumableMaterial 'IN00000007','MA00000024'
exec procAddIngredient_ConsumableMaterial 'IN00000007','MA00000025'
exec procAddIngredient_ConsumableMaterial 'IN00000007','MA00000026'
exec procAddIngredient_ConsumableMaterial 'IN00000007','MA00000027'
exec procAddIngredient_ConsumableMaterial 'IN00000007','MA00000028'

exec procAddIngredient_ConsumableMaterial 'IN00000008','MA00000023'
exec procAddIngredient_ConsumableMaterial 'IN00000008','MA00000024'
exec procAddIngredient_ConsumableMaterial 'IN00000008','MA00000025'
exec procAddIngredient_ConsumableMaterial 'IN00000008','MA00000026'
exec procAddIngredient_ConsumableMaterial 'IN00000008','MA00000027'
exec procAddIngredient_ConsumableMaterial 'IN00000008','MA00000028'

exec procAddIngredient_ConsumableMaterial 'IN00000009','MA00000023'
exec procAddIngredient_ConsumableMaterial 'IN00000009','MA00000024'
exec procAddIngredient_ConsumableMaterial 'IN00000009','MA00000025'
exec procAddIngredient_ConsumableMaterial 'IN00000009','MA00000026'
exec procAddIngredient_ConsumableMaterial 'IN00000009','MA00000027'
exec procAddIngredient_ConsumableMaterial 'IN00000009','MA00000028'

exec procAddIngredient_ConsumableMaterial 'IN00000010','MA00000023'
exec procAddIngredient_ConsumableMaterial 'IN00000010','MA00000024'
exec procAddIngredient_ConsumableMaterial 'IN00000010','MA00000025'
exec procAddIngredient_ConsumableMaterial 'IN00000010','MA00000026'
exec procAddIngredient_ConsumableMaterial 'IN00000010','MA00000027'
exec procAddIngredient_ConsumableMaterial 'IN00000010','MA00000028'

exec procAddIngredient_ConsumableMaterial 'IN00000011','MA00000023'
exec procAddIngredient_ConsumableMaterial 'IN00000011','MA00000024'
exec procAddIngredient_ConsumableMaterial 'IN00000011','MA00000025'
exec procAddIngredient_ConsumableMaterial 'IN00000011','MA00000026'
exec procAddIngredient_ConsumableMaterial 'IN00000011','MA00000027'
exec procAddIngredient_ConsumableMaterial 'IN00000011','MA00000028'

exec procAddIngredient_ConsumableMaterial 'IN00000008','MA00000048'

------------------------------------------------------12

exec procAddIngredient 'ethanol',''
exec procAddIngredient 'monomer',''
exec procAddIngredient N'hạt độn nano',''
exec procAddIngredient N'Fluor',''

exec procAddIngredient_ConsumableMaterial 'IN00000012','MA00000029'
exec procAddIngredient_ConsumableMaterial 'IN00000013','MA00000029'
exec procAddIngredient_ConsumableMaterial 'IN00000014','MA00000029'
exec procAddIngredient_ConsumableMaterial 'IN00000015','MA00000029'

------------------------------------------------------16

exec procAddIngredient N'PVC',''
exec procAddIngredient N'bông tự nhiên',''

exec procAddIngredient_ConsumableMaterial 'IN00000016','MA00000030'
exec procAddIngredient_ConsumableMaterial 'IN00000017','MA00000030'

exec procAddIngredient_ConsumableMaterial 'IN00000017','MA00000031'


------------------------------------------------------18

exec procAddIngredient N'thép không gỉ',''
exec procAddIngredient N'bột kim cương',''

exec procAddIngredient_ConsumableMaterial 'IN00000018','MA00000032'
exec procAddIngredient_ConsumableMaterial 'IN00000019','MA00000032'

exec procAddIngredient_ConsumableMaterial 'IN00000018','MA00000039'
exec procAddIngredient_ConsumableMaterial 'IN00000018','MA00000040'

------------------------------------------------------20

exec procAddIngredient N'formaldehyde',''
exec procAddIngredient N'iodoform',''
exec procAddIngredient N'hydrocortisone',''
exec procAddIngredient N'prednisolone',''

exec procAddIngredient_ConsumableMaterial 'IN00000020','MA00000033'
exec procAddIngredient_ConsumableMaterial 'IN00000021','MA00000033'
exec procAddIngredient_ConsumableMaterial 'IN00000022','MA00000033'
exec procAddIngredient_ConsumableMaterial 'IN00000023','MA00000033'

------------------------------------------------------24


exec procAddIngredient N'cone gutta',''
exec procAddIngredient N'cone GGT',''

exec procAddIngredient_ConsumableMaterial 'IN00000024','MA00000034'
exec procAddIngredient_ConsumableMaterial 'IN00000025','MA00000035'

------------------------------------------------------26

exec procAddIngredient N'NaOCl',''
exec procAddIngredient N'NaCl',''
exec procAddIngredient N'[CH2N (CH2CO2H)2]2',''

exec procAddIngredient_ConsumableMaterial 'IN00000026','MA00000036'
exec procAddIngredient_ConsumableMaterial 'IN00000027','MA00000037'
exec procAddIngredient_ConsumableMaterial 'IN00000028','MA00000038'

------------------------------------------------------29

exec procAddIngredient N'Bông Xốp Collagen',''
exec procAddIngredient N'Gelatin',''
exec procAddIngredient N'Colloidal Bạc',''

exec procAddIngredient_ConsumableMaterial 'IN00000029','MA00000041'
exec procAddIngredient_ConsumableMaterial 'IN00000030','MA00000041'
exec procAddIngredient_ConsumableMaterial 'IN00000031','MA00000041'

------------------------------------------------------32

exec procAddIngredient N'nhựa PTE',''
exec procAddIngredient N'Amalgam',''
exec procAddIngredient N'Composite',''
exec procAddIngredient N'sứ mũi đánh bóng',''

exec procAddIngredient_ConsumableMaterial 'IN00000032','MA00000042'
exec procAddIngredient_ConsumableMaterial 'IN00000033','MA00000042'
exec procAddIngredient_ConsumableMaterial 'IN00000034','MA00000042'
exec procAddIngredient_ConsumableMaterial 'IN00000035','MA00000042'

------------------------------------------------------36

exec procAddIngredient N'sợi cước y tế ',''

exec procAddIngredient_ConsumableMaterial 'IN00000036','MA00000043'

------------------------------------------------------37

exec procAddIngredient N'canxi cacbonat',''
exec procAddIngredient N'glycerin',''
exec procAddIngredient N'natri hydroxymethyl cellulose',''

exec procAddIngredient_ConsumableMaterial 'IN00000037','MA00000044'
exec procAddIngredient_ConsumableMaterial 'IN00000038','MA00000044'
exec procAddIngredient_ConsumableMaterial 'IN00000039','MA00000044'

exec procAddIngredient_ConsumableMaterial 'IN00000038','MA00000047'

------------------------------------------------------40

exec procAddIngredient N'nilon',''

exec procAddIngredient_ConsumableMaterial 'IN00000040','MA00000045'

------------------------------------------------------41

exec procAddIngredient N'lidocaine hydrochloride',''
exec procAddIngredient N'epinephrine',''
exec procAddIngredient N'thuốc co mạch',''

exec procAddIngredient_ConsumableMaterial 'IN00000041','MA00000046'
exec procAddIngredient_ConsumableMaterial 'IN00000042','MA00000046'
exec procAddIngredient_ConsumableMaterial 'IN00000043','MA00000046'

------------------------------------------------------44

exec procAddIngredient N'Formaldehyde USP',''
exec procAddIngredient N'Cresol USP',''

exec procAddIngredient_ConsumableMaterial 'IN00000044','MA00000047'
exec procAddIngredient_ConsumableMaterial 'IN00000045','MA00000047'

------------------------------------------------------46

exec procAddIngredient N'oxide tricalcium',''
exec procAddIngredient N'oxide bismute',''
exec procAddIngredient N'tricalcium aluminate',''
exec procAddIngredient N'tricalcium silicate',''
exec procAddIngredient N'oxide silicate',''

exec procAddIngredient_ConsumableMaterial 'IN00000046','MA00000049'
exec procAddIngredient_ConsumableMaterial 'IN00000047','MA00000049'
exec procAddIngredient_ConsumableMaterial 'IN00000048','MA00000049'
exec procAddIngredient_ConsumableMaterial 'IN00000049','MA00000049'
exec procAddIngredient_ConsumableMaterial 'IN00000050','MA00000049'

------------------------------------------------------51

exec procAddIngredient N'axit acrylic',''
exec procAddIngredient N'bột thủy tinh mịn',''

exec procAddIngredient_ConsumableMaterial 'IN00000051','MA00000050'
exec procAddIngredient_ConsumableMaterial 'IN00000052','MA00000050'

------------------------------------------------------53

exec procAddIngredient N'amoxicillin trihydrate',''

exec procAddIngredient_ConsumableMaterial 'IN00000053','MA00000051'
exec procAddIngredient_ConsumableMaterial 'IN00000053','MA00000052'
exec procAddIngredient_ConsumableMaterial 'IN00000053','MA00000053'
exec procAddIngredient_ConsumableMaterial 'IN00000053','MA00000054'

------------------------------------------------------54

exec procAddIngredient N'metronidazole',''

exec procAddIngredient_ConsumableMaterial 'IN00000054','MA00000055'
exec procAddIngredient_ConsumableMaterial 'IN00000054','MA00000056'

------------------------------------------------------55

exec procAddIngredient N'cephalexin',''

exec procAddIngredient_ConsumableMaterial 'IN00000055','MA00000057'

------------------------------------------------------56

exec procAddIngredient N'acetaminophen',''

exec procAddIngredient_ConsumableMaterial 'IN00000056','MA00000058'

------------------------------------------------------57

exec procAddIngredient N'acid acetylsalicylic (ASA)',''

exec procAddIngredient_ConsumableMaterial 'IN00000057','MA00000059'

------------------------------------------------------58

exec procAddIngredient N'ibuprofen',''

exec procAddIngredient_ConsumableMaterial 'IN00000058','MA00000060'

------------------------------------------------------59

exec procAddIngredient N'naproxem',''

exec procAddIngredient_ConsumableMaterial 'IN00000059','MA00000061'

------------------------------------------------------60

exec procAddIngredient N'dexamethason',''

exec procAddIngredient_ConsumableMaterial 'IN00000060','MA00000062'
go
------------------------------------------------------

--FixedMaterial
create proc procAddFixedMaterial
  @meta NVARCHAR(MAX),
  @id VARCHAR(10)
AS
begin
  declare @QuanFI INT
	set @QuanFI = (select count(*) from FixedMaterial)
	if(TRIM(@meta)='') 
		begin
			set @meta = 'vat-lieu-co-dinh-' + CONVERT(varchar(MAX), @QuanFI+1)
		end
  insert into FixedMaterial(id,meta)
  VALUES (@id,@meta)
end
go

--them FixedMaterial
exec procAddFixedMaterial '','MA00000001'
exec procAddFixedMaterial '','MA00000002'
exec procAddFixedMaterial '','MA00000003'
exec procAddFixedMaterial '','MA00000004'
exec procAddFixedMaterial '','MA00000005'
exec procAddFixedMaterial '','MA00000006'
exec procAddFixedMaterial '','MA00000007'
exec procAddFixedMaterial '','MA00000008'
exec procAddFixedMaterial '','MA00000009'
exec procAddFixedMaterial '','MA00000010'
exec procAddFixedMaterial '','MA00000011'
go



--Medicine
create proc procAddMedicine
  @id VARCHAR(10),
  @ins NVARCHAR(MAX),
  @price int,
  @caredActor NVARCHAR(MAX),
  @meta NVARCHAR(MAX)
AS
begin
  declare @QuanME INT
	set @QuanME = (select count(*) from Medicine)
	if(TRIM(@meta)='') 
		begin
			set @meta = 'thuoc-' + CONVERT(varchar(MAX), @QuanME+1)
		end
  insert into Medicine(id,instruction,price,caredActor,meta)
  VALUES (@id,@ins,@price,@caredActor,@meta)
end
go

--them medicine
exec procAddMedicine 'MA00000051'
,N'Amoxicillin dạng trihydrat chỉ dùng đường uống, Amoxicillin dạng muối natri chỉ dùng đường tiêm. Hấp thu Amoxicillin không bị ảnh hưởng bởi thức ăn trong dạ dày, do đó có thể uống trước hoặc sau bữa ăn. Bột pha hỗn dịch khi dùng có thể trộn với sữa, nước quả, nước và uống ngay lập tức sau khi trộn.'
,1000
,N'Thuốc Amoxicillin chống chỉ định trong các trường hợp sau:Quá mẫn với hoạt chất, với bất kỳ Penicilin nào hoặc với bất kỳ thành phần nào của sản phẩm thuốc.Tiền sử có phản ứng quá mẫn tức thì nghiêm trọng (ví dụ như phản vệ) với một tác nhân beta-lactam khác (ví dụ như Cephalosporin, Carbapenem hoặc Monobactam).'
,'amoxicillin-500mg' 
exec procAddMedicine 'MA00000052'
,N'Amoxicillin dạng trihydrat chỉ dùng đường uống, Amoxicillin dạng muối natri chỉ dùng đường tiêm. Hấp thu Amoxicillin không bị ảnh hưởng bởi thức ăn trong dạ dày, do đó có thể uống trước hoặc sau bữa ăn. Bột pha hỗn dịch khi dùng có thể trộn với sữa, nước quả, nước và uống ngay lập tức sau khi trộn.'
,1000
,N'Thuốc Amoxicillin chống chỉ định trong các trường hợp sau:Quá mẫn với hoạt chất, với bất kỳ Penicilin nào hoặc với bất kỳ thành phần nào của sản phẩm thuốc.Tiền sử có phản ứng quá mẫn tức thì nghiêm trọng (ví dụ như phản vệ) với một tác nhân beta-lactam khác (ví dụ như Cephalosporin, Carbapenem hoặc Monobactam).'
,'amoxicillin-625mg' 
exec procAddMedicine 'MA00000053'
,N'Amoxicillin dạng trihydrat chỉ dùng đường uống, Amoxicillin dạng muối natri chỉ dùng đường tiêm. Hấp thu Amoxicillin không bị ảnh hưởng bởi thức ăn trong dạ dày, do đó có thể uống trước hoặc sau bữa ăn. Bột pha hỗn dịch khi dùng có thể trộn với sữa, nước quả, nước và uống ngay lập tức sau khi trộn.'
,1000
,N'Thuốc Amoxicillin chống chỉ định trong các trường hợp sau:Quá mẫn với hoạt chất, với bất kỳ Penicilin nào hoặc với bất kỳ thành phần nào của sản phẩm thuốc.Tiền sử có phản ứng quá mẫn tức thì nghiêm trọng (ví dụ như phản vệ) với một tác nhân beta-lactam khác (ví dụ như Cephalosporin, Carbapenem hoặc Monobactam).'
,'amoxicillin-875mg' 
exec procAddMedicine 'MA00000054'
,N'Amoxicillin dạng trihydrat chỉ dùng đường uống, Amoxicillin dạng muối natri chỉ dùng đường tiêm. Hấp thu Amoxicillin không bị ảnh hưởng bởi thức ăn trong dạ dày, do đó có thể uống trước hoặc sau bữa ăn. Bột pha hỗn dịch khi dùng có thể trộn với sữa, nước quả, nước và uống ngay lập tức sau khi trộn.'
,1000
,N'Thuốc Amoxicillin chống chỉ định trong các trường hợp sau:Quá mẫn với hoạt chất, với bất kỳ Penicilin nào hoặc với bất kỳ thành phần nào của sản phẩm thuốc.Tiền sử có phản ứng quá mẫn tức thì nghiêm trọng (ví dụ như phản vệ) với một tác nhân beta-lactam khác (ví dụ như Cephalosporin, Carbapenem hoặc Monobactam).'
,'amoxicillin-1000mg' 
exec procAddMedicine 'MA00000055'
,N'Trước khi tiến hành bôi thuốc, cần tiến hành vệ sinh răng miệng sạch sẽ. Sau đó bôi thuốc Metrogyl denta lên vùng nha chu bị viêm 2 lần/ ngày.Đối với người lớn, lấy một lượng thuốc vừa đủ và thoa lên vùng nha chu bị viêm. Thực hiện 2 lần/ngày và tùy chỉnh liều theo mức độ bệnh. Việc thay đổi liều lượng cần tham khảo ý kiến của bác sĩ, dược sĩ.Đối với trẻ em, không tự ý bôi thuốc cho trẻ em khi có tổn thương nha chu. Trong thuốc có thể có các thành phần không phù hợp với độ tuổi của trẻ. Do đó, cần tham khảo ý kiến bác sĩ trước khi bôi thuốc cho trẻ em.'
,750
,N'Người có tiền sử quá mẫn với Metronidazol hoặc những dẫn chất nitroimidazol khác. Metronidazol cũng chống chỉ định nếu gần đây (trong vòng 2 tuần) người bệnh đã dùng Disulfiram.'
,'metronidazole-27500mg'
exec procAddMedicine 'MA00000056'
,N'Trước khi tiến hành bôi thuốc, cần tiến hành vệ sinh răng miệng sạch sẽ. Sau đó bôi thuốc Metrogyl denta lên vùng nha chu bị viêm 2 lần/ ngày.Đối với người lớn, lấy một lượng thuốc vừa đủ và thoa lên vùng nha chu bị viêm. Thực hiện 2 lần/ngày và tùy chỉnh liều theo mức độ bệnh. Việc thay đổi liều lượng cần tham khảo ý kiến của bác sĩ, dược sĩ.Đối với trẻ em, không tự ý bôi thuốc cho trẻ em khi có tổn thương nha chu. Trong thuốc có thể có các thành phần không phù hợp với độ tuổi của trẻ. Do đó, cần tham khảo ý kiến bác sĩ trước khi bôi thuốc cho trẻ em.'
,750
,N'Người có tiền sử quá mẫn với Metronidazol hoặc những dẫn chất nitroimidazol khác. Metronidazol cũng chống chỉ định nếu gần đây (trong vòng 2 tuần) người bệnh đã dùng Disulfiram.'
,'metronidazole-500mg' 
exec procAddMedicine 'MA00000057'
,N'Thuốc được sử dụng đường uống.Người lớn uống 1 - 4 g/ngày chia làm nhiều lần trong ngày. Hầu hết các trường hợp nhiễm trùng sẽ đáp ứng với liều 500 mg mỗi 8 giờ.Liều khuyến cáo hàng ngày cho trẻ em là 25 - 50 mg/kg chia làm nhiều lần trong ngày.'
,1000
,N'Cefalexin chống chỉ định ở những bệnh nhân dị ứng với nhóm cephalosporin hoặc với bất kỳ thành phần nào của thuốc.Cefalexin nên được dùng một cách thận trọng cho những bệnh nhân có biểu hiện quá mẫn với các thuốc khác. Cần thận trọng khi dùng cephalosporin cho những bệnh nhân quá mẫn với penicilin, vì có một số bằng chứng về khả năng gây dị ứng chéo một phần giữa penicilin và cephalosporin.'
,'cephalexin-500mg' 
exec procAddMedicine 'MA00000058'
,N'Người lớn mỗi lần uống hoặc đặt hậu môn một viên với hàm lượng 325 - 650mg, sử dụng cách nhau 4 - 6 giờ. Nếu dùng thuốc với hàm lượng 1000mg thì thời gian giữa 2 lần dùng thuốc cách nhau 6 đến 8 giờ.Trẻ sơ sinh 10-15mg/kg, mỗi lần dùng cách 6 - 8 giờ (một ngày dùng khoảng 3-4 lần).Trẻ lớn hơn iều dùng giống với trẻ sơ sinh nhưng có thể cho trẻ dùng mỗi lần cách nhau 4 - 6 giờ, tức trong ngày dùng 4-6 lần nhưng kèm theo lời khuyên không dùng quá năm lần trong vòng 24 giờ.'
,1750
,N'chống chỉ định sử dụng paracetamol: Người bệnh dị ứng, mẫn cảm với paracetamol.Người có tiểu sử mắc các bệnh lý về gan, bị tổn thương gan.Người nghiện bia rượu hoặc thường xuyên sử dụng thức uống có cồn và các loại chất kích thích khác.Suy dinh dưỡng nghiêm trọng.Đang sử dụng một số loại thuốc khác có thể gây tương tác thuốc không tốt cho người bệnh.'
,'paracetamol-500mg' 
exec procAddMedicine 'MA00000059'
,N'Khi uống thuốc, uống cả viên thuốc thay vì bẻ đôi hoặc nghiền nát viên thuốc. Viên nén có lớp phủ đặc biệt giúp dạ dày tiêu hóa và hấp thụ thuốc tốt hơn. Không nhai hoặc nghiền nát thuốc ở dạng viên nén vì như vậy sẽ làm lớp phủ bọc ngoài viên thuốc ngừng hoạt động.'
,500
,N'Chống chỉ định của aspirin: Người có tiền sử dị ứng với aspirin.Người bệnh xuất huyết tiêu hóa.Phụ nữ mang thai trong 3 tháng cuối (tăng nguy cơ chảy máu thai kỳ).Trẻ em dưới 12 tuổi (tăng nguy cơ hội chứng Reye).Người bị rối loạn đông máu.Bệnh nhân mắc bệnh gan, thận, đặc biệt là các trường hợp suy gan hoặc suy thận nặng.'
,'aspirin-100mg' 
exec procAddMedicine 'MA00000060'
,N'Người lớn: Liều uống thông thường để giảm đau: 1,2 - 1,8 g/ngày, chia làm nhiều liều nhỏ, tuy nhiên liều duy trì từ 0,6 - 1,2 g/ngày là đã có hiệu quả.Trẻ em thì liều uống thông thường để giảm đau hoặc sốt là 20 - 30 mg/ kg/ngày, chia làm nhiều liều nhỏ.'
,430
,N'Ibuprofen thuốc chống chỉ định trong các trường hợp sau: Mẫn cảm với Ibuprofen.Loét dạ dày tá tràng tiến triển.Quá mẫn với aspirin hoặc với các thuốc chống viêm không steroid khác (hen, viêm mũi, nổi mày đay sau khi dùng Aspirin).Người bệnh bị hen hay bị co thắt phế quản, rối loạn chảy máu, bệnh tim mạch, tiền sử loét dạ dày tá tràng, suy gan hoặc suy thận (mức lọc cầu thận dưới 30 ml/phút).Ba tháng cuối của thai kỳ.Trẻ sơ sinh thiếu tháng đang có chảy máu như chảy máu dạ dày, xuất huyết trong sọ và trẻ có giảm tiểu cầu và rối loạn đông máu. Trẻ sơ sinh có nhiễm khuẩn hoặc nghi ngờ nhiễm khuẩn chưa được điều trị. Trẻ sơ sinh thiếu tháng nghi ngờ viêm ruột hoại tử.'
,'ibuprofen-400mg' 
exec procAddMedicine 'MA00000061'
,N'Người lớn liều thông thường naproxen: 250 – 500 mg/lần x 2 lần/ngày, sáng và chiều; hoặc 250 mg uống buổi sáng và 500 mg uống buổi chiều.Trẻ em 2 – 18 tuổi, liều thông thường 5 – 7,5 mg/kg naproxen, 2 lần/ngày (tối đa 1000 mg/ngày).'
,600
,N'Thuốc chống chỉ định với trường hợp: Người có tiền sử mẫn cảm với naproxen và các thuốc chống viêm không steroid khác, những người có tiền sử viêm mũi dị ứng, hen phế quản, nổi mày đay sau khi dùng aspirin, đặc biệt người đã có dị ứng với aspirin.Suy gan nặng.Suy thận nặng.Loét dạ dày - tá tràng.Viêm trực tràng hoặc chảy máu trực tràng.Phụ nữ 3 tháng cuối thai kỳ.Điều trị đau trong thời gian phẫu thuật ghép nối tắt động mạch vành.'
,'naproxen-500mg' 
exec procAddMedicine 'MA00000062'
,N'Người lớn iều ban đầu 0,75-9 mg/ngày, 2-4 lần tùy theo bệnh.Trẻ em iều thuốc uống 0,02-0,3 mg/kg/ngày hoặc 0,6-10 mg/m2/ngày chia 3 - 4 lần.'
,1200
,N'Không dùng Dexamethasone cho các trường hợp sau: Quá mẫn với Dexamethasone hoặc các hợp phần khác của chế phẩm.Nhiễm nấm toàn thân, sốt rét thể não, nhiễm virus tại chỗ hoặc nhiễm khuẩn lao, lậu chưa kiểm soát và khớp bị hủy hoại nặng.Bệnh nhãn khoa do nhiễm virus Herpes simplex; nhiễm nấm hoặc nhiễm khuẩn lao ở mắt.'
,'dexamethason 0-5mg' 
go

--Bill
create proc procAddBill
  @patId VARCHAR(10),
  @meta NVARCHAR(MAX)
AS
BEGIN
  declare @QuanBI INT
	set @QuanBI = (select count(*) from Bill)
	if(TRIM(@meta)='') 
		begin
			set @meta = 'hoa-don-' + CONVERT(varchar(MAX), @QuanBI+1)
		end
  declare @id varchar(10)
	set @id = dbo.autoid('BI', @QuanBI+1)
  insert into Bill(id,patid,meta)
  VALUES (@id,@patId,@meta)
END
go


--them bill
exec procAddBill 'AC00000014',''
exec procAddBill 'AC00000015',''
go

--Bill_Service
create proc procAddBill_Service
  @billId varchar(10),
  @serId varchar(10),
  @quantity int,
  @meta NVARCHAR(MAX)
AS
BEGIN
  declare @QuanBS INT
	set @QuanBS = (select count(*) from Bill_Service)
	if(TRIM(@meta)='') 
		begin
			set @meta = 'hoa-don-' + CONVERT(varchar(MAX), @QuanBS+1)
		end
  insert into Bill_Service(billID,SerID,quantity,meta)
  VALUES (@billId,@serId,@quantity,@meta)
END
go

--them Bill_Service
exec procAddBill_Service 'BI00000001','SE00000002',1,''
exec procAddBill_Service 'BI00000001','SE00000003',1,''

exec procAddBill_Service 'BI00000002','SE00000030',2,''
GO


--Prescription
create proc procAddPrescription
  @denId varchar(10),
  @patId varchar(10),
  @billId varchar(10),
  @note NVARCHAR(MAX),
  @meta NVARCHAR(MAX)
AS
BEGIN
  declare @QuanPR INT
	set @QuanPR = (select count(*) from Prescription)
	if(TRIM(@meta)='') 
		BEGIN
			set @meta = 'toa-thuoc-' + CONVERT(varchar(MAX), @QuanPR+1)
		end
	declare @id varchar(10)
	set @id = dbo.autoid('PR', @QuanPR+1)
  insert into Prescription(id,denid,patid,billID,note,meta)
  VALUES (@id,@denId,@patId,@billId,@note,@meta)
END
go

--them Precription
exec procAddPrescription 'AC00000007','AC00000014','BI00000001',N'nhổ 2 loại răng khác nhau','' 
exec procAddPrescription 'AC00000004','AC00000015','BI00000002',N'Vá răng và hồi phục',''
GO

--Prescription_Medicine
create proc procAddPrescription_Medicine
  @preID varchar(10),
  @medID varchar(10),
  @quantityMedicine int,
  @meta NVARCHAR(MAX)
AS
BEGIN
  declare @QuanPM INT
	set @QuanPM = (select count(*) from Prescription_Medicine)
	if(TRIM(@meta)='') 
		BEGIN
			set @meta = 'chi-tiet-toa-thuoc' + CONVERT(varchar(MAX), @QuanPM+1)
		end
  insert into Prescription_Medicine(preID,medId,quantityMedicine,meta)
  VALUES (@preID,@medID,@quantityMedicine,@meta)
END
go

--them Prescription_Medicine
exec procAddPrescription_Medicine 'PR00000001','MA00000058',3,N''
exec procAddPrescription_Medicine 'PR00000001','MA00000062',2,N''
exec procAddPrescription_Medicine 'PR00000001','MA00000061',2,N''
exec procAddPrescription_Medicine 'PR00000001','MA00000060',2,N''

exec procAddPrescription_Medicine 'PR00000002','MA00000055',3,N''
GO

--them calendar
create proc procAddCalendar
  @timeStart DATETIME,
  @timeEnd DATETIME,
  @personID VARCHAR(10),
  @meta NVARCHAR(MAX)
AS
BEGIN
  -- Kiem tra dieu kien thoi gian
  if(@timeStart<GETDATE()) 
  BEGIN
    PRINT N'Thời gian bắt đầu phải sau thời gian hiện tại!'
    RETURN
  END
  else if(@timeEnd<=@timeStart) 
  BEGIN
    PRINT N'Thời gian kết thúc phải sau thời gian bắt đầu!'
    RETURN
  END

  -- Kiem tra trung lich cua nhan su
  IF
  (
    NOT
      (select COUNT(*)
      from Calendar ca
      WHERE 
      ca.Personid = (select id from Person WHERE id = @personID)
      AND
      (ca.timeEnd <= @timeStart OR ca.timeStart >= timeEnd)) 
      = 
      (select COUNT(*) from Calendar where Personid = @personID and able = 1)
      -- Neu có chỉ 1 lịch không phù hợp thì báo lỗi
  )
  BEGIN
    PRINT N'Thời gian làm việc của lịch bị trùng'
    RETURN
  END

  declare @QuanCA INT
	set @QuanCA = (select count(*) from Calendar)
	if(TRIM(@meta)='') 
		begin
			set @meta = 'lich-lam-viec-' + CONVERT(varchar(MAX), @QuanCA+1)
		end
	declare @id varchar(10)
	set @id = dbo.autoid('CA', @QuanCA+1)

  -- them vao bang
  insert into Calendar(id,timeStart,timeEnd,meta,Personid)
  VALUES (@id,@timeStart,@timeEnd,@meta,@personID)
END
GO

-- them calendar
-- le tan
exec procAddCalendar '2024-11-01 06:00:00', '2024-11-01 12:00:00','AC00000002',''
exec procAddCalendar '2024-11-01 12:00:00', '2024-11-02 00:00:00','AC00000003',''
exec procAddCalendar '2024-11-02 00:00:00', '2024-11-02 12:00:00','AC00000003',''

-- nha si
exec procAddCalendar '2024-11-01 06:00:00', '2024-11-01 14:00:00','AC00000004',''
exec procAddCalendar '2024-11-01 14:00:00', '2024-11-01 22:00:00','AC00000005',''
exec procAddCalendar '2024-11-01 22:00:00', '2024-11-02 06:00:00','AC00000006',''
go

--them Appointment
create proc procAddAppointment
  @denID VARCHAR(10),
  @patID VARCHAR(10),
  @timeStart DATETIME,
  @timeEnd DATETIME,
  @symptom NVARCHAR(MAX),
  @state NVARCHAR(50),
  @note NVARCHAR(MAX),
  @meta NVARCHAR(MAX)
AS
BEGIN
  -- Kiem tra dieu kien thoi gian
  if(@timeStart<GETDATE()) 
  BEGIN
    PRINT N'Thời gian bắt đầu phải sau thời gian hiện tại!'
    RETURN
  END
  else if(@timeEnd<=@timeStart) 
  BEGIN
    PRINT N'Thời gian kết thúc phải sau thời gian bắt đầu!'
    RETURN
  END
  
  ----------------------------------------------------------
  -- Kiem tra trung lich hen benh nhan
  IF
  (
    NOT
      (select COUNT(*)
      from Appointment ap
      WHERE 
      ap.patID = (select id from Person WHERE id = @patID)
      AND
      (ap.timeEnd <= @timeStart OR ap.timeStart >= timeEnd)) 
      = 
      (select COUNT(*) from Appointment where patID = @patID and able = 1)
      -- Neu có chỉ 1 lịch không phù hợp thì báo lỗi
  )
  BEGIN
    PRINT N'Thời gian hẹn của bệnh nhân bị trùng'
    RETURN
  END

  -- Kiem tra trung lich hen nha si
  IF
  (
    NOT
      (select COUNT(*)
      from Appointment ap
      WHERE 
      ap.denId = (select id from Person WHERE id = @denID)
      AND
      (ap.timeEnd <= @timeStart OR ap.timeStart >= timeEnd)) 
      = 
      (select COUNT(*) from Appointment where denId = @denID and able = 1)
      -- Neu có chỉ 1 lịch không phù hợp thì báo lỗi
  )
  BEGIN
    PRINT N'Thời gian hẹn của nha sĩ bị trùng'
    RETURN
  END



  ----------------------------------------------------------

  
  -- Kiem tra lich hen co trong khoang thoi gian lam viec cua bac si khong
 
  -- Gio hen bat dau va ket thuc phai trong thoi gian bat dau va ket thuc 
  -- phien lam viec cua nha si do
  IF (
    NOT EXISTS
    (
      select 1
      from Calendar ca
      WHERE 
      Personid = @denID --Kiem tra lich cua bac si @denID
      AND
      (ca.timeStart <= @timeStart) --thoi gian bat dau lam viec truoc thoi gian bat dau hen
      AND
      (ca.timeEnd   >= @timeEnd) -- thoi gian ket thuc lam viec sau thoi gian ket thuc
      AND 
      able = 1
    )
  )
  BEGIN
    DECLARE @dentistName NVARCHAR(100)
    SET @dentistName = (select name from Person where id = @denID)
    PRINT N'Không có thời gian làm việc của nha sĩ ' 
    + @dentistName
    + N' phù hợp với lịch hẹn'
    RETURN 
  END

  declare @QuanAP INT
	set @QuanAP = (select count(*) from Appointment)
	if(TRIM(@meta)='') 
		begin
			set @meta = 'lich-hen-' + CONVERT(varchar(MAX), @QuanAP+1)
		end
	declare @id varchar(10)
	set @id = dbo.autoid('AP', @QuanAP+1)

  -- them vao bang
  insert into Appointment(id,denid,patid,symptom,state,note,timeStart,timeEnd,meta)
  VALUES (@id,@denID,@patID,@symptom,@state,@note,@timeStart,@timeEnd,@meta)
END
GO

--them lich hen
exec procAddAppointment 'AC00000004','AC00000014'
,'2024-11-01 06:00:00', '2024-11-01 14:00:00'
,N'Bệnh nhân bị đau và buốt răng'
,N'Chưa xong'
,N'',''

exec procAddAppointment 'AC00000005','AC00000015'
,'2024-11-01 14:00:00', '2024-11-01 16:00:00'
,N'Bệnh nhân bị đau và buốt răng'
,N'Chưa xong'
,N'',''
go

--Advertisement
create proc procAddAdvertisement
  @title NVARCHAR(MAX),
  @msg NVARCHAR(MAX),
  @img VARCHAR(MAX),
  @meta VARCHAR(MAX)
as
begin
	declare @QuanAD INT
	set @QuanAD = (select count(*) from Advertisement)
	if(TRIM(@meta)='') 
		begin
			set @meta = 'quang-cao-' + CONVERT(varchar(MAX), @QuanAD+1)
		end
	declare @id VARCHAR(10)
	set @id = dbo.autoid('AD', @QuanAD+1)
	insert into Advertisement(id,title,msg,img,meta)
	values (@id,@title,@msg,@img,@meta)
end
go

--them quang cao
exec procAddAdvertisement 
N'Niềng răng đẹp tự tin chỉ với 2 triệu',
N'
Bạn có ngay một nụ cười đẹp tự tin, cung răng đều đẹp mà không phải lo lắng về giá cả.Chỉ với 2 triệu / tháng. Thoải mái khi niềng.Bác sĩ gắn mắc cài tuần tự từng răng, từng hàm để bạn quen dần nên gần như không đau và ít ảnh hưởng đến sinh hoạt ăn uống, trò chuyện.Bạn có thể chọn phối màu dây thun theo sở thích, cá tính riêng trong khi niềng răng và chúc mừng bạn với nụ cười tự tin, cung răng đều, đẹp sau khi tháo mắc cài.',
'',''

exec procAddAdvertisement 
N'Răng sứ tự nhiên đẹp theo tiêu chuẩn vàng',
N'Hình dáng và kích thước răng hoàn hảo: phù hợp với khuôn mặt, kích thước ngang và chiều cao của răng, kích thước tương quan giữa các răng theo đúng tiêu chuẩn vàng.Màu sắc hài hòa: răng sứ Nha Khoa Kim có màu sắc phù hợp với màu da, môi và các răng xung quanh, tạo nên sự tự nhiên khó nhận ra.Cảm giác thoải mái: răng sứ Nha Khoa Kim không chỉ đẹp mà còn mang lại cảm giác thoải mái khi ăn uống, nói chuyện, giúp bạn tự tin hơn mỗi ngày.Các dòng phôi sứ có nguồn gốc xuất xứ từ Mý, Đức, Úc, chứng nhận FDA, CE, 100% sứ nguyên chất, không tạp chất kim loại.Quy trình kiểm soát nhiễm khuẩn được thực hiện cực kỳ chặt chẽ, đảm bảo không gian sạch sẽ và an toàn tuyệt đối cho mọi khách hàng với từng phòng riêng biệt.',
'',''
go

--Comment
create proc procAddComment
  @patID VARCHAR(10),
  @title NVARCHAR(MAX),
  @msg NVARCHAR(MAX),
  @img VARCHAR(MAX),
  @meta VARCHAR(MAX)
as
begin
	declare @QuanCO INT
	set @QuanCO = (select count(*) from Comment)
	if(TRIM(@meta)='') 
		begin
			set @meta = 'binh-luan-' + CONVERT(varchar(MAX), @QuanCO+1)
		end
	declare @id VARCHAR(10)
	set @id = dbo.autoid('CO', @QuanCO+1)
	insert into Comment(id,patid,title,msg,img,meta)
	values (@id,@patID,@title,@msg,@img,@meta)
end
go

--them comment
exec procAddComment 'AC00000014',N'Khách hàng bọc răng sứ',N'Bọc răng sứ nhìn như răng thật hehe','',''
exec procAddComment 'AC00000015',N'Khách hàng niềng răng',N'Dịch vụ trên cả tuyệt vời:33','',''
go

--NEWS

CREATE PROCEDURE procAddNews
  @title NVARCHAR(MAX),
  @descrip NVARCHAR(MAX),
  @msg NVARCHAR(MAX),
  @meta VARCHAR(MAX)
AS
BEGIN
    DECLARE @QuanNE INT
    SET @QuanNE = (SELECT COUNT(*) FROM NEWS)

    -- Tạo ID mới dựa trên số lượng hiện tại
    DECLARE @id VARCHAR(10)
    SET @id = dbo.autoid('NE', @QuanNE + 1)

    -- Đặt đường dẫn cho hình ảnh dựa trên ID
    DECLARE @imgPath VARCHAR(MAX)
    SET @imgPath = CONCAT('Content/images/Blog/', @id, '.jpg')

    -- Thực hiện chèn vào bảng NEWS
    INSERT INTO NEWS (id, title, descrip,msg, img, meta)
    VALUES (@id, @title, @descrip,@msg, @imgPath, @meta)
END
GO
--them news
exec procAddNews 
    @title = N'Muốn hạ đường huyết, nên tập thể dục buổi nào là tốt nhất?',
	@descrip = N'Những nghiên cứu giúp ích cho việc cải thiện đường huyết',
    @msg = N'Các bằng chứng nghiên cứu trước đây cho thấy tập thể dục ở cường độ 
	vừa đến mạnh có thể giúp cải thiện đường huyết ở những người bị kháng insulin. 
	Chưa dừng lại ở đó, một nghiên cứu mới đây còn phát hiện thời điểm tập luyện giúp kiểm soát đường huyết tốt nhất trong ngày.
	Nghiên cứu được công bố trên chuyên san Obesity. 
	Trong nghiên cứu, các nhà khoa học đã phân tích dữ liệu thu thập từ 186 người. 
	Độ tuổi trung bình của họ là 46 tuổi và chỉ số khối cơ thể là 33, tức trạng thái béo phì. 
	Những người này được mang thiết bị đeo tay để theo dõi cường độ vận động và đường huyết, 
	theo trang thông tin sức khỏe Everyday Health (Mỹ).
	Có nhiều nguyên nhân giúp giải thích lợi ích đặc biệt của tập thể dục vào buổi tối. 
	Nguyên nhân đầu tiên cần nhắc đến là tập thể dục sẽ giúp cải thiện tình trạng kháng insulin, 
	cho phép cơ thể sử dụng hoóc môn insulin hiệu quả hơn. 
	Do đó, tập thể dục vào buổi tối sẽ giúp giảm nguy cơ tăng đường huyết sau bữa ăn tối.',
    @meta = N'muon-ha-duong-huyet-nen-tap-the-duc-buoi-nao-la-tot-nhat'
go
exec procAddNews 
    @title = N'5 cảnh báo sức khỏe được tiết lộ qua răng',
	@descrip = N'Một vài lời khuyên cho việc chăm sóc răng miệng',
    @msg = N'Chăm sóc sức khỏe răng miệng hằng ngày rất quan trọng 
	vì giúp ngăn ngừa sâu răng và bệnh nướu răng. 
	Do đó, một số vấn đề răng miệng có thể là lời cảnh báo về bệnh nguy hiểm tiềm ẩn.
	Đánh răng, dùng chỉ nha khoa và khám răng định kỳ giúp tăng cường sức khỏe răng miệng. 
	Nhờ đó, mọi người sẽ không phải bị sưng nướu hay sâu răng nghiêm trọng, 
	theo chuyên trang sức khỏe Healthline (Mỹ). Thiếu máu
	Các chuyên gia cho biết thiếu sắt sẽ gây thiếu máu. 
	Thiếu máu khiến các cơ quan trong cơ thể không nhận đủ lượng máu giàu dinh dưỡng và ô xy.
	Với răng, tình trạng này kéo dài sẽ ảnh hưởng xấu đến men răng và gây sâu răng.
	Nguy cơ chuyển dạ sớm
	Ở phụ nữ mang thai, sâu răng hay viêm nướu có thể làm tăng nguy cơ chuyển dạ sớm và sinh non. 
	Nguyên nhân là do vi khuẩn từ mảng bám hay vị trí viêm nhiễm trong răng sẽ theo mạch máu
	để đến nhau thai. 
	Tình trạng này gây viêm và tăng nguy cơ làm vỡ túi ối, dẫn đến chuyển dạ sớm.
	Ngoài ra, bệnh nướu răng kéo dài cũng khiến cơ thể giải phóng prostaglandin. Chất này hoạt 
	động khá giống hoóc môn giúp điều hòa quá trình chuyển dạ và tăng nguy cơ sinh non.',
    @meta = N'5-canh-bao-suc-khoe-duoc-tiet-lo-qua-rang'
go
exec procAddNews 
    @title = N'Xử trí khi bị chấn thương răng miệng',
	@descrip = N'Một số biện pháp hữu ít sau khi bị chấn thương răng miệng',
    @msg = N'Răng bị sứt hoặc gãy có thể gắn lại được nhưng mảnh răng bị sứt, 
	gãy cần được bảo quản đúng bằng cách đặt vào khăn hoặc túi ẩm với một vài 
	giọt nước sạch hoặc nước bọt. Đó là một trong các tình huống cần lưu ý khi xử trí khi 
	chấn thương răng miệng.
	Chấn thương răng miệng là sự cố dễ gặp trong đời sống. 
	Đặc biệt, trong mùa bão lũ, việc tới các cơ sở y tế sẽ mất nhiều thời gian hơn bình 
	thường khi chấn thương xảy ra.
	Dưới đây là một số biện pháp xử trí nhanh trong trường hợp bệnh nhân có chấn
	thương răng miệng nhưng chưa thể đến cơ sở y tế kịp thời:
	Răng lung lay: dùng đầu ngón tay ấn nhẹ để định vị lại răng đúng vị trí. 
	Không ép răng vào ổ răng. Cố định răng bằng khăn giấy hoặc gạc ẩm.
	Liên hệ với bác sĩ răng hàm mặt càng sớm càng tốt để được điều trị kịp thời.
	Có thể gọi điện cho bác sĩ răng hàm mặt để được hướng dẫn nếu có thể gọi được.
	Răng bị sứt hoặc gãy: đặt mảnh răng bị sứt, 
	gãy vào khăn hoặc túi ẩm với một vài giọt nước hoặc nước bọt vì có thể gắn lại được. 
	Súc miệng bằng nước ấm và giữ sạch cho đến khi bạn có thể đến gặp bác sĩ răng hàm mặt. 
	Bác sĩ răng hàm mặt có thể sử dụng chính mảnh gãy này để gắn vào phần răng còn lại, 
	đây là một cách phục hồi rất tốt mà không cần phải sử dụng các vật liệu thay thế khác.
	Răng bị bật, rơi ra khỏi huyệt ổ răng: nên tìm lại răng, 
	chỉ cầm vào thân răng (vùng nhẵn, bóng), không chạm vào vùng chân răng (kể cả trong lúc rửa răng), 
	cắm lại răng vào huyệt ổ răng ngay tại hiện trường tai nạn (nếu có thể).',
    @meta = N'xu-tri-khi-bi-chan-thuong-rang-mieng'
go
--Select * from News
--Menu
create proc procAddMenu
  @name NVARCHAR(100),
  @link NVARCHAR(MAX),
  @meta VARCHAR(MAX)
as
begin
	declare @QuanME INT
	set @QuanME = (select count(*) from Menu)
	if(TRIM(@meta)='') 
		begin
			set @meta = 'menu-' + CONVERT(varchar(MAX), @QuanME+1)
		end
	declare @id VARCHAR(10)
	set @id = dbo.autoid('ME', @QuanME+1)
	insert into Menu(id,name,link,meta)
	values (@id,@name,@link,@meta)
end
go

--them menu
exec procAddMenu N'Trang chủ','/','trang-chu'
exec procAddMenu N'Nha sĩ','/Dentist','nha-si'
exec procAddMenu N'Tin tức','/Blog','tin-tuc'
exec procAddMenu N'Dịch vụ','/Service','dich-vu'
exec procAddMenu N'Bình luận','/Comments','binh-luan'
exec procAddMenu N'Liên hệ','/Contact','lien-he'
exec procAddMenu N'Tài khoản','/Login','tai-khoan'
go

update Menu set name = 'Dentist' where name ='Doctors'
go
--Select * from menu

--CLinic
create proc procAddClinic
  @name NVARCHAR(100),
  @phoneNumber VARCHAR(10),
  @address NVARCHAR(MAX),
  @img VARCHAR(MAX),
  @email VARCHAR(100),
  @facebook VARCHAR(MAX),
  @zalo VARCHAR(MAX),
  @instagram VARCHAR(MAX),
  @youtube VARCHAR(MAX),
  @title NVARCHAR(MAX),
  @msg NVARCHAR(MAX),
  @dateStartClinic VARCHAR(MAX),
  @meta VARCHAR(MAX)
as
begin
	declare @QuanCL INT
	set @QuanCL = (select count(*) from Clinic)
	if(TRIM(@meta)='') 
		begin
			set @meta = 'thong-tin-phong-kham-' + CONVERT(varchar(MAX), @QuanCL+1)
		end
	declare @id VARCHAR(10)
	set @id = dbo.autoid('CL', @QuanCL+1)
	insert into Clinic(id,name,phoneNumber,address,img,email,facebook,zalo,instagram,youtube,title,msg,meta,dateStartClinic)
	values (@id,@name,@phoneNumber,@address,@img,@email,@facebook,@zalo,@instagram,@youtube,@title,@msg,@meta,@dateStartClinic  )
end
go

--them thong tin phong kham 
exec procAddClinic 
N'Bình Minh',
'0909908752',
N'178 Phạm Hùng, Tân Hưng, Quận 7, Hồ Chí Minh, Việt Nam',
'',
'phongkhambinhminh@gmail.com',
'https://www.facebook.com/profile.php?id=61551934727425',
'',
'https://www.instagram.com/_demo_official',
'https://www.youtube.com/@nhakhoathuyanh',
N'Chúng tôi hiểu rằng mỗi khách hàng đều có những nhu cầu và mong muốn khác nhau. Vì vậy, chúng tôi luôn lắng nghe và tư vấn tận tình để đưa ra những giải pháp phù hợp nhất cho từng trường hợp. Hãy để chúng tôi đồng hành cùng bạn trên hành trình chinh phục nụ cười hoàn hảo.',
N'Nha khoa chuyên nghiệp, chăm sóc tận tâm.',
'2004-12-12',
'thong-tin-nha-khoa'
go

--them new order 

--bang tin tuc
ALTER TABLE NEWS
ADD new_order INT
go

UPDATE NEWS
SET new_order = [order];
go

--bang service_category
ALTER TABLE Service_Category
ADD new_order INT
go

UPDATE Service_Category
SET new_order = [order];
go

--bang service
ALTER TABLE Service
ADD new_order INT
go

UPDATE Service
SET new_order = [order];
go

--bang bill
ALTER TABLE Bill
ADD new_order INT
go

UPDATE Bill
SET new_order = [order];
go


--bang prescription
ALTER TABLE Prescription
ADD new_order INT
go

UPDATE Prescription
SET new_order = [order];
go

--bang prescription_Medicine
ALTER TABLE Prescription_Medicine
ADD new_order INT
go

UPDATE Prescription_Medicine
SET new_order = [order];
go

