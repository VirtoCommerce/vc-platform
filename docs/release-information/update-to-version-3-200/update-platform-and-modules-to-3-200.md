# Update platform and modules to .NET 6 version

## Introduction

This article describes how to update VC Platform manager and VC modules to .NET 6 (version since 3.2xx)

## Before the update
Please review the [.NET 6 release notes](https://github.com/dotnet/core/blob/main/release-notes/6.0/README.md).

### Breaking change note
Because of breaking changes introduced in the Entity Framework and for some other reasons, VC modules and platform are imcompatible with different versions of each other.
VC Platform's minor version **equal or above 3.2xx** can't load and manage VC modules version **below 3.2xx**. And the other way round.
For the described before reasons there is no chance for partial update. **You need to update to .NET 6 all your project.**

### Slow migration
Order module particularly contains a potentially long perform migration which could be in case your project have a lot of order's. Consider to apply it manually.

#### Manually migration example
```bash
cd your_project_path
dotnet ef database update --context "OrderDbContext|YourOwnContext" --connection "ConnectionString"
```

## Developer's experience
Make sure that [.NET 6 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/6.0) and [Visual studio 2022](https://visualstudio.microsoft.com/vs/) have been installed to your environment.

### Updating extended VC module dependencies
There is a vc-build tool which can be usefull to update dependencies in your module.
See the [details](https://github.com/VirtoCommerce/vc-build#update).

## List of releases
| # | Name | Link |
| --- | --- | --- |
| 1 | [VC Platform](https://github.com/VirtoCommerce/vc-platform) | [3.200]() |
| 2 | [VC Google ecommerce analitycs](https://github.com/VirtoCommerce/vc-module-google-ecommerce-analytics) | [3.200](https://github.com/VirtoCommerce/vc-module-google-ecommerce-analytics/releases/tag/3.200.0) |
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
| 28 | [VC File sistem assets](https://github.com/VirtoCommerce/vc-module-filesystem-assets) | [3.200](https://github.com/VirtoCommerce/vc-module-filesystem-assets/releases/tag/3.200.0) |
| 29 | [VC Azure blob assets](https://github.com/VirtoCommerce/vc-module-azureblob-assets) | [3.200](https://github.com/VirtoCommerce/vc-module-azureblob-assets/releases/tag/3.200.0) |
| 30 | [VC Marketing](https://github.com/VirtoCommerce/vc-module-marketing) | [3.200](https://github.com/VirtoCommerce/vc-module-marketing/releases/tag/3.200.0) |
| 31 | [VC Content](https://github.com/VirtoCommerce/vc-module-content) | [3.200](https://github.com/VirtoCommerce/vc-module-content/releases/tag/3.200.0) |
| 32 | [VC Tax](https://github.com/VirtoCommerce/vc-module-tax) | [3.200](https://github.com/VirtoCommerce/vc-module-tax/releases/tag/3.200.0) |
| 33 | [VC XApi](https://github.com/VirtoCommerce/vc-module-experience-api) | [3.200](https://github.com/VirtoCommerce/vc-module-experience-api/releases/tag/3.200.0) |
| 34 | [VC WebHooks](https://github.com/VirtoCommerce/vc-module-webhooks) | [3.200](https://github.com/VirtoCommerce/vc-module-webhooks/releases/tag/3.200.0) |
| 35 | [VC Subscription](https://github.com/VirtoCommerce/vc-module-subscription) | [3.200](https://github.com/VirtoCommerce/vc-module-subscription/releases/tag/3.200.0) |
| 36 | [VC Sitemaps](https://github.com/VirtoCommerce/vc-module-sitemaps) | [3.200](https://github.com/VirtoCommerce/vc-module-sitemaps/releases/tag/3.200.0) |
| 37 | [VC Bulk actions](https://github.com/VirtoCommerce/vc-module-bulk-actions) | [3.200](https://github.com/VirtoCommerce/vc-module-bulk-actions/releases/tag/3.200.0) |

## FAQ

### How can I migrate my extension module?
1. You need to install all required environment, like VS 2022 and .NET 6 SDK
2. Bump all dependent VirtoCommerce NuGet packages. (At least to 3.200 version)
3. Fix all issues and gain the solution compile successful.
4. Note, during developing and debugging you should not compile your module either with .NET Core 3.1 and .NET 6 at the same time.

