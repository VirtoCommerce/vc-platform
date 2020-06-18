---
title: Calling external web service when order is placed online
description: Calling external web service when order is placed online
layout: docs
date: 2015-03-18T20:11:12.560Z
priority: 1
---
## Summary

It is a common integration scenario when order needs to be submitted to external/backend system when it is placed online for further processing. The backend service is guaranteed to be always available so such case needs to be handled. This document explores such scenario and provides guidance on implementing solution to handle this.

## Overview

The basic idea is to save order to the queue (service bus queue or storage queue) on checkout process. Then scheduled job retrieves order from the queue and tries to send it to the web service. If web service is not available then order remains in the queue for the next try.

## Implementation

### Storage Queue Implementation

Virto commerce framework provides all needed functionality to implement such task:

1. Frontend calls workflow вЂњShoppingCartCheckoutWorkflowвЂќ when checkout is proceeded. This workflow can be extended with new activity that writes order to the queue. Read more about creating workflow and activity: [docs/old-versions/vc113devguide/working-with-orders/creating-workflow-activities](docs/old-versions/vc113devguide/working-with-orders/creating-workflow-activities)
2. There are ready classes for accessing Azure queue: AzureQueueReader and AzureQueueWriter (namespace вЂћVirtoCommerce.Foundation.Data.Azure.CQRSвЂњ in вЂћCommerceFoundation.Data.AzureвЂњ assembly). Writer should be used in workflow activity and Reader in job.
3. Job implementation and registering in to the scheduler: [docs/old-versions/vc113devguide/architecture/long-running-tasks](docs/old-versions/vc113devguide/architecture/long-running-tasks)

### Service Bus Queue Implementation

In this scenario you will need to configure service bus. Service bus also offers advanced security and access management.

1. Frontend calls workflow вЂњShoppingCartCheckoutWorkflowвЂќ when checkout is proceeded. This workflow can be extended with new activity that writes order to the service bus queue. Read more about creating workflow and activity:В [docs/old-versions/vc113devguide/working-with-orders/creating-workflow-activities](docs/old-versions/vc113devguide/working-with-orders/creating-workflow-activities)
2. To begin using Service Bus queues in Windows Azure:
  * you must first create a service namespace:В <a href="http://www.windowsazure.com/en-us/documentation/articles/service-bus-dotnet-how-to-use-queues/#create-a-service-namespace" rel="nofollow">http://www.windowsazure.com/en-us/documentation/articles/service-bus-dotnet-how-to-use-queues/#create-a-service-namespace</a>
  * try sample applicationВ to send and receive messages from Service Bus queue:В <a href="http://code.msdn.microsoft.com/windowsazure/Getting-Started-Brokered-aa7a0ac3?fwLinkID=280015" rel="nofollow">http://code.msdn.microsoft.com/windowsazure/Getting-Started-Brokered-aa7a0ac3?fwLinkID=280015</a>. It requires connection string of your created queue. Select your created service bus namespace in Azure managment portal and click "Connection Information" on the bottom of page. Copy "ACS connection string" and paste it to application app.config file (value of setting key "Microsoft.ServiceBus.ConnectionString").
3. Job implementation and registering in to the scheduler: [docs/old-versions/vc113devguide/architecture/long-running-tasks](docs/old-versions/vc113devguide/architecture/long-running-tasks)
4. Subscribe to the topic in the service bus queue to receive notifications when new item is added and submit the order to the external service.

### Other things that may need to consider

Microsoft Azure supports two types of the queue mechanisms: Storage queue and Service Bus queue. Here is detailed comparison of them:В <a href="http://msdn.microsoft.com/en-us/library/windowsazure/hh767287.aspx" rel="nofollow">http://msdn.microsoft.com/en-us/library/windowsazure/hh767287.aspx</a>

Integrating with on-premiseВ web service means to expose it to security risks. Microsoft Azure provides Service Bus Relay Service that helps to establish communication between two applications in more secure and flexible way:

* How to use Service Bus Relay:В <a href="http://www.windowsazure.com/en-us/documentation/articles/service-bus-dotnet-how-to-use-relay/" rel="nofollow">http://www.windowsazure.com/en-us/documentation/articles/service-bus-dotnet-how-to-use-relay/</a>
* More informations about Service Bus Relayed messaging:В <a href="http://msdn.microsoft.com/en-us/library/windowsazure/jj860549.aspx" rel="nofollow">http://msdn.microsoft.com/en-us/library/windowsazure/jj860549.aspx</a>
