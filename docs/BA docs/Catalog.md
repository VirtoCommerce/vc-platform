# Catalog

Managing catalog data is the scenario that results keeping and appearing the products in different conditions or for different users.

## Overview

*Describe main entities definitions, navigation, indexation, etc.*

### Main Entities

Catalog data are managed in Catalog module. Native VirtoCommerce functionality contains the following types:

Type | Description 
---|---
Catalog | An entity that collects products, categories and identifies them to the store
Category | A container for other Categories and Products or Variations with own parameters. It collects products in a certain way. 
Product | An orderable item of merchandise for sale, can be used for targeted promotions, etc.

Relations between entities:

Type |Relations
---|--- 
Catalog |(many) catalogs <-> (many) languages; (one) catalog <-> (many) stores; (one) catalog <-> (many) properties; 
Category |(many) categories <-> (one) physical  catalog; (one) category <-> (many) products; (many) categories <-> many tags; (many) categories <-> (many) virtual catalog;
Product |(many) products <-> (one) physical catalog; (many) products <-> (one) category;

Where (many) entity1 <-> (one) entity2 means that entity1 can be related with only one entity2, and entity2 can have many relations with entity1.

It is important to know that there are 4 type of properties:

Type | Description
--- |---
Catalog property | property appears only for this catalog
Category property | property inherits by Categories in this catalog
Product property | property inherited through Categories to their products
Variation property | property inherited through Categories > through their products to products variations


There are two Catalog Types:

Type | Description
--- |---
Physical catalog | A central location to manage your store merchandise, create catalog for a brand, product line or a particular supplier.
Virtual catalog | A subset of own categories and links to products and categories from different physical catalogs.

### Indexation

Categories and Products are kept in the index. You need run Product and Category Indexation to put into index their changes.

### Atomic functions / scenarios

Description | Module | Link
--- |---|---
How to create Physical Catalog  | Catalog module| 
How to create Virtual Catalog  | Catalog module| 
How to edit Catalog | Catalog module | 
How to delete Catalog | Catalog module | 
How to export data from Catalog | Export module | 
How to import Catalog data | Export module | 
How to create Category in Catalog | Catalog module | 
How to link Category to virtual Catalog | Catalog module| 
How to link Products to virtual Catalog | Catalog module| 
How to run indexation for products | Search module | 
How to run indexation for categories | Search module | 
How to set relation Catalog to Store | Store module | 
How to add properties to Catalog | Catalog module| 
How to add properties to Category in Catalog | Catalog module| 
How to add properties to products in Catalog | Catalog module| 
How to add properties to variations in Catalog | Catalog module| 

### Code extensibility

Name | Module |Description | 
--- |---|---|---
Extend Catalog | Catalog module | Add custom field |

## User scenario examples

Name | Description | 
--- |---|---
How to display product in Store in the few Categories  | Create needed categories in virtual catalog, link the products to them and relate this catalog to store | 
How to change product displaying in store without changing the location in the admin side | Create needed categories in virtual catalog, link the products to them and relate this catalog to store | 
