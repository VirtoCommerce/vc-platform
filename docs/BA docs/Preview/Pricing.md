# Pricing

Managing prices is the scenario that results appearing different prices for one product in different conditions or for different users. 

## Overview

*Describe main entities definitions, navigation, indexation, etc.*

### Main Entities

Pricing data are managed in Pricing module. Native VirtoCommerce functionality contains the following entities:

Entity | Description 
---|--- 
Price lists | A collection of price lines
Price list assignments | A relation from price list to catalog and users conditions
Prices | A value in the Price field in Priceline conditions for selected product
Price lines | A relation between product and prices with some conditions (minimum quantity, timeframes)

Each product can have a few prices related to different price lists. Each price will appear for product in the store if conditions are met. You can make Personalized prices with additional conditions.

It is important to understand the relations between entities:

Type |Relations
---|--- 
Price lists | (many) price lists <-> (one) currency; (one) price list <-> (many) price list assignments; 
Price list assignments | (many) price list assignments <-> (one) catalog; (many) price list assignment <-> (one) price list;
Prices | (many) prices <-> (one) price list; (many) prices <-> (one) product;
Price lines | (many) price lines <-> (one) price list; (one) price line <-> (one) sale price; (one) price line <-> (one) list price; 

Where (many)entity1 <-> (one)entity2 means that entity1 can be related with only one entity2, and entity2 can have many relations with entity1.

There are two Price Types with a different meaning:

Type | Description
--- |---
List Price | A regular price, specified for each product. 
Sale Price | A sales price, which should be lower than regular price. It has higher priority than list price, specified for each product. 

### Indexation

Product’s prices are kept in the index. You need to run Product Indexation to put price changes into the index.

### Atomic functions / scenarios

Description | Module | Link
--- |---|---
How to create Price list | Pricing module|
How to delete Price list | Pricing module |
How to export Price list | Export module |
How to export Price line | Price Export Import module|
How to import Price line | Price Export Import module|
How to create Price list assignments | Pricing module|
How to edit Price list assignments | Pricing module|
How to make priority for price list assignments | Pricing module|
How to change Eligible shoppers for price list assignments | Pricing module|
How to add price for product | Pricing module|
How to add new currency | Pricing module|
How to set timeframes for price | Pricing module|
How to run indexation for products | Search module |

### Code extensibility

Name | Module |Description | Link
--- |---|---|---
Extend Price list assignments | Pricing module | Add custom field |
Extend Price list assignments | Pricing module | Add conditions for Eligible shoppers |

## User scenario examples

Name | Description | LInk
--- |---|---
How to create and assign personalized prices | Set Eligible shoppers block in price list assignment and add prices related to this price list |
How to create volume based pricing | Set quantity for price lines |
How to create temporary price | Set timeframes for price line or set timeframes for price list assignment |
How to create static Sales | Set Sale price for needed products |


