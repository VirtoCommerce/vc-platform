using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.ApplicationInsights.Extensibility;


namespace VirtoCommerce.Platform.Web.Hangfire
{
    public class IgnoreHangfireTelemetryProcessor : ITelemetryProcessor
    {
        private ITelemetryProcessor Next { get; set; }

        // Link processors to each other in a chain.
        public IgnoreHangfireTelemetryProcessor(ITelemetryProcessor next)
        {
            Next = next;
        }

        public void Process(ITelemetry item)
        {
            var dependencyTelemetry = item as DependencyTelemetry;
            if (dependencyTelemetry != null && dependencyTelemetry.Type == "SQL" && dependencyTelemetry.Data.Contains("HangFire"))
            {
                // To filter out an item, just terminate the chain:
                return;
            }
            // Send everything else:
            Next.Process(item);
        }
    }
}
