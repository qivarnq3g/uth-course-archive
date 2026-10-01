create database DB04;
use DB04;

create table Category (
  CategoryID char(3) primary key,
  CategoryName varchar(50)
);

create table Product (
  ProductID char(4) primary key,
  ProductName varchar(50),
  UnitPrice int,
  CategoryID char(3)
);

create table Customer (
  CustomerID char(4) primary key,
  CustomerName varchar(50),
  CustomerAddress varchar(100)
);

create table `Order` (
  OrderID char(4) primary key,
  OrderDate date,
  RequiredDate date,
  CustomerID char(4)
);

create table OrderDetail (
  OrderID char(4),
  ProductID char(4),
  OrderQuantity int,
  primary key (OrderID, ProductID)
);

create table Delivery (
  DeliveryID char(4) primary key,
  DeliveryDate date,
  OrderID char(4)
);

create table DeliveryDetail (
  DeliveryID char(4),
  ProductID char(4),
  DeliveryQuantity int,
  primary key (DeliveryID, ProductID)
);

insert into Category values 
('C01', 'Beverage'),
('C02', 'Snack'),
('C03', 'Electronics');

insert into Product values 
('P001', 'iPhone 15 Pro', 1200, 'C01'),
('P002', 'Samsung Galaxy S24', 1100, 'C01'),
('P003', 'Lenovo Legion 7', 1800, 'C02'),
('P004', 'Lenovo LOQ 15', 1300, 'C02'),
('P005', 'MacBook Air M3', 1400, 'C02'),
('P006', 'iPad Pro M4', 1000, 'C03'),
('P007', 'Samsung Galaxy Tab S9', 950, 'C03'),
('P008', 'Asus ROG Phone 8', 1150, 'C01'),
('P009', 'Dell XPS 13', 1250, 'C02'),
('P010', 'Huawei MatePad Pro', 800, 'C03');

insert into Customer values 
('M01', 'Alice', '123 Main St'),
('M02', 'Bob', '456 Elm St'),
('M03', 'Charlie', '789 Oak St'),
('M04', 'David', '101 North Rd'),
('M05', 'Eva', '202 West Blvd'),
('M06', 'Frank', '303 East Ln');

insert into `Order` values 
('D01', '2025-01-10', '2025-01-15', 'M01'),
('D02', '2025-05-20', '2025-05-25', 'M02'),
('D03', '2024-12-30', '2025-01-05', 'M03'),
('D04', '2025-03-05', '2025-03-10', 'M04'),
('D05', '2025-04-15', '2025-04-20', 'M05'),
('D06', '2025-06-01', '2025-06-05', 'M06'),
('D07', '2025-06-10', '2025-06-15', 'M01'),
('D08', '2025-06-12', '2025-06-18', 'M02'),
('D09', '2025-06-13', '2025-06-17', 'M03'),
('D10', '2025-06-14', '2025-06-20', 'M04');

insert into OrderDetail values 
('D01', 'P001', 2),
('D01', 'P002', 3),
('D02', 'P003', 1),
('D02', 'P004', 5),
('D03', 'P005', 4),
('D04', 'P001', 3),
('D05', 'P002', 2),
('D06', 'P006', 1),
('D07', 'P007', 10),
('D08', 'P008', 1),
('D09', 'P009', 1),
('D10', 'P010', 6);

insert into Delivery values 
('V01', '2025-01-12', 'D01'),
('V02', '2025-05-22', 'D02'),
('V03', '2025-01-03', 'D03'),
('V04', '2025-03-07', 'D04'),
('V05', '2025-04-18', 'D05');

insert into DeliveryDetail values 
('V01', 'P001', 2),
('V01', 'P002', 3),
('V02', 'P003', 1),
('V02', 'P004', 5),
('V03', 'P005', 4),
('V04', 'P001', 3),
('V05', 'P002', 2);

-- Tính trị giá của từng đơn đặt hàng
select A.OrderID, sum(B.UnitPrice * A.OrderQuantity) as TotalValue
from OrderDetail A, Product B
where A.ProductID = B.ProductID
group by A.OrderID;

-- Tính trị giá của từng đợt giao hàng 
select A.DeliveryID, sum(B.UnitPrice * A.DeliveryQuantity) as TotalValue
from DeliveryDetail A, Product B
where A.ProductID = B.ProductID
group by A.DeliveryID;

-- Đơn đặt hàng D01 được giao mấy lần
select count(*) as DeliveryCount
from Delivery A
where A.OrderID = 'D01';

-- Đơn đặt hàng nào chưa được giao
select A.OrderID
from `Order` A
where A.OrderID not in (
  select B.OrderID
  from Delivery B
);

-- Tính trị giá của từng đơn đặt hàng được đặt trong Quý 1 năm 2025
select A.OrderID,
       sum(A.OrderQuantity * B.UnitPrice) as Cost
from OrderDetail A, Product B, `Order` C
where A.ProductID = B.ProductID
  and A.OrderID = C.OrderID
  and Quarter(C.OrderDate) = 1
  and Year(C.OrderDate) = 2025
group by A.OrderID;
