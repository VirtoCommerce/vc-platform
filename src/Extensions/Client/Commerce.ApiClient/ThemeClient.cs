#region

using System;
using System.Net.Http;
using System.Threading.Tasks;
using VirtoCommerce.ApiClient.DataContracts.Themes;
using VirtoCommerce.ApiClient.Utilities;

#endregion

namespace VirtoCommerce.ApiClient
{
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

        #region Public Methods and Operators

        public async Task<ThemeAsset[]> GetThemeAssetsAsync(string storeId, string themeId, DateTime since, bool loadContent = false)
        {
            var parameters = new { LastUpdateDate = since, loadContent };
            return await
                GetAsync<ThemeAsset[]>(
                    CreateRequestUri(
                        String.Format(RelativePaths.ThemeAssets, storeId, themeId), parameters));
        }

        public async Task<Theme[]> GetThemesAsync(string storeId)
        {
            return await
                GetAsync<Theme[]>(
                    CreateRequestUri(
                        String.Format(RelativePaths.Themes, storeId))).ConfigureAwait(false);
        }

        #endregion

        protected class RelativePaths
        {
            #region Constants

            public const string ThemeAssets = "cms/{0}/themes/{1}/assets";
            public const string Themes = "cms/{0}/themes";

            #endregion
        }
    }
}
