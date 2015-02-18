#region

using System;
using System.Collections;

#endregion

namespace VirtoCommerce.ApiClient.Caching
{

    #region

    #endregion

    public interface ICacheRepository
    {
        #region Public Indexers

        object this[string key] { get; set; }

        #endregion

        #region Public Methods and Operators

        void Add(string key, object value);

        void Add(string key, object value, TimeSpan timeSpan);

        void Clear();

        object Get(string key);

        object GetAndLock(string key, TimeSpan timeSpan, out object lockHandle);

        IDictionaryEnumerator GetEnumerator();

        object PutAndUnlock(string key, object value, object lockHandle);

        bool Remove(string key);

        void Unlock(string key, object lockHandle);

        #endregion
    }
}
