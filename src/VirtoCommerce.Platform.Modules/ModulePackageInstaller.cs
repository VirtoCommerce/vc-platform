using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using Microsoft.Extensions.Logging;
using VirtoCommerce.Platform.Core.Modularity;

namespace VirtoCommerce.Platform.Modules;

/// <summary>
/// Static module installation and uninstallation operations.
/// Handles ZIP extraction and directory management without DI.
/// </summary>
public static class ModulePackageInstaller
{
    /// <summary>
    /// Install a module from a ZIP file to the target module directory.
    /// </summary>
    public static void Install(string zipPath, string targetModulePath)
    {
        ArgumentNullException.ThrowIfNull(zipPath);
        ArgumentNullException.ThrowIfNull(targetModulePath);

        if (!File.Exists(zipPath))
        {
            throw new FileNotFoundException($"Module package not found: {zipPath}");
        }

        if (!Directory.Exists(targetModulePath))
        {
            Directory.CreateDirectory(targetModulePath);
        }

        ModuleLogger.CreateLogger(typeof(ModulePackageInstaller)).LogInformation("Extracting {FileName} to {TargetPath}", Path.GetFileName(zipPath), targetModulePath);
        ZipFile.ExtractToDirectory(zipPath, targetModulePath, overwriteFiles: true);
        ModuleLogger.CreateLogger(typeof(ModulePackageInstaller)).LogInformation("Successfully installed to {TargetPath}", targetModulePath);
    }

    /// <summary>
    /// Uninstall a module by deleting its directory.
    /// </summary>
    public static void Uninstall(string modulePath)
    {
        ArgumentNullException.ThrowIfNull(modulePath);

        if (Directory.Exists(modulePath))
        {
            ModuleLogger.CreateLogger(typeof(ModulePackageInstaller)).LogInformation("Removing module directory: {ModulePath}", modulePath);
            Directory.Delete(modulePath, recursive: true);
            ModuleLogger.CreateLogger(typeof(ModulePackageInstaller)).LogInformation("Successfully uninstalled from {ModulePath}", modulePath);
        }
    }

    /// <summary>
    /// Validate that a module can be uninstalled: check no other installed modules depend on it.
    /// Returns list of error messages (empty if valid).
    /// </summary>
    public static List<string> ValidateUninstall(
        string moduleId,
        IReadOnlyList<ManifestModuleInfo> installedModules)
    {
        return ModuleDiscovery.ValidateUninstall(moduleId, installedModules);
    }
}
