using System;
using System.IO.Abstractions;
using System.Runtime.InteropServices;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Modularity;

namespace VirtoCommerce.Platform.Modules.Local;

public class CopyFilePolicy : ICopyFilePolicy
{
    private readonly IFileSystem _fileSystem;
    private readonly ILibraryVersionProvider _libraryVersionProvider;

    public CopyFilePolicy(IFileSystem fileSystem, ILibraryVersionProvider libraryVersionProvider)
    {
        _fileSystem = fileSystem;
        _libraryVersionProvider = libraryVersionProvider;
    }

    public RequireCopyFileReason RequireCopy(Architecture architecture, string sourceFilePath, string targetFilePath)
    {
        var result = AbstractTypeFactory<RequireCopyFileReason>.TryCreateInstance();
        NoTarget(result, targetFilePath);
        if (!result.NoTarget)
        {
            Version(result, sourceFilePath, targetFilePath);
            Bitwise(result, architecture, sourceFilePath, targetFilePath);
            LaterDate(result, sourceFilePath, targetFilePath);
        }

        return result;
    }

    private void NoTarget(RequireCopyFileReason reasons, string targetPath)
    {
        var fileInfo = _fileSystem.FileInfo.New(targetPath);
        reasons.NoTarget = !fileInfo.Exists;
    }

    private void Version(RequireCopyFileReason reasons, string sourcePath, string targetPath)
    {
        var sourceVersion = _libraryVersionProvider.GetFileVersion(sourcePath);
        var targetVersion = _libraryVersionProvider.GetFileVersion(targetPath);
        if (sourceVersion >= targetVersion)
        {
            reasons.Version = sourceVersion > targetVersion;
        }
    }

    public void LaterDate(RequireCopyFileReason reasons, string sourcePath, string targetPath)
    {
        if (reasons.Version == false && reasons.Bitwise == null)
        {
            var sourceFile = _fileSystem.FileInfo.New(sourcePath);
            var targetFile = _fileSystem.FileInfo.New(targetPath);

            reasons.LaterDate = targetFile.LastWriteTimeUtc < sourceFile.LastWriteTimeUtc;
        }
    }

    public void Bitwise(RequireCopyFileReason reasons, Architecture architecture, string sourceFilePath, string targetFilePath)
    {
        if (reasons.Version == false)
        {
            reasons.Bitwise = ReplaceBitwiseReason(architecture, sourceFilePath, targetFilePath);
        }
    }

    private bool? ReplaceBitwiseReason(Architecture environment, string sourceFilePath, string targetFilePath)
    {
        if (_libraryVersionProvider.IsManagedLibrary(targetFilePath)
            || !sourceFilePath.EndsWith(".dll", StringComparison.InvariantCultureIgnoreCase))
        {
            return null;
        }

        var targetDllArchitecture = _libraryVersionProvider.GetArchitecture(targetFilePath);
        var sourceDllArchitecture = _libraryVersionProvider.GetArchitecture(sourceFilePath);

        if (environment == targetDllArchitecture && environment != sourceDllArchitecture)
        {
            return false;
        }
        if (environment == sourceDllArchitecture && environment != targetDllArchitecture)
        {
            return true;
        }

        return null;
    }
}
