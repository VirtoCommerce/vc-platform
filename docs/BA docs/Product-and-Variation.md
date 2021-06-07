# Product and variations

Managing products is the main scenario that results 

## Overview

*Describe main entities definitions, navigation, indexation, etc.*

### Main Entities

Products and Variations are managed in Catalog module. Native VirtoCommerce functionality contains the following types:

Type | Description 
---|--- 
Product | orderable item of merchandise for sale, can be used for targeted promotions, etc.
Variation | orderable item existing as part of product

Relations between entities:

Type |Relations
---|--- 
Product |(many) products <-> (one) physical catalog; (many) products <-> (one) category; (many)products <-> (many) tags; (many) products <-> (many) properties;
Variation |(many) variations <-> (one) product; 

Where (many) entity1 <-> (one) entity2 means that entity1 can be related with only one entity2, and entity2 can have many relations with entity1.

Applying of properties and relations:

Type | Description | 
--- |--- |
Product property | created in Catalog | property inherited through Categories to their products
Product property | created in Category | property inherits and appears for products only in this Category
Product property |created in product| custom property appears only for this product and its variations
Variation property | created in Catalog | property inherited through Categories > through their products to products variations
Variation property | created in Category | property inherits and appears for products variations only in this Category
Variation property |created in Variation| property appears only for this variation


You can assign tags to Category. Tags help personalize pricing and access for users. **Tags in Category have the same meaning as customer user groups.**

The following policies exist for tag distribution:

Setting | Description
--- |---
TAGS DOWNTREE | propagates all tags assigned to products for all their variations. 
TAGS DOWNUP | propagates tags up to hierarchy from descendants to parents.


### Indexation

Products and Variations are kept in the index. You need run Product Indexation to put into index their changes.

### Atomic functions / scenarios

Description | Module | Link
--- |---|---
How to create Product | Catalog module |
How to manage Product| Catalog module |
How to create Variation | Catalog module |
How to run indexation for products | Search module |
How to add properties for Product | Catalog module|
How to add tags for Product | Catalog module |
How to hide and show Product | Catalog module |
How to add Product description | Catalog module |
How to make Product available for subscription |Subscription module|
How to add timeframe for Product availability | Catalog module |


### Code extensibility

Name | Module |Description | Link
--- |---|---|---
Extend Product | Catalog module | Add custom field |

### XAPI built-in scenarios

Following built-in business API scenarios can be used by front-end developers "as is" when create registration user experience

Name | Description | Link
--- | --- | ---

## User scenario examples

Name | Description | LInk
--- |---|---
How to set access for users to Product | Manage tag and user group relation |
