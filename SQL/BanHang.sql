CREATE DATABASE BanHang
GO
USE BanHang
GO

CREATE TABLE Product (
    ProductID INT NOT NULL,
    ProductName NVARCHAR(255),
    Price DECIMAL(10, 2),
    CONSTRAINT PK_Product PRIMARY KEY (ProductID)
);
GO


CREATE TABLE Customer (
    CustomerID INT NOT NULL,
    CustomerName NVARCHAR(50),
    CustomerAddress NVARCHAR(255),
    PhoneNumber VARCHAR(15),
	CONSTRAINT PK_Customer PRIMARY KEY (CustomerID)
);

GO

CREATE TABLE Employee (
    EmployeeID INT NOT NULL,
    EmployeeName NVARCHAR(50),
    Position VARCHAR(50),
    ContactInfo VARCHAR(255),
	CONSTRAINT PK_Employee PRIMARY KEY (EmployeeID)
);
GO

CREATE TABLE OrderPD (
    OrderID INT NOT NULL,
	EmployeeID INT,
    CustomerID INT,
    OrderDate DATETIME,
    Total DECIMAL(10, 2),
    OrderStatus NVARCHAR(50),
	CONSTRAINT PK_OrderPD PRIMARY KEY (OrderID)
);
GO

CREATE TABLE OrderDetails (
    OrderDetailID INT NOT NULL,
    OrderID INT,
    ProductID INT,
    Price DECIMAL(10, 2),
	CONSTRAINT PK_OrderDetails PRIMARY KEY (OrderDetailID)
);

CREATE TABLE Payment (
    PaymentID INT NOT NULL,
    OrderID INT,
    PaymentMethod NVARCHAR(50),
    Amount DECIMAL(10, 2),
    PaymentDate DATETIME,
	CONSTRAINT PK_Payment PRIMARY KEY (PaymentID)
);
GO

---------------------------------------------
ALTER TABLE OrderPD
ADD CONSTRAINT FK_Order_Customer FOREIGN KEY (CustomerID) REFERENCES Customer(CustomerID),
	CONSTRAINT FK_Order_Employee FOREIGN KEY (EmployeeID) REFERENCES Employee(EmployeeID);
GO

ALTER TABLE Payment
ADD CONSTRAINT FK_Payment_Order FOREIGN KEY (OrderID) REFERENCES OrderPD(OrderID);
GO

ALTER TABLE OrderDetails
ADD CONSTRAINT FK_OrderDetails_Order FOREIGN KEY (OrderID) REFERENCES OrderPD(OrderID),
	CONSTRAINT FK_OrderDetails_Product FOREIGN KEY (ProductID) REFERENCES Product(ProductID);
GO
















