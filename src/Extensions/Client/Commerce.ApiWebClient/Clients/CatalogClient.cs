using System.Linq;
using System.Net;
using System.Threading.Tasks;
using VirtoCommerce.ApiClient;
using VirtoCommerce.ApiClient.DataContracts;
using VirtoCommerce.ApiClient.Extensions;
using VirtoCommerce.ApiWebClient.Caching;
using VirtoCommerce.ApiWebClient.Caching.Interfaces;
using VirtoCommerce.ApiWebClient.Configuration.Catalog;

namespace VirtoCommerce.ApiWebClient.Clients
{
    public class CatalogClient
    {
        #region Cache Constants
        public const string ItemCacheKey = "C:I:{0}:g:{1}";
        public const string ItemCodeCacheKey = "C:Ic:{0}:g:{1}";
        public const string CategoriesCacheKey = "C:CT:{0}:{1}";
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
                string.Format(CatalogConfiguration.Instance.Connection.DataServiceUri, catalog, lang));

        }
        #region Item Methods

        /// <summary>
        /// Gets the item.
        /// </summary>
        /// <param name="slug">The slug. Can be id or seo keyword</param>
        /// <param name="catalogId">The catalog identifier.</param>
        /// <param name="language">The language.</param>
        /// <param name="responseGroup">The response group.</param>
        /// <param name="useCache">if set to <c>true</c> [use cache].</param>
        /// <returns></returns>
        public async Task<CatalogItem> GetItemAsync(string slug, string catalogId, string language, ItemResponseGroups responseGroup = ItemResponseGroups.ItemMedium, bool useCache = true)
        {
            var client = GetClient(language, catalogId);

            try
            {
                return await Helper.GetAsync(
                    CacheHelper.CreateCacheKey(Constants.CatalogCachePrefix,
                        string.Format(ItemCacheKey, CacheHelper.CreateCacheKey(slug), responseGroup)),
                    () => client.GetProductAsync(slug, responseGroup),
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
        /// <param name="catalogId">The catalog identifier.</param>
        /// <param name="language">The language.</param>
        /// <param name="useCache">if set to <c>true</c> [use cache].</param>
        /// <param name="responseGroup">The response group.</param>
        /// <returns></returns>
        public async Task<CatalogItem> GetItemByCodeAsync(string code, string catalogId, string language, bool useCache = true, ItemResponseGroups responseGroup = ItemResponseGroups.ItemMedium)
        {
            var client = GetClient(language, catalogId);
            return await Helper.GetAsync(
              CacheHelper.CreateCacheKey(Constants.CatalogCachePrefix, string.Format(ItemCodeCacheKey, CacheHelper.CreateCacheKey(code), responseGroup)),
                () =>
                {
                    try
                    {
                        return client.GetProductByCodeAsync(code,responseGroup);
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
        /// <summary>
        /// Gets the category asynchronous.
        /// </summary>
        /// <param name="slug">The slug. Can by id or seo keyword</param>
        /// <param name="catalogId">The catalog identifier.</param>
        /// <param name="language">The language.</param>
        /// <param name="useCache">if set to <c>true</c> [use cache].</param>
        /// <returns></returns>
        public async Task<Category> GetCategoryAsync(string slug, string catalogId, string language, bool useCache = true)
        {
            var client = GetClient(language, catalogId);

            try
            {
                return await Helper.GetAsync(
                    CacheHelper.CreateCacheKey(Constants.CatalogCachePrefix,
                         string.Format(CategoryIdCacheKey, catalogId, slug)),
                    () => client.GetCategoryAsync(slug),
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

        public async Task<Category[]> GetCategoriesAsync(string catalogId, string language, bool useCache = true)
        {
            var client = GetClient(language, catalogId);

            try
            {
                var result = await Helper.GetAsync(
                    CacheHelper.CreateCacheKey(Constants.CatalogCachePrefix,
                         string.Format(CategoriesCacheKey, catalogId, language)),
                    () =>client.GetCategoriesAsync(),
                    CatalogConfiguration.Instance.Cache.CategoryTimeout,
                    _isEnabled && useCache);

                if (result.TotalCount > 0)
                {
                    return result.Items.ToArray();
                }
            }
            catch (ManagementClientException ex)
            {
                if (ex.StatusCode == HttpStatusCode.NotFound)
                {
                    return null;
                }
                throw;
            }

            return new Category[0];
        }
        #endregion


        CacheHelper _cacheHelper;
        public CacheHelper Helper
        {
            get { return _cacheHelper ?? (_cacheHelper = new CacheHelper(_cacheRepository)); }
        }
    }

}
