using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace VirtoCommerce.Platform.Core.Modularity;

/// <summary>
/// Allows modules to participate in platform startup phases before the standard IModule lifecycle.
/// Implementations are discovered via the startupType element in module.manifest.
/// </summary>
public interface IPlatformStartup
{
    /// <summary>
    /// Called during Program.cs ConfigureAppConfiguration phase.
    /// Use to add configuration sources (e.g., Azure App Configuration).
    /// </summary>
    void ConfigureAppConfiguration(IConfigurationBuilder builder, IHostEnvironment env);

    /// <summary>
    /// Called during Program.cs ConfigureServices phase.
    /// Use to register host-level services.
    /// </summary>
    void ConfigureHostServices(IServiceCollection services, IConfiguration config);

    /// <summary>
    /// Called during Startup.ConfigureServices after modules are loaded.
    /// Use for application-level service registration.
    /// </summary>
    void ConfigureServices(IServiceCollection services, IConfiguration config);

    /// <summary>
    /// Called during Startup.Configure.
    /// </summary>
    void Configure(IApplicationBuilder app, IConfiguration config);
}
