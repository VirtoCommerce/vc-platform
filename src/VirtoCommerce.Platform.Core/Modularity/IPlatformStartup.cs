using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace VirtoCommerce.Platform.Core.Modularity
{
    /// <summary>
    /// Allows modules to participate in early platform startup phases that occur
    /// before or outside the standard <see cref="IModule.Initialize"/>/<see cref="IModule.PostInitialize"/> lifecycle.
    /// <para>
    /// Implementations must have a parameterless constructor (instantiated via <see cref="System.Activator.CreateInstance(System.Type)"/>).
    /// </para>
    /// <para>
    /// Declare the implementation type in <c>module.manifest</c> using the <c>&lt;startupType&gt;</c> element.
    /// </para>
    /// </summary>
    public interface IPlatformStartup
    {
        /// <summary>
        /// Controls the order in which startup types are invoked within the same phase.
        /// Lower values run first. Use <see cref="StartupPriority"/> constants for well-known values.
        /// </summary>
        int Priority => StartupPriority.Default;

        /// <summary>
        /// Determines when <see cref="Configure"/> is called in the middleware pipeline.
        /// </summary>
        PipelinePhase Phase => PipelinePhase.Initialization;

        /// <summary>
        /// Called during host building (Program.cs ConfigureAppConfiguration phase).
        /// Use this to add configuration sources (e.g., Azure App Configuration).
        /// </summary>
        /// <param name="builder">The configuration builder to add sources to.</param>
        /// <param name="hostEnvironment">The host environment (provides EnvironmentName, ContentRootPath, etc.).</param>
        void ConfigureAppConfiguration(IConfigurationBuilder builder, IHostEnvironment hostEnvironment) { }

        /// <summary>
        /// Called during host building (Program.cs ConfigureServices phase).
        /// Use this to register hosted services (e.g., Hangfire server).
        /// </summary>
        /// <param name="services">The host-level service collection.</param>
        /// <param name="configuration">The fully-built host configuration.</param>
        void ConfigureHostServices(IServiceCollection services, IConfiguration configuration) { }

        /// <summary>
        /// Called during Startup.ConfigureServices, after modules are loaded via AddModules().
        /// Use this to register application-level services.
        /// </summary>
        void ConfigureServices(IServiceCollection services, IConfiguration configuration) { }

        /// <summary>
        /// Called during Startup.Configure at the pipeline position determined by <see cref="Phase"/>.
        /// Use this to add middleware or perform initialization.
        /// </summary>
        void Configure(IApplicationBuilder app, IConfiguration configuration) { }
    }
}
