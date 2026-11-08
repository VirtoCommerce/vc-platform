using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using VirtoCommerce.Platform.Core.Security;
using VirtoCommerce.Platform.Security.OpenIddict;
using VirtoCommerce.Platform.Web.Security;
using Xunit;

namespace VirtoCommerce.Platform.Web.Tests.Security;

/// <summary>
/// One Platform serves several front-ends: the storefront has its own sign-in page, and the Platform host has only
/// the built-in one.
/// </summary>
public class OAuthLoginPathProviderTests
{
    private readonly OAuthLoginPathProvider _provider = new(Options.Create(new AuthorizationOptions
    {
        OAuthLoginPath = "/oauth/authorize",
        OAuthLoginPaths =
        [
            new OAuthHostLoginPath { Host = "platform.example" },
            new OAuthHostLoginPath { Host = "vendors.example", Path = "/vendor/sign-in" },
            new OAuthHostLoginPath { Host = "legacy.example", Path = "" },
            new OAuthHostLoginPath { Host = "vendors.example", Path = "/ignored" },
        ],
    }));

    [Fact]
    public async Task GetLoginPathAsync_HostNotListed_UsesTheDefaultPath()
    {
        Assert.Equal("/oauth/authorize", await _provider.GetLoginPathAsync("store.example"));
    }

    [Fact]
    public async Task GetLoginPathAsync_HostWithoutAPath_SelectsTheBuiltInLoginPage()
    {
        Assert.Null(await _provider.GetLoginPathAsync("platform.example"));
    }

    [Fact]
    public async Task GetLoginPathAsync_HostWithAnEmptyPath_SelectsTheBuiltInLoginPage()
    {
        Assert.Null(await _provider.GetLoginPathAsync("legacy.example"));
    }

    [Fact]
    public async Task GetLoginPathAsync_HostWithItsOwnPath_UsesTheFirstEntry()
    {
        Assert.Equal("/vendor/sign-in", await _provider.GetLoginPathAsync("vendors.example"));
    }

    [Fact]
    public async Task GetLoginPathAsync_HostMatchIgnoresCase()
    {
        Assert.Null(await _provider.GetLoginPathAsync("Platform.Example"));
    }

    [Fact]
    public async Task GetLoginPathAsync_UnknownHost_UsesTheDefaultPath()
    {
        Assert.Equal("/oauth/authorize", await _provider.GetLoginPathAsync(null));
    }

    [Fact]
    public async Task GetLoginPathAsync_NothingConfigured_SelectsTheBuiltInLoginPage()
    {
        var provider = new OAuthLoginPathProvider(Options.Create(new AuthorizationOptions()));

        Assert.Null(await provider.GetLoginPathAsync("platform.example"));
    }

    [Fact]
    public async Task GetLoginPathAsync_NoHostsListed_KeepsThePreviousBehaviour()
    {
        var provider = new OAuthLoginPathProvider(Options.Create(new AuthorizationOptions { OAuthLoginPath = "/oauth/authorize" }));

        Assert.Equal("/oauth/authorize", await provider.GetLoginPathAsync("platform.example"));
    }

    // Environment variables such as Authorization__OAuthLoginPaths__0__Host=Platform.Example: no host in a variable's name
    [Fact]
    public async Task GetLoginPathAsync_HostsFromTheConfiguration_AreBoundInOrder()
    {
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string>
            {
                ["Authorization:OAuthLoginPath"] = "/oauth/authorize",
                ["Authorization:OAuthLoginPaths:0:Host"] = "Platform.Example",
                ["Authorization:OAuthLoginPaths:1:Host"] = "vendors.example",
                ["Authorization:OAuthLoginPaths:1:Path"] = "/vendor/sign-in",
            })
            .Build();
        var options = configuration.GetSection("Authorization").Get<AuthorizationOptions>();
        var provider = new OAuthLoginPathProvider(Options.Create(options));

        Assert.Null(await provider.GetLoginPathAsync("platform.example"));
        Assert.Equal("/vendor/sign-in", await provider.GetLoginPathAsync("vendors.example"));
        Assert.Equal("/oauth/authorize", await provider.GetLoginPathAsync("store.example"));
    }

    [Fact]
    public void AddSecurityServices_RegistersTheProviderSoAModuleCanReplaceIt()
    {
        var services = new ServiceCollection();
        services.AddSingleton<IOAuthLoginPathProvider, ReplacedProvider>();

        services.AddSecurityServices();

        Assert.Single(services, x => x.ServiceType == typeof(IOAuthLoginPathProvider));
    }

    private sealed class ReplacedProvider : IOAuthLoginPathProvider
    {
        public Task<string> GetLoginPathAsync(string host) => Task.FromResult<string>(null);
    }
}
