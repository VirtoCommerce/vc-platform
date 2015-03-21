using System.Linq;
using System.Web;
using System.Web.Routing;
using VirtoCommerce.Foundation.AppConfig.Model;
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
                    DecodeRouteData(routeData.Values, SeoUrlKeywordTypes.Item);
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

            var itemId = SettingsHelper.SeoDecode(itemEncoded, SeoUrlKeywordTypes.Item, 
                values.ContainsKey(Constants.Language) ? values[Constants.Language].ToString() : null);

            // try getting outline from context
            var outline = HttpContext.Current.Items["browsingoutline_" + itemId.ToLower()] as string;

            if (string.IsNullOrEmpty(outline))
            {
                var item = CartHelper.CatalogClient.GetItem(itemId, StoreHelper.CustomerSession.CatalogId);

                if (item == null)
                {
                    return null;
                }

                //TODO: should find closest match to current path
                outline = item.GetItemCategoryRouteValue();
            }
            else
            {
                outline = outline.Split(';').First();
            }

            return outline;
        }

        public override string GetMainRouteKey()
        {
            return Constants.Item;
        }
    }
}
