using System.Net;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace VirtoCommerce.Platform.Web.Extensions
{
    public static class EndpointRouteBuilderExtensions
    {
        /// <summary>
        /// The page at the site root when no admin UI module maps one. The platform itself is API only:
        /// an admin UI module (VirtoCommerce.AdminUI or an alternative) maps its own page at "/", and the
        /// lowest possible order lets that page win without the module having to know about this one.
        /// </summary>
        public static IEndpointConventionBuilder MapPlatformLandingPage(this IEndpointRouteBuilder endpoints)
        {
            return endpoints
                .MapGet("/", (HttpContext context) =>
                {
                    var pathBase = WebUtility.HtmlEncode(context.Request.PathBase.Value);

                    return Results.Content($$"""
                        <!DOCTYPE html>
                        <html lang="en">
                        <head>
                            <meta charset="utf-8" />
                            <meta name="viewport" content="width=device-width, initial-scale=1.0" />
                            <title>Virto Commerce Platform</title>
                            <style>
                                body { font-family: system-ui, sans-serif; margin: 2rem; color: #262626; }
                                code { background: #f2f2f2; padding: 0 .25rem; }
                            </style>
                        </head>
                        <body>
                            <h1>Virto Commerce Platform</h1>
                            <p>The platform is running. No admin UI is installed on this instance.</p>
                            <p>To use the back office, install the <code>VirtoCommerce.AdminUI</code> module or another admin UI module.</p>
                            <ul>
                                <li><a href="{{pathBase}}/docs">API documentation</a></li>
                                <li><a href="{{pathBase}}/health">Health</a></li>
                            </ul>
                        </body>
                        </html>
                        """, "text/html; charset=utf-8");
                })
                .WithOrder(int.MaxValue)
                .AllowAnonymous()
                .ExcludeFromDescription();
        }
    }
}
