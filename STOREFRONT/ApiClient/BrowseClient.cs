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

        public virtual async Task<ResponseCollection<Category>> GetCategoriesAsync(
            string store,
            string language,
            string parentId = null)
        {
            var parameters = new { store, language, parentId };
            return
                await GetAsync<ResponseCollection<Category>>(
                    CreateRequestUri(RelativePaths.CategorySearch, parameters)).ConfigureAwait(false);
        }

        public virtual async Task<Category> GetCategoryAsync(string store, string language, string categoryId)
        {
            var parameters = new { store, language };
            return
                await
                    GetAsync<Category>(
                        CreateRequestUri(String.Format(RelativePaths.Category, categoryId), parameters))
                        .ConfigureAwait(false);
        }

        public virtual async Task<Category> GetCategoryByCodeAsync(string store, string language, string code)
        {
            var parameters = new { store, language, code };
            return
                await
                    GetAsync<Category>(CreateRequestUri(RelativePaths.Categories, parameters))
                        .ConfigureAwait(false);
        }

        public virtual async Task<Category> GetCategoryByKeywordAsync(string store, string language, string keyword)
        {
            var parameters = new { store, language, keyword };
            return
                await
                    GetAsync<Category>(CreateRequestUri(RelativePaths.Categories, parameters))
                        .ConfigureAwait(false);
        }

        public virtual async Task<Product> GetProductAsync(string store, string language, string productId)
        {
            var parameters = new { store, language };
            return
                await
                    GetAsync<Product>(
                        CreateRequestUri(String.Format(RelativePaths.Product, productId), parameters))
                        .ConfigureAwait(false);
        }

        public virtual async Task<Product> GetProductAsync(
            string store,
            string language,
            string productId,
            ItemResponseGroups responseGroup)
        {
            var parameters =
                new
                {
                    store,
                    language,
                    responseGroup = responseGroup.GetHashCode().ToString(CultureInfo.InvariantCulture)
                };

            return await
                GetAsync<Product>(
                    CreateRequestUri(String.Format(RelativePaths.Product, productId), parameters)).ConfigureAwait(false);
        }

        public virtual async Task<Product> GetProductByCodeAsync(string store, string language, string code)
        {
            var parameters = new { store, language, code };
            return
                await
                    GetAsync<Product>((CreateRequestUri(RelativePaths.Products, parameters))).ConfigureAwait(false);
        }

        public virtual async Task<Product> GetProductByCodeAsync(
            string store,
            string language,
            string code,
            ItemResponseGroups responseGroup)
        {
            var parameters = new
            {
                store,
                language,
                code,
                responseGroup = responseGroup.GetHashCode()
                    .ToString(CultureInfo.InvariantCulture)
            };

            return
                await
                    GetAsync<Product>((CreateRequestUri(RelativePaths.Products, parameters))).ConfigureAwait(false);
        }

        public virtual async Task<Product> GetProductByKeywordAsync(string store, string language, string keyword)
        {
            var parameters = new { store, language, keyword };
            return
                await GetAsync<Product>(CreateRequestUri(RelativePaths.Products, parameters)).ConfigureAwait(false);
        }

        public virtual async Task<Product> GetProductByKeywordAsync(
            string store,
            string language,
            string keyword,
            ItemResponseGroups responseGroup)
        {
            var parameters = new
            {
                store,
                language,
                keyword,
                responseGroup = responseGroup.GetHashCode()
                    .ToString(CultureInfo.InvariantCulture)
            };

            return
                await
                    GetAsync<Product>((CreateRequestUri(RelativePaths.Products, parameters))).ConfigureAwait(false);
        }

        /// <summary>
        ///     List items matching the given query
        /// </summary>
        public virtual async Task<ProductSearchResult> GetProductsAsync(
            string store,
            string language,
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
                await GetAsync<ProductSearchResult>(
                    CreateRequestUri(RelativePaths.ProductsSearch, query.GetQueryString(parameters))).ConfigureAwait(false);
        }

        public virtual async Task<IEnumerable<CatalogItem>> GetCatalogItemsByIdsAsync(IEnumerable<string> catalogItemsIds, string storeId, string responseGroup)
        {
            var ids = new List<string>();
            foreach (var catalogItemId in catalogItemsIds)
            {
                ids.Add(string.Format("ids={0}", catalogItemId));
            }

            var queryString = string.Join("&", ids);
            queryString += "&store=" + storeId;
            queryString += "&responseGroup=" + responseGroup;

            return await GetAsync<IEnumerable<CatalogItem>>(CreateRequestUri(RelativePaths.Products, queryString)).ConfigureAwait(false);
        }

        #endregion

        protected class RelativePaths
        {
            #region Constants

            public const string Categories = "mp/categories";

            public const string Category = "mp/categories/{0}";

            public const string CategorySearch = "mp/categories/search";

            public const string Product = "mp/products/{0}";

            public const string Products = "mp/products";

            public const string ProductsSearch = "mp/products/search";

            #endregion
        }
    }
}
