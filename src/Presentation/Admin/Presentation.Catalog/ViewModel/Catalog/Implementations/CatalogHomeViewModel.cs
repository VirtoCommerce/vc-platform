using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Interactivity.InteractionRequest;
using RequestEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using VirtoCommerce.Client.Globalization;
using VirtoCommerce.Foundation.AppConfig.Factories;
using VirtoCommerce.Foundation.AppConfig.Model;
using VirtoCommerce.Foundation.AppConfig.Repositories;
using VirtoCommerce.Foundation.Catalogs.Factories;
using VirtoCommerce.Foundation.Catalogs.Model;
using VirtoCommerce.Foundation.Catalogs.Repositories;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.Foundation.Frameworks.Extensions;
using VirtoCommerce.Foundation.Importing.Model;
using VirtoCommerce.Foundation.Importing.Repositories;
using VirtoCommerce.Foundation.Security.Model;
using VirtoCommerce.ManagementClient.Catalog.Model;
using VirtoCommerce.ManagementClient.Catalog.Services;
using VirtoCommerce.ManagementClient.Catalog.ViewModel.Catalog.Interfaces;
using VirtoCommerce.ManagementClient.Catalog.ViewModel.Titles;
using VirtoCommerce.ManagementClient.Catalog.ViewModel.Wizard;
using VirtoCommerce.ManagementClient.Core.Infrastructure;
using VirtoCommerce.ManagementClient.Core.Infrastructure.DataVirtualization;
using VirtoCommerce.ManagementClient.Core.Infrastructure.DragDrop;
using VirtoCommerce.ManagementClient.Core.Infrastructure.Navigation;
using VirtoCommerce.ManagementClient.Core.Infrastructure.Tiles;
using VirtoCommerce.ManagementClient.Localization;
using catalogModel = VirtoCommerce.Foundation.Catalogs.Model;

namespace VirtoCommerce.ManagementClient.Catalog.ViewModel.Catalog.Implementations
{
    public class CatalogHomeViewModel : ViewModelHomeEditableBase<Item>, ICatalogHomeViewModel, IVirtualListLoader<IItemViewModel>, ISupportDelayInitialization, IDropTarget
    {
        #region Dependencies

        private readonly IAuthenticationContext _authContext;
        private readonly ICatalogEntityFactory _entityFactory;
        private readonly IRepositoryFactory<ICatalogRepository> _catalogRepository;
        private readonly IRepositoryFactory<IImportRepository> _importRepository;
        private readonly IRepositoryFactory<IAppConfigRepository> _seoRepository;
        private readonly IAppConfigEntityFactory _seoFactory;
        private readonly ITitleHomeCaptionViewModel _viewTitle;
        private readonly IViewModelsFactory<IQueryViewModel> _queryVmFactory;
        private readonly IViewModelsFactory<ICreateCategoryViewModel> _wizardCategoryVmFactory;
        private readonly IViewModelsFactory<IItemTypeSelectionStepViewModel> _selectionVmFactory;
        private readonly IViewModelsFactory<ICreateCatalogViewModel> _wizardCatalogVmFactory;
        private readonly IViewModelsFactory<ICreateVirtualCatalogViewModel> _wizardVirtualCatalogVmFactory;
        private readonly IViewModelsFactory<ISearchCategoryViewModel> _searchCategoryVmFactory;
        private readonly IViewModelsFactory<ICreateItemViewModel> _wizardItemVmFactory;
        private readonly IViewModelsFactory<IItemTypeSelectionStepViewModel> _itemTypeVmFactory;
        private readonly IViewModelsFactory<IItemViewModel> _itemVmFactory;
        private readonly IViewModelsFactory<ITreeCatalogViewModel> _treeCatalogVmFactory;
        private readonly IViewModelsFactory<ITreeVirtualCatalogViewModel> _treeVirtualCatalogVmFactory;
        private readonly NavigationManager _navManager;
        private readonly TileManager _tileManager;

        #endregion

        #region private members

        private readonly CatalogMainViewModel _parentViewModel;

        private bool _isInSearchMode;
        private bool _isInSearchAllMode;
        private ItemFilter _currentFilter;

        #endregion

#if DESIGN
        public CatalogHomeViewModel()
        {
            var catalogRepository = new MockCatalogService();

            //Get all catalogs
            Action getAllCatalogsAction = () =>
            {
                var catalogs = catalogRepository.Catalogs.OrderBy(x => x.Name).ToList();
                Action<List<CatalogBase>> callback = EndSearchCatalogs;
                Application.Current.Dispatcher.Invoke(callback, catalogs);
            };

            getAllCatalogsAction.BeginInvoke(null, null);
        }
#endif
        #region Constructor

        public CatalogHomeViewModel(CatalogMainViewModel parentViewModel, IAppConfigEntityFactory seoFactory, IRepositoryFactory<IAppConfigRepository> seoRepository, IRepositoryFactory<ICatalogRepository> catalogRepository,
            IRepositoryFactory<IImportRepository> importRepository, IViewModelsFactory<ICreateCategoryViewModel> wizardCategoryVmFactory,
            ITitleHomeCaptionViewModel captionVm, IViewModelsFactory<IQueryViewModel> queryVmFactory, IViewModelsFactory<IItemTypeSelectionStepViewModel> selectionVmFactory,
            IViewModelsFactory<ICreateCatalogViewModel> wizardCatalogVmFactory, IViewModelsFactory<ICreateVirtualCatalogViewModel> wizardVirtualCatalogVmFactory,
            IViewModelsFactory<ISearchCategoryViewModel> searchCategoryVmFactory, IViewModelsFactory<ICreateItemViewModel> wizardItemVmFactory,
            IViewModelsFactory<IItemTypeSelectionStepViewModel> itemTypeVmFactory, IViewModelsFactory<IItemViewModel> itemVmFactory,
            IViewModelsFactory<ITreeCatalogViewModel> treeCatalogVmFactory, IViewModelsFactory<ITreeVirtualCatalogViewModel> treeVirtualCatalogVmFactory,
            ICatalogEntityFactory entityFactory, IAuthenticationContext authContext, NavigationManager navManager, TileManager tileManager)
        {
            _parentViewModel = parentViewModel;
            _entityFactory = entityFactory;
            _authContext = authContext;
            _catalogRepository = catalogRepository;
            _importRepository = importRepository;
            _seoRepository = seoRepository;
            _seoFactory = seoFactory;
            _navManager = navManager;
            _tileManager = tileManager;

            _queryVmFactory = queryVmFactory;
            _wizardCategoryVmFactory = wizardCategoryVmFactory;
            _selectionVmFactory = selectionVmFactory;
            _wizardCatalogVmFactory = wizardCatalogVmFactory;
            _wizardVirtualCatalogVmFactory = wizardVirtualCatalogVmFactory;
            _searchCategoryVmFactory = searchCategoryVmFactory;
            _wizardItemVmFactory = wizardItemVmFactory;
            _itemTypeVmFactory = itemTypeVmFactory;
            _itemVmFactory = itemVmFactory;
            _treeCatalogVmFactory = treeCatalogVmFactory;
            _treeVirtualCatalogVmFactory = treeVirtualCatalogVmFactory;

            CommandsInits();
            PopulateTiles();

            _viewTitle = captionVm;

            if (_viewTitle != null)
            {
                _viewTitle.Title = "Catalogs".Localize();
            }
        }

        private void CommandsInits()
        {
            CommonNotifyRequest = new InteractionRequest<Notification>();

            QueryCreateCommand = new DelegateCommand(RaiseItemFilterCreateInteractionRequest);
            QueryRunCommand = new DelegateCommand<ItemFilter>(RaiseRunQueryInteractionRequest);
            QueryEditCommand = new DelegateCommand<ItemFilter>(RaiseItemFilterEditInteractionRequest);
            QueryDeleteCommand = new DelegateCommand<ItemFilter>(RaiseItemFilterDeleteInteractionRequest);

            SearchAllItemsCommand = new DelegateCommand(DoSearchAllItems);
            SearchFilterCatalogs = new ObservableCollection<CatalogBase>();
            ClearFiltersCommand = new DelegateCommand(DoClearFilters);
            RefreshTreeItemsCommand = new DelegateCommand(DoRefreshTreeItems);
            TreeItemDeleteCommand = new DelegateCommand<IViewModel>(RaiseTreeItemDeleteInteractionRequest, HasManageTreeItemsPermission);

            CreateCategoryCommand = new DelegateCommand(RaiseCreateCategoryInteractionRequest, () => SelectedCatalogItem != null && _authContext.CheckPermission(PredefinedPermissions.CatalogCategoriesManage) && !(SelectedCatalogItem is ITreeCategoryViewModel && ((ITreeCategoryViewModel)SelectedCatalogItem).InnerItem is LinkedCategory));
            CreateCatalogCommand = new DelegateCommand(RaiseCreateCatalogInteractionRequest, () => _authContext.CheckPermission(PredefinedPermissions.CatalogCatalogsManage));
            CreateLinkedCategoryCommand = new DelegateCommand(RaiseCreateLinkedCategoryInteractionRequest, () => _authContext.CheckPermission(PredefinedPermissions.CatalogVirtual_CatalogsManage) && CanExecuteCreateLinkedCategoryCommand(SelectedCatalogItem));
            RefreshItemsCommand = new DelegateCommand(RaiseRefreshCommand, () => SelectedCatalogItem != null);

            ItemMoveCommand = new DelegateCommand<IList>(RaiseItemMoveInteractionRequest, x => _authContext.CheckPermission(PredefinedPermissions.CatalogItemsManage) && x != null && x.Count > 0);
            ItemDuplicateCommand = new DelegateCommand<IList>(RaiseItemDuplicateInteractionRequest, x => _authContext.CheckPermission(PredefinedPermissions.CatalogItemsManage) && x != null && x.Count > 0);

            TreeViewSelectedItemChangedCommand = new DelegateCommand<object>(TreeViewSelectedItemChanged);
        }

        #endregion

        #region Commands

        public DelegateCommand<object> TreeViewSelectedItemChangedCommand { get; private set; }

        #endregion

        #region Commands Implementation

        private void TreeViewSelectedItemChanged(object item)
        {
            SelectedTreeViewItem = item;
        }

        private void RaiseRunQueryInteractionRequest(ItemFilter filter)
        {
            _currentFilter = filter;
            ListItemsSource.Refresh();
            RefreshCaption();
        }

        private void RaiseItemFilterCreateInteractionRequest()
        {
            var filter = new ItemFilter();
            if (RaiseItemFilterEditInteractionRequest(filter, "Create Search Filter".Localize()))
            {
                AllQueries.Add(filter);
                Virtoway.WPF.State.ElementStateOperations.SetPropertyValue("CustomItemsQuery", filter.Name, filter.StringExpression);
            }
        }

        private void RaiseItemFilterEditInteractionRequest(ItemFilter originalItem)
        {
            var item = new ItemFilter { Name = originalItem.Name, StringExpression = originalItem.StringExpression };
            if (RaiseItemFilterEditInteractionRequest(item, "Edit item filter".Localize()))
            {
                Virtoway.WPF.State.ElementStateOperations.SetPropertyValue("CustomItemsQuery", originalItem.Name, null);
                // copy all values to original:
                // originalItem.InjectFrom<CloneInjection>(item);
                originalItem.Expression = item.Expression;
                originalItem.Name = item.Name;
                originalItem.StringExpression = item.StringExpression;
                Virtoway.WPF.State.ElementStateOperations.SetPropertyValue("CustomItemsQuery", originalItem.Name, originalItem.StringExpression);
            }
        }

        private bool RaiseItemFilterEditInteractionRequest(ItemFilter item, string title)
        {
            var result = false;
            var itemVM = _queryVmFactory.GetViewModelInstance(new System.Collections.Generic.KeyValuePair<string, object>("item", item));

            CommonConfirmRequest.Raise(new ConditionalConfirmation(itemVM.Validate)
            {
                Content = itemVM,
                Title = title
            },
            (x) =>
            {
                result = x.Confirmed;
                if (result)
                {
                    item.Expression = itemVM.GetCompleteExpression();
                }
            });
            return result;
        }

        private void RaiseItemFilterDeleteInteractionRequest(ItemFilter selectedItem)
        {
            if (selectedItem != null)
            {
                CommonConfirmRequest.Raise(new ConditionalConfirmation()
                {
                    Content = string.Format("Are you sure you want to delete filter '{0}'?".Localize(), selectedItem.Name),
                    Title = "Delete".Localize(null, LocalizationScope.DefaultCategory)
                },
                (x) =>
                {
                    if (x.Confirmed)
                    {
                        Virtoway.WPF.State.ElementStateOperations.SetPropertyValue("CustomItemsQuery", selectedItem.Name, null);
                        AllQueries.Remove(selectedItem);
                    }
                });
            }
        }

        private void RaiseCreateCategoryInteractionRequest()
        {
            if (SelectedCatalogItem != null)
            {
                var category = CreateCatalogEntity<Category>();

                if (SelectedCatalogItem is ITreeCategoryViewModel)
                {
                    var parentCategory = (ITreeCategoryViewModel)SelectedCatalogItem;
                    category.ParentCategory = parentCategory.InnerItem;
                    category.ParentCategoryId = parentCategory.InnerItem.CategoryId;
                }
                category.CatalogId = GetCatalogId(SelectedCatalogItem);
                category.Catalog = GetCatalog(SelectedCatalogItem);

                var wizardViewModel = _wizardCategoryVmFactory.GetViewModelInstance(new System.Collections.Generic.KeyValuePair<string, object>("item", category));

                var confirmation = new Confirmation { Title = "Create Category".Localize(), Content = wizardViewModel };
                ItemAdd(confirmation, category);
            }
        }

        private void RaiseCreateCatalogInteractionRequest()
        {
            if (_authContext.CheckPermission(PredefinedPermissions.CatalogCatalogsManage) &&
                _authContext.CheckPermission(PredefinedPermissions.CatalogVirtual_CatalogsManage))
            {
                var allAvailableOptions = new[]
					{
						new ItemTypeSelectionModel("Catalog".Localize(),"A central location to manage your store merchandise, create catalog for a brand, product line or a particular supplier.".Localize()),
						new ItemTypeSelectionModel("Virtual Catalog".Localize(),"A subset of products and categories found in master catalogs.".Localize())
					};
                var itemVM =
                    _selectionVmFactory.GetViewModelInstance(new System.Collections.Generic.KeyValuePair<string, object>("allAvailableOptions", allAvailableOptions));
                // show selection step
                CommonConfirmRequest.Raise(
                    new ConditionalConfirmation(() => !string.IsNullOrEmpty(itemVM.SelectedItemType)) { Content = itemVM, Title = "Select Catalog type".Localize() }, (x) =>
                            {
                                if (x.Confirmed)
                                {
                                    if (itemVM.SelectedItemType == allAvailableOptions[0].Value)
                                    {
                                        CreateCatalog();
                                    }
                                    else if (itemVM.SelectedItemType == allAvailableOptions[1].Value)
                                    {
                                        CreateVirtualCatalog();
                                    }
                                }
                            });
            }
            else if (_authContext.CheckPermission(PredefinedPermissions.CatalogCatalogsManage))
            {
                CreateCatalog();
            }
            else if (_authContext.CheckPermission(PredefinedPermissions.CatalogVirtual_CatalogsManage))
            {
                CreateVirtualCatalog();
            }
        }

        private void CreateCatalog()
        {
            var item = CreateCatalogEntity<catalogModel.Catalog>();
            var itemVM = _wizardCatalogVmFactory.GetViewModelInstance(new System.Collections.Generic.KeyValuePair<string, object>("item", item));
            var confirmation = new Confirmation { Content = itemVM, Title = "Create Catalog".Localize() };
            ItemAdd(confirmation, item);
        }

        private void CreateVirtualCatalog()
        {
            var item = CreateCatalogEntity<VirtualCatalog>();
            var itemVM = _wizardVirtualCatalogVmFactory.GetViewModelInstance(new System.Collections.Generic.KeyValuePair<string, object>("item", item));
            var confirmation = new Confirmation { Content = itemVM, Title = "Create Virtual Catalog".Localize() };
            ItemAdd(confirmation, item);
        }

        private void RaiseCreateLinkedCategoryInteractionRequest()
        {
            // ParameterOverride ParameterValue cannot be null
            var itemVM = _searchCategoryVmFactory.GetViewModelInstance(
                new System.Collections.Generic.KeyValuePair<string, object>("catalogInfo", string.Empty)
                );
            itemVM.SearchModifier = SearchCategoryModifier.RealCatalogsOnly;
            itemVM.InitializeForOpen();
            CommonConfirmRequest.Raise(
                new ConditionalConfirmation(() => itemVM.SelectedItem != null) { Content = itemVM, Title = "Select linked Category".Localize() },
                (x) =>
                {
                    if (x.Confirmed)
                    {
                        var item = CreateCatalogEntity<LinkedCategory>();
                        item.CatalogId = GetCatalogId(SelectedCatalogItem);
                        if (SelectedCatalogItem is ITreeCategoryViewModel)
                            item.ParentCategoryId = ((ITreeCategoryViewModel)SelectedCatalogItem).InnerItem.CategoryId;

                        var category = itemVM.SelectedItem;
                        item.LinkedCategoryId = category.CategoryId;
                        item.LinkedCatalogId = category.CatalogId;
                        item.Code = Guid.NewGuid().ToString();
                        item.Priority = 0;
                        item.IsActive = true;

                        using (var repository = _catalogRepository.GetRepositoryInstance())
                        {
                            repository.Add(item);
                            repository.UnitOfWork.Commit();
                        }

                        var catalogParentEntity = (CatalogEntityViewModelBase)SelectedCatalogItem;
                        if (catalogParentEntity.IsExpanded)
                            catalogParentEntity.Refresh();
                        else
                            catalogParentEntity.IsExpanded = true;
                    }
                });
        }

        private bool CanExecuteCreateLinkedCategoryCommand(IViewModel parentVM)
        {
            // must be in Virtual Catalog and not in Linked category
            var result = GetCatalog(parentVM) is VirtualCatalog;
            var categoryVM = parentVM as ITreeCategoryViewModel;
            while (result && categoryVM != null)
            {
                result = !(categoryVM.InnerItem is LinkedCategory);
                categoryVM = categoryVM.Parent as ITreeCategoryViewModel;
            }

            return result;
        }

        private void RaiseItemCreateInteractionRequest<T>(string itemTitle) where T : Item
        {
            var item = CreateCatalogEntity<T>();
            item.CatalogId = GetCatalogId(SelectedCatalogItem);
            item.Catalog = GetCatalog(SelectedCatalogItem);
            item.MinQuantity = 1;

            var itemVM = _wizardItemVmFactory.GetViewModelInstance(
                new System.Collections.Generic.KeyValuePair<string, object>("item", item),
                new System.Collections.Generic.KeyValuePair<string, object>("parentEntityVM", SelectedCatalogItem));

            var confirmation = new Confirmation { Content = itemVM, Title = string.Format("Create {0}".Localize(), itemTitle) };
            ItemAdd(confirmation, item);
        }

        private void RaiseItemMoveInteractionRequest(IList selectedItemsList)
        {
            // initial checks
            if (selectedItemsList.Count == 0
                && (_isInSearchMode
                    || !(SelectedCatalogItem is ITreeCategoryViewModel)
                    || GetCatalog(SelectedCatalogItem) is VirtualCatalog))
            {
                CommonNotifyRequest.Raise(new Notification
                {
                    Content = "Selection is not valid.".Localize(),
                    Title = "Error".Localize(null, LocalizationScope.DefaultCategory)
                });
                return;
            }

            // ParameterOverride ParameterValue cannot be null
            var catalogId = selectedItemsList.Count > 0 ? string.Empty : GetCatalogId(SelectedCatalogItem);
            var itemVM = _searchCategoryVmFactory.GetViewModelInstance(
                new System.Collections.Generic.KeyValuePair<string, object>("catalogInfo", catalogId)
                );
            itemVM.SearchModifier = SearchCategoryModifier.RealCatalogsOnly | SearchCategoryModifier.UserCanChangeSearchCatalog;
            itemVM.InitializeForOpen();
            CommonConfirmRequest.Raise(
                new ConditionalConfirmation(() => itemVM.SelectedItem != null) { Content = itemVM, Title = "Select item's Category".Localize() },
                (xx) =>
                {
                    if (xx.Confirmed)
                    {
                        string itemsText;
                        Category oldCategory = null;
                        if (selectedItemsList.Count > 0)
                        {
                            itemsText = string.Format("{0} item(s)".Localize(), selectedItemsList.Count);
                        }
                        else
                        {
                            oldCategory = (Category)((ITreeCategoryViewModel)SelectedCatalogItem).InnerItem;
                            itemsText = string.Format("all items from '{0}'".Localize(), oldCategory.Name);
                        }
                        string confirmationContent = string.Format("Are you sure you want to move {0} to Category '{1}'?".Localize(), itemsText, itemVM.SelectedItem.Name);
                        CommonConfirmRequest.Raise(new ConditionalConfirmation(() => true)
                        {
                            Title = "Confirmation".Localize(null, LocalizationScope.DefaultCategory),
                            Content = confirmationContent
                        }, xxx =>
                        {
                            if (xxx.Confirmed)
                            {
                                using (var repository = _catalogRepository.GetRepositoryInstance())
                                {
                                    var selectedItems = selectedItemsList.Cast<VirtualListItem<IItemViewModel>>();
                                    selectedItems.ToList().ForEach(y =>
                                    {
                                        var item = y.Data.InnerItem;
                                        item = repository.Items.Where(x => x.ItemId == item.ItemId).Expand(x => x.CategoryItemRelations).First();

                                        // Item can be only in 1 category in a real catalog
                                        var relation = item.CategoryItemRelations.FirstOrDefault(x => x.CatalogId == item.CatalogId);
                                        if (relation == null)
                                        {
                                            relation = CreateCatalogEntity<CategoryItemRelation>();
                                            relation.ItemId = item.ItemId;
                                            item.CategoryItemRelations.Add(relation);
                                        }

                                        item.CatalogId = itemVM.SelectedItem.CatalogId;
                                        relation.CatalogId = item.CatalogId;
                                        relation.CategoryId = itemVM.SelectedItem.CategoryId;
                                    });

                                    repository.UnitOfWork.Commit();
                                }

                                ListItemsSource.Refresh();
                            }
                        });
                    }
                });
        }

        private void RaiseItemDuplicateInteractionRequest(IList selectedItemsList)
        {
            var selectedItems = selectedItemsList.Cast<VirtualListItem<IItemViewModel>>();
            ItemDuplicate(selectedItems.Select(x => (IViewModelDetailBase)x.Data).ToList());
        }

        private void RaiseTreeItemDeleteInteractionRequest(IViewModel selectedItem)
        {
            var baseSelectedItem = selectedItem as CatalogEntityViewModelBase;
            if (baseSelectedItem != null)
            {
                Action onAfterDelete = () =>
                {
                    if (baseSelectedItem is ITreeCatalogViewModel || baseSelectedItem is ITreeVirtualCatalogViewModel)
                        RefreshTreeItemsCommand.Execute();

                    //ListItemsSource.Refresh();
                };

                using (var repository = _catalogRepository.GetRepositoryInstance())
                {
                    baseSelectedItem.Delete(repository, CommonConfirmRequest, CommonNotifyRequest, onAfterDelete);
                }
            }
        }

        private void DoSearchAllItems()
        {
            _isInSearchAllMode = !string.IsNullOrEmpty(SearchFilterAll);
            ListItemsSource.Refresh();
            RefreshCaption();
        }

        private void DoClearFilters()
        {
            // _isInSearchMode = false;
            SearchFilterItemType = SearchFilterName = SearchFilterCode = null;
            SearchFilterCatalog = null;
            OnPropertyChanged("SearchFilterName");
            OnPropertyChanged("SearchFilterCode");
            OnPropertyChanged("SearchFilterItemType");
            OnPropertyChanged("SearchFilterCatalog");
        }

        private void DoRefreshTreeItems()
        {
            Action getAllCatalogsAction = () =>
            {
                using (var repository = _catalogRepository.GetRepositoryInstance())
                {
                    var catalogs = repository.Catalogs.OrderBy(x => x.Name).ToList();
                    OnUIThread(() => { EndSearchCatalogs(catalogs); });
                }
            };
            getAllCatalogsAction.BeginInvoke(null, null);
        }

        #endregion

        #region ViewModelHomeEditableBase

        protected override bool CanItemAddExecute()
        {
            return _authContext.CheckPermission(PredefinedPermissions.CatalogItemsManage) && GetCatalog(SelectedCatalogItem) is catalogModel.Catalog;
        }

        protected override bool CanItemDeleteExecute(IList x)
        {
            return _authContext.CheckPermission(PredefinedPermissions.CatalogItemsManage) && x != null && x.Count > 0;
        }

        protected override void RaiseItemAddInteractionRequest()
        {
            var allAvailableOptions = new[]
				{
					new ItemTypeSelectionModel("Variation/SKU","Represents orderable item of merchandise for sale. Contains prices and inventory information."),
					new ItemTypeSelectionModel("Product","A container for other Variations or SKUs. Contains product details, such as names, description and images. Can be used for targeted promotions."),
					new ItemTypeSelectionModel("Bundle","A grouping of items used for promotional purposes. Prices and availability is calculated based on included items."),
					new ItemTypeSelectionModel("Package","A grouping of items sold as a single unit. Package has it's own pricing and inventory."),
					new ItemTypeSelectionModel("Dynamic Kit","A dynamic kit is a type of catalog item which can be dynamically configured by the customer. This configuration (or grouping) of products is based on the customer's needs and is sold as a single unit.")
				};
            allAvailableOptions.ToList().ForEach(x => { x.Value = x.Value.Localize(); x.Description = x.Description.Localize(); });
            var itemVM = _itemTypeVmFactory.GetViewModelInstance(new System.Collections.Generic.KeyValuePair<string, object>("allAvailableOptions", allAvailableOptions));
            // show selection step
            CommonConfirmRequest.Raise(new ConditionalConfirmation(() => !string.IsNullOrEmpty(itemVM.SelectedItemType))
            {
                Content = itemVM,
                Title = "Select Item type".Localize()
            },
             (x) =>
             {
                 if (x.Confirmed)
                 {
                     if (itemVM.SelectedItemType == allAvailableOptions[1].Value)
                     {
                         RaiseItemCreateInteractionRequest<Product>(itemVM.SelectedItemType);
                     }
                     else if (itemVM.SelectedItemType == allAvailableOptions[0].Value)
                     {
                         RaiseItemCreateInteractionRequest<Sku>(itemVM.SelectedItemType);
                     }
                     else if (itemVM.SelectedItemType == allAvailableOptions[2].Value)
                     {
                         RaiseItemCreateInteractionRequest<Bundle>(itemVM.SelectedItemType);
                     }
                     else if (itemVM.SelectedItemType == allAvailableOptions[3].Value)
                     {
                         RaiseItemCreateInteractionRequest<Package>(itemVM.SelectedItemType);
                     }
                     else if (itemVM.SelectedItemType == allAvailableOptions[4].Value)
                     {
                         RaiseItemCreateInteractionRequest<DynamicKit>(itemVM.SelectedItemType);
                     }
                 }
             });
        }

        protected override void AfterItemAddSaved(object item)
        {
            if (item is Item)
            {
                using (var seoRepository = _seoRepository.GetRepositoryInstance())
                {
                    var i = (Item)item;
                    var itemName = ReplaceRestrictedChars(i.Name);
                    var checkItem = seoRepository.SeoUrlKeywords.Where(s => s.Keyword.Equals(itemName, StringComparison.InvariantCultureIgnoreCase) &&
                        s.Language.Equals(i.Catalog.DefaultLanguage, StringComparison.InvariantCultureIgnoreCase) &&
                        s.KeywordType.Equals((int)SeoUrlKeywordTypes.Item)).FirstOrDefault();

                    var seo = _seoFactory.CreateEntity<Foundation.AppConfig.Model.SeoUrlKeyword>();
                    seo.KeywordValue = i.ItemId;
                    seo.Keyword = checkItem == null ? itemName : "_" + itemName + "_";
                    seo.Language = i.Catalog.DefaultLanguage;
                    seo.IsActive = true;
                    seo.KeywordType = (int)SeoUrlKeywordTypes.Item;
                    seoRepository.Add(seo);
                    seoRepository.UnitOfWork.Commit();
                }

                // Open the item when wizard is complete
                var itemViewModel = _itemVmFactory.GetViewModelInstance(new System.Collections.Generic.KeyValuePair<string, object>("item", item));
                var openTracking = (IOpenTracking)itemViewModel;
                openTracking.OpenItemCommand.Execute();

                base.AfterItemAddSaved(item);
            }
            else if (item is CategoryBase)
            {
                if (item is Category)
                {
                    using (var seoRepository = _seoRepository.GetRepositoryInstance())
                    {
                        var category = (Category)item;
                        var itemName = ReplaceRestrictedChars(category.Name);
                        var checkItem = seoRepository.SeoUrlKeywords.Where(s => s.Keyword.Equals(itemName, StringComparison.InvariantCultureIgnoreCase) &&
                            s.Language.Equals(category.Catalog.DefaultLanguage, StringComparison.InvariantCultureIgnoreCase) &&
                            s.KeywordType.Equals((int)SeoUrlKeywordTypes.Category)).FirstOrDefault();

                        var seo = _seoFactory.CreateEntity<VirtoCommerce.Foundation.AppConfig.Model.SeoUrlKeyword>();
                        seo.KeywordValue = category.CategoryId;
                        seo.Keyword = checkItem == null ? itemName : "_" + itemName + "_";
                        seo.Language = category.Catalog.DefaultLanguage;
                        seo.IsActive = true;
                        seo.KeywordType = (int)SeoUrlKeywordTypes.Category;
                        seoRepository.Add(seo);
                        seoRepository.UnitOfWork.Commit();
                    }
                }

                var catalogParentEntity = (CatalogEntityViewModelBase)SelectedTreeViewItem;
                if (catalogParentEntity.IsExpanded)
                    catalogParentEntity.Refresh();
                else
                    catalogParentEntity.IsExpanded = true;
            }
            else if (item is CatalogBase)
            {
                RefreshTreeItemsCommand.Execute();
            }
        }

        const string invalidKeywordCharacters = @"$+;=%{}[]|\/@ ~#!^*&?:'<>,";
        private string ReplaceRestrictedChars(string source)
        {
            var target = source;
            foreach (var ch in invalidKeywordCharacters.ToCharArray())
            {
                target = target.Replace(ch, '-');
            }
            return target;
        }

        protected override void RaiseItemDeleteInteractionRequest(IList selectedItemsList)
        {
            var selectedItems = selectedItemsList.Cast<VirtualListItem<IItemViewModel>>();
            ItemDelete(selectedItems.Select(x => ((IViewModelDetailBase)x.Data)).ToList());
        }

        protected override void RaiseSearchCommand()
        {
            _isInSearchMode = true;
            base.RaiseSearchCommand();
        }

        #endregion

        #region ICatalogHomeViewModel Members
        public DelegateCommand SearchAllItemsCommand { get; private set; }
        public DelegateCommand ClearFiltersCommand { get; private set; }

        public DelegateCommand RefreshTreeItemsCommand { get; private set; }

        public DelegateCommand<IViewModel> OpenCatalogEntityCommand { get; private set; }

        public InteractionRequest<Notification> CommonNotifyRequest { get; private set; }

        public DelegateCommand QueryCreateCommand { get; private set; }
        public DelegateCommand<ItemFilter> QueryRunCommand { get; private set; }
        public DelegateCommand<ItemFilter> QueryEditCommand { get; private set; }
        public DelegateCommand<ItemFilter> QueryDeleteCommand { get; private set; }

        public DelegateCommand CreateCategoryCommand { get; private set; }
        public DelegateCommand CreateCatalogCommand { get; private set; }

        public DelegateCommand CreateLinkedCategoryCommand { get; private set; }
        public DelegateCommand<IViewModel> TreeItemDeleteCommand { get; private set; }

        public DelegateCommand<IList> ItemMoveCommand { get; private set; }
        public DelegateCommand<IList> ItemDuplicateCommand { get; private set; }

        private ObservableCollection<IViewModel> _rootCatalogs;
        public ObservableCollection<IViewModel> RootCatalogs
        {
            get { return _rootCatalogs ?? (_rootCatalogs = new ObservableCollection<IViewModel>()); }
        }

        private IViewModel _selectedCatalogItem;
        public IViewModel SelectedCatalogItem
        {
            get
            {
                return _selectedCatalogItem;
            }
            set
            {
                _isInSearchAllMode = false;
                _isInSearchMode = false;
                _currentFilter = null;
                if (_selectedCatalogItem != value)
                {
                    _selectedCatalogItem = value;
                    RefreshSelectedCatalog();
                    TreeItemDeleteCommand.RaiseCanExecuteChanged();
                }
            }
        }

        public object SearchFilterCatalog { get; set; }
        public ObservableCollection<CatalogBase> SearchFilterCatalogs { get; set; }

        private ObservableCollection<ItemFilter> _allQueries;
        public ObservableCollection<ItemFilter> AllQueries
        {
            get { return _allQueries; }
            set { _allQueries = value; OnPropertyChanged(); }
        }

        public string SearchFilterAll { get; set; }

        public string SearchFilterName { get; set; }
        public string SearchFilterCode { get; set; }
        public string SearchFilterItemType { get; set; }
        public string[] SearchFilterItemTypes { get; set; }
        #endregion

        #region IVirtualListLoader<IItemViewModel> Members

        public bool CanSort
        {
            get { return true; }
        }

        public IList<IItemViewModel> LoadRange(int startIndex, int count, SortDescriptionCollection sortDescriptions, out int overallCount)
        {
            var retVal = new List<IItemViewModel>();

            using (var repository = _catalogRepository.GetRepositoryInstance())
            {
                var canQuery = false;
                string catalogId;
                var query = repository.Items;

                // if advanced search was performed
                if (_currentFilter != null)
                {
                    canQuery = true;
                    query = query.Where(_currentFilter.Expression);
                    // query = query.Where(x => x.CategoryItemRelations.Any(y => y.Category.LinkedCategories.Count > 1));
                    //query = query.Where(x => x.CategoryItemRelations.Any(y => y.Category.Code.Contains("a")));
                    //query = query.Where(x => x.CategoryItemRelations.Any(y => y.Category.Code.Length > 7));
                    //query = query.Where(x => x.CategoryItemRelations.Any(y => y.CatalogId == "Sony"));
                }
                else if (_isInSearchAllMode) // quick search from a single textbox
                {
                    canQuery = true;
                    query = query.Where(x => x.Name.Contains(SearchFilterAll) || x.Code.Contains(SearchFilterAll));
                }
                else if (_isInSearchMode) // if simple search was performed
                {
                    if (!string.IsNullOrEmpty(SearchFilterName))
                    {
                        canQuery = true;
                        query = query.Where(x => x.Name.Contains(SearchFilterName));
                    }

                    if (!string.IsNullOrEmpty(SearchFilterCode))
                    {
                        canQuery = true;
                        query = query.Where(x => x.Code.Contains(SearchFilterCode));
                    }

                    if (SearchFilterItemType == SearchFilterItemTypes[0])
                    {
                        canQuery = true;
                        query = query.OfType<Sku>();
                    }
                    else if (SearchFilterItemType == SearchFilterItemTypes[1])
                    {
                        canQuery = true;
                        query = query.OfType<Product>();
                    }
                    else if (SearchFilterItemType == SearchFilterItemTypes[2])
                    {
                        canQuery = true;
                        query = query.OfType<Bundle>();
                    }
                    else if (SearchFilterItemType == SearchFilterItemTypes[3])
                    {
                        canQuery = true;
                        query = query.OfType<Package>();
                    }
                    else if (SearchFilterItemType == SearchFilterItemTypes[4])
                    {
                        canQuery = true;
                        query = query.OfType<DynamicKit>();
                    }

                    if (SearchFilterCatalog is CatalogBase)
                    {
                        canQuery = true;
                        catalogId = ((CatalogBase)SearchFilterCatalog).CatalogId;
                        query = query.Where(x => x.CatalogId == catalogId);
                    }
                }
                else // simple catalog browsing
                {
                    if (SelectedCatalogItem is ITreeCategoryViewModel)
                    {
                        canQuery = true;
                        var parentCategory = (ITreeCategoryViewModel)SelectedCatalogItem;
                        string categoryId;
                        if (parentCategory.InnerItem is LinkedCategory)
                        {
                            categoryId = ((LinkedCategory)parentCategory.InnerItem).LinkedCategoryId;
                        }
                        else
                        {
                            categoryId = parentCategory.InnerItem.CategoryId;
                        }
                        // checking current category and level 1 (direct) subcategories.
                        query = query.Where(x => x.CategoryItemRelations.Any(y => y.CategoryId == categoryId || y.Category.ParentCategoryId == categoryId));
                    }
                    else if (!string.IsNullOrEmpty(catalogId = GetCatalogId(SelectedCatalogItem)))
                    {
                        canQuery = true;
                        var catalog = GetCatalog(SelectedCatalogItem);
                        if (catalog is VirtualCatalog)
                        {
                            // item relations are accessed through CategoryItemRelations. They can be related to Virtual catalog:
                            // a) directly: x.CatalogId
                            // b) trough Category, which is related to LinkedCategory in Virtual Catalog.
                            query = query.Where(i => i.CategoryItemRelations.Any(x => x.CatalogId == catalogId || x.Category.LinkedCategories.Any(y => y.CatalogId == catalogId)));
                        }
                        else
                        {
                            query = query.Where(c => c.CatalogId == catalogId);
                        }
                    }
                }

                if (canQuery)
                {
                    overallCount = query.Count();
                    var orderedItems = sortDescriptions.Count == 0 ? query.OrderBy(x => x.ItemId) : ApplySortDescriptions(query, sortDescriptions);
                    var items = orderedItems.Skip(startIndex).Take(count).ToList();
                    retVal.AddRange(items.Select(item => _itemVmFactory.GetViewModelInstance(new System.Collections.Generic.KeyValuePair<string, object>("item", item))));
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
            if (ListItemsSource == null)
            {
                OnUIThread(async () =>
                    {
                        SearchFilterItemTypes = new[] { "Variation", "Product", "Bundle", "Package", "Dynamic Kit" };
                        OnPropertyChanged("SearchFilterItemTypes");

                        if (!RootCatalogs.Any())
                        {
                            //Get all catalogs
                            using (var repository = _catalogRepository.GetRepositoryInstance())
                            {
                                var catalogs = await Task.Factory.StartNew(() => repository.Catalogs.OrderBy(x => x.Name).ToList());
                                EndSearchCatalogs(catalogs);
                            }
                        }
                        ListItemsSource =
                            await
                            Task.Factory.StartNew(
                                () => new VirtualList<IItemViewModel>(this, 25, SynchronizationContext.Current));

                        AllQueries = new ObservableCollection<ItemFilter>();
                        var savedQueries =
                            Virtoway.WPF.State.ElementStateOperations.GetProperties("CustomItemsQuery");
                        if (savedQueries != null)
                        {
                            savedQueries.ForEach(x =>
                                {
                                    try
                                    {
                                        var filter = new ItemFilter { Name = x.Name, StringExpression = x.Value };
                                        AllQueries.Add(filter);
                                        filter.Expression =
                                            ExpressionBuilder.BuildExpression<Item, bool>(filter.StringExpression);
                                    }
                                    catch (Exception)
                                    {
                                        // throw;
                                    }
                                });
                        }

                        RefreshCaption();
                    });
            }

            OnUIThread(InitializeGestures);
            _parentViewModel.ViewTitle = _viewTitle as CatalogHomeTitleViewModel;
        }

        #endregion

        #region Private Methods

        internal static CatalogBase GetCatalog(IViewModel itemViewModel)
        {
            CatalogBase result = null;

            if (itemViewModel is ITreeCategoryViewModel)
            {
                var parent = ((ITreeCategoryViewModel)itemViewModel).Parent;
                while (parent is ITreeCategoryViewModel)
                {
                    parent = ((ITreeCategoryViewModel)parent).Parent;
                }
                itemViewModel = parent;
            }

            if (itemViewModel is ITreeCatalogViewModel)
            {
                result = ((ITreeCatalogViewModel)itemViewModel).InnerItem;
            }
            else if (itemViewModel is ITreeVirtualCatalogViewModel)
            {
                result = ((ITreeVirtualCatalogViewModel)itemViewModel).InnerItem;
            }

            return result;
        }

        private static string GetCatalogId(IViewModel itemViewModel)
        {
            string result = null;
            if (itemViewModel is ITreeCatalogViewModel)
            {
                result = ((ITreeCatalogViewModel)itemViewModel).InnerItem.CatalogId;
            }
            else if (itemViewModel is ITreeCategoryViewModel)
            {
                result = ((ITreeCategoryViewModel)itemViewModel).InnerItem.CatalogId;
            }
            else if (itemViewModel is ITreeVirtualCatalogViewModel)
            {
                result = ((ITreeVirtualCatalogViewModel)itemViewModel).InnerItem.CatalogId;
            }

            return result;
        }

        private void RefreshSelectedCatalog()
        {
            RefreshCaption();

            ListItemsSource.Refresh();
            OnPropertyChanged("SelectedCatalogItem");
            // OnPropertyChanged("IsRealCategory");
            CreateCategoryCommand.RaiseCanExecuteChanged();
            CreateLinkedCategoryCommand.RaiseCanExecuteChanged();
            RefreshItemsCommand.RaiseCanExecuteChanged();
            ItemAddCommand.RaiseCanExecuteChanged();
        }

        private void EndSearchCatalogs(List<CatalogBase> catalogs)
        {
            RootCatalogs.Clear();
            foreach (var catalog in catalogs)
            {
                AddCatalogToTree(catalog);
            }
            // SelectedCatalogItem = RootCatalogs.FirstOrDefault();
            SelectedCatalogItem = null;

            SearchFilterCatalogs.SetItems(catalogs);
        }

        private void AddCatalogToTree(CatalogBase catalog)
        {
            var catalogParameter = new System.Collections.Generic.KeyValuePair<string, object>("item", catalog);
            var catalogViewModel = catalog is catalogModel.Catalog ? (IViewModel)_treeCatalogVmFactory.GetViewModelInstance(catalogParameter) : _treeVirtualCatalogVmFactory.GetViewModelInstance(catalogParameter);

            RootCatalogs.Add(catalogViewModel);
        }

        private T CreateCatalogEntity<T>()
        {
            var catalogEntityName = _entityFactory.GetEntityTypeStringName(typeof(T));
            return (T)_entityFactory.CreateEntityForType(catalogEntityName);
        }

        private void RefreshCaption()
        {
            string subTitle = "";
            if (_currentFilter != null)
            {
                subTitle = string.Format("RESULTS FOR SAVED FILTER    \"{0}\"".Localize(), _currentFilter.Name.ToUpper());
            }
            else
            {
                string path = "";
                var item = SelectedCatalogItem as CatalogEntityViewModelBase;
                while (item != null)
                {
                    path = item.DisplayName + (String.IsNullOrEmpty(path) ? "" : " - ") + path;
                    item = item.Parent as CatalogEntityViewModelBase;
                }
                if (!String.IsNullOrEmpty(path))
                {
                    subTitle = string.Format("BROWSING    \"{0}\"".Localize(), path.ToUpper());
                }
            }
            if (_isInSearchAllMode)
            {
                subTitle += (String.IsNullOrEmpty(SearchFilterAll) ? "" : "      ") + string.Format("SEARCH RESULTS FOR   \"{0}\"".Localize(), SearchFilterAll.ToUpper());
            }
            (_viewTitle as CatalogHomeTitleViewModel).SubTitle = (String.IsNullOrEmpty(subTitle)) ? "MERCHANDISE MANAGEMENT".Localize() : subTitle;
        }

        #endregion

        #region IDropTarget Members

        public void DragOver(DropInfo dropInfo)
        {
            var canAcceptData = dropInfo.DragInfo != null;

            if (canAcceptData)
            {
                var realSourceItem = dropInfo.DragInfo.SourceItem as HierarchyViewModelBase;
                var realTargetItem = dropInfo.TargetItem as IViewModel;

                canAcceptData = realSourceItem != realTargetItem
                            && realSourceItem is ITreeCategoryViewModel
                            && (realTargetItem is ITreeCategoryViewModel || realTargetItem is ITreeCatalogViewModel || realTargetItem is ITreeVirtualCatalogViewModel)
                            && realSourceItem.Parent != realTargetItem
                            && GetCatalogId(realSourceItem) == GetCatalogId(realTargetItem);

                if (canAcceptData && realTargetItem is ITreeCategoryViewModel)
                {
                    canAcceptData = !(((ITreeCategoryViewModel)realTargetItem).InnerItem is LinkedCategory);
                }

                if (canAcceptData)
                {
                    dropInfo.Effects = DragDropEffects.Move;
                    dropInfo.DropTargetAdorner = DropTargetAdorners.Highlight;
                }
            }
        }

        public void Drop(DropInfo dropInfo)
        {
            var droppedVM = dropInfo.Data;
            var targetVM = dropInfo.TargetItem;

            if (droppedVM is IHierarchy)
            {
                // preparing to refresh UI
                var sourceVM = droppedVM as HierarchyViewModelBase;
                var sourceParentVM = sourceVM.Parent;

                ((IHierarchy)droppedVM).SetParent(null, targetVM);

                // refresh UI
                sourceParentVM.LoadChildrens();
                ((HierarchyViewModelBase)targetVM).LoadChildrens();
            }
        }

        #endregion

        #region PublicProperties

        public object SelectedTreeViewItem { get; private set; }

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
            if (_authContext.CheckPermission(PredefinedPermissions.CatalogCatalogsManage)
                || _authContext.CheckPermission(PredefinedPermissions.CatalogCategoriesManage)
                || _authContext.CheckPermission(PredefinedPermissions.CatalogItemsManage)
                || _authContext.CheckPermission(PredefinedPermissions.CatalogVirtual_CatalogsManage)
                || _authContext.CheckPermission(PredefinedPermissions.CatalogLinked_CategoriesManage)
                )
            {
                _tileManager.AddTile(new NumberTileItem
                    {
                        IdModule = NavigationNames.MenuName,
                        IdTile = "Products",
                        TileTitle = "Products",
                        TileCategory = NavigationNames.ModuleName,
                        Order = 0,
                        IdColorSchema = TileColorSchemas.Schema2,
                        NavigateCommand = new DelegateCommand(() => NavigateToTabPage(NavigationNames.HomeName)),
                        Refresh = async (tileItem) =>
                            {
                                try
                                {
                                    using (var repository = _catalogRepository.GetRepositoryInstance())
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

            if (_authContext.CheckPermission(PredefinedPermissions.CatalogCatalogsManage)
                || _authContext.CheckPermission(PredefinedPermissions.CatalogCategoriesManage)
                || _authContext.CheckPermission(PredefinedPermissions.CatalogItemsManage)
                || _authContext.CheckPermission(PredefinedPermissions.CatalogVirtual_CatalogsManage)
                || _authContext.CheckPermission(PredefinedPermissions.CatalogLinked_CategoriesManage)
                )
            {
                _tileManager.AddTile(new NumberTileItem()
                    {
                        IdModule = NavigationNames.MenuName,
                        IdTile = "Catalogs",
                        TileTitle = "Catalogs",
                        TileCategory = NavigationNames.ModuleName,
                        Order = 1,
                        IdColorSchema = TileColorSchemas.Schema4,
                        NavigateCommand = new DelegateCommand(() => NavigateToTabPage(NavigationNames.HomeName)),
                        Refresh = async (tileItem) =>
                            {
                                try
                                {
                                    using (var repository = _catalogRepository.GetRepositoryInstance())
                                    {
                                        if (tileItem is NumberTileItem)
                                        {
                                            var query = await Task.Factory.StartNew(() => repository.Catalogs.Count());
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

            if (_authContext.CheckPermission(PredefinedPermissions.CatalogCatalog_Import_JobsRun))
            {
                _tileManager.AddTile(new NumberTileItem()
                    {
                        IdModule = NavigationNames.MenuName,
                        IdTile = "ImportJob",
                        TileTitle = "Import",
                        TileCategory = NavigationNames.ModuleName,
                        Order = 5,
                        IdColorSchema = TileColorSchemas.Schema2,
                        NavigateCommand = new DelegateCommand(() => NavigateToTabPage(NavigationNames.HomeNameCatalogImportJob)),
                        Refresh = async (tileItem) =>
                            {
                                try
                                {
                                    using (var repository = _importRepository.GetRepositoryInstance())
                                    {
                                        if (tileItem is NumberTileItem)
                                        {
                                            //todo get real available Importers for catalog
                                            var availableImporters = (ImportEntityType[])Enum.GetValues(typeof(ImportEntityType));
                                            var query = await Task.Factory.StartNew(() =>
                                                {
                                                    var items = repository.ImportJobs;
                                                    var count = 0;
                                                    foreach (var item in items)
                                                    {
                                                        if (availableImporters.Any(importer => importer.ToString() == item.EntityImporter))
                                                        {
                                                            count++;
                                                        }
                                                    }
                                                    return count;
                                                });

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
        }

        #endregion

        #region Permissions

        private bool HasManageTreeItemsPermission(object vm)
        {
            bool retVal = false;
            var baseSelectedItem = SelectedTreeViewItem as CatalogEntityViewModelBase;
            if (baseSelectedItem != null)
            {
                var p = baseSelectedItem.GetType().GetProperty("InnerItem");
                var selectedItemType = p.GetValue(baseSelectedItem, null).GetType();
                if (selectedItemType == typeof(catalogModel.Catalog))
                    retVal = _authContext.CheckPermission(PredefinedPermissions.CatalogCatalogsManage);
                else if (selectedItemType == typeof(VirtualCatalog))
                    retVal = _authContext.CheckPermission(PredefinedPermissions.CatalogVirtual_CatalogsManage);
                else if (selectedItemType == typeof(Category))
                    retVal = _authContext.CheckPermission(PredefinedPermissions.CatalogCategoriesManage);
                else if (selectedItemType == typeof(LinkedCategory))
                    retVal = _authContext.CheckPermission(PredefinedPermissions.CatalogLinked_CategoriesManage);
            }
            return retVal;
        }

        public bool HasAddCatalogPermission
        {
            get
            {
                return _authContext.CheckPermission(PredefinedPermissions.CatalogCatalogsManage) ||
                    _authContext.CheckPermission(PredefinedPermissions.CatalogVirtual_CatalogsManage);
            }
        }


        #endregion
    }
}