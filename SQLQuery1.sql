-- Tạo bảng Danh mục hàng hóa (Product Categories)
CREATE TABLE ProductCategories (
    category_id INT PRIMARY KEY,
    category_name VARCHAR(255) NOT NULL
);

-- Tạo bảng Nhà cung cấp (Suppliers)
CREATE TABLE Suppliers (
    supplier_id INT PRIMARY KEY,
    supplier_name VARCHAR(255) NOT NULL,
    contact_name VARCHAR(255),
    contact_email VARCHAR(255),
    contact_phone VARCHAR(20)
);

-- Tạo bảng Kho (Warehouses)
CREATE TABLE Warehouses (
    warehouse_id INT PRIMARY KEY,
    warehouse_name VARCHAR(255) NOT NULL,
    location VARCHAR(255)
);

-- Tạo bảng Hàng hóa (Products)
CREATE TABLE Products (
    product_id INT PRIMARY KEY,
    product_name VARCHAR(255) NOT NULL,
    description TEXT,
    price DECIMAL(10, 2),
    stock_quantity INT,
    warehouse_id INT,
    FOREIGN KEY (warehouse_id) REFERENCES Warehouses (warehouse_id)
);

-- Tạo bảng Liên kết sản phẩm và danh mục (Product Categories Link)
CREATE TABLE ProductCategoriesLink (
    product_id INT,
    category_id INT,
    PRIMARY KEY (product_id, category_id),
    FOREIGN KEY (product_id) REFERENCES Products (product_id),
    FOREIGN KEY (category_id) REFERENCES ProductCategories (category_id)
);

-- Tạo bảng Hóa đơn nhập (Purchase Orders)
CREATE TABLE PurchaseOrders (
    order_id INT PRIMARY KEY,
    supplier_id INT,
    order_date DATE,
    FOREIGN KEY (supplier_id) REFERENCES Suppliers (supplier_id)
);

-- Tạo bảng Chi tiết đơn nhập (Purchase Order Details)
CREATE TABLE PurchaseOrderDetails (
    order_detail_id INT PRIMARY KEY,
    order_id INT,
    product_id INT,
    quantity INT,
    unit_price DECIMAL(10, 2),
    FOREIGN KEY (order_id) REFERENCES PurchaseOrders (order_id),
    FOREIGN KEY (product_id) REFERENCES Products (product_id)
);

-- Tạo bảng Phân chia hàng hóa (Product Transfers)
CREATE TABLE ProductTransfers (
    transfer_id INT PRIMARY KEY,
    product_id INT,
    from_warehouse_id INT,
    to_warehouse_id INT,
    quantity INT,
    transfer_date DATE,
    FOREIGN KEY (product_id) REFERENCES Products (product_id),
    FOREIGN KEY (from_warehouse_id) REFERENCES Warehouses (warehouse_id),
    FOREIGN KEY (to_warehouse_id) REFERENCES Warehouses (warehouse_id)
);

--------------------------------------------------------------------------------------
CREATE TABLE Suppliers (
    supplier_id INT IDENTITY PRIMARY KEY,
    supplier_name NVARCHAR(255) NOT NULL,
    contact_email VARCHAR(255),
    contact_phone VARCHAR(20)
);



CREATE TABLE Products (
    product_id INT IDENTITY PRIMARY KEY,
    product_name NVARCHAR(255) NOT NULL,
    description TEXT NULL,
    price FLOAT,
    quantity INT NULL,
);

CREATE TABLE PurchaseOrders (
    order_id INT IDENTITY PRIMARY KEY,
    supplier_id INT,
    order_date DATE,
);

CREATE TABLE PurchaseOrderDetails (
    order_id INT,
    product_id INT,
    quantity INT,
    price FLOAT,
	PRIMARY KEY (order_id,product_id)
);
