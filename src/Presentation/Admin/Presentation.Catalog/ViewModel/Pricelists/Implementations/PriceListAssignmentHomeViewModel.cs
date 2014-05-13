using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Interactivity.InteractionRequest;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.Foundation.Security.Model;
using VirtoCommerce.ManagementClient.Catalog.ViewModel.Pricelists.Interfaces;
using VirtoCommerce.ManagementClient.Catalog.ViewModel.Wizard;
using VirtoCommerce.ManagementClient.Core.Infrastructure;
using VirtoCommerce.ManagementClient.Core.Infrastructure.DataVirtualization;
using VirtoCommerce.Foundation.Catalogs.Factories;
using VirtoCommerce.Foundation.Catalogs.Model;
using VirtoCommerce.Foundation.Catalogs.Repositories;

namespace VirtoCommerce.ManagementClient.Catalog.ViewModel.Pricelists.Implementations
{
	public class PriceListAssignmentHomeViewModel : ViewModelHomeEditableBase<PricelistAssignment>, IPriceListAssignmentHomeViewModel, IVirtualListLoader<IPriceListAssignmentViewModel>, ISupportDelayInitialization
    {
        #region Dependencies

        private readonly ICatalogEntityFactory _entityFactory;
        private readonly IAuthenticationContext _authContext;
		private readonly IRepositoryFactory<IPricelistRepository> _pricelistRepository;
		private readonly IViewModelsFactory<IPriceListAssignmentViewModel> _itemVmFactory;
		private readonly IViewModelsFactory<ICreatePriceListAssignmentViewModel> _wizardVmFactory;

        #endregion

        public PriceListAssignmentHomeViewModel(IRepositoryFactory<IPricelistRepository> pricelistRepository,
			ICatalogEntityFactory entityFactory, IAuthenticationContext authContext, IViewModelsFactory<IPriceListAssignmentViewModel> itemVmFactory, IViewModelsFactory<ICreatePriceListAssignmentViewModel> wizardVmFactory)
        {
            _entityFactory = entityFactory;
            _authContext = authContext;
	        _pricelistRepository = pricelistRepository;
	        _itemVmFactory = itemVmFactory;
	        _wizardVmFactory = wizardVmFactory;

            ClearFiltersCommand = new DelegateCommand(DoClearFilters);

        }

		#region ViewModelHomeEditableBase

		protected override bool CanItemAddExecute()
		{
			return _authContext.CheckPermission(PredefinedPermissions.PricingPrice_List_AssignmentsManage);
		}

		protected override bool CanItemDeleteExecute(IList x)
		{
			return _authContext.CheckPermission(PredefinedPermissions.PricingPrice_List_AssignmentsManage) && x != null && x.Count > 0;
		}

		protected override void RaiseItemAddInteractionRequest()
		{
			var item = _entityFactory.CreateEntity<PricelistAssignment>();
			var itemVM = _wizardVmFactory.GetViewModelInstance(new KeyValuePair<string, object>("item", item));

			var confirmation = new Confirmation { Content = itemVM, Title = "Create Price List Assignment".Localize() };
			ItemAdd(confirmation);
		}

		protected override void RaiseItemDeleteInteractionRequest(IList selectedItemsList)
		{
			var selectedItems = selectedItemsList.Cast<VirtualListItem<IPriceListAssignmentViewModel>>();
			ItemDelete(selectedItems.Select(x => ((IViewModelDetailBase)x.Data)).ToList());
		}

		#endregion



        #region IPriceListAssignmentHomeViewModel Members

        public string SearchFilterKeyword { get; set; }
        public string SearchFilterName { get; set; }

        public DelegateCommand ClearFiltersCommand { get; private set; }

        #endregion

        #region IVirtualListLoader<IViewModel> Members

        public bool CanSort
        {
            get { return false; }
        }

        public IList<IPriceListAssignmentViewModel> LoadRange(int startIndex, int count, SortDescriptionCollection sortDescriptions, out int overallCount)
        {
            var retVal = new List<IPriceListAssignmentViewModel>();

            using (var repository = _pricelistRepository.GetRepositoryInstance())
            {
                var query = repository.PricelistAssignments;

                if (!string.IsNullOrEmpty(SearchFilterKeyword))
                    query = query.Where(x => x.Name.Contains(SearchFilterKeyword)
                        || x.Description.Contains(SearchFilterKeyword));
                else
                {
                    if (!string.IsNullOrEmpty(SearchFilterName))
                        query = query.Where(x => x.Name.Contains(SearchFilterName));
                }

                overallCount = query.Count();
                var items = query.OrderByDescending(x => x.Priority).Skip(startIndex).Take(count).ToList();
	            retVal.AddRange(items.Select(item => _itemVmFactory.GetViewModelInstance(new KeyValuePair<string, object>("item", item))));
            }
            return retVal;
        }

        #endregion

        #region ISupportDelayInitialization Members

        public void InitializeForOpen()
        {
            if (ListItemsSource == null)
            {
                OnUIThread(() => ListItemsSource = new VirtualList<IPriceListAssignmentViewModel>(this, 25, SynchronizationContext.Current));
            }
        }

        #endregion

        #region Private members

        private void DoClearFilters()
        {
            SearchFilterKeyword = SearchFilterName = null;
            OnPropertyChanged("SearchFilterKeyword");
            OnPropertyChanged("SearchFilterName");
        }

        #endregion

    }
}
