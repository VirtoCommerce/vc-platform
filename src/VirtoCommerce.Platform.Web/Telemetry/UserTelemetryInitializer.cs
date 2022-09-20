using Microsoft.ApplicationInsights.AspNetCore.TelemetryInitializers;
using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using VirtoCommerce.Platform.Core.Telemetry;
using VirtoCommerce.Platform.Data.Repositories;

namespace VirtoCommerce.Platform.Web.Telemetry
{
    /// <summary>
    /// A telemetry initializer that will gather user identity context information.
    /// </summary>
    public class UserTelemetryInitializer : ITelemetryInitializer
    {
        private readonly ApplicationInsightsOptions _options;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserTelemetryInitializer(IHttpContextAccessor httpContextAccessor, IOptions<ApplicationInsightsOptions> options)            
        {
            _options = options?.Value ?? new ApplicationInsightsOptions();
            _httpContextAccessor = httpContextAccessor;
        }

        public void Initialize(ITelemetry telemetry)
        {
            var userId = _httpContextAccessor?.HttpContext?.User?.Identity?.Name;

            if (userId != null)
            {
                telemetry.Context.User.AuthenticatedUserId = userId;
            }
            if (!string.IsNullOrEmpty(_options.RoleName))
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
