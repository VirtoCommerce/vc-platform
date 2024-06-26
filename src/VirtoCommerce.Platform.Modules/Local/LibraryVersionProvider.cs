using System;
using System.Diagnostics;
using System.IO;
using System.IO.Abstractions;
using System.Reflection;
using System.Runtime.InteropServices;
using VirtoCommerce.Platform.Core.Modularity;

namespace VirtoCommerce.Platform.Modules.Local;

public class LibraryVersionProvider : ILibraryVersionProvider
{
    private readonly IFileSystem _fileSystem;

    public LibraryVersionProvider(IFileSystem fileSystem)
    {
        _fileSystem = fileSystem;
    }

    public Version GetFileVersion(string filePath)
    {
        var fileVersionInfo = FileVersionInfo.GetVersionInfo(filePath);
        var version = new Version(fileVersionInfo.FileMajorPart, fileVersionInfo.FileMinorPart,
            fileVersionInfo.FileBuildPart, fileVersionInfo.FilePrivatePart);
        return version;
    }

    public Architecture? GetArchitecture(string filePath)
    {
        using var fs = _fileSystem.FileStream.New(filePath, FileMode.Open, FileAccess.Read);
        using var br = new BinaryReader(fs);

        const int startPosition = 0x3C;

        fs.Seek(startPosition, SeekOrigin.Begin);
        var peOffset = br.ReadInt32();
        fs.Seek(peOffset, SeekOrigin.Begin);
        var peHead = br.ReadUInt32();
        const int peSignature = 0x00004550;
        if (peHead != peSignature)
        {
            return null;
        }

        var machineType = br.ReadUInt16();

        // https://stackoverflow.com/questions/480696/how-to-find-if-a-native-dll-file-is-compiled-as-x64-or-x86
        Architecture? archType = machineType switch
        {
            0x8664 => Architecture.X64,
            0xAA64 => Architecture.Arm64,
            0x1C0 => Architecture.Arm,
            0x14C => Architecture.X86,
            _ => null
        };

        return archType;
    }

    public bool IsManagedLibrary(string filePath)
    {
        try
        {
            AssemblyName.GetAssemblyName(filePath);
            return true;
        }
        catch
        {
            // file is unmanaged
        }

        return false;
    }
}
