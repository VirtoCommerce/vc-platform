using Hangfire;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

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
                  logging.AddConfiguration(hostingContext.Configuration.GetSection("Logging"));
                  logging.AddConsole();
                  logging.AddDebug();
                  logging.AddEventSourceLogger();
                  //Enable Azure logging
                  //https://docs.microsoft.com/en-us/aspnet/core/fundamentals/logging/?view=aspnetcore-2.2#logging-in-azure
                  logging.AddAzureWebAppDiagnostics();
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
                    services.AddHangfireServer();
                }
            });

    }
}
