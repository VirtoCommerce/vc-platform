using System.IO;
using VirtoCommerce.Platform.Core.Modularity;
using VirtoCommerce.Platform.Modules;
using Xunit;

namespace VirtoCommerce.Platform.Tests.Modularity;

public class ModuleManifestReaderTests
{
    [Fact]
    public void ReadAll_NonExistentDirectory_ReturnsEmpty()
    {
        var result = ModuleManifestReader.ReadAll("/nonexistent/path/modules");

        Assert.Empty(result);
    }

    [Fact]
    public void ReadAll_EmptyDirectory_ReturnsEmpty()
    {
        var tempDir = Path.Combine(Path.GetTempPath(), "moduletest_" + Path.GetRandomFileName());
        Directory.CreateDirectory(tempDir);

        try
        {
            var result = ModuleManifestReader.ReadAll(tempDir);
            Assert.Empty(result);
        }
        finally
        {
            Directory.Delete(tempDir, true);
        }
    }

    [Fact]
    public void ReadAll_WithValidManifest_ReturnsModuleInfo()
    {
        var tempDir = Path.Combine(Path.GetTempPath(), "moduletest_" + Path.GetRandomFileName());
        var moduleDir = Path.Combine(tempDir, "TestModule");
        Directory.CreateDirectory(moduleDir);

        File.WriteAllText(Path.Combine(moduleDir, "module.manifest"), @"<?xml version=""1.0"" encoding=""utf-8""?>
<module>
  <id>TestModule</id>
  <version>1.2.3</version>
  <platformVersion>3.0.0</platformVersion>
  <title>Test Module</title>
  <description>A test module</description>
</module>");

        try
        {
            var result = ModuleManifestReader.ReadAll(tempDir);

            Assert.Single(result);
            Assert.Equal("TestModule", result[0].Id);
            Assert.Equal("1.2.3", result[0].Version.ToString());
            Assert.True(result[0].IsInstalled);
        }
        finally
        {
            Directory.Delete(tempDir, true);
        }
    }

    [Fact]
    public void ReadAll_ExcludesArtifactsDirectory()
    {
        var tempDir = Path.Combine(Path.GetTempPath(), "moduletest_" + Path.GetRandomFileName());
        var moduleDir = Path.Combine(tempDir, "TestModule");
        var artifactsDir = Path.Combine(tempDir, "TestModule", "artifacts", "publish");
        Directory.CreateDirectory(moduleDir);
        Directory.CreateDirectory(artifactsDir);

        var manifestContent = @"<?xml version=""1.0"" encoding=""utf-8""?>
<module>
  <id>TestModule</id>
  <version>1.0.0</version>
  <platformVersion>3.0.0</platformVersion>
</module>";

        File.WriteAllText(Path.Combine(moduleDir, "module.manifest"), manifestContent);
        File.WriteAllText(Path.Combine(artifactsDir, "module.manifest"), manifestContent);

        try
        {
            var result = ModuleManifestReader.ReadAll(tempDir);

            Assert.Single(result); // Only the non-artifacts manifest
        }
        finally
        {
            Directory.Delete(tempDir, true);
        }
    }

    [Fact]
    public void Read_SingleManifest_ParsesAllFields()
    {
        var tempDir = Path.Combine(Path.GetTempPath(), "moduletest_" + Path.GetRandomFileName());
        Directory.CreateDirectory(tempDir);

        var manifestPath = Path.Combine(tempDir, "module.manifest");
        File.WriteAllText(manifestPath, @"<?xml version=""1.0"" encoding=""utf-8""?>
<module>
  <id>VirtoCommerce.Orders</id>
  <version>3.800.0</version>
  <platformVersion>3.800.0</platformVersion>
  <title>Orders Module</title>
  <description>Provides order management</description>
  <assemblyFile>bin/net10.0/VirtoCommerce.OrdersModule.dll</assemblyFile>
  <moduleType>VirtoCommerce.OrdersModule.Web.Module</moduleType>
  <dependencies>
    <dependency id=""VirtoCommerce.Core"" version=""3.800.0"" />
    <dependency id=""VirtoCommerce.Cart"" version=""3.800.0"" />
  </dependencies>
</module>");

        try
        {
            var result = ModuleManifestReader.Read(manifestPath);

            Assert.NotNull(result);
            Assert.Equal("VirtoCommerce.Orders", result.Id);
            Assert.Equal("3.800.0", result.Version.ToString());
            Assert.Contains("VirtoCommerce.Core", result.DependsOn);
            Assert.Contains("VirtoCommerce.Cart", result.DependsOn);
            Assert.Equal(2, result.Dependencies.Count);
            Assert.True(result.IsInstalled);
        }
        finally
        {
            Directory.Delete(tempDir, true);
        }
    }

    [Fact]
    public void ReadAll_WithModuleWithoutAssembly_SetsInitializedState()
    {
        var tempDir = Path.Combine(Path.GetTempPath(), "moduletest_" + Path.GetRandomFileName());
        var moduleDir = Path.Combine(tempDir, "DataModule");
        Directory.CreateDirectory(moduleDir);

        File.WriteAllText(Path.Combine(moduleDir, "module.manifest"), @"<?xml version=""1.0"" encoding=""utf-8""?>
<module>
  <id>DataModule</id>
  <version>1.0.0</version>
  <platformVersion>3.0.0</platformVersion>
</module>");

        try
        {
            var result = ModuleManifestReader.ReadAll(tempDir);

            Assert.Single(result);
            Assert.Equal(ModuleState.Initialized, result[0].State);
        }
        finally
        {
            Directory.Delete(tempDir, true);
        }
    }
}
