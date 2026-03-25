using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Modularity;
using VirtoCommerce.Platform.Modules;
using Xunit;

namespace VirtoCommerce.Platform.Tests.Modularity;

public class ModuleRegistryTests
{
    [Fact]
    public void IsInstalled_ExistingModule_ReturnsTrue()
    {
        // Arrange
        ModuleRegistry.Initialize([
            CreateModule("Module1", "1.0.0", isInstalled: true),
        ]);

        // Act, Assert
        Assert.True(ModuleRegistry.IsInstalled("Module1"));
    }

    [Fact]
    public void IsInstalled_UnknownModule_ReturnsFalse()
    {
        // Arrange
        ModuleRegistry.Initialize([
            CreateModule("Module1", "1.0.0", isInstalled: true),
        ]);

        // Act, Assert
        Assert.False(ModuleRegistry.IsInstalled("Unknown"));
    }

    [Fact]
    public void IsInstalled_NotInstalledModule_ReturnsFalse()
    {
        // Arrange
        ModuleRegistry.Initialize([
            CreateModule("Module1", "1.0.0", isInstalled: false),
        ]);

        // Act, Assert
        Assert.False(ModuleRegistry.IsInstalled("Module1"));
    }

    [Fact]
    public void IsInstalled_CaseInsensitive()
    {
        // Arrange
        ModuleRegistry.Initialize([
            CreateModule("Module1", "1.0.0", isInstalled: true),
        ]);

        // Act, Assert
        Assert.True(ModuleRegistry.IsInstalled("module1"));
        Assert.True(ModuleRegistry.IsInstalled("MODULE1"));
    }

    [Fact]
    public void IsInstalled_WithMinVersion_SatisfiedVersion_ReturnsTrue()
    {
        // Arrange
        ModuleRegistry.Initialize([
            CreateModule("Module1", "2.5.0", isInstalled: true),
        ]);

        // Act, Assert
        Assert.True(ModuleRegistry.IsInstalled("Module1", "2.0.0"));
        Assert.True(ModuleRegistry.IsInstalled("Module1", "2.5.0"));
    }

    [Fact]
    public void IsInstalled_WithMinVersion_UnsatisfiedVersion_ReturnsFalse()
    {
        // Arrange
        ModuleRegistry.Initialize([
            CreateModule("Module1", "1.0.0", isInstalled: true),
        ]);

        // Act, Assert
        Assert.False(ModuleRegistry.IsInstalled("Module1", "1.1.0"));
        Assert.False(ModuleRegistry.IsInstalled("Module1", "2.0.0"));
    }

    [Fact]
    public void GetModule_ExistingModule_ReturnsInfo()
    {
        // Arrange
        ModuleRegistry.Initialize([
            CreateModule("Module1", "1.0.0", isInstalled: false),
        ]);

        // Act
        var result = ModuleRegistry.GetModule("Module1");

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Module1", result.Id);
    }

    [Fact]
    public void GetModule_UnknownModule_ReturnsNull()
    {
        // Arrange
        ModuleRegistry.Initialize([
            CreateModule("Module1", "1.0.0", isInstalled: true),
        ]);

        // Act, Assert
        Assert.Null(ModuleRegistry.GetModule("Unknown"));
    }

    [Fact]
    public void GetAllModules_ReturnsAll()
    {
        // Arrange
        ModuleRegistry.Initialize([
            CreateModule("Module1", "1.0.0", isInstalled: true),
            CreateModule("Module2", "2.0.0", isInstalled: false),
        ]);

        // Act, Assert
        Assert.Equal(2, ModuleRegistry.GetAllModules().Count);
    }

    [Fact]
    public void GetInstalledModules_ReturnsOnlyInstalled()
    {
        // Arrange
        ModuleRegistry.Initialize([
            CreateModule("Module1", "1.0.0", isInstalled: true),
            CreateModule("Module2", "2.0.0", isInstalled: false),
            CreateModule("Module3", "3.0.0", isInstalled: true),
        ]);

        // Act
        var installed = ModuleRegistry.GetInstalledModules();

        // Assert
        Assert.Equal(2, installed.Count);
    }

    [Fact]
    public void GetFailedModules_ReturnsOnlyFailed()
    {
        // Arrange
        ModuleRegistry.Initialize([
            CreateModule("Good", "1.0.0", isInstalled: true),
            CreateModule("Bad", "1.0.0", isInstalled: true, error: "Something went wrong"),
        ]);

        // Act
        var failed = ModuleRegistry.GetFailedModules();

        // Assert
        Assert.Single(failed);
        Assert.Equal("Bad", failed[0].Id);
    }


    private static ManifestModuleInfo CreateModule(string id, string version, bool isInstalled, string error = null)
    {
        var manifest = new ModuleManifest
        {
            Id = id,
            Version = version,
            PlatformVersion = "3.0.0",
        };

        var moduleInfo = AbstractTypeFactory<ManifestModuleInfo>.TryCreateInstance();
        moduleInfo.LoadFromManifest(manifest);
        moduleInfo.IsInstalled = isInstalled;

        if (!error.IsNullOrEmpty())
        {
            moduleInfo.Errors.Add(error);
        }

        return moduleInfo;
    }
}
