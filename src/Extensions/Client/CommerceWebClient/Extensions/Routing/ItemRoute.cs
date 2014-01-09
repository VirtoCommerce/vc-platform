using System.Collections.Generic;
using System.Linq;
using System.Web.Routing;
using VirtoCommerce.Web.Client.Extensions.RouteHandlers;

namespace VirtoCommerce.Web.Client.Extensions.Routing
{
    public class ItemRoute : Route
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

        public override VirtualPathData GetVirtualPath(RequestContext requestContext, RouteValueDictionary values)
        {
            if (requestContext.RouteData.Values["item"] == null)
                return null;

            return base.GetVirtualPath(requestContext, values);
        }
    }
}
