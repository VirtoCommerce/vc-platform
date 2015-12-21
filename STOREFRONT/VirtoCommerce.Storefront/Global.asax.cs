using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using NLog;
using VirtoCommerce.Client.Client;
using VirtoCommerce.Storefront.Common;
using VirtoCommerce.Storefront.Controllers;

namespace VirtoCommerce.Storefront
{
    public class MvcApplication : System.Web.HttpApplication
    {
        /// <summary>
        /// We Use this method for generate current user id for caching keys
        /// </summary>
        /// <param name="context"></param>
        /// <param name="arg"></param>
        /// <returns></returns>
        public override string GetVaryByCustomString(HttpContext context, string arg)
        {
            if (arg.Equals("User", StringComparison.InvariantCultureIgnoreCase))
            {
                string userId = context.User.Identity.Name;
                if (!context.User.Identity.IsAuthenticated)
                {
                    var anonymousCookie = context.Request.Cookies.Get(StorefrontConstants.AnonymousCustomerIdCookie);
                    if(anonymousCookie != null)
                    {
                        userId = anonymousCookie.Value;
                    }
                }
                return string.Format("{0}", userId);
            }
            return base.GetVaryByCustomString(context, arg);
        }

        protected void Application_Start()
        {
        }
     
        protected void Application_Error(Object sender, EventArgs e)
        {
            Exception exception = Server.GetLastError();
            HttpException httpException = exception as HttpException;
            ApiException apiException = exception as ApiException;

            var isNotFound = false;
            if(apiException != null)
            {
                isNotFound = apiException.ErrorCode == 404;
            }
            else if(httpException != null)
            {
                isNotFound = httpException.GetHttpCode() == 404;
            }

            if (isNotFound)
            {
                RouteData routeData = new RouteData();
                routeData.Values.Add("controller", "Error");
                routeData.Values.Add("action", "Http404");
                // Clear the error, otherwise, we will always get the default error page.
                Server.ClearError();
                // Call the controller with the route
                IController errorController = new ErrorController();
                errorController.Execute(new RequestContext(new HttpContextWrapper(Context), routeData));
            }

            //Log exception
            var log = LogManager.GetCurrentClassLogger();
            log.Error(exception);

            //Response.Clear();
            //Server.ClearError();
            //Response.TrySkipIisCustomErrors = true;
        }

    }
}
