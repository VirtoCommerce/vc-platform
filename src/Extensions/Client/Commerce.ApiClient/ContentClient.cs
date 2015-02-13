namespace VirtoCommerce.ApiClient
{
    #region

    using System;
    using System.Net.Http;
    using System.Threading.Tasks;

    using VirtoCommerce.ApiClient.DataContracts.Contents;
    using VirtoCommerce.ApiClient.Extensions;
    using VirtoCommerce.ApiClient.Utilities;
    using VirtoCommerce.Web.Core.DataContracts;

    #endregion

    public class ContentClient : BaseClient
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the AdminManagementClient class.
        /// </summary>
        /// <param name="adminBaseEndpoint">Admin endpoint</param>
        /// <param name="token">Access token</param>
        public ContentClient(Uri adminBaseEndpoint, string token)
            : base(adminBaseEndpoint, new TokenMessageProcessingHandler(token))
        {
        }

        /// <summary>
        ///     Initializes a new instance of the AdminManagementClient class.
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
        public Task<ResponseCollection<DynamicContentItemGroup>> GetDynamicContentAsync(
            string[] placeHolder,
            TagQuery query)
        {
            return
                this.GetAsync<ResponseCollection<DynamicContentItemGroup>>(
                    CreateRequestUri(
                        String.Format(RelativePaths.Contents, string.Join(",", placeHolder)),
                        query.GetQueryString()));
        }

        #endregion

        protected class RelativePaths
        {
            #region Constants

            public const string Contents = "contents/{0}";

            #endregion
        }
    }
}