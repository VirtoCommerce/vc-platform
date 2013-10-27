using System;
using System.Collections.Generic;
using System.ComponentModel;
using VirtoCommerce.ManagementClient.Core.Infrastructure;
using VirtoCommerce.Foundation.Catalogs.Model;
using catalogModel = VirtoCommerce.Foundation.Catalogs.Model;

namespace VirtoCommerce.ManagementClient.DynamicContent.ViewModel
{
    public interface ISearchCategoryViewModel : IViewModel, ISupportDelayInitialization
    {
        SearchCategoryModifier SearchModifier { set; }
        List<catalogModel.CatalogBase> AvailableCatalogs { get; }
        CategoryBase SelectedItem { get; }
		bool UseSubCategories { get; }

        ICollectionView ListItemsSource { get; }
    }

    [Flags]
    public enum SearchCategoryModifier
    {
        RealCatalogsOnly,
        UserCanChangeSearchCatalog
    }
}
