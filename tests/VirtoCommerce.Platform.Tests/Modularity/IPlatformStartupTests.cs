using FluentAssertions;
using VirtoCommerce.Platform.Core.Modularity;
using Xunit;

namespace VirtoCommerce.Platform.Tests.Modularity;

public class IPlatformStartupTests
{
    [Fact]
    public void IPlatformStartup_TheTwoNewMembers_DeclareDefaultsAndNeedNoOverride()
    {
        var afterRouting = typeof(IPlatformStartup).GetMethod(nameof(IPlatformStartup.ConfigureAfterRouting));
        var afterAuthentication = typeof(IPlatformStartup).GetMethod(nameof(IPlatformStartup.ConfigureAfterAuthentication));

        afterRouting.Should().NotBeNull();
        afterAuthentication.Should().NotBeNull();

        // A default implementation on an interface member compiles to a non-abstract method body.
        afterRouting.IsAbstract.Should().BeFalse();
        afterAuthentication.IsAbstract.Should().BeFalse();

        // The four pre-existing members stay abstract: adding a default to one of those would let an
        // implementer silently stop participating in a phase it used to participate in.
        var configureAppConfiguration = typeof(IPlatformStartup).GetMethod(nameof(IPlatformStartup.ConfigureAppConfiguration));
        var configureHostServices = typeof(IPlatformStartup).GetMethod(nameof(IPlatformStartup.ConfigureHostServices));
        var configureServices = typeof(IPlatformStartup).GetMethod(nameof(IPlatformStartup.ConfigureServices));
        var configure = typeof(IPlatformStartup).GetMethod(nameof(IPlatformStartup.Configure));

        configureAppConfiguration.Should().NotBeNull();
        configureHostServices.Should().NotBeNull();
        configureServices.Should().NotBeNull();
        configure.Should().NotBeNull();

        configureAppConfiguration.IsAbstract.Should().BeTrue();
        configureHostServices.IsAbstract.Should().BeTrue();
        configureServices.IsAbstract.Should().BeTrue();
        configure.IsAbstract.Should().BeTrue();

        // The runtime half of the default-implementation claim is AC40b, in Platform.Web.Tests: an
        // implementer overriding neither member serves a request unchanged.
        new LegacyPlatformStartup().Should().BeAssignableTo<IPlatformStartup>();
    }
}
