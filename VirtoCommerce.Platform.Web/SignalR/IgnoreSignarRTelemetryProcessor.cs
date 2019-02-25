using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.ApplicationInsights.Extensibility;

namespace VirtoCommerce.Platform.Web.SignalR
{
    /// <summary>
    /// Application insight telemetry processor which exclude all SignarR requests from statistic.
    /// </summary>
    [CLSCompliant(false)]
    public class IgnoreSignarRTelemetryProcessor : ITelemetryProcessor
    {
        private ITelemetryProcessor Next { get; set; }

        // Link processors to each other in a chain.
        public IgnoreSignarRTelemetryProcessor(ITelemetryProcessor next)
        {
            Next = next;
        }

        public void Process(ITelemetry item)
        {
            var request = item as RequestTelemetry;

            if (request != null && request.Url != null && request.Url.AbsolutePath.ToLowerInvariant().Contains("/signalr/"))
            {
                // To filter out an item, just terminate the chain:
                return;
            }
            // Send everything else:
            Next.Process(item);
        }
    }
}
