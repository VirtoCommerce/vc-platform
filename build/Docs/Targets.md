:::
## Init
Creates vc-package.json boilerplate with the latest version number of the platform.
Version number can be specified by PlatformVersion parameter
For example:
```console
vc-build Init
vc-build Init -PlatformVersion 3.52.0
```
:::
:::
## Install
This command downloads and install into the current folder the platform or modules with versions that are passed as the command parameters or defined in vc-package.json.

vc-package.json - file is used to maintain the list of installed modules with their versions. This allows vc-build to easily restore the platform with the modules when on a different machine, such as a build server, without all those packages.

vc-build install (with no args)
This target downloads and install into the current folder the platform and modules with versions described in vc-package.json. If vc-package.json is not found in the local folder, by default the command will download and install the latest platform and modules versions that are marked with the commerce group.

By default, install target will install all modules listed as dependencies in vc-package.json.
Path to vc-package.json, discovery and probing paths can be overridden with PackageManifestPath, DiscoveryPath, ProbingPath parameters. Also we can skip dependency solving with SkipDependencySolving parameter.
Examples:
```console
vc-build install (with no args)
vc-build install -platform -version <version>
vc-build install -module <module> -version <version>
vc-build install -module <module>:<version>
vc-build install -PackageManifestPath some_directory/vc-package.json -DiscoveryPath ../modules -ProbingPath platform_dir/app_data/modules -SkipDependencySolving
```
:::
:::
## Update
Updates platform and modules to last versions
```console
vc-build Update
```
:::
:::
## InstallModules
Installs modules according to vc-package.json and solves dependencies
```console
vc-build InstallModules
vc-build InstallModules -DiscoveryPath ../modules
```
:::
:::
## InstallPlatform
Installs platform according to vc-package.json
```console
vc-build InstallPlatform
```
:::
:::
## Uninstall
Gets -Module parameter and removes specified modules
```console
vc-build uninstall -Module VirtoCommerce.Cart VirtoCommerce.Catalog
```
:::
:::
## Clean
Cleans bin, obj and artifacts directories
```console
vc-build clean
```
:::
:::
## Restore
Executes dotnet restore
```console
vc-build restore
```
:::
:::
## Compile
Executes dotnet build
```console
vc-build compile
```
:::
:::
## Pack
Creates nuget packages by executing dotnet pack
```console
vc-build pack
```
:::
:::
## Test
Executes unit tests and generates coverage report
Can get -TestsFilter parameter to filter tests which should be run
```console
vc-build Test
vc-build Test -TestsFilter "Category!=IntegrationTest"
```
:::
:::
## PublishPackages
Publishes nuget packages in ./artifacts directory with -Source and -ApiKey parameters.
-Source is "https://api.nuget.org/v3/index.json" by default
```console
vc-build PublishPackages -ApiKey %SomeApiKey%
```
:::
:::
## QuickRelease
Creates a release branch from dev. Merges it into master. Increments version in dev branch and removes release/* branch.
```console
vc-build uninstall -Module VirtoCommerce.Cart VirtoCommerce.Catalog
```
:::
:::
## Publish
Executes dotnet publish
```console
vc-build publish
```
:::
:::
## WebPackBuild
Executes "npm ci" and then "npm run webpack:build"
```console
vc-build WebPackBuild
```
:::
:::
## Compress
Compresses an artifact to the archive and filters excess files
```console
vc-build compress
```
:::
:::
## PublishModuleManifest
Updates modules_v3.json with information from the current artifact's module.manifest
```console
vc-build PublishModuleManifest
```
:::
:::
## SonarQubeStart
Starts sonar scanner by executing "dotnet sonarscanner begin". 
Gets parameters: SonarBranchName, SonarPRBase, SonarPRBranch, SonarPRNumber, SonarGithubRepo, SonarPRProvider, SonarAuthToken
```console
vc-build SonarQubeStart -SonarBranchName dev -SonarAuthToken *** -RepoName vc-module-marketing
```
:::
:::
## SonarQubeEnd
Executes "dotnet sonarscanner end"
Gets parameters: SonarAuthToken
```console
vc-build SonarQubeEnd -SonarAuthToken %SonarToken%
```
:::
:::
## Release
Creates the github release
Gets parameters: GitHubUser, GitHubToken, ReleaseBranch
```console
vc-build release -GitHubUser VirtoCommerce -GitHubToken %token% 
```
:::
