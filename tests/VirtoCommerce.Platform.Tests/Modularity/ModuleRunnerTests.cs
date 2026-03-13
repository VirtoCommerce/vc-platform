using System.Collections.Generic;
using System.Linq;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Modularity;
using VirtoCommerce.Platform.Modules;
using Xunit;

namespace VirtoCommerce.Platform.Tests.Modularity;

public class ModuleRunnerTests
{
    [Fact]
    public void SortByDependency_LinearChain_CorrectOrder()
    {
        // A -> B -> C (C has no deps, should come first)
        var moduleC = CreateModule("C");
        var moduleB = CreateModule("B", "C");
        var moduleA = CreateModule("A", "B");

        var sorted = ModuleRunner.SortByDependency([moduleA, moduleB, moduleC]);

        var names = sorted.Select(m => m.ModuleName).ToList();
        Assert.True(names.IndexOf("C") < names.IndexOf("B"));
        Assert.True(names.IndexOf("B") < names.IndexOf("A"));
    }

    [Fact]
    public void SortByDependency_DiamondDependency_NoDuplicates()
    {
        // A -> B, A -> C, B -> D, C -> D
        var moduleD = CreateModule("D");
        var moduleB = CreateModule("B", "D");
        var moduleC = CreateModule("C", "D");
        var moduleA = CreateModule("A", "B", "C");

        var sorted = ModuleRunner.SortByDependency([moduleA, moduleB, moduleC, moduleD]);

        Assert.Equal(4, sorted.Count);
        var names = sorted.Select(m => m.ModuleName).ToList();

        // D must come before B and C, both before A
        Assert.True(names.IndexOf("D") < names.IndexOf("B"));
        Assert.True(names.IndexOf("D") < names.IndexOf("C"));
        Assert.True(names.IndexOf("B") < names.IndexOf("A"));
        Assert.True(names.IndexOf("C") < names.IndexOf("A"));
    }

    [Fact]
    public void SortByDependency_NoDependencies_ReturnsAll()
    {
        var modules = new List<ManifestModuleInfo>
        {
            CreateModule("X"),
            CreateModule("Y"),
            CreateModule("Z"),
        };

        var sorted = ModuleRunner.SortByDependency(modules);

        Assert.Equal(3, sorted.Count);
    }

    [Fact]
    public void SortByDependency_EmptyList_ReturnsEmpty()
    {
        var sorted = ModuleRunner.SortByDependency([]);
        Assert.Empty(sorted);
    }

    [Fact]
    public void SortByDependency_SingleModule_ReturnsSingle()
    {
        var sorted = ModuleRunner.SortByDependency([CreateModule("Only")]);
        Assert.Single(sorted);
        Assert.Equal("Only", sorted[0].ModuleName);
    }

    [Fact]
    public void SortByDependency_OptionalDependency_Excluded()
    {
        // A depends optionally on B - B should not affect ordering if not required
        var moduleA = CreateModule("A");
        moduleA.DependsOn.Add("B");
        moduleA.Dependencies.Add(new ModuleIdentity("B", SemanticVersion.Parse("1.0.0"), optional: true));
        var moduleB = CreateModule("B");

        // Should not throw even though B is optional and sort should work
        var sorted = ModuleRunner.SortByDependency([moduleA, moduleB]);
        Assert.Equal(2, sorted.Count);
    }

    private static ManifestModuleInfo CreateModule(string id, params string[] dependencies)
    {
        var manifest = new ModuleManifest
        {
            Id = id,
            Version = "1.0.0",
            PlatformVersion = "3.0.0",
            Dependencies = dependencies.Select(d => new ManifestDependency { Id = d, Version = "1.0.0" }).ToArray()
        };

        var moduleInfo = AbstractTypeFactory<ManifestModuleInfo>.TryCreateInstance();
        moduleInfo.LoadFromManifest(manifest);
        moduleInfo.IsInstalled = true;
        return moduleInfo;
    }
}
