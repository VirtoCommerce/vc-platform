# Meet the new 3.0 major version of Virto Commerce platform 
Our development efforts were focused on moving to ASP.NET Core, performance, architecture improvements, further enhancements and fixing arhitectural bugs. 

## What were our objective when starting development project on VC platform v3? 

- Change primary technology stack to .NET Core for the platform application and all key modules. 
- Eliminate known technical and architecture design issues of 2.x version (Caching, Overloaded core module, Asynchronous code, Platform Complexity, Extensibility, Performance, Authentication and Authorization) 
- Provide easy and clear migration from 2.x version by preserving complete backward compatibility for API and Database Schema 
- The platform and 18 core modules were planned to be migrated. 

## Release status note 
- *We encourage you to try and investigate the new version of the product and give us your feedback*
- *This is a beta release, which hasn't been verified on a production project yet*
- *We have delivered a simple migration from 2.x version by preserving complete backward compatibility for API and Database Schema. You'll need an additional effort when there are custom changes in 2.x version. Please follow our migration guide during the project migration*
- **_We cannot guarantee the backward compatibility of current beta version with the final 3.X release_**

## The Virto Commerce Release Notes below are a subset of the larger list of changes in migration to ASP.NET Core. 

## What does Virto V3 provide to developers and architects?
- Improved extensibility and unification.
- Increase in development speed and decrease in time to market. 
- Unified architecture and usage of good architecture practices leads to shorter learning curve for developers who are new to working with Virto Commerce.

## Used technological stack 
- **ASP.NET Core 2.2.0** as base platform 
- **EF Core 2.2.0** as primary ORM
- **ASP.NET Core Identity 2.2.0** for authentification and authorization
- **OpenIddict 2.0.0** for OAuth authorization
- **WebPack** as primary design/runtime bundler and minifier
- **Swashbuckle.AspNetCore.SwaggerGen** for Swagger docs and UI
- **SignalR Core** for push notifcations
- **AngularJS 1.4** as primary framework for SPA
- **HangFire 1.6.21** for run background tasks

**Platform changes**:
  - Configuration
    - Use NET Core configuration paradigm (configuration providers and strongly types IOptions)
  - Solution structure
    - Split concrete implementations into projects (Modules, Assets etc)
  - DI
    - Replaced Unity DI with builtin .NET Core DI Microsoft.Extensions.DependencyInjection
  - Modularity
    - Completely reworked assembly and dependency loading into platform process
    - Changed IModule abstraction to have only two methods Initialize and PostInitialize.
    - Changed module.manifest file structure (removed settings and permissions sections)
 - Security
    - Completely migrate authentification and authorization to the default ASP.NET Identity without any extensions
    - OpenIddict server to support all OAuth flows also used for token based authorization
    - Removed Hmac and simple key authorization for call platform API
    - Now permissions are defined only in design time in special fluent syntax
    - Added localization for permissions
    - The storefront switched to work with using barrier token authorization
 - Persistent infrastructure
    - New migrations
    - TPH inheritence model only (map hierarchy to single table)
    - DbContext now is defined separately from repository
    - Using  DbContext triggers for auditing and change logging
    - Switch to asynchronous calls of DbCOntext methods
 - Settings
    - Now settings are defined only in design time in special fluent syntax
    - Added localization for settings
    - Allow to change setting value through any .NET Core configuration provider
 - Caching
    - Replaced CacheManager with ASP.NET InMemory
    - Strongly typed cache regions and cache dependencies 
    - Allow to manage expiration time of cached objects and disable cache 
    - Removed special CacheModule, now caching is implemented in place where it is needed. 
 - Dynamic properties
    - Changed registration logic, now using manual registration instead of using reflection as it was done in 2.x
 - Logging
    - Used builtin .NET Core  ILog abstraction and logic instead of ICommonLogging and NLog
 - UI
    - Replaced Gulp + Bower to Webpack + npm 
     
**Modules changes**:
- Changed module solution structure (Core project, Constants, Caching)
- Switched all DAL into asynchronous operations
- Export/Import is now streamed for all modules

**New modules**:
- `Notifications module` (written from scratch) key features:
    - Functionality which was spread across the system is shifted to dedicated module 
    - Manage notification availability for each store
    - Unlimited cannels types for sending notifications (Email, Sms, Social networks etc)
    - Possibility to activate/deactivate each notification individually for each store 
    - New flexible extendibility model 
    - Allows to preview a notification template with data
    - Support of LIQUID syntax for templates based on Scriban engine 
    - The new notification messaged feed allows to search and preview individual messages 
- `Tax module` key features:
    - The tax calculation functionality which was spread across the system is shifted to a dedicated module which is now responsible for tax settings and calculation 
    - The new module is a single integration point for third party software and custom extensions 
- `Shipping module` key features:
    -  The shipping costs calculation functionality which was spread across the system is shifted to a dedicated module which is now responsible for shipping methods, related settings and shipping costs calculation
    - The new module is a single integration point for third party software and custom extensions 
- `Payment module` key features:
    - The payment methods functionality and integrations which were spread across the system are shifted to a dedicated module which is now responsible for payment methods and related settings 
    - The new module is a single integration point for payment gateways integration
- `Search module` key features:
    - The new module is a single integration point for search engines integration and provides a generic UI and program components for indexed search
    
**Removed modules**: 
-  ~~**VirtoCommerce.Domain**~~ project and nuget package (now each module defines self domain model and abstractions in Core project)
-  ~~**VirtoCommerce.Cache**~~
-  ~~**VirtoCommerce.DynamicExpressions**~~

**Next steps**:
- Implement cache synchronization logic between multiple platform instances use `Redis` cache for this purposes 
- Resource based authorization (scope bounded permissions)
- Move module projects to GitHub repositiories
- Remaining modules
    - ElasticSearch
    - AzureSearch
    
# Getting started:

## Precompiled binary getting started
- Download archive with platform precompiled version [VirtoCommerce.Platform.3.0.0.beta.zip](https://github.com/VirtoCommerce/vc-platform-core/releases/tag/v3.0.0.beta)
- Unpack follow zip to local disk to path `C:\vc-platform-3`. In result you should get the folder which contains platform precompiled code. 
- Run platform by command `dotnet.exe C:\vc-platform-3\VirtoCommerce.Platform.Web.dll`
- Open in your browser follow url `https://localhost:5001` in the warning for not private connections that appears click advanced and continue work.
- On the first request the application will create and initialize database. After that you should see the sign in page. Use the following credentials: `admin/store`

## Source code  getting started
   - Get the latest source code from `master` branch
   - Make a symlink for Modules folder 
   ```cmd 
   mklink /d C:\vc-platform-3\VirtoCommerce.Platform.Web\Modules C:\vc-platform-3\Modules
   ```
   - Open and compile platform solution `VirtoCommerce.Platform.sln` in any preffered IDE
   - Install Node.js dependencies. 
   ```
   npm install
   ```
   - build and pack js scripts and css for platform 
   ```
   npm run webpack:dev
   ```
   - repeat previous command for each module in `Modules` folder
   
## Run [storefront](https://github.com/VirtoCommerce/vc-storefront-core) with new platform version
- Deploy  the latest storefront version from `dev` branch by any of preffered way described there https://virtocommerce.com/docs/vc2devguide/deployment/storefront-deployment
- Make changes  in  `appsettings.json`    
```json
...
//Comment the follow settings
// "AppId": "...",
// "SecretKey": "..."
...
//Uncomment the follow settings
"UserName": "admin",
"Password": "store"
```

## Run platform with Http in development mode

### Using CLI in development mode
- Make shure that you have installed latest version of dot.net core SDK 2.2
- To run platform navigate to `VirtoCommerce.Platform.Web` folder via CLI and run `dotnet run -c Development --no-launch-profile`. You can add `--no-build` flag to speed up start if you already compile solution.
- Browse to `https://localhost:5001`

## Run platform with Https support

### Using VisualStudio
Open `VirtoCommerce.Platform.Web` project properties and in `Debug` section set checkbox - `Enable SSL`. VisualStudio can ask you to install Certificate, just accept and platform will be running with Https support.

### Using CLI with Https
- Make shure that you have installed latest version of dot.net core SDK 2.2
- Install SSL certificate with CLI command `dotnet dev-certs https --trust`
- Restart your machine
- To run platform navigate to `VirtoCommerce.Platform.Web` folder via CLI and run `dotnet run -c Release --no-launch-profile`. You can add `--no-build` flag to speed up start if you already compile solution.
- Browse to `https://localhost:5001`


# How to debug module
- Install and run platform as described in steps above.
- Navigate into platform app directory and find `Modules` folder
- Clone a module from GitHub into `Modules` folder, if module already installed as binary just replace it folder
- Open and build  a module solution in Visual Studio
- Run platform by using CLI `dotnet run -c Development --no-build --no-restore` as described in steps above
- Attach debugger into `dotnet` process


# How to migrate your solution from 2.x to 3.0 platform version
- If your solution doesn't have any custom modules and extensions you just need to use the connection string to the old database for the new 3.0 platfrom version and after first run the update scripts will transfer all your data to the new scheme otherwise, you need to convert your models according to this instruction https://github.com/VirtoCommerce/vc-platform/blob/release/3.0.0/docs/Migrate-Extension-module-from-the-Platform-2.0-to-3.0-version.md.

# License
Copyright (c) Virto Solutions LTD.  All rights reserved.

Licensed under the Virto Commerce Open Software License (the "License"); you
may not use this file except in compliance with the License. You may
obtain a copy of the License at

http://virtocommerce.com/opensourcelicense

Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or
implied.
