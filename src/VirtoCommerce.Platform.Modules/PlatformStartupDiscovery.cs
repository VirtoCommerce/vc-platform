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

        foreach (var module in modules)
        {
            if (string.IsNullOrEmpty(module.StartupType) || module.Assembly == null)
            {
                continue;
            }

            try
            {
                var startupType = module.Assembly.GetType(module.StartupType)
                    ?? FindTypeByName(module.Assembly, module.StartupType);

                if (startupType == null)
                {
                    ModuleLogger.CreateLogger(typeof(PlatformStartupDiscovery)).LogWarning("Startup type '{StartupType}' not found in {ModuleId}", module.StartupType, module.Id);
                    continue;
                }

                if (!typeof(IPlatformStartup).IsAssignableFrom(startupType))
                {
                    ModuleLogger.CreateLogger(typeof(PlatformStartupDiscovery)).LogWarning("Type '{StartupType}' does not implement IPlatformStartup in {ModuleId}", module.StartupType, module.Id);
                    continue;
                }

                if (Activator.CreateInstance(startupType) is IPlatformStartup instance)
                {
                    startups.Add(instance);
                    ModuleLogger.CreateLogger(typeof(PlatformStartupDiscovery)).LogInformation("Discovered {StartupTypeName} from {ModuleId} (priority: {Priority})", startupType.Name, module.Id, instance.Priority);
                }
            }
            catch (Exception ex)
            {
                ModuleLogger.CreateLogger(typeof(PlatformStartupDiscovery)).LogError(ex, "Error loading startup type from {ModuleId}", module.Id);
            }
        }

        startups.Sort((a, b) => a.Priority.CompareTo(b.Priority));
        _startups = startups;
        ModuleLogger.CreateLogger(typeof(PlatformStartupDiscovery)).LogInformation("Discovered {StartupCount} platform startup types", startups.Count);
        return _startups;
    }

    /// <summary>
    /// Get previously discovered startups.
    /// </summary>
    public static IReadOnlyList<IPlatformStartup> GetStartups() => _startups;

    public static void RunConfigureAppConfiguration(
        IReadOnlyList<IPlatformStartup> startups,
        IConfigurationBuilder builder,
        IHostEnvironment env)
    {
        foreach (var startup in startups)
        {
            startup.ConfigureAppConfiguration(builder, env);
        }
    }

    public static void RunConfigureHostServices(
        IReadOnlyList<IPlatformStartup> startups,
        IServiceCollection services,
        IConfiguration config)
    {
        foreach (var startup in startups)
        {
            startup.ConfigureHostServices(services, config);
        }
    }

    public static void RunConfigureServices(
        IReadOnlyList<IPlatformStartup> startups,
        IServiceCollection services,
        IConfiguration config)
    {
        foreach (var startup in startups)
        {
            startup.ConfigureServices(services, config);
        }
    }

    public static void RunConfigure(
        IReadOnlyList<IPlatformStartup> startups,
        PipelinePhase phase,
        IApplicationBuilder app,
        IConfiguration config)
    {
        foreach (var startup in startups.Where(s => s.Phase == phase))
        {
            startup.Configure(app, config);
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
            .FirstOrDefault(t => t.FullName == typeName || t.Name == typeName);
    }
}
