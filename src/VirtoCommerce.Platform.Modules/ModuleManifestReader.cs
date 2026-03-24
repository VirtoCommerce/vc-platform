using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Extensions.Logging;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Modularity;

namespace VirtoCommerce.Platform.Modules;

/// <summary>
/// Reads module.manifest files from a discovery directory.
/// Pure data extraction - no side effects beyond filesystem reads.
/// Works without DI.
/// </summary>
public static class ModuleManifestReader
{
    /// <summary>
    /// Scan discoveryPath for all module.manifest files, excluding "artifacts" directories.
    /// </summary>
    public static IList<ManifestModuleInfo> ReadAll(string discoveryPath)
    {
        ArgumentNullException.ThrowIfNull(discoveryPath);

        var logger = ModuleLogger.CreateLogger(typeof(ModuleManifestReader));
        logger.LogInformation("Loading modules");

        if (!discoveryPath.EndsWith(Path.DirectorySeparatorChar))
        {
            discoveryPath += Path.DirectorySeparatorChar;
        }

        var result = new List<ManifestModuleInfo>();

        if (!Directory.Exists(discoveryPath))
        {
            logger.LogWarning("Discovery path not found: {DiscoveryPath}", discoveryPath);
            return result;
        }

        foreach (var manifestFile in Directory.EnumerateFiles(discoveryPath, "module.manifest", SearchOption.AllDirectories))
        {
            // Exclude manifests from built module artifacts
            var relativePath = manifestFile.Substring(discoveryPath.Length);
            if (relativePath.Contains("artifacts"))
            {
                continue;
            }

            var moduleInfo = Read(manifestFile);
            if (moduleInfo != null)
            {
                result.Add(moduleInfo);
            }
        }

        logger.LogDebug("Found {ModuleCount} module manifests in {DiscoveryPath}", result.Count, discoveryPath);
        return result;
    }

    /// <summary>
    /// Read a single module.manifest file and return ManifestModuleInfo.
    /// When probingPath is provided, sets Ref to the file:// URI pointing to the assembly in the probing path.
    /// </summary>
    public static ManifestModuleInfo Read(string manifestFilePath)
    {
        ArgumentNullException.ThrowIfNull(manifestFilePath);

        try
        {
            var manifest = ManifestReader.Read(manifestFilePath);
            var moduleInfo = AbstractTypeFactory<ManifestModuleInfo>.TryCreateInstance();
            moduleInfo.LoadFromManifest(manifest);
            moduleInfo.FullPhysicalPath = Path.GetDirectoryName(manifestFilePath);

            if (string.IsNullOrEmpty(moduleInfo.AssemblyFile))
            {
                moduleInfo.State = ModuleState.Initialized;
            }

            moduleInfo.IsInstalled = true;
            return moduleInfo;
        }
        catch (Exception ex)
        {
            var logger = ModuleLogger.CreateLogger(typeof(ModuleManifestReader));
            logger.LogError(ex, "Error reading manifest {ManifestPath}", manifestFilePath);
            return null;
        }
    }
}
