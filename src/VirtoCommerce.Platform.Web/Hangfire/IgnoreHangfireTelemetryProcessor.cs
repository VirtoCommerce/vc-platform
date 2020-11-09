using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.ApplicationInsights.Extensibility;


namespace VirtoCommerce.Platform.Web.Hangfire
{
    /// <summary>
    /// Application insight telemetry processor which exclude all dependency SQL queries related to Hangfire.
    /// </summary>
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
            if (item is DependencyTelemetry dependencyTelemetry &&
                dependencyTelemetry.Type == "SQL" &&
                dependencyTelemetry.Data.Contains("HangFire"))
            {
                // To filter out an item, just terminate the chain:
                return;
            }
            // Send everything else:
            Next.Process(item);
        }
    }
}
