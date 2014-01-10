using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Routing;
using VirtoCommerce.Web.Client.Extensions.RouteHandlers;

namespace VirtoCommerce.Web.Client.Extensions.Routing
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
            var retVal = base.GetRouteData(httpContext);

            if (retVal != null)
            {
                if (!retVal.Values.ContainsKey("item") || retVal.Values["item"] == null)
                {
                    retVal = null;
                }
                else
                {
                    var item = retVal.Values["item"].ToString();
                    retVal.Values["item"] = SeoDecode(item, ItemRouteHandler.ItemMappings);
                }
            }
            return retVal;
        }


        public override VirtualPathData GetVirtualPath(RequestContext requestContext, RouteValueDictionary values)
        {
            if (values.ContainsKey("item") && values["item"] != null)
            {
                var item = values["item"].ToString();
                values["item"] = SeoEncode(item, ItemRouteHandler.ItemMappings);
            }
            return base.GetVirtualPath(requestContext, values);
        }
    }
}
