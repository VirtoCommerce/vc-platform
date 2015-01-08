using System;
using System.Linq;
using VirtoCommerce.Foundation;
using VirtoCommerce.Foundation.Customers.Services;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.Foundation.Frameworks.Extensions;
using VirtoCommerce.Foundation.Stores;
using VirtoCommerce.Foundation.Stores.Model;
using VirtoCommerce.Foundation.Stores.Repositories;


namespace VirtoCommerce.Client
{
    public class StoreClient
    {
        #region Cache Constants
        public const string StoreCacheKey = "S:S:{0}";
        public const string CountriesCacheKey = "S:C:{0}";
        public const string FulfillmentCacheKey = "S:F:{0}";
        #endregion

        #region Private Variables
        private readonly bool _isEnabled;
        private readonly IStoreRepository _storeRepository;
        private readonly ICacheRepository _cacheRepository;
        private readonly ICustomerSessionService _customerSession;
        #endregion


        /// <summary>
        /// Initializes the <see cref="StoreClient"/> class.
        /// </summary>
        public StoreClient(IStoreRepository storeRepository, ICustomerSessionService customerSession, ICacheRepository cacheRepository)
        {
            _storeRepository = storeRepository;
            _cacheRepository = cacheRepository;
            _customerSession = customerSession;
            _isEnabled = StoreConfiguration.Instance.Cache.IsEnabled;
        }

        /// <summary>
        /// Gets the store by id.
        /// </summary>
        /// <param name="storeId">The store id.</param>
        /// <returns></returns>
        public Store GetStoreById(string storeId)
        {
            var allStores = GetStores();

            //return allStores.Where(x => x.StoreId == storeId || storeId == "").FirstOrDefault();
            return allStores.Where(x => x.StoreId.Equals(storeId, StringComparison.OrdinalIgnoreCase) || storeId == "").FirstOrDefault();
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
            var store = stores.FirstOrDefault(x => x.StoreId == currentStoreId);

            if (store == null || store.LinkedStores == null || !store.LinkedStores.Any() || !(store.LinkedStores.Select(p => p.LinkedStoreId == storeId).Any()))
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
                        return _storeRepository.Stores.ExpandAll().ToArray();
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
							  (!string.IsNullOrEmpty(s.SecureUrl) && url.Contains(s.SecureUrl)) select s).ToArray();

            return stores.Length > 0 ? stores[0].StoreId : String.Empty;
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

            var store = GetStoreById(session.StoreId);

            session["store"] = store;

            return store;
        }

        CacheHelper _cacheHelper;
        public CacheHelper Helper
        {
            get { return _cacheHelper ?? (_cacheHelper = new CacheHelper(_cacheRepository)); }
        }
    }

}
