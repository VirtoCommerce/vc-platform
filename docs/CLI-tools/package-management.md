The `vc-build` provides the set of targets that allow you to easily install, uninstall, and update platform dependencies by simple CLI commands execution.

## Install

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

### Examples:
```console
vc-build install 
```

- `vc-build install -platform -version <version>`

Fetch and install the platform with the specific version. If the platform with specified version does not exist in the registry, then this will fail.
If no version is specified, the latest platform version will be installed.

### Examples:
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

### Examples:
```console
vc-build install -module VirtoCommerce.Cart
vc-build install -module VirtoCommerce.Cart -version 3.12.0
vc-build install -module VirtoCommerce.Cart:3.12.0 VirtoCommerce.Core:3.20.0
```

## Update

```console
vc-build update (with no args)
vc-build update -platform -version <version>
vc-build update -module <module> -version <version>
```
This command will update the platform and all modules listed to the version specified by `<version>`, respecting semver.
If `<version>` is not specified the component will updated to the latest version.
If no args are specified, the platform and all modules in the specified location will be updated.

This command also updated the installed dependencies versions in the `vc-package.json` 

### Examples:
```console
vc-build update
vc-build update -platform
vc-build update -platform -version 3.14.0
vc-build update -module VirtoCommerce.Cart
vc-build update -module VirtoCommerce.Cart -version 3.30.0
```

## Uninstall
```console
vc-build uninstall -module <module>
```
This uninstalls a module and completely removes all modules that depend on it.
It also removes uninstalled modules from your `vc-package.json`.

### Examples:
```console
vc-build uninstall -module VirtoCommerce.Cart
```