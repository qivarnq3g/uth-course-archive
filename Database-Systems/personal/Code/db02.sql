create database DB02;
use db02;

create table Category
(
CategoryID char(3) primary key,
CategoryName varchar(50)
);

create table Product 
(
ProductID char(3) primary key,
ProductName varchar(50),
UnitPrice int,
CategoryID char(3)
);

create table Invoice
(
InvoiceID char(3) primary key,
InvoiceDate date,
Description varchar(100)
);

create table InvoiceDetail
(
InvoiceID char(3),
ProductID char(3),
Quantity int
);

insert into Category values 
('C01', 'Electronics'), 
('C02', 'Clothing'), 
('C03', 'Food & Beverages'), 
('C04', 'Home appliances'), 
('C05', 'Books & Stationery');

insert into Product values 
('P01', 'Laptop', 1000, 'C01'), 
('P02', 'Smartphone', 800, 'C01'), 
('P03', 'Headphones', 50, 'C01'), 
('P04', 'Monitor', 150, 'C01'), 
('P05', 'Charger', 20, 'C01'), 
('P06', 'T-Shirt', 15, 'C02'), 
('P07', 'Jeans', 40, 'C02'), 
('P08', 'Jacket', 60, 'C02'), 
('P09', 'Sneakers', 75, 'C02'), 
('P10', 'Hat', 10, 'C02'), 
('P11', 'Soda', 2, 'C03'), 
('P12', 'Chips', 3, 'C03'), 
('P13', 'Bread', 1, 'C03'), 
('P14', 'Milk', 2, 'C03'), 
('P15', 'Chocolate', 5, 'C03'), 
('P16', 'Blender', 35, 'C04'), 
('P17', 'Microwave', 90, 'C04'), 
('P18', 'Fan', 30, 'C04'), 
('P19', 'Iron', 25, 'C04'), 
('P20', 'Toaster', 28, 'C04'), 
('P21', 'Notebook', 3, 'C05'), 
('P22', 'Pen Set', 2, 'C05'), 
('P23', 'Calculator', 15, 'C05'), 
('P24', 'Marker Pack', 4, 'C05'), 
('P25', 'Highlighter', 3, 'C05'), 
('P26', 'Power Bank', 45, 'C01'), 
('P27', 'Hoodie', 50, 'C02'), 
('P28', 'Coffee', 6, 'C03'), 
('P29', 'Air Fryer', 120, 'C04'), 
('P30', 'Planner', 7, 'C05');

insert into Invoice values 
('I01', '2023-01-15', 'New Year Sale'), 
('I02', '2023-03-22', 'Spring Discount'), 
('I03', '2023-07-05', 'Summer Sale'), 
('I04', '2023-11-11', 'Singles Day Deals'), 
('I05', '2024-02-14', 'Valentine Offer'), 
('I06', '2024-10-01', 'National Day Promotions'), 
('I07', '2025-04-30', 'Flash Sale'), 
('I08', '2025-05-20', 'Summer Pre-Sale'), 
('I09', '2025-08-15', 'Back to School'), 
('I10', '2025-12-25', 'Christmas Mega Sale');

insert into InvoiceDetail values 
('I01', 'P01', 1), 
('I01', 'P06', 2), 
('I01', 'P12', 3), 
('I02', 'P08', 1), 
('I02', 'P14', 2), 
('I02', 'P17', 1), 
('I02', 'P22', 4), 
('I03', 'P04', 2), 
('I03', 'P09', 1), 
('I03', 'P18', 1), 
('I04', 'P02', 1), 
('I04', 'P10', 2), 
('I05', 'P13', 5), 
('I05', 'P25', 3), 
('I06', 'P07', 2), 
('I06', 'P21', 2), 
('I06', 'P23', 1), 
('I07', 'P03', 3), 
('I07', 'P19', 2), 
('I08', 'P05', 1), 
('I08', 'P15', 2), 
('I08', 'P29', 1), 
('I09', 'P20', 2), 
('I09', 'P27', 1), 
('I09', 'P28', 2), 
('I10', 'P11', 3), 
('I10', 'P24', 2), 
('I10', 'P26', 1), 
('I10', 'P30', 2);

-- 5.1. Find the products that belong to category ID ”CO1”.
select ProductName
from Product
where CategoryID = 'C01';

-- 5.2. Find the products (ID, name and price) that belong to category ID ”CO2”.
select ProductID, ProductName, UnitPrice
from Product
where CategoryID = 'C02';

-- 5.3. Find the products (*) with a unit price between 10 and 50.
select *
from Product
where UnitPrice >= 10 and UnitPrice <= 50;

-- Câu 1: Hãy liệt kê các hoá đơn trong Q1 2025
-- Update Invoice
-- Set InvoiceDate = '2025-01-10'
-- Where InvoiceID = 'I01'

update Invoice
set InvoiceDate = '2025-01-10'
where InvoiceId = 'I01';

select *
from Invoice
where quarter(InvoiceDate) = 1
	and year(InvoiceDate) = 2025;

-- Câu 2: Hãy liệt kê các mặt hàng (Mã số) của các hoá đơn trong tháng 2 năm 2025
update Invoice
set InvoiceDate = '2025-02-10'
where InvoiceId = 'I01';
update Invoice
set InvoiceDate = '2025-02-14'
where InvoiceId = 'I02';
update Invoice
set InvoiceDate = '2025-02-12'
where InvoiceId = 'I03';
update Invoice
set InvoiceDate = '2025-03-07'
where InvoiceID in (
    'I01', 'I02', 'I03'
);

select ProductID
from Invoice A, InvoiceDetail B
where A.InvoiceID = B.InvoiceID
  and month(InvoiceDate) = 2
  and year(InvoiceDate) = 2025;
  
-- Câu 3: Các mặt hàng (Mã số, tên, số lượng) của các hoá đơn trong năm 2025
select C.ProductID, ProductName, Quantity
from Invoice A, InvoiceDetail B, Product C
where A.InvoiceID = B.InvoiceID
  and B.ProductID = C.ProductID
  and year(InvoiceDate) = 2025;
  
-- Câu 4: Các loại hàng (*) được đặt trong các hoá đơn của năm 2025
select distinct A.*
from Category A, Product B, InvoiceDetail C, Invoice D
where A.CategoryID = B.CategoryID
	and B.ProductID = C.ProductID
    and C.InvoiceID = D.InvoiceID
    and year(InvoiceDate) = 2025;

select A, count(*)
from R
group by A;

-- Số lượng các mặt hàng của từng loại hàng
select CategoryID, count(*)
from Product
group by CategoryID;

-- 5.7. List the total quantity of each invoice.
select InvoiceID, sum(Quantity) as 'Total'
from InvoiceDetail
group by InvoiceID;

-- 5.8. List the total quantity of each invoice on date d.
select A.InvoiceID, sum(B.Quantity) as 'Total'
from Invoice A, InvoiceDetail B
where A.InvoiceID = B.InvoiceID
    and InvoiceDate = '2025-03-07'
group by A.InvoiceID;

-- 5.9. Calculate the total Cost (quantity times unit price) for each invoice.
select InvoiceID, sum(Quantity * UnitPrice) as 'Total Cost'
from InvoiceDetail A, Product B
where A.ProductID = B.ProductID
group by InvoiceID;

-- Tính trị giá của từng hoá đơn được lặp trong ngày '2025-03-07'
select A.InvoiceID, sum(Quantity * UnitPrice) as 'Total Cost'
from InvoiceDetail A, Product B, Invoice C
where A.ProductID = B.ProductID
	and A.InvoiceID = C.InvoiceID
    and InvoiceDate = '2025-03-07'
group by A.InvoiceID;
