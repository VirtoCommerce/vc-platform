using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Interactivity.InteractionRequest;
using Omu.ValueInjecter;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Transactions;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using VirtoCommerce.Client.Globalization;
using VirtoCommerce.Foundation.AppConfig.Repositories;
using VirtoCommerce.Foundation.Catalogs.Factories;
using VirtoCommerce.Foundation.Catalogs.Model;
using VirtoCommerce.Foundation.Catalogs.Repositories;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.Foundation.Frameworks.ConventionInjections;
using VirtoCommerce.Foundation.Frameworks.Extensions;
using VirtoCommerce.Foundation.Security.Model;
using VirtoCommerce.ManagementClient.Asset.ViewModel.Interfaces;
using VirtoCommerce.ManagementClient.Catalog.Infrastructure;
using VirtoCommerce.ManagementClient.Catalog.Model;
using VirtoCommerce.ManagementClient.Catalog.ViewModel.Catalog.Interfaces;
using VirtoCommerce.ManagementClient.Catalog.ViewModel.Pricelists.Interfaces;
using VirtoCommerce.ManagementClient.Catalog.ViewModel.Wizard;
using VirtoCommerce.ManagementClient.Core.Infrastructure;
using VirtoCommerce.ManagementClient.Core.Infrastructure.Navigation;
using VirtoCommerce.ManagementClient.Localization;
using catalogModel = VirtoCommerce.Foundation.Catalogs.Model;


namespace VirtoCommerce.ManagementClient.Catalog.ViewModel.Catalog.Implementations
{
    public class ItemViewModel : ViewModelDetailAndWizardBase<Item>, IItemViewModel
    {
        protected IPricelistRepository _priceListRepository;

        #region Dependencies

        private readonly IAuthenticationContext _authContext;
        private readonly IRepositoryFactory<ICatalogRepository> _repositoryFactory;
        private readonly IRepositoryFactory<IPricelistRepository> _pricelistRepositoryFactory;
        private readonly IRepositoryFactory<IAppConfigRepository> _appConfigRepositoryFactory;
        private readonly IViewModelsFactory<IPropertyValueBaseViewModel> _propertyValueVmFactory;
        private readonly IViewModelsFactory<IPriceViewModel> _priceVmFactory;
        private readonly IViewModelsFactory<IItemAssetViewModel> _assetVmFactory;
        private readonly IViewModelsFactory<IAssociationGroupEditViewModel> _associationGroupEditVmFactory;
        private readonly IViewModelsFactory<IAssociationGroupViewModel> _associationGroupVmFactory;
        private readonly IViewModelsFactory<IItemRelationViewModel> _itemRelationVmFactory;
        private readonly IViewModelsFactory<IEditorialReviewViewModel> _reviewVmFactory;
        private readonly IViewModelsFactory<ICategoryItemRelationViewModel> _categoryVmFactory;
        private readonly IViewModelsFactory<IItemSeoViewModel> _seoVmFactory;
        private readonly INavigationManager _navManager;

        #endregion

        #region Constructor

        /// <summary>
        /// public. For viewing
        /// </summary>
        public ItemViewModel(
            IRepositoryFactory<IAppConfigRepository> appConfigRepositoryFactory,
            IViewModelsFactory<IItemSeoViewModel> seoVmFactory,
            IRepositoryFactory<ICatalogRepository> repositoryFactory,
            IRepositoryFactory<IPricelistRepository> pricelistRepositoryFactory, IViewModelsFactory<IPropertyValueBaseViewModel> propertyValueVmFactory,
            IViewModelsFactory<IPriceViewModel> priceVmFactory, IViewModelsFactory<IItemAssetViewModel> assetVmFactory,
            IViewModelsFactory<IAssociationGroupEditViewModel> associationGroupEditVmFactory, IViewModelsFactory<IAssociationGroupViewModel> associationGroupVmFactory,
            IViewModelsFactory<IItemRelationViewModel> itemRelationVmFactory, IViewModelsFactory<IEditorialReviewViewModel> reviewVmFactory,
            IViewModelsFactory<ICategoryItemRelationViewModel> categoryVmFactory,
            ICatalogEntityFactory entityFactory, Item item, IAuthenticationContext authContext, INavigationManager navManager)
            : this(repositoryFactory, pricelistRepositoryFactory, propertyValueVmFactory, entityFactory, item, authContext, false, priceVmFactory)
        {
            _assetVmFactory = assetVmFactory;
            _associationGroupVmFactory = associationGroupVmFactory;
            _associationGroupEditVmFactory = associationGroupEditVmFactory;
            _itemRelationVmFactory = itemRelationVmFactory;
            _reviewVmFactory = reviewVmFactory;
            _categoryVmFactory = categoryVmFactory;
            _seoVmFactory = seoVmFactory;
            _appConfigRepositoryFactory = appConfigRepositoryFactory;

            _navManager = navManager;
            ViewTitle = new ViewTitleBase { Title = ItemTypeTitle, SubTitle = item.Name.ToUpper() };

            OpenItemCommand = new DelegateCommand(() =>
            {
                if (_authContext.CheckPermission(PredefinedPermissions.CatalogItemsManage))
                {
                    _navManager.Navigate(NavigationData);
                }
            });

            InitCommands();

            ItemRelationAddCommand = new DelegateCommand(RaiseItemRelationAddInteractionRequest);
            ItemRelationEditCommand = new DelegateCommand<ItemRelation>(RaiseItemRelationEditInteractionRequest, x => x != null);
            ItemRelationDeleteCommand = new DelegateCommand<ItemRelation>(RaiseItemRelationDeleteInteractionRequest, x => x != null);

            AssociationGroupAddCommand = new DelegateCommand(RaiseAssociationGroupAddInteractionRequest);
            AssociationGroupEditCommand = new DelegateCommand<AssociationGroup>(RaiseAssociationGroupEditInteractionRequest);
            AssociationGroupDeleteCommand = new DelegateCommand<object>(RaiseAssociationGroupDeleteInteractionRequest, x => x is IAssociationGroupViewModel);

            EditorialReviewAddCommand = new DelegateCommand(RaiseEditorialReviewAddInteractionRequest);
            EditorialReviewEditCommand = new DelegateCommand<EditorialReview>(RaiseEditorialReviewEditInteractionRequest, x => HasPublishPermission(x) && x != null);
            EditorialReviewDeleteCommand = new DelegateCommand<EditorialReview>(RaiseEditorialReviewDeleteInteractionRequest, x => HasPublishPermission(x) && x != null);

            CategoryItemRelationAddCommand = new DelegateCommand<string>(RaiseCategoryItemRelationAddInteractionRequest, x => ItemCategoryViewsSelectedIndex > 0 || InnerItem.CategoryItemRelations.All(y => y.CatalogId != InnerItem.CatalogId));
            CategoryItemRelationEditCommand = new DelegateCommand<CategoryItemRelation>(RaiseCategoryItemRelationEditInteractionRequest, x => x != null);
            CategoryItemRelationDeleteCommand = new DelegateCommand<CategoryItemRelation>(RaiseCategoryItemRelationDeleteInteractionRequest, x => x != null);

            ItemAssetAddCommand = new DelegateCommand<string>(RaiseItemAssetAddInteractionRequest);
            ItemAssetEditCommand = new DelegateCommand<ItemAsset>(RaiseItemAssetEditInteractionRequest, x => x != null);
            ItemAssetDeleteCommand = new DelegateCommand<ItemAsset>(RaiseItemAssetDeleteInteractionRequest, x => x != null);
        }

        /// <summary>
        /// protected. For a step
        /// </summary>
        protected ItemViewModel(
            IRepositoryFactory<ICatalogRepository> repositoryFactory,
            IRepositoryFactory<IPricelistRepository> pricelistRepositoryFactory,
            IViewModelsFactory<IPropertyValueBaseViewModel> vmFactory,
            ICatalogEntityFactory entityFactory,
            Item item,
            IAuthenticationContext authContext,
            IViewModelsFactory<IPriceViewModel> priceVmFactory)
            : this(repositoryFactory, pricelistRepositoryFactory, vmFactory, entityFactory, item, authContext, true, priceVmFactory)
        {
            InitCommands();
        }

        private ItemViewModel(IRepositoryFactory<ICatalogRepository> repositoryFactory, IRepositoryFactory<IPricelistRepository> pricelistRepositoryFactory, IViewModelsFactory<IPropertyValueBaseViewModel> vmFactory, ICatalogEntityFactory entityFactory, Item item, IAuthenticationContext authContext, bool isWizardMode, IViewModelsFactory<IPriceViewModel> priceVmFactory)
            : base(entityFactory, item, isWizardMode)
        {
            _propertyValueVmFactory = vmFactory;
            _repositoryFactory = repositoryFactory;
            _pricelistRepositoryFactory = pricelistRepositoryFactory;
            _authContext = authContext;
            _priceVmFactory = priceVmFactory;
        }

        #endregion

        public string ItemTypeTitle
        {
            get
            {
                string result = "Item";
                if (InnerItem is Product)
                    result = "Product";
                else if (InnerItem is Bundle)
                    result = "Bundle";
                else if (InnerItem is Sku)
                    result = "Variation";
                else if (InnerItem is Package)
                    result = "Package";
                else if (InnerItem is DynamicKit)
                    result = "Dynamic Kit";
                return result;
            }
        }

        private bool _IsInitializingItemRelations;
        public bool IsInitializingItemRelations
        {
            get { return _IsInitializingItemRelations; }
            set { _IsInitializingItemRelations = value; OnPropertyChanged(); }
        }

        private bool _IsInitializingPricing;
        public bool IsInitializingPricing
        {
            get { return _IsInitializingPricing; }
            set { _IsInitializingPricing = value; OnPropertyChanged(); }
        }

        private bool _IsInitializingItemCategories;
        public bool IsInitializingItemCategories
        {
            get { return _IsInitializingItemCategories; }
            set { _IsInitializingItemCategories = value; OnPropertyChanged(); }
        }

        private const int TabIndexOverview = 0;
        private const int TabIndexProperties = 1;

        private int _selectedTabIndex;
        public int SelectedTabIndex
        {
            get { return _selectedTabIndex; }
            protected set { _selectedTabIndex = value; OnPropertyChanged(); }
        }

        public void RaiseCanExecuteChanged()
        {
            PropertyValueEditCommand.RaiseCanExecuteChanged();
            PropertyValueDeleteCommand.RaiseCanExecuteChanged();
        }

        #region CatalogEntityViewModelBase Members

        public override string IconSource
        {
            get
            {
                string result = string.Empty;
                if (InnerItem is Product)
                    result = "Icon_Product";
                else if (InnerItem is Bundle)
                    result = "Icon_Bundle";
                else if (InnerItem is Sku)
                    result = "Icon_Variation";
                else if (InnerItem is Package)
                    result = "Icon_Package";
                else if (InnerItem is DynamicKit)
                    result = "Icon_DynamicKit";

                return result;
            }
        }

        public override string DisplayName
        {
            get
            {
                return OriginalItem.Name;
            }
        }

        public override Brush ShellDetailItemMenuBrush
        {
            get
            {
                var result =
                  (SolidColorBrush)Application.Current.TryFindResource("CatalogDetailItemMenuBrush");

                return result ?? base.ShellDetailItemMenuBrush;
            }
        }

        private NavigationItem _navigationData;
        public override NavigationItem NavigationData
        {
            get
            {
                return _navigationData ??
                       (_navigationData = new NavigationItem(OriginalItem.ItemId, NavigationNames.HomeName, NavigationNames.MenuName,
                                                             this));
            }
        }
        #endregion

        #region ViewModelDetailAndWizardBase Members

        public override string ExceptionContextIdentity { get { return string.Format("Catalog item ({0})", DisplayName); } }

        protected override void GetRepository()
        {
            Repository = _repositoryFactory.GetRepositoryInstance();
        }

        protected override bool HasPermission()
        {
            return _authContext.CheckPermission(PredefinedPermissions.PricingPrice_ListsManage);
        }

        // function almost duplicated in CategoryViewModel
        protected override bool IsValidForSave()
        {
            var result = InnerItem.Validate();

            // Code should be unique in scope of catalog
            var isCodeValid = true;
            if (InnerItem.Code != OriginalItem.Code)
            {
                var count = ItemRepository.Items
                                          .Where(x =>
                                              x.CatalogId == InnerItem.CatalogId && x.Code == InnerItem.Code && x.ItemId != InnerItem.ItemId)
                                          .Count();

                if (count > 0)
                {
                    InnerItem.SetError("Code", "An item with this Code already exists in this catalog".Localize(), true);
                    SelectedTabIndex = TabIndexOverview;
                    isCodeValid = false;
                }
            }

            var isPropertyValuesValid = PropertiesAndValues.All(x => x.IsValid);
            if (!isPropertyValuesValid && isCodeValid)
            {
                SelectedTabIndex = TabIndexProperties;
                var val = PropertiesAndValues.First(x => !x.IsValid);
                if (!string.IsNullOrEmpty(val.Locale) && val.Locale != FilterLanguage)
                {
                    RaisePropertiesLocalesFilter(val.Locale);
                }
            }

            var seoIsValid = true;
            if (SeoStepViewModel != null)
            {
                seoIsValid = SeoStepViewModel.IsValid;
                if (!seoIsValid)
                    SelectedTabIndex = (InnerItem is Bundle) ? 2 : 3;
            }

            return result && isPropertyValuesValid && isCodeValid && seoIsValid;
        }

        /// <summary>
        /// Return RefusedConfirmation for Cancel Confirm dialog
        /// </summary>
        protected override RefusedConfirmation CancelConfirm()
        {
            return new RefusedConfirmation
            {
                Content = string.Format("Save changes to Item '{0}'?".Localize(), DisplayName),
                Title = "Action confirmation".Localize(null, LocalizationScope.DefaultCategory)
            };
        }

        protected override void LoadInnerItem()
        {
            LoadInnerItem(ItemLoadingMode.Default);
        }

        protected override void LoadInnerItem(ItemLoadingMode itemLoadingMode)
        {
            try
            {
                var query = ItemRepository.Items.Where(x => x.ItemId == OriginalItem.ItemId);

                if (itemLoadingMode < ItemLoadingMode.Delete)
                {
                    query = query.ExpandAll()
                        .Expand("AssociationGroups/Associations/CatalogItem")
                        .Expand("ItemPropertyValues")
                        // .Expand("Catalog")
                        .Expand("CategoryItemRelations/Category")
                        .Expand("EditorialReviews/CatalogItem");
                    if (itemLoadingMode == ItemLoadingMode.Default)
                        query = query.Expand("PropertySet/PropertySetProperties/Property/PropertyValues");
                }

                Item item = query.Single();

                if (itemLoadingMode == ItemLoadingMode.Default)
                {
                    var queryCatalog = ItemRepository.Catalogs
                        .OfType<catalogModel.Catalog>()
                        .Where(x => x.CatalogId == item.CatalogId)
                        .Expand(x => x.CatalogLanguages)
                        .SingleOrDefault();
                    item.Catalog = queryCatalog;
                }

                OnUIThread(() => { InnerItem = item; });
            }
            catch (Exception ex)
            {
                ShowErrorDialog(ex, string.Format("An error occurred when trying to load {0}", ExceptionContextIdentity));
            }
        }

        protected override void InitializePropertiesForViewing()
        {
            InitializeOverviewObjects();

            OnUIThread(() =>
            {
                ItemAssetsImagesView = new ListCollectionView(InnerItem.ItemAssets);
                ItemAssetsImagesView.Filter = (x => ((ItemAsset)x).AssetType == "image");
                OnPropertyChanged("ItemAssetsImagesView");

                ItemAssetsFilesView = new ListCollectionView(InnerItem.ItemAssets);
                ItemAssetsFilesView.Filter = (x => ((ItemAsset)x).AssetType != "image");
                OnPropertyChanged("ItemAssetsFilesView");

                InitializePropertiesAndValues();
                InitializeAssociationGroupViewModels();
                IsInitializing = false;

                // InitializingItemCategories
                ItemCategoryInfos = new ObservableCollection<CollectionViewInfo>();
                CreateViewForCategoriesInCatalog(InnerItem.CatalogId);

                ItemRepository.Catalogs.OfType<VirtualCatalog>().ToArray()
                    .Select(x => x.CatalogId)
                    .ToList()
                    .ForEach(CreateViewForCategoriesInCatalog);

                OnPropertyChanged("ItemCategoryInfos");
                if (ItemCategoryViewsSelectedIndex < 0)
                    ItemCategoryViewsSelectedIndex = 0;

                IsInitializingItemCategories = false;

                // InitializingItemRelations
                var allItemRelations = ItemRepository.ItemRelations.Expand("ChildItem")
                                                 .Where(x => x.ParentItemId == InnerItem.ItemId)
                                                 .ToList();
                _ItemRelations = new CollectionChangeGeneral<ItemRelation>(allItemRelations);
                OnPropertyChanged("ItemRelations");

                IsInitializingItemRelations = false;

                // InitializingPricing tab
                InitializePricing();
            });

            InitSeoStep();
        }

        protected override void BeforeSaveChanges()
        {
            Initialize_ItemRelations_ForSave();
        }

        protected override void DoSaveChanges()
        {
            using (var transaction = new TransactionScope(TransactionScopeOption.Suppress))
            {
                if (IsWizardMode)
                {
                    Repository.Add(InnerItem);
                }
                Repository.UnitOfWork.Commit();
                if (_priceListRepository != null)
                    _priceListRepository.UnitOfWork.Commit();

                transaction.Complete();
            }
        }

        protected override void AfterSaveChangesUI()
        {
            ItemRelations.CommitChanges();

            OriginalItem.InjectFrom(InnerItem);

            if (SeoStepViewModel != null)
            {
                SeoStepViewModel.SaveSeoKeywordsChanges();
            }
        }

        protected override void SetSubscriptionUI()
        {
            InnerItem.PropertyChanged += InnerItem_PropertyChanged;
            InnerItem.ItemAssets.CollectionChanged += ViewModel_PropertyChanged;
            InnerItem.ItemAssets.ToList().ForEach(x => x.PropertyChanged += ViewModel_PropertyChanged);
            InnerItem.AssociationGroups.CollectionChanged += ViewModel_PropertyChanged;
            InnerItem.EditorialReviews.CollectionChanged += ViewModel_PropertyChanged;
            InnerItem.EditorialReviews.ToList().ForEach(x => x.PropertyChanged += ViewModel_PropertyChanged);
            InnerItem.CategoryItemRelations.CollectionChanged += ViewModel_PropertyChanged;
            InnerItem.CategoryItemRelations.ToList().ForEach(x => x.PropertyChanged += ViewModel_PropertyChanged);

            foreach (var item in InnerItem.AssociationGroups)
            {
                item.PropertyChanged += ViewModel_PropertyChanged;
                foreach (var association in item.Associations)
                {
                    association.PropertyChanged += ViewModel_PropertyChanged;
                }
                item.Associations.CollectionChanged += ViewModel_PropertyChanged;
            }

            PriceLists.ForEach(x =>
                {
                    x.Prices.CollectionChanged += ViewModel_PropertyChanged;
                    x.Prices.ToList().ForEach(x1 => x1.PropertyChanged += ViewModel_PropertyChanged);
                });

            ItemRelations.CollectionChanged = ViewModel_PropertyChanged;

            if (SeoStepViewModel != null)
            {
                if (SeoStepViewModel.SeoKeywords != null)
                    SeoStepViewModel.SeoKeywords.ForEach(keyword => keyword.PropertyChanged += ViewModel_PropertyChanged);
            }
        }

        protected override void CloseSubscriptionUI()
        {
            InnerItem.PropertyChanged -= InnerItem_PropertyChanged;
            InnerItem.ItemAssets.CollectionChanged -= ViewModel_PropertyChanged;
            InnerItem.AssociationGroups.CollectionChanged -= ViewModel_PropertyChanged;
            InnerItem.EditorialReviews.CollectionChanged -= ViewModel_PropertyChanged;
            InnerItem.EditorialReviews.ToList().ForEach(x => x.PropertyChanged -= ViewModel_PropertyChanged);
            InnerItem.CategoryItemRelations.CollectionChanged -= ViewModel_PropertyChanged;
            InnerItem.CategoryItemRelations.ToList().ForEach(x => x.PropertyChanged -= ViewModel_PropertyChanged);

            foreach (var item in InnerItem.AssociationGroups)
            {
                item.PropertyChanged -= ViewModel_PropertyChanged;
                foreach (var association in item.Associations)
                {
                    association.PropertyChanged -= ViewModel_PropertyChanged;
                }
                item.Associations.CollectionChanged -= ViewModel_PropertyChanged;
            }

            if (PriceLists != null)
                PriceLists.ForEach(x =>
                    {
                        x.Prices.CollectionChanged -= ViewModel_PropertyChanged;
                        x.Prices.ToList().ForEach(x1 => x1.PropertyChanged -= ViewModel_PropertyChanged);
                    });

            ItemRelations.Unsubscribe(ViewModel_PropertyChanged);

            if (SeoStepViewModel != null)
            {
                if (SeoStepViewModel.SeoKeywords != null)
                    SeoStepViewModel.SeoKeywords.ForEach(keyword => keyword.PropertyChanged -= ViewModel_PropertyChanged);
            }
        }

        protected override void DoDuplicate()
        {
            var item = InnerItem.DeepClone(EntityFactory as IKnownSerializationTypes);
            item.ItemId = item.GenerateNewKey();
            item.Code = Guid.NewGuid().ToString().Substring(0, 10);
            item.Name = item.Name + "_1";

            item.AssociationGroups.ToList().ForEach(x =>
            {
                x.ItemId = item.ItemId;
                x.AssociationGroupId = x.GenerateNewKey();
                x.Associations.ToList().ForEach(y =>
                {
                    y.CatalogItem = null;
                    y.AssociationGroupId = x.AssociationGroupId;
                    y.AssociationId = y.GenerateNewKey();
                });
            });

            item.CategoryItemRelations.ToList().ForEach(x =>
            {
                x.ItemId = item.ItemId;
                x.CategoryItemRelationId = x.GenerateNewKey();
                x.Category = null;
            });

            item.EditorialReviews.ToList().ForEach(x =>
            {
                x.ItemId = item.ItemId;
                x.EditorialReviewId = x.GenerateNewKey();
            });

            item.ItemAssets.ToList().ForEach(x =>
            {
                x.ItemAssetId = x.GenerateNewKey();
                x.ItemId = item.ItemId;
            });

            item.ItemPropertyValues.ToList().ForEach(x =>
            {
                x.ItemId = item.ItemId;
                x.PropertyValueId = x.GenerateNewKey();
            });

            Repository.Add(item);

            // Prices
            _priceListRepository = _pricelistRepositoryFactory.GetRepositoryInstance();
            var itemPrices = _priceListRepository.Prices.Where(x => x.ItemId == InnerItem.ItemId).ToList();
            itemPrices.ForEach(x =>
            {
                var price = x.DeepClone(EntityFactory as IKnownSerializationTypes);
                price.ItemId = item.ItemId;
                price.PriceId = price.GenerateNewKey();
                _priceListRepository.Add(price);
                // Repository.Add(price);
            });

            // Relations
            var allItemRelations = ItemRepository.ItemRelations.Where(x => x.ParentItemId == InnerItem.ItemId).ToList();
            allItemRelations.ForEach(x =>
            {
                var relation = x.DeepClone(EntityFactory as IKnownSerializationTypes);
                relation.ItemRelationId = relation.GenerateNewKey();
                relation.ParentItemId = item.ItemId;
                Repository.Add(relation);
            });
        }

        protected override bool BeforeDelete()
        {
            var itemRelations = ItemRepository.ItemRelations.Where(ir => ir.ChildItemId == OriginalItem.ItemId || ir.ParentItemId == OriginalItem.ItemId).ToList();
            foreach (var itemRelation in itemRelations)
            {
                Repository.Remove(itemRelation);
            }

            // find and remove Associations where current item is associated to other Items.
            var associations = ItemRepository.Associations.Where(a => a.ItemId == OriginalItem.ItemId).ToList();
            foreach (var association in associations)
            {
                Repository.Remove(association);
            }

            using (var seoRepository = _appConfigRepositoryFactory.GetRepositoryInstance())
            {
                var deleteList = seoRepository.SeoUrlKeywords.Where(x => x.KeywordValue.Equals(InnerItem.ItemId, StringComparison.InvariantCultureIgnoreCase));
                if (deleteList.Count() > 0)
                    deleteList.ToList().ForEach(seoRepository.Remove);
                seoRepository.UnitOfWork.Commit();
            }

            return true;
        }

        #endregion

        #region IItemViewModel Members

        public ICatalogRepository ItemRepository { get { return (ICatalogRepository)Repository; } }

        public IEnumerable<PropertySet> AllAvailableItemTypes
        {
            get;
            private set;
        }

        public TaxCategory[] AllAvailableTaxCategories { get; private set; }
        public Packaging[] AvailablePackageTypes { get; private set; }
        public AvailabilityRule[] AvailableAvailabilityRules { get; private set; }

        public DelegateCommand<object> PropertyValueEditCommand { get; private set; }
        public DelegateCommand<object> PropertyValueDeleteCommand { get; private set; }

        #endregion

        #region Overview tab

        protected void InitializeOverviewObjects()
        {
            InitializePropertySets();

            // Initialize Other Overview Objects
            if (AllAvailableTaxCategories == null)
            {
                var temp = ItemRepository.TaxCategories.ToList();
                temp.Insert(0, new TaxCategory() { TaxCategoryId = null, Name = "Not taxable".Localize() });
                AllAvailableTaxCategories = temp.ToArray();
                OnPropertyChanged("AllAvailableTaxCategories");

                AvailablePackageTypes = ItemRepository.Packagings.ToArray();
                OnPropertyChanged("AvailablePackageTypes");

                AvailableAvailabilityRules = (AvailabilityRule[])Enum.GetValues(typeof(AvailabilityRule));
                OnPropertyChanged("AvailableAvailabilityRules");
            }
        }

        private void InitializePropertySets()
        {
            if (AllAvailableItemTypes == null)
            {
                // query PropertySets
                var query = ItemRepository.PropertySets.Where(x => x.CatalogId == InnerItem.CatalogId);

                // this should work... but it doesn't!
                //var predicateOR = PredicateBuilder.False<PropertySet>();
                //predicateOR = predicateOR.Or(x => x.TargetType == PropertyTargetType.All.ToString());
                //if (InnerItem is Product) predicateOR = predicateOR.Or(x=>x.TargetType == PropertyTargetType.Product.ToString());
                //else if (InnerItem is Bundle) predicateOR = predicateOR.Or(x => x.TargetType == PropertyTargetType.Bundle.ToString());
                //else if (InnerItem is Sku) predicateOR = predicateOR.Or(x => x.TargetType == PropertyTargetType.Sku.ToString());
                //else if (InnerItem is Package) predicateOR = predicateOR.Or(x => x.TargetType == PropertyTargetType.Package.ToString());
                //else if (InnerItem is DynamicKit) predicateOR = predicateOR.Or(x => x.TargetType == PropertyTargetType.DynamicKit.ToString());
                //AllAvailableItemTypes = query.Where(predicateOR).ToList();

                if (InnerItem is Product)
                    query = query.Where(x => x.TargetType == PropertyTargetType.Product.ToString() || x.TargetType == PropertyTargetType.All.ToString());
                else if (InnerItem is Bundle)
                    query = query.Where(x => x.TargetType == PropertyTargetType.Bundle.ToString() || x.TargetType == PropertyTargetType.All.ToString());
                else if (InnerItem is Sku)
                    query = query.Where(x => x.TargetType == PropertyTargetType.Sku.ToString() || x.TargetType == PropertyTargetType.All.ToString());
                else if (InnerItem is Package)
                    query = query.Where(x => x.TargetType == PropertyTargetType.Package.ToString() || x.TargetType == PropertyTargetType.All.ToString());
                else if (InnerItem is DynamicKit)
                    query = query.Where(x => x.TargetType == PropertyTargetType.DynamicKit.ToString() || x.TargetType == PropertyTargetType.All.ToString());

                // AllAvailableItemTypes = query.ToList();
                AllAvailableItemTypes = query.Expand("PropertySetProperties/Property/PropertyValues").ToList();
                OnPropertyChanged("AllAvailableItemTypes");
            }
        }

        protected virtual void InnerItem_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Code")
                SeoStepViewModel.ChangeKeywordValue(InnerItem.Code);
            if (e.PropertyName == "PropertySetId")
            {
                CategoryViewModel.SetupPropertiesAndValues(InnerItem.PropertySet, InnerItem.ItemPropertyValues, InnerItemCatalogLanguages, PropertiesAndValues, IsWizardMode);
            }
        }
        #endregion

        public DelegateCommand PriceAddCommand { get; private set; }
        public DelegateCommand<Price> PriceEditCommand { get; private set; }
        public DelegateCommand<Price> PriceDeleteCommand { get; private set; }

        // public ObservableCollection<ItemRelation> ItemRelations { get; private set; }
        private CollectionChangeGeneral<ItemRelation> _ItemRelations;
        /// <summary>
        /// Gets the item relations. This collection is managed separately as adding Item.ItemRelation would create circular dependencies. 
        /// </summary>
        /// <value>
        /// The item relations.
        /// </value>
        public CollectionChangeGeneral<ItemRelation> ItemRelations
        {
            get
            {
                return _ItemRelations ?? (_ItemRelations = new CollectionChangeGeneral<ItemRelation>());
            }
        }

        public DelegateCommand ItemRelationAddCommand { get; private set; }
        public DelegateCommand<ItemRelation> ItemRelationEditCommand { get; private set; }
        public DelegateCommand<ItemRelation> ItemRelationDeleteCommand { get; private set; }

        public DelegateCommand AssociationGroupAddCommand { get; private set; }
        public DelegateCommand<AssociationGroup> AssociationGroupEditCommand { get; private set; }
        public DelegateCommand<object> AssociationGroupDeleteCommand { get; private set; }

        public DelegateCommand EditorialReviewAddCommand { get; private set; }
        public DelegateCommand<EditorialReview> EditorialReviewEditCommand { get; private set; }
        public DelegateCommand<EditorialReview> EditorialReviewDeleteCommand { get; private set; }

        public ObservableCollection<CollectionViewInfo> ItemCategoryInfos { get; private set; }
        public DelegateCommand<string> CategoryItemRelationAddCommand { get; private set; }
        public DelegateCommand<CategoryItemRelation> CategoryItemRelationEditCommand { get; private set; }
        public DelegateCommand<CategoryItemRelation> CategoryItemRelationDeleteCommand { get; private set; }

        public ICollectionView ItemAssetsImagesView { get; private set; }
        public ICollectionView ItemAssetsFilesView { get; private set; }
        public DelegateCommand<string> ItemAssetAddCommand { get; private set; }
        public DelegateCommand<ItemAsset> ItemAssetEditCommand { get; private set; }
        public DelegateCommand<ItemAsset> ItemAssetDeleteCommand { get; private set; }

        IAssociationGroupViewModel _associationGroupViewModelSelected;
        public object AssociationGroupViewModelSelected
        {
            get { return _associationGroupViewModelSelected; }
            set
            {
                _associationGroupViewModelSelected = value as IAssociationGroupViewModel;
                OnPropertyChanged();
            }
        }

        ObservableCollection<IAssociationGroupViewModel> _associationGroupViewModels;
        public ObservableCollection<IAssociationGroupViewModel> AssociationGroupViewModels
        {
            get
            {
                if (_associationGroupViewModels == null)
                {
                    _associationGroupViewModels = new ObservableCollection<IAssociationGroupViewModel>();
                    var itemsView = (IEditableCollectionView)CollectionViewSource.GetDefaultView(_associationGroupViewModels);
                    itemsView.NewItemPlaceholderPosition = NewItemPlaceholderPosition.AtEnd;
                }
                return _associationGroupViewModels;
            }
        }

        private int _itemCategoryViewsSelectedIndex;
        public int ItemCategoryViewsSelectedIndex
        {
            get { return _itemCategoryViewsSelectedIndex; }
            set { _itemCategoryViewsSelectedIndex = value; OnPropertyChanged(); CategoryItemRelationAddCommand.RaiseCanExecuteChanged(); }
        }

        internal static void RaiseAssetPickInteractionRequest(IViewModelsFactory<IPickAssetViewModel> vmFactory, InteractionRequest<Confirmation> confirmRequest, ICatalogEntityFactory entityFactory, Action<ItemAsset> finalAction)
        {
            var itemVM = vmFactory.GetViewModelInstance();
            itemVM.AssetPickMode = true;
            itemVM.RootItemId = null;

            confirmRequest.Raise(
                new ConditionalConfirmation(itemVM.Validate) { Content = itemVM, Title = "Select main image".Localize() },
                (x) =>
                {
                    if (x.Confirmed)
                    {
                        var item = (ItemAsset)entityFactory.CreateEntityForType(typeof(ItemAsset));
                        item.AssetId = itemVM.SelectedAsset.FolderItemId;
                        item.AssetType = itemVM.SelectedAsset.ContentType;
                        item.GroupName = "primaryimage";

                        finalAction(item);
                    }
                });
        }

        public bool HasPublishPermission(EditorialReview item)
        {
            return item != null &&
                (item.ReviewState != ReviewStateConverter.ToInt(ReviewState.Active) || _authContext.CheckPermission(PredefinedPermissions.CatalogEditorialReviewsPublish));
        }

        #region private members

        /// <summary>
        /// Inits the commands that are common in wizard and details dialogs
        /// </summary>
        private void InitCommands()
        {
            PropertiesAndValues = new ObservableCollection<PropertyAndPropertyValueBase>();
            PropertiesLocalesFilterCommand = new DelegateCommand<string>(RaisePropertiesLocalesFilter);
            PropertyValueEditCommand = new DelegateCommand<object>(RaisePropertyValueEditInteractionRequest, x => x != null);
            PropertyValueDeleteCommand = new DelegateCommand<object>(RaisePropertyValueDeleteInteractionRequest, x => x != null);

            PriceAddCommand = new DelegateCommand(RaisePriceAddInteractionRequest, () => PriceListSelected != null);
            PriceEditCommand = new DelegateCommand<Price>(RaisePriceEditInteractionRequest, x => x != null);
            PriceDeleteCommand = new DelegateCommand<Price>(RaisePriceDeleteInteractionRequest, x => x != null);
        }

        private void RaisePropertyValueEditInteractionRequest(object originalItemObject)
        {
            CategoryViewModel.RaisePropertyValueEditInteractionRequest<ItemPropertyValue>(_propertyValueVmFactory, CommonConfirmRequest, (ICatalogEntityFactory)EntityFactory, OnItemValueConfirmed, (PropertyAndPropertyValueBase)originalItemObject, FilterLanguage);
        }

        // function almost duplicated in CategoryViewModel
        private void OnItemValueConfirmed(PropertyAndPropertyValueBase originalItem, PropertyAndPropertyValueBase item)
        {
            if (!item.IsMultiValue)
            {
                if (originalItem.Property != null)
                {
                    item.Value.Name = originalItem.Property.Name;
                    item.Value.Locale = originalItem.Locale;
                }

                if (originalItem.Value == null)
                    InnerItem.ItemPropertyValues.Add((ItemPropertyValue)item.Value);
                else
                    UpdatePropertyValue(item.Value);
            }
            else
            {
                if (originalItem.Values == null)
                    originalItem.Values = new ObservableCollection<PropertyValueBase>();

                var listToRemove = originalItem.Values.Where(val => item.Values.All(y => y.PropertyValueId != val.PropertyValueId)).ToList();
                listToRemove.ForEach(x =>
                {
                    var itemToRemove = InnerItem.ItemPropertyValues.First(y => y.KeyValue == x.KeyValue);
                    InnerItem.ItemPropertyValues.Remove(itemToRemove);
                    originalItem.Values.Remove(x);
                });

                foreach (var value in item.Values)
                {
                    if (originalItem.Values.All(y => y.PropertyValueId != value.PropertyValueId))
                    {
                        InnerItem.ItemPropertyValues.Add(new ItemPropertyValue
                        {
                            ItemId = InnerItem.ItemId,
                            Name = item.PropertyName,
                            Locale = value.Locale,
                            KeyValue = value.PropertyValueId,
                            ValueType = item.PropertyValueType
                        });
                        originalItem.Values.Add(new PropertyValue()
                        {
                            Name = item.PropertyName,
                            Locale = value.Locale,
                            KeyValue = value.PropertyValueId,
                            ValueType = item.PropertyValueType
                        });
                    }
                }
            }

            // for GUI update
            originalItem.Value = originalItem.Value ?? item.Value;
            OnViewModelPropertyChangedUI(null, null);
        }

        // function almost duplicated in CategoryViewModel
        private void UpdatePropertyValue(PropertyValueBase newValue)
        {
            //TODO update value according to ValueType
            var item = InnerItem.ItemPropertyValues.FirstOrDefault(x => x.PropertyValueId == newValue.PropertyValueId);
            if (item == null)
            {
                item = CreateEntity<ItemPropertyValue>();
                InnerItem.ItemPropertyValues.Add(item);
            }
            item.InjectFrom(newValue);
            item.ItemId = InnerItem.ItemId;
        }

        // function almost duplicated in CategoryViewModel
        private void RaisePropertyValueDeleteInteractionRequest(object originalItemObject)
        {
            var originalItem = (PropertyAndPropertyValueBase)originalItemObject;
            var item = originalItem.Value as ItemPropertyValue;
            if (item != null)
            {
                InnerItem.ItemPropertyValues.Remove(item);
                if (originalItem.Property == null)
                {
                    PropertiesAndValues.Remove(originalItem);
                }
                else
                {
                    originalItem.Value = null;
                }

                OnViewModelPropertyChangedUI(null, null);
            }
            else if (originalItem.IsMultiValue && originalItem.Values != null)
            {
                foreach (var value in originalItem.Values)
                {
                    var itemToRemove = InnerItem.ItemPropertyValues.First(y => y.KeyValue == value.KeyValue);
                    InnerItem.ItemPropertyValues.Remove(itemToRemove);
                }

                originalItem.Values = null;
                OnViewModelCollectionChangedUI(null, null);
            }
        }

        private void RaiseAssociationGroupAddInteractionRequest()
        {
            var item = (AssociationGroup)EntityFactory.CreateEntityForType("AssociationGroup");
            item.Priority = 1;
            if (RaiseAssociationGroupEditInteractionRequest(item, "Add Association Group".Localize()))
            {
                InnerItem.AssociationGroups.Add(item);
                AssociationGroupViewModelsAdd(item);
                AssociationGroupViewModelSelected = AssociationGroupViewModels[AssociationGroupViewModels.Count - 1];
            }
        }

        private void RaiseAssociationGroupEditInteractionRequest(AssociationGroup originalItem)
        {
            var item = originalItem.DeepClone(EntityFactory as CatalogEntityFactory);
            if (RaiseAssociationGroupEditInteractionRequest(item, "Edit Association Group".Localize()))
            {
                // copy all values to original:
                OnUIThread(() => originalItem.InjectFrom<CloneInjection>(item));
            }
        }

        private bool RaiseAssociationGroupEditInteractionRequest(AssociationGroup item, string title)
        {
            bool result = false;
            var itemVM = _associationGroupEditVmFactory.GetViewModelInstance(
                new KeyValuePair<string, object>("item", item),
                new KeyValuePair<string, object>("selectedNames", InnerItem.AssociationGroups.Select(x => x.Name))
                );
            var confirmation = new ConditionalConfirmation(itemVM.Validate) { Title = title, Content = itemVM };

            CommonConfirmRequest.Raise(confirmation, (x) =>
            {
                result = x.Confirmed;
            });

            return result;
        }

        private void RaiseAssociationGroupDeleteInteractionRequest(object objectItem)
        {
            var item = (IAssociationGroupViewModel)objectItem;
            var confirmation = new ConditionalConfirmation
            {
                Content = string.Format("Are you sure you want to delete Association Group '{0}'?".Localize(), item.InnerItem.Name),
                Title = "Delete confirmation".Localize(null, LocalizationScope.DefaultCategory)
            };

            CommonConfirmRequest.Raise(confirmation, (x) =>
            {
                if (x.Confirmed)
                {
                    item.InnerItem.Associations.Clear();
                    InnerItem.AssociationGroups.Remove(item.InnerItem);
                    AssociationGroupViewModels.Remove(item);
                }
            });
        }

        private void InitializeAssociationGroupViewModels()
        {
            AssociationGroupViewModels.Clear();
            foreach (var item in InnerItem.AssociationGroups)
            {
                AssociationGroupViewModelsAdd(item);
            }

            if (AssociationGroupViewModels.Count > 0)
                AssociationGroupViewModelSelected = AssociationGroupViewModels[0];
        }

        private void AssociationGroupViewModelsAdd(AssociationGroup item)
        {
            item.PropertyChanged += ViewModel_PropertyChanged;
            foreach (var association in item.Associations)
            {
                association.PropertyChanged += ViewModel_PropertyChanged;
            }
            item.Associations.CollectionChanged += ViewModel_PropertyChanged;

            var itemVM = _associationGroupVmFactory.GetViewModelInstance(
                new KeyValuePair<string, object>("item", item),
                new KeyValuePair<string, object>("parent", this)
                );
            AssociationGroupViewModels.Add(itemVM);
        }

        #region Properties tab

        public string FilterLanguage { get; private set; }
        public List<string> InnerItemCatalogLanguages { get; protected set; }
        public ObservableCollection<PropertyAndPropertyValueBase> PropertiesAndValues { get; protected set; }
        public DelegateCommand<string> PropertiesLocalesFilterCommand { get; private set; }

        // function almost duplicated in CategoryViewModel
        protected void InitializePropertiesAndValues()
        {
            if (IsWizardMode)
            {
                InnerItemCatalogLanguages = new List<string>();
                InnerItemCatalogLanguages.Add(InnerItem.Catalog.DefaultLanguage);
            }
            else
            {
                var innerItemCatalog = (catalogModel.Catalog)InnerItem.Catalog;
                // query catalog languages
                if (innerItemCatalog.CatalogLanguages.Count == 0)
                {
                    var catalogLanguages = ItemRepository.Catalogs
                        .OfType<catalogModel.Catalog>()
                        .Where(x => x.CatalogId == innerItemCatalog.CatalogId)
                        .Expand(x => x.CatalogLanguages)
                        .Single()
                        .CatalogLanguages.ToList();
                    innerItemCatalog.CatalogLanguages.Add(catalogLanguages);
                }
                InnerItemCatalogLanguages = innerItemCatalog.CatalogLanguages.Select(x => x.Language).ToList();
            }

            OnPropertyChanged("InnerItemCatalogLanguages");

            // filter values by locale
            PropertiesLocalesFilterCommand.Execute(InnerItem.Catalog.DefaultLanguage);

            CategoryViewModel.SetupPropertiesAndValues(InnerItem.PropertySet, InnerItem.ItemPropertyValues, InnerItemCatalogLanguages, PropertiesAndValues, IsWizardMode);
        }

        // function duplicated in CategoryViewModel
        private void RaisePropertiesLocalesFilter(string locale)
        {
            var view = CollectionViewSource.GetDefaultView(PropertiesAndValues);
            view.Filter = (x =>
                {
                    var propertyAndPropertyValue = (PropertyAndPropertyValueBase)x;
                    var result = string.IsNullOrEmpty(propertyAndPropertyValue.Locale) || propertyAndPropertyValue.Locale.Equals(locale, StringComparison.InvariantCultureIgnoreCase);
                    return result;
                });

            FilterLanguage = locale;
            OnPropertyChanged("FilterLanguage");
        }

        #endregion

        #region Pricing tab

        public List<Pricelist> PriceLists { get; set; }

        private Pricelist _PriceListSelected;
        public Pricelist PriceListSelected
        {
            get { return _PriceListSelected; }
            set { _PriceListSelected = value; PriceRaiseCanExecuteChanged(); }
        }

        protected void InitializePricing()
        {
            _priceListRepository = _pricelistRepositoryFactory.GetRepositoryInstance();
            var itemPrices = _priceListRepository.Prices.Where(x => x.ItemId == InnerItem.ItemId)
              .Expand("CatalogItem")
              .Expand("Pricelist").ToList();

            var priceListAssignments = _priceListRepository.PricelistAssignments.Expand(x => x.Pricelist).ToList();
            var priceListsList = priceListAssignments.Select(x => x.Pricelist).Distinct();

            // assigning price to pricelist
            foreach (var priceList in priceListsList)
            {
                var priceListPrices = itemPrices.Where(x => x.Pricelist == priceList).ToList();
                priceListPrices.ForEach(x => priceList.Prices.Add(x));
            }
            PriceLists = priceListsList.ToList();
            OnPropertyChanged("PriceLists");

            if (PriceLists.Count == 1)
            {
                PriceListSelected = PriceLists[0];
                OnPropertyChanged("PriceListSelected");
            }

            IsInitializingPricing = false;
        }

        public void PriceRaiseCanExecuteChanged()
        {
            PriceAddCommand.RaiseCanExecuteChanged();
            PriceEditCommand.RaiseCanExecuteChanged();
            PriceDeleteCommand.RaiseCanExecuteChanged();
        }

        private void RaisePriceAddInteractionRequest()
        {
            var item = (Price)EntityFactory.CreateEntityForType("Price");
            item.MinQuantity = 1;
            item.ItemId = InnerItem.ItemId;
            item.PricelistId = PriceListSelected.PricelistId;
            bool b = RaisePriceEditInteractionRequest(item, PriceListSelected.Prices, "Add Price".Localize());
            if (b)
            {
                //item.PricelistId = PriceListSelected.PricelistId;
                item.CatalogItem = InnerItem;
                //item.ItemId = InnerItem.ItemId;
                //_priceListRepository.Add(item);
                PriceListSelected.Prices.Add(item);
            }
        }

        private void RaisePriceEditInteractionRequest(Price originalItem)
        {
            var item = originalItem.DeepClone(EntityFactory as CatalogEntityFactory);
            if (RaisePriceEditInteractionRequest(item, PriceListSelected.Prices, "Edit Price".Localize()))
            {
                OnUIThread(() => originalItem.InjectFrom<CloneInjection>(item));
            }
        }

        private bool RaisePriceEditInteractionRequest(Price item, Collection<Price> prices, string title)
        {
            var result = false;
            var itemVM = _priceVmFactory.GetViewModelInstance(
                new KeyValuePair<string, object>("item", item),
                new KeyValuePair<string, object>("preloadedItems", prices),
                new KeyValuePair<string, object>("isAllFieldsVisible", false)
                );
            var confirmation = new ConditionalConfirmation(itemVM.Validate) { Title = title, Content = itemVM };

            CommonConfirmRequest.Raise(confirmation, (x) =>
            {
                result = x.Confirmed;
            });

            return result;
        }

        private void RaisePriceDeleteInteractionRequest(Price item)
        {
            var confirmation = new ConditionalConfirmation
            {
                Content = string.Format("Are you sure you want to delete Price '{0}'?".Localize(), item.PriceId),
                Title = "Delete confirmation".Localize(null, LocalizationScope.DefaultCategory)
            };

            CommonConfirmRequest.Raise(confirmation, (x) =>
            {
                if (x.Confirmed)
                {
                    PriceListSelected.Prices.Remove(item);
                }
            });
        }
        #endregion

        #region RELATIONS tab

        public void ItemRelationRaiseCanExecuteChanged()
        {
            ItemRelationEditCommand.RaiseCanExecuteChanged();
            ItemRelationDeleteCommand.RaiseCanExecuteChanged();
        }

        private void RaiseItemRelationAddInteractionRequest()
        {
            var item = (ItemRelation)EntityFactory.CreateEntityForType("ItemRelation");
            item.Quantity = 1;
            item.Priority = 1;
            item.ParentItemId = InnerItem.ItemId;
            if (RaiseItemRelationEditInteractionRequest(item, "Add Relation".Localize()))
            {
                if (InnerItem is Product)
                    item.RelationTypeId = ItemRelationType.Sku;
                else if (InnerItem is Bundle)
                    item.RelationTypeId = ItemRelationType.PackageItem; // ???
                item.ParentItemId = InnerItem.ItemId;

                ItemRelations.Add(item);
            }
        }

        private void RaiseItemRelationEditInteractionRequest(ItemRelation originalItem)
        {
            var item = originalItem.DeepClone(EntityFactory as CatalogEntityFactory);
            if (RaiseItemRelationEditInteractionRequest(item, "Edit Relation".Localize()))
            {
                ItemRepository.Attach(item.ChildItem);
                // copy all values to original:
                OnUIThread(() =>
                    {
                        //originalItem.InjectFrom<CloneInjection>(item);
                        originalItem.InjectFrom(item); // use shallow inject, not deep
                        // fake assign for UI triggers to display updated values.
                        originalItem.ChildItemId = originalItem.ChildItemId;
                    });
            }
        }

        private bool RaiseItemRelationEditInteractionRequest(ItemRelation item, string title)
        {
            var result = false;
            var itemVM = _itemRelationVmFactory.GetViewModelInstance(
                new KeyValuePair<string, object>("item", item),
                new KeyValuePair<string, object>("parent", this)
                );
            var confirmation = new ConditionalConfirmation(itemVM.Validate) { Title = title, Content = itemVM };

            CommonConfirmRequest.Raise(confirmation, (x) =>
            {
                result = x.Confirmed;
            });

            return result;
        }

        private void RaiseItemRelationDeleteInteractionRequest(ItemRelation item)
        {
            var confirmation = new ConditionalConfirmation
            {
                Content = string.Format("Are you sure you want to delete Relation '{0}'?".Localize(), item.ItemRelationId),
                Title = "Delete confirmation".Localize(null, LocalizationScope.DefaultCategory)
            };

            CommonConfirmRequest.Raise(confirmation, (x) =>
            {
                if (x.Confirmed)
                {
                    ItemRelations.Remove(item);
                }
            });
        }

        private void Initialize_ItemRelations_ForSave()
        {
            foreach (var removedItem in ItemRelations.RemovedItems)
            {
                var item = ItemRelations.OriginalItems.FirstOrDefault(x => x.ItemRelationId == removedItem.ItemRelationId);
                if (item != null)
                {
                    Repository.Remove(item);
                }
            }

            foreach (var updatedItem in ItemRelations.UpdatedItems)
            {
                var item = ItemRelations.OriginalItems.SingleOrDefault(x => x.ItemRelationId == updatedItem.ItemRelationId);
                if (item != null)
                {
                    item.InjectFrom(updatedItem);
                }
            }

            foreach (var item in ItemRelations.AddedItems)
            {
                Repository.Add(item);
            }
        }

        #endregion

        #region EDITORIAL REVIEWS tab

        public void EditorialReviewRaiseCanExecuteChanged()
        {
            EditorialReviewEditCommand.RaiseCanExecuteChanged();
            EditorialReviewDeleteCommand.RaiseCanExecuteChanged();
        }

        private void RaiseEditorialReviewAddInteractionRequest()
        {
            var item = (EditorialReview)EntityFactory.CreateEntityForType("EditorialReview");
            item.CatalogItem = InnerItem;
            item.Locale = InnerItem.Catalog.DefaultLanguage;
            item.Priority = 1;
            RaiseEditorialReviewEditInteractionRequest(item, "Add Editorial Review".Localize());
        }

        private void RaiseEditorialReviewEditInteractionRequest(EditorialReview originalItem)
        {
            RaiseEditorialReviewEditInteractionRequest(originalItem, "Edit Editorial Review".Localize());
        }

        private void RaiseEditorialReviewEditInteractionRequest(EditorialReview item, string title)
        {
            var itemVM = _reviewVmFactory.GetViewModelInstance(
                new KeyValuePair<string, object>("item", item),
                new KeyValuePair<string, object>("isWizardMode", false)
                );
            // confirmation.Title = title;

            if (itemVM is IOpenTracking)
            {
                var openTracking = itemVM as IOpenTracking;
                openTracking.OpenItemCommand.Execute();
            }
        }

        private void RaiseEditorialReviewDeleteInteractionRequest(EditorialReview item)
        {
            var confirmation = new ConditionalConfirmation
            {
                Content = string.Format("Are you sure you want to delete Editorial Review '{0}'?".Localize(), item.EditorialReviewId),
                Title = "Delete confirmation".Localize(null, LocalizationScope.DefaultCategory)
            };

            CommonConfirmRequest.Raise(confirmation, (x) =>
            {
                if (x.Confirmed)
                {
                    InnerItem.EditorialReviews.Remove(item);
                }
            });
        }
        #endregion

        #region CATEGORies tab
        private void RaiseCategoryItemRelationAddInteractionRequest(string catalogID)
        {
            var item = CreateEntity<CategoryItemRelation>();
            item.CatalogId = catalogID;
            item.Priority = 1;
            if (RaiseCategoryItemRelationEditInteractionRequest(item, "Add Item Category".Localize()))
            {
                InnerItem.CategoryItemRelations.Add(item);

                CategoryItemRelationAddCommand.RaiseCanExecuteChanged();
            }
        }

        private void RaiseCategoryItemRelationEditInteractionRequest(CategoryItemRelation originalItem)
        {
            var item = originalItem.DeepClone(EntityFactory as IKnownSerializationTypes);
            if (RaiseCategoryItemRelationEditInteractionRequest(item, "Edit Item Category".Localize()))
            {
                // copy all values to original:
                OnUIThread(() => originalItem.InjectFrom<CloneInjection>(item));

                ItemCategoryInfos.Clear();
                CreateViewForCategoriesInCatalog(InnerItem.CatalogId);

                ItemRepository.Catalogs
                    .OfType<VirtualCatalog>().ToArray()
                    .Select(x => x.CatalogId).ToList()
                    .ForEach(CreateViewForCategoriesInCatalog);
                OnPropertyChanged("ItemCategoryInfos");
            }
        }

        private bool RaiseCategoryItemRelationEditInteractionRequest(CategoryItemRelation item, string title)
        {
            var result = false;
            var itemVM = _categoryVmFactory.GetViewModelInstance(
                new KeyValuePair<string, object>("item", item)
                );
            var confirmation = new ConditionalConfirmation(itemVM.Validate) { Title = title, Content = itemVM };

            CommonConfirmRequest.Raise(confirmation, (x) =>
            {
                result = x.Confirmed;
            });

            return result;
        }

        private void RaiseCategoryItemRelationDeleteInteractionRequest(CategoryItemRelation item)
        {
            var confirmation = new ConditionalConfirmation
            {
                Content = string.Format("Are you sure you want to delete Item Category '{0}'?".Localize(), ((Category)item.Category).Name),
                Title = "Delete confirmation".Localize(null, LocalizationScope.DefaultCategory)
            };

            CommonConfirmRequest.Raise(confirmation, (x) =>
            {
                if (x.Confirmed)
                {
                    InnerItem.CategoryItemRelations.Remove(item);
                    CategoryItemRelationAddCommand.RaiseCanExecuteChanged();
                }
            });
        }
        #endregion

        #region ASSETS tab
        private void RaiseItemAssetAddInteractionRequest(string assetType)
        {
            var item = CreateEntity<ItemAsset>();
            item.AssetType = assetType;
            if (RaiseItemAssetEditInteractionRequest(item, "Add Asset".Localize()))
            {
                InnerItem.ItemAssets.Add(item);
            }
        }

        private void RaiseItemAssetEditInteractionRequest(ItemAsset originalItem)
        {
            var item = originalItem.DeepClone(EntityFactory as CatalogEntityFactory);
            if (RaiseItemAssetEditInteractionRequest(item, "Edit Asset".Localize()))
            {
                // copy all values to original:
                OnUIThread(() => originalItem.InjectFrom<CloneInjection>(item));
            }
        }

        private bool RaiseItemAssetEditInteractionRequest(ItemAsset item, string title)
        {
            var result = false;
            var itemVM = _assetVmFactory.GetViewModelInstance(new KeyValuePair<string, object>("item", item));
            var confirmation = new ConditionalConfirmation(itemVM.Validate) { Title = title, Content = itemVM };

            CommonConfirmRequest.Raise(confirmation, (x) =>
            {
                result = x.Confirmed;
            });

            return result;
        }

        private void RaiseItemAssetDeleteInteractionRequest(ItemAsset item)
        {
            var confirmation = new ConditionalConfirmation
            {
                Content = string.Format("Are you sure you want to delete Asset '{0}'?".Localize(), item.AssetId),
                Title = "Delete confirmation".Localize(null, LocalizationScope.DefaultCategory)
            };

            CommonConfirmRequest.Raise(confirmation, (x) =>
            {
                if (x.Confirmed)
                {
                    InnerItem.ItemAssets.Remove(item);
                }
            });
        }
        #endregion

        #region SEO tab

        public IItemSeoViewModel SeoStepViewModel { get; private set; }

        protected void InitSeoStep()
        {
            var itemParameter = new KeyValuePair<string, object>("item", InnerItem);
            var languagesParameter = new KeyValuePair<string, object>("languages", InnerItemCatalogLanguages ?? new List<string>());
            SeoStepViewModel =
                    _seoVmFactory.GetViewModelInstance(itemParameter, languagesParameter);
            OnPropertyChanged("SeoStepViewModel");
        }

        #endregion

        protected T CreateEntity<T>()
        {
            return (T)EntityFactory.CreateEntityForType(EntityFactory.GetEntityTypeStringName(typeof(T)));
        }

        private void CreateViewForCategoriesInCatalog(string x)
        {
            // can't use CollectionViewSource.GetDefaultView as it always returns THE SAME OBJECT!
            var view = new ListCollectionView(InnerItem.CategoryItemRelations)
                {
                    Filter = (y => y is CategoryItemRelation && ((CategoryItemRelation)y).CatalogId == x)
                };

            var info = new CollectionViewInfo { CatalogId = x, View = view };
            ItemCategoryInfos.Add(info);
        }

        #endregion
    }

    public class CollectionViewInfo
    {
        public ICollectionView View { get; set; }
        public string CatalogId { get; set; }
    }
}