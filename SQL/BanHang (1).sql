CREATE DATABASE BanHang
GO
USE BanHang
GO

CREATE TABLE ProcesedFood (
    ProductID INT NOT NULL,
    ProductName NVARCHAR(255),
    Price DECIMAL(10, 2),
	Images VARCHAR(50),
    CONSTRAINT PK_Product PRIMARY KEY (ProductID)
);
GO

CREATE TABLE Employee (
    Id INT NOT NULL,
    Name NVARCHAR(50),
    Avatar VARCHAR(50),
	Age INT,
    Position NVARCHAR(255), ---chức vụ
	CONSTRAINT PK_Employee PRIMARY KEY (Id)
);
GO

CREATE TABLE OrderPD (
    OrderID INT NOT NULL,
	EmployeeID INT,
    OrderDate DATETIME,
    Total DECIMAL(10, 2),
    OrderStatus NVARCHAR(50),
	PaymentID INT,
	CONSTRAINT PK_OrderPD PRIMARY KEY (OrderID)
);
GO

CREATE TABLE OrderDetails (
    OrderID INT NOT NULL,
    ProductID INT NOT NULL,
    Price DECIMAL(10, 2),
	Quantity INT,
	CONSTRAINT PK_OrderDetails PRIMARY KEY (OrderID, ProductID)
);

CREATE TABLE Payment (
    PaymentID INT NOT NULL,
    PaymentMethod NVARCHAR(50),
    Amount DECIMAL(10, 2),
    PaymentDate DATETIME,
	CONSTRAINT PK_Payment PRIMARY KEY (PaymentID)
);
GO

---------------------------------------------
ALTER TABLE OrderPD
ADD CONSTRAINT FK_Order_Payment FOREIGN KEY (PaymentID) REFERENCES Payment(PaymentID),
	CONSTRAINT FK_Order_Employee FOREIGN KEY (EmployeeID) REFERENCES Employee(Id);
GO


ALTER TABLE OrderDetails
ADD CONSTRAINT FK_OrderDetails_Order FOREIGN KEY (OrderID) REFERENCES OrderPD(OrderID),
	CONSTRAINT FK_OrderDetails_Product FOREIGN KEY (ProductID) REFERENCES ProcesedFood(ProductID);
GO

----------------------------------------------
SELECT * FROM ProcesedFood
SELECT * FROM Employee
SELECT * FROM OrderPD
SELECT * FROM OrderDetails
SELECT * FROM Payment

----------------------------------------------
INSERT INTO ProcesedFood VALUES	(1,N'Phin sữa đá',29,'PhinSuaDa.jpg'),
								(2,N'Phin đá đen',29,'PhinDaDen.jpg'),
								(3,N'Bạc xỉu',29,'BacXiu.jpg'),
								(4,N'PhinDi hạnh nhân',45,'PhinDiHanhNhan.jpg'),
								(5,N'PhinDi kem sữa',45,'PhinDiKemSua.jpg'),
								(6,N'Trà sen vàng',45,'TraSenVan.jpg'),
								(7,N'Trà thạch đào',45,'TraThachDao.jpg'),
								(8,N'Trà thạch vải',45,'TraThachVai.jpg'),
								(9,N'Trà xanh đậu đỏ',45,'TraXanhDauDo.jpg'),
								(10,N'Bánh chuối',29,'BanhChuoi.jpg'),
								(11,N'Phô mai chanh dây',29,'PhoMaiChanhDay.jpg'),
								(12,N'Phô mai Coffee',29,'PhoMaiCoffee.jpg'),
								(13,N'Phô mai trà xanh',29,'PhoMaiTraXanh.jpg'),
								(14,N'Freeze trà xanh',55,'FreezeTraXanh.jpg');
GO

INSERT INTO Employee VALUES (1,N'Nguyễn Văn Tú','avatar1.jpg',30,N'Quản lý'),
							(2,N'Phạm Thị Hồng Nhung','avatar2.jpg',25,N'Nhân viên bán hàng'),
							(3,N'Trần Văn Đăng','avatar3.jpg',28,N'Nhân viên bán hàng'),
							(4,N'Lê Thị Hợp','avatar4.jpg',35,N'Kế toán'),
							(5,N'Huỳnh Văn Bánh','avatar5.jpg',22,N'Nhân viên bán hàng'),
							(6,N'Nguyễn Hồng Nhất','avatar6.jpg',26,N'Nhân viên bán hàng'),
							(7,N'Trần Văn Phú','avatar7.jpg',25,N'Nhân viên bán hàng'),
							(8,N'Nguyễn Hải Phúc','avatar8.jpg',25,N'Nhân viên bán hàng'),
							(9,N'Phạm Văn Hai','avatar9.jpg',24,N'Nhân viên bán hàng'),
							(10,N'Lìu Mỹ Hồng','avatar10.jpg',23,N'Nhân viên bán hàng');
GO

SET DATEFORMAT dmy
INSERT INTO OrderPD (OrderID,EmployeeID,OrderDate,OrderStatus,PaymentID) 
VALUES	(1,1,'10/02/2023',N'Đã giao hàng',1),
		(2,2,'10/03/2023',N'Đang xử lý',2),
		(3,3,'10/04/2023',N'Đang xử lý',3),
		(4,4,'10/05/2023',N'Đã giao hàng',4),
		(5,5,'10/06/2023',N'Đã giao hàng',5),
		(6,6,'10/07/2023',N'Đã giao hàng',6),
		(7,7,'10/08/2023',N'Đang xử lý',7),
		(8,8,'10/09/2023',N'Đang xử lý',8),
		(9,9,'10/20/2023',N'Đã giao hàng',9),
		(10,10,'10/21/2023',N'Đã giao hàng',10),
		(11,1,'10/22/2023',N'Đã giao hàng',11),
		(12,2,'10/23/2023',N'Đang xử lý',12),
		(13,3,'10/24/2023',N'Đang xử lý',13),
		(14,4,'10/25/2023',N'Đã giao hàng',14),
		(15,5,'10/26/2023',N'Đã giao hàng',15),
		(16,6,'10/27/2023',N'Đã giao hàng',16),
		(17,7,'10/28/2023',N'Đang xử lý',17),
		(18,8,'10/29/2023',N'Đang xử lý',18),
		(19,9,'11/02/2023',N'Đã giao hàng',19),
		(20,10,'11/03/2023',N'Đã giao hàng',20),
		(21,1,'11/04/2023',N'Đã giao hàng',21),
		(22,2,'11/05/2023',N'Đang xử lý',22),
		(23,3,'11/06/2023',N'Đang xử lý',23),
		(24,4,'11/07/2023',N'Đã giao hàng',24),
		(25,5,'11/08/2023',N'Đã giao hàng',25),
		(26,6,'11/09/2023',N'Đã giao hàng',26),
		(27,7,'11/10/2023',N'Đang xử lý',27),
		(28,8,'11/11/2023',N'Đang xử lý',28),
		(29,9,'11/12/2023',N'Đã giao hàng',29),
		(30,10,'11/13/2023',N'Đã giao hàng',30),
		(31,1,'11/14/2023',N'Đã giao hàng',31),
		(32,2,'11/15/2023',N'Đã giao hàng',32),
		(33,3,'11/16/2023',N'Đã giao hàng',33),
		(34,4,'11/17/2023',N'Đang xử lý',34),
		(35,5,'11/18/2023',N'Đang xử lý',35),
		(36,6,'11/19/2023',N'Đã giao hàng',36),
		(37,7,'11/20/2023',N'Đã giao hàng',37),
		(38,8,'11/21/2023',N'Đã giao hàng',38),
		(39,9,'11/22/2023',N'Đang xử lý',39),
		(40,10,'11/23/2023',N'Đang xử lý',40),
		(41,1,'11/24/2023',N'Đã giao hàng',41),
		(42,2,'11/25/2023',N'Đã giao hàng',42),
		(43,3,'11/26/2023',N'Đã giao hàng',43),
		(44,4,'11/27/2023',N'Đang xử lý',44),
		(45,5,'11/28/2023',N'Đang xử lý',45),
		(46,6,'11/29/2023',N'Đã giao hàng',46),
		(47,7,'11/30/2023',N'Đã giao hàng',47),
		(48,8,'12/01/2023',N'Đang xử lý',48),
		(49,9,'12/02/2023',N'Đã giao hàng',49),
		(50,10,'12/03/2023',N'Đã giao hàng',50);
GO

INSERT INTO OrderDetails
VALUES  (1,1,29,1),
		(1,4,45,2),
		(2,3,29,1),
		(2,4,45,2),
		(2,1,29,1),
		(3,6,45,2),
		(4,2,29,1),
		(4,7,45,1),
		(4,9,45,1),
		(4,10,29,1),
		(5,12,29,2),
		(5,11,29,1),
		(6,4,45,3),
		(7,5,45,2),
		(8,6,45,1),
		(9,7,45,2),
		(10,8,45,1),
		(11,1,29,1),
		(11,4,45,2),
		(11,3,29,1),
		(12,4,45,2),
		(12,1,29,1),
		(13,6,45,2),
		(14,2,29,1),
		(15,7,45,1),
		(16,9,45,1),
		(17,10,29,1),
		(17,12,29,2),
		(17,11,29,1),
		(18,4,45,3),
		(18,5,45,2),
		(19,6,45,1),
		(20,7,45,2),
		(20,8,45,1),
		(20,1,29,1),
		(21,4,45,2),
		(21,3,29,1),
		(22,4,45,2),
		(23,1,29,1),
		(24,6,45,2),
		(25,2,29,1),
		(26,7,45,1),
		(27,9,45,1),
		(28,10,29,1),
		(28,12,29,2),
		(29,11,29,1),
		(30,4,45,3),
		(31,1,29,1),
		(31,4,45,2),
		(32,3,29,1),
		(33,4,45,2),
		(33,1,29,1),
		(33,6,45,2),
		(34,2,29,1),
		(34,7,45,1),
		(35,9,45,1),
		(36,10,29,1),
		(36,12,29,2),
		(37,11,29,1),
		(38,4,45,3),
		(39,5,45,2),
		(40,6,45,1),
		(41,7,45,2),
		(41,8,45,1),
		(41,1,29,1),
		(42,4,45,2),
		(42,3,29,1),
		(43,4,45,2),
		(44,1,29,1),
		(45,6,45,2),
		(46,2,29,1),
		(47,7,45,1),
		(48,9,45,1),
		(48,10,29,1),
		(49,12,29,2),
		(50,11,29,1),
		(50,4,45,3);
GO

SET DATEFORMAT dmy
INSERT INTO Payment (PaymentID,PaymentMethod,PaymentDate) 
VALUES  (1,N'Chuyễn khoản','10/02/2023'),
		(2,N'Tiền mặt','10/03/2023'),
		(3,N'Chuyễn khoản','10/04/2023'),
		(4,N'Tiền mặt','10/05/2023'),
		(5,N'Chuyễn khoản','10/06/2023'),
		(6,N'Tiền mặt','10/07/2023'),
		(7,N'Chuyễn khoản','10/08/2023'),
		(8,N'Tiền mặt','10/09/2023'),
		(9,N'Chuyễn khoản','10/20/2023'),
		(10,N'Tiền mặt','10/21/2023'),
		(11,N'Chuyễn khoản','10/22/2023'),
		(12,N'Tiền mặt','10/23/2023'),
		(13,N'Chuyễn khoản','10/24/2023'),
		(14,N'Tiền mặt','10/25/2023'),
		(15,N'Chuyễn khoản','10/26/2023'),
		(16,N'Tiền mặt','10/27/2023'),
		(17,N'Chuyễn khoản','10/28/2023'),
		(18,N'Tiền mặt','10/29/2023'),
		(19,N'Chuyễn khoản','11/02/2023'),
		(20,N'Tiền mặt','11/03/2023'),
		(21,N'Chuyễn khoản','11/04/2023'),
		(22,N'Tiền mặt','11/05/2023'),
		(23,N'Chuyễn khoản','11/06/2023'),
		(24,N'Tiền mặt','11/07/2023'),
		(25,N'Chuyễn khoản','11/08/2023'),
		(26,N'Tiền mặt','11/09/2023'),
		(27,N'Chuyễn khoản','11/10/2023'),
		(28,N'Tiền mặt','11/11/2023'),
		(29,N'Chuyễn khoản','11/12/2023'),
		(30,N'Tiền mặt','11/13/2023'),
		(31,N'Chuyễn khoản','11/14/2023'),
		(32,N'Tiền mặt','11/15/2023'),
		(33,N'Chuyễn khoản','11/16/2023'),
		(34,N'Tiền mặt','11/17/2023'),
		(35,N'Chuyễn khoản','11/18/2023'),
		(36,N'Tiền mặt','11/19/2023'),
		(37,N'Chuyễn khoản','11/20/2023'),
		(38,N'Tiền mặt','11/21/2023'),
		(39,N'Chuyễn khoản','11/22/2023'),
		(40,N'Tiền mặt','11/23/2023'),
		(41,N'Chuyễn khoản','11/24/2023'),
		(42,N'Tiền mặt','11/25/2023'),
		(43,N'Chuyễn khoản','11/26/2023'),
		(44,N'Tiền mặt','11/27/2023'),
		(45,N'Chuyễn khoản','11/28/2023'),
		(46,N'Tiền mặt','11/29/2023'),
		(47,N'Chuyễn khoản','11/30/2023'),
		(48,N'Tiền mặt','12/01/2023'),
		(49,N'Chuyễn khoản','12/02/2023'),
		(50,N'Tiền mặt','12/03/2023');
GO



---------------------------------------------
UPDATE OrderPD
SET Total = (
    SELECT SUM(Price * Quantity)
    FROM OrderDetails
    WHERE OrderDetails.OrderID = OrderPD.OrderID
);
GO

UPDATE Payment
SET Amount = (
    SELECT SUM(OrderPD.Total)
    FROM OrderPD
    WHERE OrderPD.PaymentID = Payment.PaymentID
);
GO







