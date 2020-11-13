using Microsoft.ApplicationInsights.DependencyCollector;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using VirtoCommerce.Platform.Core.Telemetry;

namespace VirtoCommerce.Platform.Web.Telemetry
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Charge AppInsights options and add the telemetry
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static IServiceCollection AddAppInsightsTelemetry(this IServiceCollection services, IConfiguration configuration)
        {
            // The following lines enables Application Insights telemetry collection.
            var aiOptions = new Microsoft.ApplicationInsights.AspNetCore.Extensions.ApplicationInsightsServiceOptions();
            // Disable adaptive sampling before custom configuration to have a choice between processors in Configure,
            // according to instructions: https://docs.microsoft.com/en-us/azure/azure-monitor/app/sampling#configure-sampling-settings
            aiOptions.EnableAdaptiveSampling = false;
            services.AddApplicationInsightsTelemetry(aiOptions);

            // Always ignore SignalRTelemetry
            services.AddApplicationInsightsTelemetryProcessor<IgnoreSignalRTelemetryProcessor>();

            // Charge sampling options to enable custom configuration of sampling processors
            services.AddOptions<SamplingOptions>().Bind(configuration.GetSection("VirtoCommerce:ApplicationInsights:SamplingOptions"));

            // Enable SQL dependencies filtering, if need
            var ignoreSqlTelemetryOptionsSection = configuration.GetSection("VirtoCommerce:ApplicationInsights:IgnoreSqlTelemetryOptions");
            if (ignoreSqlTelemetryOptionsSection.Exists())
            {
                services.AddOptions<IgnoreSqlTelemetryOptions>().Bind(ignoreSqlTelemetryOptionsSection);
                services.AddApplicationInsightsTelemetryProcessor<IgnoreSqlTelemetryProcessor>();
            }

            // Force SQL reflection in dependencies, if need
            if (configuration["VirtoCommerce:ApplicationInsights:EnableLocalSqlCommandTextInstrumentation"]?.ToLower() == "true" ||
                configuration["VirtoCommerce:ApplicationInsights:EnableSqlCommandTextInstrumentation"]?.ToLower() == "true")
            {
                // Next line forces to gather detailed SQL info for AI
                // See instructions here: https://docs.microsoft.com/en-us/azure/azure-monitor/app/asp-net-dependencies#advanced-sql-tracking-to-get-full-sql-query
                services.ConfigureTelemetryModule<DependencyTrackingTelemetryModule>((module, o) => { module.EnableSqlCommandTextInstrumentation = true; });
            }

            return services;
        }
    }
}
