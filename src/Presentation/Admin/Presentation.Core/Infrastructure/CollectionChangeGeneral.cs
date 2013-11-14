using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;

namespace VirtoCommerce.ManagementClient.Core.Infrastructure
{
	public class CollectionChangeGeneral<T> : ICollectionChange<T>, IDisposable where T : INotifyPropertyChanged
	{
		public CollectionChangeGeneral()
			: this(new List<T>())
		{
		}

		public CollectionChangeGeneral(IEnumerable<T> items)
		{
			OriginalItems = new List<T>(items);
			InnerItems = new ObservableCollection<T>(items);
			InnerItems.ToList().ForEach(x => x.PropertyChanged += OnPropertyChanged);
			AddedItems = new Collection<T>();
			UpdatedItems = new ObservableCollection<T>();
			RemovedItems = new Collection<T>();
		}

		public List<T> OriginalItems { get; private set; }
		public ObservableCollection<T> InnerItems { get; set; }
		public ICollection<T> AddedItems { get; private set; }
		public ObservableCollection<T> UpdatedItems { get; private set; }
		public ICollection<T> RemovedItems { get; private set; }

		public NotifyCollectionChangedEventHandler CollectionChanged
		{
			set
			{
				// InnerItems.ToList().ForEach(x => x.PropertyChanged += value);
				InnerItems.ToList().ForEach(x =>
					{
						x.PropertyChanged -= OnPropertyChanged;
						x.PropertyChanged += OnPropertyChanged;
					});

				InnerItems.CollectionChanged += value;
				UpdatedItems.CollectionChanged += value;
			}
		}

		public void Unsubscribe(NotifyCollectionChangedEventHandler value)
		{
			// InnerItems.ToList().ForEach(x.PropertyChanged -= value);

			InnerItems.CollectionChanged -= value;
			UpdatedItems.CollectionChanged -= value;
		}

		public void Add(T item)
		{
			item.PropertyChanged += OnPropertyChanged;
			AddedItems.Add(item);
			InnerItems.Add(item);
		}

		public void Remove(T item)
		{
			if (AddedItems.Contains(item))
				AddedItems.Remove(item);
			else
			{
				if (UpdatedItems.Contains(item))
					UpdatedItems.Remove(item);
				RemovedItems.Add(item);
			}
			InnerItems.Remove(item);
			item.PropertyChanged -= OnPropertyChanged;
		}

		public void CommitChanges()
		{
			RemovedItems.Clear();
			UpdatedItems.Clear();
			AddedItems.Clear();

			OriginalItems.Clear();
			OriginalItems.AddRange(InnerItems);
		}

		public void Dispose()
		{
			InnerItems.ToList().ForEach(x => x.PropertyChanged -= OnPropertyChanged);
		}

		private void OnPropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
		{
			var item = (T)sender;
			if (!AddedItems.Contains(item) && !UpdatedItems.Contains(item))
				UpdatedItems.Add(item);
		}
	}
}
