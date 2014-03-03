using System;
using DevTrends.MvcDonutCaching;

namespace VirtoCommerce.Web.Client.Extensions.Filters.Caching
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
    public class CustomDonutOutputCacheAttribute : DonutOutputCacheAttribute
    {
        public CustomDonutOutputCacheAttribute()
            : base(new CacheKeyBuilder())
        {
            
        }
    }
}
