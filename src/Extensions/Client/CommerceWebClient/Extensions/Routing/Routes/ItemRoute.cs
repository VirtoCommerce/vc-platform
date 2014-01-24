using System;
using System.Linq;
using System.Web;
using System.Web.Routing;
using VirtoCommerce.Foundation.AppConfig.Model;
using VirtoCommerce.Foundation.Catalogs.Services;
using VirtoCommerce.Web.Client.Helpers;

namespace VirtoCommerce.Web.Client.Extensions.Routing.Routes
{
    public class ItemRoute : CategoryRoute
    {
        public ItemRoute(string url, IRouteHandler routeHandler)
            : base(url, routeHandler)
        {
        }

        public ItemRoute(string url, RouteValueDictionary defaults, IRouteHandler routeHandler)
            : base(url, defaults, routeHandler)
        {
        }

        public ItemRoute(string url, RouteValueDictionary defaults, RouteValueDictionary constraints, IRouteHandler routeHandler)
            : base(url, defaults, constraints, routeHandler)
        {
        }

        public ItemRoute(string url, RouteValueDictionary defaults, RouteValueDictionary constraints, RouteValueDictionary dataTokens, IRouteHandler routeHandler)
            : base(url, defaults, constraints, dataTokens, routeHandler)
        {
        }

        public override RouteData GetRouteData(HttpContextBase httpContext)
        {
            var routeData = base.GetRouteData(httpContext);

            if (routeData != null)
            {
                //If there is no item in route, then route is not valid
                if (!routeData.Values.ContainsKey(Constants.Item) || routeData.Values[Constants.Item] == null)
                {
                    routeData = null;
                }
                else
                {
                    //Decode the value
                    var item = routeData.Values[Constants.Item].ToString();
                    routeData.Values[Constants.Item] = SettingsHelper.SeoDecode(item, SeoUrlKeywordTypes.Item, routeData.Values[Constants.Language].ToString());
                }
            }
            return routeData;
        }


        public override VirtualPathData GetVirtualPath(RequestContext requestContext, RouteValueDictionary values)
        {
            EncodeVirtualPath(requestContext, values, SeoUrlKeywordTypes.Item);
            return base.GetVirtualPath(requestContext, values);
        }

        protected override string ModifyCategoryPath(RouteValueDictionary values)
        {
            var itemEncoded = values[Constants.Item] as string;

            if (string.IsNullOrEmpty(itemEncoded))
                return null;

            var itemCode = SettingsHelper.SeoDecode(itemEncoded, SeoUrlKeywordTypes.Item);

            var item = CartHelper.CatalogClient.GetItem(itemCode, bycode: true);

            if (item == null)
            {
                return null;
            }

            //TODO: should find closest match to current path
            var outline = item.GetItemCategoryRouteValue();

            return outline;
        }
    }
}
