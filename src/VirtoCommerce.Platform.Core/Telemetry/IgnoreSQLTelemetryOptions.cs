using System;

namespace VirtoCommerce.Platform.Core.Telemetry
{
    [Obsolete("Not used anymore. Moved to VirtoCommerce.ApplicationInsights module.")]
    public class IgnoreSqlTelemetryOptions
    {
        public string[] QueryIgnoreSubstrings { get; set; } = new string[] { };
    }
}
