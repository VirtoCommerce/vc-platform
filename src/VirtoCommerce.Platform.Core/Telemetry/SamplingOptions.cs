using Microsoft.ApplicationInsights.WindowsServer.Channel.Implementation;

namespace VirtoCommerce.Platform.Core.Telemetry
{
    /// <summary>
    /// AppInsights sampling options
    /// </summary>
    public class SamplingOptions
    {
        /// <summary>
        /// Type of sampling processor: adaptive or fixed. See https://docs.microsoft.com/en-us/azure/azure-monitor/app/sampling
        /// </summary>
        public SamplingProcessor Processor { get; set; } = SamplingProcessor.Adaptive;
        /// <summary>
        /// Adaptive sampling settings (if Processor==SamplingProcessor.Adaptive)
        /// </summary>
        public SamplingPercentageEstimatorSettings Adaptive { get; set; } = new SamplingPercentageEstimatorSettings();
        /// <summary>
        /// Fixed sampling settings (if Processor==SamplingProcessor.Fixed)
        /// </summary>
        public FixedProcessorSettings Fixed { get; set; } = new FixedProcessorSettings();
        /// <summary>
        /// A semi-colon delimited list of types that you do want to subject to sampling.
        /// Recognized types are: Dependency, Event, Exception, PageView, Request, Trace. The specified types will be sampled.
        /// All types included by default.
        /// </summary>
        public string IncludedTypes { get; set; } = "Dependency;Event;Exception;PageView;Request;Trace";
        /// <summary>
        /// A semi-colon delimited list of types that you do not want to be subject to sampling.
        /// Recognized types are: Dependency, Event, Exception, PageView, Request, Trace.
        /// Empty by default.
        /// </summary>
        public string ExcludedTypes { get; set; }
    }
}
