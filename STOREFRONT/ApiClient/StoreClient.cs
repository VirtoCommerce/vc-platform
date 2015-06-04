#region

using System;
using System.Net.Http;
using System.Threading.Tasks;
using VirtoCommerce.ApiClient.DataContracts.Stores;
using VirtoCommerce.ApiClient.Utilities;

#endregion

namespace VirtoCommerce.ApiClient
{
    public class StoreClient : BaseClient
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the StoreClient class.
        /// </summary>
        /// <param name="adminBaseEndpoint">Admin endpoint</param>
        /// <param name="appId">The API application ID.</param>
        /// <param name="secretKey">The API secret key.</param>
        public StoreClient(Uri adminBaseEndpoint, string appId, string secretKey)
            : base(adminBaseEndpoint, new HmacMessageProcessingHandler(appId, secretKey))
        {
        }

        /// <summary>
        ///     Initializes a new instance of the StoreClient class.
        /// </summary>
        /// <param name="adminBaseEndpoint">Admin endpoint</param>
        /// <param name="handler"></param>
        public StoreClient(Uri adminBaseEndpoint, MessageProcessingHandler handler)
            : base(adminBaseEndpoint, handler)
        {
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     List items matching the given query
        /// </summary>
        public Task<Store[]> GetStoresAsync()
        {
            return GetAsync<Store[]>(CreateRequestUri(RelativePaths.Stores));
        }

        public async Task<SyncAssetGroup[]> GetStoreAssetsAsync(string storeId, string theme, DateTime? themeUpdated, DateTime? pagesUpdated)
        {
            var parameters = new{ themeUpdated, pagesUpdated, theme };
            return await
                GetAsync<SyncAssetGroup[]>(
                    CreateRequestUri(
                        String.Format(RelativePaths.StoreAssets, storeId), parameters));
        }

        #endregion

        protected class RelativePaths
        {
            #region Constants

            public const string Stores = "mp/stores";
            public const string StoreAssets = "cms/sync/stores/{0}/assets";
            #endregion
        }
    }
}
