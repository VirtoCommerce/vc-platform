using Microsoft.ApplicationInsights;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.ExceptionHandling;

namespace VirtoCommerce.Platform.Web.App_Start
{
    public class AiExceptionHandler : ExceptionHandler
    {
        public override void Handle(ExceptionHandlerContext context)
        {
            if (context != null && context.Exception != null)
            {
                var ai = new TelemetryClient();
                ai.TrackException(context.Exception);
            }
            base.Handle(context);
        }
    }
}