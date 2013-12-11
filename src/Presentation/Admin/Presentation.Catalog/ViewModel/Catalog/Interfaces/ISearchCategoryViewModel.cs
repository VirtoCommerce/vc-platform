using System;
using System.Collections.Generic;
using System.ComponentModel;
using VirtoCommerce.ManagementClient.Core.Infrastructure;
using VirtoCommerce.Foundation.Catalogs.Model;

namespace VirtoCommerce.ManagementClient.Catalog.ViewModel.Catalog.Interfaces
{
    public interface ISearchCategoryViewModel : IViewModel, ISupportDelayInitialization
    {
        SearchCategoryModifier SearchModifier { set; }
        List<CatalogBase> AvailableCatalogs { get; }
        Category SelectedItem { get; }

        ICollectionView ListItemsSource { get; }
    }

    [Flags]
    public enum SearchCategoryModifier
    {
		RealCatalogsOnly = 1 << 0,
		UserCanChangeSearchCatalog = 1 << 1
    }
}
