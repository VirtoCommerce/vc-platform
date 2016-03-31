using CacheManager.Core;

namespace VirtoCommerce.Storefront.Model.Common
{
    public class LocalCacheManager : CacheManagerBase, ILocalCacheManager
    {
        public LocalCacheManager(ICacheManager<object> cacheManager)
            : base(cacheManager)
        {
        }
    }
}
