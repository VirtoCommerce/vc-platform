#region

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Http;
using System.Threading.Tasks;
using VirtoCommerce.ApiClient.Configuration.Catalog;
using VirtoCommerce.ApiClient.DataContracts;
using VirtoCommerce.ApiClient.DataContracts.Search;
using VirtoCommerce.ApiClient.Extensions;
using VirtoCommerce.ApiClient.Utilities;

#endregion

namespace VirtoCommerce.ApiClient
{
    public class BrowseClient : BaseClient
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the BrowseClient class.
        /// </summary>
        /// <param name="adminBaseEndpoint">Admin endpoint</param>
        /// <param name="appId">The API application ID.</param>
        /// <param name="secretKey">The API secret key.</param>
        public BrowseClient(Uri adminBaseEndpoint, string appId, string secretKey)
            : base(adminBaseEndpoint, new HmacMessageProcessingHandler(appId, secretKey))
        {
        }

        /// <summary>
        ///     Initializes a new instance of the BrowseClient class.
        /// </summary>
        /// <param name="adminBaseEndpoint">Admin endpoint</param>
        /// <param name="handler"></param>
        public BrowseClient(Uri adminBaseEndpoint, MessageProcessingHandler handler)
            : base(adminBaseEndpoint, handler)
        {
        }

        #endregion

        #region Public Methods and Operators

        public virtual Task<ResponseCollection<Category>> GetCategoriesAsync(string store, string language, string parentId = null)
        {
            var parameters = new { store, language, parentId };
            return
                GetAsync<ResponseCollection<Category>>(
                    CreateRequestUri(RelativePaths.Categories, parameters));
        }

        public virtual Task<Category> GetCategoryAsync(string store, string language, string categoryId)
        {
            var parameters = new { store, language};
            return GetAsync<Category>(CreateRequestUri(String.Format(RelativePaths.Category, categoryId), parameters));
        }

        public virtual Task<Category> GetCategoryByCodeAsync(string store, string language, string code)
        {
            var parameters = new { store, language, code };
            return GetAsync<Category>(CreateRequestUri(RelativePaths.Categories, parameters));
        }

        public virtual Task<Category> GetCategoryByKeywordAsync(string store, string language, string keyword)
        {
            var parameters = new { store, language, keyword };
            return GetAsync<Category>(CreateRequestUri(RelativePaths.Categories, parameters));
        }

        public Task<Product> GetProductAsync(string store, string language, string productId)
		{
            var parameters = new { store, language};
			return GetAsync<Product>(CreateRequestUri(String.Format(RelativePaths.Product, productId), parameters));
		}

        public virtual Task<Product> GetProductAsync(string store, string language, string productId, ItemResponseGroups responseGroup)
        {
            var parameters = new { store, language, responseGroup = responseGroup.GetHashCode().ToString(CultureInfo.InvariantCulture) };

            return
                GetAsync<Product>(
                    CreateRequestUri(String.Format(RelativePaths.Product, productId), parameters));
        }

        public Task<Product> GetProductByCodeAsync(string store, string language, string code)
		{
            var parameters = new { store, language, code };
			return GetAsync<Product>((CreateRequestUri(RelativePaths.Products, parameters)));
		}

        public virtual Task<Product> GetProductByKeywordAsync(string store, string language, string keyword)
        {
            var parameters = new { store, language, keyword };
            return GetAsync<Product>(CreateRequestUri(RelativePaths.Products, parameters));
        }

        public virtual Task<Product> GetProductByKeywordAsync(string store, string language, string keyword, ItemResponseGroups responseGroup)
        {
            var parameters = new
            {
                store,
                language,
                keyword,
                responseGroup = responseGroup.GetHashCode()
                    .ToString(CultureInfo.InvariantCulture)
            };

            return GetAsync<Product>((CreateRequestUri(RelativePaths.Products, parameters)));
        }

        public virtual Task<Product> GetProductByCodeAsync(string store, string language, string code, ItemResponseGroups responseGroup)
        {
            var parameters = new
            {
                store,
                language,
                code,
                responseGroup = responseGroup.GetHashCode()
                    .ToString(CultureInfo.InvariantCulture)
            };

            return GetAsync<Product>((CreateRequestUri(RelativePaths.Products, parameters)));
        }

        /// <summary>
        ///     List items matching the given query
        /// </summary>
        public virtual Task<ProductSearchResult> GetProductsAsync(string store, string language, 
            BrowseQuery query,
            ItemResponseGroups? responseGroup = null)
        {
            var parameters = new
            {
                store,
                language,
                responseGroup = responseGroup.GetHashCode()
                    .ToString(CultureInfo.InvariantCulture)
            };
            return
                GetAsync<ProductSearchResult>(
                    CreateRequestUri(RelativePaths.Products, query.GetQueryString(parameters)));
        }

        #endregion

        #region Methods

        protected override TimeSpan GetCacheTimeOut(string requestUrl)
        {
            if (requestUrl.Contains(RelativePaths.Categories))
            {
                return CatalogConfiguration.Instance.Cache.CategoryCollectionTimeout;
            }

            return base.GetCacheTimeOut(requestUrl);
        }

        #endregion

        protected class RelativePaths
        {
            #region Constants

            public const string Categories = "mp/categories";

            public const string Category = "mp/categories/{0}";

            public const string Product = "mp/products/{0}";

            public const string Products = "mp/products";

            #endregion
        }
    }
}
