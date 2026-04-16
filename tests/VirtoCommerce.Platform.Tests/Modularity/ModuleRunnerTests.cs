using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging.Abstractions;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Modularity;
using VirtoCommerce.Platform.Modules;
using Xunit;

namespace VirtoCommerce.Platform.Tests.Modularity;

public class ModuleRunnerTests
{
    private readonly ModuleBootstrapper _bootstrapper = CreateBootstrapper();

    [Fact]
    public void SortByDependency_EmptyList_ReturnsEmpty()
    {
        // Arrange
        var modules = new List<ManifestModuleInfo>();

        // Act
        var sorted = _bootstrapper.SortModulesByDependency(modules);

        // Assert
        Assert.Equal(modules.Count, sorted.Count);
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
        var sorted = _bootstrapper.SortModulesByDependency(modules);

        // Assert
        Assert.Equal(modules.Count, sorted.Count);
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
        var sorted = _bootstrapper.SortModulesByDependency(modules);

        // Assert
        Assert.Equal(modules.Count, sorted.Count);
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
        var sorted = _bootstrapper.SortModulesByDependency(modules);

        // Assert
        Assert.Equal(modules.Count, sorted.Count);
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
        var sorted = _bootstrapper.SortModulesByDependency(modules);

        // Assert
        Assert.Equal(modules.Count, sorted.Count);
        Assert.Equal("D B C A", string.Join(" ", sorted.Select(x => x.ModuleName)));
    }

    [Fact]
    public void SortByDependency_MissingDependency_NoErrors()
    {
        // Arrange
        var modules = new List<ManifestModuleInfo>
        {
            CreateModule("A", dependencies: ["B", "D"]),
            CreateModule("B", dependencies: ["C"]),
            CreateModule("C"),
        };

        // Act
        var sorted = _bootstrapper.SortModulesByDependency(modules);

        // Assert
        Assert.Equal(modules.Count, sorted.Count);
        Assert.Equal("C B A", string.Join(" ", sorted.Select(x => x.ModuleName)));
        Assert.True(sorted.All(x => x.Errors.Count == 0));
    }

    [Fact]
    public void SortByDependency_OptionalDependency_CorrectOrder()
    {
        // Arrange
        var modules = new List<ManifestModuleInfo>
        {
            CreateModule("A", dependencies: ["C"]),
            CreateModule("B", dependencies: [CreateDependency("C", isOptional: true)]),
            CreateModule("C"),
        };

        // Act
        var sorted = _bootstrapper.SortModulesByDependency(modules);

        // Assert
        Assert.Equal(modules.Count, sorted.Count);
        Assert.Equal("C A B", string.Join(" ", sorted.Select(x => x.ModuleName)));
    }

    [Fact]
    public void SortByDependency_Boost_CorrectOrder()
    {
        // Arrange
        var modules = new List<ManifestModuleInfo>
        {
            CreateModule("A", dependencies: ["D"]),
            CreateModule("B", dependencies: ["D"]),
            CreateModule("C", dependencies: ["D"]),
            CreateModule("D", dependencies: ["F"]),
            CreateModule("E", dependencies: ["F"]),
            CreateModule("F"),
        };

        // Act
        var normal = _bootstrapper.SortModulesByDependency(modules);

        var boostedBootstrapper = CreateBootstrapper(new LocalStorageModuleCatalogOptions
        {
            ModuleSequenceBoost = ["B", "C"],
        });

        var boosted = boostedBootstrapper.SortModulesByDependency(modules);

        // Assert
        Assert.Equal(modules.Count, normal.Count);
        Assert.Equal(modules.Count, boosted.Count);
        Assert.Equal("F D E A B C", string.Join(" ", normal.Select(x => x.ModuleName)));
        Assert.Equal("F D E B C A", string.Join(" ", boosted.Select(x => x.ModuleName)));
    }

    [Fact]
    public void SortByDependency_Cycle_ThrowException()
    {
        // Arrange
        var modules = new List<ManifestModuleInfo>
        {
            CreateModule("A", dependencies: ["B"]),
            CreateModule("B", dependencies: ["C"]),
            CreateModule("C", dependencies: ["A"]),
            CreateModule("D"),
            CreateModule("E", dependencies: ["C"]),
        };

        // Act
        var sorted = _bootstrapper.SortModulesByDependency(modules);

        // Assert
        Assert.Equal(modules.Count, sorted.Count);
        Assert.Equal("D A B C E", string.Join(" ", sorted.Select(x => x.ModuleName)));

        Assert.Empty(sorted[0].Errors);

        foreach (var module in sorted.Skip(1))
        {
            Assert.Single(module.Errors);
            Assert.Equal("This module either has a loop in the dependencies or it depends on a module with such a loop.", module.Errors.Single());
        }
    }


    private static ModuleBootstrapper CreateBootstrapper(LocalStorageModuleCatalogOptions options = null)
    {
        return new ModuleBootstrapper(
            NullLoggerFactory.Instance,
            options ?? new LocalStorageModuleCatalogOptions());
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
