using System;

namespace VirtoCommerce.Platform.Caching
{
    public class CachingOptions
    {
        public bool CacheEnabled { get; set; } = true;

        public TimeSpan? CacheAbsoluteExpiration { get; set; }
        public TimeSpan? CacheSlidingExpiration { get; set; } = TimeSpan.FromMinutes(5);
    }
}
