using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using Microsoft.Extensions.Options;
using VirtoCommerce.Platform.Core.Modularity;

namespace VirtoCommerce.Platform.Modules.Local;

public class FileMetadataProvider : IFileMetadataProvider
{
    private readonly LocalStorageModuleCatalogOptions _options;

    public FileMetadataProvider(IOptions<LocalStorageModuleCatalogOptions> options)
    {
        _options = options.Value;
    }

    public bool Exists(string filePath)
    {
        return File.Exists(filePath);
    }

    public DateTime? GetDate(string filePath)
    {
        if (!File.Exists(filePath))
        {
            return null;
        }

        var fileInfo = new FileInfo(filePath);

        return fileInfo.LastWriteTimeUtc;
    }

    public Version GetVersion(string filePath)
    {
        if (!File.Exists(filePath))
        {
            return null;
        }

        var fileVersionInfo = FileVersionInfo.GetVersionInfo(filePath);

        return new Version(
            fileVersionInfo.FileMajorPart,
            fileVersionInfo.FileMinorPart,
            fileVersionInfo.FileBuildPart,
            fileVersionInfo.FilePrivatePart);
    }

    public Architecture? GetArchitecture(string filePath)
    {
        if (!_options.AssemblyFileExtensions.Any(x => filePath.EndsWith(x, StringComparison.OrdinalIgnoreCase)))
        {
            return null;
        }

        const int startPosition = 0x3C;
        const int peSignature = 0x00004550;

        var fileInfo = new FileInfo(filePath);
        if (!fileInfo.Exists || fileInfo.Length < startPosition + sizeof(uint))
        {
            return null;
        }

        using var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
        using var reader = new BinaryReader(stream);

        stream.Seek(startPosition, SeekOrigin.Begin);
        var peOffset = reader.ReadUInt32();

        if (fileInfo.Length < peOffset + sizeof(uint) + sizeof(ushort))
        {
            return null;
        }

        stream.Seek(peOffset, SeekOrigin.Begin);
        var peHead = reader.ReadUInt32();

        if (peHead != peSignature)
        {
            return null;
        }

        var machineType = reader.ReadUInt16();

        // https://stackoverflow.com/questions/480696/how-to-find-if-a-native-dll-file-is-compiled-as-x64-or-x86
        return machineType switch
        {
            0x8664 => Architecture.X64,
            0xAA64 => Architecture.Arm64,
            0x1C0 => Architecture.Arm,
            0x14C => Architecture.X86,
            _ => null
        };
    }
}
