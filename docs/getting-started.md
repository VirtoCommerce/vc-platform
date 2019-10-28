---
date: '2019-10-22'
title: 'Virto Commerce Getting started'
layout: docs
---


# Getting started

If you are new to Virto Commerce, follow these steps to set up and launch your store.

## Step 1. Create a catalog

The catalog is what your customers are constantly working with. It is very important that the catalog is easy to browse and find the right products.

Typically, to start working with the new catalog you should follow several steps:

* Create a new catalogue
* Create categories
* Create a product
* Create a product with variants

Additionally you can read [Working with Products catalog](https://github.com/VirtoCommerce/vc-content/blob/deploy/pages/docs/vc2userguide/merchandise-management/products-catalog.md) article. It describes how to work with products [catalog](https://github.com/VirtoCommerce/vc-content/blob/deploy/pages/docs/vc2userguide/merchandise-management/products-catalog.md#common-catalogs), [categories](https://github.com/VirtoCommerce/vc-content/blob/deploy/pages/docs/vc2userguide/merchandise-management/products-catalog.md#categories), [items](https://github.com/VirtoCommerce/vc-content/blob/deploy/pages/docs/vc2userguide/merchandise-management/products-catalog.md#items-products), product [properties](https://github.com/VirtoCommerce/vc-content/blob/deploy/pages/docs/vc2userguide/merchandise-management/products-catalog.md#properties)), product [variations](https://github.com/VirtoCommerce/vc-content/blob/deploy/pages/docs/vc2userguide/merchandise-management/products-catalog.md#variations) and [virtual catalogs](https://github.com/VirtoCommerce/vc-content/blob/deploy/pages/docs/vc2userguide/merchandise-management/products-catalog.md#virtual-catalogs)

## Step 2. Configure your store

Virto Commerce platform is 4-MULTI: multi-language, multi-currency, multi-theme and multi-store by design. This gives the ability to have and run multiple stores on the same system.

To configure your store you must accomplish a set of actions:

1. Create first store
2. Select your catalog
3. Select available currency, languages
4. Configure advanced properties:
    1. Store description, emails and URLs
    2. Fulfillment centers
    3. Operational timezone
5. Setup theme

Check that new store is visible.

The [Managing stores](https://github.com/VirtoCommerce/vc-content/blob/deploy/pages/docs/vc2userguide/configuration/store.md) article helps you to configure your first store. Also at this step you should configure [Users, Roles and Role Assignment](https://github.com/VirtoCommerce/vc-content/blob/deploy/pages/docs/vc2userguide/users-management-roles-and-role-assignment.md)

## Step 3. Configure prices and inventory

In the Virto Commerce, a price list is a set of pricing details that can be assigned to items. All prices are stored in a price list, which is associated with a catalog and a currency. When the price list is created in the Virto Commerce, you need to define which products will be included into this price list and what prices will be defined for them. You can do it by creating price list assignment.

Price list assignment identifies the price list from which a product price is shown for customers. Moreover, price list assignments can do much more than simply provide default list and sale prices for products. This feature allows you to provide custom prices to targeted customers according to customer's characteristics such as age, gender, geographic location and searched terms.

Thus, price configuring consists of the following actions:

1. Create price list
2. Add items to the price list
3. Create price list assignment
4. Manage inventory

After this step completed, you can [Open new products in Virto Commerce Storefront](https://github.com/VirtoCommerce/vc-content/blob/deploy/pages/docs/lessons/lesson2.md#open-new-products-in-virto-commerce-storefront-frontend) , browse the catalog, add product to shopping cart and create order. You can view customer order via admin UI.

## Step 4. Configure payments, shipments, taxes

By default, you see demo payment and shipmen methods. Virto Commerce provides the ready to use module which. You can set your own configurations:

* Install Payment module (For example: PayPal)
* Configure [Shipments](https://github.com/VirtoCommerce/vc-content/blob/deploy/pages/docs/vc2userguide/order-management/working-with-shipments.md) method
* Configure [Taxes](https://github.com/VirtoCommerce/vc-content/blob/deploy/pages/docs/vc2userguide/order-management/working-with-taxes.md) method

## Step 5. Launch Marketing & promotional tools

Marketing your store is essential to gain your customers loyalty and raise awareness among potential customers.

There are several marketing tools in Virto Commerce: you can publish content, create personalized shopping experiences for customers, manage promotions.

Start with creation of two simple promotions:

1. 5% if cart subtotal > 100 $
2. Add Gif product

Then you can play with extended e-commerce marketing, like 10% if product is in-stock.

You can find more details about how to create first promotions here:

* [Managing promotions](https://github.com/VirtoCommerce/vc-content/blob/deploy/pages/docs/vc2userguide/marketing/promotions.md)
* [Create a content publishing](https://github.com/VirtoCommerce/vc-content/blob/deploy/pages/docs/vc2userguide/marketing/how-to-add-an-advertising-spot-via-marketing/create-a-content-publishing.md)
* [Dynamic content](https://github.com/VirtoCommerce/vc-content/blob/deploy/pages/docs/vc2userguide/marketing/dynamic-content.md)
* [Combine active promotions](https://github.com/VirtoCommerce/vc-content/blob/deploy/pages/docs/vc2userguide/marketing/combine-active-promotions.md)

## Step 6. Configure User Experience

To enhance User Experience Virto Commerce contains a set of tools:

* [Menu item in Virto Commerce Storefront](https://github.com/VirtoCommerce/vc-content/blob/deploy/pages/docs/lessons/lesson2.md#creating-a-new-menu-item-in-virto-commerce-storefront-frontend)
* Catalog filters in Virto Commerce Storefront
* Currencies, languages
* Link lists
* Theme settings file
* Login on behalf
* Social login

## Step 7. Go to live with MVP and review additional cases and Virto Commerce

Now you are ready to launch your MVP store.
