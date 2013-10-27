using System;
using VirtoCommerce.Foundation.Assets.Services;

namespace VirtoCommerce.Web.Client.Services.Assets
{
    /// <summary>
    /// Class FileSystemAssetUrl.
    /// </summary>
    public class FileSystemAssetUrl : IAssetUrl
    {
        /// <summary>
        /// Resolves the URL.
        /// </summary>
        /// <param name="assetId">The asset identifier.</param>
        /// <returns>System.String.</returns>
        public string ResolveUrl(string assetId)
        {
            return String.Format("{0}{1}{2}", "~/asset", "/", assetId);
        }
    }
}
