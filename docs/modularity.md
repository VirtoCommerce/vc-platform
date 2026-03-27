# Virto Commerce Platform Modularity Architecture

This document describes the platform's module loading architecture, the static module system classes, the `IPlatformStartup` extension point, and deployment scenarios for Docker and CI/CD pipelines.

## Overview

The platform uses a **static, DI-free module loading pipeline** that runs in `Program.Main()` before the ASP.NET Core host is built. This design enables modules to participate in the earliest startup phases&mdash;including adding configuration sources and host-level services&mdash;before `Startup.ConfigureServices()` executes.

### Design Principles

- **No DI during discovery and loading.** Module manifests are read, assemblies are copied and loaded using plain static methods. This eliminates the legacy `services.BuildServiceProvider()` anti-pattern.
- **Separation of concerns.** Each phase (discovery, copying, loading, initialization) is handled by a dedicated static class with a single responsibility.
- **Graceful degradation.** A module that fails to load does not block platform startup. Errors are accumulated in `ManifestModuleInfo.Errors` and reported after startup completes.
- **Backward compatibility.** The `LocalModuleCatalogAdapter` bridges the static system to DI-dependent code that resolves `ILocalModuleCatalog` or `IModuleCatalog`.

## Startup Flow

```
Program.Main()
 |
 |  1. Build bootstrap IConfiguration (appsettings.json + env vars)
 |  2. ModuleLogger.Initialize()                          -- set up logging for static classes
 |  3. ModuleRunner.Initialize()                          -- store boost options for dependency sorting
 |  4. ModuleRunner.SortModulesByDependency(              -- scan manifests + sort by deps
 |       ModuleManifestReader.ReadAll())
 |  5. ModuleCopier.Initialize()                          -- set options + file copy policy
 |     ModuleCopier.Copy()                                -- rebuild probing if .rebuild marker,
 |                                                           then copy DLLs to probing path
 |  6. ModuleLoader.Initialize()                          -- register native library resolver
 |     ModuleLoader.LoadModules()                         -- load all module assemblies + deps
 |  7. ModuleDiscovery.ValidateModules()                  -- platform version + dependency validation
 |  8. ModuleRegistry.Initialize()                        -- populate global module index
 |  9. PlatformStartupDiscovery.DiscoverStartups()        -- find IPlatformStartup types
 |
 +--Host.CreateDefaultBuilder(args)
     |
     +--ConfigureAppConfiguration
     |   RunConfigureAppConfiguration()      -- modules add config sources
     |
     +--ConfigureServices (host-level)
     |   RunConfigureHostServices()          -- modules register host services
     |   AddHangfireServer()                 -- Hangfire (platform, kept for now)
     |
     +--Startup.ConfigureServices()
     |   RunConfigureServices()              -- IPlatformStartup app-level services
     |   ModuleRunner.InitializeModules()    -- IModule.Initialize() for each module
     |   mvcBuilder.AddApplicationPart()     -- register API controllers
     |   Register ILocalModuleCatalog in DI  -- backward-compat adapter
     |
     +--Startup.Configure()
         RunConfigure()                      -- IPlatformStartup middleware (before routing)
         UseRouting / UseAuth / ...
         ExecuteSynchronized:
           Platform migrations
           UseHangfire
           ModuleRunner.PostInitializeModules() -- IModule.PostInitialize()
         UseEndpoints / Swagger
```

## Sequence Diagrams

### Platform Startup — Full Module Loading Pipeline

This diagram shows the complete startup sequence from `Program.Main()` through `Startup.Configure()`, including every static module class invocation and when `IPlatformStartup` and `IModule` lifecycle methods execute.

```mermaid
sequenceDiagram
    autonumber
    participant P as Program.Main
    participant ML as ModuleLogger
    participant MR as ModuleManifestReader
    participant MC as ModuleCopier
    participant MRun as ModuleRunner
    participant MLd as ModuleLoader
    participant MD as ModuleDiscovery
    participant MReg as ModuleRegistry
    participant PSD as PlatformStartupDiscovery
    participant Host as Host Builder
    participant S as Startup

    rect rgb(240, 248, 255)
    Note over P: Phase 1 — LoadModules() (before DI)
    P->>ML: Initialize(serilogFactory)
    P->>MRun: Initialize(boostOptions)

    P->>MR: ReadAll(discoveryPath)
    MR-->>P: List of ManifestModuleInfo
    P->>MRun: SortModulesByDependency(modules)
    MRun-->>P: Sorted modules

    P->>MC: Initialize(options, metadataProvider)
    P->>MC: Copy(modules, arch)
    Note right of MC: Checks .rebuild marker → clears probing if found<br/>Then copies DLLs if RefreshProbingFolderOnStart or probing missing

    P->>MLd: Initialize(isDevelopment)
    P->>MLd: LoadModules(modules, probingPath)
    Note right of MLd: Loads all module assemblies + deps

    P->>MD: ValidateModules(modules, platformVersion)
    Note right of MD: Checks versions, deps,<br/>cascades errors to dependents

    P->>MReg: Initialize(modules)
    P->>PSD: DiscoverStartups(modules)
    Note right of PSD: Finds IPlatformStartup types<br/>via manifest startupType
    end

    rect rgb(255, 248, 240)
    Note over P,Host: Phase 2 — Host Building
    P->>Host: CreateHostBuilder(args)

    Host->>PSD: RunConfigureAppConfiguration()
    Note right of PSD: IPlatformStartup.ConfigureAppConfiguration()

    Host->>PSD: RunConfigureHostServices()
    Note right of PSD: IPlatformStartup.ConfigureHostServices()
    end

    rect rgb(240, 255, 240)
    Note over S: Phase 3 — Startup.ConfigureServices()
    S->>PSD: RunConfigureServices()
    Note right of PSD: IPlatformStartup.ConfigureServices()

    S->>MRun: InitializeModules(modules, services, config, env, catalog)
    loop Each module in dependency order
        MRun->>MRun: CreateModuleInstance(moduleInfo)
        Note right of MRun: IModule.Initialize(services)<br/>registers DI services
    end

    Note over S: Register API controllers + backward-compat DI
    end

    rect rgb(255, 240, 255)
    Note over S: Phase 4 — Startup.Configure()
    S->>PSD: RunConfigure()
    Note right of PSD: IPlatformStartup.Configure(app, config)<br/>adds early middleware

    Note over S: UseRouting, UseAuth, static files...

    rect rgb(255, 255, 224)
    Note over S: ExecuteSynchronized (distributed lock)
    Note over S: Platform migrations, Hangfire, permissions
    S->>MRun: PostInitializeModules(sortedModules, app)
    loop Each module in dependency order
        Note right of MRun: IModule.PostInitialize(appBuilder)
    end
    end

    Note over S: UseEndpoints, Swagger, health checks
    end
```

### IPlatformStartup vs IModule — Execution Order

This diagram focuses on the relative ordering of `IPlatformStartup` and `IModule` lifecycle methods. `IPlatformStartup` methods always execute first at each stage.

```mermaid
sequenceDiagram
    autonumber
    participant PS as IPlatformStartup
    participant IM as IModule

    rect rgb(240, 248, 255)
    Note over PS,IM: Program.Main — Before Host Build
    Note over PS: ConfigureAppConfiguration()<br/>Add config sources (Azure, Consul)
    Note over PS: ConfigureHostServices()<br/>Register host-level services
    end

    rect rgb(240, 255, 240)
    Note over PS,IM: Startup.ConfigureServices()
    PS->>PS: ConfigureServices(services, config)
    Note right of PS: Runs BEFORE IModule.Initialize

    IM->>IM: Initialize(serviceCollection)
    Note right of IM: Called via ModuleRunner.InitializeModules()<br/>in dependency order (no-deps first)
    end

    rect rgb(255, 240, 255)
    Note over PS,IM: Startup.Configure()
    PS->>PS: Configure(app, config)
    Note right of PS: Adds early middleware<br/>before UseRouting

    Note over PS,IM: ... routing, auth, static files ...

    Note over PS,IM: ExecuteSynchronized (distributed lock)
    Note over PS,IM: Platform migrations, Hangfire

    IM->>IM: PostInitialize(appBuilder)
    Note right of IM: Called via ModuleRunner.PostInitializeModules()<br/>in dependency order
    end

    rect rgb(255, 248, 240)
    Note over PS,IM: Runtime — Module Uninstall
    IM->>IM: Uninstall()
    Note right of IM: Called by ModuleManagementService<br/>before deleting module directory
    end
```

### Runtime Module Install / Uninstall

This diagram shows the runtime flow when a module is installed or uninstalled via the Admin UI, including the `.rebuild` marker mechanism that triggers a probing folder rebuild on next startup. The Admin UI sends lightweight `ModuleInstallRequest[]` payloads (id + optional version) to the `/install/v2`, `/update/v2`, and `/uninstall/v2` endpoints. Legacy endpoints accepting `ModuleDescriptor[]` at `/install`, `/update`, `/uninstall` are preserved for backward compatibility with external consumers.

```mermaid
sequenceDiagram
    autonumber
    participant UI as Admin UI
    participant API as ModulesController
    participant HF as Hangfire
    participant MMS as ModuleManagementService
    participant MPI as ModulePackageInstaller
    participant MC as ModuleCopier
    participant P as Program.Main (next startup)

    rect rgb(240, 255, 240)
    Note over UI,MC: Install / Update Module
    UI->>API: POST /install/v2<br/>[{id, version?}, ...]
    Note right of API: ModuleInstallRequest[] payload
    API->>HF: Enqueue(ModuleBackgroundJob)
    API-->>UI: ModulePushNotification
    HF->>MMS: InstallModules(requests, progress)
    MMS->>MMS: Validate (platform version, deps)

    loop Each module
        MMS->>MPI: Download(packageUrl, zipPath, httpClient)
        MMS->>MPI: Install(zipPath, targetDir)
        Note right of MPI: Extract ZIP to discovery path
    end

    MMS->>MC: InvalidateProbingFolder()
    Note right of MC: Writes .rebuild marker file<br/>(DLLs are locked by running process)
    end

    rect rgb(255, 240, 240)
    Note over UI,MC: Uninstall Module
    UI->>API: POST /uninstall/v2<br/>[{id}, ...]
    API->>HF: Enqueue(ModuleBackgroundJob)
    API-->>UI: ModulePushNotification
    HF->>MMS: UninstallModules(moduleIds, progress)
    MMS->>MMS: Validate (no dependents)

    loop Each module
        MMS->>MMS: module.Uninstall()
        Note right of MMS: IModule.Uninstall() cleanup
        MMS->>MPI: Uninstall(moduleDir)
        Note right of MPI: Delete discovery directory
    end

    MMS->>MC: InvalidateProbingFolder()
    Note right of MC: Writes .rebuild marker file
    end

    rect rgb(255, 248, 240)
    Note over P: Next Startup — Probing Rebuild
    P->>MC: Copy(modules, arch)
    Note right of MC: .rebuild marker found →<br/>clear probing folder contents,<br/>then rebuild from scratch with<br/>correct DLL versions
    end
```

## Static Module Classes

All classes are in the `VirtoCommerce.Platform.Modules` namespace.

### ModuleLogger

Static logger factory for the module loading pipeline. Must be initialized before any other module class is used. Falls back to `NullLoggerFactory` if not initialized.

| Method | Description |
|--------|-------------|
| `Initialize(loggerFactory)` | Set the `ILoggerFactory`. Call once from `Program.Main` before module loading. |
| `CreateLogger(type)` | Create a logger for the given type. Used internally by all static module classes. |
| `Reset()` | Reset to `NullLoggerFactory` (for unit tests). |

### ModuleManifestReader

Scans a directory tree for `module.manifest` XML files and returns a list of `ManifestModuleInfo` objects. Pure filesystem reads with no side effects.

| Method | Description |
|--------|-------------|
| `ReadAll(discoveryPath)` | Recursively finds all `module.manifest` files, excluding `artifacts/` subdirectories. |
| `Read(manifestFilePath)` | Reads a single manifest. Returns `null` on error (logged to console). |

Modules without an `<assemblyFile>` element (manifest-only modules) are immediately set to `ModuleState.Initialized`.

### ModuleCopier

Copies module assemblies from discovery directories to the probing path. Handles version comparison, CPU architecture filtering, and file-locking conflicts. Delegates filtering decisions to an `IFileCopyPolicy`.

| Method | Description |
|--------|-------------|
| `Initialize(options, metadataProvider)` | Set the `LocalStorageModuleCatalogOptions` and `IFileMetadataProvider`. Must be called before `Copy`. Stores discovery/probing paths and creates the internal `FileCopyPolicy`. |
| `Copy(modules, environmentArchitecture)` | Main entry point. Checks for the `.rebuild` marker and clears probing folder contents if found. Then, if `RefreshProbingFolderOnStart` is enabled or the probing folder is missing, copies each module's `bin/` folder contents to the probing path. The `environmentArchitecture` parameter (`Architecture` enum) controls which native binaries to select. |
| `InvalidateProbingFolder()` | Write a `.rebuild` marker file inside the probing folder. Called at runtime after install/uninstall when loaded assemblies are locked by the running process. On next startup, `Copy` will detect this marker. |
| `CopyModule(sourceDirectoryPath, targetDirectoryPath, environmentArchitecture)` | Copies a single module's binaries with smart filtering. |
| `GetTargetRelativePath(sourceRelativeFilePath)` | Map a source-relative path to its target-relative path in the probing folder. Returns `null` if the file should be skipped (TPA, reference assemblies). |
| `IsCopyRequired(sourceFilePath, targetFilePath, environmentArchitecture, out result)` | Check whether a file should be copied based on version, architecture, and date comparison. |

**Copy rules:**

- Skips Trusted Platform Assemblies (TPA) already provided by the .NET runtime.
- Skips reference assemblies (`ref/` folders) and design-time assemblies.
- Preserves `runtimes/` directory structure for native libraries.
- Preserves language subdirectory structure for `*.resources.dll`.
- Flattens all other assemblies (`.dll`, `.exe`, `.pdb`, `.deps.json`, etc.) into the probing root.
- Compares source and target by **version**, **CPU architecture**, and **file date** before copying. A file is only overwritten when the source is newer or has a better architecture match.

### ModuleLoader

Loads module assemblies and their dependencies into the default `AssemblyLoadContext`.

| Method | Description |
|--------|-------------|
| `Initialize(isDevelopmentEnvironment)` | Call once before loading any modules. Registers the native library resolver on `AssemblyLoadContext.Default`. |
| `LoadModules(modules, probingPath)` | Loads all module assemblies by iterating the list and calling `LoadModule` for each. |
| `LoadModule(module, probingPath)` | Loads a single module's main assembly and all dependencies declared in its `.deps.json` file. Sets `module.Assembly` and `module.State` on success, or appends to `module.Errors` on failure. |

**Dependency resolution order:**

1. Read the module's `.deps.json` for the full dependency graph.
2. For each dependency, probe in the module's `bin/` directory, then in additional probing paths from `.runtimeconfig.json`.
3. Native libraries are tracked in a concurrent dictionary and resolved via the `ResolvingUnmanagedDll` callback.
4. Assemblies already present in the default load context (TPA) are reused, not reloaded.

An internal cache prevents loading the same assembly twice when multiple modules share a dependency.

### ModuleRunner

Creates `IModule` instances via reflection and calls `Initialize` / `PostInitialize` in dependency order. Boost options for dependency sorting are stored as a static property via `Initialize()`, called once from `Program.Main`.

| Method | Description |
|--------|-------------|
| `Initialize(boostOptions)` | Store `ModuleSequenceBoostOptions` for dependency sorting. Called once from `Program.Main` before any sort or initialization. |
| `SortModulesByDependency(modules)` | Topological sort using `ModuleDependencySolver`. Handles duplicate module entries from merged catalogs via `GroupBy` + dedup. Optional dependencies and dependencies on modules not in the list are excluded from the graph. |
| `InitializeModules(modules, services, config?, env?, catalog?)` | For each module (sorted): creates instance, sets `IHasConfiguration`, `IHasHostEnvironment`, `IHasModuleCatalog` properties, then calls `IModule.Initialize(services)`. Skips modules with errors. |
| `PostInitializeModules(modules, appBuilder)` | Calls `IModule.PostInitialize(app)` on every initialized module. |

`CreateModuleInstance(moduleInfo)` is internal. It finds the `IModule` implementation in the loaded assembly. If multiple candidates exist, matches by `ModuleType` from the manifest.

Errors are captured in `moduleInfo.Errors`; the method does not throw.

### ModuleRegistry

Thread-safe global registry populated once and queried from any code path without DI. Used by controllers, health checks, tag helpers, Swagger, and static file serving instead of `ILocalModuleCatalog`.

| Method | Description |
|--------|-------------|
| `Initialize(modules)` | Stores the module list and builds a case-insensitive dictionary index. Logs module and error counts. |
| `GetAllModules()` | Returns all modules (installed + failed). |
| `GetInstalledModules()` | Modules with `IsInstalled = true` and no errors. |
| `GetFailedModules()` | Modules with errors. |
| `IsInstalled(moduleId)` | O(1) lookup. |
| `IsInstalled(moduleId, minVersion)` | Version-aware check. |
| `GetModule(moduleId)` | Returns `ManifestModuleInfo` or `null`. |
| `Reset()` | Clears the registry (for unit tests). |

### ModuleDiscovery

Static logic for external module manifest parsing, version merging, and validation. No HTTP — works on already-downloaded data.

| Method | Description |
|--------|-------------|
| `ParseExternalManifest(json, platformVersion, includePrerelease?)` | Parse external module manifest JSON into a list of `ManifestModuleInfo`. Selects the latest compatible stable (and optionally prerelease) version per module. |
| `MergeWithInstalled(externalModules, installedModules)` | Merge external modules with locally installed modules. Installed modules keep their state; external modules show as available for install/update. |
| `ValidateModules(modules, platformVersion)` | Validate all loaded modules at startup. Checks platform version, dependency versions, and incompatibilities. Populates `ManifestModuleInfo.Errors`. Cascades errors to dependents (if module A fails, all modules depending on A also fail). Optional dependencies do not cascade. |
| `ValidateInstall(module, installedModules, platformVersion)` | Validate that a module can be installed. Returns list of error messages (empty if valid). Checks platform version, incompatibilities, and major version changes. |
| `ValidateUninstall(moduleId, installedModules, excludeModuleIds?)` | Validate that a module can be uninstalled. Returns errors if other installed modules depend on it. `excludeModuleIds` allows batch uninstall (their dependencies are ignored). |
| `GetDependencies(selectedModules, allAvailableModules)` | Returns the given modules plus all their transitive dependencies (prerequisites), sorted in dependency order. Walks DOWN the dependency graph. For each dependency, prefers installed version, then latest compatible. |

**Validation cascade:** When `ValidateModules` finds a failed module (e.g., incompatible platform version), it propagates errors to all modules that depend on it. This prevents cryptic DI errors at runtime — the error is surfaced at startup.

### ModulePackageInstaller

Static operations for module installation, uninstallation, downloading, and external manifest loading. No DI.

| Method | Description |
|--------|-------------|
| `Install(zipPath, targetModulePath)` | Extract a module ZIP file to the target directory. |
| `Uninstall(modulePath)` | Delete a module directory. If deletion fails due to locked files (e.g., `FileSystemWatcher`), logs a warning and continues. |
| `Download(packageUrl, targetZipPath, httpClient, options?)` | Download a module package from a URL. Supports authorization headers via `ExternalModuleCatalogOptions`. |
| `LoadExternalModules(httpClient, options)` | Download and parse external module manifests from all configured URLs (main + extra). Returns deduplicated module list. |
| `LoadModulesManifest(httpClient, options, manifestUrl)` | Download and parse a single external module manifest URL. |

### PlatformStartupDiscovery

Discovers `IPlatformStartup` implementations from loaded module assemblies and orchestrates their lifecycle methods.

| Method | Description |
|--------|-------------|
| `DiscoverStartups(modules)` | For each module with a `StartupType` and a loaded assembly, resolves the type, validates it implements `IPlatformStartup`, creates an instance, and stores it. |
| `GetStartups()` | Returns previously discovered startups. |
| `RunConfigureAppConfiguration(startups, builder, env)` | Calls `ConfigureAppConfiguration` on each startup. |
| `RunConfigureHostServices(startups, services, config)` | Calls `ConfigureHostServices` on each startup. |
| `RunConfigureServices(startups, services, config)` | Calls `ConfigureServices` on each startup. |
| `RunConfigure(startups, app, config)` | Calls `Configure` on each startup. |
| `Reset()` | Clears state (for unit tests). |

### LocalModuleCatalogAdapter

A thin adapter that extends `ModuleCatalog` and implements `ILocalModuleCatalog`. It wraps the pre-loaded module list so that DI-dependent code in external modules resolving `ILocalModuleCatalog` or `IModuleCatalog` continues to work unchanged. Platform code itself uses `ModuleRegistry` directly.

```csharp
[Obsolete("Use ModuleRegistry instead.")]
public class LocalModuleCatalogAdapter : ModuleCatalog, ILocalModuleCatalog
{
    public LocalModuleCatalogAdapter(IEnumerable<ManifestModuleInfo> modules, ModuleSequenceBoostOptions boostOptions = null)
        : base(modules, Options.Create(boostOptions ?? new ModuleSequenceBoostOptions())) { }

    protected override void InnerLoad() { /* no-op */ }
    public override void AddModule(ModuleInfo moduleInfo) => throw new NotSupportedException();
    public override void Reload() => throw new NotSupportedException();
}
```

## Module Management Service

`IModuleManagementService` is a DI-registered singleton for module catalog management from the Admin UI. It merges external (from manifest URLs) and locally installed modules, caches the result, and orchestrates install/uninstall operations.

Registered in DI via `services.AddSingleton<IModuleManagementService, ModuleManagementService>()`.

### ModuleInstallRequest

A lightweight DTO for install/update/uninstall requests. Replaces the heavy `ModuleDescriptor` (~20 fields) with only the two fields needed by the service layer.

```csharp
public class ModuleInstallRequest
{
    public string Id { get; set; }
    public string Version { get; set; } // optional: null means "latest available"
}
```

### IModuleManagementService

```csharp
public interface IModuleManagementService
{
    IList<ManifestModuleInfo> GetModules();
    void ReloadModules();
    IList<ManifestModuleInfo> GetDependencies(IList<string> moduleIds);
    IList<ManifestModuleInfo> GetDependents(IList<string> moduleIds);
    ManifestModuleInfo AddUploadedModule(ManifestModuleInfo module);
    void InstallModules(IList<ModuleInstallRequest> requests, IProgress<ProgressMessage> progress);
    void UninstallModules(IList<string> moduleIds, IProgress<ProgressMessage> progress);
    IList<ManifestModuleInfo> GetNotInstalledModulesFromGroups(IList<string> groups);
    Task<bool> ValidateModuleVersionAsync(string moduleId, string version);
    Task<ManifestModuleInfo> RegisterCustomModuleVersionAsync(string moduleId, string version);
}
```

| Method | Description |
|--------|-------------|
| `GetModules()` | Returns merged list of external + installed modules. Lazy-loaded on first access via `ModulePackageInstaller.LoadExternalModules()` + `ModuleDiscovery.MergeWithInstalled()`. |
| `ReloadModules()` | Clears cached modules and re-fetches from external manifest URLs. |
| `GetDependencies(moduleIds)` | Accepts module IDs. Resolves to `ManifestModuleInfo` internally (installed version preferred, then latest). Returns selected modules + all transitive prerequisites, sorted in dependency order. Walks DOWN the graph. |
| `GetDependents(moduleIds)` | Accepts module IDs. Resolves to installed `ManifestModuleInfo` internally. Returns installed modules that depend ON the given modules (reverse dependencies). Walks UP the graph. |
| `AddUploadedModule(module)` | Add an uploaded module to the merged catalog. Validates dependencies before adding. |
| `InstallModules(modules, progress)` | Install or update modules. Accepts `IList<ModuleInstallRequest>` (id + optional version). Validates platform version and dependencies. Downloads ZIP, extracts to discovery path. Rollback on failure. |
| `UninstallModules(moduleIds, progress)` | Uninstall modules by ID. Validates no other modules depend on them. Calls `module.Uninstall()`, deletes directory. |
| `GetNotInstalledModulesFromGroups(groups)` | Get modules from requested groups, including their dependencies. Returns only modules not yet installed. |
| `ValidateModuleVersionAsync(moduleId, version)` | Validate that a specific module version package exists at the download URL. Returns `true` if the package is found. |
| `RegisterCustomModuleVersionAsync(moduleId, version)` | Validate, register, and prepare a custom module version for installation. Returns the registered `ManifestModuleInfo` if valid, or `null` if the package was not found. |

### How It Works

1. **First call to `GetModules()`**: Downloads external manifests, merges with `ModuleRegistry.GetAllModules()`, caches result.
2. **Install**: Validates → downloads ZIP → extracts via `ModulePackageInstaller.Install()` → updates `IsInstalled` flag → calls `ModuleCopier.InvalidateProbingFolder()` to write `.rebuild` marker. Rollback on failure.
3. **Uninstall**: Validates dependents → calls `IModule.Uninstall()` → deletes directory via `ModulePackageInstaller.Uninstall()` → calls `ModuleCopier.InvalidateProbingFolder()`. Rollback on failure.
4. **Next startup**: `ModuleCopier.Copy()` detects the `.rebuild` marker, clears all probing folder contents (preserving the directory itself for symlinks/ACLs), and forces a clean rebuild with correct DLL versions.

## Module Management REST API

The `ModulesController` exposes REST endpoints for module operations. Endpoints exist in two forms: legacy endpoints accepting `ModuleDescriptor[]` (for backward compatibility with external consumers) and new lightweight endpoints accepting `ModuleInstallRequest[]` (used by the Admin UI).

### Batch Endpoints

Legacy endpoints accept `ModuleDescriptor[]` for backward compatibility. The `/v2` variants accept lightweight `ModuleInstallRequest[]`.

| Method | Route | Request Body | Description |
|--------|-------|-------------|-------------|
| `POST` | `/api/platform/modules/install` | `ModuleDescriptor[]` | Install modules (legacy). |
| `POST` | `/api/platform/modules/install/v2` | `ModuleInstallRequest[]` | Install modules using lightweight requests. |
| `POST` | `/api/platform/modules/update` | `ModuleDescriptor[]` | Update modules (legacy). |
| `POST` | `/api/platform/modules/update/v2` | `ModuleInstallRequest[]` | Update modules using lightweight requests. |
| `POST` | `/api/platform/modules/uninstall` | `ModuleDescriptor[]` | Uninstall modules (legacy). |
| `POST` | `/api/platform/modules/uninstall/v2` | `ModuleInstallRequest[]` | Uninstall modules using lightweight requests. |

### Single-Module Endpoints

| Method | Route | Description |
|--------|-------|-------------|
| `POST` | `/api/platform/modules/{moduleId}/install` | Install latest available version of a module. |
| `POST` | `/api/platform/modules/{moduleId}/versions/{version}/install` | Install a specific version of a module. Validates the package URL first. Returns `404` if the version is not found. |
| `POST` | `/api/platform/modules/{moduleId}/uninstall` | Uninstall a module. |
| `GET`  | `/api/platform/modules/{moduleId}/versions/{version}/validate` | Validate that a specific module version package exists at the download URL. Returns `true`/`false`. |

### Other Endpoints

| Method | Route | Description |
|--------|-------|-------------|
| `GET`  | `/api/platform/modules` | List all modules (installed + available). |
| `POST` | `/api/platform/modules/reload` | Re-fetch external module manifests. |
| `POST` | `/api/platform/modules/getmissingdependencies` | Get transitive dependencies not yet installed. |
| `POST` | `/api/platform/modules/getdependents` | Get installed modules that depend on the given modules. |
| `POST` | `/api/platform/modules/localstorage` | Upload a module ZIP for installation. |
| `POST` | `/api/platform/modules/restart` | Restart the web application. |
| `POST` | `/api/platform/modules/autoinstall` | Trigger auto-install for configured module bundles. |
| `GET`  | `/api/platform/modules/autoinstall/state` | Get auto-install state (excluded from API explorer). |
| `GET`  | `/api/platform/modules/loading-order` | Get module loading order (dependency-sorted). |

## IPlatformStartup Interface

Allows modules to hook into platform startup phases that occur **before** the standard `IModule` lifecycle. Implementations are discovered via the `<startupType>` element in `module.manifest`.

```csharp
public interface IPlatformStartup
{
    void ConfigureAppConfiguration(IConfigurationBuilder builder, IHostEnvironment env);
    void ConfigureHostServices(IServiceCollection services, IConfiguration config);
    void ConfigureServices(IServiceCollection services, IConfiguration config);
    void Configure(IApplicationBuilder app, IConfiguration config);
}
```

### Lifecycle Methods

| Method | When It Runs | Use Case |
|--------|-------------|----------|
| `ConfigureAppConfiguration` | `Program.CreateHostBuilder()`, inside `ConfigureAppConfiguration` callback | Add configuration sources: Azure App Configuration, Consul, Vault |
| `ConfigureHostServices` | `Program.CreateHostBuilder()`, inside host-level `ConfigureServices` callback | Register hosted services, background job servers |
| `ConfigureServices` | `Startup.ConfigureServices()`, **before** `ModuleRunner.InitializeModules()` | Application-level DI registrations that need to run before modules |
| `Configure` | `Startup.Configure()`, at the very start before routing middleware | Add early middleware to the HTTP pipeline |

## Module Manifest

The `module.manifest` XML file declares a module's metadata, dependencies, and optional startup type.

```xml
<?xml version="1.0" encoding="utf-8"?>
<module>
  <id>VirtoCommerce.AzureAppConfiguration</id>
  <version>1.0.0</version>
  <platformVersion>3.800.0</platformVersion>

  <title>Azure App Configuration</title>
  <description>Provides Azure App Configuration integration as a module</description>
  <authors>
    <author>Virto Commerce</author>
  </authors>

  <assemblyFile>VirtoCommerce.AzureAppConfiguration.dll</assemblyFile>
  <moduleType>VirtoCommerce.AzureAppConfiguration.Module</moduleType>
  <startupType>VirtoCommerce.AzureAppConfiguration.AzureAppConfigStartup</startupType>

  <dependencies>
    <dependency id="VirtoCommerce.Core" version="3.800.0" />
  </dependencies>
</module>
```

**Key elements:**

| Element | Required | Description |
|---------|----------|-------------|
| `id` | Yes | Unique module identifier |
| `version` | Yes | Semantic version (may include `-tag`) |
| `platformVersion` | Yes | Minimum platform version required |
| `assemblyFile` | No | DLL filename. If omitted, the module is manifest-only (no code). |
| `moduleType` | No | Fully-qualified `IModule` implementation class name |
| `startupType` | No | Fully-qualified `IPlatformStartup` implementation class name |
| `dependencies` | No | Other modules this module depends on |

## Configuration

Module paths are configured in `appsettings.json` under the `VirtoCommerce` section:

```json
{
  "VirtoCommerce": {
    "DiscoveryPath": "modules",
    "ProbingPath": "app_data/modules",
    "RefreshProbingFolderOnStart": true
  }
}
```

| Setting | Default | Description |
|---------|---------|-------------|
| `DiscoveryPath` | `modules` | Directory where installed modules are stored (each in its own subdirectory with a `module.manifest` file) |
| `ProbingPath` | `app_data/modules` | Flat directory where all module assemblies are copied for loading. Created automatically if missing. |
| `RefreshProbingFolderOnStart` | `true` | When `true`, copies assemblies from discovery to probing at every startup. Set to `false` to skip the copy phase (requires a pre-populated probing folder). |

The CPU architecture for assembly copying is detected automatically from the running process via `RuntimeInformation.ProcessArchitecture`.

## Example: Implementing IPlatformStartup

This example shows a module that adds Azure App Configuration as a configuration source:

```csharp
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.AzureAppConfiguration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using VirtoCommerce.Platform.Core.Modularity;

namespace VirtoCommerce.AzureAppConfiguration;

public class AzureAppConfigStartup : IPlatformStartup
{
    public void ConfigureAppConfiguration(IConfigurationBuilder builder, IHostEnvironment env)
    {
        // Build current config to check for connection string
        var config = builder.Build();
        var connectionString = config.GetConnectionString("AzureAppConfigurationConnectionString");

        if (!string.IsNullOrWhiteSpace(connectionString))
        {
            builder.AddAzureAppConfiguration(options =>
            {
                options.Connect(connectionString)
                    .Select(KeyFilter.Any)
                    .Select(KeyFilter.Any, env.EnvironmentName)
                    .ConfigureRefresh(refresh =>
                    {
                        refresh.Register("Sentinel", refreshAll: true);
                    });
            });
        }
    }

    public void ConfigureServices(IServiceCollection services, IConfiguration config)
    {
        var connectionString = config.GetConnectionString("AzureAppConfigurationConnectionString");
        if (!string.IsNullOrWhiteSpace(connectionString))
        {
            services.AddAzureAppConfiguration();
        }
    }

    public void Configure(IApplicationBuilder app, IConfiguration config)
    {
        var connectionString = config.GetConnectionString("AzureAppConfigurationConnectionString");
        if (!string.IsNullOrWhiteSpace(connectionString))
        {
            app.UseAzureAppConfiguration();
        }
    }
}
```

## Deployment Scenarios

### Standard Development (RefreshProbingFolderOnStart = true)

The default mode. Each startup copies assemblies from `DiscoveryPath` to `ProbingPath`:

```
modules/
  VirtoCommerce.Catalog/
    module.manifest
    bin/
      VirtoCommerce.CatalogModule.dll
      VirtoCommerce.CatalogModule.deps.json
      ...
  VirtoCommerce.Orders/
    module.manifest
    bin/
      ...

app_data/modules/            <-- populated at startup
  VirtoCommerce.CatalogModule.dll
  VirtoCommerce.CatalogModule.deps.json
  VirtoCommerce.OrdersModule.dll
  ...
```

## Diagnostics

### Logging

All static module classes use `ModuleLogger` (backed by `ILoggerFactory`, typically Serilog). Log output includes structured properties:

```
[INF] ModuleManifestReader: Found 12 module manifests in /app/modules
[DBG] ModuleCopier: Copying assemblies from /app/modules/VirtoCommerce.Catalog/bin
[DBG] ModuleCopier: Updating VirtoCommerce.CatalogModule.dll: NewVersion=True
[WRN] ModuleCopier: File 'SomeLib.dll' was not updated (file in use by another process)
[DBG] ModuleLoader: Loaded VirtoCommerce.Catalog 3.800.0
[WRN] ModuleDiscovery: Module VirtoCommerce.Broken has errors: Module platform version 3.1111.0 is incompatible...
[WRN] ModuleDiscovery: 2 modules failed validation (including cascaded dependents)
[INF] ModuleRegistry: Loaded modules: 12, with errors: 2
[DBG] ModuleRunner: Initializing VirtoCommerce.Catalog 3.800.0
[DBG] ModuleRunner: Post-initializing VirtoCommerce.Catalog
```

### Failed Modules

After startup, failed modules are logged at Warning/Error level. Modules that fail validation also cascade errors to their dependents:

Query failed modules programmatically:

```csharp
var failed = ModuleRegistry.GetFailedModules();
```

### Health Checks

The `ModulesHealthChecker` reports module health at `/health`:

```json
{
  "Modules health": {
    "status": "Unhealthy",
    "description": "Module VirtoCommerce.Broken has errors"
  }
}
```
