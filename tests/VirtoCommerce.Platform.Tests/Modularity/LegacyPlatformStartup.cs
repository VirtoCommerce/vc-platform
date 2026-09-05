using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using VirtoCommerce.Platform.Core.Modularity;

namespace VirtoCommerce.Platform.Tests.Modularity;

/// <summary>
/// Implements the four members the interface declared before the two hooks were added, and
/// overrides neither new one. Its existence is the compile-time half of AC39: if either new member
/// lacked a default implementation, this class would not compile.
/// </summary>
public class LegacyPlatformStartup : IPlatformStartup
{
    public void ConfigureAppConfiguration(IConfigurationBuilder builder, IHostEnvironment env) { }

    public void ConfigureHostServices(IServiceCollection services, IConfiguration config) { }

    public void ConfigureServices(IServiceCollection services, IConfiguration config) { }

    public void Configure(IApplicationBuilder app, IConfiguration config) { }
}
