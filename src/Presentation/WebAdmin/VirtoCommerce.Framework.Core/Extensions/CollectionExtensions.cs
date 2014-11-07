using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtoCommerce.Framework.Core.Extensions
{
    public static class CollectionExtensions
    {
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

        public static void AddRange<T>(this ICollection<T> obj, IEnumerable<T> newItems)
        {
            if (obj == null)
                throw new ArgumentNullException("obj");

            if (newItems != null)
            {
                foreach (var item in newItems)
                    obj.Add(item);
            }
        }

        public static void Replace<T>(this ICollection<T> obj, IEnumerable<T> newItems)
        {
            if (obj == null)
                throw new ArgumentNullException("obj");

            obj.Clear();
            obj.AddRange(newItems);
        }

        public static void ProcessWithPaging<T>(this ICollection<T> obj, int pageSize, Action<IEnumerable<T>, int, int> action)
        {
            if (obj == null)
                throw new ArgumentNullException("obj");
            if (action == null)
                throw new ArgumentNullException("action");

            int skipCount = 0;
            int totalCount = obj.Count;

            do
            {
                var items = obj.Skip(skipCount).Take(pageSize);

                if (items.Any())
                    action(items, skipCount, totalCount);

                skipCount += pageSize;
            }
            while (skipCount < totalCount);
        }

		public static bool IsNullCollection<T>(this ICollection<T> collection)
		{
			return collection is NullCollection<T>;
		}

		public static void Patch<T>(this ICollection<T> source, ICollection<T> target, IEqualityComparer<T> comparer, Action<T, T> patch)
		{
			//Change
			foreach (var sourceItem in source)
			{
				var targetItem = target.FirstOrDefault(x => comparer.Equals(x, sourceItem));
				if (targetItem != null)
				{
					patch(sourceItem, targetItem);
				}
			}
			//Add
			foreach (var newItem in source.Except(target, comparer))
			{
				target.Add(newItem);
			}
			//Remove
			foreach (var removedItem in target.Except(source, comparer).ToArray())
			{
				target.Remove(removedItem);
			}

		}
    }
}
