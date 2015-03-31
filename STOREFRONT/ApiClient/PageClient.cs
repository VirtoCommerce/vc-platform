#region

using System;
using System.Net.Http;
using System.Threading.Tasks;
using VirtoCommerce.ApiClient.DataContracts.Pages;
using VirtoCommerce.ApiClient.DataContracts.Stores;
using VirtoCommerce.ApiClient.Utilities;

#endregion

namespace VirtoCommerce.ApiClient
{
    public class PageClient : BaseClient
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the StoreClient class.
        /// </summary>
        /// <param name="adminBaseEndpoint">Admin endpoint</param>
        /// <param name="appId">The API application ID.</param>
        /// <param name="secretKey">The API secret key.</param>
        public PageClient(Uri adminBaseEndpoint, string appId, string secretKey)
            : base(adminBaseEndpoint, new HmacMessageProcessingHandler(appId, secretKey))
        {
        }

        /// <summary>
        ///     Initializes a new instance of the StoreClient class.
        /// </summary>
        /// <param name="adminBaseEndpoint">Admin endpoint</param>
        /// <param name="handler"></param>
        public PageClient(Uri adminBaseEndpoint, MessageProcessingHandler handler)
            : base(adminBaseEndpoint, handler)
        {
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     Gets page
        /// </summary>
        public Task<Page[]> GetPagesAsync(string storeId, string language, string page)
        {
            return GetAsync<Page[]>(CreateRequestUri(String.Format(RelativePaths.Pages, storeId, language, page)));
        }

        public async Task<Page[]> GetPagesAsync(string storeId, DateTime since)
        {
            var parameters = new { LastUpdateDate = since };
            return await
                GetAsync<Page[]>(
                    CreateRequestUri(
                        String.Format(RelativePaths.PagesSince, storeId), parameters));
        }
        #endregion

        protected class RelativePaths
        {
            #region Constants

            public const string Pages = "cms/{0}/pages/{1}/{2}";
            public const string PagesSince = "cms/{0}/pages";

            #endregion
        }
    }
}
