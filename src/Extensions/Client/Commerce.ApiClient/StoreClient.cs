namespace VirtoCommerce.ApiClient
{
    #region

    using System;
    using System.Net.Http;
    using System.Threading.Tasks;

    using VirtoCommerce.ApiClient.Utilities;
    using VirtoCommerce.Web.Core.DataContracts.Store;

    #endregion

    public class StoreClient : BaseClient
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the AdminManagementClient class.
        /// </summary>
        /// <param name="adminBaseEndpoint">Admin endpoint</param>
        /// <param name="token">Access token</param>
        public StoreClient(Uri adminBaseEndpoint, string token)
            : base(adminBaseEndpoint, new TokenMessageProcessingHandler(token))
        {
        }

        /// <summary>
        ///     Initializes a new instance of the AdminManagementClient class.
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
            return this.GetAsync<Store[]>(this.CreateRequestUri(RelativePaths.Stores));
        }

        #endregion

        protected class RelativePaths
        {
            #region Constants

            public const string Stores = "stores";

            #endregion
        }
    }
}