CREATE DATABASE [CustomerRegister]
GO

USE [CustomerRegister]
GO

CREATE TABLE [Customer](
	[Id] [uniqueidentifier] NOT NULL,
	[FirstName] [varchar](50) NOT NULL,
 CONSTRAINT [PK_Customer] PRIMARY KEY CLUSTERED ([Id] ASC)
)
GO