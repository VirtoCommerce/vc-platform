### VirtoCommerce.GlobalTool

The official CLI [.NET Core GlobalTool](https://docs.microsoft.com/en-us/dotnet/core/tools/global-tools) for building, deploy releases, create and push packages, and manage environments for projects based on VirtoCommerce. Is powered by [nuke.build](https://nuke.build/) A cross-platform build automation system with C# DSL.

### Installation

To use VirtoCommerce.GlobalTool  install the  package:

```console

dotnet tool install VirtoCommerce.GlobalTool  -g

```

**Note:** For the VirtoCommerce.GlobalTool may not have a stable release at the moment then system will show you this error

```console

error NU1103: Unable to find a stable package virtocommerce.globaltool with version
error NU1103:   - Found 1 version(s) in nuget.org [ Nearest version: 3.0.0-beta0004 ]

```
Then for install a prerelease version you need to run this command and pass in `--version` parameter a latest pre-release version

```console

dotnet tool install VirtoCommerce.GlobalTool --version 3.0.0-beta004  -g

```


### Update to latest version 
For update VirtoCommerce.GlobalTool to the latest version you need to run this command

```console

dotnet tool update VirtoCommerce.GlobalTool -g

```

### Usage
To use VirtoCommerce.GlobalTool by invoke the tool using the following command: `vc-build`

To get the all list of targets 
```console

vc-build help

```
Command output

```console
NUKE Execution Engine version 0.21.2 (Windows,.NETStandard,Version=v2.0)

Target with name 'help' does not exist. Available targets are:
  - Clean
  - Compile
  - Compress
  - MassPullAndBuild
  - Pack
  - Publish
  - PublishModuleManifest
  - PublishPackages
  - Release
  - Restore
  - SonarQubeEnd
  - SonarQubeStart
  - StartAnalyzer
  - SwaggerValidation
  - Test
  - WebPackBuild

```

## Compress
The target is used to create a redistributed zip archive for a module or platform. After executing, it  placed resulting zip in the `artifacts` folder.
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

