#region
using System.Web.Mvc;
using System.Web.Routing;
using VirtoCommerce.Web.Models.Routing.Constraints;
using VirtoCommerce.Web.Models.Routing.Routes;

#endregion

namespace VirtoCommerce.Web.Models.Routing
{
    public static class SeoRoutesExtensions
    {
        #region Public Methods and Operators
        public static void MapSeoRoutes(this RouteCollection routes)
        {
            var itemRoute =
                new NormalizeRoute(
                    new ItemRoute(
                        Constants.ItemRoute,
                        new RouteValueDictionary
                        {
                            { "controller", "Product" },
                            { "action", "ProductByKeywordAsync" },
                            { Constants.Language, UrlParameter.Optional }
                        },
                        new RouteValueDictionary
                        {
                            { Constants.Language, new LanguageRouteConstraint() },
                            { Constants.Store, new StoreRouteConstraint() },
                            { Constants.Category, new CategoryRouteConstraint() },
                            { Constants.Item, new ItemRouteConstraint() }
                        },
                        new RouteValueDictionary { { "namespaces", new[] { "VirtoCommerce.Web.Controllers" } } },
                        new MvcRouteHandler()));

            var categoryRoute =
                new NormalizeRoute(
                    new CategoryRoute(
                        Constants.CategoryRoute,
                        new RouteValueDictionary
                        {
                            { "controller", "Collections" },
                            { "action", "GetCollectionByKeywordAsync" },
                            { Constants.Language, UrlParameter.Optional }
                        },
                        new RouteValueDictionary
                        {
                            { Constants.Language, new LanguageRouteConstraint() },
                            { Constants.Store, new StoreRouteConstraint() },
                            { Constants.Category, new CategoryRouteConstraint() }
                        },
                        new RouteValueDictionary { { "namespaces", new[] { "VirtoCommerce.Web.Controllers" } } },
                        new MvcRouteHandler()));

            var storeRoute =
                new NormalizeRoute(
                    new StoreRoute(
                        Constants.StoreRoute,
                        new RouteValueDictionary { { "controller", "Home" }, { "action", "Index" } },
                        new RouteValueDictionary
                        {
                            { Constants.Language, new LanguageRouteConstraint() },
                            { Constants.Store, new StoreRouteConstraint() }
                        },
                        new RouteValueDictionary { { "namespaces", new[] { "VirtoCommerce.Web.Controllers" } } },
                        new MvcRouteHandler()));

            var defaultRoute = new NormalizeRoute(new Route(string.Format("{{{0}}}/{{controller}}/{{action}}/{{id}}", Constants.Language),
                    new RouteValueDictionary { { "id", UrlParameter.Optional }, { "action", "Index" } },
                    new RouteValueDictionary { { Constants.Language, new LanguageRouteConstraint() } },
                    new RouteValueDictionary { { "namespaces", new[] { "VirtoCommerce.Web.Controllers" } } },
                    new MvcRouteHandler()));

            routes.Add("Item", itemRoute);
            routes.Add("Category", categoryRoute);
            routes.Add("Store", storeRoute);

            //Other actions
            routes.Add("RelativeDefault", defaultRoute);
        }
        #endregion
    }
}