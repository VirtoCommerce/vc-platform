namespace VirtoCommerce.Platform.Core.Telemetry
{
    /// <summary>
    /// AppInsights settings for fixed sampling
    /// </summary>
    public class FixedProcessorSettings
    {
        /// <summary>
        /// Data sampling percentage (between 0 and 100).
        /// All sampling percentage must be in a ratio of 100/N where N is a whole number (2, 3, 4, …). E.g. 50 for 1/2 or 33.33 for 1/3.
        /// Failure to follow this pattern can result in unexpected / incorrect computation of values in the portal.
        /// </summary>
        public double SamplingPercentage { get; set; } = 100;
    }
}
