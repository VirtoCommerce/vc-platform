using System;
using System.IO;
using Microsoft.Extensions.Logging.Abstractions;
using VirtoCommerce.Platform.Core.Modularity;
using VirtoCommerce.Platform.Modules;
using Xunit;

namespace VirtoCommerce.Platform.Tests.Modularity;

public class ModuleManifestReaderTests : IDisposable
{
    private readonly string _discoveryPath;

    public ModuleManifestReaderTests()
    {
        _discoveryPath = Path.Combine(Path.GetTempPath(), nameof(ModuleManifestReaderTests) + Path.GetRandomFileName());
        Directory.CreateDirectory(_discoveryPath);
    }

    public void Dispose()
    {
        Directory.Delete(_discoveryPath, true);
        GC.SuppressFinalize(this);
    }

    [Fact]
    public void ReadAll_NonExistentDirectory_ReturnsEmpty()
    {
        // Arrange
        var bootstrapper = CreateBootstrapper("/nonexistent/path/modules");

        // Act
        var result = bootstrapper.ReadLocalManifests();

        // Assert
        Assert.Empty(result);
    }

    [Fact]
    public void ReadAll_EmptyDirectory_ReturnsEmpty()
    {
        // Arrange
        var bootstrapper = CreateBootstrapper(_discoveryPath);

        // Act
        var result = bootstrapper.ReadLocalManifests();

        // Assert
        Assert.Empty(result);
    }

    [Fact]
    public void ReadAll_WithValidManifest_ReturnsModuleInfo()
    {
        // Arrange
        const string manifestContent =
            """
            <?xml version="1.0" encoding="utf-8"?>
            <module>
              <id>TestModule</id>
              <version>1.2.3</version>
              <platformVersion>3.0.0</platformVersion>
              <title>Test Module</title>
              <description>A test module</description>
            </module>
            """;

        SaveManifest(manifestContent, "Test.Module");
        var bootstrapper = CreateBootstrapper(_discoveryPath);

        // Act
        var result = bootstrapper.ReadLocalManifests();

        // Assert
        Assert.Single(result);
        Assert.Equal("TestModule", result[0].Id);
        Assert.Equal("1.2.3", result[0].Version.ToString());
        Assert.True(result[0].IsInstalled);
    }

    [Fact]
    public void ReadAll_KeepDuplicates()
    {
        // Arrange
        const string manifestContent =
            """
            <?xml version="1.0" encoding="utf-8"?>
            <module>
              <id>TestModule</id>
              <version>1.0.0</version>
              <platformVersion>3.0.0</platformVersion>
            </module>
            """;

        SaveManifest(manifestContent, "Module1");
        SaveManifest(manifestContent, "Module2");
        var bootstrapper = CreateBootstrapper(_discoveryPath);

        // Act
        var result = bootstrapper.ReadLocalManifests();

        // Assert
        Assert.Equal(2, result.Count);
    }

    [Fact]
    public void ReadAll_ExcludesArtifactsDirectory()
    {
        // Arrange
        const string manifestContent =
            """
            <?xml version="1.0" encoding="utf-8"?>
            <module>
              <id>TestModule</id>
              <version>1.0.0</version>
              <platformVersion>3.0.0</platformVersion>
            </module>
            """;

        SaveManifest(manifestContent, "Test.Module");
        SaveManifest(manifestContent, Path.Combine("Test.Module", "artifacts", "publish"));
        var bootstrapper = CreateBootstrapper(_discoveryPath);

        // Act
        var result = bootstrapper.ReadLocalManifests();

        // Assert
        Assert.Single(result);
        Assert.DoesNotContain("artifacts", result[0].FullPhysicalPath);
    }

    [Fact]
    public void ReadAll_WithModuleWithoutAssembly_SetsInitializedState()
    {
        // Arrange
        const string manifestContent =
            """
            <?xml version="1.0" encoding="utf-8"?>
            <module>
              <id>DataModule</id>
              <version>1.0.0</version>
              <platformVersion>3.0.0</platformVersion>
            </module>
            """;

        SaveManifest(manifestContent, "Test.Module");
        var bootstrapper = CreateBootstrapper(_discoveryPath);

        // Act
        var modules = bootstrapper.ReadLocalManifests();

        // Assert
        Assert.Single(modules);
        Assert.Equal(ModuleState.Initialized, modules[0].State);
    }

    [Fact]
    public void Read_SingleManifest_ParsesAllFields()
    {
        // Arrange
        const string manifestContent =
            """
            <?xml version="1.0" encoding="utf-8"?>
            <module>
              <id>VirtoCommerce.Orders</id>
              <version>3.800.0</version>
              <platformVersion>3.800.0</platformVersion>
              <title>Orders Module</title>
              <description>Provides order management</description>
              <assemblyFile>bin/net10.0/VirtoCommerce.OrdersModule.dll</assemblyFile>
              <moduleType>VirtoCommerce.OrdersModule.Web.Module</moduleType>
              <dependencies>
                <dependency id="VirtoCommerce.Core" version="3.800.0" />
                <dependency id="VirtoCommerce.Cart" version="3.800.0" />
              </dependencies>
            </module>
            """;

        var manifestPath = SaveManifest(manifestContent, "Test.Module");
        var bootstrapper = CreateBootstrapper(_discoveryPath);

        // Act
        var module = bootstrapper.ReadManifest(manifestPath);

        // Assert
        Assert.NotNull(module);
        Assert.Equal("VirtoCommerce.Orders", module.Id);
        Assert.Equal("3.800.0", module.Version.ToString());
        Assert.Contains("VirtoCommerce.Core", module.DependsOn);
        Assert.Contains("VirtoCommerce.Cart", module.DependsOn);
        Assert.Equal(2, module.Dependencies.Count);
        Assert.True(module.IsInstalled);
    }


    private static ModuleBootstrapper CreateBootstrapper(string discoveryPath)
    {
        var options = new LocalStorageModuleCatalogOptions { DiscoveryPath = discoveryPath };
        return new ModuleBootstrapper(NullLoggerFactory.Instance, options);
    }

    private string SaveManifest(string manifestContent, string directoryName)
    {
        var modulePath = Path.Combine(_discoveryPath, directoryName);
        Directory.CreateDirectory(modulePath);

        var manifestPath = Path.Combine(modulePath, "module.manifest");
        File.WriteAllText(manifestPath, manifestContent);

        return manifestPath;
    }
}
