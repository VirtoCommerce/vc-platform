using System.ComponentModel;
using Microsoft.Practices.Prism.Commands;

namespace VirtoCommerce.ManagementClient.Core.Infrastructure
{
	public interface IViewModelHomeBase
	{
		ICollectionView ListItemsSource { get; }

		DelegateCommand RefreshItemsCommand { get; }

		DelegateCommand SearchItemsCommand { get; }
	}
}
