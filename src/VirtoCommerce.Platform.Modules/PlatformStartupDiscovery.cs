using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using VirtoCommerce.Platform.Core.Modularity;

namespace VirtoCommerce.Platform.Modules;

/// <summary>
/// Discovers IPlatformStartup implementations from loaded module assemblies.
/// Runs before DI is available - uses static methods and ModuleLogger.
/// Stores discovered startups for later retrieval (like ModuleRegistry pattern).
/// </summary>
public static class PlatformStartupDiscovery
{
    private static IReadOnlyList<IPlatformStartup> _startups = Array.Empty<IPlatformStartup>();

    /// <summary>
    /// Discover IPlatformStartup implementations from already-loaded modules.
    /// Modules must have Assembly loaded and StartupType set in manifest.
    /// Returns instances sorted by Priority (ascending) and stores them internally.
    /// </summary>
    public static IReadOnlyList<IPlatformStartup> DiscoverStartups(IEnumerable<ManifestModuleInfo> modules)
    {
        var startups = new List<IPlatformStartup>();
        var logger = ModuleLogger.CreateLogger(typeof(PlatformStartupDiscovery));

        foreach (var module in modules)
        {
            if (string.IsNullOrEmpty(module.StartupType) || module.Assembly == null)
            {
                continue;
            }

            try
            {
                var startupType = module.Assembly.GetType(module.StartupType) ??
                                  FindTypeByName(module.Assembly, module.StartupType);

                if (startupType == null)
                {
                    logger.LogWarning("Startup type '{StartupType}' not found in {ModuleId}", module.StartupType, module.Id);
                    continue;
                }

                if (!typeof(IPlatformStartup).IsAssignableFrom(startupType))
                {
                    logger.LogWarning("Type '{StartupType}' does not implement IPlatformStartup in {ModuleId}", module.StartupType, module.Id);
                    continue;
                }

                if (Activator.CreateInstance(startupType) is IPlatformStartup instance)
                {
                    startups.Add(instance);
                    logger.LogInformation("Discovered {StartupTypeName} from {ModuleId}", startupType.Name, module.Id);
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error loading startup type from {ModuleId}", module.Id);
            }
        }

        _startups = startups;

        logger.LogDebug("Platform startup extensions: {StartupCount}", startups.Count);

        return _startups;
    }

    /// <summary>
    /// Get previously discovered startups.
    /// </summary>
    public static IReadOnlyList<IPlatformStartup> GetStartups() => _startups;

    public static void RunConfigureAppConfiguration(
        IReadOnlyList<IPlatformStartup> startups,
        IConfigurationBuilder builder,
        IHostEnvironment environment)
    {
        foreach (var startup in startups)
        {
            startup.ConfigureAppConfiguration(builder, environment);
        }
    }

    public static void RunConfigureHostServices(
        IReadOnlyList<IPlatformStartup> startups,
        IServiceCollection services,
        IConfiguration configuration)
    {
        foreach (var startup in startups)
        {
            startup.ConfigureHostServices(services, configuration);
        }
    }

    public static void RunConfigureServices(
        IReadOnlyList<IPlatformStartup> startups,
        IServiceCollection services,
        IConfiguration configuration)
    {
        foreach (var startup in startups)
        {
            startup.ConfigureServices(services, configuration);
        }
    }

    public static void RunConfigure(
        IReadOnlyList<IPlatformStartup> startups,
        PlatformStartupConfigurePhases phase,
        IApplicationBuilder applicationBuilder,
        IConfiguration configuration)
    {
        foreach (var startup in startups.Where(s => s.ConfigurePhases.HasFlag(phase)))
        {
            startup.Configure(applicationBuilder, configuration);
        }
    }

    /// <summary>
    /// Reset internal state (for testing).
    /// </summary>
    public static void Reset()
    {
        _startups = Array.Empty<IPlatformStartup>();
    }

    private static Type FindTypeByName(Assembly assembly, string typeName)
    {
        return assembly.GetTypes()
            .FirstOrDefault(x =>
                x.FullName == typeName ||
                x.Name == typeName ||
                x.AssemblyQualifiedName?.StartsWith(typeName, StringComparison.Ordinal) == true);
    }
}
