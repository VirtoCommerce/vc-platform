using System.Collections.Generic;
using System.ComponentModel;
using VirtoCommerce.Foundation.Catalogs.Model;
using VirtoCommerce.ManagementClient.Core.Infrastructure;

namespace VirtoCommerce.ManagementClient.Catalog.ViewModel.Catalog.Interfaces
{
	public interface ISearchItemViewModel : IViewModel
	{
		List<CatalogBase> AvailableCatalogs { get; }

		string ExcludeItemId { set; }

		Item SelectedItem { get; }
		ICollectionView ListItemsSource { get; }
	}
}
