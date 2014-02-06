using System;
using System.Activities.Statements;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using VirtoCommerce.Foundation.AppConfig;
using VirtoCommerce.Web.Client.Extensions;

namespace VirtoCommerce.Web.Areas.VirtoAdmin
{
    public class VirtoAdminAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "VirtoAdmin";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            var adminRoute = new Route("admin",
                new RouteValueDictionary 
                { 
                    {"controller", "Install"},
                    {"action", "Index"} 
                },
                null,
                new RouteValueDictionary { 
                    {"area", AreaName},
                },
                new MvcRouteHandler());

            context.Routes.Add(adminRoute);

            if (!AppConfigConfiguration.Instance.Setup.IsCompleted)
            {
                var defaultRoute = new Route("{controller}/{action}/{id}",
                      new RouteValueDictionary { 
                          { "id", UrlParameter.Optional },
                          {"controller", "Install"},
                          {"action", "Index"} 
                      },
                        new RouteValueDictionary
                    {
                        {"controller", new ControllerConstraint()},
                    },
                     new RouteValueDictionary { 
                        {"area", AreaName}
                     },
                      new MvcRouteHandler());

                context.Routes.Add(defaultRoute);
                context.Routes.Redirect(r => r.MapRoute("redirect", "{*returnUrl}")).To(adminRoute);
            }
        }
    }

    /// <summary>
    /// Allows only valid controllers
    /// </summary>
    public class ControllerConstraint : IRouteConstraint
    {
        public bool Match(HttpContextBase httpContext, Route route, string parameterName, RouteValueDictionary values,
            RouteDirection routeDirection)
        {
            var controllerName = values[parameterName].ToString();
            var factory = ControllerBuilder.Current.GetControllerFactory();
            IController controller;
            try
            {
                controller = factory.CreateController(httpContext.Request.RequestContext, controllerName);
            }
            catch
            {
                return false;
            }
            return controller != null;
        }
    }
}
