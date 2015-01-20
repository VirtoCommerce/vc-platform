using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using VirtoCommerce.ApiClient.Extensions;
using VirtoCommerce.ApiClient.Utilities;
using VirtoCommerce.Web.Core.DataContracts;

namespace VirtoCommerce.ApiClient
{
    using VirtoCommerce.ApiClient.Caching;
    using VirtoCommerce.Web.Core.Configuration.Catalog;

    public class BrowseClient : BaseClient
    {
        protected class RelativePaths
        {
            public const string Products = "products";
            public const string Categories = "categories";
            public const string Category = "categories/{0}";
            public const string Product = "products/{0}";
        }

        /// <summary>
        /// Initializes a new instance of the AdminManagementClient class.
        /// </summary>
        /// <param name="adminBaseEndpoint">Admin endpoint</param>
        /// <param name="token">Access token</param>
        public BrowseClient(Uri adminBaseEndpoint, string token)
            : base(adminBaseEndpoint, new TokenMessageProcessingHandler(token))
        {
        }

        /// <summary>
        /// Initializes a new instance of the AdminManagementClient class.
        /// </summary>
        /// <param name="adminBaseEndpoint">Admin endpoint</param>
        /// <param name="handler"></param>
        public BrowseClient(Uri adminBaseEndpoint, MessageProcessingHandler handler)
            : base(adminBaseEndpoint, handler)
        {

        }

        /// <summary>
        /// List items matching the given query
        /// </summary>
        public virtual Task<ResponseCollection<Product>> GetProductsAsync(BrowseQuery query, ItemResponseGroups? responseGroup = null)
        {
            return GetAsync<ResponseCollection<Product>>(CreateRequestUri(RelativePaths.Products, query.GetQueryString(responseGroup)));
        }

        public virtual Task<Product> GetProductAsync(string productId, ItemResponseGroups responseGroup)
        {
            var query = new List<KeyValuePair<string, string>>
            {
                 new KeyValuePair<string, string>("responseGroup", responseGroup.GetHashCode().ToString()),
            };
            
            return GetAsync<Product>(CreateRequestUri(String.Format(RelativePaths.Product, productId), query.ToArray()));
        }

        public virtual Task<Product> GetProductByCodeAsync(string code, ItemResponseGroups responseGroup)
        {
            var query = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("code", code),
                 new KeyValuePair<string, string>("responseGroup", responseGroup.GetHashCode().ToString()),
            };

            return GetAsync<Product>((CreateRequestUri(RelativePaths.Products, query.ToArray())));
        }

        public virtual Task<ResponseCollection<Category>> GetCategoriesAsync(string parentId = null)
        {
            return GetAsync<ResponseCollection<Category>>(CreateRequestUri(RelativePaths.Categories, string.Format("parentId={0}", parentId)));
        }

        public virtual Task<Category> GetCategoryByCodeAsync(string code)
        {
            return GetAsync<Category>(CreateRequestUri(RelativePaths.Categories, "code=" + code));
        }

        public virtual Task<Category> GetCategoryAsync(string categoryId)
        {
            return GetAsync<Category>(CreateRequestUri(String.Format(RelativePaths.Category, categoryId)));
        }
    }

    /*
    public class BrowseCachedClient : BrowseClient
    {
        #region Private Variables
        private readonly bool _isEnabled;
        private readonly ICacheRepository _cacheRepository = new HttpCacheRepository();
        #endregion

        public BrowseCachedClient(Uri adminBaseEndpoint, string token)
            : base(adminBaseEndpoint, token)
        {
        }

        public BrowseCachedClient(Uri adminBaseEndpoint, MessageProcessingHandler handler)
            : base(adminBaseEndpoint, handler)
        {
        }

        protected override Task<T> GetAsync<T>(Uri requestUri, string userId = null)
        {
            // TODO: vary cache timeout based on url requested, since we know the exact URI for each resource
            return Helper.GetAsync(requestUri.ToString(),
                () => base.GetAsync<T>(requestUri, userId),
                CatalogConfiguration.Instance.Cache.ItemTimeout,
                true);
        }

    }
     * */
}
