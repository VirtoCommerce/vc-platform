# Platform 3 Overview

## Overview

Virto Commerce is a highly scalable eCommerce product for fast-growing and large companies. It provides powerful enterprise-class features right out-of-the-box and gives you the flexibility to create your own unique eCommerce solution while utilizing agile principles.

Our Virto Commerce 3 development efforts were focused on moving to ASP.NET Core, performance, architecture improvements, further enhancements and fixing architectural bugs.

Virto Commerce 3 is a major release and it consists of Virto Commerce Platform and Virto Commerce Modules. It provides easy and clear migration from 2.x version by preserving complete backward compatibility for API and Database Schema. During development, the platform and 18 core modules were moved.

## Technology Stack Used

In our work, we always try to use advanced technologies. Our decision to chose technologies described below was the result of our extensive experience working with Microsoft products.

We decided to use the following stack of technologies:

* ASP.NET Core 2.2.0 as base platform
* EF Core 2.2.0 as primary ORM
* ASP.NET Core Identity 2.2.0 for authentification and authorization
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

## Platform Capabilities

Now let's look at a list of core platform capabilities. It is important to understand that this list contains platform capabilities. These platform capabilities are not add-on features developed on top of an ecommerce application.

### Style Guide

We created our own theme for an application using <a href="http://operatino.github.io/MCSS/en/" rel="nofollow">MCSS</a> methodology. We also created a [style guide](guides/style-guide) and [visual constructor](guides/blade-constructor) for navigation elements (blades), allowing you to easily create an HTML markup that can then be used in extension modules.

### Navigation

![Virto Commerce 3 Navigation Overview](../../assets/images/docs/image00.png "Virto Commerce 3 Navigation Overview")

Here, we present a unified navigation concept for user interfaces. We really liked the navigation idea used in the new <a href="http://portal.azure.com" rel="nofollow">portal.azure.com</a> portal with horizontal scrolling, and we decided to take it as a base.

The main navigation elements are:

* **blade** - analog of Windows in classic interface
* **widget container** which contains **widget**
* **main menu** - global navigation menu for all the modules


### Modularity

![Virto Commerce 3 Modules Screenshot](../../assets/images/docs/image03.png "Virto Commerce 3 Modules Screenshot")

Platform allows extending system function in runtime by installing extension modules. Considering that each module can provide its own **user interface and REST API services**, the platform can be customized to serve many specific business needs.

Modularity is one of the most difficult problems we encountered. To solve it, we had to adopt <a href="https://compositewpf.codeplex.com/" rel="nofollow">Microsoft PRISM</a> from WPF and make it work in ASP.NET MVC. Modules contain information about versions and dependencies which are used by the system during module initialization and installation.

Besides extending user interface and API, each module can use its own Database Schema with support for data migration during version updates. It can also extend or overwrite functionality from other modules using Unity - IoC and Dependency injection container.

We won’t list all the user interface extensibility points, but here are just a few of them: main menu, toolbars, widgets, notifications, etc.

### Import/Export

Platform comes with advanced import/export functionality that allows each module to specify (through API interface) how the data can be imported and exported from that module and platform provides a unified UI to export and import data as shown below.

*View of platform export/import UI:*

![Virto Commerce 3 Export Screenshot](../../assets/images/docs/export.png "Virto Commerce 3 Export Screenshot")

This feature can then be used for various scenarios including:

* **Publishing** content from **Staging to Production** environment
* Creating **starter e-commerce stores** with all themes, catalogs, products, properties, promotions, marketing banners etc as preconfigured


## The future

We do not stop there and continue to develop Virto Commerce. The next version aims to improve Virto Commerce Platform in a few key ways:

* TODO aim 1
* TODO aim 2
* TODO aim 3

## References

* Setup
  * [Deploy Platform 3 from precompiled binaries on Windows](/docs/deploy-from-precompiled-binaries-windows.md)
  * [Deploy Platform 3 from precompiled binaries on Linux](/docs/deploy-from-precompiled-binaries-linux.md)
  * [Connect Storefront to Platform](/docs/connect-storefront-to-platform-v3.md)
* [Getting Started](/docs/getting-started.md)
* [What’s new](/docs/whats-new.md)
* [Migration from 2.0 to 3.0]()