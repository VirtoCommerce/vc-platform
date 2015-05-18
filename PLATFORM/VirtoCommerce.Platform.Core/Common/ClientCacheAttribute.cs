using System;
using System.Net.Http.Headers;
using System.Web.Http.Filters;

namespace VirtoCommerce.Platform.Core.Common
{
    public class ClientCacheAttribute : ActionFilterAttribute
    {
        public int Duration { get; set; }

        public bool MustRevalidate { get; set; }

        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
			if (actionExecutedContext != null && actionExecutedContext.Response != null)
			{
				actionExecutedContext.Response.Headers.CacheControl = new CacheControlHeaderValue
																	  {
																		  MaxAge = TimeSpan.FromSeconds(Duration),
																		  MustRevalidate = MustRevalidate,
																		  Public = true
																	  };
			}
        }
    }
}
