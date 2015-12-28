using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DotLiquid;

namespace VirtoCommerce.LiquidThemeEngine.Objects
{
    public class ItemCollection<T> : Drop, IEnumerable<T>, ICollection, ILiquidContains
        where T : class
    {
        public ItemCollection(IEnumerable<T> collection)
        {
            Root = (collection ?? Enumerable.Empty<T>()).ToList();
        }

        public IList<T> Root { get; set; }

        public object SyncRoot { get { return Root; } }

        public int Size { get { return Root.Count; } }

        public bool IsSynchronized { get { return true; } }


        public int Count { get { return TotalCount; } }

        public virtual int TotalCount { get; set; }


        public static implicit operator ItemCollection<T>(string[] a)
        {
            return new ItemCollection<T>(a as IEnumerable<T>);
        }

        public void CopyTo(Array array, int index)
        {
        }

        public IEnumerator<T> GetEnumerator()
        {
            return Root.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return Root.GetEnumerator();
        }

        public bool Contains(object value)
        {
            return Root.Contains(value as T);
        }
    }
}
