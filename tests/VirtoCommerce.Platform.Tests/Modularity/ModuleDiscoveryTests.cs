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
        var platformVersion = SemanticVersion.Parse("3.800.0");
        var module = CreateModule("TestModule", "1.0.0", platformVersion: "3.0.0");
        var installedModules = new List<ManifestModuleInfo>();

        var errors = ModuleDiscovery.ValidateInstall(module, installedModules, platformVersion);

        Assert.Empty(errors);
    }

    [Fact]
    public void ValidateInstall_IncompatiblePlatformVersion_ReturnsError()
    {
        var platformVersion = SemanticVersion.Parse("3.800.0");
        var module = CreateModule("TestModule", "1.0.0", platformVersion: "4.0.0");
        var installedModules = new List<ManifestModuleInfo>();

        var errors = ModuleDiscovery.ValidateInstall(module, installedModules, platformVersion);

        Assert.Single(errors);
        Assert.Equal("Target platform version 4.0.0 is incompatible with current 3.800.0", errors[0]);
    }

    [Fact]
    public void ValidateInstall_IncompatibleModule_ReturnsError()
    {
        var platformVersion = SemanticVersion.Parse("3.800.0");
        var module = CreateModule("NewModule", "1.0.0", platformVersion: "3.0.0");
        module.Incompatibilities.Add(new ModuleIdentity("OldModule", SemanticVersion.Parse("1.0.0")));

        var installedModules = new List<ManifestModuleInfo>
        {
            CreateModule("OldModule", "1.0.0", platformVersion: "3.0.0", isInstalled: true),
        };

        var errors = ModuleDiscovery.ValidateInstall(module, installedModules, platformVersion);

        Assert.Single(errors);
        Assert.Equal("NewModule:1.0.0 is incompatible with installed OldModule:1.0.0", errors[0]);
    }

    [Fact]
    public void ValidateUninstall_NoDependents_NoErrors()
    {
        var installedModules = new List<ManifestModuleInfo>
        {
            CreateModule("ModuleA", "1.0.0", isInstalled: true),
            CreateModule("ModuleB", "1.0.0", isInstalled: true),
        };

        var errors = ModuleDiscovery.ValidateUninstall("ModuleA", installedModules);

        Assert.Empty(errors);
    }

    [Fact]
    public void ValidateUninstall_HasDependents_ReturnsError()
    {
        var moduleA = CreateModule("ModuleA", "1.0.0", isInstalled: true);
        var moduleB = CreateModule("ModuleB", "1.0.0", isInstalled: true, dependencyIds: ["ModuleA"]);

        var installedModules = new List<ManifestModuleInfo> { moduleA, moduleB };

        var errors = ModuleDiscovery.ValidateUninstall("ModuleA", installedModules);

        Assert.Single(errors);
        Assert.Equal("Unable to uninstall 'ModuleA' because 'ModuleB' depends on it", errors[0]);
    }

    [Fact]
    public void MergeWithInstalled_ExternalNewerThanLocal_IncludesExternal()
    {
        var externalModules = new List<ManifestModuleInfo>
        {
            CreateModule("ModuleA", "2.0.0"),
        };

        var installedModules = new List<ManifestModuleInfo>
        {
            CreateModule("ModuleA", "1.0.0", isInstalled: true),
        };

        var merged = ModuleDiscovery.MergeWithInstalled(externalModules, installedModules);

        // Both should be present (external newer + installedModules)
        Assert.Equivalent(externalModules.Concat(installedModules), merged);
    }

    [Fact]
    public void MergeWithInstalled_LocalNewerThanExternal_SkipsExternal()
    {
        var externalModules = new List<ManifestModuleInfo>
        {
            CreateModule("ModuleA", "1.0.0"),
        };

        var installedModules = new List<ManifestModuleInfo>
        {
            CreateModule("ModuleA", "2.0.0", isInstalled: true),
        };

        var merged = ModuleDiscovery.MergeWithInstalled(externalModules, installedModules);

        // External version should be skipped, installedModules should be present
        Assert.Single(merged);
        Assert.Equal("2.0.0", merged[0].Version.ToString());
    }

    [Fact]
    public void MergeWithInstalled_InstalledOnlyModule_Included()
    {
        var externalModules = new List<ManifestModuleInfo>();
        var installedModules = new List<ManifestModuleInfo>
        {
            CreateModule("LocalOnly", "1.0.0", isInstalled: true),
        };

        var merged = ModuleDiscovery.MergeWithInstalled(externalModules, installedModules);

        Assert.Single(merged);
        Assert.Equal("LocalOnly", merged[0].Id);
    }

    private static ManifestModuleInfo CreateModule(string id, string version, string platformVersion = "3.0.0", bool isInstalled = false, string[] dependencyIds = null)
    {
        var manifest = new ModuleManifest
        {
            Id = id,
            Version = version,
            PlatformVersion = platformVersion,
            Dependencies = dependencyIds?.Select(x => new ManifestDependency { Id = x, Version = "1.0.0" }).ToArray(),
        };

        var moduleInfo = AbstractTypeFactory<ManifestModuleInfo>.TryCreateInstance();
        moduleInfo.LoadFromManifest(manifest);
        moduleInfo.IsInstalled = isInstalled;

        return moduleInfo;
    }
}
