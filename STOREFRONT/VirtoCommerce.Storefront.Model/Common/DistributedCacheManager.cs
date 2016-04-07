using CacheManager.Core;

namespace VirtoCommerce.Storefront.Model.Common
{
    public class DistributedCacheManager : CacheManagerBase, IDistributedCacheManager
    {
        public DistributedCacheManager(ICacheManager<object> cacheManager)
            : base(cacheManager)
        {
        }
    }
}
