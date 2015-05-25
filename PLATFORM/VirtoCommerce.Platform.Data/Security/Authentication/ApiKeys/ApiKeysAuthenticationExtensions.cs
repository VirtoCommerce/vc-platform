using System;
using Microsoft.Owin.Extensions;
using Owin;

namespace VirtoCommerce.Platform.Data.Security.Authentication.ApiKeys
{
    public static class ApiKeysAuthenticationExtensions
    {
        public static IAppBuilder UseApiKeysAuthentication(this IAppBuilder app, ApiKeysAuthenticationOptions options)
        {
            if (app == null)
            {
                throw new ArgumentNullException("app");
            }

            app.Use<ApiKeysAuthenticationMiddleware>(options);
            app.UseStageMarker(PipelineStage.Authenticate);
            return app;
        }
    }
}
