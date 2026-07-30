using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.Net.Http.Headers;

namespace VirtoCommerce.Platform.Web.Security.Authentication
{
    /// <summary>
    /// Handles cookie authentication challenges.
    /// API requests get a status code, so that API clients (including the admin SPA) can detect an expired
    /// session instead of transparently following the redirect and receiving the login page HTML with 200 OK.
    /// Browser navigation keeps redirecting to the login page, which the OpenID Connect authorization
    /// endpoint (/connect/authorize) relies on.
    /// </summary>
    public static class ApiCookieRedirectHandler
    {
        public const string ApiPathPrefix = "/api";
        private const string XmlHttpRequest = "XMLHttpRequest";

        public static Task HandleAsync(RedirectContext<CookieAuthenticationOptions> context, int statusCode)
        {
            if (IsApiRequest(context.Request))
            {
                context.Response.StatusCode = statusCode;
            }
            else
            {
                context.Response.Redirect(context.RedirectUri);
            }

            return Task.CompletedTask;
        }

        /// <summary>
        /// A request is treated as an API request when it targets the API path prefix, or when the client marks it
        /// as an AJAX request. The latter preserves the behavior of the replaced built-in handler, which SignalR
        /// hub negotiation depends on.
        /// </summary>
        public static bool IsApiRequest(HttpRequest request)
        {
            return request.Path.StartsWithSegments(ApiPathPrefix, StringComparison.OrdinalIgnoreCase) ||
                   request.Headers[HeaderNames.XRequestedWith] == XmlHttpRequest;
        }
    }
}
