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
    [DataContract]
    public class ItemCollection<T> : Drop, IEnumerable<T>, ICollection
    {
        #region Fields
        private readonly IEnumerable<T> _collection;
        #endregion

        #region Constructors and Destructors
        public ItemCollection(IEnumerable<T> collection)
        {
            this._collection = collection ?? Enumerable.Empty<T>();
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
                return this._collection;
            }
        }

        public int Size
        {
            get
            {
                return this.Root.Count();
            }
        }

        public object SyncRoot
        {
            get
            {
                return this.Root;
            }
        }

        public virtual int TotalCount { get; set; }
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
            return this.Root.GetEnumerator();
        }
        #endregion

        #region Explicit Interface Methods
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.Root.GetEnumerator();
        }
        #endregion
    }
}