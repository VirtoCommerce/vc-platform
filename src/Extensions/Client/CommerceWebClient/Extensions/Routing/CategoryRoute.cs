using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Routing;
using VirtoCommerce.Web.Client.Extensions.RouteHandlers;

namespace VirtoCommerce.Web.Client.Extensions.Routing
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
            var retVal =  base.GetRouteData(httpContext);

            if (retVal != null)
            {
                if (!retVal.Values.ContainsKey("category") || retVal.Values["category"] == null)
                {
                    retVal = null;
                }
                else
                {
                    var item = retVal.Values["category"].ToString();
                    retVal.Values["category"] = SeoDecode(item, CategoryRouteHandler.CatMappings);
                }
            }
           
            return retVal;
        }

        public override VirtualPathData GetVirtualPath(RequestContext requestContext, RouteValueDictionary values)
        {
            if (values.ContainsKey("category") && values["category"] != null)
            {
                var category = values["category"].ToString();
                values["category"] = SeoEncode(category, CategoryRouteHandler.CatMappings);
            }
            return base.GetVirtualPath(requestContext, values);
        }
    }
}
