using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Practices.Prism.Commands;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.Foundation.Marketing.Factories;
using VirtoCommerce.Foundation.Marketing.Model;
using VirtoCommerce.Foundation.Marketing.Repositories;
using VirtoCommerce.Foundation.Security.Model;
using VirtoCommerce.ManagementClient.Core.Infrastructure;
using VirtoCommerce.ManagementClient.Core.Infrastructure.DataVirtualization;
using VirtoCommerce.ManagementClient.Core.Infrastructure.Navigation;
using VirtoCommerce.ManagementClient.Core.Infrastructure.Tiles;
using VirtoCommerce.ManagementClient.Marketing.Infrastructure;
using VirtoCommerce.ManagementClient.Marketing.ViewModel.Interfaces;
using VirtoCommerce.ManagementClient.Marketing.ViewModel.Wizard.Interfaces;

namespace VirtoCommerce.ManagementClient.Marketing.ViewModel.Implementations
{
	public class MarketingHomeViewModel : ViewModelHomeEditableBase<Promotion>, IMarketingHomeViewModel, IVirtualListLoader<IPromotionViewModelBase>, ISupportDelayInitialization
	{
		#region Dependencies

		private readonly IMarketingEntityFactory _entityFactory;
		private readonly IAuthenticationContext _authContext;
		private readonly IRepositoryFactory<IMarketingRepository> _marketingRepository;
		private readonly IViewModelsFactory<IItemTypeSelectionStepViewModel> _itemTypeVmFactory;
		private readonly IViewModelsFactory<ICreateCatalogPromotionViewModel> _wizardCatalogVmFactory;
		private readonly IViewModelsFactory<ICreateCartPromotionViewModel> _wizardCartVmFactory;
		private readonly IViewModelsFactory<ICartPromotionViewModel> _itemCartVmFactory;
		private readonly IViewModelsFactory<ICatalogPromotionViewModel> _itemCatalogVmFactory;
		private readonly NavigationManager _navManager;
		private readonly TileManager _tileManager;
		private string[] _searchFilterStates;
		private string[] _searchFilterTypes;

		#endregion

		public string SearchFilterName { get; set; }
		public string SearchFilterType { get; set; }
		public string[] SearchFilterTypes
		{
			get { return _searchFilterTypes ?? (_searchFilterTypes = new[] { "Catalog", "Cart" }); }
		}

		public string SearchFilterState { get; set; }
		public string[] SearchFilterStates
		{
			get { return _searchFilterStates ?? (_searchFilterStates = new[] { "Active", "Inactive", "Archived" }); }
		}

		public string SearchFilterKeyword { get; set; }


#if DESIGN
public MarketingHomeViewModel()
        {

        }
#endif

		#region ctor

		public MarketingHomeViewModel(
			IRepositoryFactory<IMarketingRepository> marketingRepository,
			IMarketingEntityFactory entityFactory,
			IAuthenticationContext authContext,
			IViewModelsFactory<IItemTypeSelectionStepViewModel> itemTypeVmFactory,
			IViewModelsFactory<ICreateCatalogPromotionViewModel> wizardCatalogVmFactory,
			IViewModelsFactory<ICreateCartPromotionViewModel> wizardCartVmFactory,
			IViewModelsFactory<ICartPromotionViewModel> itemCartVmFactory,
			IViewModelsFactory<ICatalogPromotionViewModel> itemCatalogVmFactory,
			NavigationManager navManager,
			TileManager tileManager)
		{
			_marketingRepository = marketingRepository;
			_entityFactory = entityFactory;
			_authContext = authContext;
			_navManager = navManager;
			_tileManager = tileManager;
			_itemTypeVmFactory = itemTypeVmFactory;
			_wizardCartVmFactory = wizardCartVmFactory;
			_wizardCatalogVmFactory = wizardCatalogVmFactory;
			_itemCartVmFactory = itemCartVmFactory;
			_itemCatalogVmFactory = itemCatalogVmFactory;

			PromotionItemCreateCommand = new DelegateCommand(RaisePromotionItemCreateInteractionRequest);
			PromotionCartCreateCommand = new DelegateCommand(RaiseCartPromotionCreateInteractionRequest);
			ItemDuplicateCommand = new DelegateCommand<IList>(RaisePromotionDuplicateInteractionRequest, x => x != null && x.Count > 0);
			ClearFiltersCommand = new DelegateCommand(DoClearFilters);
			CreateItemCommand = new DelegateCommand(CreateItem);
			PopulateTiles();
		}

		#endregion

		#region ViewModelHomeEditableBase

		protected override bool CanItemAddExecute()
		{
			return true;
		}

		protected override bool CanItemDeleteExecute(IList x)
		{
			return x != null && x.Count > 0;
		}

		protected override void RaiseItemAddInteractionRequest()
		{
		}

		protected override void RaiseItemDeleteInteractionRequest(IList selectedItemsList)
		{
			var selectedItems = selectedItemsList.Cast<VirtualListItem<IPromotionViewModelBase>>();
			ItemDelete(selectedItems.Select(x => ((IViewModelDetailBase)x.Data)).ToList());
		}

		#endregion

		#region IMarketingHomeViewModel Members

		public DelegateCommand PromotionItemCreateCommand { get; private set; }
		public DelegateCommand PromotionCartCreateCommand { get; private set; }
		public DelegateCommand<IList> ItemDuplicateCommand { get; private set; }
		public DelegateCommand CreateItemCommand { get; private set; }


		public DelegateCommand ClearFiltersCommand { get; private set; }

		#endregion

		#region IVirtualListLoader<Promotion> Members

		public bool CanSort
		{
			get
			{
				return true;
			}
		}

		public IList<IPromotionViewModelBase> LoadRange(int startIndex, int count, System.ComponentModel.SortDescriptionCollection sortDescriptions, out int overallCount)
		{
			var retVal = new List<IPromotionViewModelBase>();
			//Dictionary<string, string> sortBy = new Dictionary<string, string>();
			//foreach (System.ComponentModel.SortDescription sortDesc in sortDescriptions)
			//{
			//    sortBy.Add(sortDesc.PropertyName, (sortDesc.Direction == System.ComponentModel.ListSortDirection.Ascending ? "ASC" : "DESC"));
			//}

			using (var repository = _marketingRepository.GetRepositoryInstance())
			{
				var query = repository.Promotions;

				// filter results
				if (!string.IsNullOrEmpty(SearchFilterKeyword))
					query = query.Where(x => x.Name.Contains(SearchFilterKeyword)
						|| x.Description.Contains(SearchFilterKeyword)
						|| x.Status.Contains(SearchFilterKeyword));
				else
				{
					if (!string.IsNullOrEmpty(SearchFilterName))
					{
						query = query.Where(x => x.Name.Contains(SearchFilterName));
					}

					//  SearchFilterType
					if (SearchFilterType == SearchFilterTypes[0])
					{
						query = query.OfType<CatalogPromotion>();
					}
					else if (SearchFilterType == SearchFilterTypes[1])
					{
						query = query.OfType<CartPromotion>();
					}

					//  SearchFilterState
					if (SearchFilterStates.Contains(SearchFilterState))
					{
						// not supported:
						// query = query.Where(x => SearchFilterState == (string)PromotionStateConverter.Current.Convert(x, null, null, null));
						query = query.Where(x => x.Status == SearchFilterState);
					}
				}

				overallCount = query.Count();
				var results = query.OrderBy(x => x.Name).Skip(startIndex).Take(count).ToList();
				retVal.AddRange(from item in results
								let itemParameter = new KeyValuePair<string, object>("item", item)
								select (item is CartPromotion) ? _itemCartVmFactory.GetViewModelInstance(itemParameter) : (IPromotionViewModelBase)_itemCatalogVmFactory.GetViewModelInstance(itemParameter));
			}
			return retVal;
		}

		#endregion

		#region ISupportDelayInitialization Members

		public void InitializeForOpen()
		{
			if (ListItemsSource == null)
			{
				OnUIThread(() => ListItemsSource = new VirtualList<IPromotionViewModelBase>(this, 20, SynchronizationContext.Current));
			}
		}

		#endregion

		#region private members

		private void CreateItem()
		{
			var itemVM = _itemTypeVmFactory.GetViewModelInstance();
			//show selection step
			CommonConfirmRequest.Raise(new ConditionalConfirmation(() => !string.IsNullOrEmpty(itemVM.SelectedItemType))
			{
				Content = itemVM,
				Title = "Select item type".Localize()
			},
			(x) =>
			{
				if (x.Confirmed)
				{
					if (itemVM.SelectedItemType == itemVM.SearchFilterItemTypes[0].Value)
					{
						RaiseCartPromotionCreateInteractionRequest();
					}
					else if (itemVM.SelectedItemType == itemVM.SearchFilterItemTypes[1].Value)
					{
						RaisePromotionItemCreateInteractionRequest();
					}
				}
			}
			);
		}

		private void RaisePromotionItemCreateInteractionRequest()
		{
			var item = _entityFactory.CreateEntity<CatalogPromotion>();
			item.Status = PromotionStatusConverter.ValueActive;
			item.StartDate = DateTime.UtcNow.AddDays(-1);
			item.Priority = 1;

			var itemVM = _wizardCatalogVmFactory.GetViewModelInstance(
				new KeyValuePair<string, object>("item", item));

			ItemAdd(new ConditionalConfirmation { Title = "Create catalog promotion".Localize(), Content = itemVM });
		}

		private void RaiseCartPromotionCreateInteractionRequest()
		{
			Promotion item = _entityFactory.CreateEntity<CartPromotion>();
			item.Status = PromotionStatusConverter.ValueActive;
			item.StartDate = DateTime.UtcNow.AddDays(-1);
			item.Priority = 1;

			var itemVM = _wizardCartVmFactory.GetViewModelInstance(
				new KeyValuePair<string, object>("item", item));

			ItemAdd(new ConditionalConfirmation { Title = "Create cart promotion".Localize(), Content = itemVM });
		}

		private void RaisePromotionDuplicateInteractionRequest(IList selectedItemsList)
		{
			var selectedItems = selectedItemsList.Cast<VirtualListItem<IPromotionViewModelBase>>();
			ItemDuplicate(selectedItems.Select(x => ((IViewModelDetailBase)x.Data)).ToList());
		}

		private void DoClearFilters()
		{
			SearchFilterKeyword = SearchFilterName = SearchFilterType = SearchFilterState = null;
			OnPropertyChanged("SearchFilterKeyword");
			OnPropertyChanged("SearchFilterName");
			OnPropertyChanged("SearchFilterType");
			OnPropertyChanged("SearchFilterState");
		}
		#endregion

		#region Tiles

		private bool NavigateToTabPage(string id)
		{

			var navigationData = _navManager.GetNavigationItemByName(NavigationNames.HomeName);
			if (navigationData != null)
			{
				_navManager.Navigate(navigationData);
				var mainViewModel = _navManager.GetViewFromRegion(navigationData) as SubTabsDefaultViewModel;

				return (mainViewModel != null && mainViewModel.SetCurrentTabById(id));
			}
			return false;
		}

		private void PopulateTiles()
		{

			if (_authContext.CheckPermission(PredefinedPermissions.MarketingPromotionsManage))
			{
				_tileManager.AddTile(new NumberTileItem()
				{
					IdModule = NavigationNames.MenuName,
					IdTile = "ActivePromotions",
                    TileTitle = "Active promotions",
                    TileCategory = NavigationNames.ModuleName,
					Order = 0,
					IdColorSchema = TileColorSchemas.Schema3,
					NavigateCommand = new DelegateCommand(() => OnUIThread(async () =>
					{
						if (NavigateToTabPage(NavigationNames.HomeName))
						{
							SearchFilterState = SearchFilterStates[0];
							await Task.Run(() => Thread.Sleep(300));  // we need some time to parse xaml  
							SearchItemsCommand.Execute();
						}
					})),
					Refresh = async (tileItem) =>
					{
						try
						{
							using (var repository = _marketingRepository.GetRepositoryInstance())
							{
								if (tileItem is NumberTileItem)
								{
									var query = await Task.Factory.StartNew(() => repository.Promotions.Count());
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

			if (_authContext.CheckPermission(PredefinedPermissions.MarketingPromotionsManage))
			{
				_tileManager.AddTile(new IconTileItem()
					{
						IdModule = NavigationNames.MenuName,
						IdTile = "NewPromotion",
						TileIconSource = "Icon_Module_Promotions",
                        TileTitle = "New promotion",
                        TileCategory = NavigationNames.ModuleName,
						Order = 1,
						IdColorSchema = TileColorSchemas.Schema1,
						NavigateCommand = new DelegateCommand(() => OnUIThread(async () =>
							{
								if (NavigateToTabPage(NavigationNames.HomeName))
								{
									await Task.Run(() => Thread.Sleep(300)); // we need some time to parse xaml  
									CreateItemCommand.Execute();

								}
							}))
					});
			}
		}

		#endregion
	}
}
