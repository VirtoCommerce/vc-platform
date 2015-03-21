using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Routing;
using VirtoCommerce.ApiWebClient.Extensions.Routing.Constraints;
using VirtoCommerce.ApiWebClient.Helpers;
using VirtoCommerce.Web.Core.DataContracts;
using VirtoCommerce.Web.Core.DataContracts.Store;

namespace VirtoCommerce.ApiWebClient.Extensions.Routing.Routes
{
    using VirtoCommerce.ApiClient;

    public class StoreRoute : Route
    {

        private Object thisLock = new Object();

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
            if (!values.ContainsKey(Constants.Store))
            {
                values.Add(Constants.Store, ClientContext.Session.StoreId);
            }

            //var storeId = SettingsHelper.SeoDecode(values[Constants.Store].ToString(), SeoUrlKeywordTypes.Store, values[Constants.Language] as string);
            var storeSlug = values[Constants.Store].ToString();
            var store = StoreHelper.StoreClient.GetStore(storeSlug, values[Constants.Language] as string);

            if (!values.ContainsKey(Constants.Language))
            {
                values.Add(Constants.Language,
                    ClientContext.Session.Language ??
                    StoreHelper.StoreClient.GetCurrentStore().DefaultLanguage);
            }

            if (store != null && !IsValidStoreLanguage(store, values[Constants.Language].ToString()))
            {
                //Reset to default language if validation fails
                values[Constants.Language] = store.DefaultLanguage;
            }

            var isLanguageNeeded = IsLanguageNeeded(store);
            var isStoreNeeded = IsStoreNeeded(store);


            if (!isStoreNeeded || !isLanguageNeeded)
            {
                //Need to be in lock to make sure other thread does not change originalUrl in this block
                lock (thisLock)
                {
                    var modifiedUrl = Url;

                    //If for request store URL is used do not show it in path
                    if (!isStoreNeeded)
                    {
                        modifiedUrl = modifiedUrl.Replace(string.Format("/{{{0}}}", Constants.Store), string.Empty);
                        values.Remove(Constants.Store);
                    }
                    else
                    {
                        EncodeVirtualPath(values, SeoUrlKeywordTypes.Store);
                    }

                    if (!isLanguageNeeded)
                    {
                        modifiedUrl = modifiedUrl.Replace(string.Format("{{{0}}}", Constants.Language), string.Empty);
                        values.Remove(Constants.Language);
                    }

                    //The route URL cannot start with a '/' or '~' character and it cannot contain a '?' character.
                    if (modifiedUrl.StartsWith("/") || modifiedUrl.StartsWith("~"))
                    {
                        modifiedUrl = modifiedUrl.Substring(1, modifiedUrl.Length - 1);
                    }

                    var originalUrl = Url;

                    Url = modifiedUrl;

                    var retVal = base.GetVirtualPath(requestContext, values);

                    //Restore original URL
                    if (!string.IsNullOrEmpty(originalUrl) && !originalUrl.Equals(Url))
                    {
                        Url = originalUrl;
                    }

                    return retVal;
                }
            }

            EncodeVirtualPath(values, SeoUrlKeywordTypes.Store);
            return base.GetVirtualPath(requestContext, values);

        }

        public override RouteData GetRouteData(HttpContextBase httpContext)
        {
            //Check if MVC can resolve route by itself
            var routeData = base.GetRouteData(httpContext);

            //Otherwise lets resolve ourselves
            if (routeData == null)
            {
                var requestPath = httpContext.Request.AppRelativeCurrentExecutionFilePath.Substring(2) +
                                  httpContext.Request.PathInfo;
                var pathSegments = requestPath.Split(new[] { '/' }, StringSplitOptions.RemoveEmptyEntries);

                //Store route can only have up to 2 segments
                //Other routes have unlimited number of segments due to category path
                if (GetType() == typeof(StoreRoute) && pathSegments.Length > Url.Split(new[] { '/' }).Length)
                {
                    return null;
                }

                var values = new RouteValueDictionary();

                //If route does not have any language or store add defaults to route data

                //If both store and languange are missing
                if (pathSegments.Length == 0)
                {
                    values.Add(Constants.Language,
                        ClientContext.Session.Language ??
                        StoreHelper.StoreClient.GetCurrentStore().DefaultLanguage);
                    values.Add(Constants.Store, ClientContext.Session.StoreId);
                }
                else
                {
                    //Check if some values match constraint for language or store. The expected route is {lang}/{store}/...
                    //But Language and/or store can be skipped then defaults or current values are used
                    var languageConstraint = new LanguageRouteConstraint();

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
                        ClientContext.Session.Language ??
                        StoreHelper.StoreClient.GetCurrentStore().DefaultLanguage);
                    }

                    var storeFound = false;
                    var storeCandidate = !languageFound
                        ? pathSegments[0]
                        : pathSegments.Length > 1 ? pathSegments[1] : null;
                    var storeConstraint = new StoreRouteConstraint();

                    if (!string.IsNullOrEmpty(storeCandidate) && storeConstraint.Match(httpContext, this, Constants.Store,
                       new RouteValueDictionary { { Constants.Store, storeCandidate } }.Merge(values), RouteDirection.IncomingRequest))
                    {
                        storeFound = true;
                        values.Add(Constants.Store, storeCandidate);
                    }
                    else
                    {
                        values.Add(Constants.Store, ClientContext.Session.StoreId);
                    }

                    int startIndex;
                    //Both lang and store not found
                    if (!languageFound & !storeFound)
                    {
                        startIndex = 0;
                    }
                    //either lang or store found
                    else if (languageFound ^ storeFound)
                    {
                        startIndex = 1;
                    }
                    else
                    {
                        startIndex = 2;
                    }

                    if (pathSegments.Length > startIndex)
                    {
                        var categoryParseEndIndex = pathSegments.Length;

                        if (GetType() == typeof(ItemRoute))
                        {
                            categoryParseEndIndex = pathSegments.Length - 1;
                            //Last must be item code
                            values.Add(Constants.Item, pathSegments[categoryParseEndIndex]);
                        }
                        //Parse category path
                        if (categoryParseEndIndex > startIndex)
                        {
                            var categoryPath = string.Empty;
                            for (var i = startIndex; i < categoryParseEndIndex; i++)
                            {
                                if (!string.IsNullOrEmpty(categoryPath))
                                {
                                    categoryPath += "/";
                                }
                                categoryPath += pathSegments[i];
                            }
                            values.Add(Constants.Category, categoryPath);
                        }
                    }

                    if (values.Keys.Any(key => !Url.Contains(key)))
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

            return routeData;
        }

        protected virtual void EncodeVirtualPath(RouteValueDictionary values, SeoUrlKeywordTypes type)
        {
            var routeValueKey = type.ToString().ToLower();
            var session = StoreHelper.CustomerSession;
            var language = values.ContainsKey(Constants.Language) ? values[Constants.Language] as string : session.Language;

            if (values.ContainsKey(routeValueKey) && values[routeValueKey] != null)
            {
                values[routeValueKey] = SettingsHelper.EncodeRouteValue(values[routeValueKey].ToString(), type, language);           
            }
        }


        public virtual string GetMainRouteKey()
        {
            return Constants.Store;
        }

        protected bool ProcessConstraints(HttpContextBase httpContext, RouteValueDictionary values, RouteDirection routeDirection)
        {
            return Constraints == null || Constraints.All(constraintsItem => ProcessConstraint(httpContext, constraintsItem.Value, constraintsItem.Key, values, routeDirection));
        }

        protected virtual bool IsLanguageNeeded(Store dbStore)
        {
            if (dbStore != null)
            {
                return dbStore.Languages.Length > 1;
            }
            return true;
        }

        /// <summary>
        /// Returns true if there is only not closed store or current store override url
        /// </summary>
        /// <param name="dbStore"></param>
        /// <returns></returns>
        protected virtual bool IsStoreNeeded(Store dbStore)
        {
            if (StoreHelper.StoreClient.GetStores().Count(x => x.StoreState != StoreState.Closed) <= 1)
            {
                return false;
            }

            if (dbStore != null)
            {
                return string.IsNullOrEmpty(dbStore.Url) && string.IsNullOrEmpty(dbStore.SecureUrl);
            }

            return true;
        }
        /// <summary>
        /// Returns true if store has only one available language
        /// </summary>
        /// <param name="dbStore"></param>
        /// <param name="lang"></param>
        /// <returns></returns>
        private bool IsValidStoreLanguage(Store dbStore, string lang)
        {
            try
            {
                if (dbStore == null)
                {
                    return false;
                }

                var culture = lang.ToSpecificLangCode();
                if (!dbStore.Languages.Any(l => l.ToSpecificLangCode().Equals(culture, StringComparison.InvariantCultureIgnoreCase)))
                {
                    //Store does not support this language
                    return false;
                }

            }
            catch
            {
                //Language is not valid
                return false;
            }

            return true;
        }
    }
}
