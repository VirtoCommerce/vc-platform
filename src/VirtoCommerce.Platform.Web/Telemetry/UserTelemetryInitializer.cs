using Microsoft.ApplicationInsights.AspNetCore.TelemetryInitializers;
using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using VirtoCommerce.Platform.Core.Telemetry;

namespace VirtoCommerce.Platform.Web.Telemetry
{
    /// <summary>
    /// A telemetry initializer that will gather user identity context information.
    /// </summary>
    public class UserTelemetryInitializer : TelemetryInitializerBase
    {
        private readonly ApplicationInsightsOptions _options;

        public UserTelemetryInitializer(IHttpContextAccessor httpContextAccessor, IOptions<ApplicationInsightsOptions> options)
            : base(httpContextAccessor)
        {
            _options = options?.Value ?? new ApplicationInsightsOptions();
        }

        /// <summary>
        /// Initializes user id and name for Azure Web App case
        /// </summary>
        /// <param name="platformContext">Platform context.</param>
        /// <param name="requestTelemetry">Request telemetry.</param>
        /// <param name="telemetry">Telemetry item.</param>
        protected override void OnInitializeTelemetry(HttpContext platformContext, RequestTelemetry requestTelemetry, ITelemetry telemetry)
        {
            if (platformContext.User?.Identity?.Name != null)
            {
                telemetry.Context.User.AuthenticatedUserId = platformContext.User.Identity.Name;
            }
            if(!string.IsNullOrEmpty(_options.RoleName))
            {
                telemetry.Context.Cloud.RoleName = _options.RoleName;
            }

            if (!string.IsNullOrEmpty(_options.RoleInstance))
            {
                telemetry.Context.Cloud.RoleInstance = _options.RoleInstance;
            }
        }
    }
}
