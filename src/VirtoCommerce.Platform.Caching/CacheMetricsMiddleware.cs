using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;

namespace VirtoCommerce.Platform.Caching;

/// <summary>
/// Attaches cache lookup totals to the HTTP server activity, including lookups inside child activities.
/// </summary>
public class CacheMetricsMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext context)
    {
        using var metrics = CacheRequestMetrics.Begin(context.Features.Get<IHttpActivityFeature>()?.Activity);
        await next(context);
    }
}
