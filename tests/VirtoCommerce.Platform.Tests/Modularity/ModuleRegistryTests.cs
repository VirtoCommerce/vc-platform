using System.Collections.Generic;
using Microsoft.Extensions.Logging.Abstractions;
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
        var bootstrapper = CreateBootstrapperWithModules([
            CreateModule("Module1", "1.0.0", isInstalled: true),
        ]);

        // Act, Assert
        Assert.True(bootstrapper.IsInstalled("Module1"));
    }

    [Fact]
    public void IsInstalled_UnknownModule_ReturnsFalse()
    {
        // Arrange
        var bootstrapper = CreateBootstrapperWithModules([
            CreateModule("Module1", "1.0.0", isInstalled: true),
        ]);

        // Act, Assert
        Assert.False(bootstrapper.IsInstalled("Unknown"));
    }

    [Fact]
    public void IsInstalled_NotInstalledModule_ReturnsFalse()
    {
        // Arrange
        var bootstrapper = CreateBootstrapperWithModules([
            CreateModule("Module1", "1.0.0", isInstalled: false),
        ]);

        // Act, Assert
        Assert.False(bootstrapper.IsInstalled("Module1"));
    }

    [Fact]
    public void IsInstalled_CaseInsensitive()
    {
        // Arrange
        var bootstrapper = CreateBootstrapperWithModules([
            CreateModule("Module1", "1.0.0", isInstalled: true),
        ]);

        // Act, Assert
        Assert.True(bootstrapper.IsInstalled("module1"));
        Assert.True(bootstrapper.IsInstalled("MODULE1"));
    }

    [Fact]
    public void IsInstalled_WithMinVersion_SatisfiedVersion_ReturnsTrue()
    {
        // Arrange
        var bootstrapper = CreateBootstrapperWithModules([
            CreateModule("Module1", "2.5.0", isInstalled: true),
        ]);

        // Act, Assert
        Assert.True(bootstrapper.IsInstalled("Module1", "2.0.0"));
        Assert.True(bootstrapper.IsInstalled("Module1", "2.5.0"));
    }

    [Fact]
    public void IsInstalled_WithMinVersion_UnsatisfiedVersion_ReturnsFalse()
    {
        // Arrange
        var bootstrapper = CreateBootstrapperWithModules([
            CreateModule("Module1", "1.0.0", isInstalled: true),
        ]);

        // Act, Assert
        Assert.False(bootstrapper.IsInstalled("Module1", "1.1.0"));
        Assert.False(bootstrapper.IsInstalled("Module1", "2.0.0"));
    }

    [Fact]
    public void GetModule_ExistingModule_ReturnsInfo()
    {
        // Arrange
        var bootstrapper = CreateBootstrapperWithModules([
            CreateModule("Module1", "1.0.0", isInstalled: false),
        ]);

        // Act
        var result = bootstrapper.GetModule("Module1");

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Module1", result.Id);
    }

    [Fact]
    public void GetModule_UnknownModule_ReturnsNull()
    {
        // Arrange
        var bootstrapper = CreateBootstrapperWithModules([
            CreateModule("Module1", "1.0.0", isInstalled: true),
        ]);

        // Act, Assert
        Assert.Null(bootstrapper.GetModule("Unknown"));
    }

    [Fact]
    public void GetAllModules_ReturnsAll()
    {
        // Arrange
        var bootstrapper = CreateBootstrapperWithModules([
            CreateModule("Module1", "1.0.0", isInstalled: true),
            CreateModule("Module2", "2.0.0", isInstalled: false),
        ]);

        // Act, Assert
        Assert.Equal(2, bootstrapper.GetModules().Count);
    }

    [Fact]
    public void GetInstalledModules_ReturnsOnlyInstalled()
    {
        // Arrange
        var bootstrapper = CreateBootstrapperWithModules([
            CreateModule("Module1", "1.0.0", isInstalled: true),
            CreateModule("Module2", "2.0.0", isInstalled: false),
            CreateModule("Module3", "3.0.0", isInstalled: true),
        ]);

        // Act
        var installed = bootstrapper.GetInstalledModules();

        // Assert
        Assert.Equal(2, installed.Count);
    }

    [Fact]
    public void GetFailedModules_ReturnsOnlyFailed()
    {
        // Arrange
        var bootstrapper = CreateBootstrapperWithModules([
            CreateModule("Good", "1.0.0", isInstalled: true),
            CreateModule("Bad", "1.0.0", isInstalled: true, error: "Something went wrong"),
        ]);

        // Act
        var failed = bootstrapper.GetFailedModules();

        // Assert
        Assert.Single(failed);
        Assert.Equal("Bad", failed[0].Id);
    }


    private static ModuleBootstrapper CreateBootstrapperWithModules(List<ManifestModuleInfo> modules)
    {
        var bootstrapper = new ModuleBootstrapper(NullLoggerFactory.Instance, new LocalStorageModuleCatalogOptions());
        bootstrapper.RegisterModules(modules);

        return bootstrapper;
    }

    private static ManifestModuleInfo CreateModule(string id, string version, bool isInstalled, string error = null)
    {
        var manifest = new ModuleManifest
        {
            Id = id,
            Version = version,
            PlatformVersion = "3.0.0",
        };

        var module = AbstractTypeFactory<ManifestModuleInfo>.TryCreateInstance();
        module.LoadFromManifest(manifest);
        module.IsInstalled = isInstalled;

        if (!error.IsNullOrEmpty())
        {
            module.Errors.Add(error);
        }

        return module;
    }
}
