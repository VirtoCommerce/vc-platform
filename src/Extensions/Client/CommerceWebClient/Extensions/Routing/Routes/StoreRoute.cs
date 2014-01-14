using System;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Routing;
using VirtoCommerce.Foundation.AppConfig.Model;
using VirtoCommerce.Web.Client.Helpers;

namespace VirtoCommerce.Web.Client.Extensions.Routing.Routes
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
            if (!values.ContainsKey("lang"))
            {
                values.Add("lang",
                            StoreHelper.CustomerSession.Language ??
                            StoreHelper.StoreClient.GetCurrentStore().DefaultLanguage);
            }
            if (!values.ContainsKey("store"))
            {
                values.Add("store", StoreHelper.CustomerSession.StoreId);
            }
            ModifyVirtualPath(requestContext, values, SeoUrlKeywordTypes.Store);
            return base.GetVirtualPath(requestContext, values);
        }

        public override RouteData GetRouteData(HttpContextBase httpContext)
        {
            var routeData = base.GetRouteData(httpContext);

            if (routeData == null)
            {
                var requestPath = httpContext.Request.AppRelativeCurrentExecutionFilePath.Substring(2) +
                                  httpContext.Request.PathInfo;
                var pathSegments = requestPath.Split(new[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
                RouteValueDictionary values = null;

                //If route does not have any language or store add defaults to route data
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
                        values.Add("lang", pathSegments[0]);
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
                foreach (var value in Defaults.Where(value => !routeData.Values.ContainsKey(value.Key)))
                {
                    routeData.Values.Add(value.Key, value.Value);
                }
            }

            if (!routeData.Values.ContainsKey("store") || !routeData.Values.ContainsKey("lang"))
            {
                return null;
            }
            //Decode route value
            var store = routeData.Values["store"].ToString();
            routeData.Values["store"] = SettingsHelper.SeoDecode(store, SeoUrlKeywordTypes.Store, routeData.Values["lang"].ToString());

            var storeId = routeData.Values["store"].ToString();
            var dbStore = StoreHelper.StoreClient.GetStoreById(storeId);

            //If such store does not exist this route is not valid
            if (dbStore == null)
            {
                return null;
            }

            try
            {
                var culture = CultureInfo.CreateSpecificCulture(routeData.Values["lang"].ToString());
                if(!dbStore.Languages.Any(l=>l.LanguageCode.Equals(culture.Name)))
                {
                    //Store does not support this language
                    return null;
                }
            }
            catch
            {
                //Language is not valid
                return null;
            }

            return routeData;
        }

        protected void ModifyVirtualPath(RequestContext requestContext, RouteValueDictionary values, SeoUrlKeywordTypes type)
        {
            string routeValueKey = type.ToString().ToLower();

            if (values.ContainsKey(routeValueKey) && values[routeValueKey] != null)
            {
                values[routeValueKey] = SettingsHelper.SeoEncode(values[routeValueKey].ToString(), type);
            }
            else if (requestContext.RouteData.Values.ContainsKey(routeValueKey))
            {
                var valueEncoded = SettingsHelper.SeoEncode(requestContext.RouteData.Values[routeValueKey].ToString(), type);
                if (!values.ContainsKey(routeValueKey))
                {
                    values.Add(routeValueKey, valueEncoded);
                }
                else
                {
                    values[routeValueKey] = valueEncoded;
                }
            }
        }

        protected bool ProcessConstraints(HttpContextBase httpContext, RouteValueDictionary values, RouteDirection routeDirection)
        {
            return Constraints == null || Constraints.All(constraintsItem => ProcessConstraint(httpContext, constraintsItem.Value, constraintsItem.Key, values, routeDirection));
        }
    }
}
