using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using VirtoCommerce.Foundation;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.Foundation.Stores.Model;
using VirtoCommerce.Foundation.Stores.Repositories;
using VirtoCommerce.Framework.Web.Settings;

namespace VirtoCommerce.MerchandisingModule.Web.Controllers
{
    public class BaseController : ApiController
    {
        #region Constants

        public const string StoreCacheKey = "S:{0}";

        #endregion

        #region Fields

        private readonly CacheHelper _cache;
        private readonly ISettingsManager _settingsManager;
        private readonly Func<IStoreRepository> _storeRepository;

        #endregion

        #region Constructors and Destructors

        public BaseController(
            Func<IStoreRepository> storeRepository,
            ISettingsManager settingsManager,
            ICacheRepository cache)
        {
            this._settingsManager = settingsManager;
            this._storeRepository = storeRepository;
            this._cache = new CacheHelper(cache);
        }

        #endregion

        #region Properties

        protected CacheHelper Cache
        {
            get { return this._cache; }
        }

        #endregion

        #region Methods

        protected virtual string GetCatalogId(string storeId)
        {
            var stores = this.GetAllStores();

            if (stores.Any())
            {
                var store =
                    stores.FirstOrDefault(x => x.StoreId.Equals(storeId, StringComparison.InvariantCultureIgnoreCase));
                if (store != null)
                {
                    return store.Catalog;
                }
            }

            throw new InvalidOperationException(string.Format("No store exits with id {0}", storeId));
        }

        private IEnumerable<Store> GetAllStores()
        {
            var storeTimeout = this._settingsManager.GetValue("Stores.Caching.StoreTimeout", 30);
            return this._cache.Get(
                CacheHelper.CreateCacheKey(Constants.StoreCachePrefix, string.Format(StoreCacheKey, "all")),
                () => this.GetAllStoresInternal(),
                new TimeSpan(0, 0, storeTimeout)).AsQueryable();
        }

        private Store[] GetAllStoresInternal()
        {
            using (var repository = this._storeRepository())
            {
                var stores = repository.Stores;
                return stores.ToArray();
            }
        }

        #endregion
    }
}
