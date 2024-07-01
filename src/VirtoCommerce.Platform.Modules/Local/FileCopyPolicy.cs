using System.Runtime.InteropServices;
using VirtoCommerce.Platform.Core.Modularity;

namespace VirtoCommerce.Platform.Modules.Local;

public class FileCopyPolicy : IFileCopyPolicy
{
    private readonly IFileMetadataProvider _metadataProvider;

    public FileCopyPolicy(IFileMetadataProvider metadataProvider)
    {
        _metadataProvider = metadataProvider;
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
