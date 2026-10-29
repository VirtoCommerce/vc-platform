using System.Net;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using VirtoCommerce.Platform.Core;

namespace VirtoCommerce.Platform.Web.Extensions
{
    public static class EndpointRouteBuilderExtensions
    {
        /// <summary>
        /// The page at the site root when no admin UI owns it. The platform itself is API only: admin UI modules
        /// declare themselves in module.manifest (&lt;app&gt; with &lt;adminUI&gt;), and VirtoCommerce:PlatformUI:RootApp
        /// picks the one served at "/". <see cref="Infrastructure.AdminUI.AdminUIMatcherPolicy"/> chooses between this
        /// page and the admin UI pages mapped at "/".
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
                            <p>To use the back office, install an admin UI module: <code>VirtoCommerce.AdminAngularJs</code>, <code>VirtoCommerce.AdminShell</code> or another one.</p>
                            <ul>
                                <li><a href="{{pathBase}}/docs">API documentation</a></li>
                                <li><a href="{{pathBase}}/health">Health</a></li>
                            </ul>
                        </body>
                        </html>
                        """, "text/html; charset=utf-8");
                })
                .WithMetadata(new AdminUIPageAttribute(appId: null))
                .AllowAnonymous()
                .ExcludeFromDescription();
        }
    }
}
