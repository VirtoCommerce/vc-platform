using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using Microsoft.Extensions.Logging;
using VirtoCommerce.Platform.Core.Modularity;
using VirtoCommerce.Platform.Modules.AssemblyLoading;

namespace VirtoCommerce.Platform.Modules;

/// <summary>
/// Copies module assemblies from discovery directories to the probing path.
/// Static, no DI. No distributed locking (caller's responsibility).
/// </summary>
public static class ModuleCopier
{
    // Default file extension lists (from LocalStorageModuleCatalogOptions defaults)
    private static readonly string[] _localizationExtensions = ["resources.dll"];
    private static readonly string[] _assemblyExtensions = [".dll", ".exe"];
    private static readonly string[] _serviceExtensions = [".pdb", ".xml", ".deps.json", ".runtimeconfig.json", ".runtimeconfig.dev.json", ".dep", ".zip"];
    private static readonly string[] _referenceAssemblyFolders = ["ref"];
    private static readonly string _runtimesDirectory = "runtimes" + Path.DirectorySeparatorChar;

    /// <summary>
    /// Copy all module assemblies to probingPath. Also copies platform assemblies from discoveryPath/bin.
    /// </summary>
    public static void CopyAll(
        string discoveryPath,
        string probingPath,
        IReadOnlyList<ManifestModuleInfo> modules,
        Architecture? targetArchitecture = null)
    {
        ArgumentNullException.ThrowIfNull(discoveryPath);
        ArgumentNullException.ThrowIfNull(probingPath);
        ArgumentNullException.ThrowIfNull(modules);

        // Resolve architecture once: explicit override or auto-detect (supports X86, X64, Arm, Arm64)
        var resolvedArch = targetArchitecture ?? RuntimeInformation.ProcessArchitecture;

        if (!Directory.Exists(probingPath))
        {
            Directory.CreateDirectory(probingPath);
        }

        // Copy platform assemblies
        CopyModule(discoveryPath, probingPath, resolvedArch);

        // Copy each module's assemblies
        foreach (var module in modules)
        {
            if (module.FullPhysicalPath != null)
            {
                CopyModule(module.FullPhysicalPath, probingPath, resolvedArch);
            }
        }
    }

    /// <summary>
    /// Copy a single module's bin/ directory contents to the probing path.
    /// </summary>
    public static void CopyModule(string modulePath, string probingPath, Architecture? targetArchitecture = null)
    {
        if (modulePath == null)
        {
            return;
        }

        var sourceBinPath = Path.Combine(modulePath, "bin");
        if (!Directory.Exists(sourceBinPath))
        {
            return;
        }

        // Explicit override or auto-detect (supports X86, X64, Arm, Arm64)
        var environment = targetArchitecture ?? RuntimeInformation.ProcessArchitecture;

        foreach (var sourceFilePath in Directory.EnumerateFiles(sourceBinPath, "*", SearchOption.AllDirectories))
        {
            var sourceRelativePath = Path.GetRelativePath(sourceBinPath, sourceFilePath);
            var targetRelativePath = GetTargetRelativePath(sourceRelativePath);

            if (targetRelativePath == null)
            {
                continue;
            }

            var targetFilePath = Path.Combine(probingPath, targetRelativePath);
            CopyFile(environment, sourceFilePath, targetFilePath);
        }
    }

    /// <summary>
    /// Map a source-relative file path to its target-relative path in the probing folder.
    /// Returns null if the file should be skipped (TPA, reference assemblies, etc.).
    /// </summary>
    public static string GetTargetRelativePath(string sourceRelativeFilePath)
    {
        var fileName = Path.GetFileName(sourceRelativeFilePath);

        // Skip Trusted Platform Assemblies
        if (Tpa.ContainsAssembly(fileName))
        {
            return null;
        }

        // Skip reference assembly directories
        if (IsReferenceDirectory(sourceRelativeFilePath))
        {
            return null;
        }

        // Preserve runtimes directory structure
        if (sourceRelativeFilePath.StartsWith(_runtimesDirectory, StringComparison.OrdinalIgnoreCase))
        {
            return sourceRelativeFilePath;
        }

        // Localization files: keep language folder
        if (IsLocalizationFile(fileName))
        {
            return Path.Combine(GetLastDirectoryName(sourceRelativeFilePath), fileName);
        }

        // Assembly and related files: flatten to root
        if (IsAssemblyRelatedFile(fileName))
        {
            return fileName;
        }

        return null;
    }

    /// <summary>
    /// Check if a file needs to be copied based on version, architecture, and date.
    /// </summary>
    public static bool IsCopyRequired(Architecture environment, string sourceFilePath, string targetFilePath)
    {
        if (!File.Exists(targetFilePath))
        {
            return IsArchitectureCompatible(sourceFilePath, environment);
        }

        var result = new FileCompareResult
        {
            NewFile = false,
        };

        CompareDates(sourceFilePath, targetFilePath, result);
        CompareVersions(sourceFilePath, targetFilePath, result);
        CompareArchitecture(sourceFilePath, targetFilePath, environment, result);

        return result.NewVersion && result.SameOrNewArchitecture ||
               result.NewArchitecture && result.SameOrNewVersion ||
               result.NewDate && result.SameOrNewArchitecture && result.SameOrNewVersion;
    }

    private static void CopyFile(Architecture environment, string sourceFilePath, string targetFilePath)
    {
        if (!File.Exists(targetFilePath))
        {
            if (!IsArchitectureCompatible(sourceFilePath, environment))
            {
                return;
            }
        }
        else
        {
            var result = new FileCompareResult();
            CompareDates(sourceFilePath, targetFilePath, result);
            CompareVersions(sourceFilePath, targetFilePath, result);
            CompareArchitecture(sourceFilePath, targetFilePath, environment, result);

            var shouldCopy = result.NewVersion && result.SameOrNewArchitecture ||
                             result.NewArchitecture && result.SameOrNewVersion ||
                             result.NewDate && result.SameOrNewArchitecture && result.SameOrNewVersion;

            if (!shouldCopy)
            {
                return;
            }
        }

        var targetDir = Path.GetDirectoryName(targetFilePath);
        if (targetDir != null && !Directory.Exists(targetDir))
        {
            Directory.CreateDirectory(targetDir);
        }

        try
        {
            File.Copy(sourceFilePath, targetFilePath, true);
        }
        catch (IOException)
        {
            // Another process may be copying the same file
            ModuleLogger.CreateLogger(typeof(ModuleCopier)).LogWarning("Could not copy {FileName} (file in use)", Path.GetFileName(sourceFilePath));
        }
    }

    private static bool IsArchitectureCompatible(string filePath, Architecture environment)
    {
        var arch = GetArchitecture(filePath);
        if (arch == null)
        {
            return true;
        }

        return arch == environment || (arch == Architecture.X86 && environment == Architecture.X64);
    }

    private static bool IsReferenceDirectory(string relativeFilePath)
    {
        var dirName = GetLastDirectoryName(relativeFilePath);
        return _referenceAssemblyFolders.Any(f => string.Equals(f, dirName, StringComparison.OrdinalIgnoreCase));
    }

    private static bool IsLocalizationFile(string fileName)
    {
        return _localizationExtensions.Any(ext => fileName.EndsWith(ext, StringComparison.OrdinalIgnoreCase));
    }

    private static bool IsAssemblyRelatedFile(string fileName)
    {
        return _assemblyExtensions.Any(ext => fileName.EndsWith(ext, StringComparison.OrdinalIgnoreCase)) ||
               _serviceExtensions.Any(ext => fileName.EndsWith(ext, StringComparison.OrdinalIgnoreCase));
    }

    private static string GetLastDirectoryName(string relativeFilePath)
    {
        return Path.GetFileName(Path.GetDirectoryName(relativeFilePath));
    }

    private static void CompareDates(string sourceFilePath, string targetFilePath, FileCompareResult result)
    {
        var sourceDate = File.Exists(sourceFilePath) ? File.GetLastWriteTimeUtc(sourceFilePath) : (DateTime?)null;
        var targetDate = File.Exists(targetFilePath) ? File.GetLastWriteTimeUtc(targetFilePath) : (DateTime?)null;
        result.NewDate = sourceDate > targetDate;
    }

    private static void CompareVersions(string sourceFilePath, string targetFilePath, FileCompareResult result)
    {
        var sourceVersion = GetVersion(sourceFilePath);
        var targetVersion = GetVersion(targetFilePath);
        result.SameVersion = sourceVersion == targetVersion;
        result.NewVersion = targetVersion is not null && sourceVersion > targetVersion;
    }

    private static void CompareArchitecture(string sourceFilePath, string targetFilePath, Architecture environment, FileCompareResult result)
    {
        var sourceArch = GetArchitecture(sourceFilePath);
        var targetArch = GetArchitecture(targetFilePath);

        result.CompatibleArchitecture = sourceArch == targetArch ||
                                        sourceArch == environment ||
                                        (sourceArch == Architecture.X86 && environment == Architecture.X64);

        if (result.CompatibleArchitecture)
        {
            result.SameArchitecture = sourceArch == targetArch;
            result.NewArchitecture = sourceArch == environment && targetArch != environment;
        }
    }

    private static Version GetVersion(string filePath)
    {
        if (!File.Exists(filePath))
        {
            return null;
        }

        try
        {
            var info = FileVersionInfo.GetVersionInfo(filePath);
            return new Version(info.FileMajorPart, info.FileMinorPart, info.FileBuildPart, info.FilePrivatePart);
        }
        catch
        {
            return null;
        }
    }

    private static Architecture? GetArchitecture(string filePath)
    {
        if (!_assemblyExtensions.Any(ext => filePath.EndsWith(ext, StringComparison.OrdinalIgnoreCase)))
        {
            return null;
        }

        var fileInfo = new FileInfo(filePath);
        if (!fileInfo.Exists || fileInfo.Length < 0x3C + sizeof(uint))
        {
            return null;
        }

        try
        {
            using var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            using var reader = new BinaryReader(stream);

            stream.Seek(0x3C, SeekOrigin.Begin);
            var peOffset = reader.ReadUInt32();

            if (fileInfo.Length < peOffset + sizeof(uint) + sizeof(ushort))
            {
                return null;
            }

            stream.Seek(peOffset, SeekOrigin.Begin);
            if (reader.ReadUInt32() != 0x00004550) // PE signature
            {
                return null;
            }

            return reader.ReadUInt16() switch
            {
                0x8664 => Architecture.X64,
                0xAA64 => Architecture.Arm64,
                0x1C0 => Architecture.Arm,
                0x14C => Architecture.X86,
                _ => null
            };
        }
        catch
        {
            return null;
        }
    }
}
