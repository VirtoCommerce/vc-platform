#region

using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using VirtoCommerce.Web.Controllers;

#endregion

namespace VirtoCommerce.Web
{
    public class MvcApplication : HttpApplication
    {
        #region Methods

        protected void Application_Error(object sender, EventArgs e)
        {
            /*
            var exception = this.Server.GetLastError();
            this.Server.ClearError();

            var routeData = new RouteData();
            routeData.Values.Add("controller", "Errors");
            routeData.Values.Add("action", "Http500");
            routeData.Values.Add("exception", exception);

            var httpException = exception as HttpException;

            if (httpException != null)
            {
                var statusCode = httpException.GetHttpCode();

                if (statusCode == 404)
                {
                    routeData.Values["action"] = "Http404";
                }
            }

            IController controller = new ErrorsController();
            controller.Execute(new RequestContext(new HttpContextWrapper(this.Context), routeData));

            this.Response.End();
             * */
        }

        protected void Application_Start()
        {
            EnginesConfig.RegisterEngines(ViewEngines.Engines);
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }

        #endregion
    }
}
