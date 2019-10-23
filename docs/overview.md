# Platform 3 Overview

## Overview

Virto Commerce is a highly scalable eCommerce product for fast-growing and large companies. It provides powerful enterprise-class features right out-of-the-box and gives you the flexibility to create your own unique eCommerce solution while utilizing agile principles.

Our Virto Commerce 3 development efforts were focused on moving to ASP.NET Core, performance, architecture improvements, further enhancements and fixing architectural bugs.

Virto Commerce 3 is a major release and it consists of Virto Commerce Platform and Virto Commerce Modules. It provides easy and clear migration from 2.x version by preserving complete backward compatibility for API and Database Schema. During development, the platform and 18 core modules were moved.

## Technology Stack Used

In our work, we always try to use advanced technologies. Our decision to choose technologies described below was the result of our extensive experience working with Microsoft products.

We decided to use the following stack of technologies:

* ASP.NET Core 2.2.0 as base platform
* EF Core 2.2.0 as primary ORM
* ASP.NET Core Identity 2.2.0 for authentication and authorization
* OpenIddict 2.0.0 for OAuth authorization
* WebPack as primary design/runtime bundler and minifier
* Swashbuckle.AspNetCore.SwaggerGen for Swagger docs and UI
* SignalR Core for push notifcations
* AngularJS 1.4 as primary framework for SPA
* HangFire 1.6.21 for run background tasks

## Solution Architecture Principles Overview

<a class="crosslink" href="https://virtocommerce.com/glossary/what-is-b2b-ecommerce" target="_blank">Platform and modules</a> are written using <a href="https://en.wikipedia.org/wiki/Domain-driven_design" rel="nofollow">DDD</a>, <a href="https://en.wikipedia.org/wiki/SOLID_(object-oriented_design)" rel="nofollow">SOLID</a>, <a href="https://en.wikipedia.org/wiki/Test-driven_development" rel="nofollow">Test Driven Development</a> methodologies. For a presentation layer, we use <a href="https://en.wikipedia.org/wiki/Model_View_ViewModel" rel="nofollow">MVVM</a> (thanks to AngularJS - the transition from WPF didn’t create any complications).

![Virto Commerce 3 Architecture](/docs/media/architecture-circle.png "Virto Commerce 3 Architecture")

## Comparison with Platform 2.x

In the new version, we change primary technology stack to .NET Core for the platform application and all key modules. Eliminate known technical and architecture design issues of 2.x version (Caching, Overloaded core module, Asynchronous code, Platform Complexity, Extensibility, Performance, Authentication and Authorization)
Improve the extensibility and unification of the application. Unified architecture and good architecture practices usage reduce the training time for developers who just start to work with Virto Commerce.

Virto Commerce Platform 3 helps you increase development speed and significantly reduce time to market.

## Introduction to the Virto Commerce

These Virto Commerce docs help you learn and use the Virto Commerce platform, from your local solution to optimizing complex enterprise solutions. 

* Setup
  * [Deploy Platform 3 from precompiled binaries on Windows](/docs/deploy-from-precompiled-binaries-windows.md)
  * [Deploy Platform 3 from precompiled binaries on Linux](/docs/deploy-from-precompiled-binaries-linux.md)
  * [Connect Storefront to Platform](/docs/connect-storefront-to-platform-v3.md)
* [Getting Started](/docs/getting-started.md)
* [What’s new](/docs/whats-new.md)
* [Migration from 2.0 to 3.0]()
