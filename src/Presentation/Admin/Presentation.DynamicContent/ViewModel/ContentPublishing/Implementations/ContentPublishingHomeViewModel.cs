using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.Foundation.Security.Model;
using VirtoCommerce.ManagementClient.Core.Infrastructure.Navigation;
using VirtoCommerce.ManagementClient.Core.Infrastructure.Tiles;
using VirtoCommerce.ManagementClient.Core.Infrastructure;
using VirtoCommerce.ManagementClient.Core.Infrastructure.DataVirtualization;
using Microsoft.Practices.Prism.Interactivity.InteractionRequest;
using Microsoft.Practices.Prism.Commands;
using System.ComponentModel;
using VirtoCommerce.Foundation.Marketing.Factories;
using VirtoCommerce.Foundation.Marketing.Repositories;
using System.Threading;
using VirtoCommerce.ManagementClient.DynamicContent.ViewModel.ContentPublishing.Interfaces;
using System.Collections;
using VirtoCommerce.Foundation.Marketing.Model.DynamicContent;
using VirtoCommerce.ManagementClient.DynamicContent.ViewModel.Wizard.Interfaces;

namespace VirtoCommerce.ManagementClient.DynamicContent.ViewModel.ContentPublishing.Implementations
{
	public class ContentPublishingHomeViewModel : ViewModelHomeEditableBase<DynamicContentPublishingGroup>, IContentPublishingHomeViewModel, IVirtualListLoader<IContentPublishingItemViewModel>, ISupportDelayInitialization
	{
		#region Dependencies

		private readonly IDynamicContentEntityFactory _entityFactory;
		private readonly IAuthenticationContext _authContext;
		private readonly IViewModelsFactory<ICreateContentPublishingItemViewModel> _wizardVmFactory;
		private readonly IViewModelsFactory<IContentPublishingItemViewModel> _itemVmFactory;
		private readonly IRepositoryFactory<IDynamicContentRepository> _repositoryFactory;
		private readonly NavigationManager _navManager;
		private readonly TileManager _tileManager;

		#endregion

		#region Constructor

		public ContentPublishingHomeViewModel(IRepositoryFactory<IDynamicContentRepository> repositoryFactory, IDynamicContentEntityFactory entityFactory,
			IViewModelsFactory<ICreateContentPublishingItemViewModel> wizardVmFactory, IViewModelsFactory<IContentPublishingItemViewModel> itemVmFactory, IAuthenticationContext authContext,
			NavigationManager navManager, TileManager tileManager)
		{
			_entityFactory = entityFactory;
			_authContext = authContext;
			_wizardVmFactory = wizardVmFactory;
			_itemVmFactory = itemVmFactory;
			_navManager = navManager;
			_repositoryFactory = repositoryFactory;
			_tileManager = tileManager;

			CommonNotifyRequest = new InteractionRequest<Notification>();

			ItemDuplicateCommand = new DelegateCommand<IList>(RaiseItemDuplicateInteractionRequest, x => x != null && x.Count > 0);

			ClearFiltersCommand = new DelegateCommand(DoClearFilters);

			ViewTitle = new ViewTitleBase
			{
                Title = "Marketing",
                SubTitle = "CONTENT PUBLISHING".Localize()
			};
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
			var item = _entityFactory.CreateEntity<DynamicContentPublishingGroup>();

			var itemVM = _wizardVmFactory.GetViewModelInstance(
				new KeyValuePair<string, object>("item", item));

			var confirmation = new Confirmation()
			{
				Content = itemVM,
                Title = "Create content publishing group".Localize()
			};

			ItemAdd(confirmation);
		}

		protected override void RaiseItemDeleteInteractionRequest(IList selectedItemsList)
		{
			var selectedItems = selectedItemsList.Cast<VirtualListItem<IContentPublishingItemViewModel>>();
			ItemDelete(selectedItems.Select(x => (IViewModelDetailBase)x.Data).ToList());
		}

		#endregion

		#region IContentPublishingHomeViewModel members

		private IViewModel _selectedContentPublishingItem;
		public IViewModel SelectedContentPublishingItem
		{
			get
			{
				return _selectedContentPublishingItem;
			}
			set
			{
				_selectedContentPublishingItem = value;
				ListItemsSource.Refresh();

				OnPropertyChanged();
			}
		}

		public DelegateCommand<IList> ItemDuplicateCommand { get; private set; }

		#endregion

		#region Public Properties

		public DateTime? SearchFilterDateFrom { get; set; }
		public DateTime? SearchFilterDateTo { get; set; }
		public string SearchFilterKeyword { get; set; }

		#endregion

		#region Commands and Requests

		public DelegateCommand ClearFiltersCommand { get; private set; }
		public InteractionRequest<Notification> CommonNotifyRequest { get; private set; }

		#endregion

		#region IVirtualListLoader<IContentPublishingViewModel> Members

		public bool CanSort
		{
			get { return false; }
		}

		public IList<IContentPublishingItemViewModel> LoadRange(int startIndex, int count, SortDescriptionCollection sortDescriptions, out int overallCount)
		{
			var retVal = new List<IContentPublishingItemViewModel>();

			using (var repository = _repositoryFactory.GetRepositoryInstance())
			{
				var query = repository.PublishingGroups;

				if (!String.IsNullOrEmpty(SearchFilterKeyword))
					query = query.Where(x =>
						x.Name.Contains(SearchFilterKeyword) ||
						x.Description.Contains(SearchFilterKeyword)
						//|| x.ContentPlaces.Select(z => z.ContentPlace).Any(y => y.Name.Contains(SearchFilterKeyword))
						//|| x.ContentItems.Select(z => z.ContentItem).Any(y => y.Name.Contains(SearchFilterKeyword))
						);

				if (SearchFilterDateFrom != null)
					query = query.Where(x => x.StartDate == null || x.StartDate >= SearchFilterDateFrom);

				if (SearchFilterDateTo != null)
					query = query.Where(x => x.EndDate == null || x.EndDate <= SearchFilterDateTo);

				overallCount = query.Count();
				var items = query.OrderBy(x => x.DynamicContentPublishingGroupId).Skip(startIndex).Take(count).ToList();
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
				OnUIThread(
					() => ListItemsSource = new VirtualList<IContentPublishingItemViewModel>(this, 25, SynchronizationContext.Current));
			}
		}

		#endregion

		#region private members

		private void DoClearFilters()
		{
			SearchFilterKeyword = null;
			SearchFilterDateFrom = SearchFilterDateTo = null;
			OnPropertyChanged("SearchFilterKeyword");
			OnPropertyChanged("SearchFilterDateFrom");
			OnPropertyChanged("SearchFilterDateTo");
		}

		private void RaiseItemDuplicateInteractionRequest(IList selectedItemsList)
		{
			var selectedItems = selectedItemsList.Cast<VirtualListItem<IContentPublishingItemViewModel>>();
			ItemDuplicate(selectedItems.Select(x => (IViewModelDetailBase) x.Data).ToList());
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
			if (_authContext.CheckPermission(PredefinedPermissions.MarketingContent_PublishingManage))
			_tileManager.AddTile(new IconTileItem()
			{
				IdModule = NavigationNames.MenuName,
				IdTile = "PublishAd",
				TileIconSource = "Icon_ContentPublishing",
                TileTitle = "Publish ad",
                TileCategory = NavigationNames.ModuleName,
				Order = 6,
				IdColorSchema = TileColorSchemas.Schema4,
				NavigateCommand = new DelegateCommand(() => OnUIThread(async () =>
				{
					if (NavigateToTabPage(NavigationNames.HomeNameContentPublishing))
					{
						await Task.Run(() => Thread.Sleep(300));  // we need some time to parse xaml  
						//AddItemCommand.Execute();
					}
				}))
			});

		}

		#endregion

	}
}
