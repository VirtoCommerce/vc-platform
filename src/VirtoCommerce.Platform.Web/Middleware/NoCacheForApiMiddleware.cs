using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace VirtoCommerce.Platform.Web.Middleware;

public class NoCacheForApiMiddleware
{
    private const string NoCache = "no-cache";
    private const string ExpiresMinusOne = "-1";

    private readonly RequestDelegate _next;

    public NoCacheForApiMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public Task InvokeAsync(HttpContext context)
    {
        // Apply Cache-Control header for API responses
        if (context.Request.Path.StartsWithSegments("/api", StringComparison.OrdinalIgnoreCase))
        {
            context.Response.Headers.CacheControl = NoCache;
            context.Response.Headers.Pragma = NoCache;
            context.Response.Headers.Expires = ExpiresMinusOne;
        }

        // Proceed with the request pipeline
        return _next(context);
    }
}
