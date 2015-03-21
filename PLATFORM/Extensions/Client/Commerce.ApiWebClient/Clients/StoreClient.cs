using System;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using VirtoCommerce.ApiClient;
using VirtoCommerce.ApiClient.Extensions;
using VirtoCommerce.ApiWebClient.Extensions;
using VirtoCommerce.Web.Core.Configuration.Store;
using VirtoCommerce.Web.Core.DataContracts.Store;

namespace VirtoCommerce.ApiWebClient.Clients
{
    using VirtoCommerce.ApiClient.Caching;
    using VirtoCommerce.ApiClient.Session;

    using CacheHelper = VirtoCommerce.ApiWebClient.Caching.CacheHelper;

    public class StoreClient
    {
        #region Cache Constants
        public const string StoreCacheKey = "S:S:{0}";
        #endregion

        #region Private Variables
        private readonly bool _isEnabled;
        private readonly ICacheRepository _cacheRepository;
        private readonly ICustomerSessionService _customerSession;
        #endregion


        /// <summary>
        /// Initializes the <see cref="StoreClient"/> class.
        /// </summary>
        public StoreClient(ICustomerSessionService customerSession, ICacheRepository cacheRepository)
        {
            _customerSession = customerSession;
            _cacheRepository = cacheRepository;
            _isEnabled = StoreConfiguration.Instance.Cache.IsEnabled;
        }

        public ApiClient.StoreClient GetClient(string lang)
        {
            return  ClientContext.Clients.CreateStoreClient();

        }


        /// <summary>
        /// Gets the store. First treats slug as storeId then as keyword
        /// </summary>t
        /// <param name="slug">The slug.</param>
        /// <param name="language">The language.</param>
        /// <returns></returns>
        public Store GetStore(string slug, string language = null)
        {
            var allStores = GetStores();

            var store = allStores.FirstOrDefault(x => x.Id.Equals(slug, StringComparison.OrdinalIgnoreCase));

            if (store == null)
            {

                var keyword =  allStores.SelectMany(x => x.SeoKeywords).Where(x=>x.Keyword.Equals(slug, StringComparison.InvariantCultureIgnoreCase)).ToArray().SeoKeyword(language);

                if (keyword != null)
                {
                    store = allStores.FirstOrDefault(x => x.Id.Equals(keyword.KeywordValue, StringComparison.InvariantCultureIgnoreCase));
                }
            }
            return store;
        }

        /// <summary>
        /// Determines whether is linked account store is authorized to access the specified store (of currently viewed store).
        /// </summary>
        /// <param name="storeId">The default store id for the user.</param>
        /// <param name="currentStoreId">The current store id.</param>
        /// <returns>
        ///   <c>true</c> if [is linked account authorized] [the specified store id]; otherwise, <c>false</c>.
        /// </returns>
        public bool IsLinkedAccountAuthorized(string storeId, string currentStoreId = "")
        {
            var isAuthorized = true;

            if (String.IsNullOrEmpty(currentStoreId))
            {
                var session = _customerSession.CustomerSession;
                if (session != null)
                {
                    currentStoreId = session.StoreId;
                }
            }

            if (String.IsNullOrEmpty(currentStoreId))
            {
                return false;
            }

            if (String.IsNullOrEmpty(storeId))
            {
                return false;
            }

            if (storeId.Equals(currentStoreId, StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }

            var stores = GetStores();

            // check linked stores
            var store = stores.FirstOrDefault(x => x.Id == currentStoreId);

            if (store == null || store.LinkedStores == null || !store.LinkedStores.Any() || !(store.LinkedStores.Contains(storeId)))
            {
                isAuthorized = false;
            }


            return isAuthorized;
        }

        /// <summary>
        /// Gets the stores.
        /// </summary>
        /// <returns></returns>
        public Store[] GetStores()
        {     
            var stores =  Helper.Get(
                CacheHelper.CreateCacheKey(Constants.StoreCachePrefix, string.Format(StoreCacheKey, "all")),
                () =>
                {
                    try
                    {
                        var client = GetClient("en-us");
                        return Task.Run(() => client.GetStoresAsync()).Result;
                    }
                    catch
                    {

                        return null;
                    }
                },
                StoreConfiguration.Instance.Cache.StoreTimeout,
                _isEnabled);

            return stores ?? new Store[0];
        }

        /// <summary>
        /// Gets the store id by URL.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <returns></returns>
        public string GetStoreIdByUrl(string url)
        {
            var allStores = GetStores();
            url = url.ToLower();
            var stores = (from s in allStores
                          where
                              (!string.IsNullOrEmpty(s.Url) && url.Contains(s.Url)) ||
                              (!string.IsNullOrEmpty(s.SecureUrl) && url.Contains(s.SecureUrl))
                          select s).ToArray();

            return stores.Length > 0 ? stores[0].Id : String.Empty;
        }

        /// <summary>
        /// Gets the current store.
        /// </summary>
        /// <returns></returns>
        public Store GetCurrentStore()
        {
            var session = _customerSession.CustomerSession;

            var storeObject = session["store"];
            if (storeObject != null)
                return storeObject as Store;

            if (string.IsNullOrEmpty(session.StoreId))
                return null;

            var store = GetStore(session.StoreId);

            session["store"] = store;

            return store;
        }

        CacheHelper _cacheHelper;
        public CacheHelper Helper
        {
            get { return _cacheHelper ?? (_cacheHelper = new CacheHelper(_cacheRepository)); }
        }

        private static CultureInfo TryGetCultureInfo(string languageCode)
        {
            try
            {
                return !string.IsNullOrEmpty(languageCode) ? CultureInfo.CreateSpecificCulture(languageCode) : null;
            }
            catch
            {
                return null;
            }
        }
    }

}
