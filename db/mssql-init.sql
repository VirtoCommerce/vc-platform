USE [master]
GO

IF DB_ID('VirtoCommerce3') IS NOT NULL
  set noexec on               -- prevent creation when already exists

CREATE DATABASE [VirtoCommerce3];
GO

USE [VirtoCommerce3]
GO

CREATE LOGIN [virto] WITH PASSWORD = 'virto', CHECK_POLICY=OFF
GO

CREATE USER [virto] FOR LOGIN [virto] WITH DEFAULT_SCHEMA=[dbo]
GO

EXEC sp_addrolemember 'db_owner', 'virto';
GO
