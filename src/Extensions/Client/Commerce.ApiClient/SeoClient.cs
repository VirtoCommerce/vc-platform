using System;
using System.Net.Http;
using System.Threading.Tasks;
using VirtoCommerce.ApiClient.Extensions;
using VirtoCommerce.ApiClient.Utilities;
using VirtoCommerce.Web.Core.DataContracts;

namespace VirtoCommerce.ApiClient
{
    public class SeoClient : BaseClient
    {
        protected class RelativePaths
        {
            public const string Keywords = "keywords";
        }

        /// <summary>
        /// Initializes a new instance of the AdminManagementClient class.
        /// </summary>
        /// <param name="adminBaseEndpoint">Admin endpoint</param>
        /// <param name="token">Access token</param>
        public SeoClient(Uri adminBaseEndpoint, string token)
            : base(adminBaseEndpoint, new TokenMessageProcessingHandler(token))
        {
        }

        /// <summary>
        /// Initializes a new instance of the AdminManagementClient class.
        /// </summary>
        /// <param name="adminBaseEndpoint">Admin endpoint</param>
        /// <param name="handler"></param>
        public SeoClient(Uri adminBaseEndpoint, MessageProcessingHandler handler)
            : base(adminBaseEndpoint, handler)
        {

        }

        /// <summary>
        /// List items matching the given query
        /// </summary>
        public Task<SeoKeyword[]> GetKeywordsAsync()
        {
            return GetAsync<SeoKeyword[]>(CreateRequestUri(RelativePaths.Keywords));
        }
    }
}
