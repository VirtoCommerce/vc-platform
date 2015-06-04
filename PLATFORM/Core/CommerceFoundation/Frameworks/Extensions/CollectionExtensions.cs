using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtoCommerce.Foundation.Frameworks.Extensions
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

		public static void Patch<T>(this ICollection<T> source, ICollection<T> target, Action<T, T> patch)
		{
			source.Patch(target, EqualityComparer<T>.Default, patch);
		}


		public static void Patch<T>(this ICollection<T> source, ICollection<T> target, IEqualityComparer<T> comparer, Action<T, T> patch)
		{
			Action<EntryState, T, T> patchAction = (state, x, y) =>
				{
					if(state == EntryState.Modified)
					{
						patch(x, y);
					}
					else if(state == EntryState.Added)
					{
						target.Add(x);
					}
					else if(state == EntryState.Deleted)
					{
						target.Remove(x);
					}
				};

			source.CompareTo(target, comparer, patchAction);
		}

		public static void CompareTo<T>(this ICollection<T> source, ICollection<T> target, IEqualityComparer<T> comparer, Action<EntryState, T, T> action)
		{
			//Change
			foreach (var sourceItem in source)
			{
				var targetItem = target.FirstOrDefault(x => comparer.Equals(x, sourceItem));
				if (targetItem != null && !targetItem.Equals(default(T)))
				{
					action(EntryState.Modified, sourceItem, targetItem);
				}
			}
			//Add
			foreach (var newItem in source.Except(target, comparer))
			{
				action(EntryState.Added, newItem, newItem);
			}
			//Remove
			foreach (var removedItem in target.Except(source, comparer).ToArray())
			{
				action(EntryState.Deleted, removedItem, removedItem);
			}
		}

		public static void  ObserveCollection<T>(this ObservableCollection<T> collection, Action<T> addAction, Action<T> removeAction)
		{
			collection.CollectionChanged += (sender, args) =>
			{
				if (args.Action == NotifyCollectionChangedAction.Add)
				{
					foreach (var newItem in args.NewItems)
					{
						if(addAction != null)
							addAction((T)newItem);
					}
				}
				else if (args.Action == NotifyCollectionChangedAction.Remove)
				{
					foreach (var removeItem in args.OldItems)
					{
						if(removeAction != null)
							removeAction((T)removeItem);
					}
				}
			};
		}

    }
}
