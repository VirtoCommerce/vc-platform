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
        /// <param name="thumb">Is thumbnail</param>
        /// <returns>System.String.</returns>
        public string ResolveUrl(string assetId, bool thumb)
        {
            if (thumb)
            {
                if (!assetId.Contains(".thumb"))
                {
                    var extIdx = assetId.LastIndexOf(".", StringComparison.Ordinal);
                    if (extIdx != -1)
                    {
                        assetId = string.Format("{0}thumb{1}", assetId.Substring(0, extIdx + 1),
                            assetId.Substring(extIdx));
                    }
                }
            }
            else
            {
                assetId = assetId.Replace(".thumb", "");
            }
            return String.Format("{0}{1}{2}", "~/asset", "/", assetId);
        }
    }
}
