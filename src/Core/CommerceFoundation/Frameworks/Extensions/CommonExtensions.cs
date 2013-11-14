using System.Collections.Generic;
using System.Linq;

namespace VirtoCommerce.Foundation.Frameworks.Extensions
{
    public static class CommonExtensions
    {
        /// <summary>
        /// helper clears the collection and then adds all items to it
        /// </summary>
        public static void SetItems<T>(this ICollection<T> collection, IEnumerable<T> items)
        {
            collection.Clear();
            collection.Add<T>(items);
        }

        /// <summary>
        /// helper adds all items to collection
        /// </summary>
        public static void Add<T>(this ICollection<T> collection, IEnumerable<T> items)
        {
            foreach (T item in items)
            {
                // item.SetParent(Parent, ParentPropertyName);
                collection.Add(item);
            }
        }

        /// <summary>
        /// delete item's children (whole collection) helper
        /// </summary>
        /// <typeparam name="T">StorageEntity</typeparam>
        /// <param name="sourceCollection"></param>
        /// <param name="repository"></param>
        public static void DeleteCollectionItems<T>(this ICollection<T> sourceCollection, IRepository repository) where T : StorageEntity
        {
            sourceCollection.ToList().ForEach(x =>
            {
                repository.Attach(x);
                repository.Remove(x);
                sourceCollection.Remove(x);
            });
        }

    }
}
