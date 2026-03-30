using System.Collections.Generic;

namespace VirtoCommerce.Platform.Core.Modularity;

/// <summary>
/// Read-only provider for querying loaded modules.
/// All lists are sorted in dependency order (no-deps first).
/// </summary>
public interface IModuleService
{
    /// <summary>
    /// Returns all modules (installed and not), sorted by dependency order.
    /// </summary>
    IList<ManifestModuleInfo> GetModules();

    /// <summary>
    /// Returns installed modules with no errors, sorted by dependency order.
    /// </summary>
    IList<ManifestModuleInfo> GetInstalledModules();

    /// <summary>
    /// Returns modules that have errors.
    /// </summary>
    IList<ManifestModuleInfo> GetFailedModules();

    /// <summary>
    /// Check if a module is installed.
    /// </summary>
    bool IsInstalled(string moduleId);

    /// <summary>
    /// Check if a module is installed with at least the specified version.
    /// </summary>
    bool IsInstalled(string moduleId, string minVersion);

    /// <summary>
    /// Get module info by ID, or null if not found.
    /// </summary>
    ManifestModuleInfo GetModule(string moduleId);
}
