---
title: System Requirements - Virto Commerce 2 Developer Guide
description: System Requirements for Virto Commerce
layout: docs
date: 2015-12-03T21:55:00.170Z
priority: 2
---
## Software requirements

* Windows Server 2008 or later (IIS 7 or later)В 
* Microsoft .NET Framework 4.6.1В 
* Visual C++ Redistributable Packages for Visual Studio 2013 (for storefront)В 
* Microsoft SQL Server 2008 or later

## Storage requirements

Precompiled <a class="crosslink" href="https://virtocommerce.com/b2b-ecommerce-platform" target="_blank">Virto Commerce</a> Manager with storefront take about 130 MB. When using the Deploy to Azure button on GitHub, the Azure web app downloads the source code and compiles it on the Azure web app - this takes about 800 MB.This means even the F1 (free) service plan is enough to host both web applications. S2 has 50 GB of storage, which is more than enough for both applications and product assets (images) of very large catalog.В So when choosing between S2 and P2 you should only consider the performance of these instances. 

Also it is advisable to store product assets in a separate blob storage (Azure Storage Account).
