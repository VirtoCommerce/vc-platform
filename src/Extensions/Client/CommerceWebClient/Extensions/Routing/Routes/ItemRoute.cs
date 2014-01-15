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
                    var item = routeData.Values[Constants.Item].ToString();
                    routeData.Values[Constants.Item] = SettingsHelper.SeoDecode(item, SeoUrlKeywordTypes.Item, routeData.Values[Constants.Language].ToString());
                }
            }
            return routeData;
        }


        public override VirtualPathData GetVirtualPath(RequestContext requestContext, RouteValueDictionary values)
        {
            ModifyVirtualPath(requestContext, values, SeoUrlKeywordTypes.Item);
            return base.GetVirtualPath(requestContext, values);
        }
    }
}
