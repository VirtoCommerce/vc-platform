using System.Collections.Generic;
using System.Linq;
using Hangfire;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.AzureAppConfiguration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Logger;
using VirtoCommerce.Platform.Web.Extensions;

namespace VirtoCommerce.Platform.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
           Host.CreateDefaultBuilder(args)
            .ConfigureLogging((hostingContext, logging) =>
            {
                logging.ClearProviders();
            })
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<Startup>();
                webBuilder.ConfigureKestrel((context, options) => { options.Limits.MaxRequestBodySize = null; });

                webBuilder.ConfigureAppConfiguration((context, configurationBuilder) =>
                {
                    var configuration = configurationBuilder.Build();

                    // Load configuration from Azure App Configuration
                    // Azure App Configuration will be loaded last i.e. it will override any existing sections
                    // configuration loads all keys that have no label and keys that have label based on the environment (Development, Production etc)
                    if (configuration.TryGetAzureAppConfigurationConnectionString(out var connectionString))
                    {
                        configurationBuilder.AddAzureAppConfiguration(options =>
                        {
                            options
                            .Connect(connectionString)
                            .Select(KeyFilter.Any)
                            .Select(KeyFilter.Any, context.HostingEnvironment.EnvironmentName)
                            .ConfigureRefresh(refreshOptions =>
                            {
                                // Reload all configuration values if the "Sentinel" key value is modified
                                refreshOptions.Register("Sentinel", refreshAll: true);
                            });
                        });
                    }
                });

            })
            .ConfigureServices((hostingContext, services) =>
            {
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
            })
            .UseSerilog((context, services, loggerConfiguration) =>
            {
                // read from configuration
                _ = loggerConfiguration.ReadFrom.Configuration(context.Configuration);

                // enrich configuration from external sources
                var configurationServices = services.GetService<IEnumerable<ILoggerConfigurationService>>();
                foreach (var service in configurationServices)
                {
                    service.Configure(loggerConfiguration);
                }
            });
    }
}
