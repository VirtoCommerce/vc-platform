using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VirtoCommerce.Foundation.Frameworks;
using Microsoft.ApplicationServer.Caching;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace VirtoCommerce.Caching.AppFabric
{
    public class AppFabricCacheRepository : ICacheRepository
    {
        private static DataCacheFactory _factory;
        private static DataCache _cache;

        private static DataCache GetCache()
        {
            if (_cache != null) return _cache;

            var configuration = new DataCacheFactoryConfiguration();
            _factory = new DataCacheFactory(configuration);
            _cache = _factory.GetCache("default");

            return _cache;
        }

        public void Add(string key, object value)
        {
            var cache = GetCache();
            cache.Add(key, Serialize(value));
        }

        public void Add(string key, object value, TimeSpan timeout)
        {
            var cache = GetCache();
            cache.Add(key, Serialize(value), timeout);
        }

        public object Get(string key)
        {
            var cache = GetCache();
            return Deserialize(cache.Get(key));
        }

        public object this[string key]
        {
            get { return this.Get(key); }
            set
            {
                var cache = GetCache();
                cache.Put(key, Serialize(value));
            }
        }

        public bool Remove(string key)
        {
            var cache = GetCache();
            return cache.Remove(key);
        }

        public void Clear()
        {
            var cache = GetCache();
            foreach (string region in cache.GetSystemRegions())
            {
                cache.ClearRegion(region);
            }
        }

        public IDictionaryEnumerator GetEnumerator()
        {
            throw new NotImplementedException();
        }


        public object GetAndLock(string key, TimeSpan timespan, out object lockHandle)
        {
            var cache = GetCache();
            DataCacheLockHandle handle;
            object val = Deserialize(cache.GetAndLock(key, timespan, out handle));
            lockHandle = handle;
            return val;
        }

        public object PutAndUnlock(string key, object value, object lockHandle)
        {
            var cache = GetCache();
            return cache.PutAndUnlock(key, Serialize(value), (DataCacheLockHandle)lockHandle);
        }

        public void Unlock(string key, object lockHandle)
        {
            var cache = GetCache();
            cache.Unlock(key, (DataCacheLockHandle)lockHandle);
        }

        private object Deserialize(object source)
        {
            if (source == null)
                return null;

            // Initialize a storage medium to hold the serialized object
            MemoryStream stream = new MemoryStream((byte[])source);

            // Construct a serialization formatter
            BinaryFormatter formatter = new BinaryFormatter();
            object target = formatter.Deserialize(stream);
            stream.Close();
            return target;        
        }

        private byte[] Serialize(object source)
        {
            if (source == null)
                return null;

            // Initialize a storage medium to hold the serialized object
            MemoryStream stream = new MemoryStream();

            // Construct a serialization formatter
            BinaryFormatter formatter = new BinaryFormatter();

            // Serialize the object graph into the memory stream
            formatter.Serialize(stream, source);
            
            byte[] target = stream.ToArray();

            // Cleanup
            stream.Close();

            return target;
        }
    }
}
