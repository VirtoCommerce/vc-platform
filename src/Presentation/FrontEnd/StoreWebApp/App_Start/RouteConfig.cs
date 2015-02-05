using System.Web.Mvc;
using System.Web.Routing;
using VirtoCommerce.Web.Client.Extensions;
using VirtoCommerce.Web.Client.Extensions.Routing;
using VirtoCommerce.Web.Client.Extensions.Routing.Constraints;
using VirtoCommerce.Web.Client.Extensions.Routing.Routes;
using VirtoCommerce.Web.Client.Helpers;
using VirtoCommerce.Web.Virto.Helpers;

namespace VirtoCommerce.Web
{
    /// <summary>
    /// Class RouteConfig.
    /// </summary>
    public class RouteConfig
    {
        /// <summary>
        /// Registers the routes.
        /// </summary>
        /// <param name="routes">The routes.</param>
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("virto/services/{MyJob}.svc/{*pathInfo}");
            routes.IgnoreRoute("virto/dataservices/{MyJob}.svc/{*pathInfo}");
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.IgnoreRoute(".html");

            //Ignore all calls to bundles
            routes.IgnoreRoute("bundles/{*pathInfo}");
            //Ignore all calls to areas (Areas supposed to serve own routes)
            routes.IgnoreRoute("areas/{*pathInfo}");

            //This makes sure index.html (or any other deafult document is opened for empty url
            routes.IgnoreRoute("");

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
                    new RouteValueDictionary { { "namespaces", new[] { "VirtoCommerce.Web.Controllers" } } },
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
                new RouteValueDictionary { { "namespaces", new[] { "VirtoCommerce.Web.Controllers" } } },
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
                new RouteValueDictionary { { "namespaces", new[] { "VirtoCommerce.Web.Controllers" } } },
                new MvcRouteHandler()));

            routes.Add("Item", itemRoute);
            routes.Add("Category", categoryRoute);
            routes.Add("Store", storeRoute);

            //Legacy redirects
            routes.Redirect(r => r.MapRoute("old_Category", string.Format("c/{{{0}}}", Constants.Category))).To(categoryRoute,
                x =>
                {
                    //Expect to receive category code
                    if (x.RouteData.Values.ContainsKey(Constants.Category))
                    {
                        var category = CatalogHelper.CatalogClient.GetCategory(x.RouteData.Values[Constants.Category].ToString());
                        if (category != null)
                        {
                            return new RouteValueDictionary { { Constants.Category, category.CategoryId } };
                        }
                    }
                    return null;
                });
            routes.Redirect(r => r.MapRoute("old_Item", string.Format("p/{{{0}}}", Constants.Item))).To(itemRoute,
                x =>
                {
                    //Resolve item category dynamically
                    //Expect to receive item code
                    if (x.RouteData.Values.ContainsKey(Constants.Item))
                    {
                        var item = CatalogHelper.CatalogClient.GetItemByCode(x.RouteData.Values[Constants.Item].ToString(), StoreHelper.CustomerSession.CatalogId);
                        if (item != null)
                        {
                            return new RouteValueDictionary { { Constants.Category, item.GetItemCategoryRouteValue() } };
                        }
                    }
                    return null;
                });

            var defaultRoute = new NormalizeRoute(new Route(string.Format("{{{0}}}/{{controller}}/{{action}}/{{id}}", Constants.Language),
                new RouteValueDictionary { { "id", UrlParameter.Optional }, { "action", "Index" } },
                new RouteValueDictionary { { Constants.Language, new LanguageRouteConstraint() } },
                new RouteValueDictionary { { "namespaces", new[] { "VirtoCommerce.Web.Controllers" } } },
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
                new[] { "VirtoCommerce.Web.Controllers" });
        }
    }
}