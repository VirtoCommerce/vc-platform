using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;

namespace VirtoCommerce.Platform.Caching;

/// <summary>
/// Attaches cache lookup totals to the HTTP server activity, including lookups inside child activities.
/// </summary>
public class CacheMetricsMiddleware(RequestDelegate next)
{
    public Task InvokeAsync(HttpContext context)
    {
        var activity = context.Features.Get<IHttpActivityFeature>()?.Activity;
        return activity is { IsAllDataRequested: true, Recorded: true }
            ? InvokeWithMetricsAsync(context, activity)
            : next(context);
    }

    private async Task InvokeWithMetricsAsync(HttpContext context, Activity activity)
    {
        // The async builder scopes the AsyncLocal write and restores the caller's ExecutionContext.
        using var metrics = CacheRequestMetrics.Begin(activity);
        await next(context);
    }
}
