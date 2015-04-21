#region

using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Routing;
using VirtoCommerce.ApiClient;
using VirtoCommerce.ApiClient.Extensions;
using VirtoCommerce.Web.Models.Convertors;
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
                            //{ Constants.Language, UrlParameter.Optional }

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
                        Constants.CategoryRouteWithTags,
                        new RouteValueDictionary
                        {
                            { "controller", "Collections" },
                            { "action", "GetCollectionByKeywordAsync" },
                            //{ Constants.Language, UrlParameter.Optional },
                            { Constants.Tags, UrlParameter.Optional },
                        },
                        new RouteValueDictionary
                        {
                            { Constants.Language, new LanguageRouteConstraint() },
                            { Constants.Store, new StoreRouteConstraint() },
                            { Constants.Category, new CategoryRouteConstraint() }//,
                            //{ Constants.Tags, @"^$|[0-9][0-9]"}
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

            //Legacy redirects
            routes.Redirect(r => r.MapRoute("old_Collection", string.Format("collections/{{{0}}}", Constants.Category))).To(categoryRoute,
                x =>
                {
                    //Expect to receive category code
                    if (x.RouteData.Values.ContainsKey(Constants.Category))
                    {
                        var client = ClientContext.Clients.CreateBrowseClient();
                        var category = Task.Run(() => client.GetCategoryByCodeAsync("samplestore", "en-us", x.RouteData.Values[Constants.Category].ToString())).Result;
                        if (category != null)
                        {
                            var model = category.AsWebModel();
                            return new RouteValueDictionary { { Constants.Category, model.Outline } };
                        }
                    }
                    return null;
                });

            /*
            routes.Redirect(r => r.MapRoute("old_Item", string.Format("p/{{{0}}}", Constants.Item))).To(itemRoute,
                x =>
                {
                    //Resolve item category dynamically
                    //Expect to receive item code
                    if (x.RouteData.Values.ContainsKey(Constants.Item))
                    {
                        var client = ClientContext.Clients.CreateBrowseClient(ClientContext.Session.StoreId, ClientContext.Session.Language);
                        var item = Task.Run(() => client.GetProductByCodeAsync(x.RouteData.Values[Constants.Item].ToString(), ItemResponseGroups.ItemMedium)).Result;
                        if (item != null)
                        {
                            //TODO return category for item
                            return new RouteValueDictionary { { Constants.Category, item.Outline } };
                        }
                    }
                    return null;
                });
             * */

            var defaultRoute = new NormalizeRoute(
                new Route(string.Format("{0}/{{controller}}/{{action}}/{{id}}", Constants.StoreRoute),
                    new RouteValueDictionary { { "id", UrlParameter.Optional }, { "action", "Index" } },
                    new RouteValueDictionary
                    {
                        { Constants.Language, new LanguageRouteConstraint() },
                        { Constants.Store, new StoreRouteConstraint() }
                    },
                    new RouteValueDictionary { { "namespaces", new[] { "VirtoCommerce.Web.Controllers" } } },
                    new MvcRouteHandler()));


            /*
            var defaultRoute = new NormalizeRoute(
    new StoreRoute(
        string.Format("{{{0}}}/{{controller}}/{{action}}/{{id}}", Constants.StoreRoute),
        new RouteValueDictionary { { "id", UrlParameter.Optional }, { "action", "Index" } },
        new RouteValueDictionary
                    {
                        { Constants.Language, new LanguageRouteConstraint() }
                    },
        new RouteValueDictionary { { "namespaces", new[] { "VirtoCommerce.Web.Controllers" } } },
        new MvcRouteHandler()));
            */
            routes.Add("Item", itemRoute);
            routes.Add("Category", categoryRoute);
            routes.Add("Store", storeRoute);

            //Other actions
            routes.Add("RelativeDefault", defaultRoute);
        }
        #endregion
    }
}