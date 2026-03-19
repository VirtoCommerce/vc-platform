# What's new

!!! note
    Welcome to v3 on .NET10 - Meet the new version of Virto Commerce Platform. Our development efforts were focused on moving to ASP.NET Core, performance, architecture improvements and further enhancements. The changes below are a subset of the larger list of changes in update to ASP.NET Core.


## Architectural and conceptual changes

### For developers and architects

- Improved extensibility and unification
- Increase in development speed and decrease in time to market
- Unified architecture and usage of good architecture practices leads to shorter learning curve for developers who are new to Virto Commerce
- Cross-Platform Support: run on Windows, Mac or Linux

### Technology stack

#### Application & Data

- .NET 10
- ASP.NET Core 10
    - ASP.NET Core Identity
    - ASP.NET Core SignalR
- Entity Framework Core 10
- OpenIddict 7
- HangFire 1.8.6
- AngularJS 1.8

#### DevOps and Utilities
- Visual Studio 2026
- NodeJS 22 LTS
- Webpack 5

### Techniques

#### Caching
  - ASP.NET Core in-memory caching is used
  - Strongly typed *cache regions* and *change tokens* for cache dependencies
  - Ability to manage cached objects' expiration time and disabling the caching
  - Hybrid caching policy for keeping cached data consistent in multiple Platform instances
  - The dedicated "VirtoCommerce.Cache" module was removed; now caching is implemented in place where it is needed
#### Dependency injection (DI)
  - Unity DI replaced by built-in .NET Core DI (*Microsoft.Extensions.DependencyInjection*)
#### Development
  - Virto Commerce developmet switched to [GitFlow Workflow](https://nvie.com/posts/a-successful-git-branching-model/) model
  - [GitVersion](https://github.com/GitTools/GitVersion) tool to ease Semantic Versioning
  - Both Platform and all modules repository structure unified to have *build*, *docs*, *src*, *tests* folders on the top.
  - The recommended structure of a module solution was updated:
     - *Permissions*, *Settings* and other constants should be defined in the *.Core* project
     - *Caching* should be defined and done in the *.Data* project
     - All v3 modules were refactored to follow the recommended structure
  - All methods in search and CRUD services made asynchronous:
     - Returning *async Task* or *async Task<T\>*
     - The methods renamed to end with "Async"
  - All API controller methods made asynchronous:
     - Returning *async Task* or *async Task<T\>*
     - The endpoint names left unchanged for backward compatibility (**not ending** with "Async")
#### DevOps
  - Nuke - [Build Automation System for C#/.NET aka VirtoCommerce.GlobalTool](https://github.com/VirtoCommerce/vc-build/blob/main/docs/CLI-tools/introduction.md)
#### Dynamic properties
  - Dynamic properties registration logic changed. Now manual registration used instead of reflection
#### Export/Import
  - Export/Import is now streamed for all modules
#### Security
  - Now permissions are defined only in design-time using a special fluent syntax
  - Localization for permissions added
  - Storefront switched to work using barrier token authentication
#### Settings
  - Settings are defined in design-time, using special fluent syntax
  - Localization for settings added
  - Now setting value can be changed using any .NET Core configuration provider
#### Modularity
  - *Module.manifest* file structure changed (scripts, styles, settings and permissions declarations removed)
  - *IModule* abstraction changed to have only *Initialize* and *PostInitialize* methods; *Module.cs* structure simplified.
#### Persistency infrastructure
  - Object-relational mapper (ORM) switched to Entity Framework Core (EF Core)
  - New EF Core migrations generated
  - Only Table per Hierarchy (TPH, hierarchy mapping to a single table) inheritance model now supported
  - DbContext defined separately from repository
  - [EntityFramework.Triggers](https://github.com/NickStrupat/EntityFramework.Triggers) for auditing and change logging
  - Calls to DbContext methods refactored to be asynchronous

## Changes in VC Platform

### Solution code structure
  - Functionality specific implementations were split into dedicated projects (Assets, Modules, etc.)
### Configuration and Options
  - [ASP.NET Core configuration providers](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/configuration/?view=aspnetcore-2.2) used together with the [options pattern](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/configuration/options?view=aspnetcore-2.2).
### Modularity
  - Assembly and dependency loading completely reworked into platform process
### Security
  - Authentification and authorization fully updated to ASP.NET Core Identity without any extensions
  - OpenIddict used as an OpenID Connect server to support all OAuth flows; it's also used for token based authentication
  - OAuth2 Password and Client credentials flow for Machine to Machine (M2M) applications
  - HMAC and simple key authorization support removed for API calls
### Logging
  - Built-in .NET Core *ILogger* abstraction and logic instead of *ICommonLogging* and *NLog*
### Virto Commerce Manager app
  - Webpack + npm instead of Gulp + Bower
  - New make-up for Commerce Manager app UI

## New modules

### Notifications module
  - Functionality, which was spread across the system, now gathered into a dedicated module
  - Unlimited channel types for notification sending (email, SMS, social networks, etc.):
  - Notification availability management for each store
  - Possibility to activate/deactivate each notification individually for each store
  - New flexible extendibility model
  - Notification template preview with data
  - Support of LIQUID syntax for templates based on Scriban engine
  - New notification messages feed enables to search and preview individual messages
  - Enhanced notifications management UI
### Tax module
  - Tax calculation functionality, which was spread across the system, now gathered into a dedicated module, responsible for tax settings and calculation
  - The new module is a single integration point for third party software and custom extensions
### Shipping module
  - Shipping costs calculation functionality, which was spread across the system, now gathered into a dedicated module, responsible for shipping methods, related settings and shipping costs calculation
  - The new module is a single integration point for third party software and custom extensions 
### Payment module
  - Payment methods functionality and integrations, which were spread across the system, now gathered into a dedicated module, responsible for payment methods and related settings
  - The new module is a single point for payment gateways integrations
### Search module
  - Provides a generic UI and programming components for indexed search
  - The new module is a single point for search engine integrations

## Major changes in modules

### Commerce core module
  - `VirtoCommerce.Domain` project removed
    - Now each module self-defines domain model and abstractions in corresponding `.Core` projects. Multiple packages from corresponding modules will be distributed instead.
    - Nuget package `VirtoCommerce.Domain` was left unupdated from previous version. There won't be any update to v3, nor any replacement package in v3.
  - Common functionality and model moved from `Virto Commerce dynamic expression library module`
### Catalog module
  - support for model extending added
  - `VirtoCommerce.CatalogModule.Web.Core` project removed. Model from `.Core` project used in API directly
### Marketing module
  - "Dynamic expression" building refactored to "compile time" expressions
  - New serialization logic for expressions. Serialized expression format changed to JSON (backwards compatible)

## Removed modules

- `Smart caching module`
  - Now caching is implemented in place where it is needed (Platform and modules)
- `Virto Commerce dynamic expression library module`
  - Common functionality and model moved to `Commerce core module`
  - Module specific functionality was split to corresponding modules (Marketing, Pricing, etc.)

## Platform v2 and v3 versions compatibility

### The list of code breaking changes
- The following [list of breaking changes](update-to-version-3/code-breaking-changes-included-in-v3.md) have the potential to break existing solutions when upgrading them to 3.x

### API

- Both versions are compatible on API level. API clients should be able to switch between the versions only by changing the Platform endpoint URL and credentials
- v2 and v3 swagger API specifications comparison generated by [swagger-diff](https://github.com/Sayi/swagger-diff) tool: [v2v3Changelog.html](../media/v2v3Changelog.html)

### Database

  - The v2 and v3 databases have structural differences;
  - Special migrations were added to upgrade Platform and VC modules from v2 database to v3 *automatically*;
  - Any database related extensions made in custom modules, should be upgraded by adding special DB migrations in module code (no manual changes to DB)
  - Any custom existing v2 database should be upgraded before using by Platform v3
 
## Current Virto Commerce Modules

You can find list of Virto Commerce modules by following link: [Virto Commerce Modules](https://docs.virtocommerce.org/platform/user-guide/2.0/versions/virto3-products-versions/)
and more on [GitHub](https://github.com/VirtoCommerce/).



