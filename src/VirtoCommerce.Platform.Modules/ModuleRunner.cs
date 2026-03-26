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
    private static ModuleSequenceBoostOptions _boostOptions = new();

    /// <summary>
    /// Set the boost options used by <see cref="SortModulesByDependency"/>. Call once from Program.Main.
    /// </summary>
    public static void Initialize(ModuleSequenceBoostOptions boostOptions = null)
    {
        _boostOptions = boostOptions ?? new ModuleSequenceBoostOptions();
    }

    /// <summary>
    /// Sort modules by dependency order, layer by layer.
    /// Modules with no dependencies come first, then those depending on the first group, and so on.
    /// Within each layer modules are sorted by id; boosted modules come first within their layer.
    /// Throws <see cref="CyclicDependencyFoundException"/> when a cycle is detected.
    /// </summary>
    public static IList<ManifestModuleInfo> SortModulesByDependency(IList<ManifestModuleInfo> modules)
    {
        ArgumentNullException.ThrowIfNull(modules);

        if (modules.Count == 0)
        {
            return [];
        }

        var ignoreCase = StringComparer.OrdinalIgnoreCase;
        var boostedIds = new HashSet<string>(_boostOptions.ModuleSequenceBoost ?? [], ignoreCase);

        // Deduplicate by ID: prefer installed version, then latest.
        var remaining = modules
            .GroupBy(x => x.Id, ignoreCase)
            .ToDictionary(
                g => g.Key,
                g => g.OrderByDescending(x => x.IsInstalled).ThenByDescending(x => x.Version).First(),
                ignoreCase);

        var allIds = remaining.Keys.ToHashSet(ignoreCase);
        var resultIds = new HashSet<string>(ignoreCase);
        var result = new List<ManifestModuleInfo>(remaining.Count);

        while (remaining.Count > 0)
        {
            var layer = remaining.Values
                .Where(x => x.Dependencies.All(d => resultIds.Contains(d.Id) || !allIds.Contains(d.Id)))
                .OrderBy(x => boostedIds.Contains(x.Id) ? 0 : 1)
                .ThenBy(x => x.Id, ignoreCase)
                .ToList();

            if (layer.Count == 0)
            {
                throw new CyclicDependencyFoundException("At least one cyclic dependency has been found in the module catalog. Cycles in the module dependencies are not allowed.");
            }

            foreach (var module in layer)
            {
                result.Add(module);
                resultIds.Add(module.Id);
                remaining.Remove(module.Id);
            }
        }

        return result;
    }

    /// <summary>
    /// Run Initialize on all modules in dependency order.
    /// Skips modules with errors. Sets IHasConfiguration/IHasHostEnvironment/IHasModuleCatalog properties.
    /// </summary>
    public static void InitializeModules(
        IList<ManifestModuleInfo> modules,
        IServiceCollection serviceCollection,
        IConfiguration configuration = null,
        IHostEnvironment hostEnvironment = null,
#pragma warning disable VC0014 // Type is obsolete
        IModuleCatalog moduleCatalog = null
#pragma warning restore VC0014
        )
    {
        ArgumentNullException.ThrowIfNull(modules);
        ArgumentNullException.ThrowIfNull(serviceCollection);

        var logger = ModuleLogger.CreateLogger(typeof(ModuleRunner));

        foreach (var moduleInfo in modules)
        {
            if (moduleInfo.Errors.Count > 0)
            {
                logger.LogWarning("Skipping module {ModuleId} (has errors)", moduleInfo.Id);
                continue;
            }

            if (moduleInfo.Assembly == null)
            {
                logger.LogDebug("Skipping module {ModuleId} (no assembly)", moduleInfo.Id);
                continue;
            }

            try
            {
                logger.LogDebug("Initializing module {ModuleId} {ModuleVersion}", moduleInfo.Id, moduleInfo.Version);

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
                logger.LogError(ex, "Failed to initialize module {ModuleId}", moduleInfo.Id);
            }
        }
    }

    /// <summary>
    /// Run PostInitialize on all initialized modules.
    /// </summary>
    public static void PostInitializeModules(
        IList<ManifestModuleInfo> modules,
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
                logger.LogDebug("Post-initializing module {ModuleId}", moduleInfo.Id);
                moduleInfo.ModuleInstance.PostInitialize(appBuilder);
            }
            catch (Exception ex)
            {
                moduleInfo.Errors.Add(ex.ToString());
                logger.LogError(ex, "Failed to post-initialize module {ModuleId}", moduleInfo.Id);
            }
        }
    }


    /// <summary>
    /// Create an IModule instance from a loaded assembly via reflection.
    /// </summary>
    internal static IModule CreateModuleInstance(ManifestModuleInfo moduleInfo)
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
}
