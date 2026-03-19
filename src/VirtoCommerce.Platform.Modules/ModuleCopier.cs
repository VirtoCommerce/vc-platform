using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using Microsoft.Extensions.Logging;
using VirtoCommerce.Platform.Core.Modularity;

namespace VirtoCommerce.Platform.Modules;

/// <summary>
/// Copies module assemblies from discovery directories to the probing path.
/// Static, no DI. No distributed locking (caller's responsibility).
/// </summary>
public static class ModuleCopier
{
    private static ILogger _logger;
    private static IFileCopyPolicy _fileCopyPolicy;

    public static void Initialize(IFileCopyPolicy fileCopyPolicy)
    {
        _logger = ModuleLogger.CreateLogger(typeof(ModuleCopier));
        _fileCopyPolicy = fileCopyPolicy;
    }

    /// <summary>
    /// Copy all module assemblies to probingPath. Also copies platform assemblies from discoveryPath/bin.
    /// </summary>
    public static void CopyAll(
        string discoveryPath,
        string probingPath,
        IReadOnlyList<ManifestModuleInfo> modules,
        Architecture environmentArchitecture)
    {
        ArgumentNullException.ThrowIfNull(discoveryPath);
        ArgumentNullException.ThrowIfNull(probingPath);
        ArgumentNullException.ThrowIfNull(modules);

        _logger.LogDebug("Copying modules from {From} to {To}, count: {Count}, architecture: {Architecture}",
            discoveryPath, probingPath, modules.Count, environmentArchitecture);

        if (!Directory.Exists(probingPath))
        {
            Directory.CreateDirectory(probingPath);
        }

        foreach (var module in modules)
        {
            if (module.FullPhysicalPath != null)
            {
                CopyModule(module.FullPhysicalPath, probingPath, environmentArchitecture);
            }
        }

        _logger.LogDebug("Module copying completed");
    }

    /// <summary>
    /// Copy a single module's bin/ directory contents to the probing path.
    /// </summary>
    public static void CopyModule(string sourceDirectoryPath, string targetDirectoryPath, Architecture environmentArchitecture)
    {
        if (sourceDirectoryPath == null)
        {
            return;
        }

        var sourceBinPath = Path.Combine(sourceDirectoryPath, "bin");
        if (!Directory.Exists(sourceBinPath))
        {
            _logger.LogDebug("No bin directory for module at {Path}, skipping", sourceDirectoryPath);
            return;
        }

        _logger.LogDebug("Copying assemblies from {SourceBinPath}", sourceBinPath);

        // Explicit override or auto-detect (supports X86, X64, Arm, Arm64)

        foreach (var sourceFilePath in Directory.EnumerateFiles(sourceBinPath, "*", SearchOption.AllDirectories))
        {
            var sourceRelativePath = Path.GetRelativePath(sourceBinPath, sourceFilePath);
            var targetRelativePath = GetTargetRelativePath(sourceRelativePath);

            if (targetRelativePath == null)
            {
                continue;
            }

            var targetFilePath = Path.Combine(targetDirectoryPath, targetRelativePath);
            CopyFile(sourceFilePath, targetFilePath, environmentArchitecture);
        }
    }

    /// <summary>
    /// Map a source-relative file path to its target-relative path in the probing folder.
    /// Returns null if the file should be skipped (TPA, reference assemblies, etc.).
    /// </summary>
    public static string GetTargetRelativePath(string sourceRelativeFilePath)
    {
        return _fileCopyPolicy.GetTargetRelativePath(sourceRelativeFilePath);
    }

    private static void CopyFile(string sourceFilePath, string targetFilePath, Architecture environmentArchitecture)
    {
        if (!IsCopyRequired(sourceFilePath, targetFilePath, environmentArchitecture, out var result))
        {
            return;
        }

        _logger.LogDebug("Updating {TargetFile}: NewVersion={NewVersion}, NewArchitecture={NewArchitecture}, NewDate={NewDate}",
            Path.GetFileName(targetFilePath), result.NewVersion, result.NewArchitecture, result.NewDate);

        var targetDirectoryPath = Path.GetDirectoryName(targetFilePath);
        if (targetDirectoryPath != null && !Directory.Exists(targetDirectoryPath))
        {
            Directory.CreateDirectory(targetDirectoryPath);
        }

        try
        {
            File.Copy(sourceFilePath, targetFilePath, true);
        }
        catch (IOException)
        {
            // VP-3719: Swallow only for same-version date-only refresh (another process may be copying the same file).
            // For version upgrades, architecture changes, or new files, re-throw so the failure is visible to operators.
            if (result.NewDate)
            {
                _logger.LogWarning("File '{TargetFile}' was not updated by '{SourceFile}' of the same version but later modified date, because probably it was used by another process",
                    targetFilePath, sourceFilePath);
            }
            else
            {
                throw;
            }
        }
    }

    public static bool IsCopyRequired(string sourceFilePath, string targetFilePath, Architecture environmentArchitecture, out FileCompareResult result)
    {
        return _fileCopyPolicy.IsCopyRequired(sourceFilePath, targetFilePath, environmentArchitecture, out result);
    }
}
