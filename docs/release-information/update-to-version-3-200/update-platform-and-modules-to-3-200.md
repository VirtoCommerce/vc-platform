# Updating Platform and Modules to .NET 6

## Introduction

This document will guide you through updating VC Platform Manager and VC modules (version 3.2xx or higher) to .NET 6.

## Prior to Update
Before you start running your update, please review [.NET 6 Release Notes](https://github.com/dotnet/core/blob/main/release-notes/6.0/README.md).

### Breaking Change Note
Because of some breaking changes introduced into the Entity Framework, as well as for some other reasons, VC modules are imcompatible with the Platform having a different version, and vice versa.
Technically, VC Platform **version 3.2xx or higher** cannot load and manage VC modules with any version **below 3.2xx**, and the other way round.
For the above reasons, there is no option for partial update, which means **you have to update your entire project to .NET 6.**

### Slow Performance
For the Order module, it may take particularly long to migrate in case your project has a lot of orders. For this reason, you might want to do the migration manually.

#### How to Manually Migrate Your Project
This is an example of how you can migrate your project manually:
```bash
cd your_project_path
dotnet ef database update --context "OrderDbContext|YourOwnContext" --connection "ConnectionString"
```

## Developer Experience
Make sure both [.NET 6 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/6.0) and [Visual studio 2022](https://visualstudio.microsoft.com/vs/) have been installed to your environment.

### Updating Extended VC Module Dependencies
The vc-build tool may come in especially handy when updating the dependencies within your module.
You can find the details [here](https://github.com/VirtoCommerce/vc-build#update).

## List of Releases
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

## FAQs

### How can I migrate my extension module?
1. Make sure all required prerequisites, such as VS 2022 and .NET 6 SDK, have been installed to your environment.
2. Bump all dependent VirtoCommerce NuGet packages to at least version 3.200.
3. Fix all issues and make sure the solution gets compiled successfully.

*Please note: When the developing and debugging processes are running, do not compile your module with .NET Core 3.1 and .NET 6 at the same time.*

