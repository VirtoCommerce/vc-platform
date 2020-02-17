using System.Diagnostics;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using System.Web.Mvc;
using VirtoCommerce.Platform.Web.Controllers;

namespace VirtoCommerce.Platform.Web
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
			filters.Add(new AiHandleErrorAttribute());

			System.Web.Http.GlobalConfiguration.Configuration.Filters.Add(new ResponseTimeHeaderFilter());
        }
    }
    /// <summary>
    /// Filter add X-Response-Time header to response contains elapsed response time in milliseconds 
    /// </summary>
    public class ResponseTimeHeaderFilter : System.Web.Http.Filters.ActionFilterAttribute
    {
        public const string XResponseTimeHeader = "X-Response-Time";

        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            actionContext.Request.Properties[XResponseTimeHeader] = Stopwatch.StartNew();
        }

        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            object timer;
            if (actionExecutedContext != null && actionExecutedContext.Request != null && actionExecutedContext.Request.Properties.TryGetValue(XResponseTimeHeader, out timer))
            {
                var stopWatch = timer as Stopwatch;
                if (stopWatch != null)
                {
                    stopWatch.Stop();
                    if (actionExecutedContext.Response != null)
                    {
                        actionExecutedContext.Response.Headers.Add(XResponseTimeHeader, stopWatch.ElapsedMilliseconds.ToString());
                    }
                }
            }
        }
    }
}
