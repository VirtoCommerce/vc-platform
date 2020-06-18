---
title: Frontend B2C Accelerator
description: Frontend B2C Accelerator
layout: docs
date: 2015-03-18T20:11:12.560Z
priority: 2
---
Frontend Accelerator is designed to provide out of the box Business to Consumer website that can then be easily extended to incorporate custom design and business logic.

The website is built using MVC4/Razor technologies and utilizes best Microsoft practices for web development.

## Folders

|Name|Description|
|----|-----------|
|/App_Data|Contains configuration and log files|
|/App_Start|Classes that are initialized when application starts|
|/Content|CSS and Image resources|
|/Controllers|Controller classes|
|/Models|Various models used by the frotnend|
|/Scripts|Javascripts|
|/Views|Razor view templates|
|/Virto|Virto Commerce specific classes and implementations including bootstrapper|

## Security and Authentication

B2C Accelerator implement standard .net authentication and utilizes <a href="http://msdn.microsoft.com/en-us/library/webmatrix.webdata.websecurity(v=vs.111).aspx" rel="nofollow">WebSecurity</a> class for authenitcation purpose. WebSecurity is a more universal version of Membership Provider model used in the previous versions of .net. Under the covers in still relies on membership provider. Because of this simplified and more universal API it makes it very easy to configure integration with public authentication API's like google, facebooks and so on. For more information on how to configure it check <a href="http://go.microsoft.com/fwlink/?LinkID=252166" rel="nofollow">http://go.microsoft.com/fwlink/?LinkID=252166</a>.

## Web Services

B2C Accelerator includes the following web services:

### SOAP Services

* ~/Virto/Services/AuthenticationService.svc
* ~/Virto/Services/AssetService.svc
* ~/Virto/Services/ImportService.svc
* ~/Virto/Services/SecurityService.svc
* ~/Virto/Services/OrderService.svc

### OData Services

* ~/Virto/DataServices/ImportDataService.svc
* ~/Virto/DataServices/AppConfigDataService.svc
* ~/Virto/DataServices/CatalogDataService.svc
* ~/Virto/DataServices/CustomerDataService.svc
* ~/Virto/DataServices/DynamicContentDataService.svc
* ~/Virto/DataServices/InventoryDataService.svc
* ~/Virto/DataServices/MarketingDataService.svc
* ~/Virto/DataServices/OrderDataService.svc
* ~/Virto/DataServices/ReviewDataService.svc
* ~/Virto/DataServices/SearchDataService.svc
* ~/Virto/DataServices/SecurityDataService.svc
* ~/Virto/DataServices/StoreDataService.svc

All the services besides AuthenticationService are secure and use OAuth/Token authentication. This means the valid token must be passed before any method is called. The token is obtained using authentication service.

Services are configured inside web.config file under the front end root folder. It is possible and sometime necessary to move services to a different website. Simply copy necessary config files located under App_Data folder as well as configuration settings from web.config file.

## Payment Gateways

Payments must be first added in the Commerce Manager and then associated with a store. In order for the payment UI to be displayed the template with a name of the registered payment method must be added under under Checkout\DisplayTemplate, for example Credit.chtml for the credit payment method. Also IPaymentOption implementation must be added that will add appropriate payment object into the order object.
