using System.Collections.Generic;
using System.Linq;
using System.Web.Routing;
using VirtoCommerce.Web.Client.Extensions.RouteHandlers;

namespace VirtoCommerce.Web.Client.Extensions.Routing
{
    public class CategoryRoute : Route
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

        public override VirtualPathData GetVirtualPath(RequestContext requestContext, RouteValueDictionary values)
        {
            if (requestContext.RouteData.Values["category"] == null)
                return null;

            if (values.ContainsKey("category") && values["category"] != null)
            {
                var category = values["category"].ToString();

                //Check if category can be reversed
                if (CategoryRouteHandler.Mappings.ContainsValue(category))
                {
                    values["category"] = CategoryRouteHandler.Mappings.FirstOrDefault(x => x.Value == category).Key;
                }
            }
            return base.GetVirtualPath(requestContext, values);
        }
    }
}
