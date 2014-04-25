-- From the article "Low risk ways to use F# at work"
-- http://fsharpforfunandprofit.com/posts/low-risk-ways-to-use-fsharp-at-work/

-- Create tables to use with the SqlInFsharp code
-- If run again, deletes all tables and then recreates them.

USE SqlInFsharp 


/* =====================================================
Create Customer table  
===================================================== */

IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Customer' AND TABLE_SCHEMA='dbo')
   DROP TABLE dbo.Customer
GO

CREATE TABLE dbo.Customer (
	CustomerId int NOT NULL IDENTITY(1,1)
	,Name varchar(50) NOT NULL 
	,Email varchar(50) NOT NULL 
	,Birthdate datetime NULL 

	CONSTRAINT PK_Customer PRIMARY KEY CLUSTERED (CustomerId)
	)


/* =====================================================
Create CustomerImport table  
===================================================== */

IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'CustomerImport' AND TABLE_SCHEMA='dbo')
   DROP TABLE dbo.CustomerImport 
GO

CREATE TABLE dbo.CustomerImport (
	CustomerId int NOT NULL IDENTITY(1,1)
	,FirstName varchar(50) NOT NULL 
	,LastName varchar(50) NOT NULL 
	,EmailAddress varchar(50) NOT NULL 
	,Age int NULL 

	CONSTRAINT PK_CustomerImport PRIMARY KEY CLUSTERED (CustomerId)
	)


/* =====================================================
Create Country table  
===================================================== */

IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Country' AND TABLE_SCHEMA='dbo')
   DROP TABLE dbo.Country
GO

CREATE TABLE dbo.Country (
	IsoCode char(3) NOT NULL 
	,CountryName varchar(50) NOT NULL 

	CONSTRAINT PK_CountryName PRIMARY KEY CLUSTERED (IsoCode)
	)

