using System;
using Microsoft.Owin.Extensions;
using Owin;

namespace VirtoCommerce.Platform.Data.Security.Authentication.Hmac
{
    public static class HmacAuthenticationExtensions
    {
        public static IAppBuilder UseHmacAuthentication(this IAppBuilder app, HmacAuthenticationOptions options)
        {
            if (app == null)
            {
                throw new ArgumentNullException("app");
            }

            app.Use<HmacAuthenticationMiddleware>(options);
            app.UseStageMarker(PipelineStage.Authenticate);
            return app;
        }
    }
}
