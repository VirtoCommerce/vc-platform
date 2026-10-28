using System.IO;
using VirtoCommerce.Platform.Modules.Local;
using Xunit;

namespace VirtoCommerce.Platform.Tests.Modularity;

public class FileMetadataProviderTests
{
    private readonly FileMetadataProvider _metadataProvider = new();

    [Fact]
    public void AnyCpuAssemblyHasNoArchitecture()
    {
        // Arrange
        // VirtoCommerce.Platform.Modules is built as AnyCPU, like every module assembly.
        // Such assemblies carry the X86 machine type (0x14C) in the PE header on every platform,
        // so the machine type alone must not be reported as their architecture.
        var filePath = typeof(FileCopyPolicy).Assembly.Location;

        // Act
        var architecture = _metadataProvider.GetArchitecture(filePath);

        // Assert
        Assert.Null(architecture);
    }

    [Fact]
    public void MissingFileHasNoArchitecture()
    {
        // Arrange
        var filePath = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName() + ".dll");

        // Act
        var architecture = _metadataProvider.GetArchitecture(filePath);

        // Assert
        Assert.Null(architecture);
    }

    [Fact]
    public void FileWhichIsNotPortableExecutableHasNoArchitecture()
    {
        // Arrange
        var filePath = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName() + ".dll");
        File.WriteAllText(filePath, "This is not a portable executable file.");

        try
        {
            // Act
            var architecture = _metadataProvider.GetArchitecture(filePath);

            // Assert
            Assert.Null(architecture);
        }
        finally
        {
            File.Delete(filePath);
        }
    }
}
