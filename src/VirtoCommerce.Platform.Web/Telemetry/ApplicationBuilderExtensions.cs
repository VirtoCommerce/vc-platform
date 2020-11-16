using System;
using System.Collections.Generic;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.ApplicationInsights.WindowsServer.TelemetryChannel;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using VirtoCommerce.Platform.Core.Telemetry;

namespace VirtoCommerce.Platform.Web.Telemetry
{
    public static class ApplicationBuilderExtensions
    {
        /// <summary>
        /// Configure AppInsights sampling accordingly to
        /// https://docs.microsoft.com/en-us/azure/azure-monitor/app/sampling#configure-sampling-settings
        /// </summary>
        /// <param name="app"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseAppInsightsTelemetry(this IApplicationBuilder app)
        {
            var samplingOptions = app.ApplicationServices.GetRequiredService<IOptions<ApplicationInsightsOptions>>().Value.SamplingOptions;

            var configuration = app.ApplicationServices.GetService<TelemetryConfiguration>();

            var builder = configuration.DefaultTelemetrySink.TelemetryProcessorChainBuilder;
            if (samplingOptions.Processor == SamplingProcessor.Adaptive)
            {
                // Using adaptive rate sampling
                builder.Use(x =>
                    new AdaptiveSamplingTelemetryProcessor(samplingOptions.Adaptive, null, x)
                    {
                        ExcludedTypes = samplingOptions.ExcludedTypes,
                        IncludedTypes = samplingOptions.IncludedTypes
                    }
                );
            }
            else
            {
                // Using fixed rate sampling
                builder.Use(x =>
                    new SamplingTelemetryProcessor(x)
                    {
                        SamplingPercentage = samplingOptions.Fixed.SamplingPercentage,
                        ExcludedTypes = samplingOptions.ExcludedTypes,
                        IncludedTypes = samplingOptions.IncludedTypes
                    }
                );
            }

            builder.Build();

            var telemetryProcessorsLogInfo = new Dictionary<string, ITelemetryProcessor>();
            foreach (var processor in configuration.DefaultTelemetrySink.TelemetryProcessors)
            {
                telemetryProcessorsLogInfo.Add(processor.GetType().Name, processor);
            }
            var telemetryProcessors = Newtonsoft.Json.JsonConvert.SerializeObject(telemetryProcessorsLogInfo, Newtonsoft.Json.Formatting.Indented);
            var logger = app.ApplicationServices.GetRequiredService<ILogger<Startup>>();
            logger.LogInformation($@"ApplicationInsights telemetry processors list and settings:{Environment.NewLine}{telemetryProcessors}{Environment.NewLine}");

            return app;
        }
    }
}
