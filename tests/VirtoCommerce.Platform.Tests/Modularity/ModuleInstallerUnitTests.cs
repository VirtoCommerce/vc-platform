using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Microsoft.Extensions.Logging.Abstractions;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Modularity;
using VirtoCommerce.Platform.Modules;
using Xunit;

namespace VirtoCommerce.Platform.Tests.Modularity
{
    [Collection("Modularity")]
    public class ModuleInstallerUnitTests
    {
        private readonly ModuleBootstrapper _bootstrapper = new(
            NullLoggerFactory.Instance,
            new LocalStorageModuleCatalogOptions());
        [Theory]
        [ClassData(typeof(ModularityTestData))]
        public void ValidateInstall_ChecksCompatibility(string currentVersionPlatform, ModuleManifest[] moduleManifests, ModuleManifest[] installedModuleManifests, bool isInstallable)
        {
            //Arrange
            var platformVersion = SemanticVersion.Parse(currentVersionPlatform);
            var modules = GetManifestModuleInfos(moduleManifests);
            var installedModules = GetManifestModuleInfos(installedModuleManifests);
            foreach (var m in installedModules)
            { m.IsInstalled = true; }

            //Act
            var allValid = modules.All(module =>
            {
                var errors = _bootstrapper.ValidateInstall(module, installedModules, platformVersion);
                return errors.Count == 0;
            });

            //Assert
            allValid.Should().Be(isInstallable);
        }

        [Theory]
        [ClassData(typeof(DepencencyTestData))]
        public void GetDependencies_DetectsMissingDeps(ModuleManifest[] moduleManifests, ModuleManifest[] installedModuleManifests, bool hasMissingDependency)
        {
            //Arrange
            var modules = GetManifestModuleInfos(moduleManifests);
            var installedModules = GetManifestModuleInfos(installedModuleManifests);
            foreach (var m in installedModules)
            { m.IsInstalled = true; }

            var allAvailable = modules.Concat(installedModules).ToList();

            //Act
            var result = _bootstrapper.GetDependencies(modules, allAvailable);

            //Assert — check if any declared dependency is NOT resolved in the result
            var allDeclaredDeps = modules
                .Where(m => m.Dependencies != null)
                .SelectMany(m => m.Dependencies)
                .Select(d => d.Id)
                .Distinct(StringComparer.OrdinalIgnoreCase);

            var resolvedIds = result.Select(r => r.Id).ToHashSet(StringComparer.OrdinalIgnoreCase);
            var hasMissing = allDeclaredDeps.Any(depId => !resolvedIds.Contains(depId));

            hasMissing.Should().Be(hasMissingDependency);
        }

        private static ManifestModuleInfo[] GetManifestModuleInfos(ModuleManifest[] moduleManifests)
        {
            return moduleManifests.Select(x =>
            {
                var module = AbstractTypeFactory<ManifestModuleInfo>.TryCreateInstance();
                module.LoadFromManifest(x);
                return module;
            }).ToArray();
        }

        class ModularityTestData : IEnumerable<object[]>
        {
            public IEnumerator<object[]> GetEnumerator()
            {
                //Scenario 1
                yield return new object[]
                {
                    "3.0.0",
                    new[] { new ModuleManifest { Id = "A", Version = "3.0.0", PlatformVersion = "3.0.0", Dependencies = new []{ new ManifestDependency { Id = "B", Version = "3.2.0" } }} },
                    new[]
                    {
                        //installed
                        new ModuleManifest { Id = "B", Version = "3.5.0", PlatformVersion = "3.0.0" },
                        new ModuleManifest { Id = "D", Version = "3.0.0", PlatformVersion = "3.0.0", Dependencies = new []{ new ManifestDependency { Id = "B", Version = "3.0.0" } } }
                    },
                    true
                };
                //2
                yield return new object[]
                {
                    "3.0.0",
                    new[] { new ModuleManifest { Id = "A", Version = "3.5.0", PlatformVersion = "3.0.0", Dependencies = new []{ new ManifestDependency { Id = "B", Version = "3.5.0" } }} },
                    new[]
                    {
                        //installed. Don't check dependency version
                        new ModuleManifest { Id = "B", Version = "3.2.0", PlatformVersion = "3.0.0" },
                        new ModuleManifest { Id = "D", Version = "3.0.0", PlatformVersion = "3.0.0", Dependencies = new []{ new ManifestDependency { Id = "B", Version = "3.2.0" } } }
                    },
                    true
                };
                //3
                yield return new object[]
                {
                    "3.0.0",
                    new[] { new ModuleManifest { Id = "A", Version = "3.0.0", PlatformVersion = "3.0.0", Dependencies = new []{ new ManifestDependency { Id = "B", Version = "3.2.0" } }} },
                    new[]
                    {
                        //installed
                        new ModuleManifest { Id = "B", Version = "4.0.0", PlatformVersion = "3.0.0" }
                    },
                    true
                };
                //4
                yield return new object[]
                {
                    "3.0.0",
                    new[] { new ModuleManifest { Id = "A", Version = "3.0.0", PlatformVersion = "3.0.0", Dependencies = new []{ new ManifestDependency { Id = "B", Version = "3.0.0" } }} },
                    new[]
                    {
                        //installed
                        new ModuleManifest { Id = "B", Version = "4.0.0", PlatformVersion = "3.0.0" }
                    },
                    true
                };
                //5
                yield return new object[]
                {
                    "3.0.0",
                    new[] { new ModuleManifest { Id = "A", Version = "4.0.0", PlatformVersion = "3.0.0", Dependencies = new []{ new ManifestDependency { Id = "B", Version = "4.0.0" } }} },
                    new[]
                    {
                        //installed. Don't check dependency version
                        new ModuleManifest { Id = "B", Version = "3.5.0", PlatformVersion = "3.0.0" },
                        new ModuleManifest { Id = "C", Version = "3.0.0", PlatformVersion = "3.0.0", Dependencies = new []{ new ManifestDependency { Id = "B", Version = "3.0.0" } } },
                        new ModuleManifest { Id = "D", Version = "3.0.0", PlatformVersion = "3.0.0", Dependencies = new []{ new ManifestDependency { Id = "B", Version = "3.0.0" } } }
                    },
                    true
                };
                //6
                yield return new object[]
                {
                    "3.0.0",
                    new[] { new ModuleManifest { Id = "A", Version = "3.5.0", PlatformVersion = "3.0.0", Dependencies = new []{ new ManifestDependency { Id = "B", Version = "3.5.0" } }} },
                    new[]
                    {
                        //installed
                        new ModuleManifest { Id = "B", Version = "4.0.0", PlatformVersion = "3.0.0" },
                        new ModuleManifest { Id = "C", Version = "3.0.0", PlatformVersion = "3.0.0", Dependencies = new []{ new ManifestDependency { Id = "B", Version = "4.0.0" } } }
                    },
                    true
                };
                //7
                yield return new object[]
                {
                    "3.0.0",
                    new[]
                    {
                        new ModuleManifest { Id = "A", Version = "3.5.0", PlatformVersion = "3.0.0", Dependencies = new []{ new ManifestDependency { Id = "B", Version = "3.5.0" } }},
                        new ModuleManifest { Id = "C", Version = "3.0.0", PlatformVersion = "3.0.0", Dependencies = new []{ new ManifestDependency { Id = "B", Version = "4.0.0" } }},
                        new ModuleManifest { Id = "D", Version = "3.0.0", PlatformVersion = "3.0.0", Dependencies = new []{ new ManifestDependency { Id = "B", Version = "3.0.0" } }}
                    },
                    Array.Empty<ModuleManifest>(),
                    true
                };
                //8
                yield return new object[]
                {
                    "3.0.0",
                    new[]
                    {
                        new ModuleManifest { Id = "A", Version = "3.5.0", PlatformVersion = "3.0.0", Dependencies = new []{ new ManifestDependency { Id = "B", Version = "3.5.0" } }},
                        new ModuleManifest { Id = "B", Version = "3.6.0", PlatformVersion = "3.0.0", Dependencies = new []{ new ManifestDependency { Id = "A", Version = "3.0.0" } }}
                    },
                    new[] { new ModuleManifest { Id = "B", Version = "3.0.0", PlatformVersion = "3.0.0" }},
                    true
                };
                //9
                yield return new object[]
                {
                    "3.0.0",
                    new[]
                    {
                        new ModuleManifest { Id = "A", Version = "3.8.0", PlatformVersion = "3.0.0" }
                    },
                    new[]
                    {
                        new ModuleManifest { Id = "A", Version = "3.8.0", PlatformVersion = "3.0.0", Dependencies = new []{ new ManifestDependency { Id = "D", Version = "3.2.0" } }},
                        new ModuleManifest { Id = "B", Version = "3.5.0", PlatformVersion = "3.0.0", Dependencies = new []{ new ManifestDependency { Id = "C", Version = "3.0.0" } }},
                        new ModuleManifest { Id = "C", Version = "3.9.0", PlatformVersion = "3.0.0", Dependencies = new []{ new ManifestDependency { Id = "A", Version = "3.0.0" } }},
                        new ModuleManifest { Id = "D", Version = "3.2.0", PlatformVersion = "3.0.0" }
                    },
                    true
                };
                //10
                yield return new object[]
                {
                    "3.0.0",
                    new[]
                    {
                        new ModuleManifest { Id = "A", Version = "4.0.0", PlatformVersion = "3.0.0", Dependencies = new []{ new ManifestDependency { Id = "B", Version = "4.0.0" } }}
                    },
                    new[]
                    {
                        new ModuleManifest { Id = "A", Version = "3.8.0", PlatformVersion = "3.0.0" },
                        new ModuleManifest { Id = "C", Version = "3.9.0", PlatformVersion = "3.0.0", Dependencies = new []{ new ManifestDependency { Id = "A", Version = "3.0.0" } }}
                    },
                    true
                };

                yield return new object[] { "3.1.0-preview-5555", new[] { new ModuleManifest { Id = "A", Version = "3.2.0-prerelease-22", PlatformVersion = "3.0.0" } }, Array.Empty<ModuleManifest>(), true };
                yield return new object[] { "3.1.0", new[] { new ModuleManifest { Id = "A", Version = "3.2.0-prerelease-22", PlatformVersion = "3.0.0" } }, Array.Empty<ModuleManifest>(), true };
                yield return new object[] { "3.0.0", new[] { new ModuleManifest { Id = "A", Version = "3.0.0", PlatformVersion = "3.0.0" } }, Array.Empty<ModuleManifest>(), true };
                yield return new object[] { "3.0.0", new[] { new ModuleManifest { Id = "A", Version = "3.0.1", PlatformVersion = "3.0.1" } }, Array.Empty<ModuleManifest>(), false };
                yield return new object[] { "3.0.0-alpha1", new[] { new ModuleManifest { Id = "A", Version = "3.0.0", PlatformVersion = "3.0.0" } }, Array.Empty<ModuleManifest>(), false };
                yield return new object[] { "3.0.0-alpha1", new[] { new ModuleManifest { Id = "A", Version = "3.0.1", PlatformVersion = "3.0.1" } }, Array.Empty<ModuleManifest>(), false };
                yield return new object[]
                {
                    "3.0.0",
                    new[] { new ModuleManifest { Id = "A", Version = "3.1.0", PlatformVersion = "3.0.0" } },
                    new[] { new ModuleManifest { Id = "A", Version = "3.0.0", PlatformVersion = "3.0.0" } }, //installed
                    true
                };
                yield return new object[]
                {
                    "3.0.0",
                    new[] { new ModuleManifest { Id = "A", Version = "3.0.0", PlatformVersion = "3.0.0" } },
                    new[] { new ModuleManifest { Id = "A", Version = "3.1.0", PlatformVersion = "3.0.0" } }, //installed
                    true
                };
                yield return new object[] { "3.0.0", new[] { new ModuleManifest { Id = "A", Version = "3.1.0", PlatformVersion = "3.0.0" } }, Array.Empty<ModuleManifest>(), true };
                yield return new object[] { "3.0.0", new[] { new ModuleManifest { Id = "A", Version = "3.1.0-alpha001", PlatformVersion = "3.0.0" } }, Array.Empty<ModuleManifest>(), true };
                yield return new object[]
                {
                    "3.0.0-alpha001",
                    new[] {new ModuleManifest {Id = "A", Version = "3.1.0-alpha001", PlatformVersion = "3.0.0-alpha001" } },
                    new[] { new ModuleManifest { Id = "A", Version = "3.0.0", PlatformVersion = "3.0.0-alpha001" } }, //installed
                    true
                };
                yield return new object[]
                {
                    "3.0.0",
                    new[] {new ModuleManifest {Id = "A", Version = "3.1.0-alpha001", PlatformVersion = "3.0.0" } },
                    new[] { new ModuleManifest { Id = "A", Version = "3.0.0", PlatformVersion = "3.0.0" } }, //installed
                    true
                };
                yield return new object[]
                {
                    "3.0.0-alpha001",
                    new[] {new ModuleManifest {Id = "A", Version = "3.1.0-alpha001", PlatformVersion = "3.0.0" } },
                    new[] { new ModuleManifest { Id = "A", Version = "3.0.0", PlatformVersion = "3.0.0" } }, //installed
                    false
                };
                yield return new object[]
                {
                    "3.0.0-alpha001",
                    new[] {new ModuleManifest {Id = "A", Version = "3.1.0-alpha002", PlatformVersion = "3.0.0-alpha001" } },
                    new[] { new ModuleManifest { Id = "A", Version = "3.1.0-alpha001", PlatformVersion = "3.0.0-alpha001" } }, //installed
                    true
                };
                yield return new object[]
                {
                    "3.0.0-alpha001",
                    new[] {new ModuleManifest {Id = "A", Version = "3.0.0-alpha001", PlatformVersion = "3.0.0-alpha001" } },
                    new[] { new ModuleManifest { Id = "A", Version = "3.1.0-alpha002", PlatformVersion = "3.0.0-alpha001" } }, //installed
                    true
                };
                yield return new object[]
                {
                    "3.0.0-alpha001",
                    new[] {new ModuleManifest {Id = "A", Version = "3.0.0", PlatformVersion = "3.0.0-alpha001" } },
                    new[] { new ModuleManifest { Id = "A", Version = "3.1.0-alpha001", PlatformVersion = "3.0.0-alpha001" } }, //installed
                    true
                };
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }
        }

        class DepencencyTestData : IEnumerable<object[]>
        {
            public IEnumerator<object[]> GetEnumerator()
            {
                yield return new object[]
                {
                    //modules
                    new[]
                    {
                        new ModuleManifest { Id = "A", Version = "3.5.0", PlatformVersion = "3.0.0", Dependencies = new []{ new ManifestDependency { Id = "B", Version = "3.5.0" } }},
                    },
                    //installed
                    Array.Empty<ModuleManifest>(),
                    //has missing dependency
                    true
                };
                yield return new object[]
                {
                    new[]
                    {
                        new ModuleManifest { Id = "A", Version = "3.5.0", PlatformVersion = "3.0.0", Dependencies = new []{ new ManifestDependency { Id = "B", Version = "3.5.0" } }},
                        new ModuleManifest { Id = "D", Version = "3.0.0", PlatformVersion = "3.0.0", Dependencies = new []{ new ManifestDependency { Id = "B", Version = "4.0.0" } }},
                    },
                    new[]
                    {
                        //installed
                        new ModuleManifest { Id = "C", Version = "3.5.0", PlatformVersion = "3.0.0" },
                    },
                    true
                };
                yield return new object[]
                {
                    new[] { new ModuleManifest { Id = "A", Version = "3.0.0", PlatformVersion = "3.0.0", Dependencies = new []{ new ManifestDependency { Id = "B", Version = "3.5.0" } }} },
                    new[]
                    {
                        //installed
                        new ModuleManifest { Id = "B", Version = "3.2.0", PlatformVersion = "3.0.0" },
                    },
                    true
                };
                yield return new object[]
                {
                    new[] { new ModuleManifest { Id = "A", Version = "3.0.0", PlatformVersion = "3.0.0", Dependencies = new []{ new ManifestDependency { Id = "B", Version = "3.2.0" } }} },
                    new[]
                    {
                        //installed
                        new ModuleManifest { Id = "B", Version = "3.5.0", PlatformVersion = "3.0.0" },
                    },
                    false
                };
                yield return new object[]
                {
                    new[] { new ModuleManifest { Id = "A", Version = "3.0.0", PlatformVersion = "3.0.0", Dependencies = new []{ new ManifestDependency { Id = "B", Version = "3.2.0" } }} },
                    new[]
                    {
                        //installed
                        new ModuleManifest { Id = "B", Version = "3.2.0", PlatformVersion = "3.0.0" },
                    },
                    false
                };
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }
        }
    }
}
