#region
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using DotLiquid;

#endregion

namespace VirtoCommerce.Web.Models
{

    #region
    #endregion

    [DataContract]
    public class ItemCollection<T> : Drop, IEnumerable<T>, ICollection //, IVirtualCollection
    {
        #region Fields
        private readonly IEnumerable<T> _Collection;
        #endregion

        #region Constructors and Destructors
        public ItemCollection(IEnumerable<T> collections)
        {
            this._Collection = collections;
        }
        #endregion

        #region Public Properties
        public int Count
        {
            get
            {
                return this.TotalCount;
            }
        }

        public bool IsSynchronized
        {
            get
            {
                return true;
            }
        }

        public IEnumerable<T> Root
        {
            get
            {
                return this._Collection;
            }
        }

        public int Size
        {
            get
            {
                return this._Collection.Count();
            }
        }

        public object SyncRoot
        {
            get
            {
                return this.Root;
            }
        }

        public int TotalCount { get; set; }
        #endregion

        #region Public Methods and Operators
        public static implicit operator ItemCollection<T>(string[] a)
        {
            return new ItemCollection<T>(a as IEnumerable<T>);
        }

        public void CopyTo(Array array, int index)
        {
        }

        public IEnumerator<T> GetEnumerator()
        {
            return this._Collection.GetEnumerator();
        }
        #endregion

        #region Explicit Interface Methods
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this._Collection.GetEnumerator();
        }
        #endregion
    }
}