namespace VirtoCommerce.ApiClient
{
    #region

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Http;
using System.Threading.Tasks;

using VirtoCommerce.ApiClient.DataContracts.Search;
using VirtoCommerce.ApiClient.Extensions;
using VirtoCommerce.ApiClient.Utilities;
    using VirtoCommerce.Web.Core.Configuration.Catalog;
using VirtoCommerce.Web.Core.DataContracts;

    #endregion

    public class BrowseClient : BaseClient
    {
        #region Constructors and Destructors

        /// <summary>
		/// Initializes a new instance of the BrowseClient class.
        /// </summary>
        /// <param name="adminBaseEndpoint">Admin endpoint</param>
		/// <param name="appId">The API application ID.</param>
		/// <param name="secretKey">The API secret key.</param>
		public BrowseClient(Uri adminBaseEndpoint, string appId, string secretKey)
			: base(adminBaseEndpoint, new HmacMessageProcessingHandler(appId, secretKey))
        {
        }

        /// <summary>
		/// Initializes a new instance of the BrowseClient class.
        /// </summary>
        /// <param name="adminBaseEndpoint">Admin endpoint</param>
        /// <param name="handler"></param>
        public BrowseClient(Uri adminBaseEndpoint, MessageProcessingHandler handler)
            : base(adminBaseEndpoint, handler)
        {
        }

        #endregion

        #region Public Methods and Operators

        public virtual Task<ResponseCollection<Category>> GetCategoriesAsync(string parentId = null)
        {
            return
                this.GetAsync<ResponseCollection<Category>>(
                    CreateRequestUri(RelativePaths.Categories, string.Format("parentId={0}", parentId)));
        }

        public virtual Task<Category> GetCategoryAsync(string categoryId)
        {
            return this.GetAsync<Category>(this.CreateRequestUri(String.Format(RelativePaths.Category, categoryId)));
        }

        public virtual Task<Category> GetCategoryByCodeAsync(string code)
        {
            return this.GetAsync<Category>(this.CreateRequestUri(RelativePaths.Categories, "code=" + code));
        }

        public virtual Task<Product> GetProductAsync(string productId, ItemResponseGroups responseGroup)
        {
            var query = new List<KeyValuePair<string, string>>
            {
                 new KeyValuePair<string, string>("responseGroup", responseGroup.GetHashCode().ToString(CultureInfo.InvariantCulture)),
            };
            
            return
                this.GetAsync<Product>(
                    CreateRequestUri(String.Format(RelativePaths.Product, productId), query.ToArray()));
        }

        public virtual Task<Product> GetProductByCodeAsync(string code, ItemResponseGroups responseGroup)
        {
            var query = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("code", code),
                 new KeyValuePair<string, string>("responseGroup", responseGroup.GetHashCode().ToString(CultureInfo.InvariantCulture)),
            };

            return this.GetAsync<Product>((CreateRequestUri(RelativePaths.Products, query.ToArray())));
        }

        /// <summary>
        ///     List items matching the given query
        /// </summary>
        public virtual Task<ProductSearchResult> GetProductsAsync(
            BrowseQuery query,
            ItemResponseGroups? responseGroup = null)
        {
            return
                this.GetAsync<ProductSearchResult>(
                    CreateRequestUri(RelativePaths.Products, query.GetQueryString(responseGroup)));
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

            public const string Categories = "categories";

            public const string Category = "categories/{0}";

            public const string Product = "products/{0}";

            public const string Products = "products";

            #endregion
        }
    }
}
