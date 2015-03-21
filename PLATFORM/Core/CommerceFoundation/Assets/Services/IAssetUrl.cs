using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtoCommerce.Foundation.Assets.Services
{
    /// <summary>
    /// Returns URL for the asset
    /// </summary>
    public interface IAssetUrl
    {
        string ResolveUrl(string assetId, bool thumb = false);
    }
}
