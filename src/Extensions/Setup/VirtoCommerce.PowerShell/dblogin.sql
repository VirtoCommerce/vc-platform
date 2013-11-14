USE master
CREATE LOGIN [virto] WITH PASSWORD = 'store'

USE VirtoCommerce
CREATE USER [virto] FOR LOGIN virto
exec sp_addrolemember 'db_owner', 'virto'