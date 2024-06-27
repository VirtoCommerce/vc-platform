using System;
using System.IO.Abstractions;
using System.Runtime.InteropServices;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Modularity;

namespace VirtoCommerce.Platform.Modules.Local;

public class CopyAssemblyRequirementValidator : ICopyAssemblyRequirementValidator
{
    private readonly IFileSystem _fileSystem;
    private readonly IAssemblyMetadataProvider _libraryVersionProvider;

    public CopyAssemblyRequirementValidator(IFileSystem fileSystem, IAssemblyMetadataProvider libraryVersionProvider)
    {
        _fileSystem = fileSystem;
        _libraryVersionProvider = libraryVersionProvider;
    }

    public CopyAssemblyRequirementResult RequireCopy(Architecture architecture, string sourceFilePath, string targetFilePath)
    {
        var result = AbstractTypeFactory<CopyAssemblyRequirementResult>.TryCreateInstance();
        SetNoTarget(result, targetFilePath);
        if (result.NoTarget == CopyAssemblyNecessity.No)
        {
            SetIsVersion(result, sourceFilePath, targetFilePath);
            SetIsBitness(result, architecture, sourceFilePath, targetFilePath);
            SetIsSourceNewByDate(result, sourceFilePath, targetFilePath);
        }

        return result;
    }

    private void SetNoTarget(CopyAssemblyRequirementResult reasons, string targetPath)
    {
        var fileInfo = _fileSystem.FileInfo.New(targetPath);
        reasons.NoTarget = !fileInfo.Exists ? CopyAssemblyNecessity.Yes : CopyAssemblyNecessity.No;
    }

    private void SetIsVersion(CopyAssemblyRequirementResult reasons, string sourcePath, string targetPath)
    {
        var sourceVersion = _libraryVersionProvider.GetVersion(sourcePath);
        var targetVersion = _libraryVersionProvider.GetVersion(targetPath);
        if (sourceVersion >= targetVersion)
        {
            reasons.IsVersion = sourceVersion > targetVersion ? CopyAssemblyNecessity.Yes : CopyAssemblyNecessity.Unknown;
        }
        else
        {
            reasons.IsVersion = CopyAssemblyNecessity.No;
        }
    }

    private void SetIsBitness(CopyAssemblyRequirementResult reasons, Architecture architecture, string sourceFilePath, string targetFilePath)
    {
        if (reasons.IsVersion == CopyAssemblyNecessity.Unknown)
        {
            reasons.IsBitness = ReplaceBitwiseReason(architecture, sourceFilePath, targetFilePath);
        }
    }

    private void SetIsSourceNewByDate(CopyAssemblyRequirementResult reasons, string sourcePath, string targetPath)
    {
        if (reasons.IsVersion == CopyAssemblyNecessity.Unknown && reasons.IsBitness == CopyAssemblyNecessity.Unknown)
        {
            var sourceFile = _fileSystem.FileInfo.New(sourcePath);
            var targetFile = _fileSystem.FileInfo.New(targetPath);

            reasons.IsSourceNewByDate = targetFile.LastWriteTimeUtc < sourceFile.LastWriteTimeUtc
                ? CopyAssemblyNecessity.Yes
                : CopyAssemblyNecessity.No;
        }
    }


    private CopyAssemblyNecessity ReplaceBitwiseReason(Architecture environment, string sourceFilePath, string targetFilePath)
    {
        if (_libraryVersionProvider.IsManaged(targetFilePath)
            || !sourceFilePath.EndsWith(".dll", StringComparison.InvariantCultureIgnoreCase))
        {
            return CopyAssemblyNecessity.Unknown;
        }

        var targetDllArchitecture = _libraryVersionProvider.GetArchitecture(targetFilePath);
        var sourceDllArchitecture = _libraryVersionProvider.GetArchitecture(sourceFilePath);

        if (environment == targetDllArchitecture && environment != sourceDllArchitecture)
        {
            return CopyAssemblyNecessity.No;
        }
        if (environment == sourceDllArchitecture && environment != targetDllArchitecture)
        {
            return CopyAssemblyNecessity.Yes;
        }

        return CopyAssemblyNecessity.Unknown;
    }
}
