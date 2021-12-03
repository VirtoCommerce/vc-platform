---
title: Database migrations
description: Database migrations
layout: docs
date: 2015-03-18T20:11:12.560Z
priority: 10
---
## Introduction

Most of the VirtoCommerce store data is stored in a database (DB). As our platform evolves, the DB structure might change and former DB (that was created in previous versions) might get obsolete. Such DB's need to migrate. We'll publish migration scripts and list them here.

## Migrations

### From version 1.6 to 1.7:

* Execute <a href="/assets/files/OrdersMigration_1_7.sql">this SQL script</a> on Orders (in case you have separate) database.
* Execute <a href="/assets/files/SecurityMigration_1_7.sql">this SQL script</a> on Security (in case you have separate) database.

### From version 1.7 to 1.8:

* Execute <a href="../../assets/files/AppConfigMigration_1_8.sql">this SQL script</a> on AppConfig (in case you have separate) database.
* Execute <a href="../../assets/files/SecurityMigration_1_8.sql">this SQL script</a> on Security (in case you have separate) database.

### From version 1.8 to 1.9:

* Execute <a href="../../assets/files/PromotionMigration_1_9.sql">this SQL script</a> on Promotion (in case you have separate) database.
* Execute <a href="../../assets/files/MigrateFromEF5toEF6.sql">this SQL script</a> for migration to EntityFramework 6.

### From version 1.9 to 1.10:

* Execute <a href="../../assets/files/OrderTotals_1_10.sql">this SQL script</a> on Orders (in case you have separate) database.

### From version 1.10 to 1.11:

* Execute <a href="../../assets/files/Stored procedures_1_11.sql">this SQL script</a> on VirtoCommerce database.

### From version 1.11 to 1.12

* Execute <a href="../../assets/files/EditorialReviewLocale_SeoTriggers_1_12.sql">this SQL script</a> on Catalogs (in case you have separate) database.

### From version 1.12 to 1.13

* Execute first this SQL <a href="../../assets/files/LineItemWeightAndParent_1.13.sql">script</a>, then this <a href="../../assets/files/ShipmentOptions_1.13.sql">script</a> and this <a href="../../assets/files/Indexes_1.13.sql">script</a> on Orders (in case you have separate) database.
* Execute this SQL <a href="../../assets/files/OptionalPropertySet_1.13.sql">script</a>  on Catalogs (in case you have separate) database.
* Execute this <a href="../../assets/files/MigrateToIndentitySecurity.sql">script</a> to migrate to asp.net identity security and this <a href="../../assets/files/AccountIdToString_1.13.sql">script</a> to convert AccountId to string on Security
