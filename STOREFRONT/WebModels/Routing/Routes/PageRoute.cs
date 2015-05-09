#region
using System.Web;
using System.Web.Routing;
using VirtoCommerce.ApiClient.DataContracts;

#endregion

namespace VirtoCommerce.Web.Models.Routing.Routes
{
    public class PageRoute : StoreRoute
    {
        #region Constructors and Destructors
        public PageRoute(string url, IRouteHandler routeHandler)
            : base(url, routeHandler)
        {
        }

        public PageRoute(string url, RouteValueDictionary defaults, IRouteHandler routeHandler)
            : base(url, defaults, routeHandler)
        {
        }

        public PageRoute(
            string url,
            RouteValueDictionary defaults,
            RouteValueDictionary constraints,
            IRouteHandler routeHandler)
            : base(url, defaults, constraints, routeHandler)
        {
        }

        public PageRoute(
            string url,
            RouteValueDictionary defaults,
            RouteValueDictionary constraints,
            RouteValueDictionary dataTokens,
            IRouteHandler routeHandler)
            : base(url, defaults, constraints, dataTokens, routeHandler)
        {
        }
        #endregion

        #region Public Methods and Operators
        public override string GetMainRouteKey()
        {
            return Constants.Page;
        }

        public override RouteData GetRouteData(HttpContextBase httpContext)
        {
            var routeData = base.GetRouteData(httpContext);

            if (routeData != null)
            {
                if (!routeData.Values.ContainsKey(Constants.Page) || routeData.Values[Constants.Page] == null)
                {
                    routeData = null;
                }
            }

            return routeData;
        }

        public override VirtualPathData GetVirtualPath(RequestContext requestContext, RouteValueDictionary values)
        {
            //this.EncodeVirtualPath(values, SeoUrlKeywordTypes.Category);
            return base.GetVirtualPath(requestContext, values);
        }
        #endregion
    }
}