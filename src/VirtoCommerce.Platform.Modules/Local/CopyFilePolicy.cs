using System;
using System.IO.Abstractions;
using System.Runtime.InteropServices;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Modularity;

namespace VirtoCommerce.Platform.Modules.Local;

public class CopyFileRequirementValidator : ICopyFileRequirementValidator
{
    private readonly IFileSystem _fileSystem;
    private readonly IAssemblyMetadataProvider _libraryVersionProvider;

    public CopyFileRequirementValidator(IFileSystem fileSystem, IAssemblyMetadataProvider libraryVersionProvider)
    {
        _fileSystem = fileSystem;
        _libraryVersionProvider = libraryVersionProvider;
    }

    public CopyFileRequirementResult RequireCopy(Architecture architecture, string sourceFilePath, string targetFilePath)
    {
        var result = AbstractTypeFactory<CopyFileRequirementResult>.TryCreateInstance();
        SetNoTarget(result, targetFilePath);
        if (result.NoTarget == CopyFileNecessary.No)
        {
            SetIsVersion(result, sourceFilePath, targetFilePath);
            SetIsBitness(result, architecture, sourceFilePath, targetFilePath);
            SetIsSourceNewByDate(result, sourceFilePath, targetFilePath);
        }

        return result;
    }

    private void SetNoTarget(CopyFileRequirementResult reasons, string targetPath)
    {
        var fileInfo = _fileSystem.FileInfo.New(targetPath);
        reasons.NoTarget = !fileInfo.Exists ? CopyFileNecessary.Yes : CopyFileNecessary.No;
    }

    private void SetIsVersion(CopyFileRequirementResult reasons, string sourcePath, string targetPath)
    {
        var sourceVersion = _libraryVersionProvider.GetVersion(sourcePath);
        var targetVersion = _libraryVersionProvider.GetVersion(targetPath);
        if (sourceVersion >= targetVersion)
        {
            reasons.IsVersion = sourceVersion > targetVersion ? CopyFileNecessary.Yes : CopyFileNecessary.Unknown;
        }
        else
        {
            reasons.IsVersion = CopyFileNecessary.No;
        }
    }

    private void SetIsBitness(CopyFileRequirementResult reasons, Architecture architecture, string sourceFilePath, string targetFilePath)
    {
        if (reasons.IsVersion == CopyFileNecessary.Unknown)
        {
            reasons.IsBitness = ReplaceBitwiseReason(architecture, sourceFilePath, targetFilePath);
        }
    }

    private void SetIsSourceNewByDate(CopyFileRequirementResult reasons, string sourcePath, string targetPath)
    {
        if (reasons.IsVersion == CopyFileNecessary.Unknown && reasons.IsBitness == CopyFileNecessary.Unknown)
        {
            var sourceFile = _fileSystem.FileInfo.New(sourcePath);
            var targetFile = _fileSystem.FileInfo.New(targetPath);

            reasons.IsSourceNewByDate = targetFile.LastWriteTimeUtc < sourceFile.LastWriteTimeUtc
                ? CopyFileNecessary.Yes
                : CopyFileNecessary.No;
        }
    }


    private CopyFileNecessary ReplaceBitwiseReason(Architecture environment, string sourceFilePath, string targetFilePath)
    {
        if (_libraryVersionProvider.IsManaged(targetFilePath)
            || !sourceFilePath.EndsWith(".dll", StringComparison.InvariantCultureIgnoreCase))
        {
            return CopyFileNecessary.Unknown;
        }

        var targetDllArchitecture = _libraryVersionProvider.GetArchitecture(targetFilePath);
        var sourceDllArchitecture = _libraryVersionProvider.GetArchitecture(sourceFilePath);

        if (environment == targetDllArchitecture && environment != sourceDllArchitecture)
        {
            return CopyFileNecessary.No;
        }
        if (environment == sourceDllArchitecture && environment != targetDllArchitecture)
        {
            return CopyFileNecessary.Yes;
        }

        return CopyFileNecessary.Unknown;
    }
}
