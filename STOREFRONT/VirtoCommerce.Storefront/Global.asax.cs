using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using NLog;
using VirtoCommerce.Storefront.Model;
using VirtoCommerce.Storefront.Model.Common;

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
                //Need redirect to requested store and language error pages  
                var urlBuilder = DependencyResolver.Current.GetService<IStorefrontUrlBuilder>();
                var workContext = DependencyResolver.Current.GetService<WorkContext>();

                switch (httpException.GetHttpCode())
                {
                    case 404:
                        Context.Response.Redirect(urlBuilder.ToAppRelative(workContext, "~/Errors/404", workContext.CurrentStore, workContext.CurrentLanguage));
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
