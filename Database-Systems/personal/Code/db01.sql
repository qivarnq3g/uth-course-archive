create database DB01;
use db01;

create table Subject
(
	SubjectID char(3) primary key,
    SubjectName varchar(30),
    Unit int
);

create table Class
(
	ClassID char(30) primary key,
    ClassName varchar(30),
    ClassYear char(30)
);

create table Student
(
	StudentID char(3) primary key,
    StudentName varchar(30),
    StudentAddress varchar(30),
    ClassID char(30)
);

create table StudentGrade
(
	StudentID char(3),
    SubjectID char(3),
    Grade float
);


insert into Subject
values
	('S01', 'XSTK', 3),
    ('S02', 'KTLT', 4),
    ('S03', 'CTRR', 4),
    ('S04', 'KTMT', 3),
    ('S05', 'CSDL', 3);
    
insert into Class
values
    ('C01', 'CN1', '2024'),
    ('C02', 'CN2', '2024'),
    ('C03', 'CN3', '2024');
    
insert into Student
values
    ('T01', 'Nguyễn Minh An', 'TP.HCM', 'C01'),
    ('T02', 'Trần Quốc Bảo', 'Hà Nội', 'C02'),
    ('T03', 'Lê Khánh Chi', 'Đã Nẵng', 'C03'),
    ('T04', 'Phạm Đức Duy', 'Hải Phòng', 'C01'),
    ('T05', 'Vũ Hoàng Giang', 'Huế', 'C02'),
    ('T06', 'Bùi Thị Hà', 'TP.HCM', 'C03'),
    ('T07', 'Đặng Tuấn Hùng', 'Cần Thơ', 'C01'),
    ('T08', 'Hồ Thảo Khánh', 'Đăk Lăk', 'C02'),
    ('T09', 'Dương Hồng Lan', 'Nghệ An', 'C03'),
    ('T10', 'Nguyễn Nhật Minh', 'Bình Dương', 'C01'),
    ('T11', 'Trịnh Thanh Ngọc', 'TP.HCM', 'C02'),
    ('T12', 'Lý Ngọc Oanh', 'Bạc Liêu', 'C03'),
    ('T13', 'Phan Anh Phúc', 'Bắc Ninh', 'C01'),
    ('T14', 'Cao Hữu Quang', 'Hà Nội', 'C02'),
    ('T15', 'Đinh Mai Thảo', 'TP.HCM', 'C03'),
    ('T16', 'Tô Kim Trang', 'Long An', 'C01'),
    ('T17', 'Nguyễn Thị Uyên', 'Đồng Nai', 'C02'),
    ('T18', 'Lâm Quốc Việt', 'Quảng Nam', 'C03'),
    ('T19', 'Hoàng Lan Xuân', 'Hà Tĩnh', 'C01'),
	('T20', 'Mai Thuỳ Yến', 'TP.HCM', 'C02');
    
insert into StudentGrade
values
	('T01', 'S01', 6.0),
	('T01', 'S02', 7.5),
	('T01', 'S03', 8.0),
	('T01', 'S04', 6.5),
	('T01', 'S05', 9.0),
	('T02', 'S01', 5.0),
	('T02', 'S02', 5.5),
	('T02', 'S03', 6.0),
	('T02', 'S04', 5.5),
	('T02', 'S05', 7.0),
	('T03', 'S01', 4.0),
	('T03', 'S02', 5.5),
	('T03', 'S03', 3.0),
	('T03', 'S04', 6.5),
	('T03', 'S05', 7.0),
	('T04', 'S01', 2.5),
	('T04', 'S02', 6.0),
	('T04', 'S03', 4.0),
	('T04', 'S04', 7.0),
	('T04', 'S05', 5.5),
	('T05', 'S01', 8.0),
	('T05', 'S02', 9.0),
	('T05', 'S03', 6.5),
	('T05', 'S04', 7.5),
	('T05', 'S05', 8.0),
	('T06', 'S02', 6.0),
	('T06', 'S03', 5.5),
	('T07', 'S03', 4.0),
	('T08', 'S01', 7.0),
	('T08', 'S05', 5.0),
	('T09', 'S04', 8.0),
	('T10', 'S02', 3.5),
	('T10', 'S03', 6.0),
	('T11', 'S01', 4.5),
	('T11', 'S02', 5.0),
	('T12', 'S03', 7.0),
	('T13', 'S05', 6.5),
	('T14', 'S04', 5.5),
	('T14', 'S01', 7.0),
	('T15', 'S02', 4.0),
	('T16', 'S01', 3.0),
	('T16', 'S02', 4.5),
	('T16', 'S03', 2.0),
	('T16', 'S04', 4.0),
	('T16', 'S05', 3.5);

select * from StudentGrade;

select StudentID
from StudentGrade
group by StudentID
having count(*) = 5 and max(Grade) < 5;

select *
from StudentGrade
where StudentID in (
    select StudentID
    from StudentGrade
    group by StudentID
    having count(*) = 5 and max(Grade) < 5
);



-- Câu 1: Danh sách các môn học
select SubjectID as 'Mã SV', SubjectName as 'Tên SV', unit as 'Số tín chỉ'
from Subject;

--  5.1. Find the students that belong to class ID ”C02”.
select *
from Student
where ClassID = 'C02';

select *
from Student
where ClassID = 'C01' or ClassID = 'C02';

select *
from Student
where ClassID in ('C01', 'C02');

select *
from Student
where ClassID not in ('C01', 'C02');

-- Câu 2: Các môn học có số tín chỉ > 3
select *
from Subject
where unit > 3;

-- Câu 3: Các môn học có số tín chỉ từ 3 đến 5
select *
from Subject
where unit >= 3 or unit <= 5;

-- Câu 4: Ai (mã số) thi được môn 'S01' điểm giỏi (điểm >= 9)
select StudenID
from StudentGrade
where Grade >= 9 and SubjectID = 'S01';

-- Câu 5: Ai (mã số) thi được môn 'S01' điểm giỏi (điểm >= 9) hay môn 'S02' điểm khá (điểm >= 7)
select StudentID
from StudentGrade
where (Grade >= 9 and SubjectID = 'S01') or (Grade >= 7 and SubjectID ='S02');

-- Phép thích đề cát (Decatesia product)
select *
from Class A, Student B;

-- Phép kết tự nhiên (Natural join)
select *
from Class A, Student B
where A.ClassID = B.ClassID;

-- 5.2. Find the students that belong to class name = ”Computer Science”.
select *
from Class A, Student B
where A.ClassID = B.ClassID
	and ClassName = 'Computer Science';
    
-- 5.3. Find the students (All information) whose class year is ”2023”.
select *
from Class A, Student B
where A.ClassID = B.ClassID
	and ClassYear = 2023;

-- 5.4. Find the Subject (Name and Unit) of the Subject ID “S01”.
select SubjectName, Unit
from Subject
where SubjectID = 'S01';

-- 5.5. What are the grade of student ”T02” for subject ”S02”?
select Grade
from StudentGrade
where StudentID = 'T02' and SubjectID = 'S02';

-- 5.6. Find the subjects (ID, Name, and Grade) in which student ”T02” failed.
select A.SubjectID, SubjectName, Grade
from StudentGrade A, Subject B
where StudentID = 'T02' and Grade < 5
	and A.SubjectID = B.SubjectID;
    
-- 5.7. Which subjects (*) did student ”T03” never take the exam for?
select *
from Subject
where SubjectID not in (
    select SubjectID
    from StudentGrade
    where StudentID = 'T03'
);

-- Danh sách các môn học (Mã số)
select SubjectID
from Subject;

-- Danh sách các môn học (Mã số) mà SV T03 đã thi
select SubjectID
from StudentGrade
where StudentID = 'T03';


-- 5.8. How many students are in each class?
select ClassID, count(*) as StudentCount
from Student
group by ClassID;

-- Chuyển đại số quan hệ sang SQL
select SubjectID, SubjectName, Grade
from Subject A, StudentGrade B
where A.SubjectID = B.SubjectID
    and Unit >= 3
    and StudentID = 'T01'
    and Grade >= 5;
    
-- Cho biết sĩ số của từng lớp (mã số và tên)
select A.ClassID, ClassName, count(*) as 'SS'
from Student A, Class B
where A.ClassID = B.ClassID
group by A.ClassID, ClassName;

-- 5.10. Calculate the GPA (Grade Point Average) of student ID ”T02”
select Round(avg(grade),2) as 'GPA of T02'
from StudentGrade
where StudentID = 'T02';

-- 5.11. Calculate the GPA for each student.
select StudentID, Round(avg(grade),2) as 'GPA'
from StudentGrade
group by StudentID;

-- Điểm trung bình của chỉ 3 em T01, T02, T03
select StudentID, Round(avg(grade),2) as 'GPA'
from StudentGrade
where StudentID in ('T01', 'T02', 'T03')
group by StudentID;

-- 5.12. Calculate the GPA of class ID ”C02”.
select avg(grade) as 'GPA of C02'
from StudentGrade A, Student B
where A.StudentID = B.StudentID
	and ClassID = 'C02';
    
CREATE TABLE R1 (
    A INT,
    B INT
);

CREATE TABLE R2 (
    B INT,
    C INT
);

INSERT INTO R1 
VALUES 
    (1, 2),
    (3, 4);

INSERT INTO R2
VALUES
	(2, 3),
    (5, 6);
    
SELECT *
FROM R1, R2;

SELECT *
FROM R1, R2
WHERE R1.B = R2.B;

SELECT *
FROM R1 JOIN R2 ON R1.B = R2.B;

SELECT *
FROM R1 LEFT JOIN R2 ON R1.B = R2.B;

SELECT *
FROM R1 RIGHT JOIN R2 ON R1.B = R2.B;

SELECT *
FROM R1 LEFT JOIN R2 ON R1.B = R2.B
UNION
SELECT *
FROM R1 RIGHT JOIN R2 ON R1.B = R2.B;

-- Q1: Các sinh viên (Mã số + tên) thi đạt môn "S01"
SELECT B.*
FROM StudentGrade A JOIN Student B ON A.StudentID = B.StudentID
WHERE SubjectID = 'S01' AND GRADE >= 5;

CREATE TABLE R
(
	A int,
    B int
);

CREATE TABLE S
(
	B int
);

INSERT INTO R
VALUES

	(1, 2),
    (1, 3),
    (2, 1),
    (2, 2),
    (2, 3),
    (3, 1),
    (3, 4);

INSERT INTO S
VALUES
	(1),
    (2);

SELECT DISTINCT A
FROM R X
WHERE NOT EXISTS (SELECT *
FROM S
WHERE B NOT IN (SELECT B
FROM R
WHERE X.A = A));

SELECT DISTINCT X.StudentID
FROM StudentGrade X
WHERE NOT EXISTS (
    SELECT *
    FROM Subject S
    WHERE S.SubjectID NOT IN (
        SELECT Y.SubjectID
        FROM StudentGrade Y
        WHERE Y.StudentID = X.StudentID));

CREATE TABLE R3
(
	A int,
    B int
);

INSERT INTO R3
VALUES

	(1, 2),
    (1, 3),
    (2, 1),
    (2, 3),
    (3, 1),
    (3, 4);

SELECT A, SUM(B)
FROM R3
GROUP BY A
HAVING SUM(B) >= ALL (SELECT SUM(B)
FROM R3
GROUP BY A);

-- Q2: Lớp nào có sĩ số lớn nhất
SELECT ClassID, COUNT(*) AS SS
FROM Student
GROUP BY ClassID
HAVING SS >= ALL (
    SELECT COUNT(*)
    FROM Student
    GROUP BY ClassID
);

-- Q3: Sinh viên nào có điểm trung bình lớn nhất
SELECT StudentID, ROUND(AVG(Grade), 2) AS DTB
FROM StudentGrade
GROUP BY StudentID
HAVING DTB >= ALL (
    SELECT AVG(Grade)
    FROM StudentGrade
    GROUP BY StudentID
);

