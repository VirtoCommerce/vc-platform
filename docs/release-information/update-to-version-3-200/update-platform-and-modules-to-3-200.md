# Updating Platform and Modules to .NET 6

## Introduction

This document will guide you through updating VC Platform Manager and VC modules to version 3.200 or higher based on .NET 6.

## Prior to Update
Before you start running your update, please review [.NET 6 Release Notes](https://github.com/dotnet/core/blob/main/release-notes/6.0/README.md).

### Breaking Change Note
As Microsoft introduced some breaking changes into Entity Framework, as well as for some other reasons, VC modules are imcompatible with the Platform having a different version, and vice versa.
Technically, VC Platform **version 3.200 or higher** cannot load and manage VC modules with any version **below 3.200**, and the other way round.
For the above reasons, there is no option for partial update, which means **you have to update your entire project to .NET 6.**

## Developer Experience
Make sure both [.NET 6 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/6.0) and [Visual Studio 2022](https://visualstudio.microsoft.com/vs/) have been installed to your environment.

## VC Build Tool
After update to Virto Commerce on .NET 6, please use this vc-build version: [virtocommerce.globaltool **2.1.0-alpha.17**](https://virtocommerce.com/docs/CLI-tools/introduction/).

```console
dotnet tool install --global VirtoCommerce.GlobalTool --version 2.1.0-alpha.17
```


## List of Releases
| # | Name | Link |
| --- | --- | --- |
| 1 | [VC Platform](https://github.com/VirtoCommerce/vc-platform) | [3.200](https://github.com/VirtoCommerce/vc-platform/releases/tag/3.200.0) |
| 2 | [VC Google ecommerce analytics](https://github.com/VirtoCommerce/vc-module-google-ecommerce-analytics) | [3.200](https://github.com/VirtoCommerce/vc-module-google-ecommerce-analytics/releases/tag/3.200.0) |
| 3 | [VC Assets](https://github.com/VirtoCommerce/vc-module-assets) | [3.200](https://github.com/VirtoCommerce/vc-module-assets/releases/tag/3.200.0) |
| 4 | [VC Notifications](https://github.com/VirtoCommerce/vc-module-notification) | [3.200](https://github.com/VirtoCommerce/vc-module-notification/releases/tag/3.200.0) |
| 5 | [VC Core](https://github.com/VirtoCommerce/vc-module-core) | [3.200](https://github.com/VirtoCommerce/vc-module-core/releases/tag/3.200.0) |
| 6 | [VC Export](https://github.com/VirtoCommerce/vc-module-export) | [3.200](https://github.com/VirtoCommerce/vc-module-export/releases/tag/3.200.0) |
| 7 | [VC Search](https://github.com/VirtoCommerce/vc-module-search) | [3.200](https://github.com/VirtoCommerce/vc-module-search/releases/tag/3.200.0) |
| 8 | [VC Store](https://github.com/VirtoCommerce/vc-module-store) | [3.200](https://github.com/VirtoCommerce/vc-module-store/releases/tag/3.200.0) |
| 9 | [VC XApi profile](https://github.com/VirtoCommerce/vc-module-profile-experience-api) | [3.200](https://github.com/VirtoCommerce/vc-module-profile-experience-api/releases/tag/3.200.0) |
| 10 | [VC Authorize.Net](https://github.com/VirtoCommerce/vc-module-Authorize.Net) | [3.200](https://github.com/VirtoCommerce/vc-module-Authorize.Net/releases/tag/3.200.0) |
| 11 | [VC Avalara](https://github.com/VirtoCommerce/vc-module-avatax) | [3.200](https://github.com/VirtoCommerce/vc-module-avatax/releases/tag/3.200.0) |
| 12 | [VC Azure search](https://github.com/VirtoCommerce/vc-module-azure-search) | [3.200](https://github.com/VirtoCommerce/vc-module-azure-search/releases/tag/3.200.0) |
| 13 | [VC Catalog CSV import](https://github.com/VirtoCommerce/vc-module-catalog-csv-import) | [3.200](https://github.com/VirtoCommerce/vc-module-catalog-csv-import/releases/tag/3.200.0) |
| 14 | [VC Catalog personalization](https://github.com/VirtoCommerce/vc-module-catalog-personalization) | [3.200](https://github.com/VirtoCommerce/vc-module-catalog-personalization/releases/tag/3.200.0) |
| 15 | [VC Catalog publishing](https://github.com/VirtoCommerce/vc-module-catalog-publishing) | [3.200](https://github.com/VirtoCommerce/vc-module-catalog-publishing/releases/tag/3.200.0) |
| 16 | [VC Dynamic associations](https://github.com/VirtoCommerce/vc-module-dynamic-associations) | [3.200](https://github.com/VirtoCommerce/vc-module-dynamic-associations/releases/tag/3.200.0) |
| 17 | [VC Elastic search](https://github.com/VirtoCommerce/vc-module-elastic-search) | [3.200](https://github.com/VirtoCommerce/vc-module-elastic-search/releases/tag/3.200.0) |
| 18 | [VC Image tools](https://github.com/VirtoCommerce/vc-module-image-tools) | [3.200](https://github.com/VirtoCommerce/vc-module-image-tools/releases/tag/3.200.0) |
| 19 | [VC Lucene search](https://github.com/VirtoCommerce/vc-module-lucene-search) | [3.200](https://github.com/VirtoCommerce/vc-module-lucene-search/releases/tag/3.200.0) |
| 20 | [VC Catalog](https://github.com/VirtoCommerce/vc-module-catalog) | [3.200](https://github.com/VirtoCommerce/vc-module-catalog/releases/tag/3.200.0) |
| 21 | [VC Pricing](https://github.com/VirtoCommerce/vc-module-pricing) | [3.200](https://github.com/VirtoCommerce/vc-module-pricing/releases/tag/3.200.0) |
| 22 | [VC Shipping](https://github.com/VirtoCommerce/vc-module-shipping) | [3.200](https://github.com/VirtoCommerce/vc-module-shipping/releases/tag/3.200.0) |
| 23 | [VC Payment](https://github.com/VirtoCommerce/vc-module-payment) | [3.200](https://github.com/VirtoCommerce/vc-module-payment/releases/tag/3.200.0) |
| 24 | [VC Cart](https://github.com/VirtoCommerce/vc-module-cart) | [3.200](https://github.com/VirtoCommerce/vc-module-cart/releases/tag/3.200.0) |
| 25 | [VC Inventory](https://github.com/VirtoCommerce/vc-module-inventory) | [3.200](https://github.com/VirtoCommerce/vc-module-inventory/releases/tag/3.200.0) |
| 26 | [VC Customer](https://github.com/VirtoCommerce/vc-module-customer) | [3.200](https://github.com/VirtoCommerce/vc-module-customer/releases/tag/3.200.0) |
| 27 | [VC Orders](https://github.com/VirtoCommerce/vc-module-order) | [3.200](https://github.com/VirtoCommerce/vc-module-order/releases/tag/3.200.0) |
| 28 | [VC File system assets](https://github.com/VirtoCommerce/vc-module-filesystem-assets) | [3.200](https://github.com/VirtoCommerce/vc-module-filesystem-assets/releases/tag/3.200.0) |
| 29 | [VC Azure blob assets](https://github.com/VirtoCommerce/vc-module-azureblob-assets) | [3.200](https://github.com/VirtoCommerce/vc-module-azureblob-assets/releases/tag/3.200.0) |
| 30 | [VC Marketing](https://github.com/VirtoCommerce/vc-module-marketing) | [3.200](https://github.com/VirtoCommerce/vc-module-marketing/releases/tag/3.200.0) |
| 31 | [VC Content](https://github.com/VirtoCommerce/vc-module-content) | [3.200](https://github.com/VirtoCommerce/vc-module-content/releases/tag/3.200.0) |
| 32 | [VC Tax](https://github.com/VirtoCommerce/vc-module-tax) | [3.200](https://github.com/VirtoCommerce/vc-module-tax/releases/tag/3.200.0) |
| 33 | [VC XApi](https://github.com/VirtoCommerce/vc-module-experience-api) | [3.200](https://github.com/VirtoCommerce/vc-module-experience-api/releases/tag/3.200.0) |
| 34 | [VC WebHooks](https://github.com/VirtoCommerce/vc-module-webhooks) | [3.200](https://github.com/VirtoCommerce/vc-module-webhooks/releases/tag/3.200.0) |
| 35 | [VC Subscription](https://github.com/VirtoCommerce/vc-module-subscription) | [3.200](https://github.com/VirtoCommerce/vc-module-subscription/releases/tag/3.200.0) |
| 36 | [VC Sitemaps](https://github.com/VirtoCommerce/vc-module-sitemaps) | [3.200](https://github.com/VirtoCommerce/vc-module-sitemaps/releases/tag/3.200.0) |
| 37 | [VC Bulk actions](https://github.com/VirtoCommerce/vc-module-bulk-actions) | [3.200](https://github.com/VirtoCommerce/vc-module-bulk-actions/releases/tag/3.200.0) |
| 38 | [VC Quote](https://github.com/VirtoCommerce/vc-module-quote) | [3.200](https://github.com/VirtoCommerce/vc-module-quote/releases/tag/3.200.0) |
| 39 | [VC Customer review](https://github.com/VirtoCommerce/vc-module-customer-review) | [3.200](https://github.com/VirtoCommerce/vc-module-customer-review/releases/tag/3.200.0) |

## FAQs

### How can I migrate my custom/extension module?
1. Make sure all required prerequisites, such as VS 2022 and .NET 6 SDK, have been installed to your environment.
2. Open project properties and change Target Framework to .NET 6.0.
    ```xml
    <TargetFramework>net6.0</TargetFramework>
    ```
3. Update All Microsoft and Entity Framework dependencies to 6.0.0.
4. Bump all dependent VirtoCommerce NuGet packages to at least version 3.200.
5. Fix all issues and make sure the solution gets compiled and load successfully.

*Please note: When the developing and debugging processes are running, do not compile your module with .NET Core 3.1 and .NET 6 at the same time.*

### Slow performing database migrations
In some cases, DB migration might take long time and throw an timeout exceptions.
You can deal with it by using [vc-build tool](https://github.com/VirtoCommerce/vc-build) and following the tips described in [this guide](https://github.com/VirtoCommerce/vc-build/blob/main/src/VirtoCommerce.Build/GrabMigrator/samples/readme.md) or [here](https://docs.microsoft.com/en-us/ef/core/cli/dotnet#dotnet-ef-database-update) or even [here](https://docs.microsoft.com/en-us/ef/core/cli/dotnet#dotnet-ef-migrations-script).
