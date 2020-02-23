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
            telemetry.Context.User.AuthenticatedUserId = httpContext?.User?.Identity?.Name;

            // Extend Request telemetry with X-Re   sponse-Time value.
            var requestTelemetry = telemetry as RequestTelemetry;

            if (httpContext != null && requestTelemetry != null)    
            {
                var xResponseTime = httpContext.Response.Headers[ResponseTimeHeaderFilterAttribute.XResponseTimeHeader];
                if(!string.IsNullOrEmpty(xResponseTime))
                    requestTelemetry.Properties.Add(ResponseTimeHeaderFilterAttribute.XResponseTimeHeader, xResponseTime);
            }
        }
    }
}
