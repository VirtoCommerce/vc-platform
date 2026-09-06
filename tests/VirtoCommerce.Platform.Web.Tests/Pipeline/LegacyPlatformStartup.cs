using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using VirtoCommerce.Platform.Core.Modularity;

namespace VirtoCommerce.Platform.Web.Tests.Pipeline;

/// <summary>
/// Every implementer that existed before the hooks is in exactly this state, which is
/// what the default-implementation form puts at risk platform-wide.
/// </summary>
internal sealed class LegacyPlatformStartup : IPlatformStartup
{
    public void ConfigureAppConfiguration(IConfigurationBuilder builder, IHostEnvironment env) { }

    public void ConfigureHostServices(IServiceCollection services, IConfiguration config) { }

    public void ConfigureServices(IServiceCollection services, IConfiguration config) { }

    public void Configure(IApplicationBuilder app, IConfiguration config) { }
}
