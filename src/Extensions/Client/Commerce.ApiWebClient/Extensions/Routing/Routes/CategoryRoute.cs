using System.Web;
using System.Web.Routing;
using VirtoCommerce.Web.Core.DataContracts;

namespace VirtoCommerce.ApiWebClient.Extensions.Routing.Routes
{
    public class CategoryRoute : StoreRoute
    {
        public CategoryRoute(string url, IRouteHandler routeHandler)
            : base(url, routeHandler)
        {
        }

        public CategoryRoute(string url, RouteValueDictionary defaults, IRouteHandler routeHandler)
            : base(url, defaults, routeHandler)
        {
        }

        public CategoryRoute(string url, RouteValueDictionary defaults, RouteValueDictionary constraints, IRouteHandler routeHandler)
            : base(url, defaults, constraints, routeHandler)
        {
        }

        public CategoryRoute(string url, RouteValueDictionary defaults, RouteValueDictionary constraints, RouteValueDictionary dataTokens, IRouteHandler routeHandler)
            : base(url, defaults, constraints, dataTokens, routeHandler)
        {
        }

        public override RouteData GetRouteData(HttpContextBase httpContext)
        {
            var routeData = base.GetRouteData(httpContext);

            if (routeData != null)
            {
                if (!routeData.Values.ContainsKey(Constants.Category) || routeData.Values[Constants.Category] == null)
                {
                    routeData = null;
                }
            }

            return routeData;
        }

        public override VirtualPathData GetVirtualPath(RequestContext requestContext, RouteValueDictionary values)
        {
            EncodeVirtualPath(values, SeoUrlKeywordTypes.Category);
            return base.GetVirtualPath(requestContext, values);
        }

        public override string GetMainRouteKey()
        {
            return Constants.Category;
        }

    }
}
