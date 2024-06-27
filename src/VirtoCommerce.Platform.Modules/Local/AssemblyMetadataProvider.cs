using System;
using System.Diagnostics;
using System.IO;
using System.IO.Abstractions;
using System.Reflection;
using System.Runtime.InteropServices;
using VirtoCommerce.Platform.Core.Modularity;

namespace VirtoCommerce.Platform.Modules.Local;

public class AssemblyMetadataProvider : IAssemblyMetadataProvider
{
    private readonly IFileSystem _fileSystem;

    public AssemblyMetadataProvider(IFileSystem fileSystem)
    {
        _fileSystem = fileSystem;
    }

    public Version GetVersion(string filePath)
    {
        var fileVersionInfo = FileVersionInfo.GetVersionInfo(filePath);
        var version = new Version(fileVersionInfo.FileMajorPart, fileVersionInfo.FileMinorPart,
            fileVersionInfo.FileBuildPart, fileVersionInfo.FilePrivatePart);
        return version;
    }

    public Architecture? GetArchitecture(string filePath)
    {
        using var stream = _fileSystem.FileStream.New(filePath, FileMode.Open, FileAccess.Read);
        using var reader = new BinaryReader(stream);

        const int startPosition = 0x3C;

        stream.Seek(startPosition, SeekOrigin.Begin);
        var peOffset = reader.ReadInt32();
        stream.Seek(peOffset, SeekOrigin.Begin);
        var peHead = reader.ReadUInt32();
        const int peSignature = 0x00004550;
        if (peHead != peSignature)
        {
            return null;
        }

        var machineType = reader.ReadUInt16();

        // https://stackoverflow.com/questions/480696/how-to-find-if-a-native-dll-file-is-compiled-as-x64-or-x86
        Architecture? architecture = machineType switch
        {
            0x8664 => Architecture.X64,
            0xAA64 => Architecture.Arm64,
            0x1C0 => Architecture.Arm,
            0x14C => Architecture.X86,
            _ => null
        };

        return architecture;
    }

    public bool IsManaged(string filePath)
    {
        try
        {
            AssemblyName.GetAssemblyName(filePath);
            return true;
        }
        catch
        {
            // file is not managed
        }

        return false;
    }
}
