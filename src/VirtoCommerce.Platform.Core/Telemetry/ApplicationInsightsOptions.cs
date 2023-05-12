using System;

namespace VirtoCommerce.Platform.Core.Telemetry
{

    /// <summary>
    /// ApplicationInsights options
    /// </summary>
    [Obsolete("Not used anymore. Moved to VirtoCommerce.ApplicationInsights module.")]
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

        /// <summary>
        /// Cloud Role Name
        /// </summary>
        public string RoleName { get; set; }

        /// <summary>
        /// Cloud Role Instance
        /// </summary>
        public string RoleInstance { get; set; }
    }
}
