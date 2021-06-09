# Category

## Overview

*Describe main entities definitions, navigation, indexation, etc.*

### Main Entities

Category data are managed in Catalog module. Native VirtoCommerce functionality contains the following types:

Type | Description 
---|---
Category | A container for other Categories and Products or Variations with own parameters. It collects products in a certain way.
Product | An orderable item of merchandise for sale, can be used for targeted promotions, etc.

It is important to understand the relations between entities:

Type | Relations
---|--- 
Category |(many) categories <-> (one) physical  catalog; (one) category <-> (many) products; (many) categories <-> (many) properties; (many) categories <-> many tags; (many) categories <-> (many) properties; (many) categories <-> (one) tax type;
Product |(many) products <-> (one) physical catalog; (many) products <-> (one) category;

Where (many) entity1 <-> (one) entity2 means that entity1 can be related with only one entity2, and entity2 can have many relations with entity1.

Applying of properties and relations:

Type | From where | Description
---|---|---
Product property | created in Catalog | property inherited through Categories to their products
Product property | created in Category | property inherits and appears for products only in this Category
Variation property | created in Catalog | property inherited through Categories > through their products to products variations
Variation property | created in Category | property inherits and appears for products variations only in this Category

You can assign tags to Category. Tags help personalize pricing and access for users. **Tags in Category have the same meaning as customer user groups.**

The following policies exist for tag distribution:

Setting | Description
--- |---
TAGS DOWNTREE | propagates all tags assigned to categories for all their descendants. 
TAGS DOWNUP | propagates tags up to hierarchy from descendants to parents.


### Indexation

Categories and Products are kept in the index. You need run Product and Category Indexation to put into index their changes.

### Atomic functions / scenarios

Description | Module | Link
--- |---|---
How to create Category | Catalog module | 
How to manage Category | Catalog module |
How to create Subcategory | Catalog module |
How to run indexation for categories | Search module |
How to add properties in Category | Catalog module|
How to add tags in Category | Catalog module |
How to hide and show Category | Catalog module |
How to set priority | Catalog module |
How to manage tax type | Catalog module |
How to manage policy for tags propagation |Settings|

### Code extensibility

Name | Module |Description | Link
--- |---|---|---
Extend Category | Catalog module | Add custom field |

### XAPI built-in scenarios

Following built-in business API scenarios can be used by front-end developers "as is" when create registration user experience

Name | Description | Link
--- | --- | ---

## User scenario examples

Name | Description | Link
--- |---|---
How to add property for products in selected category | Create product property in needed category |
How to set access for users to Category | Manage tag and user group relation |
How predefine Categories order of displaying | Set Priority fields |
