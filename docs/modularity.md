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
 |  2. ModuleManifestReader.ReadAll()        -- scan module.manifest files
 |  3. ModuleCopier.CopyAll()                -- copy DLLs to probing path (if enabled)
 |  4. ModuleAssemblyLoader.Initialize()     -- register native library resolver
 |     ModuleAssemblyLoader.LoadModule()     -- load each module assembly + deps
 |  5. ModuleRegistry.Initialize()           -- populate global module index
 |  6. PlatformStartupDiscovery.DiscoverStartups()  -- find IPlatformStartup types
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
     |   ModuleRunner.InitializeAll()        -- IModule.Initialize() for each module
     |   RunConfigureServices()              -- IPlatformStartup app-level services
     |   mvcBuilder.AddApplicationPart()     -- register API controllers
     |   Register ILocalModuleCatalog in DI  -- backward-compat adapter
     |
     +--Startup.Configure()
         RunConfigure(EarlyMiddleware)       -- before routing (config refresh, etc.)
         UseRouting / UseAuth / ...
         ExecuteSynchronized:
           Platform migrations
           UseHangfire
           RunConfigure(Initialization)      -- inside distributed lock
           ModuleRunner.PostInitializeAll()  -- IModule.PostInitialize()
         UseEndpoints
         RunConfigure(LateMiddleware)        -- after endpoints (Swagger-like)
```

## Static Module Classes

All classes are in the `VirtoCommerce.Platform.Modules` namespace.

### ModuleManifestReader

Scans a directory tree for `module.manifest` XML files and returns a list of `ManifestModuleInfo` objects. Pure filesystem reads with no side effects.

| Method | Description |
|--------|-------------|
| `ReadAll(discoveryPath, probingPath?)` | Recursively finds all `module.manifest` files, excluding `artifacts/` subdirectories. When `probingPath` is provided, sets each module's `Ref` to a `file://` URI pointing to the assembly in the probing folder. |
| `Read(manifestFilePath, probingPath?)` | Reads a single manifest. Returns `null` on error (logged to console). |

Modules without an `<assemblyFile>` element (manifest-only modules) are immediately set to `ModuleState.Initialized`.

### ModuleCopier

Copies module assemblies from discovery directories to the probing path. Handles version comparison, CPU architecture filtering, and file-locking conflicts.

| Method | Description |
|--------|-------------|
| `CopyAll(discoveryPath, probingPath, modules, targetArchitecture?)` | Copies platform binaries from `discoveryPath/bin`, then each module's `bin/` folder contents. When `targetArchitecture` is `null`, auto-detects from the running process. Pass an explicit value for cross-compilation. |
| `CopyModule(modulePath, probingPath, targetArchitecture?)` | Copies a single module's binaries with smart filtering. Same architecture override semantics as `CopyAll`. |

**Copy rules:**

- Skips Trusted Platform Assemblies (TPA) already provided by the .NET runtime.
- Skips reference assemblies (`ref/` folders) and design-time assemblies.
- Preserves `runtimes/` directory structure for native libraries.
- Preserves language subdirectory structure for `*.resources.dll`.
- Flattens all other assemblies (`.dll`, `.exe`, `.pdb`, `.deps.json`, etc.) into the probing root.
- Compares source and target by **version**, **CPU architecture**, and **file date** before copying. A file is only overwritten when the source is newer or has a better architecture match.

### ModuleAssemblyLoader

Loads module assemblies and their dependencies into the default `AssemblyLoadContext`.

| Method | Description |
|--------|-------------|
| `Initialize(isDevelopment)` | Call once before loading any modules. Registers the native library resolver on `AssemblyLoadContext.Default`. |
| `LoadModule(module, probingPath)` | Loads the module's main assembly and all dependencies declared in its `.deps.json` file. Sets `module.Assembly` and `module.State` on success, or appends to `module.Errors` on failure. |

**Dependency resolution order:**

1. Read the module's `.deps.json` for the full dependency graph.
2. For each dependency, probe in the module's `bin/` directory, then in additional probing paths from `.runtimeconfig.json`.
3. Native libraries are tracked in a concurrent dictionary and resolved via the `ResolvingUnmanagedDll` callback.
4. Assemblies already present in the default load context (TPA) are reused, not reloaded.

An internal cache prevents loading the same assembly twice when multiple modules share a dependency.

### ModuleRunner

Creates `IModule` instances via reflection and calls `Initialize` / `PostInitialize` in dependency order.

| Method | Description |
|--------|-------------|
| `SortByDependency(modules)` | Topological sort using `ModuleDependencySolver`. Optional dependencies are excluded from the graph. |
| `CreateModuleInstance(moduleInfo)` | Finds the `IModule` implementation in the loaded assembly. If multiple candidates exist, matches by `ModuleType` from the manifest. |
| `InitializeAll(modules, services, config?, env?, catalog?)` | For each module (sorted): creates instance, sets `IHasConfiguration`, `IHasHostEnvironment`, `IHasModuleCatalog` properties, then calls `IModule.Initialize(services)`. |
| `PostInitializeAll(modules, appBuilder)` | Calls `IModule.PostInitialize(app)` on every initialized module. |

Errors are captured in `moduleInfo.Errors`; the method does not throw.

### ModuleRegistry

Thread-safe global registry populated once and queried from any code path without DI.

| Method | Description |
|--------|-------------|
| `Initialize(modules)` | Stores the module list and builds a case-insensitive dictionary index. |
| `GetAllModules()` | Returns all modules. |
| `GetInstalledModules()` | Modules with no errors. |
| `GetFailedModules()` | Modules with errors. |
| `IsInstalled(moduleId)` | O(1) lookup. |
| `IsInstalled(moduleId, minVersion)` | Version-aware check. |
| `GetModule(moduleId)` | Returns `ManifestModuleInfo` or `null`. |
| `Reset()` | Clears the registry (for unit tests). |

### PlatformStartupDiscovery

Discovers `IPlatformStartup` implementations from loaded module assemblies and orchestrates their lifecycle methods.

| Method | Description |
|--------|-------------|
| `DiscoverStartups(modules)` | For each module with a `StartupType` and a loaded assembly, resolves the type, validates it implements `IPlatformStartup`, creates an instance, and stores it. Results are sorted by `Priority` (ascending). |
| `GetStartups()` | Returns previously discovered startups. |
| `RunConfigureAppConfiguration(startups, builder, env)` | Calls `ConfigureAppConfiguration` on each startup. |
| `RunConfigureHostServices(startups, services, config)` | Calls `ConfigureHostServices` on each startup. |
| `RunConfigureServices(startups, services, config)` | Calls `ConfigureServices` on each startup. |
| `RunConfigure(startups, phase, app, config)` | Calls `Configure` on startups whose `Phase` matches. |
| `Reset()` | Clears state (for unit tests). |

### LocalModuleCatalogAdapter

A thin adapter that extends `ModuleCatalog` and implements `ILocalModuleCatalog`. It wraps the pre-loaded module list so that DI-dependent code (health checks, tag helpers, static file serving, Swagger) continues to work unchanged.

```csharp
public class LocalModuleCatalogAdapter : ModuleCatalog, ILocalModuleCatalog
{
    public LocalModuleCatalogAdapter(IEnumerable<ManifestModuleInfo> modules)
        : base(modules.Cast<ModuleInfo>(), Options.Create(new ModuleSequenceBoostOptions())) { }

    protected override void InnerLoad() { /* no-op */ }
}
```

## IPlatformStartup Interface

Allows modules to hook into platform startup phases that occur **before** the standard `IModule` lifecycle.

```csharp
public interface IPlatformStartup
{
    int Priority => StartupPriority.Default;       // lower runs first
    PipelinePhase Phase => PipelinePhase.Initialization;

    void ConfigureAppConfiguration(IConfigurationBuilder builder, IHostEnvironment env) { }
    void ConfigureHostServices(IServiceCollection services, IConfiguration config) { }
    void ConfigureServices(IServiceCollection services, IConfiguration config) { }
    void Configure(IApplicationBuilder app, IConfiguration config) { }
}
```

### Lifecycle Methods

| Method | When It Runs | Use Case |
|--------|-------------|----------|
| `ConfigureAppConfiguration` | `Program.CreateHostBuilder()`, inside `ConfigureAppConfiguration` callback | Add configuration sources: Azure App Configuration, Consul, Vault |
| `ConfigureHostServices` | `Program.CreateHostBuilder()`, inside host-level `ConfigureServices` callback (runs **after** `Startup.ConfigureServices`) | Register hosted services, background job servers |
| `ConfigureServices` | `Startup.ConfigureServices()`, after `ModuleRunner.InitializeAll()` | Application-level DI registrations that depend on loaded modules |
| `Configure` | `Startup.Configure()`, at the position determined by `Phase` | Add middleware to the HTTP pipeline |

### Pipeline Phases

The `Phase` property controls when `Configure()` is called:

| Phase | Value | Position in Pipeline | Example Use |
|-------|-------|---------------------|-------------|
| `EarlyMiddleware` | 0 | After `UseHttpsRedirection`, before `UseRouting` | Configuration refresh middleware, request preprocessing |
| `Initialization` | 1 | Inside `ExecuteSynchronized` block, after platform migrations, before `IModule.PostInitialize` | Infrastructure that needs database access (Hangfire migrations, custom schema setup) |
| `LateMiddleware` | 2 | After `UseEndpoints` and module post-initialization | Middleware that depends on mapped endpoints (Swagger customization) |

### Priority Constants

`StartupPriority` defines well-known ordering values:

| Constant | Value | Intended Use |
|----------|-------|-------------|
| `ConfigurationSource` | -1000 | Configuration providers (load first) |
| `Infrastructure` | -500 | Logging, caching, telemetry |
| `Default` | 0 | General-purpose module startups |
| `Late` | 500 | Services depending on other modules |

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
    "RefreshProbingFolderOnStart": true,
    "TargetArchitecture": null
  }
}
```

| Setting | Default | Description |
|---------|---------|-------------|
| `DiscoveryPath` | `modules` | Directory where installed modules are stored (each in its own subdirectory with a `module.manifest` file) |
| `ProbingPath` | `app_data/modules` | Flat directory where all module assemblies are copied for loading. Created automatically if missing. |
| `RefreshProbingFolderOnStart` | `true` | When `true`, copies assemblies from discovery to probing at every startup. Set to `false` to skip the copy phase (requires a pre-populated probing folder). |
| `TargetArchitecture` | auto-detect | Target CPU architecture for assembly copying. Values: `X86`, `X64`, `Arm`, `Arm64`. When omitted, detected from the running process via `RuntimeInformation.ProcessArchitecture`. Set explicitly for cross-compilation (e.g., preparing an ARM64 probing folder on an X64 build machine). |

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
    // Load configuration sources before anything else
    public int Priority => StartupPriority.ConfigurationSource;

    // Use EarlyMiddleware to add config refresh middleware
    public PipelinePhase Phase => PipelinePhase.EarlyMiddleware;

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

### Docker Image Build (RefreshProbingFolderOnStart = false)

In containerized deployments the probing folder should be pre-populated at image build time. This avoids the copy overhead at every container start and prevents write operations on read-only filesystems.

**Dockerfile pattern:**

```dockerfile
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copy and build platform
COPY src/ src/
RUN dotnet publish src/VirtoCommerce.Platform.Web/VirtoCommerce.Platform.Web.csproj \
    -c Release -o /app/publish

# Install modules using vc-build or direct copy
FROM build AS modules
WORKDIR /modules

# Option A: Use vc-build CLI to install modules
RUN dotnet tool install -g VirtoCommerce.GlobalTool && \
    vc-build install -modules VirtoCommerce.Catalog VirtoCommerce.Orders \
    -DiscoveryPath /modules/discovery

# Option B: Copy pre-built module packages
# COPY modules/ /modules/discovery/

# Pre-populate probing folder at build time
# This flattens all module DLLs into a single directory
RUN mkdir -p /modules/probing && \
    for dir in /modules/discovery/*/bin; do \
      cp -n "$dir"/*.dll "$dir"/*.deps.json "$dir"/*.pdb /modules/probing/ 2>/dev/null || true; \
    done

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
COPY --from=modules /modules/discovery ./modules
COPY --from=modules /modules/probing ./app_data/modules

# Disable copy phase since probing is pre-populated
ENV VirtoCommerce__RefreshProbingFolderOnStart=false
```

**Key points:**

1. The probing folder (`app_data/modules`) is populated during the Docker build, not at runtime.
2. `RefreshProbingFolderOnStart=false` tells the platform to skip `ModuleCopier.CopyAll()`.
3. If the probing folder does not exist at startup, the platform forces a refresh regardless of the setting.
4. Module manifests must still be present under `DiscoveryPath` (the platform reads metadata from them).

### Cross-Architecture Docker Build (TargetArchitecture)

When building a Docker image for a different CPU architecture than the build machine (e.g., building an ARM64 image on an X64 CI server), use `TargetArchitecture` to tell `ModuleCopier` which native binaries to select:

```dockerfile
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
# ... build platform and install modules as above ...

# Pre-populate probing folder for ARM64 target, running on X64 build machine
FROM build AS probing
WORKDIR /app
COPY --from=build /app/publish .
COPY --from=modules /modules/discovery ./modules

# Run the platform's copier with architecture override
ENV VirtoCommerce__TargetArchitecture=Arm64
ENV VirtoCommerce__RefreshProbingFolderOnStart=true
RUN dotnet VirtoCommerce.Platform.Web.dll --copy-modules-only 2>/dev/null || true
# OR use vc-build with the architecture flag:
# RUN vc-build copy-modules -DiscoveryPath ./modules -ProbingPath ./app_data/modules -TargetArchitecture Arm64

FROM mcr.microsoft.com/dotnet/aspnet:10.0-arm64v8 AS final
WORKDIR /app
COPY --from=probing /app .
ENV VirtoCommerce__RefreshProbingFolderOnStart=false
```

The `TargetArchitecture` setting can also be passed as an environment variable or command-line argument:

```bash
# Environment variable (double-underscore for nested keys)
VirtoCommerce__TargetArchitecture=Arm64 dotnet run

# Command-line argument
dotnet run -- --VirtoCommerce:TargetArchitecture=Arm64
```

**How architecture filtering works:**

`ModuleCopier` reads the PE header of each `.dll`/`.exe` to determine its compiled architecture (X86, X64, ARM, ARM64). A file is copied only if it is compatible with the target:

- Exact architecture match is always accepted.
- X86 binaries are accepted on X64 targets (WoW64 backward compatibility).
- Architecture-neutral files (non-PE files like `.deps.json`, `.pdb`) are always copied.
- When the target already has a file, the copier prefers the source if it has a newer version, a better architecture match, or a newer file date.

### vc-build Integration

The `vc-build` CLI tool installs modules into the discovery path. The platform's static module system is compatible with vc-build's output layout:

```
vc-build install
  -modules VirtoCommerce.Catalog:3.800.0
  -DiscoveryPath ./modules
  -ProbingPath ./app_data/modules
  -SkipDependencyInstallation false
```

**After `vc-build install`**, the discovery path contains:

```
modules/
  VirtoCommerce.Catalog/
    module.manifest
    bin/
      VirtoCommerce.CatalogModule.dll
      VirtoCommerce.CatalogModule.deps.json
      ...native and managed dependencies...
```

**For Docker builds**, add a probing preparation step:

```bash
# After vc-build installs modules, pre-populate probing folder
vc-build compress -ProbingPath ./app_data/modules
```

Or use the platform's own copier as a standalone step (the static classes can be called from a build script or a small console app):

```csharp
// Build-time helper to pre-populate probing folder
var modules = ModuleManifestReader.ReadAll(discoveryPath, probingPath);
ModuleCopier.CopyAll(discoveryPath, probingPath, modules);
```

### Kubernetes with Shared Volume

When running multiple replicas, the probing folder can be shared via a persistent volume. Disable refresh so only one process populates it:

```yaml
apiVersion: apps/v1
kind: Deployment
spec:
  template:
    spec:
      containers:
        - name: platform
          env:
            - name: VirtoCommerce__RefreshProbingFolderOnStart
              value: "false"
          volumeMounts:
            - name: modules
              mountPath: /app/app_data/modules
      volumes:
        - name: modules
          persistentVolumeClaim:
            claimName: modules-pvc
```

Populate the volume once using an init container or a separate job.

### CI/CD Pipeline Example

```yaml
# Azure DevOps / GitHub Actions
steps:
  - name: Install modules
    run: |
      dotnet tool install -g VirtoCommerce.GlobalTool
      vc-build install -modules VirtoCommerce.Catalog VirtoCommerce.Orders

  - name: Prepare probing folder
    run: |
      mkdir -p app_data/modules
      # Copy all module DLLs to probing (same logic as ModuleCopier)
      find modules -path '*/bin/*.dll' -exec cp -n {} app_data/modules/ \;
      find modules -path '*/bin/*.deps.json' -exec cp -n {} app_data/modules/ \;

  - name: Build Docker image
    run: |
      docker build -t myregistry/vc-platform:latest .
```

## Diagnostics

### Console Output

The static classes log to `Console.WriteLine` with prefixed tags for visibility during early startup:

```
[MODULES] Found 12 module manifests in /app/modules
[COPY] Copying VirtoCommerce.CatalogModule.dll (1.0.0 -> 1.1.0)
[COPY] Warning: Could not copy SomeLib.dll (file in use by another process)
[LOAD] Loaded VirtoCommerce.Catalog 3.800.0
[REGISTRY] 12 modules registered, 0 with errors
[STARTUP] Discovered AzureAppConfigStartup from VirtoCommerce.AzureAppConfiguration (priority: -1000)
[INIT] Initializing VirtoCommerce.Catalog 3.800.0 (1/12)
[INIT] Post-initializing VirtoCommerce.Catalog
```

### Failed Modules

After startup completes, failed modules are logged at `Error` level via Serilog:

```
Could not load module VirtoCommerce.Broken 1.0.0. Error: Assembly not found
```

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

## Class Diagram

```
Program.Main()
    |
    v
ModuleManifestReader ──> List<ManifestModuleInfo>
    |                          |
    v                          v
ModuleCopier          ModuleAssemblyLoader
    |                          |
    |                    sets module.Assembly
    v                          |
(probing folder)               v
                       ModuleRegistry  (global static index)
                               |
                               v
                    PlatformStartupDiscovery  (global static list)
                               |
    +--------------------------+----------------------------+
    |                          |                            |
    v                          v                            v
Program.cs              Startup.ConfigureServices    Startup.Configure
ConfigureAppConfiguration   ModuleRunner.InitializeAll   RunConfigure(phase)
ConfigureHostServices       RunConfigureServices         PostInitializeAll
                            AddApplicationPart
                            LocalModuleCatalogAdapter --> DI
```

## Related Files

| File | Purpose |
|------|---------|
| `src/VirtoCommerce.Platform.Modules/ModuleManifestReader.cs` | Manifest scanning |
| `src/VirtoCommerce.Platform.Modules/ModuleCopier.cs` | Assembly copying |
| `src/VirtoCommerce.Platform.Modules/ModuleAssemblyLoader.cs` | Assembly loading |
| `src/VirtoCommerce.Platform.Modules/ModuleRunner.cs` | Module initialization |
| `src/VirtoCommerce.Platform.Modules/ModuleRegistry.cs` | Global module index |
| `src/VirtoCommerce.Platform.Modules/PlatformStartupDiscovery.cs` | IPlatformStartup orchestration |
| `src/VirtoCommerce.Platform.Modules/LocalModuleCatalogAdapter.cs` | DI backward-compat bridge |
| `src/VirtoCommerce.Platform.Core/Modularity/IPlatformStartup.cs` | Early startup hook interface |
| `src/VirtoCommerce.Platform.Core/Modularity/PipelinePhase.cs` | Middleware phase enum |
| `src/VirtoCommerce.Platform.Core/Modularity/StartupPriority.cs` | Execution order constants |
| `src/VirtoCommerce.Platform.Core/Modularity/ModuleManifest.cs` | XML manifest model |
| `src/VirtoCommerce.Platform.Core/Modularity/ManifestModuleInfo.cs` | Runtime module state |
| `src/VirtoCommerce.Platform.Core/Modularity/LocalStorageModuleCatalogOptions.cs` | Configuration options |
| `src/VirtoCommerce.Platform.Web/Program.cs` | Early loading + host builder |
| `src/VirtoCommerce.Platform.Web/Startup.cs` | DI registration + middleware |
