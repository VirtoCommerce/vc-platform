using Microsoft.Practices.Prism.Commands;
using Omu.ValueInjecter;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using VirtoCommerce.Client.Globalization;
using VirtoCommerce.Foundation.AppConfig.Repositories;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.Foundation.Frameworks.ConventionInjections;
using VirtoCommerce.Foundation.Frameworks.Extensions;
using VirtoCommerce.Foundation.Marketing.Factories;
using VirtoCommerce.Foundation.Marketing.Model.DynamicContent;
using VirtoCommerce.Foundation.Marketing.Repositories;
using VirtoCommerce.Foundation.Orders.Repositories;
using VirtoCommerce.Foundation.Stores.Repositories;
using VirtoCommerce.ManagementClient.Catalog.ViewModel.Catalog.Interfaces;
using VirtoCommerce.ManagementClient.Core.Controls;
using VirtoCommerce.ManagementClient.Core.Infrastructure;
using VirtoCommerce.ManagementClient.Core.Infrastructure.Navigation;
using VirtoCommerce.ManagementClient.DynamicContent.Model;
using VirtoCommerce.ManagementClient.DynamicContent.ViewModel.ContentPublishing.Interfaces;
using VirtoCommerce.ManagementClient.DynamicContent.ViewModel.Wizard.Interfaces;
using VirtoCommerce.ManagementClient.Localization;

namespace VirtoCommerce.ManagementClient.DynamicContent.ViewModel.ContentPublishing.Implementations
{
    public class ContentPublishingItemViewModel : ViewModelDetailAndWizardBase<DynamicContentPublishingGroup>, IContentPublishingItemViewModel
    {
        #region Dependencies

        private readonly INavigationManager _navManager;
        private readonly IRepositoryFactory<IDynamicContentRepository> _repositoryFactory;
        private readonly IRepositoryFactory<IStoreRepository> _storeRepositoryFactory;
        private readonly IViewModelsFactory<ISearchCategoryViewModel> _searchCategoryVmFactory;
        private readonly IRepositoryFactory<ICountryRepository> _countryRepositoryFactory;

        #endregion

        #region Constructor

        public ContentPublishingItemViewModel(
            IRepositoryFactory<IAppConfigRepository> appConfigRepositoryFactory,
            IRepositoryFactory<ICountryRepository> countryRepositoryFactory,
            IViewModelsFactory<ISearchCategoryViewModel> searchCategoryVmFactory,
            IRepositoryFactory<IDynamicContentRepository> repositoryFactory,
            IDynamicContentEntityFactory entityFactory,
            IRepositoryFactory<IStoreRepository> storeRepositoryFactory,
            INavigationManager navManager,
            DynamicContentPublishingGroup item)
            : base(entityFactory, item, false)
        {
            ViewTitle = new ViewTitleBase
            {
                Title = "Content Publishing",
                SubTitle = (item != null && !String.IsNullOrEmpty(item.Name)) ? item.Name.ToUpper(CultureInfo.InvariantCulture) : ""
            };

            AppConfigRepositoryFactory = appConfigRepositoryFactory;
            _countryRepositoryFactory = countryRepositoryFactory;
            _storeRepositoryFactory = storeRepositoryFactory;
            _searchCategoryVmFactory = searchCategoryVmFactory;
            _repositoryFactory = repositoryFactory;
            _navManager = navManager;

            OpenItemCommand = new DelegateCommand(() => _navManager.Navigate(NavigationData));
        }

        protected ContentPublishingItemViewModel(
            IRepositoryFactory<IAppConfigRepository> appConfigRepositoryFactory,
            IRepositoryFactory<ICountryRepository> countryRepositoryFactory,
            IViewModelsFactory<ISearchCategoryViewModel> searchCategoryVmFactory,
            IRepositoryFactory<IDynamicContentRepository> repositoryFactory,
            IRepositoryFactory<IStoreRepository> storeRepositoryFactory,
            IDynamicContentEntityFactory entityFactory,
            DynamicContentPublishingGroup item)
            : base(entityFactory, item, true)
        {
            AppConfigRepositoryFactory = appConfigRepositoryFactory;
            _countryRepositoryFactory = countryRepositoryFactory;
            _repositoryFactory = repositoryFactory;
            _storeRepositoryFactory = storeRepositoryFactory;
            _searchCategoryVmFactory = searchCategoryVmFactory;
        }

        #endregion

        #region ViewModelBase members

        public override string DisplayName
        {
            get
            {
                return OriginalItem == null ? string.Empty : OriginalItem.Name;
            }
        }

        public override string IconSource
        {
            get
            {
                return "Icon_ContentPublishing";
            }
        }

        public override Brush ShellDetailItemMenuBrush
        {
            get
            {
                var result =
                    (SolidColorBrush)Application.Current.TryFindResource("ContentPublishingDetailItemMenuBrush");

                return result ?? base.ShellDetailItemMenuBrush;
            }
        }

        private NavigationItem _navigationData;
        public override NavigationItem NavigationData
        {
            get
            {
                return _navigationData ??
                       (_navigationData = new NavigationItem(GetNavigationKey(OriginalItem.DynamicContentPublishingGroupId),
                            NavigationNames.HomeName, NavigationNames.MenuName, this));
            }
        }

        #endregion

        #region ViewModelDetailAndWizardBase members

        public override string ExceptionContextIdentity
        {
            get { return string.Format("Content publishing ({0})", DisplayName); }
        }

        protected override void GetRepository()
        {
            Repository = _repositoryFactory.GetRepositoryInstance();
        }

        protected override bool HasPermission()
        {
            return true;
        }

        protected override bool IsValidForSave()
        {
            return InnerItem.Validate();
        }

        protected override RefusedConfirmation CancelConfirm()
        {
            return new RefusedConfirmation
            {
                Content = string.Format("Save changes to content publishing '{0}'?".Localize(), DisplayName),
                Title = "Action confirmation".Localize(null, LocalizationScope.DefaultCategory)
            };
        }

        protected override void LoadInnerItem()
        {
            try
            {
                var item = (Repository as IDynamicContentRepository).PublishingGroups.Where(
                    x => x.DynamicContentPublishingGroupId == OriginalItem.DynamicContentPublishingGroupId)
                    .Expand(x => x.ContentItems)
                    .Expand(x => x.ContentPlaces)
                    .SingleOrDefault();

                OnUIThread(() => InnerItem = item);
            }
            catch (Exception ex)
            {
                ShowErrorDialog(ex, string.Format("An error occurred when trying to load {0}",
                    ExceptionContextIdentity));
            }
        }

        protected override void InitializePropertiesForViewing()
        {
            if (!IsWizardMode)
            {
                InitializeExpressionElementBlock();
                InitializeDynamicContent();
                InitializeContentPlaces();
            }
        }

        protected override void BeforeSaveChanges()
        {
            if (!IsWizardMode)
            {
                UpdateFromExpressionElementBlock();
                UpdateContentPlacesNotWizard();
                UpdateContentPlacesItems();
                UpdateDynamicContentItems();
            }

        }

        protected override void AfterSaveChangesUI()
        {
            OriginalItem.InjectFrom(InnerItem);
        }

        protected override void SetSubscriptionUI()
        {
            AttachExpressionBuilderHandlers(ExpressionElementBlock);

            if (InnerItemDynamicContent != null)
                InnerItemDynamicContent.CollectionChanged += ViewModel_PropertyChanged;

            if (InnerItemContentPlaces != null)
                InnerItemContentPlaces.CollectionChanged += ViewModel_PropertyChanged;
        }

        protected override void CloseSubscriptionUI()
        {
            DetachExpressionBuilderHandlers(ExpressionElementBlock);

            if (InnerItemDynamicContent != null)
                InnerItemDynamicContent.CollectionChanged -= ViewModel_PropertyChanged;

            if (InnerItemContentPlaces != null)
                InnerItemContentPlaces.CollectionChanged -= ViewModel_PropertyChanged;
        }

        #endregion

        #region IWizardStep members

        public override bool IsValid
        {
            get
            {
                var retval = true;
                if (this is IContentPublishingOverviewStepViewModel)
                {
                    InnerItem.Validate(false);

                    if (InnerItem.Errors.Count > 0 ||
                        String.IsNullOrEmpty(InnerItem.Name))
                    {
                        retval = false;
                        InnerItem.Errors.Clear();
                    }
                }
                else if (this is IContentPublishingDynamicContentStepViewModel)
                {
                    InnerItem.Validate(false);

                    if (InnerItemDynamicContent.Count == 0)
                        retval = false;
                }
                else if (this is IContentPublishingContentPlacesStepViewModel)
                {
                    InnerItem.Validate(false);

                    if (InnerItemContentPlaces.Count == 0)
                        retval = false;
                }

                return retval;
            }
        }

        public override bool IsLast
        {
            get
            {
                return this is IContentPublishingConditionsStepViewModel;
            }
        }

        public override string Comment
        {
            get
            {
                return string.Empty;
            }
        }

        public override string Description
        {
            get
            {
                var result = string.Empty;
                if (this is IContentPublishingOverviewStepViewModel)
                    result = string.Format("Enter Content publishing group details".Localize());
                else if (this is IContentPublishingContentPlacesStepViewModel)
                    result = "Select content places".Localize();
                else if (this is IContentPublishingDynamicContentStepViewModel)
                    result = "Select dynamic content".Localize();
                else if (this is IContentPublishingConditionsStepViewModel)
                    result = "Set availability conditions".Localize();

                return result;
            }
        }

        #endregion

        #region Commands

        public DelegateCommand<DynamicContentItemProperty> PropertyValueEditCommand
        {
            get;
            private set;
        }

        #endregion

        #region IContentPublishingItemViewModel

        public IRepositoryFactory<ICountryRepository> CountryRepositoryFactory { get { return _countryRepositoryFactory; } }

        public IViewModelsFactory<ISearchCategoryViewModel> SearchCategoryVmFactory { get { return _searchCategoryVmFactory; } }

        public IRepositoryFactory<IStoreRepository> StoreRepositoryFactory { get { return _storeRepositoryFactory; } }
        public IRepositoryFactory<IAppConfigRepository> AppConfigRepositoryFactory { get; private set; }

        public TypedExpressionElementBase ExpressionElementBlock { get; set; }

        protected override void DoDuplicate()
        {
            var item = new DynamicContentPublishingGroup();

            //OnUIThread(() => item.InjectFrom<CloneInjection>(InnerItem));
            item.InjectFrom<CloneInjection>(InnerItem);

            item.DynamicContentPublishingGroupId = item.GenerateNewKey();
            item.Name = item.Name + "_1";

            item.ContentItems.ToList().ForEach(x =>
            {
                x.PublishingGroupContentItemId = x.GenerateNewKey();
            });

            item.ContentPlaces.ToList().ForEach(x =>
            {
                x.PublishingGroupContentPlaceId = x.GenerateNewKey();
            });

            Repository.Add(item);
        }

        public ObservableCollection<DynamicContentItem> AllAvailableDynamicContent
        {
            get;
            private set;
        }
        public ObservableCollection<DynamicContentItem> InnerItemDynamicContent { get; private set; }

        public ObservableCollection<DynamicContentPlace> AllAvailableContentPlaces
        {
            get;
            private set;
        }
        public ObservableCollection<DynamicContentPlace> InnerItemContentPlaces { get; private set; }

        #endregion

        #region Private and protected methods

        protected void UpdateFromExpressionElementBlock()
        {
            if (((ConditionAndOrBlock)ExpressionElementBlock.Children[0]).Children.Any())
            {
                InnerItem.PredicateVisualTreeSerialized = SerializationUtil.Serialize(ExpressionElementBlock);
                InnerItem.ConditionExpression =
                    SerializationUtil.SerializeExpression(((ContentPublishingExpressionBlock)ExpressionElementBlock).GetExpression());
            }
            else
            {
                InnerItem.PredicateVisualTreeSerialized = null;
                InnerItem.ConditionExpression = null;
            }
        }

        protected void UpdateDynamicContentItems()
        {
            if (!IsWizardMode)
            {
                //delete old items
                var originalItems = InnerItem.ContentItems.ToList();
                foreach (var originalItem in originalItems)
                {
                    if (InnerItemDynamicContent.All(iidc => iidc.DynamicContentItemId != originalItem.DynamicContentItemId))
                    {
                        InnerItem.ContentItems.Remove(originalItem);
                        Repository.Attach(originalItem);
                        Repository.Remove(originalItem);
                    }
                }

                if (!IsWizardMode)
                    Repository.UnitOfWork.Commit();
            }

            foreach (var dynamicContentItem in InnerItemDynamicContent)
            {
                var newItem = new PublishingGroupContentItem() { DynamicContentItemId = dynamicContentItem.DynamicContentItemId, DynamicContentPublishingGroupId = InnerItem.DynamicContentPublishingGroupId };

                var existItem =
                    InnerItem.ContentItems.SingleOrDefault(x => x.DynamicContentItemId == dynamicContentItem.DynamicContentItemId);
                if (existItem == null)
                {
                    InnerItem.ContentItems.Add(newItem);
                }
            }
        }

        protected void UpdateContentPlacesItems()
        {
            foreach (var innerItemContentPlace in InnerItemContentPlaces)
            {
                var newItem = new PublishingGroupContentPlace()
                {
                    DynamicContentPlaceId = innerItemContentPlace.DynamicContentPlaceId,
                    DynamicContentPublishingGroupId = InnerItem.DynamicContentPublishingGroupId
                };

                var existItem =
                    InnerItem.ContentPlaces.SingleOrDefault(
                        x => x.DynamicContentPlaceId == innerItemContentPlace.DynamicContentPlaceId);
                if (existItem == null)
                {
                    InnerItem.ContentPlaces.Add(newItem);
                }
            }
        }

        private void UpdateContentPlacesNotWizard()
        {
            //delete old items
            var originalItems = InnerItem.ContentPlaces.ToList();

            foreach (var originalItem in originalItems)
            {
                if (InnerItemContentPlaces.All(iicp => iicp.DynamicContentPlaceId != originalItem.DynamicContentPlaceId))
                {
                    InnerItem.ContentPlaces.Remove(originalItem);
                    Repository.Attach(originalItem);
                    Repository.Remove(originalItem);
                }
            }

            Repository.UnitOfWork.Commit();

        }

        private void AttachExpressionBuilderHandlers(INotifyPropertyChanged expressionBlock)
        {
            if (expressionBlock != null)
            {
                expressionBlock.PropertyChanged += ViewModel_PropertyChanged;
                if (expressionBlock is CompositeElement)
                {
                    (expressionBlock as CompositeElement).Children.CollectionChanged += ViewModel_PropertyChanged;
                    (expressionBlock as CompositeElement).HeaderElements.CollectionChanged += ViewModel_PropertyChanged;
                    (expressionBlock as CompositeElement).Children.ToList().ForEach(AttachExpressionBuilderHandlers);
                    (expressionBlock as CompositeElement).HeaderElements.ToList().ForEach(AttachExpressionBuilderHandlers);
                }
            }
        }

        private void DetachExpressionBuilderHandlers(INotifyPropertyChanged expressionBlock)
        {
            if (expressionBlock != null)
            {
                expressionBlock.PropertyChanged -= ViewModel_PropertyChanged;
                if (expressionBlock is CompositeElement)
                {
                    (expressionBlock as CompositeElement).Children.CollectionChanged -= ViewModel_PropertyChanged;
                    (expressionBlock as CompositeElement).HeaderElements.CollectionChanged -= ViewModel_PropertyChanged;
                    (expressionBlock as CompositeElement).Children.ToList().ForEach(DetachExpressionBuilderHandlers);
                    (expressionBlock as CompositeElement).HeaderElements.ToList().ForEach(DetachExpressionBuilderHandlers);
                }
            }
        }

        #endregion

        #region Protected Initialize methods

        protected void InitializeExpressionElementBlock()
        {

            OnUIThread(() =>
            {
                if (InnerItem.PredicateVisualTreeSerialized == null)
                {
                    ExpressionElementBlock = new ContentPublishingExpressionBlock(this);
                }
                else
                {
                    ExpressionElementBlock =
                        SerializationUtil.Deserialize<TypedExpressionElementBase>(InnerItem.PredicateVisualTreeSerialized);
                    ExpressionElementBlock.InitializeAfterDeserialized(this);
                }
                OnPropertyChanged("ExpressionElementBlock");
            });

        }

        protected void InitializeDynamicContent()
        {
            if (AllAvailableDynamicContent == null)
            {

                AllAvailableDynamicContent = new ObservableCollection<DynamicContentItem>();

                OnUIThread(() => { AllAvailableDynamicContent.Add((Repository as IDynamicContentRepository).Items); });
                //AllAvailableDynamicContent.Add(repository.Items);


                OnPropertyChanged("AllAvailableDynamicContent");
            }

            // currently selected dynamic content
            InnerItemDynamicContent = new ObservableCollection<DynamicContentItem>(
                AllAvailableDynamicContent.Where(x =>
                    InnerItem.ContentItems.Any(y =>
                        y.DynamicContentItemId.Equals(x.DynamicContentItemId,
                            StringComparison.InvariantCultureIgnoreCase))));
            OnPropertyChanged("InnerItemDynamicContent");
        }

        protected void InitializeContentPlaces()
        {
            //get all available content places items for multiselect control
            if (AllAvailableContentPlaces == null)
            {

                AllAvailableContentPlaces = new ObservableCollection<DynamicContentPlace>();
                OnUIThread(() => { AllAvailableContentPlaces.Add((Repository as IDynamicContentRepository).Places); });

                OnPropertyChanged("AllAvailableContentPlaces");
            }

            // currently selected content places
            InnerItemContentPlaces = new ObservableCollection<DynamicContentPlace>(
                AllAvailableContentPlaces.Where(x =>
                    InnerItem.ContentPlaces.Any(y =>
                        y.DynamicContentPlaceId.Equals(x.DynamicContentPlaceId,
                            StringComparison.InvariantCultureIgnoreCase))));

            OnPropertyChanged("InnerItemContentPlaces");
        }

        #endregion
    }
}
