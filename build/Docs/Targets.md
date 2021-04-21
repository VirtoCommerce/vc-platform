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
Gets -Modules parameter and removes specified modules
```console
vc-build uninstall -Module VirtoCommerce.Cart VirtoCommerce.Catalog
```
:::
