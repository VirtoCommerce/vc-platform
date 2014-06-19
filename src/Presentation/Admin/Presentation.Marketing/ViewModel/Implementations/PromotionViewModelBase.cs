using Microsoft.Practices.Prism.Commands;
using Omu.ValueInjecter;
using System;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using VirtoCommerce.Client.Globalization;
using VirtoCommerce.Foundation.AppConfig.Repositories;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.Foundation.Frameworks.ConventionInjections;
using VirtoCommerce.Foundation.Frameworks.Extensions;
using VirtoCommerce.Foundation.Marketing.Factories;
using VirtoCommerce.Foundation.Marketing.Model;
using VirtoCommerce.Foundation.Marketing.Repositories;
using VirtoCommerce.Foundation.Orders.Repositories;
using VirtoCommerce.ManagementClient.Catalog.ViewModel.Catalog.Interfaces;
using VirtoCommerce.ManagementClient.Core.Controls;
using VirtoCommerce.ManagementClient.Core.Infrastructure;
using VirtoCommerce.ManagementClient.Core.Infrastructure.Navigation;
using VirtoCommerce.ManagementClient.Localization;
using VirtoCommerce.ManagementClient.Marketing.Infrastructure;
using VirtoCommerce.ManagementClient.Marketing.Model;
using VirtoCommerce.ManagementClient.Marketing.ViewModel.Interfaces;

namespace VirtoCommerce.ManagementClient.Marketing.ViewModel.Implementations
{
    public abstract class PromotionViewModelBase : ViewModelDetailAndWizardBase<Promotion>, IPromotionViewModelBase
    {
        #region Dependencies

        private readonly IRepositoryFactory<IMarketingRepository> _repositoryFactory;
        private readonly INavigationManager _navManager;

        private readonly IViewModelsFactory<ISearchCategoryViewModel> _searchCategoryVmFactory;
        private readonly IViewModelsFactory<ISearchItemViewModel> _searchItemVmFactory;
        private readonly IRepositoryFactory<IShippingRepository> _shippingRepositoryFactory;
        private readonly IRepositoryFactory<IAppConfigRepository> _appConfigRepositoryFactory;

        #endregion


#if DESIGN
          public PromotionViewModelBase()
        {
        _customerRepository = new MockMarketingRepository();
            CurrentItem = _customerRepository.Promotions.FirstOrDefault();
        
            InitializeForOpen();
        }
#endif

        #region Constructor

        protected PromotionViewModelBase(
            IRepositoryFactory<IAppConfigRepository> appConfigRepositoryFactory,
            IRepositoryFactory<IMarketingRepository> repositoryFactory,
            IMarketingEntityFactory entityFactory,
            INavigationManager navManager,
            Promotion item,
            bool isWizardMode,
            IViewModelsFactory<ISearchCategoryViewModel> searchCategoryVmFactory,
            IViewModelsFactory<ISearchItemViewModel> searchItemVmFactory,
            IRepositoryFactory<IShippingRepository> shippingRepositoryFactory)
            : base(entityFactory, item, isWizardMode)
        {
            _appConfigRepositoryFactory = appConfigRepositoryFactory;
            _searchCategoryVmFactory = searchCategoryVmFactory;
            _searchItemVmFactory = searchItemVmFactory;
            _shippingRepositoryFactory = shippingRepositoryFactory;
            _repositoryFactory = repositoryFactory;
            _navManager = navManager;
            OpenItemCommand = new DelegateCommand(() => _navManager.Navigate(NavigationData));
        }

        #endregion

        #region Public members

        private int _selectedTabIndex;

        public int SelectedTabIndex
        {
            get { return _selectedTabIndex; }
            protected set
            {
                _selectedTabIndex = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region ViewModelBase overrides

        public override string DisplayName
        {
            get
            {
                return OriginalItem == null ? ToString() : OriginalItem.Name;
            }
        }

        public override string IconSource
        {
            get
            {
                return PromotionIconSourceConverter.Current.Convert(InnerItem, null, "20", null).ToString();
            }
        }


        public override Brush ShellDetailItemMenuBrush
        {
            get
            {
                var result =
                    (SolidColorBrush)Application.Current.TryFindResource("MarketingDetailItemMenuBrush");

                return result ?? base.ShellDetailItemMenuBrush;
            }
        }

        private NavigationItem _navigationData;
        public override NavigationItem NavigationData
        {
            get
            {
                return _navigationData ??
                       (_navigationData = new NavigationItem(GetNavigationKey(OriginalItem.PromotionId),
                                                            NavigationNames.HomeName, NavigationNames.MenuName, this));
            }
        }

        #endregion

        #region ViewModelDetailAndWizardBase overrides

        public override string ExceptionContextIdentity { get { return string.Format("Promotion ({0})", DisplayName); } }

        protected override void GetRepository()
        {
            Repository = _repositoryFactory.GetRepositoryInstance();
        }

        protected override bool IsValidForSave()
        {
            return InnerItem.Validate();
        }

        protected override RefusedConfirmation CancelConfirm()
        {
            return new RefusedConfirmation
            {
                Content = string.Format("Save changes to promotion '{0}'?".Localize(), DisplayName),
                Title = "Action confirmation".Localize(null, LocalizationScope.DefaultCategory)
            };
        }

        protected override void LoadInnerItem()
        {
            var item = (Repository as IMarketingRepository).Promotions
                            .Where(x => x.PromotionId == OriginalItem.PromotionId)
                            .ExpandAll()
                            .Expand("Coupon")
                            .SingleOrDefault();
            OnUIThread(() => { InnerItem = item; });

        }

        // has to override it in each step and init only necessary property for current step
        // init all properties of the ViewModel
        protected override void InitializePropertiesForViewing()
        {
            if (!IsWizardMode)
            {
                InitializeExpressionElementBlock();
            }
        }

        protected override void BeforeSaveChanges()
        {
            // update InnerItem from Wizard step has to do manually
            if (!IsWizardMode)
            {
                foreach (var reward in InnerItem.Rewards)
                {
                    Repository.Remove(reward);
                }
                InnerItem.Rewards.Clear();
                UpdateFromExpressionElementBlock();
            }
        }

        protected override void AfterSaveChangesUI()
        {
            OriginalItem.InjectFrom<CloneInjection>(InnerItem);
        }

        protected override void DoDuplicate()
        {
            var repository = Repository as IMarketingRepository;

            var item = InnerItem.DeepClone(EntityFactory as IKnownSerializationTypes);
            item.PromotionId = item.GenerateNewKey();
            item.Name = item.Name + "_1";
            item.Status = PromotionStatusConverter.ValueInActive;
            item.Rewards.Clear();
            item.Coupon = null;
            item.CouponId = null;

            if (InnerItem.Coupon != null)
            {
                var coupon = InnerItem.Coupon.DeepClone(EntityFactory as IKnownSerializationTypes);
                coupon.CouponId = coupon.GenerateNewKey();
                item.CouponId = coupon.CouponId;
                repository.Add(coupon);
            }

            InnerItem.Rewards.ToList().ForEach(x =>
            {
                var newReward = x.DeepClone(EntityFactory as IKnownSerializationTypes);
                newReward.PromotionRewardId = newReward.GenerateNewKey();
                item.Rewards.Add(newReward);
            });

            repository.Add(item);
        }

        protected override void SetSubscriptionUI()
        {
            AttachExpressionHandlers(ExpressionElementBlock);
        }

        protected override void CloseSubscriptionUI()
        {
            DetachExpressionHandlers(ExpressionElementBlock);
        }

        protected override void OnInnerItemChangedUI()
        {
            SaveChangesCommand.RaiseCanExecuteChanged();
        }

        #endregion

        #region IPromotionViewModel Members

        public abstract string CatalogId { get; }

        public TypedExpressionElementBase ExpressionElementBlock { get; set; }

        public IViewModelsFactory<ISearchCategoryViewModel> SearchCategoryVmFactory { get { return _searchCategoryVmFactory; } }
        public IViewModelsFactory<ISearchItemViewModel> SearchItemVmFactory { get { return _searchItemVmFactory; } }
        public IRepositoryFactory<IShippingRepository> ShippingRepositoryFactory { get { return _shippingRepositoryFactory; } }
        public IRepositoryFactory<IAppConfigRepository> AppConfigRepositoryFactory { get { return _appConfigRepositoryFactory; } }

        #endregion

        #region Private members

        private void AttachExpressionHandlers(INotifyPropertyChanged expressionBlock)
        {
            if (expressionBlock != null)
            {
                expressionBlock.PropertyChanged += ViewModel_PropertyChanged;
                if (expressionBlock is CompositeElement)
                {
                    (expressionBlock as CompositeElement).Children.CollectionChanged += ViewModel_PropertyChanged;
                    (expressionBlock as CompositeElement).HeaderElements.CollectionChanged += ViewModel_PropertyChanged;
                    (expressionBlock as CompositeElement).Children.ToList().ForEach(AttachExpressionHandlers);
                    (expressionBlock as CompositeElement).HeaderElements.ToList().ForEach(AttachExpressionHandlers);
                }
            }
        }

        private void DetachExpressionHandlers(INotifyPropertyChanged expressionBlock)
        {
            if (expressionBlock != null)
            {
                expressionBlock.PropertyChanged += ViewModel_PropertyChanged;
                if (expressionBlock is CompositeElement)
                {
                    (expressionBlock as CompositeElement).Children.CollectionChanged -= ViewModel_PropertyChanged;
                    (expressionBlock as CompositeElement).HeaderElements.CollectionChanged -= ViewModel_PropertyChanged;
                    (expressionBlock as CompositeElement).Children.ToList().ForEach(DetachExpressionHandlers);
                    (expressionBlock as CompositeElement).HeaderElements.ToList().ForEach(DetachExpressionHandlers);
                }
            }
        }

        #endregion

        #region Initialize and Update

        protected virtual void InitializeExpressionElementBlock()
        {
            // Should be override in derived classes to setup ExpressionElementBlock if IsWizardMode == true
            OnUIThread(() =>
            {
                if (string.IsNullOrEmpty(InnerItem.PredicateVisualTreeSerialized))
                {
                    throw new NotImplementedException(); // PredicateVisualTreeSerialized should be not null 
                }
                ExpressionElementBlock = SerializationUtil.Deserialize<TypedExpressionElementBase>(InnerItem.PredicateVisualTreeSerialized);
                ExpressionElementBlock.InitializeAfterDeserialized(this);
                OnPropertyChanged("ExpressionElementBlock");
            });
        }

        /// <summary>
        /// Update InnerItem from ExpressionElementBlock
        /// </summary>
        public void UpdateFromExpressionElementBlock()
        {
            InnerItem.PredicateVisualTreeSerialized = SerializationUtil.Serialize(ExpressionElementBlock);
            foreach (var reward in ((PromotionExpressionBlock)ExpressionElementBlock).GetPromotionRewards())
            {
                InnerItem.Rewards.Add(reward);
            }
            InnerItem.PredicateSerialized =
                SerializationUtil.SerializeExpression(
                    ((PromotionExpressionBlock)ExpressionElementBlock).GetExpression());
        }

        #endregion

    }
}
