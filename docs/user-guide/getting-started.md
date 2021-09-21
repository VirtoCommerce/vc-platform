# Getting started

If you are new to Virto Commerce, follow these steps to set up and launch your store.

## Step 1. Create a catalog

<iframe width="560" height="315" src="https://www.youtube.com/embed/6mAkBz1VynM" title="YouTube video player" frameborder="0" allow="accelerometer; autoplay; clipboard-write; encrypted-media; gyroscope; picture-in-picture" allowfullscreen></iframe>

The catalog is what your customers are constantly working with. It is very important that the catalog is easy to browse and find the right products.

Typically, to start working with the new catalog you should follow several steps:

* Create a new catalog
* Create (or import) categories
* Create (or import) a product
* Create a product with variants

Additionally you can read [Working with Products catalog](https://github.com/VirtoCommerce/vc-content/blob/deploy/pages/docs/vc2userguide/merchandise-management/products-catalog.md) article. It describes how to work with products [catalog](https://github.com/VirtoCommerce/vc-content/blob/deploy/pages/docs/vc2userguide/merchandise-management/products-catalog.md#common-catalogs), [categories](https://github.com/VirtoCommerce/vc-content/blob/deploy/pages/docs/vc2userguide/merchandise-management/products-catalog.md#categories), [items](https://github.com/VirtoCommerce/vc-content/blob/deploy/pages/docs/vc2userguide/merchandise-management/products-catalog.md#items-products), product [properties](https://github.com/VirtoCommerce/vc-content/blob/deploy/pages/docs/vc2userguide/merchandise-management/products-catalog.md#properties), product [variations](https://github.com/VirtoCommerce/vc-content/blob/deploy/pages/docs/vc2userguide/merchandise-management/products-catalog.md#variations) and [virtual catalogs](https://github.com/VirtoCommerce/vc-content/blob/deploy/pages/docs/vc2userguide/merchandise-management/products-catalog.md#virtual-catalogs)

## Step 2. Configure your store and prices

<iframe width="560" height="315" src="https://www.youtube.com/embed/osK5iX2uYPM" title="YouTube video player" frameborder="0" allow="accelerometer; autoplay; clipboard-write; encrypted-media; gyroscope; picture-in-picture" allowfullscreen></iframe>

Virto Commerce platform is 4-MULTI: multi-language, multi-currency, multi-theme and multi-store by design. This gives the ability to have and run multiple stores on the same system.

To configure your store you must accomplish a set of actions:

1. Create first store (enter store code and name)
2. Select your catalog
3. Select available currency, languages
4. Configure advanced properties:
    1. Store description, emails and URLs
    2. Fulfillment centers
    3. Operational timezone
5. Setup theme

Open your first store in a browser.

In Virto Commerce, a pricelist is a set of pricing details that can be assigned to items. All prices are stored in a pricelist(s), which have specific currency set. When a pricelist is created, you define which products will be included into this pricelist and what prices will be defined for them.

The way to apply the prices to products on Storefront is creating a Pricelist Assignment. It binds a catalog with pricelist and can add optional conditions for this binding.

Pricelist assignment identifies the pricelist from which a product price is shown for customers. Moreover, pricelist assignments can do much more than simply provide default list and sale prices for products. This feature allows you to provide custom prices to targeted customers according to customer's characteristics such as age, gender, geographic location and searched terms.

Thus, price configuring consists of the following actions:

1. Create pricelist
2. Add items to the pricelist
3. Create pricelist assignment

## Step 3. Configure payments, shipments and taxes

<iframe width="560" height="315" src="https://www.youtube.com/embed/p64d8vmBzJo" title="YouTube video player" frameborder="0" allow="accelerometer; autoplay; clipboard-write; encrypted-media; gyroscope; picture-in-picture" allowfullscreen></iframe>

By default, you have basic payment, shipment and tax providers. It helps you to run checkout process.

For the store, you need to configure:
1. Available Payments methods
2. Available Shipments methods
3. Configure taxes


## Step 4. Launch Marketing & promotional tools

<iframe width="560" height="315" src="https://www.youtube.com/embed/9tpdl84xfm4" title="YouTube video player" frameborder="0" allow="accelerometer; autoplay; clipboard-write; encrypted-media; gyroscope; picture-in-picture" allowfullscreen></iframe>

Marketing your store is essential to gain your customer's loyalty and raise awareness among potential customers.

There are several marketing tools in Virto Commerce: you can publish content, create personalized shopping experiences for customers, manage promotions.

Start with creation of three simple promotions:

1. Each customer would receive a 5% off if cart subtotal exceeded $100
2. Add Gift product for a first time buyer
3. Add 10% discount if product is in-stock

You can find more details about how to create first promotions here.

* [Managing promotions](https://github.com/VirtoCommerce/vc-content/blob/deploy/pages/docs/vc2userguide/marketing/promotions.md)
* [Create a content publishing](https://github.com/VirtoCommerce/vc-content/blob/deploy/pages/docs/vc2userguide/marketing/how-to-add-an-advertising-spot-via-marketing/create-a-content-publishing.md)
* [Dynamic content](https://github.com/VirtoCommerce/vc-content/blob/deploy/pages/docs/vc2userguide/marketing/dynamic-content.md)
* [Combine active promotions](https://github.com/VirtoCommerce/vc-content/blob/deploy/pages/docs/vc2userguide/marketing/combine-active-promotions.md)

## Step 5. Configure inventory

<iframe width="560" height="315" src="https://www.youtube.com/embed/3xRfa0_LUZY" title="YouTube video player" frameborder="0" allow="accelerometer; autoplay; clipboard-write; encrypted-media; gyroscope; picture-in-picture" allowfullscreen></iframe>

Inventory is often the largest item a business has in its current assets, meaning it must be accurately monitored. 

Virto Commerce supports multiple Fulfillment Centers that allow managing availability for different warehouses. 

Thus, inventory configuring consists of the following actions:
1. Go to catalog
2. Select the product
3. Open Fulfillment Centers 
4. Select (Or Create) Fulfillment Centers 
5. Add Quantity 

By default, Stock Quantity will be accurately decreased by new orders and clients could not buy more. Stock control can be deactivated, for example for digital products.

## Step 6. Create Frontend User and Add Theme

<iframe width="560" height="315" src="https://www.youtube.com/embed/Xc8zl0wllmk" title="YouTube video player" frameborder="0" allow="accelerometer; autoplay; clipboard-write; encrypted-media; gyroscope; picture-in-picture" allowfullscreen></iframe>

Virto Commerce is a headless platform and multiple client apps can be connected to it. In this section, we create a frontend user for Virto Commerce Storefront (default storefront web application) and add a theme. 

## Step 7. How To Check the Store Configuration and Go to live with MVP

<iframe width="560" height="315" src="https://www.youtube.com/embed/5LWMgwzss7k" title="YouTube video player" frameborder="0" allow="accelerometer; autoplay; clipboard-write; encrypted-media; gyroscope; picture-in-picture" allowfullscreen></iframe>

Now you are ready to connect a storefront and launch your MVP store.
