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
        OAuthLoginPaths = new Dictionary<string, string>
        {
            ["platform.example"] = "",
            ["vendors.example"] = "/vendor/sign-in",
        },
    }));

    [Fact]
    public async Task GetLoginPathAsync_HostNotListed_UsesTheDefaultPath()
    {
        Assert.Equal("/oauth/authorize", await _provider.GetLoginPathAsync("store.example"));
    }

    [Fact]
    public async Task GetLoginPathAsync_HostWithAnEmptyPath_SelectsTheBuiltInLoginPage()
    {
        Assert.Null(await _provider.GetLoginPathAsync("platform.example"));
    }

    [Fact]
    public async Task GetLoginPathAsync_HostWithItsOwnPath_UsesIt()
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

    // An environment variable such as Authorization__OAuthLoginPaths__platform.example with an empty value
    [Fact]
    public async Task GetLoginPathAsync_EmptyPathFromTheConfiguration_SelectsTheBuiltInLoginPage()
    {
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string>
            {
                ["Authorization:OAuthLoginPath"] = "/oauth/authorize",
                ["Authorization:OAuthLoginPaths:Platform.Example"] = "",
            })
            .Build();
        var options = configuration.GetSection("Authorization").Get<AuthorizationOptions>();
        var provider = new OAuthLoginPathProvider(Options.Create(options));

        Assert.Null(await provider.GetLoginPathAsync("platform.example"));
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
