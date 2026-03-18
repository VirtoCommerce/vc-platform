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
    public static List<ManifestModuleInfo> ParseExternalManifest(
        string manifestJson,
        SemanticVersion platformVersion,
        bool includePrerelease = false)
    {
        ArgumentNullException.ThrowIfNull(manifestJson);
        ArgumentNullException.ThrowIfNull(platformVersion);

        var manifests = JsonSerializer.Deserialize<List<ExternalModuleManifest>>(manifestJson);
        if (manifests is null)
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

        logger.LogDebug("Parsed {ModuleCount} modules from external manifest", result.Count);

        return result;
    }

    /// <summary>
    /// Merge external modules with locally installed modules.
    /// Returns a unified list: installed modules keep their state, external modules show as available.
    /// </summary>
    public static List<ManifestModuleInfo> MergeWithInstalled(
        IReadOnlyList<ManifestModuleInfo> externalModules,
        IReadOnlyList<ManifestModuleInfo> installedModules)
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
    /// Validate that a module can be installed: platform version, dependencies, incompatibilities.
    /// Returns a list of error messages (empty if valid).
    /// </summary>
    public static List<string> ValidateInstall(
        ManifestModuleInfo moduleToInstall,
        IReadOnlyList<ManifestModuleInfo> installedModules,
        SemanticVersion platformVersion)
    {
        ArgumentNullException.ThrowIfNull(moduleToInstall);
        ArgumentNullException.ThrowIfNull(installedModules);
        ArgumentNullException.ThrowIfNull(platformVersion);

        var errors = new List<string>();

        // Check platform version compatibility
        if (!moduleToInstall.PlatformVersion.IsCompatibleWith(platformVersion))
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

        // Check major version compatibility (no automated major upgrade/downgrade)
        var installedModule = installedModules.FirstOrDefault(x => x.Id.EqualsIgnoreCase(moduleToInstall.Id));
        if (installedModule != null && !installedModule.Version.IsCompatibleWithBySemVer(moduleToInstall.Version))
        {
            var direction = installedModule.Version.Major < moduleToInstall.Version.Major ? "upgrade" : "downgrade";
            errors.Add($"Automated {direction} is not feasible due to major version change for {moduleToInstall}");
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
        IReadOnlyList<ManifestModuleInfo> installedModules,
        IReadOnlyCollection<string> excludeModuleIds = null)
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

    private static ManifestModuleInfo GetLatestCompatibleVersion(
        ExternalModuleManifest manifest,
        SemanticVersion platformVersion,
        bool prerelease)
    {
        var latestVersion = manifest.Versions
            .OrderByDescending(x => x.SemanticVersion)
            .Where(x => x.PlatformSemanticVersion.Major == platformVersion.Major)
            .FirstOrDefault(x => string.IsNullOrEmpty(x.VersionTag) != prerelease);

        if (latestVersion == null)
        {
            return null;
        }

        var result = AbstractTypeFactory<ManifestModuleInfo>.TryCreateInstance();
        result.LoadFromExternalManifest(manifest, latestVersion);

        return result;
    }
}
