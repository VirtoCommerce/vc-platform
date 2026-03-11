using System.Collections.Generic;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Modularity;
using VirtoCommerce.Platform.Modules;
using Xunit;

namespace VirtoCommerce.Platform.Tests.Modularity;

public class ModuleRegistryTests
{
    public ModuleRegistryTests()
    {
        ModuleRegistry.Reset();
    }

    [Fact]
    public void IsInstalled_ExistingModule_ReturnsTrue()
    {
        var modules = new List<ManifestModuleInfo> { CreateModule("TestModule", "1.0.0", isInstalled: true) };
        ModuleRegistry.Initialize(modules);

        Assert.True(ModuleRegistry.IsInstalled("TestModule"));
    }

    [Fact]
    public void IsInstalled_MissingModule_ReturnsFalse()
    {
        var modules = new List<ManifestModuleInfo> { CreateModule("TestModule", "1.0.0", isInstalled: true) };
        ModuleRegistry.Initialize(modules);

        Assert.False(ModuleRegistry.IsInstalled("NonExistent"));
    }

    [Fact]
    public void IsInstalled_NotInstalledModule_ReturnsFalse()
    {
        var modules = new List<ManifestModuleInfo> { CreateModule("TestModule", "1.0.0", isInstalled: false) };
        ModuleRegistry.Initialize(modules);

        Assert.False(ModuleRegistry.IsInstalled("TestModule"));
    }

    [Fact]
    public void IsInstalled_CaseInsensitive()
    {
        var modules = new List<ManifestModuleInfo> { CreateModule("TestModule", "1.0.0", isInstalled: true) };
        ModuleRegistry.Initialize(modules);

        Assert.True(ModuleRegistry.IsInstalled("testmodule"));
        Assert.True(ModuleRegistry.IsInstalled("TESTMODULE"));
    }

    [Fact]
    public void IsInstalled_WithMinVersion_SatisfiedVersion_ReturnsTrue()
    {
        var modules = new List<ManifestModuleInfo> { CreateModule("TestModule", "2.5.0", isInstalled: true) };
        ModuleRegistry.Initialize(modules);

        Assert.True(ModuleRegistry.IsInstalled("TestModule", "2.0.0"));
        Assert.True(ModuleRegistry.IsInstalled("TestModule", "2.5.0"));
    }

    [Fact]
    public void IsInstalled_WithMinVersion_UnsatisfiedVersion_ReturnsFalse()
    {
        var modules = new List<ManifestModuleInfo> { CreateModule("TestModule", "1.0.0", isInstalled: true) };
        ModuleRegistry.Initialize(modules);

        Assert.False(ModuleRegistry.IsInstalled("TestModule", "2.0.0"));
    }

    [Fact]
    public void GetModule_ExistingModule_ReturnsInfo()
    {
        var module = CreateModule("TestModule", "1.0.0", isInstalled: true);
        ModuleRegistry.Initialize([module]);

        var result = ModuleRegistry.GetModule("TestModule");
        Assert.NotNull(result);
        Assert.Equal("TestModule", result.Id);
    }

    [Fact]
    public void GetModule_MissingModule_ReturnsNull()
    {
        ModuleRegistry.Initialize([]);

        Assert.Null(ModuleRegistry.GetModule("NonExistent"));
    }

    [Fact]
    public void GetAllModules_ReturnsAll()
    {
        var modules = new List<ManifestModuleInfo>
        {
            CreateModule("Module1", "1.0.0", isInstalled: true),
            CreateModule("Module2", "2.0.0", isInstalled: false),
        };
        ModuleRegistry.Initialize(modules);

        Assert.Equal(2, ModuleRegistry.GetAllModules().Count);
    }

    [Fact]
    public void GetInstalledModules_ReturnsOnlyInstalled()
    {
        var modules = new List<ManifestModuleInfo>
        {
            CreateModule("Module1", "1.0.0", isInstalled: true),
            CreateModule("Module2", "2.0.0", isInstalled: false),
            CreateModule("Module3", "3.0.0", isInstalled: true),
        };
        ModuleRegistry.Initialize(modules);

        var installed = ModuleRegistry.GetInstalledModules();
        Assert.Equal(2, installed.Count);
    }

    [Fact]
    public void GetFailedModules_ReturnsOnlyErrored()
    {
        var good = CreateModule("GoodModule", "1.0.0", isInstalled: true);
        var bad = CreateModule("BadModule", "1.0.0", isInstalled: true);
        bad.Errors.Add("Something went wrong");

        ModuleRegistry.Initialize([good, bad]);

        var failed = ModuleRegistry.GetFailedModules();
        Assert.Single(failed);
        Assert.Equal("BadModule", failed[0].Id);
    }

    [Fact]
    public void Reset_ClearsState()
    {
        ModuleRegistry.Initialize([CreateModule("TestModule", "1.0.0", isInstalled: true)]);
        Assert.True(ModuleRegistry.IsInstalled("TestModule"));

        ModuleRegistry.Reset();

        Assert.False(ModuleRegistry.IsInstalled("TestModule"));
        Assert.Empty(ModuleRegistry.GetAllModules());
    }

    private static ManifestModuleInfo CreateModule(string id, string version, bool isInstalled)
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
        return moduleInfo;
    }
}
