using System.Collections.Generic;
using FluentAssertions;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using VirtoCommerce.Platform.Core.Modularity;
using VirtoCommerce.Platform.Modules;
using Xunit;

namespace VirtoCommerce.Platform.Tests.Modularity;

public class ModuleBootstrapperTests
{
    // Startups are discovered by walking the module list, which the catalog orders by dependency, so a
    // module's middleware is registered after that of the modules it depends on. Sorting the list any
    // other way - or running the loop concurrently - would reorder middleware in other people's modules
    // with nothing to announce it.
    [Fact]
    public void RunConfigureAfterRouting_WithSeveralPlatformStartups_RunsEachOnceInRegistrationOrder()
    {
        var log = new List<string>();
        var bootstrapper = CreateBootstrapper(log);

        bootstrapper.RunConfigureAfterRouting(Mock.Of<IApplicationBuilder>(), new ConfigurationBuilder().Build());

        log.Should().Equal("first:routing", "second:routing");
    }

    [Fact]
    public void RunConfigureAfterAuthentication_WithSeveralPlatformStartups_RunsEachOnceInRegistrationOrder()
    {
        var log = new List<string>();
        var bootstrapper = CreateBootstrapper(log);

        bootstrapper.RunConfigureAfterAuthentication(Mock.Of<IApplicationBuilder>(), new ConfigurationBuilder().Build());

        log.Should().Equal("first:authentication", "second:authentication");
    }

    // The startup between the two recorders overrides neither hook: a dispatch that reached only the
    // implementations declaring a member would still satisfy the order, but not this arrangement.
    private static ModuleBootstrapper CreateBootstrapper(IList<string> log)
    {
        var bootstrapper = new ModuleBootstrapper(NullLoggerFactory.Instance, new LocalStorageModuleCatalogOptions());

        bootstrapper.Startups.Add(new RecordingPlatformStartup("first", log));
        bootstrapper.Startups.Add(new NoHookPlatformStartup());
        bootstrapper.Startups.Add(new RecordingPlatformStartup("second", log));

        return bootstrapper;
    }
}
