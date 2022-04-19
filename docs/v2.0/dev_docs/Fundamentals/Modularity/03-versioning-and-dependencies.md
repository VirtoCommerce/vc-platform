# Module Versioning and Dependencies

Virto modules and platform use the [SemVer](https://semver.org/ "https://semver.org/") pattern. Short for Semantic Versioning, SemVer, in its latest incarnation (v2.0.0), describes the versioning scheme as **MAJOR.MINOR.PATCH**, where:

-  You increment MAJOR version when you make incompatible API changes.
    
-  You increment MINOR version when you add functionality in a backwards-compatible manner.
    
-  You increment PATCH version when you make backwards-compatible bug fixes.

Additional labels for pre-release and build metadata are available as extensions to the **MAJOR.MINOR.PATCH** format.

Along with the release version, you might also use pre-release versions by adding a prerelease tag. The resulting format of the pre-release version is as follows:

-   MAJOR.MINOR.PATCH-<alpha | beta | rc    

When it comes to module pre-release tags, we generally follow the recognized naming conventions:

- Alpha: Alpha release, typically used for work-in-progress and experiments
    
-  Beta: Beta release, typically one that is feature complete for the next planned release, but may contain some known bugs
    
-  RC: Release candidate, typically a release that is potentially final (stable), unless significant bugs emerge.

Each Virto module has a version both as a project assembly version and the one contained in the `module.manifest` file. Thus, to change the version, you will have to change both the assembly version manually in `Directory.Build.Props` (going forward, it will be automatically calculated from current Git branches and tags tanks in the [GitVersion](https://gitversion.readthedocs.io/en/latest/) utility) and the module version is the `module.manifest` file.

A module can depend on other modules and cannot function without the modules it depends on; along with this, each module has a dependency to a specific platform version.

All module and platform dependencies must be described in the `module.manifest` file, namely, in the `dependencies` section:

`module.manifest`

```json
1 <?xml version="1.0" encoding="utf-8"?>
2 <module>
3    <id>VirtoCommerce.Catalog</id>
4    <version>3.3.0</version>
5    <platformVersion>3.0.0</platformVersion>
6    <dependencies>
7        <dependency id="VirtoCommerce.Core" version="3.0.0" />
8        <dependency id="VirtoCommerce.BulkActionsModule" version="3.0.0" />
9    </dependencies>
10    ...
```

The Virto dependency version resolving logic always interprets all versions as **^major.minor.path** (Next Significant Release [Caret Version Range](https://getcomposer.org/doc/articles/versions.md) operator), and it will always allow non-breaking updates. This is the best explained by the following example:

> ^1.2.3 is equivalent to >=1.2.3 <2.0.0 as none of the releases until 2.0 should break backwards compatibility.

## Managing Third Party Dependencies for Modules

As all module assemblies are copied into the *probing* folder before being loaded into the platform application process, except the assemblies of the module in question, the platform module manager copies all third party dependency assemblies and applies the same version conflict resolution policy:

> The assembly with the highest version or the one that was modified last, wins.

This fact may lead to an unexpected update of third party dependencies for other modules and the platform application itself when resolving an assembly at runtime. You should always consider this fact when updating dependencies for your custom modules.
