using System.Diagnostics;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using System.Web.Mvc;

namespace VirtoCommerce.Platform.Web
{
	public class FilterConfig
	{
		public static void RegisterGlobalFilters(GlobalFilterCollection filters)
		{
			filters.Add(new HandleErrorAttribute());
            System.Web.Http.GlobalConfiguration.Configuration.Filters.Add(new ResponseTimeHeaderFilter());
        }
	}
    /// <summary>
    /// Filter add X-Response-Time header to response contains elapsed response time in milliseconds 
    /// </summary>
    public class ResponseTimeHeaderFilter : System.Web.Http.Filters.ActionFilterAttribute
    {
        private Stopwatch _timer { get; set; }

        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            _timer = Stopwatch.StartNew();
         }

        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            if (_timer != null)
            {
                _timer.Stop();
                if (actionExecutedContext != null && actionExecutedContext.Response != null && actionExecutedContext.Response.Headers != null)
                {
                    actionExecutedContext.Response.Headers.Add("X-Response-Time", _timer.ElapsedMilliseconds.ToString());
                }
            }
        }
    }
}