using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace VirtoCommerce.ManagementClient.Core.Infrastructure
{
	public interface ICollectionChange<T>
	{
		ObservableCollection<T> InnerItems { get; }
		ICollection<T> AddedItems { get; }
		ObservableCollection<T> UpdatedItems { get; }
		ICollection<T> RemovedItems { get; }

		NotifyCollectionChangedEventHandler CollectionChanged { set; }
	}
}
