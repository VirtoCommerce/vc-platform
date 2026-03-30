using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Modularity;

namespace VirtoCommerce.Platform.Modules;

/// <summary>
/// Static core logic for external module discovery.
/// Handles manifest parsing and version comparison without HTTP/auth (those stay in DI wrappers).
/// </summary>
public static class ModuleDiscovery
{
    /// <summary>
    /// Parse external module manifest JSON into a list of ManifestModuleInfo.
    /// Pure function - no HTTP, works on already-downloaded data.
    /// </summary>
    public static IList<ManifestModuleInfo> ParseExternalManifest(
        string manifestJson,
        SemanticVersion platformVersion,
        bool includePrerelease = false)
    {
        ArgumentNullException.ThrowIfNull(manifestJson);
        ArgumentNullException.ThrowIfNull(platformVersion);

        var manifests = JsonSerializer.Deserialize<List<ExternalModuleManifest>>(manifestJson);
        if (manifests.IsNullOrEmpty())
        {
            return [];
        }

        var result = new List<ManifestModuleInfo>();
        var logger = ModuleLogger.CreateLogger(typeof(ModuleDiscovery));

        foreach (var manifest in manifests)
        {
            if (manifest.Versions == null)
            {
                logger.LogWarning("Module {ModuleId} has invalid format, missing 'versions'", manifest.Id);
                continue;
            }

            // Load latest stable version compatible with platform major version
            var latestStable = GetLatestCompatibleVersion(manifest, platformVersion, prerelease: false);
            if (latestStable != null)
            {
                result.Add(latestStable);
            }

            if (includePrerelease)
            {
                var latestPrerelease = GetLatestCompatibleVersion(manifest, platformVersion, prerelease: true);
                if (latestPrerelease != null)
                {
                    result.Add(latestPrerelease);
                }
            }
        }

        logger.LogDebug("Parsed {ModuleCount} modules from the external manifest", result.Count);

        return result;
    }

    /// <summary>
    /// Merge external modules with locally installed modules.
    /// Returns a unified list: installed modules keep their state, external modules show as available.
    /// </summary>
    public static IList<ManifestModuleInfo> MergeWithInstalled(
        IList<ManifestModuleInfo> externalModules,
        IList<ManifestModuleInfo> installedModules)
    {
        ArgumentNullException.ThrowIfNull(externalModules);
        ArgumentNullException.ThrowIfNull(installedModules);

        var result = new List<ManifestModuleInfo>();
        var installedModulesById = installedModules.ToDictionary(x => x.Id, StringComparer.OrdinalIgnoreCase);

        foreach (var externalModule in externalModules)
        {
            if (installedModulesById.TryGetValue(externalModule.Id, out var installedModule))
            {
                if (externalModule.Equals(installedModule))
                {
                    externalModule.IsInstalled = installedModule.IsInstalled;
                    externalModule.Errors.AddRange(installedModule.Errors);
                }
                else if (installedModule.Version > externalModule.Version)
                {
                    continue; // Local is newer, skip external
                }
                // else: external is newer (update available) — add external as-is (IsInstalled=false),
                // the installed version will be added by the second loop below
            }

            result.Add(externalModule);
        }

        // Add installed modules not already in result (version-aware, matching old Except behavior).
        // Uses Contains/Equals which compares ID + Version + VersionTag, so installed v1.0
        // is added alongside external v1.1 for the update scenario.
        foreach (var installedModule in installedModules)
        {
            if (!result.Contains(installedModule))
            {
                result.Add(installedModule);
            }
        }

        return result;
    }

    /// <summary>
    /// Validate all loaded modules at startup: platform version compatibility, dependency version compatibility, incompatibilities.
    /// Populates <see cref="ManifestModuleInfo.Errors"/> for each module that fails validation.
    /// Modules with errors are skipped during initialization by <see cref="ModuleRunner"/>.
    /// </summary>
    public static void ValidateModules(IList<ManifestModuleInfo> modules, SemanticVersion platformVersion)
    {
        ArgumentNullException.ThrowIfNull(modules);
        ArgumentNullException.ThrowIfNull(platformVersion);

        var logger = ModuleLogger.CreateLogger(typeof(ModuleDiscovery));

        foreach (var module in modules)
        {
            // 1. Platform version: module target must be ≤ running platform, same major version
            if (!module.PlatformVersion.IsCompatibleWith(platformVersion) ||
                !module.PlatformVersion.IsCompatibleWithBySemVer(platformVersion))
            {
                module.Errors.Add($"Module requires platform version {module.PlatformVersion}, which is incompatible with current version {platformVersion}");
            }

            // 2. Incompatibilities: check if incompatible modules are installed
            if (module.Incompatibilities?.Count > 0)
            {
                var installedIncompatibilities = modules
                    .Where(x => module.Incompatibilities.Any(incompatible =>
                        incompatible.Id.EqualsIgnoreCase(x.Id) &&
                        incompatible.Version.IsCompatibleWith(x.Version)))
                    .ToList();

                if (installedIncompatibilities.Count > 0)
                {
                    module.Errors.Add($"{module} is incompatible with installed {string.Join(", ", installedIncompatibilities)}. You should uninstall these modules first.");
                }
            }

            // 3. Dependency version: each declared dependency must be SemVer-compatible with installed version
            if (module.Dependencies?.Count > 0)
            {
                foreach (var dependency in module.Dependencies)
                {
                    if (dependency.Optional)
                    {
                        continue;
                    }

                    var installedDependency = modules.FirstOrDefault(x => x.Id.EqualsIgnoreCase(dependency.Id));
                    if (installedDependency == null)
                    {
                        module.Errors.Add($"Module dependency {dependency.Id} {dependency.Version} is not installed");
                    }
                    else if (!dependency.Version.IsCompatibleWithBySemVer(installedDependency.Version))
                    {
                        module.Errors.Add($"Module dependency {dependency.Id} {dependency.Version} is incompatible with installed {installedDependency.Version}");
                    }
                }
            }
        }

        // 4. Cascade errors to dependents: if module A failed, all modules depending on A must also fail.
        // This prevents cryptic DI errors like "Unable to resolve IItemService" when Catalog failed but xCart still tries to initialize.
        var failedIds = modules
            .Where(x => x.Errors.Count > 0)
            .Select(x => x.Id)
            .ToHashSet(StringComparer.OrdinalIgnoreCase);

        var changed = true;
        while (changed)
        {
            changed = false;
            foreach (var module in modules.Where(x => x.Dependencies?.Count > 0 && x.Errors.Count == 0))
            {
                var failedDependency = module.Dependencies.FirstOrDefault(x =>
                    !x.Optional &&
                    failedIds.Contains(x.Id));

                if (failedDependency != null)
                {
                    module.Errors.Add($"Module skipped because its dependency '{failedDependency.Id}' has errors");
                    failedIds.Add(module.Id);
                    changed = true;
                }
            }
        }

        var errorCount = 0;
        foreach (var module in modules.Where(x => x.Errors.Count > 0))
        {
            errorCount++;
            logger.LogWarning("Module {ModuleId} has errors: {Errors}", module.Id, string.Join("; ", module.Errors));
        }

        if (errorCount > 0)
        {
            logger.LogWarning("{ErrorCount} modules failed validation (including cascaded dependents)", errorCount);
        }
    }

    /// <summary>
    /// Validate that a module can be installed: platform version, dependencies, incompatibilities.
    /// Returns a list of error messages (empty if valid).
    /// </summary>
    public static List<string> ValidateInstall(
        ManifestModuleInfo moduleToInstall,
        IList<ManifestModuleInfo> installedModules,
        SemanticVersion platformVersion)
    {
        ArgumentNullException.ThrowIfNull(moduleToInstall);
        ArgumentNullException.ThrowIfNull(installedModules);
        ArgumentNullException.ThrowIfNull(platformVersion);

        var errors = new List<string>();

        // Check platform version compatibility:
        // 1. Module target platform version must be ≤ running platform version
        // 2. Major versions must match (no cross-major-version compatibility)
        if (!moduleToInstall.PlatformVersion.IsCompatibleWith(platformVersion) ||
            !moduleToInstall.PlatformVersion.IsCompatibleWithBySemVer(platformVersion))
        {
            errors.Add($"Target platform version {moduleToInstall.PlatformVersion} is incompatible with current {platformVersion}");
        }

        // Check incompatibilities
        if (moduleToInstall.Incompatibilities?.Count > 0)
        {
            var installedIncompatibleModules = installedModules
                .Where(installed => moduleToInstall.Incompatibilities.Any(incompatible =>
                    incompatible.Id.EqualsIgnoreCase(installed.Id) &&
                    incompatible.Version.IsCompatibleWith(installed.Version)))
                .ToList();

            if (installedIncompatibleModules.Count > 0)
            {
                errors.Add($"{moduleToInstall} is incompatible with installed {string.Join(", ", installedIncompatibleModules)}");
            }
        }

        return errors;
    }

    /// <summary>
    /// Validate that a module can be uninstalled: check no installed modules depend on it.
    /// Returns list of error messages (empty if valid).
    /// </summary>
    /// <param name="moduleId">The module to uninstall.</param>
    /// <param name="installedModules">All currently installed modules.</param>
    /// <param name="excludeModuleIds">Optional set of module IDs also being uninstalled (their dependencies should be ignored).</param>
    public static List<string> ValidateUninstall(
        string moduleId,
        IList<ManifestModuleInfo> installedModules,
        IList<string> excludeModuleIds = null)
    {
        var dependingModules = installedModules
            .Where(x =>
                x.DependsOn.ContainsIgnoreCase(moduleId) &&
                (excludeModuleIds == null || !excludeModuleIds.ContainsIgnoreCase(x.Id)));

        var errors = dependingModules
            .Select(x => $"Unable to uninstall '{moduleId}' because '{x.Id}' depends on it")
            .ToList();

        return errors;
    }

    /// <summary>
    /// Returns the given modules plus all their transitive dependencies (prerequisites), sorted in dependency order.
    /// Walks DOWN the dependency graph. For each dependency, selects the best compatible version
    /// from <paramref name="allAvailableModules"/> (prefers installed, then latest compatible).
    /// </summary>
    public static IList<ManifestModuleInfo> GetDependencies(
        IList<ManifestModuleInfo> selectedModules,
        IList<ManifestModuleInfo> allAvailableModules)
    {
        ArgumentNullException.ThrowIfNull(selectedModules);
        ArgumentNullException.ThrowIfNull(allAvailableModules);

        var completeList = new List<ManifestModuleInfo>();
        var pendingList = new List<ManifestModuleInfo>(selectedModules);

        while (pendingList.Count > 0)
        {
            var moduleInfo = pendingList[0];
            pendingList.RemoveAt(0);

            // Resolve dependencies for this module
            if (moduleInfo.Dependencies != null)
            {
                foreach (var dependency in moduleInfo.Dependencies)
                {
                    // Find all available versions of this dependency
                    var candidates = allAvailableModules
                        .Where(x => x.Id.EqualsIgnoreCase(dependency.Id))
                        .Where(x => dependency.Version.IsCompatibleWithBySemVer(x.Version))
                        .OrderByDescending(x => x.Version)
                        .ToList();

                    // Prefer installed version, then latest compatible
                    var resolved = candidates.FirstOrDefault(x => x.IsInstalled) ?? candidates.FirstOrDefault();

                    if (resolved != null && !completeList.Contains(resolved) && !pendingList.Contains(resolved))
                    {
                        pendingList.Add(resolved);
                    }
                }
            }

            if (!completeList.Contains(moduleInfo))
            {
                completeList.Add(moduleInfo);
            }
        }

        return ModuleRunner.SortModulesByDependency(completeList).ToList();
    }

    private static ManifestModuleInfo GetLatestCompatibleVersion(
        ExternalModuleManifest manifest,
        SemanticVersion platformVersion,
        bool prerelease)
    {
        var latestVersion = manifest.Versions
            .OrderByDescending(x => x.SemanticVersion)
            .FirstOrDefault(x =>
                x.PlatformSemanticVersion.Major == platformVersion.Major &&
                x.VersionTag.IsNullOrEmpty() != prerelease);

        if (latestVersion == null)
        {
            return null;
        }

        var result = AbstractTypeFactory<ManifestModuleInfo>.TryCreateInstance();
        result.LoadFromExternalManifest(manifest, latestVersion);

        return result;
    }
}
