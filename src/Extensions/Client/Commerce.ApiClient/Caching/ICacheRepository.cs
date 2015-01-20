using System;
using System.Collections;

namespace VirtoCommerce.ApiClient.Caching
{
    public interface ICacheRepository
    {
        void Add(string key, object value);
        void Add(string key, object value, TimeSpan timeSpan);
        object Get(string key);
        object GetAndLock(string key, TimeSpan timeSpan, out object lockHandle);
        object PutAndUnlock(string key, object value, object lockHandle);
        void Unlock(string key, object lockHandle);
        object this[string key] { get; set; }
        bool Remove(string key);
        void Clear();
        IDictionaryEnumerator GetEnumerator();
    }
}
