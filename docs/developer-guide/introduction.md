---
title: Introduction
description: Virto Commerce 2 is a major release and has been in development for over a year. It consists of Virto Commerce Platform and Virto Commerce Modules
layout: docs
date: 2016-06-01T02:41:36.373Z
priority: 1
---
## Overview

<a class="crosslink" href="https://virtocommerce.com/b2b-ecommerce-platform" target="_blank">Virto Commerce 2</a> is a major release and has been in development for over a year. It consists of [Virto Commerce Platform](docs/vc2devguide/working-with-platform-manager) and Virto Commerce Modules.

![Virto Commerce 2 Platform Manager](../assets/images/docs/image02.png "Virto Commerce 2 Platform Manager")

## Technology Stack Used

![Virto Commerce 2 Technology Stack](../assets/images/docs/image04.jpg "Virto Commerce 2 Technology Stack")

Our decision to use these technologies was the consequence of our extensive experience with Microsoft products.

We decided to use the following stack of technologies:

* **ASP.NET MVC 5** - as a web hosting technology
* **ASP.NET Web API 2**В - to implement REST services
* **Entity Framework 6.1**В - ORM
* **EF Migrations**В - for managing database schema changes and data
* **ASP.NET Identity**В - tasks related to authorization and authentication

**AngularJS**В - forВ **SPA**В user interface. One of the major decisions for picking it is due to its popularity and quality documentation. We didnвЂ™t have any prior experience working with AngularJS. Looking back, we never had any regrets with our decision to do so.

## Architecture

<a class="crosslink" href="https://virtocommerce.com/glossary/what-is-b2b-ecommerce" target="_blank">Platform and modules</a> are written using <a href="https://en.wikipedia.org/wiki/Domain-driven_design" rel="nofollow">DDD</a>, <a href="https://en.wikipedia.org/wiki/SOLID_(object-oriented_design)" rel="nofollow">SOLID</a>, <a href="https://en.wikipedia.org/wiki/Test-driven_development" rel="nofollow">Test Driven Development</a>В methodologies. For a presentation layer, we use <a href="https://en.wikipedia.org/wiki/Model_View_ViewModel" rel="nofollow">MVVM</a>В (thanks to AngularJS - the transition from WPF didnвЂ™t create any complications).

![Virto Commerce 2 Architecture](../assets/images/docs/architecture-circle.png "Virto Commerce 2 Architecture")

## Platform Capabilities

Now let's look at a list of core platform capabilities. It is important to understand that this list contains platform capabilities. These platform capabilities are not add-on features developed on top of an ecommerce application.

## Style Guide

![Virto Commerce 2 Styleguide Screenshot](../assets/images/docs/image01.png "Virto Commerce 2 Styleguide Screenshot")

We created our own theme for an application using <a href="http://operatino.github.io/MCSS/en/" rel="nofollow">MCSS</a>В methodology. We also created aВ [style guide](guides/style-guide)В andВ [visual constructor](guides/blade-constructor)В for navigation elements (blades), allowing you to easily create an HTML markup that can then be used in extension modules.

## Navigation

![Virto Commerce 2 Navigation Overview](../assets/images/docs/image00.png "Virto Commerce 2 Navigation Overview")

Here, we present a unified navigation concept for user interfaces. We really liked the navigation idea used in the new <a href="http://portal.azure.com" rel="nofollow">portal.azure.com</a>В portal with horizontal scrolling, and we decided to take it as a base.

The main navigation elements are:

* **blade** - analog of Windows in classic interface
* **widget container** which contains **widget**
* **main menu** - global navigation menu for all the modules

## Modularity

![Virto Commerce 2 Modules Screenshot](../assets/images/docs/image03.png "Virto Commerce 2 Modules Screenshot")

Platform allows extending system function in runtime by installing extension modules. Considering that each module can provide its ownВ **user interface and REST API services**, the platform can be customized to serve many specific business needs.

Modularity is one of the most difficult problems we encountered. To solve it, we had to adopt <a href="https://compositewpf.codeplex.com/" rel="nofollow">Microsoft PRISM</a>В from WPF and make it work in ASP.NET MVC. Modules contain information about versions and dependencies which are used by the system during module initialization and installation.

Besides extending user interface and API, each module can use its own Database Schema with support for data migration during version updates. It can also extend or overwrite functionality from other modules using Unity - IoC and Dependency injection container.

We wonвЂ™t list all the user interface extensibility points, but here are just a few of them: main menu, toolbars, widgets, notifications, etc.

## Import/Export

Platform comes with advanced import/export functionality that allows each module to specify (through API interface) how the data can be imported and exported from that module and platform provides a unified UI to export and import data as shown below.

*View of platform export/import UI:*

![Virto Commerce 2 Export Screenshot](../assets/images/docs/export.png "Virto Commerce 2 Export Screenshot")

This feature can then be used for various scenarios including:

* **Publishing** content fromВ **Staging to Production**В environment
* Creating **starter e-commerce stores**В with all themes, catalogs, products, properties, promotions, marketing banners etc as preconfigured
