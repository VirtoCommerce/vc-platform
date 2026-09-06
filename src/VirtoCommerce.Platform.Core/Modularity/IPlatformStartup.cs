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

    /// <summary>
    /// Called during Startup.Configure after UseRouting, UseStaticFiles and UseModulesAndAppsFiles, and before
    /// UseAuthentication: the endpoint is matched (HttpContext.GetEndpoint() is still null when no route
    /// matched) and the caller is not yet authenticated.
    /// Use for middleware that needs the matched endpoint but must run before authentication.
    /// </summary>
    /// <remarks>
    /// Requests for platform and module static files never get here: UseStaticFiles and
    /// UseModulesAndAppsFiles serve the file and end the request first. Middleware registered here sees
    /// API and page requests, not scripts, styles or images.
    /// </remarks>
    void ConfigureAfterRouting(IApplicationBuilder app, IConfiguration config) { }

    /// <summary>
    /// Called during Startup.Configure after UseAuthentication and UseAccountLockoutMiddleware, and before
    /// UseAuthorization: HttpContext.User is set (it may be anonymous) and authorization has not run yet, so
    /// the requests it is about to reject with 401 or 403 are still visible here.
    /// Use for middleware that needs the principal and the matched endpoint and must also see the requests
    /// authorization rejects.
    /// </summary>
    /// <remarks>
    /// Requests for platform and module static files never get here: UseStaticFiles and
    /// UseModulesAndAppsFiles serve the file and end the request first. Middleware registered here sees
    /// API and page requests, not scripts, styles or images.
    /// </remarks>
    void ConfigureAfterAuthentication(IApplicationBuilder app, IConfiguration config) { }
}
