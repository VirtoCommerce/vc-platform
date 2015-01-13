using System;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using VirtoCommerce.ApiClient;
using VirtoCommerce.ApiClient.DataContracts.Store;
using VirtoCommerce.ApiClient.Extensions;
using VirtoCommerce.ApiWebClient.Caching;
using VirtoCommerce.ApiWebClient.Caching.Interfaces;
using VirtoCommerce.ApiWebClient.Configuration.Store;
using VirtoCommerce.ApiWebClient.Customer.Services;

namespace VirtoCommerce.ApiWebClient.Clients
{
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

        public ApiClient.StoreClient StoreApiClient
        {
            get
            {
                //TODO: get correct language
                return ClientContext.Clients.CreateStoreClient(string.Format(StoreConfiguration.Instance.Connection.DataServiceUri, "en-us"));
            }
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

                language = language ?? _customerSession.CustomerSession.Language;
                var langInfo = TryGetCultureInfo(language);
                language = langInfo != null ? langInfo.Name : language;

                store = allStores.FirstOrDefault(x => x.SeoKeywords != null &&
                                                      x.SeoKeywords.Any(
                                                          k =>
                                                              k.Language.Equals(language,
                                                                  StringComparison.InvariantCultureIgnoreCase) &&
                                                              k.Keyword.Equals(slug,
                                                                  StringComparison.InvariantCultureIgnoreCase)));
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
            return Helper.Get(
                CacheHelper.CreateCacheKey(Constants.StoreCachePrefix, string.Format(StoreCacheKey, "all")),
                () =>
                {
                    try
                    {
                        return Task.Run(() => StoreApiClient.GetStoresAsync()).Result;
                    }
                    catch
                    {

                        return new Store[0];
                    }
                },
                StoreConfiguration.Instance.Cache.StoreTimeout,
                _isEnabled);
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

                if (!string.IsNullOrEmpty(languageCode))
                    return CultureInfo.CreateSpecificCulture(languageCode);
            }
            catch
            {
            }
            return null;
        }
    }

}
