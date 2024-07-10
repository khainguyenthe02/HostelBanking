CREATE DATABASE HostelBanking;
go
USE HostelBanking;
go
BACKUP DATABASE HostelBanking
TO DISK = 'D:\DATN\HostelBanking.bak'
WITH FORMAT,
MEDIANAME = 'SQLServerBackups',
NAME = 'Full Backup of HostelBanking';
go
CREATE TABLE roles (
    id INT IDENTITY(1,1) PRIMARY KEY,
    role_name NVARCHAR(50) NOT NULL,
	information nvarchar(max) not null,
	delete_flag bit
);
go 
CREATE TABLE hostel_type (
    id INT IDENTITY(1,1) PRIMARY KEY,
    hostel_type_name NVARCHAR(50) NOT NULL,
	information nvarchar(max) not null,
	delete_flag bit
);
go
CREATE TABLE account (
    id INT IDENTITY(1,1) PRIMARY KEY,
    email VARCHAR(255) NOT NULL,
	password varchar(50) not null,
    full_name NVARCHAR(255) NOT NULL,
    user_address NVARCHAR(MAX) NOT NULL,
    phone_number VARCHAR(10) NOT NULL,
    status_account INT NOT NULL,
    role_id INT NOT NULL,
	 create_date date,
	 invalid_password_count INT,
	delete_flag bit,
    CONSTRAINT fk_account_role FOREIGN KEY (role_id) REFERENCES roles(id)
); 
drop table account
go
CREATE TABLE post (
    id INT IDENTITY(1,1) PRIMARY KEY,
    hostel_type_id INT NOT NULL,
    account_id INT NOT NULL,
    title NVARCHAR(255) NOT NULL,
    price FLOAT NOT NULL,
    acreage INT NOT NULL,
    dictrict NVARCHAR(255) NOT NULL,
    ward NVARCHAR(255) NOT NULL,
    description_post NTEXT,
    images VARCHAR(MAX) NOT NULL,
    create_date DATETIME NOT NULL,
    phone_number VARCHAR(10) NOT NULL,
    owner_house NVARCHAR(255) NOT NULL,
    modified_date DATETIME NOT NULL,
	payment_type INT NOT NULL,
	delete_flag bit,
    CONSTRAINT fk_post_hostelType FOREIGN KEY (hostel_type_id) REFERENCES hostel_type(id),
    CONSTRAINT fk_post_account FOREIGN KEY (account_id) REFERENCES account(id)
);
go
--create table post_image 
--(
--	id INT IDENTITY(1,1) primary key not null,
--	post_id int not null,
--	image_name varchar(100),
--	image_url varchar(max),
--	delete_flag bit,
--	FOREIGN KEY (post_id) REFERENCES post(id)
--);
go
CREATE TABLE comment (
    id INT IDENTITY(1,1) PRIMARY KEY,
    account_id INT NOT NULL,
    post_id INT NOT NULL,
    comment NVARCHAR(MAX) NOT NULL,
    create_date DATETIME NOT NULL,
	delete_flag bit,
    CONSTRAINT fk_comment_account FOREIGN KEY (account_id) REFERENCES account(id),
    CONSTRAINT fk_comment_post FOREIGN KEY (post_id) REFERENCES post(id)
);
go
CREATE TABLE report (
    id INT IDENTITY(1,1) PRIMARY KEY,
    account_id INT NOT NULL,
    post_id INT NOT NULL,
    report_status INT NOT NULL,
    detail NVARCHAR(MAX) NOT NULL,
    create_date DATETIME NOT NULL,
	delete_flag bit,
    CONSTRAINT fk_report_account FOREIGN KEY (account_id) REFERENCES account(id),
    CONSTRAINT fk_report_post FOREIGN KEY (post_id) REFERENCES post(id)
);
go
CREATE TABLE favorite (
    id INT IDENTITY(1,1) PRIMARY KEY,
    post_id INT NOT NULL,
    account_id INT NOT NULL,
	delete_flag bit,
    CONSTRAINT fk_favorite_post FOREIGN KEY (post_id) REFERENCES post(id),
    CONSTRAINT fk_favorite_account FOREIGN KEY (account_id) REFERENCES account(id)
);
go
CREATE TABLE pay_history (
    id INT IDENTITY(1,1) PRIMARY KEY,
    post_id INT NOT NULL,
    account_id INT NOT NULL,
    pay_date DATETIME NOT NULL,
    type INT NOT NULL,
    price FLOAT NOT NULL,
	delete_flag bit,
    CONSTRAINT fk_posthistory_account FOREIGN KEY (post_id) REFERENCES post(id),
    CONSTRAINT fk_payhistory_Account FOREIGN KEY (account_id) REFERENCES account(id)
);
go