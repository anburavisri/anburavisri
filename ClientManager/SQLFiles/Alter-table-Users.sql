/*
   Wednesday, September 22, 20211:18:20 PM
   User: 
   Server: DESKTOP-GI5O99G
   Database: ClientManager_v2
   Application: 
*/

/* To prevent any potential data loss issues, you should review this script in detail before running it outside the context of the database designer.*/
BEGIN TRANSACTION
SET QUOTED_IDENTIFIER ON
SET ARITHABORT ON
SET NUMERIC_ROUNDABORT OFF
SET CONCAT_NULL_YIELDS_NULL ON
SET ANSI_NULLS ON
SET ANSI_PADDING ON
SET ANSI_WARNINGS ON
COMMIT
BEGIN TRANSACTION
GO
CREATE TABLE dbo.Tmp_Users
	(
	Id int NOT NULL IDENTITY (1, 1),
	Password nvarchar(50) NOT NULL,
	FullName nvarchar(100) NOT NULL,
	Email nvarchar(100) NOT NULL,
	IsActive bit NULL,
	DateOfBirth datetime NULL,
	EmployeeId nvarchar(50) NULL,
	AddressLine1 nvarchar(100) NULL,
	AddressLine2 nvarchar(100) NULL,
	State nvarchar(50) NULL,
	City nvarchar(50) NULL,
	Pincode nvarchar(50) NULL,
	ReportingManager int NULL,
	CreatedOn datetime NULL,
	CreatedBy int NULL,
	ModifiedOn datetime NULL,
	ModifiedBy int NULL
	)  ON [PRIMARY]
GO
ALTER TABLE dbo.Tmp_Users SET (LOCK_ESCALATION = TABLE)
GO
SET IDENTITY_INSERT dbo.Tmp_Users ON
GO
IF EXISTS(SELECT * FROM dbo.Users)
	 EXEC('INSERT INTO dbo.Tmp_Users (Id, Password, FullName, Email, IsActive, ReportingManager, CreatedOn, CreatedBy, ModifiedOn, ModifiedBy)
		SELECT Id, Password, FullName, Email, IsActive, ReportingManager, CreatedOn, CreatedBy, ModifiedOn, ModifiedBy FROM dbo.Users WITH (HOLDLOCK TABLOCKX)')
GO
SET IDENTITY_INSERT dbo.Tmp_Users OFF
GO
ALTER TABLE dbo.UserRoles
	DROP CONSTRAINT FK_UserRoles_Users2
GO
ALTER TABLE dbo.SaleActivity
	DROP CONSTRAINT FK_SaleActivity_Users
GO
ALTER TABLE dbo.SaleActivity
	DROP CONSTRAINT FK_SaleActivity_Users1
GO
ALTER TABLE dbo.SaleActivity
	DROP CONSTRAINT FK_SaleActivity_Users2
GO
ALTER TABLE dbo.Users
	DROP CONSTRAINT FK_Users_Users1
GO
ALTER TABLE dbo.Users
	DROP CONSTRAINT FK_Users_Users2
GO
ALTER TABLE dbo.RepresentativeSaleTarget
	DROP CONSTRAINT FK_RepresentativeSaleTarget_Users
GO
ALTER TABLE dbo.RepresentativeSaleTarget
	DROP CONSTRAINT FK_RepresentativeSaleTarget_Users1
GO
ALTER TABLE dbo.UserRoles
	DROP CONSTRAINT FK_UserRoles_Users
GO
ALTER TABLE dbo.UserRoles
	DROP CONSTRAINT FK_UserRoles_Roles
GO
ALTER TABLE dbo.UserRoles
	DROP CONSTRAINT FK_UserRoles_Users1
GO
ALTER TABLE dbo.UserContacts
	DROP CONSTRAINT FK_UserContacts_UserDetails
GO
ALTER TABLE dbo.UserContacts
	DROP CONSTRAINT FK_UserContacts_ContactDetails
GO
ALTER TABLE dbo.UserContacts
	DROP CONSTRAINT FK_UserContacts_Users
GO
ALTER TABLE dbo.Roles
	DROP CONSTRAINT FK_Roles_Users
GO
ALTER TABLE dbo.Roles
	DROP CONSTRAINT FK_Roles_Users1
GO
ALTER TABLE dbo.ClientContacts
	DROP CONSTRAINT FK_ClientContacts_Users
GO
ALTER TABLE dbo.ClientContacts
	DROP CONSTRAINT FK_ClientContacts_Users1
GO
ALTER TABLE dbo.Clients
	DROP CONSTRAINT FK_Clients_Users
GO
ALTER TABLE dbo.Clients
	DROP CONSTRAINT FK_Clients_Users1
GO
ALTER TABLE dbo.Contact
	DROP CONSTRAINT FK_Contact_Users
GO
ALTER TABLE dbo.Contact
	DROP CONSTRAINT FK_Contact_Users1
GO
ALTER TABLE dbo.Projects
	DROP CONSTRAINT FK_ProjectDetails_UserDetails
GO
ALTER TABLE dbo.Sales
	DROP CONSTRAINT FK_SaleDetails_UserDetails
GO
ALTER TABLE dbo.Sales
	DROP CONSTRAINT FK_Sales_Users
GO
ALTER TABLE dbo.Users
	DROP CONSTRAINT FK_Users_Users
GO
DROP TABLE dbo.Users
GO
EXECUTE sp_rename N'dbo.Tmp_Users', N'Users', 'OBJECT' 
GO
ALTER TABLE dbo.Users ADD CONSTRAINT
	PK_UserDetails PRIMARY KEY CLUSTERED 
	(
	Id
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
ALTER TABLE dbo.Users ADD CONSTRAINT
	FK_Users_Users1 FOREIGN KEY
	(
	ReportingManager
	) REFERENCES dbo.Users
	(
	Id
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.Users ADD CONSTRAINT
	FK_Users_Users2 FOREIGN KEY
	(
	CreatedBy
	) REFERENCES dbo.Users
	(
	Id
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.Users ADD CONSTRAINT
	FK_Users_Users FOREIGN KEY
	(
	ModifiedBy
	) REFERENCES dbo.Users
	(
	Id
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
COMMIT
select Has_Perms_By_Name(N'dbo.Users', 'Object', 'ALTER') as ALT_Per, Has_Perms_By_Name(N'dbo.Users', 'Object', 'VIEW DEFINITION') as View_def_Per, Has_Perms_By_Name(N'dbo.Users', 'Object', 'CONTROL') as Contr_Per BEGIN TRANSACTION
GO
ALTER TABLE dbo.Sales ADD CONSTRAINT
	FK_SaleDetails_UserDetails FOREIGN KEY
	(
	RepresentativeId
	) REFERENCES dbo.Users
	(
	Id
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.Sales ADD CONSTRAINT
	FK_Sales_Users FOREIGN KEY
	(
	CreatedBy
	) REFERENCES dbo.Users
	(
	Id
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.Sales SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
select Has_Perms_By_Name(N'dbo.Sales', 'Object', 'ALTER') as ALT_Per, Has_Perms_By_Name(N'dbo.Sales', 'Object', 'VIEW DEFINITION') as View_def_Per, Has_Perms_By_Name(N'dbo.Sales', 'Object', 'CONTROL') as Contr_Per BEGIN TRANSACTION
GO
ALTER TABLE dbo.Projects ADD CONSTRAINT
	FK_ProjectDetails_UserDetails FOREIGN KEY
	(
	ProjectLead
	) REFERENCES dbo.Users
	(
	Id
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.Projects SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
select Has_Perms_By_Name(N'dbo.Projects', 'Object', 'ALTER') as ALT_Per, Has_Perms_By_Name(N'dbo.Projects', 'Object', 'VIEW DEFINITION') as View_def_Per, Has_Perms_By_Name(N'dbo.Projects', 'Object', 'CONTROL') as Contr_Per BEGIN TRANSACTION
GO
ALTER TABLE dbo.Contact ADD CONSTRAINT
	FK_Contact_Users FOREIGN KEY
	(
	CreatedBy
	) REFERENCES dbo.Users
	(
	Id
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.Contact ADD CONSTRAINT
	FK_Contact_Users1 FOREIGN KEY
	(
	ModifiedBy
	) REFERENCES dbo.Users
	(
	Id
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.Contact SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
select Has_Perms_By_Name(N'dbo.Contact', 'Object', 'ALTER') as ALT_Per, Has_Perms_By_Name(N'dbo.Contact', 'Object', 'VIEW DEFINITION') as View_def_Per, Has_Perms_By_Name(N'dbo.Contact', 'Object', 'CONTROL') as Contr_Per BEGIN TRANSACTION
GO
ALTER TABLE dbo.Clients ADD CONSTRAINT
	FK_Clients_Users FOREIGN KEY
	(
	CreatedBy
	) REFERENCES dbo.Users
	(
	Id
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.Clients ADD CONSTRAINT
	FK_Clients_Users1 FOREIGN KEY
	(
	ModifiedBy
	) REFERENCES dbo.Users
	(
	Id
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.Clients SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
select Has_Perms_By_Name(N'dbo.Clients', 'Object', 'ALTER') as ALT_Per, Has_Perms_By_Name(N'dbo.Clients', 'Object', 'VIEW DEFINITION') as View_def_Per, Has_Perms_By_Name(N'dbo.Clients', 'Object', 'CONTROL') as Contr_Per BEGIN TRANSACTION
GO
ALTER TABLE dbo.ClientContacts ADD CONSTRAINT
	FK_ClientContacts_Users FOREIGN KEY
	(
	CreatedBy
	) REFERENCES dbo.Users
	(
	Id
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.ClientContacts ADD CONSTRAINT
	FK_ClientContacts_Users1 FOREIGN KEY
	(
	ModifiedBy
	) REFERENCES dbo.Users
	(
	Id
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.ClientContacts SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
select Has_Perms_By_Name(N'dbo.ClientContacts', 'Object', 'ALTER') as ALT_Per, Has_Perms_By_Name(N'dbo.ClientContacts', 'Object', 'VIEW DEFINITION') as View_def_Per, Has_Perms_By_Name(N'dbo.ClientContacts', 'Object', 'CONTROL') as Contr_Per BEGIN TRANSACTION
GO
ALTER TABLE dbo.Roles ADD CONSTRAINT
	FK_Roles_Users FOREIGN KEY
	(
	CreatedBy
	) REFERENCES dbo.Users
	(
	Id
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.Roles ADD CONSTRAINT
	FK_Roles_Users1 FOREIGN KEY
	(
	ModifiedBy
	) REFERENCES dbo.Users
	(
	Id
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.Roles SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
select Has_Perms_By_Name(N'dbo.Roles', 'Object', 'ALTER') as ALT_Per, Has_Perms_By_Name(N'dbo.Roles', 'Object', 'VIEW DEFINITION') as View_def_Per, Has_Perms_By_Name(N'dbo.Roles', 'Object', 'CONTROL') as Contr_Per BEGIN TRANSACTION
GO
ALTER TABLE dbo.UserContacts ADD CONSTRAINT
	FK_UserContacts_UserDetails FOREIGN KEY
	(
	UserId
	) REFERENCES dbo.Users
	(
	Id
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.UserContacts ADD CONSTRAINT
	FK_UserContacts_ContactDetails FOREIGN KEY
	(
	CreatedBy
	) REFERENCES dbo.Users
	(
	Id
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.UserContacts ADD CONSTRAINT
	FK_UserContacts_Users FOREIGN KEY
	(
	ModifiedBy
	) REFERENCES dbo.Users
	(
	Id
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.UserContacts SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
select Has_Perms_By_Name(N'dbo.UserContacts', 'Object', 'ALTER') as ALT_Per, Has_Perms_By_Name(N'dbo.UserContacts', 'Object', 'VIEW DEFINITION') as View_def_Per, Has_Perms_By_Name(N'dbo.UserContacts', 'Object', 'CONTROL') as Contr_Per BEGIN TRANSACTION
GO
ALTER TABLE dbo.RepresentativeSaleTarget ADD CONSTRAINT
	FK_RepresentativeSaleTarget_Users FOREIGN KEY
	(
	UserId
	) REFERENCES dbo.Users
	(
	Id
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.RepresentativeSaleTarget ADD CONSTRAINT
	FK_RepresentativeSaleTarget_Users1 FOREIGN KEY
	(
	CreatedBy
	) REFERENCES dbo.Users
	(
	Id
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.RepresentativeSaleTarget SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
select Has_Perms_By_Name(N'dbo.RepresentativeSaleTarget', 'Object', 'ALTER') as ALT_Per, Has_Perms_By_Name(N'dbo.RepresentativeSaleTarget', 'Object', 'VIEW DEFINITION') as View_def_Per, Has_Perms_By_Name(N'dbo.RepresentativeSaleTarget', 'Object', 'CONTROL') as Contr_Per BEGIN TRANSACTION
GO
ALTER TABLE dbo.SaleActivity ADD CONSTRAINT
	FK_SaleActivity_Users FOREIGN KEY
	(
	SalesRepresentativeId
	) REFERENCES dbo.Users
	(
	Id
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.SaleActivity ADD CONSTRAINT
	FK_SaleActivity_Users1 FOREIGN KEY
	(
	ModifiedBy
	) REFERENCES dbo.Users
	(
	Id
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.SaleActivity ADD CONSTRAINT
	FK_SaleActivity_Users2 FOREIGN KEY
	(
	CreatedBy
	) REFERENCES dbo.Users
	(
	Id
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.SaleActivity SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
select Has_Perms_By_Name(N'dbo.SaleActivity', 'Object', 'ALTER') as ALT_Per, Has_Perms_By_Name(N'dbo.SaleActivity', 'Object', 'VIEW DEFINITION') as View_def_Per, Has_Perms_By_Name(N'dbo.SaleActivity', 'Object', 'CONTROL') as Contr_Per BEGIN TRANSACTION
GO
ALTER TABLE dbo.UserRoles ADD CONSTRAINT
	FK_UserRoles_Users2 FOREIGN KEY
	(
	UserId
	) REFERENCES dbo.Users
	(
	Id
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.UserRoles ADD CONSTRAINT
	FK_UserRoles_Users FOREIGN KEY
	(
	UserId
	) REFERENCES dbo.Users
	(
	Id
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.UserRoles ADD CONSTRAINT
	FK_UserRoles_Roles FOREIGN KEY
	(
	CreatedBy
	) REFERENCES dbo.Users
	(
	Id
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.UserRoles ADD CONSTRAINT
	FK_UserRoles_Users1 FOREIGN KEY
	(
	ModifiedBy
	) REFERENCES dbo.Users
	(
	Id
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.UserRoles SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
select Has_Perms_By_Name(N'dbo.UserRoles', 'Object', 'ALTER') as ALT_Per, Has_Perms_By_Name(N'dbo.UserRoles', 'Object', 'VIEW DEFINITION') as View_def_Per, Has_Perms_By_Name(N'dbo.UserRoles', 'Object', 'CONTROL') as Contr_Per 