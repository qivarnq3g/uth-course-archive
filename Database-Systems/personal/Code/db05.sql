create database DB05;
use DB05;

create table Country
(
	Countryid char(3) primary key,
    countryname varchar(40)
);


create table Province
(
	provinceid char(3) primary key,
    provincename varchar(40),
    population int,
    area decimal(8,2),
    countryid char(3)
);

create table Border
(
	provinceid char(3),
    nationid char(3),
    primary key(provinceid,nationid)
);

create table neighbor
(
		provinceid char(3),
        neighborid char(3),
        primary key(provinceid,neighborid)
);



INSERT INTO Country (Countryid, countryname) VALUES
('M01', 'Miền Bắc'),
('M02', 'Miền Trung'),
('M03', 'Miền Nam'),
('CHI', 'Trung Quốc'),
('LAO', 'Lào'),
('CAM', 'Campuchia');

INSERT INTO Province (provinceid, provincename, population, area, countryid) VALUES
('T01', 'Hà Nội', 8360, 3359.84, 'M01'),
('T02', 'Vĩnh Phúc', 1190, 1235.82, 'M01'),
('T03', 'Thái Nguyên', 1320, 3536.46, 'M01'),
('T04', 'Bắc Giang', 1890, 3895.59, 'M01'),
('T05', 'Điện Biên', 624, 9541.25, 'M01'),
('T06', 'Sơn La', 1290, 14123.49, 'M01'),
('T07', 'Hòa Bình', 870, 4590.69, 'M01'),
('T08', 'Quảng Ninh', 1350, 6178.22, 'M01'),
('T09', 'Hải Phòng', 2070, 1561.76, 'M01'),
('T10', 'Thanh Hóa', 3710, 11114.71, 'M02'),
('T11', 'Nghệ An', 3410, 16486.49, 'M02'),
('T12', 'Hà Tĩnh', 1310, 5997.14, 'M02'),
('T13', 'Quảng Bình', 910, 8065.27, 'M02'),
('T14', 'Quảng Trị', 650, 4739.82, 'M02'),
('T15', 'Thừa Thiên Huế', 1150, 4902.40, 'M02'),
('T16', 'Đà Nẵng', 1220, 1284.73, 'M02'),
('T17', 'Quảng Nam', 1510, 10574.74, 'M02'),
('T18', 'Kon Tum', 560, 9674.18, 'M02'),
('T19', 'Gia Lai', 1560, 15510.13, 'M02'),
('T20', 'Đắk Lắk', 1900, 13075.37, 'M02'),
('T21', 'Bình Định', 1500, 6066.21, 'M02'),
('T22', 'Phú Yên', 900, 5025.96, 'M02'),
('T23', 'Khánh Hòa', 1250, 5137.77, 'M02'),
('T24', 'Ninh Thuận', 600, 3355.45, 'M02'),
('T25', 'Bình Thuận', 1250, 7992.40, 'M02'),
('T26', 'TP. Hồ Chí Minh', 9230, 2095.39, 'M03'),
('T27', 'Bà Rịa - Vũng Tàu', 1170, 1982.56, 'M03'),
('T28', 'Bình Dương', 2680, 2694.64, 'M03'),
('T29', 'Đồng Nai', 3240, 5905.70, 'M03'),
('T30', 'Tây Ninh', 1180, 4041.65, 'M03'),
('T31', 'Long An', 1750, 4495.04, 'M03'),
('T32', 'An Giang', 1910, 3536.70, 'M03'),
('T33', 'Kiên Giang', 1730, 6349.00, 'M03'),
('T34', 'Cà Mau', 1190, 5221.20, 'M03');

INSERT INTO Border (provinceid, nationid) VALUES
('T05', 'CHI'),
('T08', 'CHI'),
('T35', 'CHI'),
('T36', 'CHI'),
('T37', 'CHI'),
('T38', 'CHI'),
('T39', 'CHI'),

('T05', 'LAO'),
('T06', 'LAO'),
('T10', 'LAO'),
('T11', 'LAO'),
('T12', 'LAO'),
('T13', 'LAO'),
('T14', 'LAO'),
('T15', 'LAO'),
('T17', 'LAO'),
('T18', 'LAO'),

('T18', 'CAM'),
('T19', 'CAM'),
('T20', 'CAM'),
('T30', 'CAM'),
('T31', 'CAM'),
('T32', 'CAM'),
('T33', 'CAM'),
('T42', 'CAM'),
('T43', 'CAM'),
('T44', 'CAM');


INSERT INTO neighbor (provinceid, neighborid) VALUES
('T01', 'T02'), ('T01', 'T03'), ('T01', 'T04'), ('T01', 'T07'),
('T02', 'T01'), ('T02', 'T03'),
('T03', 'T01'), ('T03', 'T02'), ('T03', 'T04'),
('T04', 'T01'), ('T04', 'T03'), ('T04', 'T08'),
('T05', 'T06'),
('T06', 'T05'), ('T06', 'T07'), ('T06', 'T10'),
('T07', 'T01'), ('T07', 'T06'), ('T07', 'T10'),
('T08', 'T04'), ('T08', 'T09'),
('T09', 'T08'),
('T10', 'T06'), ('T10', 'T07'), ('T10', 'T11'),
('T11', 'T10'), ('T11', 'T12'),
('T12', 'T11'), ('T12', 'T13'),
('T13', 'T12'), ('T13', 'T14'),
('T14', 'T13'), ('T14', 'T15'),
('T15', 'T14'), ('T15', 'T16'), ('T15', 'T17'),
('T16', 'T15'), ('T16', 'T17'),
('T17', 'T15'), ('T17', 'T16'), ('T17', 'T18'), ('T17', 'T21'),
('T18', 'T17'), ('T18', 'T19'),
('T19', 'T18'), ('T19', 'T20'), ('T19', 'T21'), ('T19', 'T22'),
('T20', 'T19'), ('T20', 'T22'), ('T20', 'T23'),
('T21', 'T17'), ('T21', 'T19'), ('T21', 'T22'),
('T22', 'T19'), ('T22', 'T20'), ('T22', 'T21'), ('T22', 'T23'),
('T23', 'T20'), ('T23', 'T22'), ('T23', 'T24'),
('T24', 'T23'), ('T24', 'T25'),
('T25', 'T24'), ('T25', 'T29'),
('T26', 'T27'), ('T26', 'T28'), ('T26', 'T29'), ('T26', 'T30'), ('T26', 'T31'),
('T27', 'T26'), ('T27', 'T29'),
('T28', 'T26'), ('T28', 'T29'), ('T28', 'T30'),
('T29', 'T25'), ('T29', 'T26'), ('T29', 'T27'), ('T29', 'T28'),
('T30', 'T26'), ('T30', 'T28'), ('T30', 'T31'),
('T31', 'T26'), ('T31', 'T30'), ('T31', 'T32'),
('T32', 'T31'), ('T32', 'T33'),
('T33', 'T32'), ('T33', 'T34'),
('T34', 'T33');

