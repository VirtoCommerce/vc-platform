#region
using System;
using System.Net.Http;
using VirtoCommerce.ApiClient.Utilities;

#endregion

namespace VirtoCommerce.ApiClient
{
    #region
    
    #endregion

    public class ThemeClient : BaseClient
    {
        #region Constructors and Destructors
        /// <summary>
        ///     Initializes a new instance of the ThemeClient class.
        /// </summary>
        /// <param name="adminBaseEndpoint">Admin endpoint</param>
        /// <param name="appId">The API application ID.</param>
        /// <param name="secretKey">The API secret key.</param>
        public ThemeClient(Uri adminBaseEndpoint, string appId, string secretKey)
            : base(adminBaseEndpoint, new HmacMessageProcessingHandler(appId, secretKey))
        {
        }

        /// <summary>
        ///     Initializes a new instance of the ThemeClient class.
        /// </summary>
        /// <param name="adminBaseEndpoint">Admin endpoint</param>
        /// <param name="handler"></param>
        public ThemeClient(Uri adminBaseEndpoint, MessageProcessingHandler handler)
            : base(adminBaseEndpoint, handler)
        {
        }
        #endregion

        /*
        /// <summary>
        ///     List items matching the given query
        /// </summary>
        public Task<Theme[]> GetDynamicContentAsync(
            string[] placeHolder,
            TagQuery query)
        {
            return
                this.GetAsync<ResponseCollection<DynamicContentItemGroup>>(
                    CreateRequestUri(
                        String.Format(RelativePaths.Contents, string.Join(",", placeHolder)),
                        query.GetQueryString()));
        }
         * */

        protected class RelativePaths
        {
            #region Constants
            public const string Themes = "cms/{0}/themes";
            #endregion
        }
    }
}