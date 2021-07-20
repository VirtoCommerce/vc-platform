using System;
using VirtoCommerce.Platform.Core.Caching;

namespace VirtoCommerce.Platform.Data.Assets
{
    [Obsolete("Use AssetCacheRegion from VirtoCommerce.AssetsModule.Data instead")]
    public class AssetCacheRegion : CancellableCacheRegion<AssetCacheRegion>
    {
    }
}
