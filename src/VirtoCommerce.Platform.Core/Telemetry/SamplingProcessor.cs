using System;

namespace VirtoCommerce.Platform.Core.Telemetry
{
    [Obsolete("Not used anymore. Moved to VirtoCommerce.ApplicationInsights module.")]
    public enum SamplingProcessor
    {
        Adaptive,
        Fixed
    }
}
