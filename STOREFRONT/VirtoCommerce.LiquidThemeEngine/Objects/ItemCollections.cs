using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DotLiquid;

namespace VirtoCommerce.LiquidThemeEngine.Objects
{
    public abstract class ItemCollection<T> : Drop, IEnumerable<T>, ICollection, ILiquidContains
        where T : class
    {
        private readonly ICollection<T> _collection;
        public ItemCollection(IEnumerable<T> collection)
        {
            _collection = (collection ?? Enumerable.Empty<T>()).ToList();
        }
        public long Size { get { return _collection.Count(); } }
        #region ICollection members
        public object SyncRoot { get { return _collection; } }

        public bool IsSynchronized { get { return true; } }

        public int Count { get { return _collection.Count; } }

        public void CopyTo(Array array, int index)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region IEnumerable<T> Members
        public IEnumerator<T> GetEnumerator()
        {
            return _collection.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _collection.GetEnumerator();
        }
        #endregion

        #region ILiquidContains Members
        public virtual bool Contains(object value)
        {
            var other = value as T;
            if(other != null)
            {
                return _collection.Any(x => x.Equals(other));
            }
            return false;
        }
        #endregion 
    }
}
