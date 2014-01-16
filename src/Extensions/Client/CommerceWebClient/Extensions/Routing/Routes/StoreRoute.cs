using System;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Routing;
using VirtoCommerce.Foundation.AppConfig.Model;
using VirtoCommerce.Web.Client.Extensions.Routing.Constraints;
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
            if (!values.ContainsKey(Constants.Language))
            {
                values.Add(Constants.Language,
                            StoreHelper.CustomerSession.Language ??
                            StoreHelper.StoreClient.GetCurrentStore().DefaultLanguage);
            }
            if (!values.ContainsKey(Constants.Store))
            {
                values.Add(Constants.Store, StoreHelper.CustomerSession.StoreId);
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

                var values = new RouteValueDictionary();

                //If route does not have any language or store add defaults to route data

                //If both store and languange are missing
                if (pathSegments.Length == 0)
                {
                    values.Add(Constants.Language,
                        StoreHelper.CustomerSession.Language ??
                        StoreHelper.StoreClient.GetCurrentStore().DefaultLanguage);
                    values.Add(Constants.Store, StoreHelper.CustomerSession.StoreId);
                }
                else
                {
                    //Check if some values match constraint for language or store
                    var languageConstraint = new LanguageRouteConstraint();
                    var storeConstraint = new StoreRouteConstraint();

                    var languageFound = false;
                    if (languageConstraint.Match(httpContext, this, Constants.Language,
                        new RouteValueDictionary { { Constants.Language, pathSegments[0] } }.Merge(values), RouteDirection.IncomingRequest))
                    {
                        languageFound = true;
                        values.Add(Constants.Language, pathSegments[0]);
                    }
                    else
                    {
                        values.Add(Constants.Language,
                        StoreHelper.CustomerSession.Language ??
                        StoreHelper.StoreClient.GetCurrentStore().DefaultLanguage);
                    }

                    var storeFound = false;
                    var storeCandidate = !languageFound
                        ? pathSegments[0]
                        : pathSegments.Length > 1 ? pathSegments[1] : null;

                    if (!string.IsNullOrEmpty(storeCandidate) && storeConstraint.Match(httpContext, this, Constants.Store,
                       new RouteValueDictionary { { Constants.Store, storeCandidate } }.Merge(values), RouteDirection.IncomingRequest))
                    {
                        storeFound = true;
                        values.Add(Constants.Store, storeCandidate);
                    }
                    else
                    {
                        values.Add(Constants.Store, StoreHelper.CustomerSession.StoreId);
                    }

                    //Shift remaining route values
                    if (pathSegments.Length == 1 && !languageFound && !storeFound)
                    {
                        //there was one segment not mapped to lang or store
                        values.Add(Constants.Category, pathSegments[0]);

                    }
                    else if (pathSegments.Length >= 2)
                    {
                        //Both lang and store not found
                        if (!languageFound & !storeFound)
                        {
                            values.Add(Constants.Category, pathSegments[0]);
                            values.Add(Constants.Item, pathSegments[1]);
                        }
                        //etiher lang or store found
                        else if (languageFound ^ storeFound)
                        {
                            values.Add(Constants.Category, pathSegments[1]);
                            if (pathSegments.Length > 2)
                            {
                                values.Add(Constants.Item, pathSegments[2]);
                            }
                        }
                        else
                        {
                            if (pathSegments.Length > 2)
                            {
                                values.Add(Constants.Category, pathSegments[2]);
                            }
                            if (pathSegments.Length > 3)
                            {
                                values.Add(Constants.Item, pathSegments[3]);
                            }
                        }
                    }

                    //Store route cannot contain item or category
                    if (GetType() == typeof(StoreRoute) && (values.ContainsKey(Constants.Item) || values.ContainsKey(Constants.Category)))
                    {
                        return null;
                    }
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

            //Decode route value
            var store = routeData.Values[Constants.Store].ToString();
            routeData.Values[Constants.Store] = SettingsHelper.SeoDecode(store, SeoUrlKeywordTypes.Store, routeData.Values[Constants.Language].ToString());

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
