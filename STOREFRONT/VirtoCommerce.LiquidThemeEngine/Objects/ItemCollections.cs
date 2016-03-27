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
        private readonly IEnumerable<T> _superset;
        public ItemCollection(IEnumerable<T> superset)
        {
            _superset = superset ?? Enumerable.Empty<T>();
        }
        public long Size
        {
            get
            {
                return _superset.Count();
            }
        }
        #region ICollection members
        public object SyncRoot
        {
            get
            {
                return _superset;
            }
        }

        public bool IsSynchronized
        {
            get
            {
                return true;
            }
        }

        public int Count
        {
            get
            {
                return _superset.Count();
            }
        }

        public void CopyTo(Array array, int index)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region IEnumerable<T> Members
        public IEnumerator<T> GetEnumerator()
        {
            return _superset.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _superset.GetEnumerator();
        }
        #endregion

        #region ILiquidContains Members
        public virtual bool Contains(object value)
        {
            var other = value as T;
            if(other != null)
            {
                return _superset.Any(x => x.Equals(other));
            }
            return false;
        }
        #endregion 
    }
}
