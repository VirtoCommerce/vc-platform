using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using NLog;
using VirtoCommerce.Storefront.Controllers;

namespace VirtoCommerce.Storefront
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
        }

        protected void Application_Error(Object sender, EventArgs e)
        {
            Exception exception = Server.GetLastError();
            HttpException httpException = Server.GetLastError() as HttpException;

            if (httpException != null)
            {
                 switch (httpException.GetHttpCode())
                {
                    case 404:
                        RouteData routeData = new RouteData();
                        routeData.Values.Add("controller", "Error");
                        routeData.Values.Add("action", "Http404");
                        // Clear the error, otherwise, we will always get the default error page.
                        Server.ClearError();

                        // Call the controller with the route
                        IController errorController = new ErrorController();
                        errorController.Execute(new RequestContext(new HttpContextWrapper(Context), routeData));
                    
                        break;
                }
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
