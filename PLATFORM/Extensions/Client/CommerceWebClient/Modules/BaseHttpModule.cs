using System;
using System.Linq;
using Microsoft.Practices.ServiceLocation;
using System.Web;
using VirtoCommerce.Foundation.Customers;
using VirtoCommerce.Foundation.Customers.Services;

namespace VirtoCommerce.Web.Client.Modules
{
    /// <summary>
    /// Class BaseHttpModule.
    /// </summary>
    public abstract class BaseHttpModule : IHttpModule
    {
        /// <summary>
        /// Gets the customer session.
        /// </summary>
        /// <value>The customer session.</value>
        public virtual ICustomerSession CustomerSession
        {
            get
            {
                var session = ServiceLocator.Current.GetInstance<ICustomerSessionService>();
                return session.CustomerSession;
            }
        }


        /// <summary>
        /// Determines whether [is request authenticated] [the specified context].
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns>
        ///   <c>true</c> if [is request authenticated] [the specified context]; otherwise, <c>false</c>.
        /// </returns>
        protected virtual bool IsRequestAuthenticated(HttpContext context)
        {
            return context.Request.IsAuthenticated;
        }


        /// <summary>
        /// Determines whether [is resource file].
        /// </summary>
        /// <returns><c>true</c> if [is resource file]; otherwise, <c>false</c>.</returns>
        protected virtual bool IsResourceFile()
        {
            var path = HttpContext.Current.Request.Url.PathAndQuery.ToLower();
            var appPath = HttpContext.Current.Request.ApplicationPath;

            if (appPath != null && appPath.Length > 1)
                appPath = appPath + "/";

            if (path.StartsWith(appPath + "bundles") ||
               path.StartsWith(appPath + "content") ||
               path.StartsWith(appPath + "scripts") ||
               path.StartsWith(appPath + "error") ||
               path.StartsWith(appPath + "asset") ||
               path.StartsWith(appPath + "signalr") ||
               path.StartsWith(appPath + "admin") ||
               path.StartsWith(appPath + "areas/virtoadmin") ||
               path.StartsWith(appPath + "virto/dataservices") ||
               path.StartsWith(appPath + "virto/services"))
            {
                return true;
            }

            return false;
        }

        public virtual bool IsAjax
        {
            get
            {
                var request = HttpContext.Current.Request;
                if (request == null)
                {
                    return false;
                }
                return (request["X-Requested-With"] == "XMLHttpRequest") || ((request.Headers != null) && (request.Headers["X-Requested-With"] == "XMLHttpRequest"));
            }
        }

        public virtual bool IsWebApi
        {
            get
            {
                return
                    !HttpContext.Current.Request.Headers.AllKeys.Any(k=>k.Equals("forcedwebapi", StringComparison.OrdinalIgnoreCase)) &&
                    HttpContext.Current.Request.RequestContext.RouteData.Route != null &&
                    HttpContext.Current.Request.RequestContext.RouteData.Route.GetType()
                        .Name.Equals("HttpWebRoute", StringComparison.OrdinalIgnoreCase);
            }
        }

        /// <summary>
        /// Disposes of the resources (other than memory) used by the module that implements <see cref="T:System.Web.IHttpModule" />.
        /// </summary>
        public abstract void Dispose();
        /// <summary>
        /// Initializes a module and prepares it to handle requests.
        /// </summary>
        /// <param name="context">An <see cref="T:System.Web.HttpApplication" /> that provides access to the methods, properties, and events common to all application objects within an ASP.NET application</param>
        public abstract void Init(HttpApplication context);
    }
}
