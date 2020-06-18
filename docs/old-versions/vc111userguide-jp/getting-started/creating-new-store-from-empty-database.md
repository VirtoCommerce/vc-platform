---
title: Creating new store from empty database - Virto Commerce 1.11 User Guide
description: Creating new store from empty database
layout: docs
date: 2016-06-03T10:21:23.333Z
priority: 1
---
## Introduction

This tutorial will describe steps that are needed to create a new functional application, when empty database is installed.

The tutorial will assume that all default settings are used including app settings (DefaultStore = SampleStore) and connection strings.

## Creating empty database

In order to create an empty database you have to execute powershell script setup-empty-database.ps1 under ~\Setup\VirtoCommerce.PowerShell folder.

The database that is created using this script does not install sample test data, however data that is required for the application to function correctly is installed here.

After script is finished you should have a ready database for creating and opening your own store.

If you would open the front-end now it should show the error page similar to this:

![](../../../../assets/images/docs/nostores.png)

This happens because you have no stores available (or default store not found) and first we need to create one.

## Creating a new store

To create a store we need to do the following steps in eCommerce manager:

1. Create new catalog
2. Create new properties and property sets for catalog
3. Create store

### Create new catalog

Open eCommerece manager and go to catalog. Click on +Add Catalog and select click OK on dialog that appears:

![](../../../../assets/images/docs/2catalog.png)

Fill in all required fields. Lets say catalogId and name is Test, add english or any other available languages and select default language as English if you like. Click Ok and catalog is created.

### Create properties for catalog

Right click on Test catalog you just created and click open catalog in popup menu. Open Properties tab and click add.

For simplicity of the tutorial we will add two shortText properties color and size as shown below. You can add more properties if you like.

![](../../../../assets/images/docs/3props.png)

After you are finished adding properties go to property sets tab and click add. Type PropertySet name. We call it "sample properties", select target "All" and add Color and Size properties to property set and click ok.

![](../../../../assets/images/docs/4propetyset.png)

### Create store

Now we should be able to create a new store. Go to settings in eCommerce manager and open Stores tab. Click add and dialog for creating new store is opened.

Fill in required fields. In code field type in SampeStore (this is default store in app settings). Use "Sample Store" as name, select Test catalog and make sure the store is Open.

You can also fill in additional fields if you like, but those should me enough.

![](../../../../assets/images/docs/5store.png)

Click next and select which languages and currencies are available in your store and also select default ones. Click next until you can finish the wizard (settings in those pages are not required for this tutorial)

Now your first store should be created. Go back to front end and try to open home page. It should open successfully. However there are still no categories or items. Creating new items and categories is described in next steps.

![](../../../../assets/images/docs/6home.png)

## Adding items

Before we can add our first product to catalog we need to associate prices with our catalog.

Go to manager price lists by clicking on appropriate drop down ribbon. Then click on price list assigments tab. Click add and dialog should show.

![](../../../../assets/images/docs/priceList.png)

Select available price and catalog and give some name to assignment. Click next (we will not use any conditions now), next (no start end date) and finish.

You can repeat the steps to assign more prices to catalog if you wish.

Now go back to catalog and select Test category. Right click to create category. Leave all default values and make sure category type is chosen. Name the category TV & Video and type tv-video as code for category and click next and save.

Now we can add product to new category. Right click on "TV & Video" category and select "create item" from popup menu. In the dialog select Product and OK. The wizard for creating product is shown. Enter the values as shown below and click next

![](../../../../assets/images/docs/item1.png)

In the second page type some description about product and click next. Property values can also be empty, so click next.

In the next page you have to define prices. Lets add 100 as list price and 99 for sale price. Click finish and item is created.

After this step you might need to re-index the search. Now you should see you new product.