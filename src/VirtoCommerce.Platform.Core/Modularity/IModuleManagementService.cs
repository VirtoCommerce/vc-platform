using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace VirtoCommerce.Platform.Core.Modularity;

/// <summary>
/// Service for module catalog management: listing (external + installed), install/uninstall, dependency resolution.
/// Registered as singleton to enable cross-request caching of external module manifests.
/// </summary>
public interface IModuleManagementService
{
    /// <summary>
    /// Returns list of external + installed modules. Lazy-loaded from external manifest URLs on first access.
    /// </summary>
    IList<ManifestModuleInfo> GetModules();

    /// <summary>
    /// Clears cached modules and re-fetches from external manifest URLs.
    /// </summary>
    void ReloadModules();

    /// <summary>
    /// Returns the given modules plus all their transitive dependencies (prerequisites), sorted in dependency order.
    /// Walks DOWN the dependency graph: "what do these modules need to be installed?"
    /// Resolves module IDs to ManifestModuleInfo internally (installed version preferred, then latest available).
    /// </summary>
    IList<ManifestModuleInfo> GetDependencies(IList<string> moduleIds);

    /// <summary>
    /// Returns installed modules that depend ON the given modules (reverse dependencies).
    /// Walks UP the dependency graph: "what will break if these modules are removed?"
    /// Resolves module IDs to installed ManifestModuleInfo internally.
    /// </summary>
    IList<ManifestModuleInfo> GetDependents(IList<string> moduleIds);


    /// <summary>
    /// Add an uploaded module to the merged catalog (for the upload/localstorage scenario).
    /// Validates dependencies before adding.
    /// </summary>
    ManifestModuleInfo AddUploadedModule(ManifestModuleInfo module);

    /// <summary>
    /// Install or update modules by ID with optional version (null = latest).
    /// Resolves modules from the merged catalog, downloads from URL, extracts ZIP, validates dependencies.
    /// Reports progress via <paramref name="progress"/>.
    /// </summary>
    void InstallModules(IList<ModuleInstallRequest> modules, IProgress<ProgressMessage> progress);

    /// <summary>
    /// Uninstall modules by ID.
    /// Resolves installed modules from the catalog, validates dependents, calls module.Uninstall(), deletes directory.
    /// Reports progress via <paramref name="progress"/>.
    /// </summary>
    void UninstallModules(IList<string> moduleIds, IProgress<ProgressMessage> progress);


    /// <summary>
    /// Get modules to auto-install from the given bundles, including their dependencies.
    /// Returns modules that are not yet installed.
    /// </summary>
    IList<ManifestModuleInfo> GetAutoInstallModules(string[] moduleBundles);

    /// <summary>
    /// Validate that a specific module version package exists at the download URL.
    /// Constructs the URL by replacing the known version in the module's PackageUrl template.
    /// </summary>
    Task<bool> ValidateModuleVersionAsync(string moduleId, string version);

    /// <summary>
    /// Validate, register, and prepare a custom module version for installation.
    /// Returns the registered <see cref="ManifestModuleInfo"/> if valid, or null if the package was not found.
    /// </summary>
    Task<ManifestModuleInfo> RegisterCustomModuleVersionAsync(string moduleId, string version);
}
