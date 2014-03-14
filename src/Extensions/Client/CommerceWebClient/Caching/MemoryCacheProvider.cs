using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Caching;
using System.Web.Caching;

namespace VirtoCommerce.Web.Client.Caching
{
    public class MemoryCacheProvider : OutputCacheProvider, IEnumerable<KeyValuePair<string, object>>
    {
        private static readonly ObjectCache Cache = MemoryCache.Default;

        public override object Add(string key, object entry, DateTime utcExpiry)
        {
            return Cache.AddOrGetExisting(key, entry, utcExpiry);            
        }

        public override object Get(string key)
        {
            return Cache.Get(key);
        }

        public override void Remove(string key)
        {
            Cache.Remove(key);
        }

        public override void Set(string key, object entry, DateTime utcExpiry)
        {
            Cache.Set(key, entry, utcExpiry);
        }

        public IEnumerator<KeyValuePair<string, object>> GetEnumerator()
        {
            return ((IEnumerable<KeyValuePair<string, object>>) Cache).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}