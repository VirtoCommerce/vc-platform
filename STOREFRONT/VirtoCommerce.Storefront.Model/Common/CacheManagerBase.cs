using System;
using System.Collections.Generic;
using CacheManager.Core;
using CacheManager.Core.Internal;

namespace VirtoCommerce.Storefront.Model.Common
{
    public class CacheManagerBase : ICacheManager<object>
    {
        private readonly ICacheManager<object> _cacheManager;

        public CacheManagerBase(ICacheManager<object> cacheManager)
        {
            _cacheManager = cacheManager;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _cacheManager.Dispose();
            }
        }

        public bool Add(string key, object value)
        {
            return _cacheManager.Add(key, value);
        }

        public bool Add(string key, object value, string region)
        {
            return _cacheManager.Add(key, value, region);
        }

        public bool Add(CacheItem<object> item)
        {
            return _cacheManager.Add(item);
        }

        public void Clear()
        {
            _cacheManager.Clear();
        }

        public void ClearRegion(string region)
        {
            _cacheManager.ClearRegion(region);
        }

        public void Expire(string key, ExpirationMode mode, TimeSpan timeout)
        {
            _cacheManager.Expire(key, mode, timeout);
        }

        public void Expire(string key, string region, ExpirationMode mode, TimeSpan timeout)
        {
            _cacheManager.Expire(key, region, mode, timeout);
        }

        public void Expire(string key, DateTimeOffset absoluteExpiration)
        {
            _cacheManager.Expire(key, absoluteExpiration);
        }

        public void Expire(string key, string region, DateTimeOffset absoluteExpiration)
        {
            _cacheManager.Expire(key, region, absoluteExpiration);
        }

        public void Expire(string key, TimeSpan slidingExpiration)
        {
            _cacheManager.Expire(key, slidingExpiration);
        }

        public void Expire(string key, string region, TimeSpan slidingExpiration)
        {
            _cacheManager.Expire(key, region, slidingExpiration);
        }

        public object Get(string key)
        {
            return _cacheManager.Get(key);
        }

        public object Get(string key, string region)
        {
            return _cacheManager.Get(key, region);
        }

        public TOut Get<TOut>(string key)
        {
            return _cacheManager.Get<TOut>(key);
        }

        public TOut Get<TOut>(string key, string region)
        {
            return _cacheManager.Get<TOut>(key, region);
        }

        public CacheItem<object> GetCacheItem(string key)
        {
            return _cacheManager.GetCacheItem(key);
        }

        public CacheItem<object> GetCacheItem(string key, string region)
        {
            return _cacheManager.GetCacheItem(key, region);
        }

        public void Put(string key, object value)
        {
            _cacheManager.Put(key, value);
        }

        public void Put(string key, object value, string region)
        {
            _cacheManager.Put(key, value, region);
        }

        public void Put(CacheItem<object> item)
        {
            _cacheManager.Put(item);
        }

        public bool Remove(string key)
        {
            return _cacheManager.Remove(key);
        }

        public bool Remove(string key, string region)
        {
            return _cacheManager.Remove(key, region);
        }

        public void RemoveExpiration(string key)
        {
            _cacheManager.RemoveExpiration(key);
        }

        public void RemoveExpiration(string key, string region)
        {
            _cacheManager.RemoveExpiration(key, region);
        }

        object ICache<object>.this[string key]
        {
            get { return _cacheManager[key]; }
            set { _cacheManager[key] = value; }
        }

        object ICache<object>.this[string key, string region]
        {
            get { return _cacheManager[key, region]; }
            set { _cacheManager[key, region] = value; }
        }

        public object AddOrUpdate(string key, object addValue, Func<object, object> updateValue)
        {
            return _cacheManager.AddOrUpdate(key, addValue, updateValue);
        }

        public object AddOrUpdate(string key, string region, object addValue, Func<object, object> updateValue)
        {
            return _cacheManager.AddOrUpdate(key, region, addValue, updateValue);
        }

        public object AddOrUpdate(string key, object addValue, Func<object, object> updateValue, int maxRetries)
        {
            return _cacheManager.AddOrUpdate(key, addValue, updateValue, maxRetries);
        }

        public object AddOrUpdate(string key, string region, object addValue, Func<object, object> updateValue, int maxRetries)
        {
            return _cacheManager.AddOrUpdate(key, region, addValue, updateValue, maxRetries);
        }

        public object AddOrUpdate(CacheItem<object> addItem, Func<object, object> updateValue)
        {
            return _cacheManager.AddOrUpdate(addItem, updateValue);
        }

        public object AddOrUpdate(CacheItem<object> addItem, Func<object, object> updateValue, int maxRetries)
        {
            return _cacheManager.AddOrUpdate(addItem, updateValue, maxRetries);
        }

        public object Update(string key, Func<object, object> updateValue)
        {
            return _cacheManager.Update(key, updateValue);
        }

        public object Update(string key, string region, Func<object, object> updateValue)
        {
            return _cacheManager.Update(key, region, updateValue);
        }

        public object Update(string key, Func<object, object> updateValue, int maxRetries)
        {
            return _cacheManager.Update(key, updateValue, maxRetries);
        }

        public object Update(string key, string region, Func<object, object> updateValue, int maxRetries)
        {
            return _cacheManager.Update(key, region, updateValue, maxRetries);
        }

        public bool TryUpdate(string key, Func<object, object> updateValue, out object value)
        {
            return _cacheManager.TryUpdate(key, updateValue, out value);
        }

        public bool TryUpdate(string key, string region, Func<object, object> updateValue, out object value)
        {
            return _cacheManager.TryUpdate(key, region, updateValue, out value);
        }

        public bool TryUpdate(string key, Func<object, object> updateValue, int maxRetries, out object value)
        {
            return _cacheManager.TryUpdate(key, updateValue, maxRetries, out value);
        }

        public bool TryUpdate(string key, string region, Func<object, object> updateValue, int maxRetries, out object value)
        {
            return _cacheManager.TryUpdate(key, region, updateValue, maxRetries, out value);
        }

        public IReadOnlyCacheManagerConfiguration Configuration
        {
            get { return _cacheManager.Configuration; }
        }

        public string Name
        {
            get { return _cacheManager.Name; }
        }

        public IEnumerable<BaseCacheHandle<object>> CacheHandles
        {
            get { return _cacheManager.CacheHandles; }
        }

        public event EventHandler<CacheActionEventArgs> OnAdd
        {
            add { _cacheManager.OnAdd += value; }
            remove { _cacheManager.OnAdd -= value; }
        }

        public event EventHandler<CacheClearEventArgs> OnClear
        {
            add { _cacheManager.OnClear += value; }
            remove { _cacheManager.OnClear -= value; }
        }

        public event EventHandler<CacheClearRegionEventArgs> OnClearRegion
        {
            add { _cacheManager.OnClearRegion += value; }
            remove { _cacheManager.OnClearRegion -= value; }
        }

        public event EventHandler<CacheActionEventArgs> OnGet
        {
            add { _cacheManager.OnGet += value; }
            remove { _cacheManager.OnGet -= value; }
        }

        public event EventHandler<CacheActionEventArgs> OnPut
        {
            add { _cacheManager.OnPut += value; }
            remove { _cacheManager.OnPut -= value; }
        }

        public event EventHandler<CacheActionEventArgs> OnRemove
        {
            add { _cacheManager.OnRemove += value; }
            remove { _cacheManager.OnRemove -= value; }
        }

        public event EventHandler<CacheUpdateEventArgs<object>> OnUpdate
        {
            add { _cacheManager.OnUpdate += value; }
            remove { _cacheManager.OnUpdate -= value; }
        }
    }
}
