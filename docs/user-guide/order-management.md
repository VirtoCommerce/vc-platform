---
title: Order Management - Virto Commerce 2 User Guide
description: The article about 'Orders' block which contains full information about orders placed by customers on the web store
layout: docs
date: 2015-06-23T14:12:31.890Z
priority: 10
---
**Orders** block contains full information about <a class="crosslink" href="https://virtocommerce.com/shopping-cart" target="_blank">orders</a> placed by customers on the web store.   

The typical steps involved in the order management process are:

* Pick inventory from the warehouse and ship it to the customer
* Send an email notification to the customer that the order is on its way

However, there may be cases when these two simple steps are not enough.
* The order may require manual review, for instance, if the payment gateway fraud decision marked the order as suspicious. This order can be placed `On hold` until it is reviewed manually and confirmed or cancelled
* Some of the items in the order may not match the customers preferences. In this case item may be returned. For instance, if production reject was detected.
* As described in the previous case, some items may not match customers preferences. If there is the same item with required properties is on stock, the item may be exchanged instead of returning. Sometimes (of it is allowed by the store policy), another item may be offered in exchange (for instance, similar model).
* The order is usually completed when all of ordered items are shipped. Some of the items that the customer has ordered may be out of the stock. In that case, the merchant may decide to split the order, and ship out only part of the order with the items that are in stock. The rest of the order will be shipped out at a later time. This is partial shipment.

All of these cases may occur in store, and each of them has a special solution that can be applied in order management block.
