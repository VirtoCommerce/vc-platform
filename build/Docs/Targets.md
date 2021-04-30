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
Gets the -Module parameter, which is an array of module ids, and updates vc-package.json.
Also module id can be  supplemented with the version number.
Discovery and Probing directories can be overrided via -DiscoveryPath and -ProbingPath
Examples:
```console
vc-build install -Module VirtoCommerce.Cart VirtoCommerce.Catalog
vc-build install -Module VirtoCommerce.Cart VirtoCommerce.Catalog:3.38.0
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
Starts sonar scanner by executing "dotnet sonarscanner begin"
Gets parameters: SonarBranchName, SonarPRBase, SonarPRBranch, SonarPRNumber, SonarGithubRepo, SonarPRProvider, SonarAuthToken
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
