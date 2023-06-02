using System.Collections.Generic;
using System.Linq;
using Hangfire;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Logger;

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
