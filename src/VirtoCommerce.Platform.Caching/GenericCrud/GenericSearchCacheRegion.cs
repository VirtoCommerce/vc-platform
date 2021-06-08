using VirtoCommerce.Platform.Core.Caching;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.Platform.Caching.GenericCrud
{
    /// <summary>
    /// Generic CRUD search cache region implementation for use with generic crud/search services.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class GenericSearchCacheRegion<T> : CancellableCacheRegion<GenericSearchCacheRegion<T>> where T : Entity
    {
    }
}
