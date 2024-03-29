-- CREATE DATABASE QUICK_INVOICE_BD;

USE QUICK_INVOICE_BD;

CREATE TABLE "USER" (
	ID INT IDENTITY,
	"USER_NAME" VARCHAR(100) NOT NULL,
	"PASSWORD" VARCHAR(150) NOT NULL,
	ACTIVE BIT DEFAULT 1 NOT NULL

	CONSTRAINT PK_USER PRIMARY KEY (ID),
	CONSTRAINT UQ_USER_USER_NAME UNIQUE ("USER_NAME")
);

CREATE TABLE PRODUCT (
	CODE VARCHAR(500) NOT NULL,
	"DESCRIPTION" VARCHAR(500) NOT NULL,
	PRICE DECIMAL(10,2) NOT NULL,
	APPLY_IVA BIT DEFAULT 0 NOT NULL

	CONSTRAINT PK_PRODUCT PRIMARY KEY (CODE)
);

CREATE TABLE SALE (
	ID INT IDENTITY,
	"USER_ID" INT,
	TOTAL_AMOUNT DECIMAL(10,2),
	"DATE" DATETIME DEFAULT GETDATE(),
	
	CONSTRAINT PK_SALE PRIMARY KEY (ID),
	CONSTRAINT FK_SALE_USER FOREIGN KEY ("USER_ID") REFERENCES "USER" (ID)
);

CREATE TABLE SALE_PRODUCT (
	SALE_ID INT,
	PRODUCT_CODE VARCHAR(500),
	"DESCRIPTION" VARCHAR(500) NOT NULL,
	PRICE DECIMAL(10,2),
	APPLY_IVA BIT DEFAULT 1 NOT NULL,
	QUANTITY INT NOT NULL

	CONSTRAINT PK_SALE_PRODUCT PRIMARY KEY (SALE_ID, PRODUCT_CODE),
	CONSTRAINT FK_SALE_PRODUCT_SALE FOREIGN KEY (SALE_ID) REFERENCES SALE (ID)
);

USE QUICK_INVOICE_BD;

INSERT INTO [USER] (USER_NAME, PASSWORD)
	 VALUES 
		   ('admin', '8c6976e5b5410415bde908bd4dee15dfb167a9c873fc4bb8a81f6f2ab448a918')

INSERT INTO PRODUCT (CODE,[DESCRIPTION],PRICE)
     VALUES
           ('1001010056','Laptop HP VICTUS',748375),
           ('1002020076','Laptop Acer Aspire',369900),
           ('9008020126','Pantalla Smart TV Samsung',999900),
           ('5001110454','Celular Apple iPhone 11',379900.55)
GO
