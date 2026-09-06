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
    [Fact]
    public void ModuleBootstrapper_WithSeveralPlatformStartups_InvokesEachHookOncePerImplementation()
    {
        var bootstrapper = new ModuleBootstrapper(NullLoggerFactory.Instance, new LocalStorageModuleCatalogOptions());
        var first = new CountingPlatformStartup();
        var second = new CountingPlatformStartup();
        bootstrapper.Startups.Add(first);
        bootstrapper.Startups.Add(new NoHookPlatformStartup());
        bootstrapper.Startups.Add(second);

        var app = Mock.Of<IApplicationBuilder>();
        var configuration = new ConfigurationBuilder().Build();

        bootstrapper.RunConfigureAfterRouting(app, configuration);
        bootstrapper.RunConfigureAfterAuthentication(app, configuration);

        first.AfterRoutingCalls.Should().Be(1);
        first.AfterAuthenticationCalls.Should().Be(1);
        second.AfterRoutingCalls.Should().Be(1);
        second.AfterAuthenticationCalls.Should().Be(1);
    }
}
