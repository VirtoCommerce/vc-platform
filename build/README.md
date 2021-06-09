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
The `vc-build` provides the set of targets that allow you to easily install, uninstall, and update platform dependencies by simple CLI commands execution.

#### Install

```console
vc-build install (with no args)
vc-build install -platform -version <version>
vc-build install -module <module> -version <version>
vc-build install -module <module>:<version>
```

This command downloads and install into the current folder the platform or modules with versions that are passed as the command parameters or defined in `vc-package.json`. 

`vc-package.json` - file is used to maintain the list of installed modules with their versions. This allows `vc-build` to easily restore the platform with the modules when on a different machine, such as a build server, without all those packages.


- `vc-build install (with no args)`

This target downloads and install into the current folder the platform and modules with versions described in `vc-package.json`. 
If `vc-package.json` is not found in the local folder, by default the command will download and install the latest platform and modules versions that are marked with the `commerce` group.

By default, `install` target will install all modules listed as dependencies in `vc-package.json`.

Examples:
```console
vc-build install 
```

- `vc-build install -platform -version <version>`

Fetch and install the platform with the specific version. If the platform with specified version does not exist in the registry, then this will fail.
If no version is specified, the latest platform version will be installed.

Examples:
```console
vc-build install -platform
vc-build install -platform -version 3.55.0
```

- `vc-build install -module -version <version>`

Install the specified version of the module. This will fail if the version has not been published to the registry.
If no version is specified, the latest module version will be installed.
You can also install multiple modules with a single command by specifying multiple modules with their versions as arguments.

If the module to be installed has dependencies, their latest versions will be installed along with it.

This command also modified the `vc-package.json` with the installed dependencies after successful command execution.

Examples:
```console
vc-build install -module VirtoCommerce.Cart
vc-build install -module VirtoCommerce.Cart -version 3.12.0
vc-build install -module VirtoCommerce.Cart:3.12.0 VirtoCommerce.Core:3.20.0
```

#### Update

```console
vc-build update (with no args)
vc-build update -platform -version <version>
vc-build update -module <module> -version <version>
```
This command will update the platform and all modules listed to the version specified by `<version>`, respecting semver.
If `<version>` is not specified the component will updated to the latest version.
If no args are specified, the platform and all modules in the specified location will be updated.

This command also updated the installed dependencies versions in the `vc-package.json` 

Examples:
```console
vc-build update
vc-build update -platform
vc-build update -platform -version 3.14.0
vc-build update -module VirtoCommerce.Cart
vc-build update -module VirtoCommerce.Cart -version 3.30.0
```

#### Uninstall
```console
vc-build uninstall -module <module>
```
This uninstalls a module and completely removes all modules that depend on it.
It also removes uninstalled modules from your `vc-package.json`.

Examples:
```console
vc-build uninstall -module VirtoCommerce.Cart
```
