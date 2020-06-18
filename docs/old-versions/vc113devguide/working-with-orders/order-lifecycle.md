---
title: Order Lifecycle
description: Order Lifecycle
layout: docs
date: 2015-03-18T20:11:12.560Z
priority: 4
---
<img src="../../../assets/images/docs/Orderstates.png" />

## Introduction

OrderВ life cycle is divided into separate distinct states. Predefined order states:

```
publicВ enumВ OrderStatus
{
  Pending,
  OnHold,
  PartiallyShipped,
  InProgress,
  Completed,
  Cancelled,
  AwaitingExchange
}
```

An order is alwaysВ in one of these states. (An exception to the rule is allowed only if order data were imported from external data source.) An order is considered to be in a state of **InProgress**, if exact enumeration mach wouldn't be found. When an order [is placed in front end](docs/old-versions/vc113devguide/working-with-orders/order-lifecycle/creating-an-order-in-frontend), it gets a state of **Pending**. An automated process (job)В ProcessOrderStatusWorkВ constantly searches for **Pending** orders and, if some time conditions are met, transits the order to the state of **InProgress**. All other state transitions are initiated from Commerce Manager.
