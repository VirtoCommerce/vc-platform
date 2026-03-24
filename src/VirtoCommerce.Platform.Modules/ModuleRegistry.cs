using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Modularity;

namespace VirtoCommerce.Platform.Modules;

/// <summary>
/// Thread-safe static registry for querying installed module status.
/// Works without DI - can be used from CLI tools and early startup.
/// </summary>
public static class ModuleRegistry
{
    private static IList<ManifestModuleInfo> _modules;
    private static Dictionary<string, ManifestModuleInfo> _modulesById;

    public static void Initialize(IList<ManifestModuleInfo> modules)
    {
        ArgumentNullException.ThrowIfNull(modules);

        _modules = modules;
        _modulesById = modules.ToDictionary(x => x.Id, StringComparer.OrdinalIgnoreCase);

        var logger = ModuleLogger.CreateLogger(typeof(ModuleRegistry));
        logger.LogInformation("Loaded modules: {ModuleCount}, with errors: {ErrorCount}", modules.Count, modules.Count(x => x.Errors.Count > 0));
    }

    public static bool IsInstalled(string moduleId)
    {
        return _modulesById.TryGetValue(moduleId, out var module) && module.IsInstalled;
    }

    public static bool IsInstalled(string moduleId, string minVersion)
    {
        if (!_modulesById.TryGetValue(moduleId, out var module) || !module.IsInstalled)
        {
            return false;
        }

        var requiredVersion = SemanticVersion.Parse(minVersion);
        return module.Version >= requiredVersion;
    }

    public static ManifestModuleInfo GetModule(string moduleId)
    {
        _modulesById.TryGetValue(moduleId, out var module);
        return module;
    }

    public static IList<ManifestModuleInfo> GetAllModules()
    {
        return _modules;
    }

    public static IList<ManifestModuleInfo> GetInstalledModules()
    {
        return _modules.Where(x => x.IsInstalled && x.Errors.Count == 0).ToList();
    }

    public static IList<ManifestModuleInfo> GetFailedModules()
    {
        return _modules.Where(x => x.Errors.Count > 0).ToList();
    }
}
