using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Interactivity.InteractionRequest;
using VirtoCommerce.Foundation.Catalogs.Model;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.Foundation.Frameworks.Extensions;
using VirtoCommerce.Foundation.Inventories.Model;
using VirtoCommerce.Foundation.Inventories.Repositories;
using VirtoCommerce.Foundation.Security.Model;
using VirtoCommerce.Foundation.Stores.Model;
using VirtoCommerce.Foundation.Stores.Repositories;
using VirtoCommerce.ManagementClient.Core.Infrastructure;
using VirtoCommerce.ManagementClient.Core.Infrastructure.DataVirtualization;
using VirtoCommerce.ManagementClient.Core.Infrastructure.Navigation;
using VirtoCommerce.ManagementClient.Core.Infrastructure.Tiles;
using VirtoCommerce.ManagementClient.Fulfillment.ViewModel.FulfillmentCenters.Interfaces;
using VirtoCommerce.ManagementClient.Fulfillment.ViewModel.Inventory.Interfaces;

namespace VirtoCommerce.ManagementClient.Fulfillment.ViewModel.Inventory.Implementations
{
	public class InventoryHomeViewModel : ViewModelBase, IInventoryHomeViewModel, IVirtualListLoader<IInventoryViewModel>, ISupportDelayInitialization
	{
		#region Dependencies
		private readonly IRepositoryFactory<IFulfillmentCenterRepository> _fulfillmentCenterRepositoryFactory;
		private readonly IRepositoryFactory<IInventoryRepository> _inventoryRepositoryFactory;
		private readonly IReceiveInventoryViewModel _receiveInventoryVm;
		private readonly IViewModelsFactory<IInventoryViewModel> _itemVmFactory;
		private readonly IAuthenticationContext _authContext;
		private readonly INavigationManager _navManager;
		private readonly TileManager _tileManager;
		#endregion

		public DelegateCommand AddInventoryListCommand
		{
			get;
			private set;
		}

		#region Constructor
		public InventoryHomeViewModel(
			IViewModelsFactory<IInventoryViewModel> itemVmFactory,
			IReceiveInventoryViewModel receiveInventoryVm,
			IRepositoryFactory<IInventoryRepository> inventoryRepositoryFactory,
			IRepositoryFactory<IFulfillmentCenterRepository> fulfillmentCenterRepositoryFactory,
			IAuthenticationContext authContext, INavigationManager navManager, TileManager tileManager)
		{
			_fulfillmentCenterRepositoryFactory = fulfillmentCenterRepositoryFactory;
			_inventoryRepositoryFactory = inventoryRepositoryFactory;
			_receiveInventoryVm = receiveInventoryVm;
			_itemVmFactory = itemVmFactory;
			_authContext = authContext;
			_navManager = navManager;
			_tileManager = tileManager;

			CommonConfirmRequest = new InteractionRequest<Confirmation>();
			CommonNotifyRequest = new InteractionRequest<Notification>();
			AddInventoryListCommand = new DelegateCommand(RaiseAddInventoryListInteractionRequest);
			SearchAllItemsCommand = new DelegateCommand(DoSearchAllItems);
			SearchFilterFulfillments = new ObservableCollection<FulfillmentCenter>();
			SearchFilterCatalogs = new ObservableCollection<CatalogBase>();
			ClearFiltersCommand = new DelegateCommand(DoClearFilters);
			SearchItemsCommand = new DelegateCommand(DoSearchItems);

			PopulateTiles();
		}
		#endregion

		private bool IsInSearchAllMode { get { return !string.IsNullOrEmpty(SearchFilterAll); } }
		public string SearchFilterAll { get; set; }
		public DelegateCommand SearchAllItemsCommand { get; private set; }

		private void UpdateFulfillmentCenterFilter()
		{
			//Get all fulfillment centers
			Action getAllFulfillmentsAction = () =>
			{
				using (var repository = _fulfillmentCenterRepositoryFactory.GetRepositoryInstance())
				{
					var fulfillments = repository.FulfillmentCenters.ToList();
					OnUIThread(() =>
						{
							SearchFilterFulfillments.SetItems(fulfillments);
							if (SearchFilterFulfillments.Count > 0)
								SearchFilterFulfillment = SearchFilterFulfillments[0];
							OnPropertyChanged("SearchFilterFulfillment");
						});
				}
			};
			getAllFulfillmentsAction.BeginInvoke(null, null);
		}

		public string SearchFilterCode { get; set; }
		public string SearchFilterName { get; set; }
		public FulfillmentCenter SearchFilterFulfillment { get; set; }
		public ObservableCollection<FulfillmentCenter> SearchFilterFulfillments { get; set; }

		public CatalogBase SearchFilterCatalog { get; set; }
		public ObservableCollection<CatalogBase> SearchFilterCatalogs { get; set; }

		public DelegateCommand ClearFiltersCommand { get; private set; }
		public DelegateCommand SearchItemsCommand { get; private set; }

		#region privates
		private void RaiseAddInventoryListInteractionRequest()
		{
			_receiveInventoryVm.SelectedFulfillmentCenter = SearchFilterFulfillment;

			var confirmation = new ConditionalConfirmation { Title = "Receive inventory".Localize(), Content = _receiveInventoryVm };

			CommonConfirmRequest.Raise(confirmation, x =>
			{
				if (x.Confirmed)
				{
					SearchFilterFulfillment = SearchFilterFulfillments.FirstOrDefault(y => y.FulfillmentCenterId == _receiveInventoryVm.SelectedFulfillmentCenter.FulfillmentCenterId);
					_receiveInventoryVm.InventoryItems.ToList().ForEach(y => y.FulfillmentCenterId = _receiveInventoryVm.SelectedFulfillmentCenter.FulfillmentCenterId);
					var l = _receiveInventoryVm.InventoryItems.Where(y => !string.IsNullOrEmpty(y.Sku) && y.InStockQuantity >= 0).ToList();
					_receiveInventoryVm.InventoryItems.Clear();
					foreach (var inventory in l)
					{
						using (var repository = _inventoryRepositoryFactory.GetRepositoryInstance())
						{
							var sku =
								repository.Inventories.ToList()
										  .FirstOrDefault(x1 => x1.Sku == inventory.Sku && x1.FulfillmentCenterId == inventory.FulfillmentCenterId);
							if (sku != null)
							{
								sku.InStockQuantity += inventory.InStockQuantity;
							}
							else
							{
								inventory.Status = (int)InventoryStatus.Enabled;
								repository.Add(inventory);
							}

							repository.UnitOfWork.Commit();
						}
					}

					SelectedInventoryItems.Refresh();
					OnSpecifiedPropertyChanged("SearchFilterFulfillment");
				}
			});
		}

		#endregion

		public InteractionRequest<Confirmation> CommonConfirmRequest
		{
			get;
			private set;
		}

		public InteractionRequest<Notification> CommonNotifyRequest { get; private set; }

		#region IInventoryHomeViewModel
		private ICollectionView _selectedInventoryItems;
		public ICollectionView SelectedInventoryItems
		{
			get
			{
				return _selectedInventoryItems;
			}
			private set
			{
				_selectedInventoryItems = value;
				OnSpecifiedPropertyChanged("SelectedInventoryItems");
			}
		}


		private IViewModel _selectedInventoryItem;
		public IViewModel SelectedInventoryItem
		{
			get
			{
				return _selectedInventoryItem;
			}
			set
			{
				_selectedInventoryItem = value;
				SelectedInventoryItems.Refresh();

				OnSpecifiedPropertyChanged("SelectedInventoryItem");
			}
		}

		/// <summary>
		/// For complete shipment command purposes
		/// </summary>
		public IMainFulfillmentViewModel ParentViewModel { get; set; }
		#endregion

		#region IVirtualListLoader<IInventoryViewModel> Members

		public bool CanSort
		{
			get { return false; }
		}

		public IList<IInventoryViewModel> LoadRange(int startIndex, int count, SortDescriptionCollection sortDescriptions, out int overallCount)
		{
			var retVal = new List<IInventoryViewModel>();

			using (var repository = _inventoryRepositoryFactory.GetRepositoryInstance())
			{
				var canQuery = false;
				var query = repository.Inventories;

				if (IsInSearchAllMode) // quick search from a single textbox
				{
					canQuery = true;
					query = query.Where(x => x.Sku.Contains(SearchFilterAll) || x.InventoryId.Contains(SearchFilterAll));
				}
				else
				{
					if (SearchFilterFulfillment == null)
						UpdateFulfillmentCenterFilter();

					if (SearchFilterFulfillment != null && !string.IsNullOrEmpty(SearchFilterFulfillment.FulfillmentCenterId))
					{
						canQuery = true;
						query = query.Where(x => x.FulfillmentCenterId == SearchFilterFulfillment.FulfillmentCenterId);
					}

					if (!string.IsNullOrEmpty(SearchFilterCode))
					{
						canQuery = true;
						query = query.Where(x => x.Sku.Contains(SearchFilterCode));
					}

					if (!string.IsNullOrEmpty(SearchFilterName))
					{
						canQuery = true;
						query = query.Where(x => x.InventoryId.Contains(SearchFilterName));
					}
				}

				if (canQuery)
				{
					overallCount = query.Count();
					var items = query.OrderBy(x => x.InventoryId).Skip(startIndex).Take(count).ToList();
					retVal.AddRange(items.Select(item => _itemVmFactory.GetViewModelInstance(new KeyValuePair<string, object>("item", item))));
				}
				else
				{
					overallCount = 0;
				}
			}

			return retVal;
		}

		#endregion

		#region ISupportDelayInitialization Members

		public void InitializeForOpen()
		{
			if (SearchFilterFulfillments.Count == 0)
			{
				UpdateFulfillmentCenterFilter();
			}

			if (SelectedInventoryItems == null)
			{
				OnUIThread(() => SelectedInventoryItems = new VirtualList<IInventoryViewModel>(this, 25, SynchronizationContext.Current));
			}
		}

		#endregion

		#region private methods

		private void DoSearchAllItems()
		{
			SelectedInventoryItems.Refresh();
		}

		private void DoSearchItems()
		{
			SelectedInventoryItems.Refresh();
		}

		private void DoClearFilters()
		{
			SearchFilterAll = null;
			OnSpecifiedPropertyChanged("SearchFilterAll");

			SearchFilterName = SearchFilterCode = null;
			SearchFilterCatalog = null;
			OnSpecifiedPropertyChanged("SearchFilterCode");
			OnSpecifiedPropertyChanged("SearchFilterName");
			OnSpecifiedPropertyChanged("SearchFilterCatalog");
		}

		private void PopulateTiles()
		{
			if (_tileManager == null)
				return;

			if (_authContext.CheckPermission(PredefinedPermissions.FulfillmentInventoryManage))
			{
				_tileManager.AddTile(new IconTileItem
				{
					IdModule = Catalog.NavigationNames.MenuName,
					IdTile = "Fulfillments",
					TileIconSource = "Icon_Module_Fulfillment",
                    TileTitle = "Fulfillment",
                    TileCategory = NavigationNames.ModuleName,
					Order = 6,
					IdColorSchema = TileColorSchemas.Schema3,
					NavigateCommand = new DelegateCommand(NavigateHome)
				});
			}
		}

		private void NavigateHome()
		{
			var navigationData = _navManager.GetNavigationItemByName(NavigationNames.HomeName);
			if (navigationData != null)
			{
				_navManager.Navigate(navigationData);
			}
		}

		#endregion
	}
}
