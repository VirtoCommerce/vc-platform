using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Interactivity.InteractionRequest;
using VirtoCommerce.Foundation.Catalogs.Factories;
using VirtoCommerce.Foundation.Catalogs.Model;
using VirtoCommerce.Foundation.Catalogs.Repositories;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.Foundation.Security.Model;
using VirtoCommerce.ManagementClient.Catalog.ViewModel.Pricelists.Interfaces;
using VirtoCommerce.ManagementClient.Catalog.ViewModel.Wizard;
using VirtoCommerce.ManagementClient.Core.Infrastructure;
using VirtoCommerce.ManagementClient.Core.Infrastructure.DataVirtualization;
using VirtoCommerce.ManagementClient.Core.Infrastructure.Navigation;
using VirtoCommerce.ManagementClient.Core.Infrastructure.Tiles;

namespace VirtoCommerce.ManagementClient.Catalog.ViewModel.Pricelists.Implementations
{
	public class PriceListHomeViewModel : ViewModelHomeEditableBase<Pricelist>, IPriceListHomeViewModel, IVirtualListLoader<IPriceListViewModel>, ISupportDelayInitialization
	{
		#region Dependencies

		private readonly ICatalogEntityFactory _entityFactory;
		private readonly IAuthenticationContext _authContext;
		private readonly IRepositoryFactory<IPricelistRepository> _pricelistRepository;
		private readonly IViewModelsFactory<IPriceListViewModel> _itemVmFactory;
		private readonly IViewModelsFactory<ICreatePriceListViewModel> _wizardVmFactory;
		private readonly INavigationManager _navManager;
		private readonly TileManager _tileManager;

		#endregion

		public PriceListHomeViewModel(ICatalogEntityFactory entityFactory, IViewModelsFactory<IPriceListViewModel> itemVmFactory,
			IViewModelsFactory<ICreatePriceListViewModel> wizardVmFactory,
			IRepositoryFactory<IPricelistRepository> pricelistRepository, IAuthenticationContext authContext,
			INavigationManager navManager, TileManager tileManager)
		{
			_entityFactory = entityFactory;
			_pricelistRepository = pricelistRepository;
			_authContext = authContext;
			_wizardVmFactory = wizardVmFactory;
			_itemVmFactory = itemVmFactory;
			_navManager = navManager;
			_tileManager = tileManager;

			ClearFiltersCommand = new DelegateCommand(DoClearFilters);

			PopulateTiles();
		}

		#region ViewModelHomeEditableBase

		protected override bool CanItemAddExecute()
		{
			return _authContext.CheckPermission(PredefinedPermissions.PricingPrice_ListsManage);
		}

		protected override bool CanItemDeleteExecute(IList x)
		{
			return _authContext.CheckPermission(PredefinedPermissions.PricingPrice_List_AssignmentsManage) && x != null &&
				   x.Count > 0;
		}

		protected override void RaiseItemAddInteractionRequest()
		{
			var item = _entityFactory.CreateEntity<Pricelist>();
			var vm = _wizardVmFactory.GetViewModelInstance(new KeyValuePair<string, object>("item", item));

			var confirmation = new Confirmation { Content = vm, Title = "Create Price List".Localize() };
			ItemAdd(confirmation);
		}

		protected override void RaiseItemDeleteInteractionRequest(IList selectedItemsList)
		{
			var selectedItems = selectedItemsList.Cast<VirtualListItem<IPriceListViewModel>>();
			ItemDelete(selectedItems.Select(x => ((IViewModelDetailBase)x.Data)).ToList());
		}

		#endregion

		#region IPriceListHomeViewModel Members

		public string SearchFilterKeyword { get; set; }
		public string SearchFilterName { get; set; }
		public string SearchFilterCurrency { get; set; }

		public DelegateCommand ClearFiltersCommand { get; private set; }

		#endregion

		#region IVirtualListLoader<IPriceListViewModel> Members

		public bool CanSort
		{
			get { return false; }
		}

		public IList<IPriceListViewModel> LoadRange(int startIndex, int count, SortDescriptionCollection sortDescriptions, out int overallCount)
		{
			var retVal = new List<IPriceListViewModel>();

			using (var repository = _pricelistRepository.GetRepositoryInstance())
			{
				var query = repository.Pricelists;

				if (!string.IsNullOrEmpty(SearchFilterKeyword))
					query = query.Where(x => x.Name.Contains(SearchFilterKeyword)
						|| x.Description.Contains(SearchFilterKeyword));
				else
				{
					if (!string.IsNullOrEmpty(SearchFilterName))
						query = query.Where(x => x.Name.Contains(SearchFilterName));

					if (!string.IsNullOrEmpty(SearchFilterCurrency))
						query = query.Where(x => x.Currency.Contains(SearchFilterCurrency));
				}

				overallCount = query.Count();
				var items = query.OrderBy(x => x.Name).Skip(startIndex).Take(count).ToList();

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
				OnUIThread(() => ListItemsSource = new VirtualList<IPriceListViewModel>(this, 25, SynchronizationContext.Current));
			}
		}

		#endregion

		#region private members

		private void DoClearFilters()
		{
			SearchFilterKeyword = SearchFilterName = SearchFilterCurrency = null;
			OnPropertyChanged("SearchFilterKeyword");
			OnPropertyChanged("SearchFilterName");
			OnPropertyChanged("SearchFilterCurrency");
		}

		#endregion

		#region Tiles

		public DelegateCommand NewPriceListNavigate()
		{
			return new DelegateCommand(() => OnUIThread(async () =>
				{
					if (NavigateToTabPage(NavigationNames.HomeNamePriceList))
					{
						await Task.Run(() => Thread.Sleep(1300)); // we need some time to parse xaml  
						ItemAddCommand.Execute();

					}
				}));
		}

		private bool NavigateToTabPage(string id)
		{
			var navigationData = _navManager.GetNavigationItemByName(NavigationNames.HomeNamePriceList);
			if (navigationData != null)
			{
				_navManager.Navigate(navigationData);
				var mainViewModel = _navManager.GetViewFromRegion(navigationData) as MainPriceListViewModel;

				return (mainViewModel != null && mainViewModel.SetCurrentTabById(id));
			}
			return false;
		}

		private void PopulateTiles()
		{
			if (_tileManager == null)
				return;

			if (_authContext.CheckPermission(PredefinedPermissions.PricingPrice_ListsManage))
			{
				_tileManager.AddTile(new NumberTileItem()
					{
						IdModule = NavigationNames.MenuName,
						IdTile = "PriceLists",
                        TileTitle = "Price lists",
                        TileCategory = NavigationNames.ModuleName,
						Order = 3,
						IdColorSchema = TileColorSchemas.Schema3,
						NavigateCommand = new DelegateCommand(() => NavigateToTabPage(NavigationNames.HomeNamePriceList)),
						Refresh = async (tileItem) =>
							{
								try
								{
									using (var repository = _pricelistRepository.GetRepositoryInstance())
									{
										if (tileItem is NumberTileItem)
										{
											var query = await Task.Factory.StartNew(() => repository.Pricelists.Count());
											(tileItem as NumberTileItem).TileNumber = query.ToString();
										}
									}
								}
								catch
								{
								}
							}
					});
			}

			//if (_authContext.CheckPermission(PredefinedPermissions.PricingPrice_ItemPricingManage))
			//{
			//	_tileManager.AddTile(new IconTileItem()
			//		{
			//			IdModule = NavigationNames.MenuName,
			//			IdTile = "NewPriceList",
			//			TileIconSource = "Icon_Module_PriceLists",
			//			TileTitle = "New price list",
			//			Order = 6,
			//			IdColorSchema = TileColorSchemas.Schema2,
			//			NavigateCommand = new DelegateCommand(() => OnUIThread(async () =>
			//				{
			//					if (NavigateToTabPage(NavigationNames.HomeNamePriceList))
			//					{
			//						await Task.Run(() => Thread.Sleep(300)); // we need some time to parse xaml  
			//						ItemAddCommand.Execute();

			//					}
			//				}))
			//		});
			//}
		}

		#endregion

	}
}
