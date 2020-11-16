namespace VirtoCommerce.Platform.Core.Telemetry
{

    /// <summary>
    /// ApplicationInsights options
    /// </summary>
    public class ApplicationInsightsOptions
    {
        /// <summary>
        /// AppInsights sampling options
        /// </summary>
        public SamplingOptions SamplingOptions { get; set; } = new SamplingOptions();

        /// <summary>
        /// Enable SQL dependencies filtering ApplicationInsights processor
        /// </summary>
        public IgnoreSqlTelemetryOptions IgnoreSqlTelemetryOptions { get; set; }

        /// <summary>
        /// Force SQL reflection in dependencies for ApplicationInsights
        /// </summary>
        public bool EnableLocalSqlCommandTextInstrumentation { get; set; }

        /// <summary>
        /// Same as EnableLocalSqlCommandTextInstrumentation
        /// </summary>
        public bool EnableSqlCommandTextInstrumentation { get; set; }
    }
}
