using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.Extensions.Options;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Telemetry;

namespace VirtoCommerce.Platform.Web.Telemetry
{
    /// <summary>
    /// Application insight telemetry processor which exclude all dependency SQL queries related to Hangfire.
    /// </summary>
    public class IgnoreSqlTelemetryProcessor : ITelemetryProcessor
    {
        private readonly IgnoreSqlTelemetryOptions _options;

        private ITelemetryProcessor Next { get; set; }

        // Link processors to each other in a chain.
        public IgnoreSqlTelemetryProcessor(IOptions<IgnoreSqlTelemetryOptions> options, ITelemetryProcessor next)
        {
            _options = options.Value;
            Next = next;
        }

        public void Process(ITelemetry item)
        {
            if (item is DependencyTelemetry dependencyTelemetry &&
                dependencyTelemetry.Type == "SQL")
            {
                foreach (var substring in _options.QueryIgnoreSubstrings)
                {
                    if (!(dependencyTelemetry.Data.IsNullOrEmpty()) && dependencyTelemetry.Data.Contains(substring))
                    {
                        // To filter out an item, just terminate the chain:
                        return;
                    }
                }
            }
            // Send everything else:
            Next.Process(item);
        }
    }
}
