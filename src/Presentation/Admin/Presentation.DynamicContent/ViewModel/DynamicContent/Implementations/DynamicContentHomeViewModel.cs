using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Interactivity.InteractionRequest;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using VirtoCommerce.Client.Globalization;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.Foundation.Frameworks.Extensions;
using VirtoCommerce.Foundation.Marketing.Factories;
using VirtoCommerce.Foundation.Marketing.Model.DynamicContent;
using VirtoCommerce.Foundation.Marketing.Repositories;
using VirtoCommerce.Foundation.Security.Model;
using VirtoCommerce.ManagementClient.Core.Infrastructure;
using VirtoCommerce.ManagementClient.Core.Infrastructure.DataVirtualization;
using VirtoCommerce.ManagementClient.Core.Infrastructure.Navigation;
using VirtoCommerce.ManagementClient.Core.Infrastructure.Tiles;
using VirtoCommerce.ManagementClient.DynamicContent.ViewModel.DynamicContent.Interfaces;
using VirtoCommerce.ManagementClient.DynamicContent.ViewModel.Wizard.Interfaces;
using VirtoCommerce.ManagementClient.Localization;

namespace VirtoCommerce.ManagementClient.DynamicContent.ViewModel.DynamicContent.Implementations
{
    public class DynamicContentHomeViewModel : ViewModelBase, IDynamicContentHomeViewModel, IVirtualListLoader<IDynamicContentItemViewModel>, ISupportDelayInitialization
    {
        #region Dependencies

        private readonly IDynamicContentEntityFactory _entityFactory;
        private readonly IAuthenticationContext _authContext;
        private readonly IRepositoryFactory<IDynamicContentRepository> _dynamicContentRepository;
        private readonly IViewModelsFactory<ICreateDynamicContentItemViewModel> _wizardVmFactory;
        private readonly IViewModelsFactory<IDynamicContentItemViewModel> _itemVmFactory;
        private readonly NavigationManager _navManager;
        private readonly TileManager _tileManager;

        #endregion

        #region Constructor

        public DynamicContentHomeViewModel(
            IRepositoryFactory<IDynamicContentRepository> dynamicContentRepository,
            IDynamicContentEntityFactory entityFactory,
            IAuthenticationContext authContext,
            IViewModelsFactory<ICreateDynamicContentItemViewModel> wizardVmFactory,
            IViewModelsFactory<IDynamicContentItemViewModel> itemVmFactory,
            NavigationManager navManager,
            TileManager tileManager)
        {
            _dynamicContentRepository = dynamicContentRepository;
            _entityFactory = entityFactory;
            _authContext = authContext;
            _navManager = navManager;
            _tileManager = tileManager;
            _wizardVmFactory = wizardVmFactory;
            _itemVmFactory = itemVmFactory;

            CommonWizardDialogRequest = new InteractionRequest<Confirmation>();
            CommonConfirmRequest = new InteractionRequest<Confirmation>();
            CommonNotifyRequest = new InteractionRequest<Notification>();

            AddItemCommand = new DelegateCommand(RaiseCreateDynamicContentItemInteractionRequest);
            DuplicateItemCommand = new DelegateCommand<IList>(RaiseItemDuplicateInteractionRequest, x => x != null && x.Count > 0);
            DeleteItemCommand = new DelegateCommand<IList>(RaiseItemDeleteInteractionRequest, x => x != null && x.Count > 0);

            ClearFiltersCommand = new DelegateCommand(DoClearFilters);
            SearchItemsCommand = new DelegateCommand(DoSearchItems);

            ViewTitle = new ViewTitleBase
            {
                Title = "Marketing",
                SubTitle = "DYNAMIC CONTENT".Localize()
            };
            PopulateTiles();
        }

        #endregion

        #region Commands

        public DelegateCommand ClearFiltersCommand { get; private set; }
        public DelegateCommand SearchItemsCommand { get; private set; }

        public DelegateCommand<IList> DuplicateItemCommand { get; private set; }
        public DelegateCommand<IList> DeleteItemCommand { get; private set; }
        public DelegateCommand AddItemCommand { get; private set; }

        public InteractionRequest<Confirmation> CommonWizardDialogRequest { get; private set; }
        public InteractionRequest<Confirmation> CommonConfirmRequest { get; private set; }
        public InteractionRequest<Notification> CommonNotifyRequest { get; private set; }


        #endregion

        #region Commands Implementation

        private void DoSearchItems()
        {
            if (SelectedDynamicContentItems != null)
                SelectedDynamicContentItems.Refresh();
        }

        private void DoClearFilters()
        {
            SearchKeywordFilter = string.Empty;
            SearchFilterItemType = null;
            OnPropertyChanged("SearchFilterItemType");
            OnPropertyChanged("SearchKeywordFilter");
        }

        private void RaiseCreateDynamicContentItemInteractionRequest()
        {
            var item = CreateEntity<DynamicContentItem>();
            item.ContentTypeId = DynamicContentType.CategoryWithImage.ToString();

            var itemVM = _wizardVmFactory.GetViewModelInstance(
                new KeyValuePair<string, object>("item", item));

            CommonWizardDialogRequest.Raise(new ConditionalConfirmation
            {
                Title = "Create dynamic content item".Localize(),
                Content = itemVM
            }, (x) =>
            {
                if (x.Confirmed)
                {
                    using (var repository = _dynamicContentRepository.GetRepositoryInstance())
                    {
                        repository.Add(item);
                        repository.UnitOfWork.Commit();
                    }

                    SelectedDynamicContentItems.Refresh();
                }
            });
        }

        private void RaiseItemDuplicateInteractionRequest(IList selectedItemsList)
        {
            var selectionCount = selectedItemsList.Count;
            if (selectionCount > 0)
            {
                var selectedItems = selectedItemsList.Cast<VirtualListItem<IDynamicContentItemViewModel>>();
                var names = selectedItems.
                    Take(4).
                    Select(x => ((ViewModelBase)x.Data).DisplayName);

                var joinedNames = string.Join(", ", names);
                if (selectionCount > 4) joinedNames += "...";

                CommonConfirmRequest.Raise(new ConditionalConfirmation()
                {
                    Content = string.Format("Are you sure you want to duplicate '{0}'?".Localize(), joinedNames),
                    Title = "Action confirmation".Localize(null, LocalizationScope.DefaultCategory)
                },
                (x) =>
                {
                    if (x.Confirmed)
                    {
                        using (var repository = _dynamicContentRepository.GetRepositoryInstance())
                        {
                            selectedItems.
                                ToList().
                                ForEach(y => y.Data.Duplicate(repository));

                            repository.UnitOfWork.Commit();
                        }

                        SelectedDynamicContentItems.Refresh();
                    }
                });
            }
        }

        private void RaiseItemDeleteInteractionRequest(IList selectedItemsList)
        {
            var selectionCount = selectedItemsList.Count;
            if (selectionCount > 0)
            {
                var selectedItems = selectedItemsList.Cast<VirtualListItem<IDynamicContentItemViewModel>>();
                var names = selectedItems.
                    Take(4).
                    Select(x => ((ViewModelBase)x.Data).DisplayName);

                var joinedNames = string.Join(", ", names);
                if (selectionCount > 4) joinedNames += "...";
                var restrictList = new List<DynamicContentItem>();
                using (var repository = _dynamicContentRepository.GetRepositoryInstance())
                {
                    foreach (var item in selectedItems)
                    {
                        var content = (item.Data as ViewModelDetailBase<DynamicContentItem>).InnerItem;
                        if (repository.PublishingGroups.Expand(publishing => publishing.ContentItems).Where(x => x.ContentItems.Any(i => i.DynamicContentItemId == content.DynamicContentItemId)).Count() > 0)
                            restrictList.Add(content);
                    }
                    if (restrictList.Any())
                    {
                        var dynamicNames = string.Empty;
                        var prefix = restrictList.Count == 1 ? "Dynamic content".Localize() : "Dynamic contents".Localize();
                        restrictList.ForEach(x => dynamicNames = string.Join(Environment.NewLine, x.Name, dynamicNames));

                        var suffix = restrictList.Count == 1 ? "it's".Localize() : "they are".Localize();
                        CommonNotifyRequest.Raise(new Notification()
                            {
                                Content = string.Format("{0}{1} {2} can't be deleted as {3} used in Content Publishing".Localize(), prefix, Environment.NewLine, dynamicNames, suffix),
                                Title = "Error".Localize(null, LocalizationScope.DefaultCategory)
                            });
                    }
                }

                if (!restrictList.Any())
                {
                    CommonConfirmRequest.Raise(new ConditionalConfirmation()
                        {
                            Content = string.Format("Are you sure you want to delete '{0}'?".Localize(null, LocalizationScope.DefaultCategory), joinedNames),
                            Title = "Delete confirmation".Localize(null, LocalizationScope.DefaultCategory)
                        },
                                               async (x) =>
                                               {
                                                   if (x.Confirmed)
                                                   {
                                                       try
                                                       {
                                                           OnUIThread(() => { ShowLoadingAnimation = true; });
                                                           await Task.Run(() =>
                                                               {
                                                                   foreach (var item in selectedItems)
                                                                   {
                                                                       (item.Data as IViewModelDetailBase).Delete();
                                                                   }
                                                               });
                                                           SelectedDynamicContentItems.Refresh();
                                                       }
                                                       finally
                                                       {
                                                           OnUIThread(() => { ShowLoadingAnimation = false; });
                                                       }
                                                   }

                                               });
                }
            }
        }

        #endregion

        #region Properties

        public object SearchFilterItemType { get; set; }
        public string SearchKeywordFilter { get; set; }

        private ICollectionView _selectedDynamicContentItems;
        public ICollectionView SelectedDynamicContentItems
        {
            get
            {
                return _selectedDynamicContentItems;
            }
            private set
            {
                _selectedDynamicContentItems = value;
                OnPropertyChanged();
            }
        }


        private IViewModel _selectedDynamicContentItem;
        public IViewModel SelectedDynamicContentItem
        {
            get
            {
                return _selectedDynamicContentItem;
            }
            set
            {
                _selectedDynamicContentItem = value;
                SelectedDynamicContentItems.Refresh();

                OnPropertyChanged();
            }
        }

        #endregion

        #region IVirtualListLoader<IDynamicContentViewModel> Members

        public bool CanSort
        {
            get { return false; }
        }

        public IList<IDynamicContentItemViewModel> LoadRange(int startIndex, int count, SortDescriptionCollection sortDescriptions, out int overallCount)
        {
            var retVal = new List<IDynamicContentItemViewModel>();

            using (var repository = _dynamicContentRepository.GetRepositoryInstance())
            {
                var query = repository.Items;

                if (SearchFilterItemType != null)
                    query = query.Where(x => x.ContentTypeId == SearchFilterItemType.ToString());

                if (!string.IsNullOrEmpty(SearchKeywordFilter))
                    query = query.Where(x => x.Name.Contains(SearchKeywordFilter) || x.Description.Contains(SearchKeywordFilter));

                overallCount = query.Count();
                var items = query.OrderBy(x => x.DynamicContentItemId).Skip(startIndex).Take(count).ToList();
                retVal.AddRange(items.Select(item => _itemVmFactory.GetViewModelInstance(new KeyValuePair<string, object>("item", item))));
            }

            return retVal;
        }

        #endregion

        #region ISupportDelayInitialization Members

        public void InitializeForOpen()
        {
            if (SelectedDynamicContentItems == null)
            {
                OnUIThread(() => SelectedDynamicContentItems = new VirtualList<IDynamicContentItemViewModel>(this, 25, SynchronizationContext.Current));
            }
        }

        #endregion

        #region Private members

        private T CreateEntity<T>()
        {
            var catalogEntityName = _entityFactory.GetEntityTypeStringName(typeof(T));
            return (T)_entityFactory.CreateEntityForType(catalogEntityName);
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
                    IdTile = "ActiveAds",
                    TileTitle = "Active ads",
                    TileCategory = NavigationNames.ModuleName,
                    Order = 3,
                    IdColorSchema = TileColorSchemas.Schema3,
                    NavigateCommand = new DelegateCommand(() => NavigateToTabPage(NavigationNames.HomeNameDynamicContent)),
                    Refresh = async (tileItem) =>
                    {
                        try
                        {
                            using (var repository = _dynamicContentRepository.GetRepositoryInstance())
                            {
                                if (tileItem is NumberTileItem)
                                {
                                    var query = await Task.Factory.StartNew(() => repository.Items.Count());
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

            if (_authContext.CheckPermission(PredefinedPermissions.MarketingDynamic_ContentManage))
            {
                _tileManager.AddTile(new NumberTileItem()
                    {
                        IdModule = NavigationNames.MenuName,
                        IdTile = "TotalAds",
                        TileTitle = "Total ads",
                        TileCategory = NavigationNames.ModuleName,
                        Order = 3,
                        IdColorSchema = TileColorSchemas.Schema2,
                        NavigateCommand = new DelegateCommand(() => NavigateToTabPage(NavigationNames.HomeNameDynamicContent)),
                        Refresh = async (tileItem) =>
                            {
                                try
                                {
                                    using (var repository = _dynamicContentRepository.GetRepositoryInstance())
                                    {
                                        if (tileItem is NumberTileItem)
                                        {
                                            var query = await Task.Factory.StartNew(() => repository.Items.Count());
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

            if (_authContext.CheckPermission(PredefinedPermissions.MarketingDynamic_ContentManage))
            {
                _tileManager.AddTile(new IconTileItem()
                    {
                        IdModule = NavigationNames.MenuName,
                        IdTile = "NewAd",
                        TileIconSource = "Icon_Dynamic",
                        TileTitle = "New ad",
                        TileCategory = NavigationNames.ModuleName,
                        Order = 5,
                        IdColorSchema = TileColorSchemas.Schema1,
                        NavigateCommand = new DelegateCommand(() => OnUIThread(async () =>
                            {
                                if (NavigateToTabPage(NavigationNames.HomeNameDynamicContent))
                                {
                                    await Task.Run(() => Thread.Sleep(300)); // we need some time to parse xaml  
                                    AddItemCommand.Execute();

                                }
                            }))
                    });
            }
        }

        #endregion
    }
}
