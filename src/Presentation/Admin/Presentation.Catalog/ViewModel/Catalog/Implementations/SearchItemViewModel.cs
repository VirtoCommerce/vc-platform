using Microsoft.Practices.Prism.Commands;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using VirtoCommerce.Foundation.Catalogs.Model;
using VirtoCommerce.Foundation.Catalogs.Repositories;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.ManagementClient.Catalog.ViewModel.Catalog.Interfaces;
using VirtoCommerce.ManagementClient.Core.Infrastructure;
using VirtoCommerce.ManagementClient.Core.Infrastructure.DataVirtualization;

namespace VirtoCommerce.ManagementClient.Catalog.ViewModel.Catalog.Implementations
{
    [ImplementPropertyChanged]
    public class SearchItemViewModel : ViewModelBase, ISearchItemViewModel, IVirtualListLoader<Item>
    {
        private readonly IRepositoryFactory<ICatalogRepository> _catalogRepository;

        public DelegateCommand ClearFiltersCommand { get; private set; }
        public DelegateCommand SearchCommand { get; private set; }

        public string SearchName { get; set; }
        public string SearchCode { get; set; }
        public string SearchFilterItemType { get; set; }
        public string[] SearchFilterItemTypes { get; set; }
        public string SearchCatalogId { get; set; }

        public bool CanChangeSearchItemType { get; private set; }
        public bool CanChangeSearchCatalog { get; private set; }

        public SearchItemViewModel(IRepositoryFactory<ICatalogRepository> catalogRepository, object catalogInfo, string searchType)
        {
            _catalogRepository = catalogRepository;

            SearchFilterItemTypes = new[] { "Variation", "Product", "Bundle", "Package", "Dynamic Kit" };

            // TODO: move it to parameter constructor and refactor all calls
            // rp, quick hack, Inventory should contain not only products but variations...
            if (searchType == "Product...")
            {
                SearchFilterItemType = SearchFilterItemTypes[1];
                CanChangeSearchItemType = true;
            }
            else
            {
                SearchFilterItemType = SearchFilterItemTypes.FirstOrDefault(
                    x => x.Equals(searchType, StringComparison.OrdinalIgnoreCase));

                if (SearchFilterItemType == null)
                    CanChangeSearchItemType = true;
            }

            var catalogBase = catalogInfo as CatalogBase;
            if (catalogBase != null)
            {
                AvailableCatalogs = new List<CatalogBase> { catalogBase };
                SearchCatalogId = AvailableCatalogs[0].CatalogId;
            }
            else
            {
                using (var repository = _catalogRepository.GetRepositoryInstance())
                {
                    var query = repository.Catalogs;
                    if (!string.IsNullOrEmpty(catalogInfo as string))
                    {
                        query = query.Where(x => x.CatalogId == (string)catalogInfo);
                        SearchCatalogId = (string)catalogInfo;
                    }
                    else
                    {
                        query = query.OrderBy(x => x.Name);
                        CanChangeSearchCatalog = true;
                    }

                    AvailableCatalogs = query.ToList();
                }
            }

            SearchCommand = new DelegateCommand(DoSearch);
            ClearFiltersCommand = new DelegateCommand(DoClearFilters);
            ListItemsSource = new VirtualList<Item>(this, 20, SynchronizationContext.Current);
            SelectedItem = null;
        }

        #region ISearchItemViewModel Members

        public List<CatalogBase> AvailableCatalogs { get; private set; }
        public string ExcludeItemId { private get; set; }

        public Item SelectedItem { get; set; }

        public ICollectionView ListItemsSource { get; private set; }

        #endregion

        #region IVirtualListLoader<Item> Members

        public bool CanSort
        {
            get
            {
                return true;
            }
        }

        public IList<Item> LoadRange(int startIndex, int count, SortDescriptionCollection sortDescriptions, out int overallCount)
        {
            var retVal = new List<Item>();

            using (var repository = _catalogRepository.GetRepositoryInstance())
            {
                var query = repository.Items;
                if (!string.IsNullOrEmpty(ExcludeItemId))
                {
                    query = query.Where(x => x.ItemId != ExcludeItemId);
                }

                if (!string.IsNullOrEmpty(SearchName))
                {
                    query = query.Where(x => x.Name.Contains(SearchName));
                }

                if (!string.IsNullOrEmpty(SearchCode))
                {
                    query = query.Where(x => x.Code.Contains(SearchCode));
                }

                if (SearchFilterItemType == SearchFilterItemTypes[1])
                {
                    query = query.OfType<Product>();
                }
                else if (SearchFilterItemType == SearchFilterItemTypes[0])
                {
                    query = query.OfType<Sku>();
                }
                else if (SearchFilterItemType == SearchFilterItemTypes[2])
                {
                    query = query.OfType<Bundle>();
                }
                else if (SearchFilterItemType == SearchFilterItemTypes[3])
                {
                    query = query.OfType<Package>();
                }
                else if (SearchFilterItemType == SearchFilterItemTypes[4])
                {
                    query = query.OfType<DynamicKit>();
                }

                if (!string.IsNullOrEmpty(SearchCatalogId))
                {
                    var catalog = repository.Catalogs.Where(cat => cat.CatalogId == SearchCatalogId).FirstOrDefault();

                    if (catalog != null)
                    {
                        if (catalog is VirtualCatalog)
                        {
                            query = query.Where(i => i.CategoryItemRelations.Any(x => x.CatalogId == SearchCatalogId || x.Category.LinkedCategories.Any(y => y.CatalogId == SearchCatalogId)));
                        }
                        else
                        {
                            query = query.Where(c => c.CatalogId == SearchCatalogId);
                        }
                    }
                }

                overallCount = query.Count();
                var results = query.OrderBy(x => x.Name).Skip(startIndex).Take(count);
                retVal.AddRange(results);
            }
            return retVal;
        }

        #endregion

        #region private members

        private void DoSearch()
        {
            ListItemsSource.Refresh();
            SelectedItem = null;
        }

        private void DoClearFilters()
        {
            SearchName = SearchCode = null;
        }

        #endregion

    }
}
