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
| 9 |  |  |
| 10 |  |  |
| 11 |  |  |
| 12 |  |  |
| 13 |  |  |
| 14 |  |  |
| 15 |  |  |
| 16 |  |  |
| 17 |  |  |
| 18 |  |  |
| 19 |  |  |
| 20 |  |  |
| 21 |  |  |
| 22 |  |  |
| 23 |  |  |
| 24 |  |  |
| 25 |  |  |
| 26 |  |  |
| 27 |  |  |
| 28 |  |  |
| 29 |  |  |
| 30 |  |  |
| 31 |  |  |
| 32 |  |  |
| 33 |  |  |
| 34 |  |  |
| 35 |  |  |
| 36 |  |  |
| 37 |  |  |

## FAQ

### How can I migrate my extension module?
1. You need to install all required environment, like VS 2022 and .NET 6 SDK
2. Bump all dependent VirtoCommerce NuGet packages. (At least to 3.200 version)
3. Fix all issues and gain the solution compile successful.
4. Note, during developing and debugging you should not compile your module either with .NET Core 3.1 and .NET 6 at the same time.

