using System;
using Microsoft.Owin.Extensions;
using Owin;

namespace VirtoCommerce.Storefront.Owin
{
    public static class WorkContextExtensions
    {
        public static IAppBuilder UseWorkContext(this IAppBuilder app, WorkContextOptions options)
        {
            if (app == null)
            {
                throw new ArgumentNullException("app");
            }

            app.Use<WorkContextOwinMiddleware>(options);
            app.UseStageMarker(PipelineStage.ResolveCache);

            return app;
        }
    }
}
