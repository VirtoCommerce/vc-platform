using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using VirtoCommerce.Platform.Core.Modularity;
using VirtoCommerce.Platform.Modules.Local;

namespace VirtoCommerce.Platform.Modules;

/// <summary>
/// Copies module assemblies from discovery directories to the probing path.
/// Static, no DI. No distributed locking (caller's responsibility).
/// </summary>
public static class ModuleCopier
{
    private static LocalStorageModuleCatalogOptions _options;
    private static FileCopyPolicy _fileCopyPolicy;
    private static ILogger _logger;

    private const string _rebuildMarkerFileName = ".rebuild";

    public static void Initialize(LocalStorageModuleCatalogOptions options, IFileMetadataProvider metadataProvider)
    {
        _options = options;
        _fileCopyPolicy = new FileCopyPolicy(metadataProvider, Options.Create(options));
        _logger = ModuleLogger.CreateLogger(typeof(ModuleCopier));
    }

    /// <summary>
    /// Copy all module assemblies to the probing folder.
    /// </summary>
    public static void Copy(IList<ManifestModuleInfo> modules, Architecture environmentArchitecture)
    {
        ArgumentNullException.ThrowIfNull(modules);

        var probingPath = _options.ProbingPath;
        var refreshProbing = _options.RefreshProbingFolderOnStart;

        // Check if a module install/uninstall occurred — clear probing folder for clean rebuild
        var markerPath = Path.Combine(probingPath, _rebuildMarkerFileName);
        if (File.Exists(markerPath))
        {
            _logger.LogInformation("Rebuild marker found — clearing probing folder for clean rebuild");

            foreach (var entry in new DirectoryInfo(probingPath).EnumerateFileSystemInfos())
            {
                if (entry is DirectoryInfo directory)
                {
                    directory.Delete(recursive: true);
                }
                else
                {
                    entry.Delete();
                }
            }

            refreshProbing = true;
        }

        if (!Directory.Exists(probingPath))
        {
            Directory.CreateDirectory(probingPath);
            refreshProbing = true;
        }

        if (refreshProbing)
        {

            _logger.LogDebug("Copying modules from {From} to {To}, count: {Count}, architecture: {Architecture}",
                _options.DiscoveryPath, probingPath, modules.Count, environmentArchitecture);

            foreach (var module in modules)
            {
                if (module.FullPhysicalPath != null)
                {
                    CopyModule(module.FullPhysicalPath, probingPath, environmentArchitecture);
                }
            }

            _logger.LogDebug("Module copying completed");
        }
    }

    /// <summary>
    /// Write a marker file to the probing folder so that the next startup rebuilds it from scratch.
    /// Called at runtime after install/uninstall when loaded assemblies are locked by the running process.
    /// </summary>
    public static void InvalidateProbingFolder()
    {
        if (!Directory.Exists(_options.ProbingPath))
        {
            Directory.CreateDirectory(_options.ProbingPath);
        }

        var markerPath = Path.Combine(_options.ProbingPath, _rebuildMarkerFileName);
        File.WriteAllBytes(markerPath, Array.Empty<byte>());

        _logger.LogInformation("Probing folder marked for rebuild on next startup");
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
