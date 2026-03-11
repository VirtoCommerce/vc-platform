using System;
using System.Collections.Generic;
using System.IO;
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
    private static readonly JsonSerializerOptions _manifestJsonOptions = new()
    {
        PropertyNameCaseInsensitive = true,
    };

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

        var result = new List<ManifestModuleInfo>();
        var manifests = JsonSerializer.Deserialize<List<ExternalModuleManifest>>(manifestJson, _manifestJsonOptions);

        if (manifests == null)
        {
            return result;
        }

        foreach (var manifest in manifests)
        {
            if (manifest.Versions == null)
            {
                ModuleLogger.CreateLogger(typeof(ModuleDiscovery)).LogWarning("Module {ModuleId} has invalid format, missing 'versions'", manifest.Id);
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
                var latestPre = GetLatestCompatibleVersion(manifest, platformVersion, prerelease: true);
                if (latestPre != null)
                {
                    result.Add(latestPre);
                }
            }
        }

        ModuleLogger.CreateLogger(typeof(ModuleDiscovery)).LogInformation("Parsed {ModuleCount} modules from external manifest", result.Count);
        return result;
    }

    /// <summary>
    /// Merge external modules with locally installed modules.
    /// Returns unified list: installed modules keep their state, external modules show as available.
    /// </summary>
    public static List<ManifestModuleInfo> MergeWithInstalled(
        IReadOnlyList<ManifestModuleInfo> external,
        IReadOnlyList<ManifestModuleInfo> installed)
    {
        ArgumentNullException.ThrowIfNull(external);
        ArgumentNullException.ThrowIfNull(installed);

        var result = new List<ManifestModuleInfo>();
        var installedById = installed.ToDictionary(m => m.Id, StringComparer.OrdinalIgnoreCase);

        foreach (var extModule in external)
        {
            if (installedById.TryGetValue(extModule.Id, out var localModule))
            {
                if (extModule.Equals(localModule))
                {
                    extModule.IsInstalled = localModule.IsInstalled;
                    extModule.Errors.AddRange(localModule.Errors);
                }
                else if (localModule.Version > extModule.Version)
                {
                    continue; // Local is newer, skip external
                }
            }

            result.Add(extModule);
        }

        // Add installed modules not found in external list
        foreach (var localModule in installed)
        {
            if (!result.Any(m => m.Id.Equals(localModule.Id, StringComparison.OrdinalIgnoreCase)))
            {
                result.Add(localModule);
            }
        }

        return result;
    }

    /// <summary>
    /// Validate that a module can be installed: platform version, dependencies, incompatibilities.
    /// Returns list of error messages (empty if valid).
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
        if (!moduleToInstall.Incompatibilities.IsNullOrEmpty())
        {
            var installedIncompat = installedModules
                .Where(m => m.IsInstalled)
                .Where(m => moduleToInstall.Incompatibilities.Any(i =>
                    i.Id.Equals(m.Id, StringComparison.OrdinalIgnoreCase) &&
                    i.Version.IsCompatibleWith(m.Version)))
                .ToList();

            if (installedIncompat.Count > 0)
            {
                errors.Add($"{moduleToInstall} is incompatible with installed {string.Join(", ", installedIncompat)}");
            }
        }

        // Check major version compatibility (no automated major upgrade/downgrade)
        var alreadyInstalled = installedModules.FirstOrDefault(m =>
            m.Id.Equals(moduleToInstall.Id, StringComparison.OrdinalIgnoreCase) && m.IsInstalled);
        if (alreadyInstalled != null && !alreadyInstalled.Version.IsCompatibleWithBySemVer(moduleToInstall.Version))
        {
            var direction = alreadyInstalled.Version.Major < moduleToInstall.Version.Major ? "upgrade" : "downgrade";
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
        var errors = new List<string>();

        var dependingModules = installedModules
            .Where(m => m.IsInstalled && m.DependsOn.Contains(moduleId))
            .Where(m => excludeModuleIds == null || !excludeModuleIds.Contains(m.Id))
            .ToList();

        foreach (var dep in dependingModules)
        {
            errors.Add($"Unable to uninstall '{moduleId}' because '{dep.Id}' depends on it");
        }

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
