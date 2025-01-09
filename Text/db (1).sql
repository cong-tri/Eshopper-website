CREATE DATABASE EShopper
USE EShopper
DROP DATABASE EShopper

----- START TABLE Categories ------
CREATE TABLE Categories (
   CAT_ID               INT                  IDENTITY(1, 1),
   CAT_Name             NVARCHAR(255)        NOT NULL,
   CAT_Description		NVARCHAR(255)        NOT NULL,
   CAT_Slug             NVARCHAR(255)        NULL,
   CAT_DisplayOrder     INT                  NOT NULL,
   CAT_Status			INT                  NOT NULL CHECK (CAT_Status IN (1, 2)), 
   /* 1: Active; 2: Inactive*/
   CreatedDate          DATETIME             NULL,
   CreatedBy            VARCHAR(255)         NULL,
   UpdatedDate          DATETIME             NULL,
   UpdatedBy            VARCHAR(255)         NULL,
   CONSTRAINT PK_CATEGORIES PRIMARY KEY (CAT_ID)
)
GO
----- END TABLE Categories -------

----- START TABLE Brands ------
CREATE TABLE Brands (
   BRA_ID               INT                  IDENTITY(1, 1),
   BRA_Name             NVARCHAR(255)        NOT NULL,
   BRA_Description		NVARCHAR(255)        NOT NULL,
   BRA_Slug             NVARCHAR(255)        NULL,
   BRA_DisplayOrder     INT                  NOT NULL,
   BRA_Status			INT                  NOT NULL CHECK (BRA_Status IN (1, 2)), /* 1: Active; 2: Inactive*/
   CreatedDate          DATETIME             NULL,
   CreatedBy            VARCHAR(255)         NULL,
   UpdatedDate          DATETIME             NULL,
   UpdatedBy            VARCHAR(255)         NULL,
   CONSTRAINT PK_BRANDS PRIMARY KEY (BRA_ID)
)
GO
----- END TABLE Brands ------

----- START TABLE Products ------
CREATE TABLE Products(
	PRO_ID				INT					IDENTITY(1, 1),
	CAT_ID				INT					NOT NULL,
	BRA_ID				INT					NOT NULL,
	PRO_Name			NVARCHAR(255)		NOT NULL,
	PRO_Description		NTEXT				NOT NULL,
	PRO_Slug			NVARCHAR(255)		NULL,
	PRO_Price			MONEY				NOT NULL,
	PRO_Image			VARCHAR(255)		NULL,
	PRO_Quantity		INT					NOT NULL,
	PRO_Status			INT					NOT NULL CHECK(PRO_Status BETWEEN 1 AND 5), 
	PRO_CapitalPrice	MONEY				NOT NULL,
	PRO_Sold			INT					NOT NULL,
	CreatedDate         DATETIME			NULL,
	CreatedBy           VARCHAR(255)        NULL,
	UpdatedDate         DATETIME            NULL,
	UpdatedBy           VARCHAR(255)        NULL,
	CONSTRAINT PK_PRODUCTS PRIMARY KEY (PRO_ID)
)
GO

--ALTER TABLE Products
--ADD PRO_Sold INT NOT NULL DEFAULT 0;
/* FOR PRO_Status: 1: In Stock; 2: Out of Stock; 3: Low Stock; 4: Back Order; 5: Pre-Order*/

/* CREATE FOREIGN KEY TO TABLE Categories*/
ALTER TABLE Products
   ADD CONSTRAINT FK_PRODUCT_RELATIONS_CATEGORI FOREIGN KEY (CAT_ID)
      REFERENCES Categories (CAT_ID)
GO
/* CREATE FOREIGN KEY TO TABLE Brands*/
ALTER TABLE Products
   ADD CONSTRAINT FK_PRODUCT_RELATIONS_BRAND FOREIGN KEY (BRA_ID)
      REFERENCES Brands (BRA_ID)
GO
----- END TABLE Products ------

----- START TABLE Coupons ------
CREATE TABLE Coupons (
	COUP_ID				INT					IDENTITY(1, 1),
	COUP_Name			NVARCHAR(255)		NOT NULL,
	COUP_Description	NVARCHAR(255)		NOT NULL,
	COUP_Status			INT					NOT NULL CHECK (COUP_Status BETWEEN 1 AND 8),
	COUP_Quantity		INT					NOT NULL,
	COUP_DateStart		DATETIME			NOT NULL,
	COUP_DateExpire		DATETIME			NOT NULL,
	CreatedDate         DATETIME            NULL,
	CreatedBy           VARCHAR(255)        NULL,
	UpdatedDate         DATETIME            NULL,
	UpdatedBy           VARCHAR(255)        NULL,
	CONSTRAINT PK_COUPONS PRIMARY KEY (COUP_ID)
)
GO
/* FOR COUP_Status:
	1: Active; 2: Inactive; 3: Expired; 4: Redeemed;
	5: Unpublished; 6: Limited; 7: Free Shipping; 8: Cash Back
*/
----- END TABLE Coupons ------

----- START TABLE Contact ------
CREATE TABLE Contact(
	CONT_ID				INT					IDENTITY(1, 1),
	CONT_Name			NVARCHAR(255)		NOT NULL,
	CONT_Description	NVARCHAR(255)		NOT NULL,
	CONT_Phone			VARCHAR(255)		NOT NULL,
	CONT_Email			NVARCHAR(255)		NOT NULL,
	CONT_Map			NVARCHAR(MAX)		NOT NULL,
	CONT_LogoImg		VARCHAR(255)		NOT NULL,
	CONT_Address		NVARCHAR(255)		NOT NULL,
	CreatedDate         DATETIME            NULL,
	CreatedBy           VARCHAR(255)        NULL,
	UpdatedDate         DATETIME            NULL,
	UpdatedBy           VARCHAR(255)        NULL,
	CONSTRAINT PK_CONTACT PRIMARY KEY (CONT_ID)
)
--DROP TABLE CONTACT
----- END TABLE Contact ------

----- START TABLE Payments -----
CREATE TABLE PayMents(
	PAY_ID				INT					IDENTITY(1, 1),
	PAY_Name			NVARCHAR(255)		NOT NULL,
	PAY_Description		NVARCHAR(255)		NOT NULL,
	PAY_Status			INT					NOT NULL CHECK(PAY_Status BETWEEN 1 AND 11),
	CreatedDate         DATETIME            NULL,
	CreatedBy           VARCHAR(255)        NULL,
	UpdatedDate         DATETIME            NULL,
	UpdatedBy           VARCHAR(255)        NULL,
	CONSTRAINT PK_PAYMENTS PRIMARY KEY (PAY_ID)
)
GO

/* FOR PAY_Status:
	1: Cash; 2: Banking; 3: Installment; 4: Pending; 
	5: Completed; 6: Failed; 7: Refunded; 8: Canceled;
	9: Verified; 10: Unverified, 11: Awaiting Confirmation
*/
----- END TABLE Payments -----

----- START TABLE Menus ------
CREATE TABLE Menus (
   MEN_ID               INT                  IDENTITY(1, 1),
   PARENT_ID            INT                  NULL,
   MEN_Title            NVARCHAR(255)        NOT NULL,
   MEN_DisplayOrder     INT                  NOT NULL,
   MEN_Icon				VARCHAR(255)		 NULL,
   MEN_Status			INT					 NOT NULL,
   -- 1: USER, 2: ADMIN
   MEN_Controller		NVARCHAR(255)		 NOT NULL,
   CreatedDate          DATETIME             NULL,
   CreatedBy            VARCHAR(255)         NULL,
   UpdatedDate          DATETIME             NULL,
   UpdatedBy            VARCHAR(255)         NULL,
   CONSTRAINT PK_MENUS PRIMARY KEY (MEN_ID)
)
GO

CREATE NONCLUSTERED INDEX RELATIONSHIP_1_FK ON Menus (PARENT_ID ASC)
GO

ALTER TABLE Menus
   ADD CONSTRAINT FK_MENUS_RELATIONS_MENUS FOREIGN KEY (PARENT_ID)
      REFERENCES Menus (MEN_ID)
GO

----- END TABLE Menus -------

----- START TABLE Banners -----
CREATE TABLE Banners(
	BAN_ID				INT					IDENTITY(1, 1),
	BAN_Title			NVARCHAR(255)		NOT NULL,
	BAN_Image			VARCHAR(255)		NOT NULL,
	BAN_Url				VARCHAR(255)		NULL,
	BAN_DisplayOrder	INT					NOT NULL,
	CreatedDate         DATETIME			NULL,
	CreatedBy           VARCHAR(255)        NULL,
	UpdatedDate         DATETIME            NULL,
	UpdatedBy           VARCHAR(255)        NULL,
   CONSTRAINT PK_BANNERS PRIMARY KEY (BAN_ID)
)
GO
----- END TABLE Banners -----

-----START TABLE AccountRoles-----
CREATE TABLE AccountRoles(
	ACR_ID				INT					IDENTITY(1,1),
	ACR_Name			NVARCHAR(255)		NOT NULL,
	ACR_Status			INT					NOT NULL,
	CreatedDate         datetime            null,
	CreatedBy           varchar(255)        null,
	UpdatedDate         datetime            null,
	UpdatedBy           varchar(255)        null,
	CONSTRAINT PK_ACCOUNTROLES PRIMARY KEY (ACR_ID)
)
-----END TABLE AccountRoles-----

-----START TABLE Accounts-----
CREATE TABLE Accounts (
   ACC_ID               int                  identity(1 ,1),
   ACC_Username         varchar(255)         not null,
   ACC_Password         nvarchar(255)        not null,
   ACC_DisplayName      nvarchar(255)        NOT NULL,
   ACC_Email            varchar(255)         NOT NULL,
   ACC_Phone            varchar(20)          NOT NULL,
   ACC_Status           INT			         null CHECK(ACC_Status IN (1, 2)),
   -- 1: Active, 2:In Active
   CreatedDate          datetime             null,
   CreatedBy            varchar(255)         null,
   UpdatedDate          datetime             null,
   UpdatedBy            varchar(255)         null,
   constraint PK_ACCOUNTS primary key (ACC_ID)
)
ALTER TABLE Accounts
	ADD CONSTRAINT UQ_ACC_Email UNIQUE (ACC_Email);

ALTER TABLE Accounts
	ADD CONSTRAINT UQ_ACC_Phone UNIQUE (ACC_Phone);

ALTER TABLE Accounts
	ADD CONSTRAINT UQ_ACC_DisplayName UNIQUE (ACC_DisplayName);

ALTER TABLE Accounts
	ADD CONSTRAINT UQ_ACC_Username UNIQUE (ACC_Username);
-----END TABLE Accounts-----

-----START TABLE AccountStatusLogins-----

CREATE TABLE AccountStatusLogins (
	ACSL_ID					  INT					IDENTITY(1,1),
	ACC_ID					  INT					NOT NULL,
	ACSL_JwtToken			  NVARCHAR(MAX)			NOT NULL,
	ACSL_Status				  INT					NOT NULL,
	--- Active = 1, Inactive = 2
	ACSL_DatetimeLogin		  DATETIME				NOT NULL,
	ACSL_ExpiredDatetimeLogin DATETIME				NULL,
	CreatedDate			      datetime              null,
	CreatedBy				  varchar(255)			null,
	UpdatedDate				  datetime              null,
	UpdatedBy                 varchar(255)          null,
	constraint PK_ACCOUNTSTATUSLOGINS primary key (ACSL_ID),
	FOREIGN KEY(ACC_ID) REFERENCES Accounts(ACC_ID)
)
-----END TABLE AccountStatusLogins-----

-----START TABLE AccountLogins-----
CREATE TABLE AccountLogins (
    LoginProvider			NVARCHAR (100)		NOT NULL,
    ProviderKey				NVARCHAR (100)		NOT NULL,
    ProviderDisplayName		NVARCHAR (255)		NULL,
    ACC_ID					INT					NOT NULL,
	CreatedDate			    datetime            null,
	CreatedBy				varchar(255)		null,
	UpdatedDate				datetime            null,
	UpdatedBy               varchar(255)        null,
    CONSTRAINT PK_ACCOUNTLOGINS PRIMARY KEY CLUSTERED (LoginProvider ASC, ProviderKey ASC),
    FOREIGN KEY(ACC_ID) REFERENCES Accounts(ACC_ID)
);
-----END TABLE AccountLogins-----

-----START TABLE Members-----
CREATE TABLE Members (
   MEM_ID               int                  IDENTITY(1,1),
   ACC_ID				INT					 NOT NULL,
   ACR_ID				INT					 NOT NULL DEFAULT 1,
   --- 1: USER, 2: ADMIN, 3: SHOP OWNER
   MEM_LastName         nvarchar(255)        null,
   MEM_FirstName        nvarchar(255)        null,
   MEM_Gender           INT                  null CHECK(MEM_Gender BETWEEN 1 AND 3) DEFAULT 1,
   --- 1: Other, 2: Male, 3: Female 
   MEM_Phone            varchar(20)          NOT NULL,
   MEM_Email            nvarchar(255)        NOT NULL,
   MEM_Address          nvarchar(255)        null,
   MEM_Status           int                  null, 
	--- Active = 1, Inactive = 2
   CreatedDate          datetime             null,
   CreatedBy            varchar(255)         null,
   UpdatedDate          datetime             null,
   UpdatedBy            varchar(255)         null,
   constraint PK_MEMBERS primary key (MEM_ID),
   FOREIGN KEY(ACC_ID) REFERENCES Accounts(ACC_ID),
   FOREIGN KEY(ACR_ID) REFERENCES AccountRoles(ACR_ID)
)
ALTER TABLE Members
	ADD CONSTRAINT UQ_MEM_Email UNIQUE (MEM_Email);

ALTER TABLE Members
	ADD CONSTRAINT UNIQUE_MEMBER_PHONE UNIQUE (MEM_Phone);
-----END TABLE Members-----

----- START TABLE Orders ------
CREATE TABLE Orders(
	ORD_ID				INT					IDENTITY(1, 1),
	MEM_ID				INT					NOT NULL,
	ORD_OrderCode		NVARCHAR(255)		NOT NULL,
	ORD_Description		NVARCHAR(255)		NOT NULL,
	ORD_Status			INT					NOT NULL CHECK(ORD_Status BETWEEN 1 AND 16),
	ORD_ShippingCost	MONEY				NOT NULL,
	ORD_CouponCode		VARCHAR(255)		NULL,
	ORD_PaymentMethod	INT					NULL CHECK(ORD_PaymentMethod BETWEEN 1 AND 11),
	ORD_TotalPrice		MONEY				NOT NULL,
	CreatedDate         DATETIME			NULL,
	CreatedBy           VARCHAR(255)        NULL,
	UpdatedDate         DATETIME            NULL,
	UpdatedBy           VARCHAR(255)        NULL,
	CONSTRAINT PK_ORDERS PRIMARY KEY (ORD_ID),
	FOREIGN KEY(MEM_ID) REFERENCES Members(MEM_ID)
)

--ALTER TABLE Orders
--ADD ORD_TotalPrice MONEY NOT NULL DEFAULT 0;

/* FOR ORD_Status:
	1: Pending; 2: Confirmed; 3: Processing; 4: Completed; 
	5: Canceled; 6: Failed; 7: On Hold; 8: Awaiting Shipment;
	9: Shipped; 10: In Transit; 11: Delivered; 12: Returned; 
	13: Lost; 14: Paid; 15: Unpaid; 16: Refunded;

	VD: 
	Order Success: Pending → Confirmed → Processing → Shipped → Delivered → Completed.
	Order Canceled: Pending → Canceled
	Order Returned: Shipped → Delivered → Returned → Refunded.
	Payment Failed: Pending → Failed.
*/
/* FOR ORD_PaymentMethod:
	1: Cash; 2: Banking; 3: Installment; 4: Pending; 
	5: Completed; 6: Failed; 7: Refunded; 8: Canceled;
	9: Verified; 10: Unverified, 11: Awaiting Confirmation
*/
----- END TABLE Orders ------

----- START TABLE OrderDetails ------
CREATE TABLE OrderDetails(
	ORDE_ID				INT					IDENTITY(1, 1),
	PRO_ID				INT					NOT NULL,
	ORD_ID				INT					NOT NULL,
	ORDE_Price			MONEY				NOT NULL,
	ORDE_Quantity		INT					NOT NULL,
	CreatedDate         DATETIME			NULL,
	CreatedBy           VARCHAR(255)        NULL,
	UpdatedDate         DATETIME            NULL,
	UpdatedBy           VARCHAR(255)        NULL,
	CONSTRAINT PK_ORDERDETAILS PRIMARY KEY (ORDE_ID),
	FOREIGN KEY(PRO_ID) REFERENCES Products(PRO_ID),
	FOREIGN KEY(ORD_ID) REFERENCES Orders(ORD_ID)
)
----- END TABLE OrderDetails ------

----- START TABLE MomoInfos ------
CREATE TABLE MomoInfos(
	MOMO_ID				INT					IDENTITY(1, 1),
	ORD_ID				INT					NOT NULL,
	ORD_Description		NVARCHAR(255)		NOT NULL,
	MOMO_FullName		NVARCHAR(255)		NOT NULL,
	MOMO_Amount			MONEY				NOT NULL,
	MOMO_DatePaid		DATETIME			NOT NULL,
	CreatedDate         DATETIME			NULL,
	CreatedBy           VARCHAR(255)        NULL,
	UpdatedDate         DATETIME            NULL,
	UpdatedBy           VARCHAR(255)        NULL,
	CONSTRAINT PK_MOMOINFOS PRIMARY KEY (MOMO_ID)
)

/* CREATE FOREIGN KEY TO TABLE Orders */
ALTER TABLE MomoInfos
   ADD CONSTRAINT FK_MOMOINFO_RELATIONS_ORDER FOREIGN KEY (ORD_ID)
      REFERENCES Orders (ORD_ID)
----- END TABLE MomoInfos ------

----- START TABLE ProductQuantities ------
CREATE TABLE ProductQuantities(
	PROQ_ID				INT					IDENTITY(1, 1),
	PRO_ID				INT					NOT NULL,
	PROQ_Quantity		INT					NOT NULL,
	CreatedDate         DATETIME			NULL,
	CreatedBy           VARCHAR(255)        NULL,
	UpdatedDate         DATETIME            NULL,
	UpdatedBy           VARCHAR(255)        NULL,
	CONSTRAINT PK_PRODUCTQUANTITIES PRIMARY KEY (PROQ_ID)
)
/* CREATE FOREIGN KEY TO TABLE Products*/
ALTER TABLE ProductQuantities
   ADD CONSTRAINT FK_PRODUCTQUANTITY_RELATIONS_PRODUCT FOREIGN KEY (PRO_ID)
      REFERENCES Products (PRO_ID)
GO
----- END TABLE ProductQuantities ------

----- START TABLE Shippings ------
CREATE TABLE Shippings (
	SHIP_ID				INT					IDENTITY(1, 1),
	SHIP_Price			MONEY				NOT NULL,
	SHIP_Ward			NVARCHAR(255)		NOT NULL,
	SHIP_District		NVARCHAR(255)		NOT NULL,
	SHIP_City			NVARCHAR(255)		NOT NULL,
	CreatedDate         DATETIME			NULL,
	CreatedBy           VARCHAR(255)        NULL,
	UpdatedDate         DATETIME            NULL,
	UpdatedBy           VARCHAR(255)        NULL,
	CONSTRAINT PK_SHIPPINGS PRIMARY KEY (SHIP_ID)
)
GO
----- END TABLE Shippings ------

----- START TABLE Ratings ------
CREATE TABLE Ratings (
	RAT_ID				INT					IDENTITY(1, 1),
	PRO_ID				INT					NOT NULL,
	RAT_Comment			NVARCHAR(255)		NOT NULL,
	RAT_Name			NVARCHAR(255)		NOT NULL,
	RAT_Email			NVARCHAR(255)		NOT NULL,
	RAT_Star			INT					NOT NULL,
	CreatedDate         DATETIME			NULL,
	CreatedBy           VARCHAR(255)        NULL,
	UpdatedDate         DATETIME            NULL,
	UpdatedBy           VARCHAR(255)        NULL,
	CONSTRAINT PK_RATINGS PRIMARY KEY (RAT_ID)
)
GO

/* CREATE FOREIGN KEY TO TABLE Products*/
ALTER TABLE Ratings
   ADD CONSTRAINT FK_RATING_RELATIONS_PRODUCT FOREIGN KEY (PRO_ID)
      REFERENCES Products (PRO_ID)
GO
----- END TABLE Ratings ------

----- START TABLE Statisticals ------
CREATE TABLE Statisticals (
	STA_ID				INT					IDENTITY(1, 1),
	STA_Quantity		INT					NOT NULL,
	STA_Status			INT					NOT NULL,
	STA_Revenue			DECIMAL				NOT NULL,
	STA_Profit			FLOAT				NOT NULL,
	CreatedDate         DATETIME			NULL,
	CreatedBy           VARCHAR(255)        NULL,
	UpdatedDate         DATETIME            NULL,
	UpdatedBy           VARCHAR(255)        NULL,
	CONSTRAINT PK_STATISTICALS PRIMARY KEY (STA_ID)
)
GO
----- END TABLE Statisticals ------

----- START TABLE VnInfos ------
CREATE TABLE VnInfos (
	VNIN_ID				INT					IDENTITY(1, 1),
	ORD_ID				INT					NOT NULL,
	TRANS_ID			INT					NOT NULL,
	PAY_ID				INT					NOT NULL,
	ORD_Description		NVARCHAR(255)		NOT NULL,
	VNIN_PaymentMethod	INT					NOT NULL,
	CreatedDate         DATETIME			NULL,
	CreatedBy           VARCHAR(255)        NULL,
	UpdatedDate         DATETIME            NULL,
	UpdatedBy           VARCHAR(255)        NULL,
	CONSTRAINT PK_VNINFOS PRIMARY KEY (VNIN_ID)
)

ALTER TABLE VnInfos
   ADD CONSTRAINT FK_VNINFO_RELATIONS_ORDER FOREIGN KEY (ORD_ID)
      REFERENCES Orders (ORD_ID)
----- END TABLE VnInfos ------

------- START TABLE Compares ------
CREATE TABLE Compares(
	COM_ID				INT					IDENTITY(1, 1),
	PRO_ID				INT					NOT NULL,
	MEM_ID				INT					NOT NULL,
	CreatedDate         DATETIME			NULL,
	CreatedBy           VARCHAR(255)        NULL,
	UpdatedDate         DATETIME            NULL,
	UpdatedBy           VARCHAR(255)        NULL,
	CONSTRAINT PK_COMPARES PRIMARY KEY (COM_ID),
	FOREIGN KEY(PRO_ID) REFERENCES Products(PRO_ID),
	FOREIGN KEY(MEM_ID) REFERENCES Members(MEM_ID)
)

------- END TABLE Compares ------

------- START TABLE Wishlists ------
CREATE TABLE Wishlists(
	WISH_ID				INT					IDENTITY(1, 1),
	PRO_ID				INT					NOT NULL,
	MEM_ID				INT					NOT NULL,
	CreatedDate         DATETIME			NULL,
	CreatedBy           VARCHAR(255)        NULL,
	UpdatedDate         DATETIME            NULL,
	UpdatedBy           VARCHAR(255)        NULL,
	CONSTRAINT PK_WISHLISTS PRIMARY KEY (WISH_ID),
	FOREIGN KEY(PRO_ID) REFERENCES Products(PRO_ID),
	FOREIGN KEY(MEM_ID) REFERENCES Members(MEM_ID)
)

------- END TABLE Wishlists ------

CREATE TABLE Blogs (
    BLG_ID				INT					IDENTITY(1, 1),
    BLG_Title			NVARCHAR(255)		NOT NULL,        
    BLG_Slug			NVARCHAR(255)		NULL,  
    BLG_Content			NVARCHAR(MAX)		NOT NULL,  
	BLG_Image			VARCHAR(255)		NULL,
    BLG_AuthorName		NVARCHAR(255)		NOT NULL,               
    BLG_PublishedAt		DATETIME			NOT NULL,           
    BLG_Status			INT					NOT NULL,
	--CHECK (BLG_Status BETWEEN 1 AND 3), 
	-- For status: 1: Draft, 2: Published, 3: Archived          
    BLG_Tags			NVARCHAR(MAX)		NULL,    
	CreatedDate         DATETIME			NULL,
	CreatedBy           VARCHAR(255)        NULL,
	UpdatedDate         DATETIME            NULL,
	UpdatedBy           VARCHAR(255)        NULL,
	CONSTRAINT PK_BLOGS PRIMARY KEY (BLG_ID)
);


INSERT INTO Categories (CAT_Name, CAT_Description, CAT_Slug, CAT_DisplayOrder, CAT_Status, CreatedDate, CreatedBy, UpdatedDate, UpdatedBy) VALUES
('PC', 'Computer', 'PC', 1, 1, '2024-12-01', 'admin', '2024-12-01', 'admin'),															     
('Cellphone', 'Cellphone ', 'Cellphone', 2, 1, '2024-12-01', 'admin', '2024-12-01', 'admin'),											   
('Laptop', 'Laptop', 'Laptop', 3, 1, '2024-12-01', 'admin', '2024-12-01', 'admin'),													     
('Ipad', 'Ipad', 'Ipad', 4, 1, '2024-12-01', 'admin', '2024-12-01', 'admin')

INSERT INTO Brands(BRA_NAME, BRA_Description, BRA_Slug, BRA_DisplayOrder, BRA_Status, CreatedDate, CreatedBy, UpdatedDate, UpdatedBy) VALUES
('Samsung', 'Samsung', 'Samsung', 1, 1, '2024-12-01', 'admin', '2024-12-01', 'admin'),
('Macbook', 'Macbook ', 'Macbook', 2, 1, '2024-12-01', 'admin', '2024-12-01', 'admin'),
('Xiaomi', 'Xiaomi', 'Xiaomi', 3, 1, '2024-12-01', 'admin', '2024-12-01', 'admin'),
('iPhone', 'iPhone', 'iPhone', 4, 1, '2024-12-01', 'admin', '2024-12-01', 'admin')

INSERT INTO Products VALUES
(1, 1, 'Product 1', 'ABC', 'Product 1', 100, '1.jpeg', 20, 1, 70, '2024-12-01', 'admin', '2024-12-01', 'admin'),
(1, 1, 'Product 2', 'ABC', 'Product 2', 100, '1.jpeg', 20, 2, 70, '2024-12-01', 'admin', '2024-12-01', 'admin'),
(2, 2, 'Product 3', 'ABC', 'Product 3', 100, '2.jpeg', 20, 3, 70, '2024-12-01', 'admin', '2024-12-01', 'admin'),
(2, 2, 'Product 4', 'ABC', 'Product 4', 100, '2.jpeg', 20, 1, 70, '2024-12-01', 'admin', '2024-12-01', 'admin'),
(3, 3, 'Product 5', 'ABC', 'Product 5', 100, '1.jpeg', 20, 2, 70, '2024-12-01', 'admin', '2024-12-01', 'admin'),
(4, 4, 'Product 6', 'ABC', 'Product 6', 100, '1.jpeg', 20, 1, 70, '2024-12-01', 'admin', '2024-12-01', 'admin'),
(5, 4, 'Product 7', 'ABC', 'Product 7', 100, '3.jpeg', 20, 2, 70, '2024-12-01', 'admin', '2024-12-01', 'admin'),
(5, 1, 'Product 8', 'ABC', 'Product 8', 100, '3.jpeg', 20, 3, 70, '2024-12-01', 'admin', '2024-12-01', 'admin')

INSERT INTO Contact VALUES 
('E-Shopper', 'ABCDEFGH', '0326034561', 'daocongtri20031609@gmail.com', 
'<iframe src="https://www.google.com/maps/embed?pb=!1m18!1m12!1m3!1d3918.415049919885!2d106.62725477457585!3d10.856002857721782!2m3!1f0!2f0!3f0!3m2!1i1024!2i768!4f13.1!3m3!1m2!1s0x317529deaaaaaaab%3A0xce800a25143c8e3a!2zQ2FvIMSQ4bqzbmcgU8OgaSBHw7Ju!5e0!3m2!1svi!2s!4v1733987658497!5m2!1svi!2s" width="100%" height="450" style="border:0;" allowfullscreen="" loading="lazy" referrerpolicy="no-referrer-when-downgrade"></iframe>', 
'logo.png', 'Quang Trung Software Park, SaigonTech Building, Lot 14, Street 5, Tan Chanh Hiep, District 12, Ho Chi Minh 700000, Vietnam','2024-12-12', 'admin', '2024-12-12', 'admin')

INSERT INTO AccountRoles (ACR_Name, ACR_Status, CreatedDate, CreatedBy, UpdatedDate, UpdatedBy) VALUES 
('User', 1, '2024-12-16', 'admin', '2024-12-16', 'admin'),
('Admin', 1, '2024-12-16', 'admin', '2024-12-16', 'admin'),
('Shop Owner', 2, '2024-12-16', 'admin', '2024-12-16', 'admin')

INSERT INTO Accounts VALUES 
('admin', '123456789', 'Admin Cong Tri', 'daocongtri20031609@gmail.com', '0326034561', 1, '2024-12-16', 'admin', '2024-12-16', 'admin'),
('admin1', '123456789', 'Admin Quynh Nhu', 'luongquynhnhu0908@gmail.com', '0981015452', 1, '2024-12-16', 'admin', '2024-12-16', 'admin'),
('user', '123456789', 'Doan Huu Thien', 'user@gmail.com', '123456789', 1, '2024-12-16', 'admin', '2024-12-16', 'admin')


INSERT INTO Members(ACC_ID, ACR_ID, MEM_LastName, MEM_FirstName, MEM_Gender, MEM_Phone, MEM_Email, MEM_Address, MEM_Status, CreatedDate, CreatedBy, UpdatedDate,  UpdatedBy) VALUES
(1, 2, 'Cong Tri', 'Dao', 2, '0326034561', 'daocongtri20031609@gmail.com', '999 Le Duc Tho P16 Q.Go Vap', 1, '2024-12-16', 'admin', '2024-12-16', 'admin'),
(2, 2, 'Quynh Nhu', 'Luong', 3, '0981015452', 'luongquynhnhu0908@gmail.com', '2276/10 QL1A Tô Ký Q.12', 1, '2024-12-16', 'admin', '2024-12-16', 'admin'),
(3, 1, 'Huu Thien', 'Doan', 2, '1234567891', 'admin2@gmail.com', '', 1, '2024-12-16', 'admin', '2024-12-16', 'admin')

SELECT O.ORD_OrderCode, O.ORD_ID, ORDE_ID, OD.PRO_ID, ORDE_Price, ORDE_Quantity FROM OrderDetails OD, Orders O
WHERE OD.ORD_ID = O.ORD_ID AND O.ORD_ID = 5
