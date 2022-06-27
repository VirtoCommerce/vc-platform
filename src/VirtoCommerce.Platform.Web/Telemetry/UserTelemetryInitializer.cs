using Microsoft.ApplicationInsights.AspNetCore.TelemetryInitializers;
using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using VirtoCommerce.Platform.Core.Telemetry;

namespace VirtoCommerce.Platform.Web.Telemetry
{
    /// <summary>
    /// A telemetry initializer that will gather user identity context information.
    /// </summary>
    public class UserTelemetryInitializer : TelemetryInitializerBase
    {
        private readonly IConfiguration _configuration;

        public UserTelemetryInitializer(IHttpContextAccessor httpContextAccessor, IConfiguration configuration)
            : base(httpContextAccessor)
        {
            _configuration = configuration;
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
            var aiVirtoOptionsSection = _configuration.GetSection("VirtoCommerce:ApplicationInsights");
            var aiSection = aiVirtoOptionsSection.Get<ApplicationInsightsOptions>() ?? new ApplicationInsightsOptions();
            if(!string.IsNullOrEmpty(aiSection.RoleName))
            {
                telemetry.Context.Cloud.RoleName = aiSection.RoleName;
            }

            if (!string.IsNullOrEmpty(aiSection.RoleInstance))
            {
                telemetry.Context.Cloud.RoleInstance = aiSection.RoleInstance;
            }
        }
    }
}
