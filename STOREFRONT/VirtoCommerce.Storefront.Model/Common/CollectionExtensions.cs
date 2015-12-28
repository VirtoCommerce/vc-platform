using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace VirtoCommerce.Platform.Core.Common
{
    /// <summary>
    /// Class that provides extension methods to Collection
    /// </summary>
    public static class CollectionExtensions
    {
        /// <summary>
        /// Add a range of items to a collection.
        /// </summary>
        /// <typeparam name="T">Type of objects within the collection.</typeparam>
        /// <param name="collection">The collection to add items to.</param>
        /// <param name="items">The items to add to the collection.</param>
        /// <returns>The collection.</returns>
        /// <exception cref="System.ArgumentNullException">An <see cref="System.ArgumentNullException"/> is thrown if <paramref name="collection"/> or <paramref name="items"/> is <see langword="null"/>.</exception>
		public static ICollection<T> AddRange<T>(this ICollection<T> collection, IEnumerable<T> items)
        {
            if (collection == null) throw new System.ArgumentNullException("collection");
            if (items == null) throw new System.ArgumentNullException("items");

            foreach (var each in items)
            {
                collection.Add(each);
            }

            return collection;
        }

        public static void AddDistinct<T>(this ICollection<T> obj, params T[] items)
        {
            AddDistinct(obj, null, items);
        }

        public static void AddDistinct<T>(this ICollection<T> obj, IEqualityComparer<T> comparer, params T[] items)
        {
            if (obj == null)
                throw new ArgumentNullException("obj");

            if (items != null)
            {
                foreach (var item in items)
                {
                    var contains = comparer != null ? obj.Contains(item, comparer) : obj.Contains(item);

                    if (!contains)
                        obj.Add(item);
                }
            }
        }


        public static void Replace<T>(this ICollection<T> obj, IEnumerable<T> newItems)
        {
            if (obj == null)
                throw new ArgumentNullException("obj");

            obj.Clear();
            obj.AddRange(newItems);
        }

        
    
    }
}
