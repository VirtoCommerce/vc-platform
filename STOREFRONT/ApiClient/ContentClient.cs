using System;
using System.Net.Http;
using System.Threading.Tasks;
using VirtoCommerce.ApiClient.DataContracts;
using VirtoCommerce.ApiClient.DataContracts.Contents;
using VirtoCommerce.ApiClient.DataContracts.Marketing;
using VirtoCommerce.ApiClient.Extensions;
using VirtoCommerce.ApiClient.Utilities;

namespace VirtoCommerce.ApiClient
{

    #region

    #endregion

    public class ContentClient : BaseClient
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the ContentClient class.
        /// </summary>
        /// <param name="adminBaseEndpoint">Admin endpoint</param>
        /// <param name="appId">The API application ID.</param>
        /// <param name="secretKey">The API secret key.</param>
        public ContentClient(Uri adminBaseEndpoint, string appId, string secretKey)
            : base(adminBaseEndpoint, new HmacMessageProcessingHandler(appId, secretKey))
        {
        }

        /// <summary>
        ///     Initializes a new instance of the ContentClient class.
        /// </summary>
        /// <param name="adminBaseEndpoint">Admin endpoint</param>
        /// <param name="handler"></param>
        public ContentClient(Uri adminBaseEndpoint, MessageProcessingHandler handler)
            : base(adminBaseEndpoint, handler)
        {
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     List items matching the given query
        /// </summary>
        public Task<DynamicContentItem[]> GetDynamicContentAsync(DynamicContentEvaluationContext context)
        {
            return SendAsync<DynamicContentEvaluationContext, DynamicContentItem[]>(
                CreateRequestUri(RelativePaths.Contents), HttpMethod.Post, context);
        }

        #endregion

        protected class RelativePaths
        {
            #region Constants

            public const string Contents = "contentitems/evaluate";

            #endregion
        }
    }
}
