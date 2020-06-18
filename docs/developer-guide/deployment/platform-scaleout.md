---
author: VirtoCommerce
category: virtocommerce-docs
date: 2017-05-26
excerpt: The developer guide to Scale-out Virto Commerce platform in Azure App services
tags: [ docs, commerce, demo, ecommerce, sdk, cache, memorycache, redis, scaleout ]
title: 'How Scale-out platform in Azure App Services'
layout: docs
---
## Introduction

Caching tools allows to develop faster and more robust applications. There are several strategies to implement caching technologies, but most popular one is the cache-aside programming pattern: This means that if your data is not present in the cache, your application, and not something else, you must reload data into the cache from the original data source.

## Problem

ItвЂ™s quite simple if you have single instance application with local cache storage. ItвЂ™s easy to manage cache lifecycle and invalidation when entries in a cache are removed/deleted.

But what to do in case with scaled out multiple instances application?

Main problem is how instances finds out when data was changed and itвЂ™s local cache data becomes irrelevant.

The point is to use local cache storage for each application instance and fill cache with fresh data by receiving invalidation message.

## Solution

To resolve this problem we need some service to which all application instances will be connected, and which can broadcast messages when cache data becomes invalidate. Redis is quite good to this task.

Redis is an open-source, networked, in-memory, key-value data store with optional durability. It has good .NET compatibility and allows any connected clients receive messages and invalidate local cache.

## Installation and configuration

Redist cache feature was implemented in Virto Commerce Platform since version 2.13.4.

**Redis**

First you should СЃreate a Redis cache using the Microsoft Azure portal (basic free plan).
 
1. In the Microsoft Azure portal, click New > Data+Storage > Redis Cache.
1. Enter the name of the cache you want to create, choose where in the world you want to run it, and then click Create. It will be ready to use in moments.

![](../../assets/images/docs/redis-create-cache.png)

After you create a Redis cache, use the portal or the command line to configure settings and monitor its usage.

![](../../assets/images/docs/redis-monitor-cache.png)

**Platform**

To start using Redis cache with Virto Commerce application just remove a comment from Redis connection string and set a correct value for it in platform web.config file

```xml
<connectionStrings>
...
    <add name="RedisConnectionString" connectionString="..." />
...
</connectionStrings>
```

**Scale out Platform on Windows Azure**

A scale out operation is the equivalent of creating multiple copies of your Platform instance and adding a load balancer to distribute the demand  between them. When you scale out a Platform in Windows Azure Web Sites there is no need to configure load balancing separately since this is already provided by the Azure.

![](../../assets/images/docs/platform-scale.png)

To scale out the Platform in Windows Azure Web Sites you would use the Instance Count slider to change the instance count between 1 and 6 in Shared mode and 1 and 10 in reserved mode. This will generate multiple running copies of your Platform instance and handle the load balancing configurations necessary to distribute incoming requests across all instances.
