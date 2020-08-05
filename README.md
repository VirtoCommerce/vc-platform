# Virto Commerce Platform - Extensible Ecommerce Applications

[![Deploy to Azure](https://aka.ms/deploytoazurebutton)](https://portal.azure.com/#create/Microsoft.Template/uri/https%3A%2F%2Fraw.githubusercontent.com%2FVirtoCommerce%2Fvc-platform%2Fmaster%2Fazuredeploy.json)

Virto Commerce is an open-source platform for an extensible e-commerce applications.

Complex digital commerce solutions for B2B, B2C or B2B2C businesses, marketplaces and derived SaaS commerce platforms.

Virto Commerce architecture was designed on principles: Microservices, API-first, Cloud-native, Headless, and Advanced Extensibility.

## Principles
The main principle is to help the development team to focus on the implementation of business features and don’t worry about **CLEAN ARCHITECTURE**.

* **MICROSERVICES** – Every application is built from headless microservices (modules). Applications and microservices are not limited to the composite applications, they can be used for building any other application and hence are functionally independent. 
* **API-FIRST** – E-commerce service with the right API design. All business logic is accessible via API: Rest or GraphQL. 
* **CLOUD NATIVE** – E-commerce service is delivered in the SaaS model. Get significant benefits for the business from: 
    1. *On-demand* - Use e-commerce service as a whole or its separate components as needed; 
    1. *Scalability* - In the cloud, it can be easily scaled to support peak demand and long-term business growth; 
    1. *Reliability* - Can leverage a solution deployed across multiple data centers and availability zones to maximize up-time and reduce potential revenue losses.
* **HEADLESS** – Allows an enterprise to support omnichannel journeys across traditional and digital touchpoints as well as new business models.
* **EXTENSIBILITY** – The API model, persistence model, business logic can be extended as needed without deploying and re-deploying solution. This provides superior business agility and keeps up to date.
* 
## Architecture Overview
The following diagram illustrates the high-level architecture and main areas of Virto Commerce solutions:

![Virto Commerce Architecture Reference](docs/media/vc-architecture-reference.png)

**Virto Commerce Platform** - Launcher of e-commerce applications in the public, hybrid and private cloud. 

**Commerce Applications** - API-based, Modular and Extensible logical set of one or several headless microservices (modules) with focus on the implementation of the business feature, like Digital Catalog, Order Management, Content Management, Marketing, etc.

**Custom Extensions** - Virto Commerce Module which allows extending API-model, Persistent model, Business logic and Admin UI in Commerce Applications.

**External Commerce Applications** - 3rd-party e-commerce applications and services. 

**Touchpoints** - Sell in your products on the website, mobile application, chatbot or any through 3rd party services: Marketplace, Dropshipping, or whatever you create. Virto Commerce Storefront Kit allows managing different brands and store under the same environment and with same features.

**Admin SPA** - Virto Commerce has an extensible and intuitive admin user interface. It lets you manage data in Commerce Applications for all channels.

**Integration middleware** - Asynchronous integration middleware for declarative integration with Non-Real-time and legacy services.


## Technology Stack Used

In our work, we always try to use advanced technologies. Our decision to choose technologies described below was the result of our extensive experience working with Microsoft products.

We decided to use the following stack of technologies:

* ASP.NET Core 3.1.0 as base platform
* EF Core 3.1.0 as primary ORM
* ASP.NET Core Identity 3.1.0 for authentication and authorization
* OpenIddict 2.0.1 for OAuth authorization
* WebPack as primary design/runtime bundler and minifier
* Swashbuckle.AspNetCore.SwaggerGen for Swagger docs and UI
* SignalR Core for push notifications
* AngularJS 1.4 as primary framework for SPA
* HangFire 1.7.8 for run background tasks

## Introduction to Virto Commerce

These Virto Commerce docs help you learn and use the Virto Commerce platform, from your local solution to optimizing complex enterprise solutions. 

* [Virto Commerce Documentation](https://virtocommerce.com/docs/latest/)
* [View on GitHub](docs/index.md)

## Comparison with 2.x

In the new version, we change primary technology stack to .NET Core for the platform application and all key modules. Eliminate known technical and architecture design issues of 2.x version (Caching, Overloaded core module, Asynchronous code, Platform Complexity, Extensibility, Performance, Authentication and Authorization)
Improve the extensibility and unification of the application. Unified architecture and good architecture practices usage reduce the training time for developers who just start to work with Virto Commerce.

Virto Commerce Platform 3 helps you increase development speed and significantly reduce time to market.

## References

* [What’s new](docs/release-information/whats-new.md)
* Deploy
  * [Deploy on Windows](docs/getting-started/deploy-from-precompiled-binaries-windows.md)
  * [Deploy on Linux](docs/getting-started/deploy-from-precompiled-binaries-linux.md)
  * [Deploy to Azure](docs/getting-started/deploy-from-precompiled-binaries-azure.md) 
  * [Deploy on MacOS](docs/getting-started/deploy-from-precompiled-binaries-MacOS.md) 
  * [Connect Storefront to Platform](docs/getting-started/connect-storefront-to-platform-v3.md)
  * [Deploy Platform 3 from source code](docs/developer-guide/deploy-from-source-code.md)
* [Getting Started](docs/user-guide/getting-started.md)
* [Update VC Platform Module from version 2.x to 3](docs/release-information/update-to-version-3/update-module-from-platform-2.0-to-version-3.md)
* Virto Commerce Documentation: https://www.virtocommerce.com/docs/latest/
* Home: https://virtocommerce.com
* Community: https://www.virtocommerce.org
* [Download Latest Release](https://github.com/VirtoCommerce/vc-platform/releases/latest)

## License

Copyright (c) Virto Solutions LTD.  All rights reserved.

Licensed under the Virto Commerce Open Software License (the "License"); you
may not use this file except in compliance with the License. You may
obtain a copy of the License at

http://virtocommerce.com/opensourcelicense

Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or
implied.
