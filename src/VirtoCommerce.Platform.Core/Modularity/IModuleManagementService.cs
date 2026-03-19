using System;
using System.Collections.Generic;

namespace VirtoCommerce.Platform.Core.Modularity
{
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
        /// </summary>
        IList<ManifestModuleInfo> GetDependencies(IList<ManifestModuleInfo> selectedModules);

        /// <summary>
        /// Returns installed modules that depend ON the given modules (reverse dependencies).
        /// Walks UP the dependency graph: "what will break if these modules are removed?"
        /// </summary>
        IList<ManifestModuleInfo> GetDependents(IList<ManifestModuleInfo> modules);


        /// <summary>
        /// Add an uploaded module to the merged catalog (for the upload/localstorage scenario).
        /// Validates dependencies before adding.
        /// </summary>
        ManifestModuleInfo AddUploadedModule(ManifestModuleInfo module);

        /// <summary>
        /// Install or update modules. Downloads from URL, extracts ZIP, validates dependencies.
        /// Reports progress via <paramref name="progress"/>.
        /// </summary>
        void InstallModules(IList<ManifestModuleInfo> modules, IProgress<ProgressMessage> progress);

        /// <summary>
        /// Uninstall modules. Validates dependents, calls module.Uninstall(), deletes directory.
        /// Reports progress via <paramref name="progress"/>.
        /// </summary>
        void UninstallModules(IList<ManifestModuleInfo> modules, IProgress<ProgressMessage> progress);


        /// <summary>
        /// Get modules to auto-install from the given bundles, including their dependencies.
        /// Returns modules that are not yet installed.
        /// </summary>
        IList<ManifestModuleInfo> GetAutoInstallModules(string[] moduleBundles);
    }
}
