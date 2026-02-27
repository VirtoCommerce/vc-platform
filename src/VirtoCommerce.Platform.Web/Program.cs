using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Hangfire;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Modularity;
using VirtoCommerce.Platform.Modules;

namespace VirtoCommerce.Platform.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            // Early discovery of IPlatformStartup types from module manifests.
            // This runs before DI is configured, so we read paths from a temporary configuration.
            var tempConfig = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: true)
                .AddEnvironmentVariables()
                .Build();

            var discoveryPath = Path.GetFullPath(tempConfig.GetValue("VirtoCommerce:DiscoveryPath", "modules"));
            var probingPath = Path.GetFullPath(tempConfig.GetValue("VirtoCommerce:ProbingPath", "app_data/modules"));
            var platformStartups = PlatformStartupDiscovery.DiscoverStartups(discoveryPath, probingPath);

            return Host.CreateDefaultBuilder(args)
                .ConfigureLogging((_, logging) =>
                {
                    logging.ClearProviders();
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    // Register discovered startups before UseStartup<Startup>() so they are
                    // available in the service collection when Startup.ConfigureServices runs.
                    // Host-level ConfigureServices (below) executes after Startup.ConfigureServices,
                    // so the registration must happen here.
                    webBuilder.ConfigureServices(services =>
                    {
                        services.AddSingleton<IReadOnlyList<IPlatformStartup>>(platformStartups);
                    });

                    webBuilder.UseStartup<Startup>();
                    webBuilder.ConfigureKestrel((_, options) => { options.Limits.MaxRequestBodySize = null; });

                    webBuilder.ConfigureAppConfiguration((context, configurationBuilder) =>
                    {
                        // Invoke IPlatformStartup.ConfigureAppConfiguration from discovered modules
                        foreach (var startup in platformStartups)
                        {
                            startup.ConfigureAppConfiguration(configurationBuilder, context.HostingEnvironment);
                        }

                        var configuration = configurationBuilder.Build();
                    });
                })
                .ConfigureServices((hostingContext, services) =>
                {
                    // Invoke IPlatformStartup.ConfigureHostServices from discovered modules
                    foreach (var startup in platformStartups)
                    {
                        startup.ConfigureHostServices(services, hostingContext.Configuration);
                    }

                    //Conditionally use the hangFire server for this app instance to have possibility to disable processing background jobs
                    // TODO: Move to a dedicated IPlatformStartup module
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
