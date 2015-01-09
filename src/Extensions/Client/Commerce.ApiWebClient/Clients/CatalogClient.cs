using System.Net;
using System.Threading.Tasks;
using VirtoCommerce.ApiClient;
using VirtoCommerce.ApiClient.DataContracts;
using VirtoCommerce.ApiClient.Extensions;
using VirtoCommerce.ApiWebClient.Caching;
using VirtoCommerce.ApiWebClient.Caching.Interfaces;
using VirtoCommerce.ApiWebClient.Configuration.Catalog;
using VirtoCommerce.ApiWebClient.Customer.Services;

namespace VirtoCommerce.ApiWebClient.Clients
{
    public class CatalogClient
    {
        #region Cache Constants
        public const string ItemCacheKey = "C:I:{0}:g:{1}";
        public const string ItemCodeCacheKey = "C:Ic:{0}:g:{1}";
        public const string CategoryCacheKey = "C:CT:{0}:{1}";
        public const string CategoryIdCacheKey = "C:CTID:{0}:{1}";
        #endregion

        #region Private Variables
        private readonly bool _isEnabled;
        private readonly ICacheRepository _cacheRepository;
        #endregion


        /// <summary>
        /// Initializes a new instance of the <see cref="CatalogClient"/> class.
        /// </summary>
        /// <param name="cacheRepository">The cache repository.</param>
        public CatalogClient(ICacheRepository cacheRepository)
        {
            _cacheRepository = cacheRepository;
            _isEnabled = CatalogConfiguration.Instance.Cache.IsEnabled;
        }

        public BrowseClient GetClient(string lang, string catalog)
        {
            return ClientContext.Clients.CreateBrowseClient(
                string.Format(CatalogConfiguration.Instance.Connection.DataServiceUri, lang, catalog));

        }
        #region Item Methods

        /// <summary>
        /// Gets the item.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="catalogId">The catalog identifier.</param>
        /// <param name="language">The language.</param>
        /// <param name="responseGroup">The response group.</param>
        /// <param name="useCache">if set to <c>true</c> [use cache].</param>
        /// <returns></returns>
        public async Task<CatalogItem> GetItemAsync(string id, string catalogId, string language, ItemResponseGroups responseGroup = ItemResponseGroups.ItemSmall, bool useCache = true)
        {
            var client = GetClient(language, catalogId);

            try
            {
                return await Helper.GetAsync(
                    CacheHelper.CreateCacheKey(Constants.CatalogCachePrefix,
                        string.Format(ItemCacheKey, CacheHelper.CreateCacheKey(id), responseGroup)),
                    () => client.GetProductAsync(id),
                    CatalogConfiguration.Instance.Cache.ItemTimeout,
                    _isEnabled && useCache);
            }
            catch (ManagementClientException ex)
            {
                if (ex.StatusCode == HttpStatusCode.NotFound)
                {
                    return null;
                }
                throw;
            }
        }



        /// <summary>
        /// Gets the item by code.
        /// </summary>
        /// <param name="code">The code.</param>
        /// <param name="useCache">if set to <c>true</c> [use cache].</param>
        /// <param name="responseGroup">The response group.</param>
        /// <returns></returns>
        public async Task<CatalogItem> GetItemByCodeAsync(string code, string catalogId, string language, bool useCache = true, ItemResponseGroups responseGroup = ItemResponseGroups.ItemSmall)
        {
            var client = GetClient(language, catalogId);
            return await Helper.GetAsync(
              CacheHelper.CreateCacheKey(Constants.CatalogCachePrefix, string.Format(ItemCodeCacheKey, CacheHelper.CreateCacheKey(code), responseGroup)),
                () =>
                {
                    try
                    {
                        return client.GetProductByCodeAsync(code);
                    }
                    catch (ManagementClientException ex)
                    {
                        if (ex.StatusCode == HttpStatusCode.NotFound)
                        {
                            return null;
                        }
                        throw;
                    }
                },
              CatalogConfiguration.Instance.Cache.ItemTimeout,
              _isEnabled && useCache);
        }


        #endregion

        #region Catalog methods
        public async Task<Category> GetCategoryAsync(string id, string catalogId, string language, bool useCache = true)
        {
            var client = GetClient(language, catalogId);

            try
            {
                return await Helper.GetAsync(
                    CacheHelper.CreateCacheKey(Constants.CatalogCachePrefix,
                         string.Format(CategoryIdCacheKey, catalogId, id)),
                    () => client.GetCategoryAsync(id),
                    CatalogConfiguration.Instance.Cache.CategoryTimeout,
                    _isEnabled && useCache);
            }
            catch (ManagementClientException ex)
            {
                if (ex.StatusCode == HttpStatusCode.NotFound)
                {
                    return null;
                }
                throw;
            }
        }
        #endregion


        CacheHelper _cacheHelper;
        public CacheHelper Helper
        {
            get { return _cacheHelper ?? (_cacheHelper = new CacheHelper(_cacheRepository)); }
        }
    }

}
