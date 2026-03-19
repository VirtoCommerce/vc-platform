using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Hangfire;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Extensions.Logging;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Modules;

namespace VirtoCommerce.Platform.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            LoadModules();
            CreateHostBuilder(args).Build().Run();
        }

        private static void LoadModules()
        {
            PlatformVersion.CurrentVersion = SemanticVersion.Parse(FileVersionInfo.GetVersionInfo(typeof(Program).Assembly.Location).ProductVersion);

            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT").EmptyToNull() ?? Environments.Production;

            // Build minimal config to get module paths
            var bootConfig = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: true)
                .AddJsonFile($"appsettings.{environment}.json", optional: true)
                .AddEnvironmentVariables()
                .Build();

            var discoveryPath = Path.GetFullPath(bootConfig.GetValue("VirtoCommerce:DiscoveryPath", "modules"));
            var probingPath = Path.GetFullPath(bootConfig.GetValue("VirtoCommerce:ProbingPath", "app_data/modules"));
            var refreshProbing = bootConfig.GetValue("VirtoCommerce:RefreshProbingFolderOnStart", true);

            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(GetLoggerConfiguration(bootConfig))
                .CreateBootstrapLogger();

            // Provide ILoggerFactory backed by Serilog to static module classes
            var bootstrapLoggerFactory = new SerilogLoggerFactory(Log.Logger);
            ModuleLogger.Initialize(bootstrapLoggerFactory);

            var modules = ModuleManifestReader.ReadAll(discoveryPath, probingPath);

            if (!Directory.Exists(probingPath))
            {
                refreshProbing = true;
                Directory.CreateDirectory(probingPath);
            }

            if (refreshProbing)
            {
                ModuleCopier.CopyAll(discoveryPath, probingPath, modules);
            }

            var isDevelopment = environment.EqualsIgnoreCase(Environments.Development);
            ModuleAssemblyLoader.Initialize(isDevelopment);

            foreach (var module in modules.Where(m => !string.IsNullOrEmpty(m.Ref)))
            {
                ModuleAssemblyLoader.LoadModule(module, probingPath);
            }

            ModuleDiscovery.ValidateModules(modules, PlatformVersion.CurrentVersion);
            ModuleRegistry.Initialize(modules);

            // Discover IPlatformStartup implementations from loaded modules
            PlatformStartupDiscovery.DiscoverStartups(modules);
        }

        private static IConfigurationRoot GetLoggerConfiguration(IConfigurationRoot bootConfig)
        {
            // Create Serilog bootstrap logger for early module loading (before DI)
            var settings = new Dictionary<string, string>();

            foreach (var (key, value) in bootConfig.GetSection("Serilog").AsEnumerable())
            {
                if (value != null &&
                    !key.StartsWith("Serilog:Using:") &&
                    !key.StartsWith("Serilog:WriteTo:"))
                {
                    settings[key] = value;
                }
            }

            // Add only platform-bundled sinks
            settings["Serilog:Using:0"] = "Serilog.Sinks.Console";
            settings["Serilog:Using:1"] = "Serilog.Sinks.Debug";
            settings["Serilog:WriteTo:0"] = "Console";
            settings["Serilog:WriteTo:1"] = "Debug";

            return new ConfigurationBuilder()
                .AddInMemoryCollection(settings)
                .Build();
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                .ConfigureLogging((_, logging) =>
                {
                    logging.ClearProviders();
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                    webBuilder.ConfigureKestrel((_, options) => { options.Limits.MaxRequestBodySize = null; });

                    webBuilder.ConfigureAppConfiguration((context, configurationBuilder) =>
                    {
                        // Let modules add configuration sources (e.g., Azure App Configuration)
                        PlatformStartupDiscovery.RunConfigureAppConfiguration(
                            PlatformStartupDiscovery.GetStartups(),
                            configurationBuilder,
                            context.HostingEnvironment);
                    });
                })
                .ConfigureServices((hostingContext, services) =>
                {
                    // Let modules register host-level services
                    PlatformStartupDiscovery.RunConfigureHostServices(
                        PlatformStartupDiscovery.GetStartups(),
                        services,
                        hostingContext.Configuration);

                    //Conditionally use the hangFire server for this app instance to have possibility to disable processing background jobs
                    if (hostingContext.Configuration.GetValue("VirtoCommerce:Hangfire:UseHangfireServer", true))
                    {
                        // Add & start hangfire server immediately.
                        // We do this there after all services initialize, to have dependencies in hangfire tasks correctly resolved.
                        // Hangfire uses the ASP.NET HostedServices to host job background processing tasks.
                        // According to the official documentation https://docs.microsoft.com/en-us/aspnet/core/fundamentals/host/hosted-services?view=aspnetcore-3.1&tabs=visual-studio#ihostedservice-interface,
                        // in order to change running hosted services after the app's pipeline, we need to place AddHangfireServer here instead of Startup.
                        services.AddHangfireServer(options =>
                        {
                            var queues = hostingContext.Configuration.GetSection("VirtoCommerce:Hangfire:Queues").Get<List<string>>();
                            if (!queues.IsNullOrEmpty())
                            {
                                queues.Add("default");
                                options.Queues = queues.Select(x => x.ToLower()).Distinct().ToArray();
                            }

                            var workerCount = hostingContext.Configuration.GetValue<int?>("VirtoCommerce:Hangfire:WorkerCount", null);
                            if (workerCount != null)
                            {
                                options.WorkerCount = workerCount.Value;
                            }
                        });
                    }
                });
        }
    }
}
