using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.ApplicationInsights.Extensibility;

namespace VirtoCommerce.Platform.Web.Telemetry
{
    /// <summary>
    /// Application insight telemetry processor which exclude all SignalR requests from statistic.
    /// </summary>
    public class IgnoreSignalRTelemetryProcessor : ITelemetryProcessor
    {
        private ITelemetryProcessor Next { get; set; }

        // Link processors to each other in a chain.
        public IgnoreSignalRTelemetryProcessor(ITelemetryProcessor next)
        {
            Next = next;
        }

        public void Process(ITelemetry item)
        {
            if (item is RequestTelemetry request && request.Url.AbsolutePath.ToLowerInvariant().Contains("/pushnotificationhub"))
            {
                // To filter out an item, just terminate the chain:
                return;
            }
            // Send everything else:
            Next.Process(item);
        }
    }
}
