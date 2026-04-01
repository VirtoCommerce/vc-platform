using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using Hangfire;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Extensions.Logging;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Modularity;
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

            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(GetLoggerConfiguration(bootConfig))
                .CreateBootstrapLogger();

            var loggerFactory = new SerilogLoggerFactory(Log.Logger);

            var options = bootConfig.GetSection("VirtoCommerce").Get<LocalStorageModuleCatalogOptions>();
            options.DiscoveryPath = Path.GetFullPath(options.DiscoveryPath);
            options.ProbingPath = Path.GetFullPath(options.ProbingPath);

            var isDevelopmentEnvironment = environment.EqualsIgnoreCase(Environments.Development);

            ModuleBootstrapper.Instance = new ModuleBootstrapper(loggerFactory, options)
                .Discover(PlatformVersion.CurrentVersion)
                .Copy(RuntimeInformation.ProcessArchitecture)
                .Load(isDevelopmentEnvironment);
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
                        ModuleBootstrapper.Instance.RunConfigureAppConfiguration(
                            configurationBuilder,
                            context.HostingEnvironment);
                    });
                })
                .ConfigureServices((hostingContext, services) =>
                {
                    // Let modules register host-level services
                    ModuleBootstrapper.Instance.RunConfigureHostServices(
                        services,
                        hostingContext.Configuration);

                    //Conditionally use the hangFire server for this app instance to have possibility to disable processing background jobs
                    if (hostingContext.Configuration.GetValue("VirtoCommerce:Hangfire:UseHangfireServer", true))
                    {
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
