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
    private static volatile IReadOnlyList<ManifestModuleInfo> _modules = [];
    private static volatile Dictionary<string, ManifestModuleInfo> _moduleIndex = new(StringComparer.OrdinalIgnoreCase);
    private static readonly object _lock = new();

    public static void Initialize(IReadOnlyList<ManifestModuleInfo> modules)
    {
        ArgumentNullException.ThrowIfNull(modules);

        lock (_lock)
        {
            _modules = modules;
            _moduleIndex = new Dictionary<string, ManifestModuleInfo>(modules.Count, StringComparer.OrdinalIgnoreCase);

            foreach (var module in modules)
            {
                _moduleIndex[module.Id] = module;
            }
        }

        var logger = ModuleLogger.CreateLogger(typeof(ModuleRegistry));
        logger.LogInformation("{ModuleCount} modules registered, {ErrorCount} with errors", modules.Count, modules.Count(m => m.Errors.Count > 0));
    }

    public static bool IsInstalled(string moduleId)
    {
        return _moduleIndex.TryGetValue(moduleId, out var module) && module.IsInstalled;
    }

    public static bool IsInstalled(string moduleId, string minVersion)
    {
        if (!_moduleIndex.TryGetValue(moduleId, out var module) || !module.IsInstalled)
        {
            return false;
        }

        var requiredVersion = SemanticVersion.Parse(minVersion);
        return module.Version >= requiredVersion;
    }

    public static ManifestModuleInfo GetModule(string moduleId)
    {
        _moduleIndex.TryGetValue(moduleId, out var module);
        return module;
    }

    public static IReadOnlyList<ManifestModuleInfo> GetAllModules()
    {
        return _modules;
    }

    public static IReadOnlyList<ManifestModuleInfo> GetInstalledModules()
    {
        return _modules.Where(m => m.IsInstalled && m.Errors.Count == 0).ToArray();
    }

    public static IReadOnlyList<ManifestModuleInfo> GetFailedModules()
    {
        return _modules.Where(m => m.Errors.Count > 0).ToArray();
    }

    public static void Reset()
    {
        lock (_lock)
        {
            _modules = [];
            _moduleIndex = new(StringComparer.OrdinalIgnoreCase);
        }
    }
}
