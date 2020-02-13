using System.Web;
using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.ApplicationInsights.Extensibility;

namespace VirtoCommerce.Platform.Web.ApplicationInsights
{
    [System.CLSCompliant(false)]
    public class CustomTelemetryInitializer : ITelemetryInitializer
    {
        public void Initialize(ITelemetry telemetry)
        {
            var httpContext = HttpContext.Current;

            // Extend All telemetry with user identity context information.
            if (httpContext != null && httpContext.User!=null)
            {
                 telemetry.Context.User.AuthenticatedUserId = httpContext.User.Identity.Name;
            }

            // Extend Request telemetry with X-Response-Time value.
            var requestTelemetry = telemetry as RequestTelemetry;

            if (httpContext != null && requestTelemetry != null)
            {
                var xRsponseTime = httpContext.Response.Headers["X-Response-Time"];
                requestTelemetry.Properties.Add("X-Response-Time", xRsponseTime);
            }
        }
    }
}
