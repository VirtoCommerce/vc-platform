using System;
using System.Collections;
using System.Collections.Generic;
using System.Web.Caching;
using Microsoft.Practices.ServiceLocation;
using VirtoCommerce.Foundation.Frameworks;

namespace VirtoCommerce.Web.Client.Caching
{
    public class MemoryCacheProvider : OutputCacheProvider, IEnumerable<KeyValuePair<string, object>>
    {
        private ICacheRepository _repository;

        private ICacheRepository Cache
        {
            get { return _repository ?? (_repository = ServiceLocator.Current.GetInstance<ICacheRepository>()); }
        }
        //private static readonly ObjectCache Cache = MemoryCache.Default;

        public override object Add(string key, object entry, DateTime utcExpiry)
        {
            //return Cache.AddOrGetExisting(key, value, null, utcExpiry);
            Cache.Add(key, entry, utcExpiry - DateTime.UtcNow);
            return entry;
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
            //Cache.Set(key, entry, utcExpiry);
            Cache.Add(key, entry, utcExpiry - DateTime.UtcNow);
        }

        public IEnumerator<KeyValuePair<string, object>> GetEnumerator()
        {
            var enumerator = Cache.GetEnumerator();
            while (enumerator.MoveNext())
            {
                yield return new KeyValuePair<string, object>(enumerator.Key.ToString(), enumerator.Value);
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}