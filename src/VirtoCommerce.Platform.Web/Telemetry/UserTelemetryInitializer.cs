using Microsoft.ApplicationInsights.AspNetCore.TelemetryInitializers;
using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.AspNetCore.Http;

namespace VirtoCommerce.Platform.Web.Telemetry
{
    /// <summary>
    /// A telemetry initializer that will gather user identity context information.
    /// </summary>
    public class UserTelemetryInitializer : TelemetryInitializerBase
    {
        public UserTelemetryInitializer(IHttpContextAccessor httpContextAccessor)
            : base(httpContextAccessor)
        {
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
        }
    }
}
