# Platform 3 Overview

[![Deploy to Azure](https://aka.ms/deploytoazurebutton)](https://portal.azure.com/#create/Microsoft.Template/uri/https%3A%2F%2Fraw.githubusercontent.com%2FVirtoCommerce%2Fvc-platform%2Fmaster%2Fazuredeploy.json)

Virto Commerce is a highly scalable eCommerce product for fast-growing and large companies. It provides powerful enterprise-class features right out-of-the-box and gives you the flexibility to create your own unique eCommerce solution while utilizing agile principles.

Our Virto Commerce 3 development efforts were focused on moving to ASP.NET Core, performance, architecture improvements, further enhancements and fixing architectural bugs.

Virto Commerce 3 is a major release and it consists of Virto Commerce Platform and Virto Commerce Modules. It provides easy and clear way to update from 2.x version by preserving complete backward compatibility for API and Database Schema. During development, the platform and 18+ core modules were moved.

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

## Solution Architecture Principles Overview

<a class="crosslink" href="https://virtocommerce.com/glossary/what-is-b2b-ecommerce" target="_blank">Platform and modules</a> are written using <a href="https://en.wikipedia.org/wiki/Domain-driven_design" rel="nofollow">DDD</a>, <a href="https://en.wikipedia.org/wiki/SOLID_(object-oriented_design)" rel="nofollow">SOLID</a>, <a href="https://en.wikipedia.org/wiki/Test-driven_development" rel="nofollow">Test Driven Development</a> methodologies. For a presentation layer, we use <a href="https://en.wikipedia.org/wiki/Model_View_ViewModel" rel="nofollow">MVVM</a>.

<img src="docs/media/architecture-circle.png" title="Virto Commerce 3 Architecture" width="400">

## Introduction to Virto Commerce

These Virto Commerce docs help you learn and use the Virto Commerce platform, from your local solution to optimizing complex enterprise solutions. 

* [Virto Commerce Documentation](https://virtocommerce.com/docs/latest/)
* [View on GitHub](docs/index.md)

## Comparison with Platform 2.x

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
