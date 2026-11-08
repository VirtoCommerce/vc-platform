using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Core.Infrastructure;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace VirtoCommerce.Platform.Web.Security;

/// <summary>
/// A consent form whose antiforgery token is refused, because the page was open too long or the browser replaced its
/// cookie, is shown again with a notice instead of an empty 400: the browser goes back to the same authorization
/// request, marked by a short-lived cookie that the consent page reads.
/// </summary>
[AttributeUsage(AttributeTargets.Method)]
public sealed class ShowConsentAgainOnExpiredFormAttribute : Attribute, IAsyncAlwaysRunResultFilter
{
    public const string CookieName = ".VirtoCommerce.ConsentExpired";

    public Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
    {
        if (context.Result is IAntiforgeryValidationFailedResult)
        {
            var httpContext = context.HttpContext;
            var request = httpContext.Request;
            var tokenField = httpContext.RequestServices.GetService<IOptions<AntiforgeryOptions>>()?.Value.FormFieldName;

            // The parameters of the authorization request, without the refused token and the pressed button
            var parameters = request.HasFormContentType
                ? request.Form.Where(x => x.Key != tokenField && !x.Key.StartsWith("submit.", StringComparison.Ordinal)).ToList()
                : [];

            httpContext.RequestServices.GetService<ILogger<ShowConsentAgainOnExpiredFormAttribute>>()?
                .LogWarning("The OAuth consent form was submitted with an invalid antiforgery token, so it is shown again");

            var options = CreateCookieOptions(request);
            options.MaxAge = TimeSpan.FromMinutes(5);
            httpContext.Response.Cookies.Append(CookieName, "1", options);

            context.Result = new LocalRedirectResult(request.PathBase + request.Path + QueryString.Create(parameters));
        }

        return next();
    }

    /// <summary>
    /// Whether a submit of the consent form of this authorization request was refused. The mark is removed, so the
    /// consent page shows its notice once.
    /// </summary>
    public static bool TakeMark(HttpContext httpContext)
    {
        if (!httpContext.Request.Cookies.ContainsKey(CookieName))
        {
            return false;
        }

        httpContext.Response.Cookies.Delete(CookieName, CreateCookieOptions(httpContext.Request));

        return true;
    }

    private static CookieOptions CreateCookieOptions(HttpRequest request)
    {
        return new CookieOptions
        {
            Path = request.PathBase + request.Path,
            HttpOnly = true,
            // As the platform's sign-in cookie: browsers keep a secure cookie for http://localhost too
            Secure = true,
            SameSite = SameSiteMode.Lax,
            // Part of signing in, not tracking: the platform's cookie policy drops other cookies until consent
            IsEssential = true,
        };
    }
}
