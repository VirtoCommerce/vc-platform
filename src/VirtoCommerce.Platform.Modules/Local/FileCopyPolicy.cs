using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using Microsoft.Extensions.Options;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Modularity;
using VirtoCommerce.Platform.Modules.AssemblyLoading;

namespace VirtoCommerce.Platform.Modules.Local;

public class FileCopyPolicy : IFileCopyPolicy
{
    private readonly IFileMetadataProvider _metadataProvider;
    private readonly LocalStorageModuleCatalogOptions _options;
    private readonly string _runtimesDirectory = "runtimes" + Path.DirectorySeparatorChar;

    public FileCopyPolicy(IFileMetadataProvider metadataProvider, IOptions<LocalStorageModuleCatalogOptions> options)
    {
        _metadataProvider = metadataProvider;
        _options = options.Value;
    }

    public string GetTargetRelativePath(string sourceRelativeFilePath)
    {
        var fileName = Path.GetFileName(sourceRelativeFilePath);

        if (IsTpaFile(fileName) || IsReferenceDirectory(sourceRelativeFilePath))
        {
            return null;
        }

        if (IsRuntimesDirectory(sourceRelativeFilePath))
        {
            return sourceRelativeFilePath;
        }

        if (IsLocalizationFile(fileName))
        {
            return Path.Combine(GetLastDirectoryName(sourceRelativeFilePath), fileName);
        }

        if (IsAssemblyRelatedFile(fileName))
        {
            return fileName;
        }

        return null;
    }

    private static bool IsTpaFile(string fileName)
    {
        return Tpa.ContainsAssembly(fileName);
    }

    private bool IsReferenceDirectory(string relativeFilePath)
    {
        // Workaround to avoid loading Reference Assemblies
        // We need to rewrite platform initialization code
        // to use correct solution with MetadataLoadContext
        // TODO: PT-6241
        var directoryName = GetLastDirectoryName(relativeFilePath);
        return _options.ReferenceAssemblyFolders.ContainsIgnoreCase(directoryName);
    }

    private bool IsRuntimesDirectory(string relativeFilePath)
    {
        return relativeFilePath.StartsWithIgnoreCase(_runtimesDirectory);
    }

    private bool IsLocalizationFile(string fileName)
    {
        return _options.LocalizationFileExtensions.Any(fileName.EndsWithIgnoreCase);
    }

    private bool IsAssemblyRelatedFile(string fileName)
    {
        return _options.AssemblyFileExtensions.Any(fileName.EndsWithIgnoreCase) ||
               _options.AssemblyServiceFileExtensions.Any(fileName.EndsWithIgnoreCase);
    }

    private static string GetLastDirectoryName(string relativeFilePath)
    {
        return Path.GetFileName(Path.GetDirectoryName(relativeFilePath));
    }

    public bool IsCopyRequired(Architecture environment, string sourceFilePath, string targetFilePath, out FileCompareResult result)
    {
        result = new FileCompareResult
        {
            NewFile = !_metadataProvider.Exists(targetFilePath),
        };

        CompareDates(sourceFilePath, targetFilePath, result);
        CompareVersions(sourceFilePath, targetFilePath, result);
        CompareArchitecture(sourceFilePath, targetFilePath, environment, result);

        return result.NewFile && result.CompatibleArchitecture ||
               result.NewVersion && result.SameOrNewArchitecture ||
               result.NewArchitecture && result.SameOrNewVersion ||
               result.NewDate && result.SameOrNewArchitecture && result.SameOrNewVersion;
    }

    private void CompareDates(string sourceFilePath, string targetFilePath, FileCompareResult result)
    {
        var sourceDate = _metadataProvider.GetDate(sourceFilePath);
        var targetDate = _metadataProvider.GetDate(targetFilePath);

        result.NewDate = sourceDate > targetDate;
    }

    private void CompareVersions(string sourceFilePath, string targetFilePath, FileCompareResult result)
    {
        var sourceVersion = _metadataProvider.GetVersion(sourceFilePath);
        var targetVersion = _metadataProvider.GetVersion(targetFilePath);

        result.SameVersion = sourceVersion == targetVersion;
        result.NewVersion = targetVersion is not null && sourceVersion > targetVersion;
    }

    private void CompareArchitecture(string sourceFilePath, string targetFilePath, Architecture environment, FileCompareResult result)
    {
        var sourceArchitecture = _metadataProvider.GetArchitecture(sourceFilePath);
        var targetArchitecture = _metadataProvider.GetArchitecture(targetFilePath);

        result.CompatibleArchitecture = sourceArchitecture == targetArchitecture ||
                                        sourceArchitecture == environment ||
                                        sourceArchitecture == Architecture.X86 && environment == Architecture.X64;

        if (result.CompatibleArchitecture)
        {
            result.SameArchitecture = sourceArchitecture == targetArchitecture;
            result.NewArchitecture = sourceArchitecture == environment && targetArchitecture != environment;
        }
    }
}
