create database DB03;
use DB03;

create table Category (
    CategoryID char(3) primary key,
    CategoryName varchar(50)
);

create table Product (
    ProductID char(4) primary key,
    ProductName varchar(50),
    UnitPrice int,
    CategoryID char(3),
    foreign key (CategoryID) references Category(CategoryID)
);

create table Warehouse (
    WarehouseID char(3) primary key,
    WarehouseAddress varchar(100),
    CategoryID char(3),
    foreign key (CategoryID) references Category(CategoryID)
);

create table Instock (
    WarehouseID char(3),
    ProductID char(4),
    Quantity int,
    primary key (WarehouseID, ProductID),
    foreign key (WarehouseID) references Warehouse(WarehouseID),
    foreign key (ProductID) references Product(ProductID)
);

insert into Category values 
('C01', 'Beverage'), 
('C02', 'Snack'), 
('C03', 'Electronics');

insert into Product values 
('P001', 'Cola', 10, 'C01'), 
('P002', 'Chips', 5, 'C02'), 
('P003', 'Water', 8, 'C01'), 
('P004', 'Cookies', 6, 'C02'), 
('P005', 'Headphones', 50, 'C03'), 
('P006', 'Soda', 9, 'C01'), 
('P007', 'Beverage', 11, 'C01');

insert into Warehouse values 
('W01', '123 Main St', 'C01'), 
('W02', '456 South Ave', 'C02'), 
('W03', '789 Tech Park', 'C03');

insert into Instock values 
('W01', 'P001', 100), 
('W01', 'P003', 150), 
('W01', 'P006', 80), 
('W01', 'P007', 60), 
('W02', 'P002', 200), 
('W02', 'P004', 100), 
('W03', 'P005', 50);

-- Tìm hàng lộn kho
select *
from Instock X
where ProductID not in (select ProductID
						from Warehouse A, Product B
                        where X.WarehouseID = A.WarehouseID
                        and A.CategoryID = B.CategoryID);
                        
-- Q1: Tổng số lượng hàng của từng kho
select B.WarehouseID, B.WarehouseAddress, sum(A.Quantity) as 'Total'
from Instock A, Warehouse B
where A.WarehouseID = B.WarehouseID
group by B.WarehouseID, B.WarehouseAddress;

-- Q2: Tính trị giá của từng kho
select A.WarehouseID, sum(A.Quantity * B.UnitPrice) as 'Total'
from Instock A, Product B
where A.ProductID = B.ProductID
group by A.WarehouseID;
