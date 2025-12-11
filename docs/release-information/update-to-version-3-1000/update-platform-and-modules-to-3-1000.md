# Upgrading to Virto Commerce on .NET 10

## Overview

Virto Commerce on NET 10 (3.1000+) introduces a significant technical update by transitioning the platform to .NET 10.
This update focuses on enhancing performance and stability while maintaining backward compatibility.
It involves updating the Target Framework to .NET 10 and integrating the latest LTS releases of third-party libraries.
Importantly, no code refactoring or alterations to the API and internal structure have been made.

The release has undergone extensive testing, including unit, end-to-end, regression, and performance tests to ensure a seamless transition as well as for other stable releases.

## Explore What's New in .NET 10

Discover the exciting features and improvements introduced in .NET 10 to maximize the benefits of the upgrade. Refer to:

* [What's new in NET10](https://learn.microsoft.com/en-us/dotnet/core/whats-new/dotnet-10/overview)
* [What's new in EF 10 Core](https://learn.microsoft.com/en-us/ef/core/what-is-new/ef-core-10.0/whatsnew)
* [What's new in ASP.NET Core 10](https://learn.microsoft.com/en-us/aspnet/core/release-notes/aspnetcore-10.0)

## Benefits

### 1. Enhanced Performance

The compiler in .NET 10 includes significant enhancements that improve performance through better code generation and optimization strategies.

### 2. Improved Stability

Stability is a cornerstone of this update. By leveraging the advancements in .NET 10, Virto Commerce offers enhanced system reliability and robustness. This translates to fewer disruptions, improved error handling, and increased overall system stability.

## Virto Commerce Update Path

1. **Install .NET 10:** Begin by ensuring that you have .NET 10 installed on your system. Follow the official installation guidelines to set up the environment for the upgrade: https://dotnet.microsoft.com/en-us/download/dotnet/10.0
2. **Update Virto Commerce Platform to v3.1000 or later**
3. **Update Virto Commerce Modules to latest stable release 12 and later**. We recommend update to Stable 14.

> By default, Virto Commerce Platform on .NET10 are backward compatible with previous platform stable release on .NET8 (3.800+). However, it is recommended to update custom modules to the latest versions to leverage new features and improvements.

## Known Limitations and Breaking Changes

If you find any new breaking changes, submit an question on [Virto Commerce Community](https://www.virtocommerce.org/c/support/12).

### Exception is thrown when applying migrations if there are pending model changes

Starting with EF Core 9.0, if the model has pending changes compared to the last migration an exception is thrown when dotnet ef database update, Migrate or MigrateAsync is called:

The model for context 'DbContext' has pending changes. Add a new migration before updating the database. This exception can be suppressed or logged by passing event ID 'RelationalEventId.PendingModelChangesWarning' to the 'ConfigureWarnings' method in 'DbContext.OnConfiguring' or 'AddDbContext'.

Forgetting to add a new migration after making model changes is a common mistake that can be hard to diagnose in some cases. The new exception ensures that the app's model matches the database after the migrations are applied.

You can find more information about PendingModelChangesWarning warning by [following link](https://www.virtocommerce.org/t/pendingmodelchangeswarning-in-net-10-ef-core-10-what-happened-how-we-diagnosed-it/817). 

### Breaking changes in Microsoft.OpenApi

Virto Commerce updated Microsoft.OpenApi from version 1.0.0 to 2.3.0 that includes some breaking changes. You will need to update and rebuild your custom module if you use Microsoft.OpenApi.
You can find more information about Breaking changes in Microsoft.OpenApi by [following link](https://github.com/microsoft/OpenAPI.NET/blob/main/docs/upgrade-guide-2.md).

## Remove BuildHost-net472 and BuildHost-netcore

After update Microsoft.EntityFrameworkCore.Design to 10.x, you will see that your project includes two folders BuildHost-net472 and BuildHost-netcore under obj folder.
These folders are created by Microsoft.EntityFrameworkCore.Design package to support design-time services for different target frameworks. 

You can remove these two folders by modifying your .csproj file to include the following PackageReference for Microsoft.EntityFrameworkCore.Design with PrivateAssets set to all:

```xml
<PackageReference Include="Microsoft.EntityFrameworkCore.Design" Version="10.0.0">
  <PrivateAssets>all</PrivateAssets>
  <IncludeAssets>runtime; build; native; analyzers; buildtransitive</IncludeAssets>
</PackageReference>
```

## Update Virto Commerce Platform and Modules to v3.1000+

### Option 1. vc-build Update command

Utilize the vc-build Update command for an automated update. This method streamlines the update process, ensuring that all components are seamlessly transitioned to the new version.

```cmd
vc-build Update -Stable -v 14
```

### Option 2. Update via package.json

If you use package.json file for automated deployment, change versions of the platform and Virto Commerce modules to 3.1000.0+. Based on latest Stable 8 or Edge release strategy.

### Option 3. Manually update

Alternatively, manually download update the platform and modules to version 3.1000+. This method provides more control over the update process, allowing for a step-by-step transition.

## Update Custom Modules

### Prerequsites

1. Install Visual Studio 2026
2. Install Virto Commerce CLI (vc-build)
```cmd
dotnet tool install VirtoCommerce.GlobalTool  -g
```
or update
```cmd
dotnet tool update VirtoCommerce.GlobalTool -g
```

2. Install dotnet-ef to version 10.0+:
```cmd
dotnet tool install --global dotnet-ef --version 10.0.0
```
or update
```cmd
dotnet tool update --global dotnet-ef --version 10.0.0
```

## Update Custom Modules

If you develop a custom module, update can be required primary to update .NET dependencies.

### Update Solution to NET8

Download and execute the [vc-net8-update.ps1 Power Shell script](vc-net10-update.ps1) in your solution folder. 

!!! info "How to enable execution of PowerShell scripts"
    Set powershell script as trusted if required, by running this power shell command:
    ```ps1
      Set-ExecutionPolicy -Scope Process -ExecutionPolicy Bypass
    ```
  

```ps1
./vc-net10-update.ps1
```

![step1 run ps1 script](../../media/updatenet10-step1-run-ps1-script.png)

![step2 run ps1 script result](../../media/updatenetStable relea-step2-ps1-script-result.png)

This script automates several these tasks, including (of course you can do it manually):

1. Updating the Target Framework to .NET 10 for every project.
2. Updating project dependencies, including Microsoft NuGet dependencies to version 10.0.0 and VirtoCommerce NuGet dependency to version 3.1000.0 and latest.
3. Updating other third-party dependencies to save version that used by Virto Commerce Platform.
4. Updating the module.manifest file to align with the changes in .NET 10.

![step3 review modified files](../../media/updatenetStable relea-step3-modified-files.png)

### Build Solution

1. Build the solution and meticulously address any compilation errors and warnings if required. This step ensures that the solution is compatible with the updated framework.
2. Verify Tests for Issues Perform a thorough verification of tests to identify and address any issues introduced by the update. This step guarantees that the updated solution maintains the expected functionality and performance.

![step3 build solution](../../media/updatenetStable relea-step4-build.png)

### Create Module Package

Generate a module package by running vc-build Compress. This step finalizes the update process, creating a package that encapsulates the updated modules for deployment.

```cmd
vc-build Compress
```

## Run and Enjoy Virto Commerce on .NET 10

With the update process completed, you can now run and enjoy the enhanced capabilities of Virto Commerce on the .NET 10 platform.
Explore the platform's new features and optimizations to leverage its full potential for a resilient and efficient e-commerce solution.

