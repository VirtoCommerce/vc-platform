using VirtoCommerce.Platform.Core.Caching;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.Platform.Caching
{
    /// <summary>
    /// Generic CRUD search cache region implementation for use with crud/search services.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class GenericSearchCachingRegion<T> : CancellableCacheRegion<GenericSearchCachingRegion<T>> where T : Entity
    {
    }
}
