#region
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Routing;
using VirtoCommerce.ApiClient;
using VirtoCommerce.ApiClient.DataContracts;
using VirtoCommerce.ApiClient.DataContracts.Stores;
using VirtoCommerce.ApiClient.Extensions;
using VirtoCommerce.Web.Models.Convertors;
using VirtoCommerce.Web.Models.Extensions;
using VirtoCommerce.Web.Models.Routing.Constraints;

#endregion

namespace VirtoCommerce.Web.Models.Routing.Routes
{
    public class StoreRoute : Route
    {
        #region Fields
        private readonly Object thisLock = new Object();
        #endregion

        #region Constructors and Destructors
        public StoreRoute(string url, IRouteHandler routeHandler)
            : base(url, routeHandler)
        {
        }

        public StoreRoute(string url, RouteValueDictionary defaults, IRouteHandler routeHandler)
            : base(url, defaults, routeHandler)
        {
        }

        public StoreRoute(
            string url,
            RouteValueDictionary defaults,
            RouteValueDictionary constraints,
            IRouteHandler routeHandler)
            : base(url, defaults, constraints, routeHandler)
        {
        }

        public StoreRoute(
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
        public SeoKeyword GetKeyword(string routeValue, SeoUrlKeywordTypes type, string language = null)
        {
            routeValue = routeValue.Split(new[] { '/' }, StringSplitOptions.RemoveEmptyEntries).Last();

            var langInfo = language != null ? language.TryGetCultureInfo() : Thread.CurrentThread.CurrentUICulture;
            language = langInfo != null ? langInfo.Name : language;
            var keyword = new SeoKeyword { Keyword = routeValue, Language = language };
            var storeId = SiteContext.Current.StoreId;
            var client = ClientContext.Clients.CreateBrowseClient();
            switch (type)
            {
                case SeoUrlKeywordTypes.Store:
                    var store = SiteContext.Current.GetShopBySlug(routeValue);
                    if (store != null && store.Keywords != null)
                    {
                        keyword = store.Keywords.SeoKeyword(language);
                    }
                    break;
                case SeoUrlKeywordTypes.Category:
                    var category =
                        Task.Run(() => client.GetCategoryAsync(storeId, language, routeValue)).Result.AsWebModel();
                    if (category != null)
                    {
                        keyword = category.Keywords.SeoKeyword(language);
                    }
                    break;
                case SeoUrlKeywordTypes.Item:
                    var item =
                        Task.Run(
                            () => client.GetProductAsync(storeId, language, routeValue, ItemResponseGroups.ItemMedium))
                            .Result;//.AsWebModel();
                    //if (item != null)
                    //{
                    //    keyword = item.Keywords.SeoKeyword(language);
                    //}
                    break;
            }

            return keyword;
        }

        public virtual string GetMainRouteKey()
        {
            return Constants.Store;
        }

        public override RouteData GetRouteData(HttpContextBase httpContext)
        {
            //Check if MVC can resolve route by itself
            var routeData = base.GetRouteData(httpContext);

            //Otherwise lets resolve ourselves
            if (routeData == null)
            {
                var requestPath = httpContext.Request.AppRelativeCurrentExecutionFilePath.Substring(2)
                                  + httpContext.Request.PathInfo;
                var pathSegments = requestPath.Split(new[] { '/' }, StringSplitOptions.RemoveEmptyEntries);

                //Store route can only have up to 2 segments
                //Other routes have unlimited number of segments due to category path
                if (this.GetType() == typeof(StoreRoute) && pathSegments.Length > this.Url.Split(new[] { '/' }).Length)
                {
                    return null;
                }

                var values = new RouteValueDictionary();

                //If route does not have any language or store add defaults to route data

                //If both store and languange are missing
                if (pathSegments.Length == 0)
                {
                    values.Add(Constants.Language, Thread.CurrentThread.CurrentUICulture);
                    values.Add(Constants.Store, SiteContext.Current.StoreId);
                }
                else
                {
                    //Check if some values match constraint for language or store. The expected route is {lang}/{store}/...
                    //But Language and/or store can be skipped then defaults or current values are used
                    var languageConstraint = new LanguageRouteConstraint();

                    var languageFound = false;
                    if (languageConstraint.Match(
                        httpContext,
                        this,
                        Constants.Language,
                        new RouteValueDictionary { { Constants.Language, pathSegments[0] } }.Merge(values),
                        RouteDirection.IncomingRequest))
                    {
                        languageFound = true;
                        values.Add(Constants.Language, pathSegments[0]);
                    }
                    else
                    {
                        values.Add(Constants.Language, Thread.CurrentThread.CurrentUICulture);
                    }

                    var storeFound = false;
                    var storeCandidate = !languageFound
                        ? pathSegments[0]
                        : pathSegments.Length > 1 ? pathSegments[1] : null;
                    var storeConstraint = new StoreRouteConstraint();

                    if (!string.IsNullOrEmpty(storeCandidate)
                        && storeConstraint.Match(
                            httpContext,
                            this,
                            Constants.Store,
                            new RouteValueDictionary { { Constants.Store, storeCandidate } }.Merge(values),
                            RouteDirection.IncomingRequest))
                    {
                        storeFound = true;
                        values.Add(Constants.Store, storeCandidate);
                    }
                    else
                    {
                        values.Add(Constants.Store, SiteContext.Current.StoreId);
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

                        if (this.GetType() == typeof(ItemRoute))
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

                    if (values.Keys.Any(key => !this.Url.Contains(key)))
                    {
                        return null;
                    }
                }

                routeData = new RouteData(this, this.RouteHandler);

                // Validate the values
                if (!this.ProcessConstraints(httpContext, values, RouteDirection.IncomingRequest))
                {
                    return null;
                }

                // Copy the matched values
                foreach (var value in values)
                {
                    routeData.Values.Add(value.Key, value.Value);
                }

                // Copy the DataTokens from the Route to the RouteData 
                if (this.DataTokens != null)
                {
                    foreach (var prop in this.DataTokens)
                    {
                        routeData.DataTokens[prop.Key] = prop.Value;
                    }
                }

                // Copy the remaining default values
                foreach (var value in this.Defaults.Where(value => !routeData.Values.ContainsKey(value.Key)))
                {
                    routeData.Values.Add(value.Key, value.Value);
                }
            }

            return routeData;
        }

        public override VirtualPathData GetVirtualPath(RequestContext requestContext, RouteValueDictionary values)
        {
            if (!values.ContainsKey(Constants.Store))
            {
                values.Add(Constants.Store, SiteContext.Current.Shop.StoreId);
            }

            //var storeId = SettingsHelper.SeoDecode(values[Constants.Store].ToString(), SeoUrlKeywordTypes.Store, values[Constants.Language] as string);
            var storeSlug = values[Constants.Store].ToString();
            var store = SiteContext.Current.GetShopBySlug(storeSlug, values[Constants.Language] as string);

            if (!values.ContainsKey(Constants.Language))
            {
                values.Add(Constants.Language, Thread.CurrentThread.CurrentUICulture.Name);
            }

            if (store != null && !this.IsValidStoreLanguage(store, values[Constants.Language].ToString()))
            {
                //Reset to default language if validation fails
                values[Constants.Language] = store.DefaultLanguage;
            }

            var isLanguageNeeded = this.IsLanguageNeeded(store);
            var isStoreNeeded = this.IsStoreNeeded(store);

            if (!isStoreNeeded || !isLanguageNeeded)
            {
                //Need to be in lock to make sure other thread does not change originalUrl in this block
                lock (this.thisLock)
                {
                    var modifiedUrl = this.Url;

                    //If for request store URL is used do not show it in path
                    if (!isStoreNeeded)
                    {
                        modifiedUrl = modifiedUrl.Replace(string.Format("/{{{0}}}", Constants.Store), string.Empty);
                        values.Remove(Constants.Store);
                    }
                    else
                    {
                        this.EncodeVirtualPath(values, SeoUrlKeywordTypes.Store);
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

                    var originalUrl = this.Url;

                    this.Url = modifiedUrl;

                    var retVal = base.GetVirtualPath(requestContext, values);

                    //Restore original URL
                    if (!string.IsNullOrEmpty(originalUrl) && !originalUrl.Equals(this.Url))
                    {
                        this.Url = originalUrl;
                    }

                    return retVal;
                }
            }

            this.EncodeVirtualPath(values, SeoUrlKeywordTypes.Store);
            return base.GetVirtualPath(requestContext, values);
        }
        #endregion

        #region Methods
        protected virtual void EncodeVirtualPath(RouteValueDictionary values, SeoUrlKeywordTypes type)
        {
            var routeValueKey = type.ToString().ToLower();
            var language = values.ContainsKey(Constants.Language)
                ? values[Constants.Language] as string
                : Thread.CurrentThread.CurrentUICulture.Name;

            if (values.ContainsKey(routeValueKey) && values[routeValueKey] != null)
            {
                values[routeValueKey] = this.EncodeRouteValue(values[routeValueKey].ToString(), type, language);
            }
        }

        protected virtual bool IsLanguageNeeded(Shop dbStore)
        {
            return dbStore == null || dbStore.Languages.Any();
        }

        /// <summary>
        ///     Returns true if there is only not closed store or current store override url
        /// </summary>
        /// <param name="dbStore"></param>
        /// <returns></returns>
        protected virtual bool IsStoreNeeded(Shop dbStore)
        {
            if (SiteContext.Current.Shops.Count(x => x.State != StoreState.Closed) <= 1)
            {
                return false;
            }

            if (dbStore != null)
            {
                return string.IsNullOrEmpty(dbStore.Url) && string.IsNullOrEmpty(dbStore.SecureUrl);
            }

            return true;
        }

        protected bool ProcessConstraints(
            HttpContextBase httpContext,
            RouteValueDictionary values,
            RouteDirection routeDirection)
        {
            return this.Constraints == null
                   || this.Constraints.All(
                       constraintsItem =>
                           this.ProcessConstraint(
                               httpContext,
                               constraintsItem.Value,
                               constraintsItem.Key,
                               values,
                               routeDirection));
        }

        private string EncodeRouteValue(string routeValue, SeoUrlKeywordTypes type, string language = null)
        {
            if (!string.IsNullOrEmpty(routeValue))
            {
                routeValue = routeValue.Split(new[] { '/' }, StringSplitOptions.RemoveEmptyEntries).Last();
                var keyword = routeValue;
                    // this.GetKeyword(routeValue, type, language); Sasha: routevalue here is already a keyword

                var client = ClientContext.Clients.CreateBrowseClient();

                if (keyword != null)
                {
                    switch (type)
                    {
                        case SeoUrlKeywordTypes.Store:
                        case SeoUrlKeywordTypes.Item:
                            return routeValue;
                        case SeoUrlKeywordTypes.Category:
                            /*
                            var category =
                                Task.Run(
                                    () =>
                                        client.GetCategoryAsync(routeValue))
                                    .Result.AsWebModel();
                            if (category != null)
                            {
                                return string.Join("/", category.BuildOutline(language).Select(x => x.Value));
                            }
                             * */

                            return routeValue; // routevalue for category is outline
                            break;
                    }
                }
            }

            return routeValue;
        }

        /// <summary>
        ///     Returns true if store has only one available language
        /// </summary>
        /// <param name="dbStore"></param>
        /// <param name="lang"></param>
        /// <returns></returns>
        private bool IsValidStoreLanguage(Shop dbStore, string lang)
        {
            try
            {
                if (dbStore == null)
                {
                    return false;
                }

                var culture = lang.ToSpecificLangCode();
                if (
                    !dbStore.Languages.Any(
                        l => l.ToSpecificLangCode().Equals(culture, StringComparison.InvariantCultureIgnoreCase)))
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
        #endregion
    }
}