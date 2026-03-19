using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using VirtoCommerce.Platform.Core.Modularity;
using VirtoCommerce.Platform.Core.Modularity.Exceptions;

namespace VirtoCommerce.Platform.Modules;

/// <summary>
/// Creates IModule instances and runs Initialize/PostInitialize in dependency order.
/// Static, no DI.
/// </summary>
public static class ModuleRunner
{
    /// <summary>
    /// Sort modules by dependency order using topological sort.
    /// Modules with no dependencies come first.
    /// </summary>
    public static IReadOnlyList<ManifestModuleInfo> SortByDependency(
        IReadOnlyList<ManifestModuleInfo> modules,
        ModuleSequenceBoostOptions boostOptions = null)
    {
        ArgumentNullException.ThrowIfNull(modules);

        if (modules.Count == 0)
        {
            return [];
        }

        var solver = new ModuleDependencySolver(boostOptions ?? new ModuleSequenceBoostOptions());
        var moduleNames = new HashSet<string>(modules.Select(m => m.ModuleName), StringComparer.OrdinalIgnoreCase);

        foreach (var module in modules)
        {
            solver.AddModule(module.ModuleName);

            if (module.DependsOn != null)
            {
                foreach (var dependency in module.DependsOn)
                {
                    // Only add dependency edge if the dependency is in the list
                    // (missing/failed modules are excluded to avoid MissedModuleException)
                    if (!moduleNames.Contains(dependency))
                    {
                        continue;
                    }

                    var isOptional = module.Dependencies?.Any(x => x.Id == dependency && x.Optional) ?? false;
                    if (!isOptional)
                    {
                        solver.AddDependency(module.ModuleName, dependency);
                    }
                }
            }
        }

        var sortedNames = solver.Solve();
        var modulesByName = modules.ToDictionary(m => m.ModuleName, StringComparer.OrdinalIgnoreCase);
        var result = new List<ManifestModuleInfo>(sortedNames.Length);

        foreach (var name in sortedNames)
        {
            if (modulesByName.TryGetValue(name, out var module))
            {
                result.Add(module);
            }
        }

        return result;
    }

    /// <summary>
    /// Create an IModule instance from a loaded assembly via reflection.
    /// </summary>
    public static IModule CreateModuleInstance(ManifestModuleInfo moduleInfo)
    {
        ArgumentNullException.ThrowIfNull(moduleInfo);

        if (moduleInfo.Assembly == null)
        {
            throw new ModuleInitializeException($"Assembly not loaded for module {moduleInfo.Id}");
        }

        var moduleTypes = moduleInfo.Assembly.GetTypes()
            .Where(x => typeof(IModule).IsAssignableFrom(x) && !x.IsAbstract)
            .ToArray();

        if (moduleTypes.Length == 0)
        {
            throw new ModuleInitializeException($"No IModule implementation found in assembly {moduleInfo.Assembly.FullName}");
        }

        Type moduleType;
        if (moduleTypes.Length == 1)
        {
            moduleType = moduleTypes[0];
        }
        else
        {
            moduleType = moduleTypes.FirstOrDefault(x => x.AssemblyQualifiedName?.StartsWith(moduleInfo.ModuleType) == true);
            if (moduleType == null)
            {
                throw new ModuleInitializeException($"Cannot resolve IModule type '{moduleInfo.ModuleType}' from assembly {moduleInfo.Assembly.FullName}");
            }
        }

        if (Activator.CreateInstance(moduleType) is not IModule instance)
        {
            throw new ModuleInitializeException($"Failed to create instance of {moduleType.FullName}");
        }

        instance.ModuleInfo = moduleInfo;
        return instance;
    }

    /// <summary>
    /// Run Initialize on all modules in dependency order.
    /// Skips modules with errors. Sets IHasConfiguration/IHasHostEnvironment/IHasModuleCatalog properties.
    /// </summary>
    public static void InitializeAll(
        IReadOnlyList<ManifestModuleInfo> modules,
        IServiceCollection serviceCollection,
        IConfiguration configuration = null,
        IHostEnvironment hostEnvironment = null,
        ModuleSequenceBoostOptions boostOptions = null,
#pragma warning disable VC0014 // Type is obsolete
        IModuleCatalog moduleCatalog = null
#pragma warning restore VC0014
        )
    {
        ArgumentNullException.ThrowIfNull(modules);
        ArgumentNullException.ThrowIfNull(serviceCollection);

        var logger = ModuleLogger.CreateLogger(typeof(ModuleRunner));
        var sorted = SortByDependency(modules, boostOptions);
        var count = 0;
        var total = sorted.Count(m => m.Assembly != null && m.Errors.Count == 0);

        foreach (var moduleInfo in sorted)
        {
            if (moduleInfo.Errors.Count > 0)
            {
                logger.LogWarning("Skipping {ModuleId} (has errors)", moduleInfo.Id);
                continue;
            }

            if (moduleInfo.Assembly == null)
            {
                continue;
            }

            try
            {
                count++;
                logger.LogDebug("Initializing {ModuleId} {ModuleVersion} ({Count}/{Total})", moduleInfo.Id, moduleInfo.Version, count, total);

                var instance = CreateModuleInstance(moduleInfo);
                moduleInfo.ModuleInstance = instance;

                if (instance is IHasConfiguration configurable && configuration != null)
                {
                    configurable.Configuration = configuration;
                }

                if (instance is IHasHostEnvironment hasHost && hostEnvironment != null)
                {
                    hasHost.HostEnvironment = hostEnvironment;
                }

#pragma warning disable VC0014 // Type or member is obsolete
                if (instance is IHasModuleCatalog hasModuleCatalog && moduleCatalog != null)
                {
                    hasModuleCatalog.ModuleCatalog = moduleCatalog;
                }
#pragma warning restore VC0014 // Type or member is obsolete

                instance.Initialize(serviceCollection);
                moduleInfo.State = ModuleState.Initialized;
            }
            catch (Exception ex)
            {
                moduleInfo.Errors.Add(ex.ToString());
                logger.LogError(ex, "Failed to initialize {ModuleId}", moduleInfo.Id);
            }
        }
    }

    /// <summary>
    /// Run PostInitialize on all initialized modules.
    /// </summary>
    public static void PostInitializeAll(
        IReadOnlyList<ManifestModuleInfo> modules,
        IApplicationBuilder appBuilder)
    {
        ArgumentNullException.ThrowIfNull(modules);
        ArgumentNullException.ThrowIfNull(appBuilder);

        var logger = ModuleLogger.CreateLogger(typeof(ModuleRunner));

        foreach (var moduleInfo in modules)
        {
            if (moduleInfo.ModuleInstance == null || moduleInfo.Errors.Count > 0)
            {
                continue;
            }

            try
            {
                logger.LogDebug("Post-initializing {ModuleId}", moduleInfo.Id);
                moduleInfo.ModuleInstance.PostInitialize(appBuilder);
            }
            catch (Exception ex)
            {
                moduleInfo.Errors.Add(ex.ToString());
                logger.LogError(ex, "Failed to post-initialize {ModuleId}", moduleInfo.Id);
            }
        }
    }
}
