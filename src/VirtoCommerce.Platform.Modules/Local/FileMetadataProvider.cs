using System;
using System.Diagnostics;
using System.IO;
using System.Reflection.PortableExecutable;
using System.Runtime.InteropServices;
using Microsoft.Extensions.Options;
using VirtoCommerce.Platform.Core.Modularity;

namespace VirtoCommerce.Platform.Modules.Local;

public class FileMetadataProvider : IFileMetadataProvider
{

    public FileMetadataProvider()
    {
    }

    [Obsolete("Use parameterless constructor.", DiagnosticId = "VC0014", UrlFormat = "https://docs.virtocommerce.org/products/products-virto3-versions")]
    public FileMetadataProvider(IOptions<LocalStorageModuleCatalogOptions> _)
    {
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

    /// <summary>
    /// Returns the processor architecture the file is bound to, or <c>null</c> when it is not bound to any:
    /// either because the file is not a portable executable, or because it is a processor-architecture-neutral
    /// (AnyCPU) managed assembly.
    /// </summary>
    public Architecture? GetArchitecture(string filePath)
    {
        if (!File.Exists(filePath))
        {
            return null;
        }

        try
        {
            using var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            using var peReader = new PEReader(stream);

            var headers = peReader.PEHeaders;

            if (IsArchitectureNeutral(headers.CorHeader))
            {
                return null;
            }

            return headers.CoffHeader.Machine switch
            {
                Machine.Amd64 => Architecture.X64,
                Machine.Arm64 => Architecture.Arm64,
                Machine.Arm or Machine.ArmThumb2 => Architecture.Arm,
                Machine.I386 => Architecture.X86,
                _ => null,
            };
        }
        catch (BadImageFormatException)
        {
            return null;
        }
    }

    /// <summary>
    /// A managed assembly declares the X86 machine type unless it is compiled for a specific architecture,
    /// so the machine type alone would report every AnyCPU assembly as X86. The CLI header tells them apart:
    /// IL-only assemblies are architecture-neutral unless they require a 32-bit process, and even then
    /// "32-bit preferred" only expresses a preference and still runs anywhere.
    /// </summary>
    private static bool IsArchitectureNeutral(CorHeader corHeader)
    {
        if (corHeader is null)
        {
            return false;
        }

        return corHeader.Flags.HasFlag(CorFlags.ILOnly) &&
               (!corHeader.Flags.HasFlag(CorFlags.Requires32Bit) || corHeader.Flags.HasFlag(CorFlags.Prefers32Bit));
    }
}
