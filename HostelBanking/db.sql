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
create table post_image 
(
	id INT IDENTITY(1,1) primary key not null,
	post_id int not null,
	image_name varchar(100),
	image_url varchar(max),
	delete_flag bit,
	FOREIGN KEY (post_id) REFERENCES post(id)
);
go
CREATE TABLE tblComment (
    PK_iCommentId INT IDENTITY(1,1) PRIMARY KEY,
    FK_iAccountId INT NOT NULL,
    FK_iPostId INT NOT NULL,
    sComment NVARCHAR(MAX) NOT NULL,
    dCreatedDate DATETIME NOT NULL,
	delete_flag bit,
    CONSTRAINT FK_Comment_Account FOREIGN KEY (FK_iAccountId) REFERENCES tblAccount(PK_iAccountId),
    CONSTRAINT FK_Comment_Post FOREIGN KEY (FK_iPostId) REFERENCES tblPost(PK_iPostId)
);
go
CREATE TABLE tblReport (
    PK_iReportId INT IDENTITY(1,1) PRIMARY KEY,
    FK_iAccountId INT NOT NULL,
    FK_iPostId INT NOT NULL,
    iReportStatus INT NOT NULL,
    sDetail NVARCHAR(MAX) NOT NULL,
    dCreatedDate DATETIME NOT NULL,
	delete_flag bit,
    CONSTRAINT FK_Report_Account FOREIGN KEY (FK_iAccountId) REFERENCES tblAccount(PK_iAccountId),
    CONSTRAINT FK_Report_Post FOREIGN KEY (FK_iPostId) REFERENCES tblPost(PK_iPostId)
);
go
CREATE TABLE tblFavorite (
    PK_iFavoriteId INT IDENTITY(1,1) PRIMARY KEY,
    FK_iPostId INT NOT NULL,
    FK_iAccountId INT NOT NULL,
	delete_flag bit,
    CONSTRAINT FK_Favorite_Post FOREIGN KEY (FK_iPostId) REFERENCES tblPost(PK_iPostId),
    CONSTRAINT FK_Favorite_Account FOREIGN KEY (FK_iAccountId) REFERENCES tblAccount(PK_iAccountId)
);
go
CREATE TABLE tblPayHistory (
    PK_iPayHistoryId INT IDENTITY(1,1) PRIMARY KEY,
    FK_iPostId INT NOT NULL,
    FK_iAccountId INT NOT NULL,
    dPayDate DATETIME NOT NULL,
    iType INT NOT NULL,
    fPrice FLOAT NOT NULL,
	delete_flag bit,
    CONSTRAINT FK_PayHistory_Post FOREIGN KEY (FK_iPostId) REFERENCES tblPost(PK_iPostId),
    CONSTRAINT FK_PayHistory_Account FOREIGN KEY (FK_iAccountId) REFERENCES tblAccount(PK_iAccountId)
);
go