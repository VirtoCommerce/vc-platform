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
        if (result.NoTarget == AssemblyCopyRequirement.Prohibited)
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
        reasons.NoTarget = !fileInfo.Exists ? AssemblyCopyRequirement.Required : AssemblyCopyRequirement.Prohibited;
    }

    private void SetIsVersion(CopyAssemblyRequirementResult reasons, string sourcePath, string targetPath)
    {
        var sourceVersion = _libraryVersionProvider.GetVersion(sourcePath);
        var targetVersion = _libraryVersionProvider.GetVersion(targetPath);
        if (sourceVersion >= targetVersion)
        {
            reasons.IsVersion = sourceVersion > targetVersion ? AssemblyCopyRequirement.Required : AssemblyCopyRequirement.Unknown;
        }
        else
        {
            reasons.IsVersion = AssemblyCopyRequirement.Prohibited;
        }
    }

    private void SetIsBitness(CopyAssemblyRequirementResult reasons, Architecture architecture, string sourceFilePath, string targetFilePath)
    {
        if (reasons.IsVersion == AssemblyCopyRequirement.Unknown)
        {
            reasons.IsBitness = ReplaceBitwiseReason(architecture, sourceFilePath, targetFilePath);
        }
    }

    private void SetIsSourceNewByDate(CopyAssemblyRequirementResult reasons, string sourcePath, string targetPath)
    {
        if (reasons.IsVersion == AssemblyCopyRequirement.Unknown && reasons.IsBitness == AssemblyCopyRequirement.Unknown)
        {
            var sourceFile = _fileSystem.FileInfo.New(sourcePath);
            var targetFile = _fileSystem.FileInfo.New(targetPath);

            reasons.IsSourceNewByDate = targetFile.LastWriteTimeUtc < sourceFile.LastWriteTimeUtc
                ? AssemblyCopyRequirement.Required
                : AssemblyCopyRequirement.Prohibited;
        }
    }


    private AssemblyCopyRequirement ReplaceBitwiseReason(Architecture environment, string sourceFilePath, string targetFilePath)
    {
        if (_libraryVersionProvider.IsManaged(targetFilePath)
            || !sourceFilePath.EndsWith(".dll", StringComparison.InvariantCultureIgnoreCase))
        {
            return AssemblyCopyRequirement.Unknown;
        }

        var targetDllArchitecture = _libraryVersionProvider.GetArchitecture(targetFilePath);
        var sourceDllArchitecture = _libraryVersionProvider.GetArchitecture(sourceFilePath);

        if (environment == targetDllArchitecture && environment != sourceDllArchitecture)
        {
            return AssemblyCopyRequirement.Prohibited;
        }
        if (environment == sourceDllArchitecture && environment != targetDllArchitecture)
        {
            return AssemblyCopyRequirement.Required;
        }

        return AssemblyCopyRequirement.Unknown;
    }
}
