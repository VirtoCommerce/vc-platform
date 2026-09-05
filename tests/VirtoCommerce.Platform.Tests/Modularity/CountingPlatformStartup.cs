using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using VirtoCommerce.Platform.Core.Modularity;

namespace VirtoCommerce.Platform.Tests.Modularity;

/// <summary>
/// Overrides both new hooks and counts the calls. The four pre-existing members are required, so
/// they are implemented and left empty.
/// </summary>
public class CountingPlatformStartup : IPlatformStartup
{
    public int AfterRoutingCalls { get; private set; }

    public int AfterAuthenticationCalls { get; private set; }

    public void ConfigureAppConfiguration(IConfigurationBuilder builder, IHostEnvironment env) { }

    public void ConfigureHostServices(IServiceCollection services, IConfiguration config) { }

    public void ConfigureServices(IServiceCollection services, IConfiguration config) { }

    public void Configure(IApplicationBuilder app, IConfiguration config) { }

    public void ConfigureAfterRouting(IApplicationBuilder app, IConfiguration config)
    {
        AfterRoutingCalls++;
    }

    public void ConfigureAfterAuthentication(IApplicationBuilder app, IConfiguration config)
    {
        AfterAuthenticationCalls++;
    }
}
