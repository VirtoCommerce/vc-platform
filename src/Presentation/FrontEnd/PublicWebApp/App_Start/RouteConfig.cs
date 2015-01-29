using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Routing;
using VirtoCommerce.ApiWebClient.Extensions;
using VirtoCommerce.ApiWebClient.Extensions.Routing;
using VirtoCommerce.ApiWebClient.Extensions.Routing.Constraints;
using VirtoCommerce.ApiWebClient.Extensions.Routing.Routes;
using VirtoCommerce.ApiWebClient.Helpers;

namespace VirtoCommerce.Web
{
    using VirtoCommerce.ApiClient;
    using VirtoCommerce.ApiClient.Extensions;
    using VirtoCommerce.Web.Core.DataContracts;

    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("virto/services/{MyJob}.svc/{*pathInfo}");
            routes.IgnoreRoute("virto/dataservices/{MyJob}.svc/{*pathInfo}");
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.IgnoreRoute(".html");

            routes.MapMvcAttributeRoutes();

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
                        {"controller", "Search"},
                        {"action", "Category"},
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
                        var client = ClientContext.Clients.CreateBrowseClient(ClientContext.Session.StoreId, ClientContext.Session.Language);
                        var category = Task.Run(() => client.GetCategoryByCodeAsync(x.RouteData.Values[Constants.Category].ToString())).Result;
                        if (category != null)
                        {
                            return new RouteValueDictionary { { Constants.Category, category.Id } };
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
                        var client = ClientContext.Clients.CreateBrowseClient(ClientContext.Session.StoreId, ClientContext.Session.Language);
                        var item = Task.Run(()=>client.GetProductByCodeAsync(x.RouteData.Values[Constants.Item].ToString(), ItemResponseGroups.ItemMedium)).Result;
                        if (item != null)
                        {
                            //TODO return category for item
                            return new RouteValueDictionary { { Constants.Category, item.Outline } };
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
