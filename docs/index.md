# Overview

## Virto Commerce Platform - Extensible E-commerce Applications

Virto Commerce is an open-source platform to build extensible e-commerce applications: complex digital commerce solutions for B2B, B2C or B2B2C businesses, marketplaces and derived SaaS commerce platforms.

Virto Commerce architecture was designed on principles: Microservices, API-first, Cloud-native, Headless, and Advanced Extensibility.


## Principles
The main principle is to help the development team to focus on the implementation of business features and don’t worry about **CLEAN ARCHITECTURE**.

* **MICROSERVICES** – Every application is built from headless microservices (modules). Applications and microservices are not limited to the composite applications, they can be used for building any other application and hence are functionally independent. 
* **API-FIRST** – E-commerce service with the right API design. All business logic is accessible via API: Rest or GraphQL. 
* **HEADLESS** – Allows an enterprise to support omnichannel journeys across traditional and digital touchpoints as well as new business models.
* **CLOUD NATIVE** – E-commerce service is delivered in the SaaS model. Get significant benefits for the business from: 
    1. *On-demand* - Use e-commerce service as a whole or its separate components as needed; 
    1. *Scalability* - In the public cloud, it can be easily scaled to support peak demand and long-term business growth; 
    1. *Reliability* - Can leverage a solution deployed across multiple data centers and availability zones to maximize up-time and reduce potential revenue losses.
* **SINGLE RESPONSIBILITY** – Every module should be as simple as possible, so a new developer can support and improve it.
* **EXTENSIBILITY** – The API model, persistence model, business logic can be extended as needed without deploying and re-deploying solution. This provides superior business agility and keeps up to date.
* **SECURITY** – Role-based security as the core functionality.
* **FLEXIABILITY** – Configure data and relations based on organization structure, contracts and dynamic conditions.

## Architecture Overview
The following diagram illustrates the high-level architecture and main areas of Virto Commerce solutions:

![Architecture Reference](media/vc-architecture-reference.png)

**Virto Commerce Platform** - Launcher of e-commerce applications in the cloud. 

**Commerce Applications** - API-based, Modular and Extensible logical set of one or several headless microservices (modules) with focus on the implementation of the business feature, like Digital Catalog, Order Management, Content Management, Marketing, etc.

**Custom Extensions** - Virto Commerce Module which allows extending API-model, Persistent model, Business logic and Admin UI in Commerce Applications.

**External Commerce Applications** - 3rd-party e-commerce applications and services. 

**Touchpoints** - Sell in your products on the website, mobile application, chatbot or any through 3rd party services: Marketplace, Dropshipping, or whatever you create. Virto Commerce Storefront Kit allows managing different brands and store under the same environment and with same features.

**Admin SPA** - Virto Commerce has an extensible and intuitive admin user interface. It lets you manage data in Commerce Applications for all channels.

**Integration middleware** - Asynchronous integration middleware for declarative integration with Non-Real-time and legacy services.


### Virto Commerce Platform
**Virto Commerce Platform** - is launcher of e-commerce applications in the cloud. It brings system functionality, modularity, dependency resolving, role-based security, API, etc.  

### Commerce Applications
**Commerce Application** - is API-based, Modular and Extensible logical set of one or several headless microservices (modules) with focus on the implementation of the business feature, like Digital Catalog, Order Management, Content Management, Marketing, etc.

The following diagram illustrates the high-level architecture of Digital Catalog application, which by default consist of Catalog, Search, Pricing, Inventory, Personalization and Store modules:

![Architecture Reference](media/vc-architecture-application.png)

Each of the applications is complete by itself and not dependent on the functioning of other applications. The constituent apps have their own consumers and interaction points. Selecting Commerce Application, you can configure the ecosystem based on your requirements. 

The different applications can be deployed / launched in different isolated environments. The application can be scaled and run on multiple instances.

The applications can be extended with custom modules. You can extend API model, persistence model, business logic and admin UI. For example, architecture reference includes Pricing Extensions which extend API model, Persistence model and Admin UI with the Recommended price field.

### Virto Commerce Headless Microservice (Module)
**Virto Commerce Headless Microservice (Module)** - is a development unit. Can consist of one or several microservice. A module must fulfill a single purpose that is narrowly defined and easy to understand. 

The following diagram illustrates the high-level structure:

!!! notes
    Virto Commerce provides implementations and utilities for all blocks out-of-the-box and the developer can focus on the implementation of business requirements:

![Architecture Reference](media/vc-architecture-module.png)

Virto Commerce Module consists of several layers:

1. **Module Manifest** - gives the information about the module, such as the most important files, dependencies and the capabilities the extension might use.
1. **Tests and Documentation** - an important part of any module, which helps a new developer to learn and improve it.
1. **API** - All business logic accessible via API: Rest or GraphQL.
1. **Security** - access to API is limited by permissions.
1. **Admin UI (optional)** - the module provides the intuitive admin user interface. It lets you manage data in Admin SPA. 
1. **Cache (optional)** - From the business logic, you can use distributed cache to improve performance. 
1. **Business Logic** - The module solution structure is constructed using N-Tier and DDD principle and it is a business unit that is able to fully provide a set of desired features.
1. **Events (optional)** - From the business logic, you can send internal and external events (Webhooks).
1. **Database** - The module has a repository and doesn't have a connection with other modules on the database layer. The module can have custom connection string and store data in the custom database. 
1. **Background Jobs (optional)** - The module can run long-running operations as Background Jobs.
1. **Log/Monitoring** - The module has native integration with Azure Application Insights service, as Native monitoring tool for .NET Core applications.

## Technology Stack Used

In our work, we always try to use advanced technologies. Our decision to choose technologies described below was the result of our extensive experience working with Microsoft products.

Virto Commerce uses following stack of technologies:

* ASP.NET Core 3.1.0
* EF Core 3.1.0 as primary ORM
* ASP.NET Core Identity 3.1.0 for authentication and authorization
* OpenIddict 2.0.1 for OAuth authorization
* WebPack as primary design/runtime bundler and minifier
* Swashbuckle.AspNetCore.SwaggerGen for Swagger docs and UI
* SignalR Core for push notifications
* AngularJS 1.4 as primary framework for SPA
* HangFire 1.7.8 for run background tasks

## Next
* [Getting Started](getting-started/deploy-from-precompiled-binaries-azure.md)
