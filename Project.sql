create database Project;
use Project;

create table Employees (
    employee_id int auto_increment primary key,
    first_name varchar(50),
    last_name varchar(50),
    email varchar(100),
    phone varchar(20),
    hire_date date,
    salary int,
    is_full_time boolean,
    availability varchar(255)
);

create table Teachers (
    teacher_id int auto_increment primary key,
    first_name varchar(50),
    last_name varchar(50),
    email varchar(100),
    phone varchar(20),
    specialty varchar(100),
    foreign key (teacher_id) references Employees(employee_id) on delete cascade on update cascade
);

create table Janitors (
    janitor_id int auto_increment primary key,
    first_name varchar(50),
    last_name varchar(50),
    email varchar(100),
    phone varchar(20),
    foreign key (janitor_id) references Employees(employee_id) on delete cascade on update cascade
);

create table Administrative_Employees (
    administrative_employee_id int auto_increment primary key,
    first_name varchar(50),
    last_name varchar(50),
    email varchar(100),
    phone varchar(20),
    department varchar(100),
    foreign key (administrative_employee_id) references Employees(employee_id) on delete cascade on update cascade
);

create table Students (
    student_id int auto_increment primary key,
    first_name varchar(50),
    last_name varchar(50),
    age int,
    graduate boolean,
    contact_email varchar(100),
    contact_phone varchar(20),
    availability varchar(255)
);

create table Parents (
    parent_id int auto_increment primary key,
    first_name varchar(50),
    last_name varchar(50),
    student_id int,
    contact_email varchar(100),
    contact_phone varchar(20),
    foreign key (student_id) references Students(student_id) on delete cascade on update cascade
);

create table Courses (
    course_id int auto_increment primary key,
    course_name varchar(100),
    teacher_id int,
    schedule varchar(100),
    status boolean,
    foreign key (teacher_id) references Teachers(teacher_id) on delete cascade on update cascade
);

create table Items (
    item_id int auto_increment primary key,
    item_name varchar(100),
    quantity int,
    item_description varchar(255)
);

create table Enrollments (
    enrollment_id int auto_increment primary key,
    student_id int,
    course_id int,
    foreign key (student_id) references Students(student_id) on delete cascade on update cascade,
    foreign key (course_id) references Courses(course_id) on delete cascade on update cascade
);

create table Course_Item (
	course_item_id int auto_increment primary key,
	course_id int,
	item_id int,
	foreign key (course_id)	references Courses(course_id) on delete cascade on update cascade,
	foreign key (item_id) references Items(item_id) on delete cascade on update cascade
)

create table Incomes (
    income_id int auto_increment primary key,
    income_type varchar(50),
    income_amount DECIMAL(10, 5),
    income_date date,
    income_description varchar(255)
);

create table Expenses (
    expense_id int auto_increment primary key,
    expense_type varchar(50),
    expense_amount DECIMAL(10, 5),
    expense_date date,
    expense_description varchar(255)
);

create table RemovedEmployees (
    employee_id int auto_increment primary key,
    first_name varchar(50),
    last_name varchar(50),
    email varchar(100),
    phone varchar(20),
    hire_date date,
    salary int,
    is_full_time boolean,
    availability varchar(255)
);


CREATE TRIGGER tr_EmployeeRemoved
ON employees BEFORE  INSERT AS BEGIN
    INSERT INTO removedemployees (employee_id, first_name, last_name, email, phone, hire_date, salary, is_full_time, availability)
    SELECT employee_id, first_name, last_name, email, phone, hire_date, salary, is_full_time, availability
    FROM Employees;
END;
