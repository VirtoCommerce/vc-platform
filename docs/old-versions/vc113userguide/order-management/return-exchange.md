---
title: Return / Exchange
description: Return / Exchange
layout: docs
date: 2015-03-18T20:11:12.560Z
priority: 3
---
## Introduction

A positive experience delivered to a customer determines whether that customer will come back or not. Online shop can enhance the user experience prior to purchase but focus on the post-purchase experience is the most important part in keeping the quality of service at satisfactory levels.В Commerce Manager users may perform returns from **Orders** module and returns are processed in **Fulfillment** module.

User must have an appropriate permissions to createВ a return or exchange.В The system automatically creates a unique code, called the Return Merchandise Authorization (RMA) code, for all such transactions. Typically, the RMA number is recorded on the physical return to assistВ fulfillmentВ center employees in matching the receipt of the return to the correct customer account.

## Create return

To create a Return, user must have appropriate permission and theВ order must be in a status of **Complete**.В Steps to create a return:В 

1. OpenВ **Orders**В module, select anВ Order from the list andВ open it.
2. Click **CreateВ return**В button insideВ **Summary**В tab.
3. A 2-step wizard appears. **Items in the order** list contains bought items with quantities. **Items to return** list contains returning items:
  <img src="../../../assets/images/docs/image2013-6-3_17_38_41.png" />
4. Select an item from the list and press
  <img src="../../../assets/images/docs/image2013-6-3_17_45_54.png" />
  button. Tip:В alternatively can double-click on item.
5. A "Specify return data" dialog pops up:
  <img src="../../../assets/images/docs/image2013-6-3_17_53_56.png" />
6. Specify **Return quantity** and **Reason**В fields.В Press OK to confirm.
7. A new record is added toВ **Items to return**В list (or updated an exchsting one). It displays Quantity, Item name and return reason. Also the "Return summary" section underneath the list is updated:
  <img src="../../../assets/images/docs/image2013-6-3_18_5_23.png" />
8. Enter **Customer comment**, if any.
9. Click **Next**. "Adjust Return processing details" step is displayed:
  <img src="../../../assets/images/docs/image2013-6-3_18_11_51.png" />
10. **Total refund amount** is repeated from previous step. User can specify whether a **Physical return** is **required before refund**. This option is checked by default.
11. Click **Finish**. A new RMA request created is saved in DB automatically. Also, the **Return / Exchange** tab is activated.

## Cancel return

RMA request processing can be canceled at any state except it's in state of **Completed**.В Steps to cancel:

1. Select the right RMA request to cancel insideВ **Return / Exchange** tab.
2. Click **Cancel**В button inside selectedВ RMA request. The return is set state of **Canceled**.

## Complete return

RMA request processing can be completed if it's in a state ofВ AwaitingCompletion.В Steps to complete:

1. Select the right RMA request to complete insideВ **Return / Exchange**В tab.
2. ClickВ **Complete**В button inside selectedВ RMA request. If Refund is needed,В aВ "Create Refund for RMA Request" wizard is displayed. This is a "Create Refund" wizard with aВ **Refund amount**В field set to RMA requestВ totalВ return amount initially:
  <img src="../../../assets/images/docs/image2013-6-4_13_51_23.png" />
3. Follow RefundsВ instructions for completing the refund.
4. If refunding succeeded, then:
  * the RMA requestВ state is set to Completed;
  * changes are saved;
  * order payments list is updated.

## Create exchange order

To create an exchange order, user must have an appropriate permission and the RMA request must be in a state of **AwaitingStockReturn** orВ **AwaitingCompletion**.В Steps to create an exchange order:

1. Select the right RMA request to create an exchangeВ insideВ **Return / Exchange**В tab.
2. Click **Create Exchange Order**В button inside selectedВ RMA request.
3. A 2-step wizard "Create an exchange Order" appears. In the wizards step 1 select items (Products) to buy: Select catalog, enter search keyword and add items to a cart. **Available items**В list contains (filtered) available items to buy. **Items in the cart**В list contains current shopping cartВ items:
  <img src="../../../assets/images/docs/image2013-6-4_18_30_57.png" />
