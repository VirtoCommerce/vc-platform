# Manage tools

The official CLI [.NET Core GlobalTool](https://docs.microsoft.com/en-us/dotnet/core/tools/global-tools) for building, deploy releases, create and push packages, and manage environments for projects based on VirtoCommerce. Is powered by [nuke.build](https://nuke.build/) A cross-platform build automation system with C# DSL.

## Publish a new version

Incerment package version in _build.csproj

```console

dotnet pack .\vc-platform\build\_build.csproj  --configuration Release --include-symbols --output .\vc-platform\artifacts /property:SymbolPackageFormat=snupkg

```

```console

dotnet nuget push VirtoCommerce.GlobalTool.{version}.nupkg  -s https://api.nuget.org/v3/index.json -k {api key}

```

## Installation

Installing VirtoCommerce.GlobalTool package:

```console

dotnet tool install VirtoCommerce.GlobalTool  -g

```

## Latest version
Run this command to update VirtoCommerce.GlobalTool to the latest version:

```console

dotnet tool update VirtoCommerce.GlobalTool -g

```

## Usage
To use VirtoCommerce.GlobalTool by invoke the tool using the following command: `vc-build`

To get the all list of targets:
```console

vc-build help

```
Command output

```console
NUKE Execution Engine version 0.24.11 (Windows,.NETCoreApp,Version=v2.1)

Target with name 'help' does not exist. Available targets are:
  - ChangeVersion
  - Clean
  - Compile
  - CompleteHotfix
  - CompleteRelease
  - Compress
  - GetManifestGit
  - GrabMigrator
  - IncremenPatch
  - IncrementMinor
  - MassPullAndBuild
  - Pack
  - Publish
  - PublishManifestGit
  - PublishModuleManifest
  - PublishPackages
  - QuickRelease
  - Release
  - Restore
  - SonarQubeEnd
  - SonarQubeStart
  - StartAnalyzer
  - StartHotfix
  - StartRelease
  - SwaggerValidation
  - Test
  - UpdateManifest
  - ValidateSwaggerSchema
  - WebPackBuild

```

## Compress
The target is used to create a redistributed zip archive for a module or platform. After executing, the resulting zip is placed in `artifacts` folder.
To execute this target, you need to run this command in the root module folder of the cloned from GitHub repository.

```console

vc-build compress

```

console output

```console

═══════════════════════════════════════
Target             Status      Duration
───────────────────────────────────────
Clean              Executed        0:00
Restore            Executed        0:07
Compile            Executed        0:06
WebPackBuild       Executed        0:00
Test               Executed        0:05
Publish            Executed        0:01
Compress           Executed        0:01
───────────────────────────────────────
Total                              0:23
═══════════════════════════════════════

```

## StartRelease, CompleteRelease, QuickRelease, StartHotfix, CompleteHotfix
Used to automate the routine operations with release branches
#### StartRelease:
- creates and pushes the new branch release/*version* from dev
#### CompleteRelease:
- merges release/*version* into master and pushes
- merges into dev branch, increments version's minor and pushes
#### QuickRelease: 
- Triggers StartRelease and then CompleteRelease
#### StartHotfix:
- Increments version's patch in master
- Creates and pushes the new branch hotfix/*version*
#### CompleteHotfix:
- Merges hotfix branch into master
- Adds tag and pushes

## Packages management
#### Init
Creates vc-package.json boilerplate with the latest version number of the platform.
Version number can be specified by -PlatformVersion parameter
For example:
```console
vc-build Init
vc-build Init -PlatformVersion 3.52.0
```
#### Install
Gets the -Module parameter, which is an array of module ids, and updates vc-package.json.
Also module id can be  supplemented with the version number.
Discovery and Probing directories can be overridden via -DiscoveryPath and -ProbingPath
Examples:
```console
vc-build install -Module VirtoCommerce.Cart VirtoCommerce.Catalog
vc-build install -Module VirtoCommerce.Cart VirtoCommerce.Catalog:3.38.0
vc-build install -Module VirtoCommerce.Cart VirtoCommerce.Catalog -DiscoveryPath ../modules -ProbingPath platform/app_data/modules
```
#### InstallModules
Installs modules according to vc-package.json and resolves dependencies
```console
vc-build InstallModules
vc-build InstallModules -DiscoveryPath ../modules -ProbingPath platform/app_data/modules
```
#### InstallPlatform
Installs platform according to vc-package.json
```console
vc-build InstallPlatform
```
#### Uninstall
Gets -Module parameter and removes specified modules
```console
vc-build uninstall -Module VirtoCommerce.Cart VirtoCommerce.Catalog
```