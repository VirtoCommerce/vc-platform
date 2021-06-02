using VirtoCommerce.Platform.Core.Caching;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.Platform.Caching.GenericCrud
{
    public class GenericSearchCacheRegion<T> : CancellableCacheRegion<GenericSearchCacheRegion<T>> where T : Entity
    {
    }
}
