using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Routing;
using VirtoCommerce.Client;
using VirtoCommerce.Web.Client.Extensions.RouteHandlers;
using VirtoCommerce.Web.Client.Helpers;

namespace VirtoCommerce.Web.Client.Extensions.Routing
{
    public class StoreRoute : Route
    {
        public StoreRoute(string url, IRouteHandler routeHandler)
            : base(url, routeHandler)
        {
        }

        public StoreRoute(string url, RouteValueDictionary defaults, IRouteHandler routeHandler)
            : base(url, defaults, routeHandler)
        {
        }

        public StoreRoute(string url, RouteValueDictionary defaults, RouteValueDictionary constraints, IRouteHandler routeHandler)
            : base(url, defaults, constraints, routeHandler)
        {
        }

        public StoreRoute(string url, RouteValueDictionary defaults, RouteValueDictionary constraints, RouteValueDictionary dataTokens, IRouteHandler routeHandler)
            : base(url, defaults, constraints, dataTokens, routeHandler)
        {
        }

        public override VirtualPathData GetVirtualPath(RequestContext requestContext, RouteValueDictionary values)
        {
            var retVal =  base.GetVirtualPath(requestContext, values);
            return retVal;
        }

        public override RouteData GetRouteData(HttpContextBase httpContext)
        {
            var routeData = base.GetRouteData(httpContext);

            if (routeData == null)
            {
                var requestPath = httpContext.Request.AppRelativeCurrentExecutionFilePath.Substring(2) +
                                  httpContext.Request.PathInfo;
                var pathSegments = requestPath.Split(new[] {'/'}, StringSplitOptions.RemoveEmptyEntries);
                RouteValueDictionary values = null;

                if (pathSegments.Length < 2)
                {
                    values = new RouteValueDictionary();


                    //If both store and languange are missing
                    if (pathSegments.Length == 0)
                    {
                        values.Add("lang",
                            StoreHelper.CustomerSession.Language ??
                            StoreHelper.StoreClient.GetCurrentStore().DefaultLanguage);
                        values.Add("store", StoreHelper.CustomerSession.StoreId);
                    }
                    //if store is missing
                    else if (pathSegments.Length == 1)
                    {
                        values.Add("store", StoreHelper.CustomerSession.StoreId);
                    }
                }

                if (values == null)
                {
                    // If we got back a null value set, that means the URL did not match
                    return null;
                }

                routeData = new RouteData(this, RouteHandler);

                // Validate the values
                if (!ProcessConstraints(httpContext, values, RouteDirection.IncomingRequest))
                {
                    return null;
                }

                // Copy the matched values
                foreach (var value in values)
                {
                    routeData.Values.Add(value.Key, value.Value);
                }

                // Copy the DataTokens from the Route to the RouteData 
                if (DataTokens != null)
                {
                    foreach (var prop in DataTokens)
                    {
                        routeData.DataTokens[prop.Key] = prop.Value;
                    }
                }

                // Copy the remaining default values
                foreach (var value in Defaults)
                {
                    if (!routeData.Values.ContainsKey(value.Key))
                    {
                        routeData.Values.Add(value.Key, value.Value);
                    }
                }
            }
            else
            {
                var storeId = routeData.Values["store"].ToString();

                //If such store does not exist this route is not valid
                if (StoreHelper.StoreClient.GetStoreById(storeId) == null)
                {
                    return null;
                }
            }

            return routeData; 
        }

        private bool ProcessConstraints(HttpContextBase httpContext, RouteValueDictionary values, RouteDirection routeDirection)
        {
            if (Constraints != null)
            {
                foreach (var constraintsItem in Constraints)
                {
                    if (!ProcessConstraint(httpContext, constraintsItem.Value, constraintsItem.Key, values, routeDirection))
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        protected string SeoEncode(string value, Dictionary<string,string> mappings)
        {
            var mapping = mappings.Where(x => x.Value.Equals(value, StringComparison.CurrentCultureIgnoreCase)).ToArray();
            return mapping.Any() ? mapping.First().Key : value;
        }

        protected string SeoDecode(string key, Dictionary<string, string> mappings)
        {
            var mapping = mappings.Where(x => x.Key.Equals(key, StringComparison.CurrentCultureIgnoreCase)).ToArray();
            return mapping.Any() ? mapping.First().Value : key;
        }
    }
}
