using System.Web.Mvc;
using System.Web.Routing;
using VirtoCommerce.Web.Client.Extensions.Routing;
using VirtoCommerce.Web.Client.Extensions.Routing.Constraints;
using VirtoCommerce.Web.Client.Extensions.Routing.Routes;

namespace Initial.Web
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("virto/services/{MyJob}.svc/{*pathInfo}");
            routes.IgnoreRoute("virto/dataservices/{MyJob}.svc/{*pathInfo}");
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.IgnoreRoute(".html");

            routes.MapRoute(
                "FailWhale",
                "FailWhale/{action}/{id}", new { controller = "Error", action = "FailWhale", id = UrlParameter.Optional });

            routes.MapRoute(
              "Assets",
              "asset/{*path}",
              new { controller = "Asset", action = "Index", path = UrlParameter.Optional });

            var itemRoute = new NormalizeRoute(
                new ItemRoute(Constants.ItemRoute,
                    new RouteValueDictionary
                    {
                        {"controller", "Catalog"},
                        {"action", "DisplayItem"},
                        {Constants.Language, UrlParameter.Optional}
                    },
                    new RouteValueDictionary
                    {
                        {Constants.Language, new LanguageRouteConstraint()},
                        {Constants.Store, new StoreRouteConstraint()},
                        {Constants.Category, new CategoryRouteConstraint()},
                        {Constants.Item, new ItemRouteConstraint()}
                    },
                    new RouteValueDictionary { { "namespaces", new[] { "Initial.Web.Controllers" } } },
                new MvcRouteHandler()));

            var categoryRoute = new NormalizeRoute(
                new CategoryRoute(Constants.CategoryRoute,
                    new RouteValueDictionary
                    {
                        {"controller", "Catalog"},
                        {"action", "Display"},
                        {Constants.Language, UrlParameter.Optional}
                    },
                 new RouteValueDictionary
                    {
                        {Constants.Language, new LanguageRouteConstraint()},
                        {Constants.Store, new StoreRouteConstraint()},
                        {Constants.Category, new CategoryRouteConstraint()}
                    },
                new RouteValueDictionary { { "namespaces", new[] { "Initial.Web.Controllers" } } },
                new MvcRouteHandler()));

            var storeRoute = new NormalizeRoute(
                new StoreRoute(Constants.StoreRoute,
                 new RouteValueDictionary
                    {
                        {"controller", "Home"},
                        {"action", "Index"}
                    },
                new RouteValueDictionary
                    {
                        {Constants.Language, new LanguageRouteConstraint()},
                        {Constants.Store, new StoreRouteConstraint()}
                    },
                new RouteValueDictionary { { "namespaces", new[] { "Initial.Web.Controllers" } } },
                new MvcRouteHandler()));

            routes.Add("Item", itemRoute);
            routes.Add("Category", categoryRoute);
            routes.Add("Store", storeRoute);

            //Legacy redirects
            //routes.Redirect(r => r.MapRoute("old_Category", string.Format("c/{{{0}}}", Constants.Category))).To(categoryRoute);
            //routes.Redirect(r => r.MapRoute("old_Item", string.Format("p/{{{0}}}", Constants.Item))).To(itemRoute,
            //    x =>
            //    {
            //        //Resolve item category dynamically
            //        if (x.RouteData.Values.ContainsKey(Constants.Item))
            //        {
            //            var item = CatalogHelper.CatalogClient.GetItem(x.RouteData.Values[Constants.Item].ToString(), bycode: true);
            //            if (item != null)
            //            {
            //                return new RouteValueDictionary { { Constants.Category, item.GetItemCategoryRouteValue() } };
            //            }
            //        };
            //        return null;
            //    });

            var defaultRoute = new NormalizeRoute(new Route(string.Format("{{{0}}}/{{controller}}/{{action}}/{{id}}", Constants.Language),
                new RouteValueDictionary { { "id", UrlParameter.Optional }, { "action", "Index" } },
                new RouteValueDictionary { { Constants.Language, new LanguageRouteConstraint() } },
                new RouteValueDictionary { { "namespaces", new[] { "Initial.Web.Controllers" } } },
                new MvcRouteHandler()));

            //Other actions
            routes.Add("Default", defaultRoute);

            //Needed for some post requests
            routes.MapRoute(
                "Default_Fallback", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new
                {
                    action = "Index",
                    id = UrlParameter.Optional
                }, // Parameter defaults
                new[] { "Initial.Web.Controllers" });
        }
    }
}