using System.Collections.Generic;
using FluentAssertions;
using Microsoft.Extensions.Logging.Abstractions;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Modularity;
using VirtoCommerce.Platform.Modules;
using Xunit;

namespace VirtoCommerce.Platform.Tests.Modularity;

public class ModuleStartupDiscoveryTests
{
    private readonly ModuleBootstrapper _bootstrapper = new(
        NullLoggerFactory.Instance,
        new LocalStorageModuleCatalogOptions());

    // Assemblies are loaded regardless of a module's errors, and InitializeModules then skips the module,
    // so a startup discovered here would register middleware resolving services nobody registered.
    [Fact]
    public void DiscoverStartupsInternal_ModuleRejectedByValidation_ContributesNoStartup()
    {
        var rejected = CreateModuleWithStartup("Rejected");
        rejected.Errors.Add("Module requires platform version 4.0.0, which is incompatible with current 3.800.0");

        _bootstrapper.DiscoverStartupsInternal([rejected]);

        _bootstrapper.Startups.Should().BeEmpty();
    }

    [Fact]
    public void DiscoverStartupsInternal_ModuleWithoutErrors_ContributesItsStartup()
    {
        _bootstrapper.DiscoverStartupsInternal([CreateModuleWithStartup("Healthy")]);

        _bootstrapper.Startups.Should().ContainSingle().Which.Should().BeOfType<NoHookPlatformStartup>();
    }

    private static ManifestModuleInfo CreateModuleWithStartup(string id)
    {
        var module = AbstractTypeFactory<ManifestModuleInfo>.TryCreateInstance();
        module.LoadFromManifest(new ModuleManifest { Id = id, Version = "1.0.0", PlatformVersion = "3.0.0" });
        module.Assembly = typeof(NoHookPlatformStartup).Assembly;
        module.StartupType = typeof(NoHookPlatformStartup).FullName;

        return module;
    }
}
