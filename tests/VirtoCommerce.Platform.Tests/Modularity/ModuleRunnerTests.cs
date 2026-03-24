using System.Collections.Generic;
using System.Linq;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Modularity;
using VirtoCommerce.Platform.Core.Modularity.Exceptions;
using VirtoCommerce.Platform.Modules;
using Xunit;

namespace VirtoCommerce.Platform.Tests.Modularity;

public class ModuleRunnerTests
{
    [Fact]
    public void SortByDependency_EmptyList_ReturnsEmpty()
    {
        // Arrange
        var modules = new List<ManifestModuleInfo>();

        // Act
        var sorted = ModuleRunner.SortModulesByDependency(modules);

        // Assert
        Assert.Empty(sorted);
    }

    [Fact]
    public void SortByDependency_SingleModule_ReturnsSingle()
    {
        // Arrange
        var modules = new List<ManifestModuleInfo>
        {
            CreateModule("A"),
        };

        // Act
        var sorted = ModuleRunner.SortModulesByDependency(modules);

        // Assert
        Assert.Single(sorted);
        Assert.Equal("A", sorted[0].ModuleName);
    }

    [Fact]
    public void SortByDependency_NoDependencies_ReturnsAll()
    {
        // Arrange
        var modules = new List<ManifestModuleInfo>
        {
            CreateModule("A"),
            CreateModule("B"),
            CreateModule("C"),
        };

        // Act
        var sorted = ModuleRunner.SortModulesByDependency(modules);

        // Assert
        Assert.Equal(3, sorted.Count);
        Assert.Equal("A B C", string.Join(" ", sorted.Select(x => x.ModuleName)));
    }

    [Fact]
    public void SortByDependency_LinearChain_CorrectOrder()
    {
        // Arrange
        // A -> B -> C (C has no dependencies, should come first)
        var modules = new List<ManifestModuleInfo>
        {
            CreateModule("A", dependencies: ["B"]),
            CreateModule("B", dependencies: ["C"]),
            CreateModule("C"),
        };

        // Act
        var sorted = ModuleRunner.SortModulesByDependency(modules);

        // Assert
        Assert.Equal("C B A", string.Join(" ", sorted.Select(x => x.ModuleName)));
    }

    [Fact]
    public void SortByDependency_DiamondDependency_NoDuplicates()
    {
        // Arrange
        // A -> B,C; B -> D; C -> D
        var modules = new List<ManifestModuleInfo>
        {
            CreateModule("A", dependencies: ["B", "C"]),
            CreateModule("B", dependencies: ["D"]),
            CreateModule("C", dependencies: ["D"]),
            CreateModule("D"),
        };

        // Act
        var sorted = ModuleRunner.SortModulesByDependency(modules);

        // Assert
        Assert.Equal(4, sorted.Count);
        Assert.Equal("D B C A", string.Join(" ", sorted.Select(x => x.ModuleName)));
    }

    [Fact]
    public void SortByDependency_Cycle_ThrowException()
    {
        // Arrange
        // A -> B -> C -> A
        var modules = new List<ManifestModuleInfo>
        {
            CreateModule("A", dependencies: ["B"]),
            CreateModule("B", dependencies: ["C"]),
            CreateModule("C", dependencies: ["A"]),
        };

        // Act, Assert
        Assert.Throws<CyclicDependencyFoundException>(() =>
            ModuleRunner.SortModulesByDependency(modules));
    }

    [Fact]
    public void SortByDependency_Boost_CorrectOrder()
    {
        // Arrange
        var modules = new List<ManifestModuleInfo>
        {
            CreateModule("A"),
            CreateModule("B"),
            CreateModule("C"),
        };

        ModuleRunner.Initialize(new ModuleSequenceBoostOptions
        {
            ModuleSequenceBoost = ["B"],
        });

        // Act
        var sorted = ModuleRunner.SortModulesByDependency(modules);

        // Assert
        Assert.Equal(3, sorted.Count);
        Assert.Equal("B A C", string.Join(" ", sorted.Select(x => x.ModuleName)));
    }

    [Fact]
    public void SortByDependency_MissingOptionalDependency_NoErrors()
    {
        // Arrange
        var modules = new List<ManifestModuleInfo>
        {
            CreateModule("A", [CreateDependency("B"), CreateDependency("C", isOptional: true)]),
            CreateModule("B"),
        };

        // Act
        var sorted = ModuleRunner.SortModulesByDependency(modules);

        // Assert
        Assert.Equal(2, sorted.Count);
        Assert.Equal("B A", string.Join(" ", sorted.Select(x => x.ModuleName)));
    }


    private static ManifestModuleInfo CreateModule(string id, string[] dependencies = null)
    {
        return CreateModule(id, dependencies?.Select(x => CreateDependency(x)).ToArray());
    }

    private static ManifestModuleInfo CreateModule(string id, ManifestDependency[] dependencies)
    {
        var manifest = new ModuleManifest
        {
            Id = id,
            Version = "1.0.0",
            PlatformVersion = "3.0.0",
            Dependencies = dependencies,
        };

        var module = AbstractTypeFactory<ManifestModuleInfo>.TryCreateInstance();
        module.LoadFromManifest(manifest);
        module.IsInstalled = true;

        return module;
    }

    private static ManifestDependency CreateDependency(string id, bool isOptional = false)
    {
        return new ManifestDependency
        {
            Id = id,
            Version = "1.0.0",
            Optional = isOptional,
        };
    }
}
