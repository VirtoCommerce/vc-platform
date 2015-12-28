using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtoCommerce.Storefront.Model.Common
{
    /// <summary>
    /// Represent dictionary  returning a default value if the key does not exist 
    /// </summary>
    public class DefaultableDictionary : IDictionary
    {
        private readonly IDictionary _dictionary;
        private readonly object _defaultValue;

        public DefaultableDictionary(Dictionary<string, object> dictionary, object defaultValue)
        {
            _dictionary = dictionary;
            _defaultValue = defaultValue;
        }

        public DefaultableDictionary(object defaultValue)
            :this(new Dictionary<string, object>(), defaultValue)
        {
        }

        #region IDictionary members
        public ICollection Keys
        {
            get
            {
                return _dictionary.Keys;
            }
        }

        public ICollection Values
        {
            get
            {
                return _dictionary.Values;
            }
        }

        public bool IsReadOnly
        {
            get
            {
                return _dictionary.IsReadOnly;
            }
        }

        public bool IsFixedSize
        {
            get
            {
                return _dictionary.IsFixedSize;
            }
        }

        public int Count
        {
            get
            {
                return _dictionary.Count;
            }
        }

        public object SyncRoot
        {
            get
            {
                return _dictionary.SyncRoot;
            }
        }

        public bool IsSynchronized
        {
            get
            {
                return _dictionary.IsSynchronized;
            }
        }

        public object this[object key]
        {
            get
            {
                object retVal;
                if (!((Dictionary<string, object>)_dictionary).TryGetValue(key.ToString(), out retVal))
                {
                    retVal = _defaultValue;
                }
                return retVal;
            }


            set { _dictionary[key] = value; }
        }



        public bool Contains(object key)
        {
            return true;
        }

        public void Add(object key, object value)
        {
            _dictionary.Add(key, value);
        }

        public void Clear()
        {
            _dictionary.Clear();
        }

        public IDictionaryEnumerator GetEnumerator()
        {
            return _dictionary.GetEnumerator();
        }

        public void Remove(object key)
        {
            _dictionary.Remove(key);
        }

        public void CopyTo(Array array, int index)
        {
            _dictionary.CopyTo(array, index);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _dictionary.GetEnumerator();
        }

        #endregion
    }
}
