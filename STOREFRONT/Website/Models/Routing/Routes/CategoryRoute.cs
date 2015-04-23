#region
using System.Web;
using System.Web.Routing;
using VirtoCommerce.ApiClient.DataContracts;

#endregion

namespace VirtoCommerce.Web.Models.Routing.Routes
{
    public class CategoryRoute : StoreRoute
    {
        #region Constructors and Destructors
        public CategoryRoute(string url, IRouteHandler routeHandler)
            : base(url, routeHandler)
        {
        }

        public CategoryRoute(string url, RouteValueDictionary defaults, IRouteHandler routeHandler)
            : base(url, defaults, routeHandler)
        {
        }

        public CategoryRoute(
            string url,
            RouteValueDictionary defaults,
            RouteValueDictionary constraints,
            IRouteHandler routeHandler)
            : base(url, defaults, constraints, routeHandler)
        {
        }

        public CategoryRoute(
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
            return Constants.Category;
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
            /*
            if (!values.ContainsKey(Constants.Tags))
            {
                values.Add(Constants.Tags, null);
            }
             * */

            this.EncodeVirtualPath(values, SeoUrlKeywordTypes.Category);
            return base.GetVirtualPath(requestContext, values);
        }
        #endregion
    }
}