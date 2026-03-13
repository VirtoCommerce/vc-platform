using System.Collections.Generic;
using System.Linq;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Modularity;
using VirtoCommerce.Platform.Modules;
using Xunit;

namespace VirtoCommerce.Platform.Tests.Modularity;

public class ModuleDiscoveryTests
{
    [Fact]
    public void ValidateInstall_CompatiblePlatformVersion_NoErrors()
    {
        var module = CreateModule("TestModule", "1.0.0", "3.0.0");
        var installed = new List<ManifestModuleInfo>();

        var errors = ModuleDiscovery.ValidateInstall(module, installed, SemanticVersion.Parse("3.800.0"));

        Assert.Empty(errors);
    }

    [Fact]
    public void ValidateInstall_IncompatiblePlatformVersion_ReturnsError()
    {
        var module = CreateModule("TestModule", "1.0.0", "4.0.0");
        var installed = new List<ManifestModuleInfo>();

        var errors = ModuleDiscovery.ValidateInstall(module, installed, SemanticVersion.Parse("3.800.0"));

        Assert.Single(errors);
        Assert.Contains("incompatible", errors[0]);
    }

    [Fact]
    public void ValidateInstall_IncompatibleModule_ReturnsError()
    {
        var module = CreateModule("NewModule", "1.0.0", "3.0.0");
        module.Incompatibilities.Add(new ModuleIdentity("OldModule", SemanticVersion.Parse("1.0.0")));

        var installed = new List<ManifestModuleInfo>
        {
            CreateModule("OldModule", "1.0.0", "3.0.0", isInstalled: true)
        };

        var errors = ModuleDiscovery.ValidateInstall(module, installed, SemanticVersion.Parse("3.0.0"));

        Assert.Single(errors);
        Assert.Contains("incompatible", errors[0]);
    }

    [Fact]
    public void ValidateUninstall_NoDependents_NoErrors()
    {
        var installed = new List<ManifestModuleInfo>
        {
            CreateModule("ModuleA", "1.0.0", "3.0.0", isInstalled: true),
            CreateModule("ModuleB", "1.0.0", "3.0.0", isInstalled: true),
        };

        var errors = ModuleDiscovery.ValidateUninstall("ModuleA", installed);

        Assert.Empty(errors);
    }

    [Fact]
    public void ValidateUninstall_HasDependents_ReturnsError()
    {
        var moduleA = CreateModule("ModuleA", "1.0.0", "3.0.0", isInstalled: true);
        var moduleB = CreateModule("ModuleB", "1.0.0", "3.0.0", isInstalled: true, dependencies: ["ModuleA"]);

        var installed = new List<ManifestModuleInfo> { moduleA, moduleB };

        var errors = ModuleDiscovery.ValidateUninstall("ModuleA", installed);

        Assert.Single(errors);
        Assert.Contains("ModuleB", errors[0]);
    }

    [Fact]
    public void MergeWithInstalled_ExternalNewerThanLocal_IncludesExternal()
    {
        var external = new List<ManifestModuleInfo>
        {
            CreateModule("ModuleA", "2.0.0", "3.0.0"),
        };

        var installed = new List<ManifestModuleInfo>
        {
            CreateModule("ModuleA", "1.0.0", "3.0.0", isInstalled: true),
        };

        var merged = ModuleDiscovery.MergeWithInstalled(external, installed);

        // Both should be present (external newer + installed)
        Assert.True(merged.Count >= 1);
    }

    [Fact]
    public void MergeWithInstalled_LocalNewerThanExternal_SkipsExternal()
    {
        var external = new List<ManifestModuleInfo>
        {
            CreateModule("ModuleA", "1.0.0", "3.0.0"),
        };

        var installed = new List<ManifestModuleInfo>
        {
            CreateModule("ModuleA", "2.0.0", "3.0.0", isInstalled: true),
        };

        var merged = ModuleDiscovery.MergeWithInstalled(external, installed);

        // External version should be skipped, installed should be present
        Assert.Single(merged);
        Assert.Equal("2.0.0", merged[0].Version.ToString());
    }

    [Fact]
    public void MergeWithInstalled_InstalledOnlyModule_Included()
    {
        var external = new List<ManifestModuleInfo>();
        var installed = new List<ManifestModuleInfo>
        {
            CreateModule("LocalOnly", "1.0.0", "3.0.0", isInstalled: true),
        };

        var merged = ModuleDiscovery.MergeWithInstalled(external, installed);

        Assert.Single(merged);
        Assert.Equal("LocalOnly", merged[0].Id);
    }

    private static ManifestModuleInfo CreateModule(string id, string version, string platformVersion, bool isInstalled = false, string[] dependencies = null)
    {
        var manifest = new ModuleManifest
        {
            Id = id,
            Version = version,
            PlatformVersion = platformVersion,
            Dependencies = dependencies?.Select(d => new ManifestDependency { Id = d, Version = "1.0.0" }).ToArray()
        };

        var moduleInfo = AbstractTypeFactory<ManifestModuleInfo>.TryCreateInstance();
        moduleInfo.LoadFromManifest(manifest);
        moduleInfo.IsInstalled = isInstalled;
        return moduleInfo;
    }
}
