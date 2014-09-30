using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using VirtoCommerce.Foundation.AppConfig.Repositories;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.Foundation.Marketing.Factories;
using VirtoCommerce.Foundation.Marketing.Model;
using VirtoCommerce.Foundation.Marketing.Repositories;
using VirtoCommerce.Foundation.Orders.Repositories;
using VirtoCommerce.Foundation.Stores.Model;
using VirtoCommerce.Foundation.Stores.Repositories;
using VirtoCommerce.ManagementClient.Catalog.ViewModel.Catalog.Interfaces;
using VirtoCommerce.ManagementClient.Core.Controls;
using VirtoCommerce.ManagementClient.Core.Infrastructure;
using VirtoCommerce.ManagementClient.Core.Infrastructure.Navigation;
using VirtoCommerce.ManagementClient.Marketing.Model;
using VirtoCommerce.ManagementClient.Marketing.ViewModel.Interfaces;

namespace VirtoCommerce.ManagementClient.Marketing.ViewModel.Implementations
{
    public class CartPromotionViewModel : PromotionViewModelBase, ICartPromotionViewModel
    {
        #region Dependencies

        protected readonly IRepositoryFactory<IStoreRepository> _storeRepositoryFactory;

        private const int TabIndexOverview = 0;
        private const int TabIndexConditions = 1;
        private const int TabIndexCoupon = 2;
        #endregion

        #region ctor

        public CartPromotionViewModel(
            IRepositoryFactory<IAppConfigRepository> appConfigRepositoryFactory,
            IRepositoryFactory<IShippingRepository> shippingRepositoryFactory,
            IViewModelsFactory<ISearchCategoryViewModel> searchCategoryVmFactory,
            IViewModelsFactory<ISearchItemViewModel> searchItemVmFactory,
            IRepositoryFactory<IMarketingRepository> repositoryFactory,
            IRepositoryFactory<IStoreRepository> storeRepositoryFactory,
            IMarketingEntityFactory entityFactory,
            INavigationManager navManager,
            Promotion item)
            : base(appConfigRepositoryFactory, repositoryFactory, entityFactory, navManager, item, false, searchCategoryVmFactory, searchItemVmFactory, shippingRepositoryFactory)
        {
            _storeRepositoryFactory = storeRepositoryFactory;

            ViewTitle = new ViewTitleBase
            {
                Title = "Promotion",
                SubTitle = (item != null && !string.IsNullOrEmpty(item.Name)) ? item.Name.ToUpper(CultureInfo.InvariantCulture) : ""
            };
        }

        public bool IsWizard
        {
            get { return !IsSingleDialogEditing; }
        }

        protected CartPromotionViewModel(
            IRepositoryFactory<IAppConfigRepository> appConfigRepositoryFactory,
            IRepositoryFactory<IShippingRepository> shippingRepositoryFactory,
            IViewModelsFactory<ISearchCategoryViewModel> searchCategoryVmFactory,
            IViewModelsFactory<ISearchItemViewModel> searchItemVmFactory,
            IRepositoryFactory<IMarketingRepository> repositoryFactory,
            IRepositoryFactory<IStoreRepository> storeRepositoryFactory,
            IMarketingEntityFactory entityFactory,
            Promotion item)
            : base(appConfigRepositoryFactory, repositoryFactory, entityFactory, null, item, true, searchCategoryVmFactory, searchItemVmFactory, shippingRepositoryFactory)
        {
            _storeRepositoryFactory = storeRepositoryFactory;
        }

        #endregion

        #region ICartPromotionViewModel Members

        public bool NoCoupon
        {
            get { return !_hasCoupon; }
            set
            {
                _hasCoupon = !value;
                if (!_hasCoupon)
                {
                    CouponCodeDisplayed = null;
                }

                OnPropertyChanged();
                if (!IsInitializing)
                    OnViewModelPropertyChangedUI(null, null);
            }
        }

        private bool _hasCoupon;
        private string _couponCode;
        private string _couponCodeDisplayed;

        public bool HasCoupon
        {
            get { return _hasCoupon; }
            set
            {
                _hasCoupon = value;
                OnPropertyChanged();
                if (_hasCoupon)
                {
                    CouponCodeDisplayed = _couponCode;
                }
            }
        }

        public string CouponCodeDisplayed
        {
            get { return _couponCodeDisplayed; }
            set
            {
                _couponCodeDisplayed = value;
                OnPropertyChanged();
                if (HasCoupon)
                {
                    _couponCode = _couponCodeDisplayed;
                    if (!IsInitializing)
                        OnViewModelPropertyChangedUI(null, null);
                }
            }
        }

        public List<Store> AvailableStores { get; private set; }

        #endregion

        #region override PromotionViewModelBase Methods

        protected override bool IsValidForSave()
		{
            var result = base.IsValidForSave();
            var isExpressionValid = IsWizardMode || (ExpressionElementBlock is CartPromotionExpressionBlock &&
                                                    (ExpressionElementBlock as CartPromotionExpressionBlock).ConditionCutomerSegmentBlock.Children.Any() &&
                                                    (ExpressionElementBlock as CartPromotionExpressionBlock).GetPromotionRewards().Any());
            var isCouponValid = !HasCoupon || !string.IsNullOrEmpty(_couponCode);

            if (!result)
                SelectedTabIndex = TabIndexOverview;
            else if (!isExpressionValid)
                SelectedTabIndex = TabIndexConditions;
            else if (!isCouponValid)
                SelectedTabIndex = TabIndexCoupon;

            return result && isExpressionValid && isCouponValid;
		}

        protected override void InitializePropertiesForViewing()
        {
            base.InitializePropertiesForViewing();
            InitializeAvailableStores();
        }

        protected override void LoadInnerItem()
        {
            base.LoadInnerItem();

            OnUIThread(() =>
            {
                HasCoupon = InnerItem.Coupon != null;
                if (HasCoupon)
                {
                    CouponCodeDisplayed = InnerItem.Coupon.Code;
                }
            });
        }

        public override string CatalogId
        {
            get
            {
                return _storeRepositoryFactory.GetRepositoryInstance()
                                               .Stores.Where(store => store.StoreId == (InnerItem as CartPromotion).StoreId).First()
                                               .Catalog;
            }
        }

        protected override bool BeforeDelete()
        {
            base.BeforeDelete();
            if (InnerItem.Coupon != null)
            {
                Repository.Remove(InnerItem.Coupon);
            }
            return true;
        }

        /// <summary>
        /// add, update or delete associated item (coupon)
        /// </summary>
        protected override void BeforeSaveChanges()
        {
            if (!IsWizardMode)
            {
                base.BeforeSaveChanges();

                if (HasCoupon)
                {
                    if (InnerItem.Coupon == null)
                    {
                        InnerItem.Coupon = EntityFactory.CreateEntity<Coupon>();
                        InnerItem.Coupon.Code = _couponCode;
                        InnerItem.CouponId = InnerItem.Coupon.CouponId;
                        Repository.Add(InnerItem.Coupon);
                    }
                    else
                    {
                        InnerItem.Coupon.Code = _couponCode;
                        Repository.Update(InnerItem.Coupon);
                    }
                }
                else if (InnerItem.Coupon != null)
                {
                    Repository.Remove(InnerItem.Coupon);
                    InnerItem.CouponId = null;
                    InnerItem.Coupon = null;
                }
            }
            else
            {
                if (InnerItem.Coupon != null)
                {
                    Repository.Add(InnerItem.Coupon);
                }
            }
        }

        protected override void InitializeExpressionElementBlock()
        {
            if (IsWizardMode)
            {
                OnUIThread(() =>
                {
                    ExpressionElementBlock = new CartPromotionExpressionBlock(this);
                    InnerItem.PredicateVisualTreeSerialized = SerializationUtil.Serialize(ExpressionElementBlock);
                });
            }
            base.InitializeExpressionElementBlock();
        }

        #endregion

        #region Initialize and Update

        protected void InitializeAvailableStores()
        {
            if (AvailableStores == null)
            {
                using (var storeRepositoryFactory = _storeRepositoryFactory.GetRepositoryInstance())
                {
                    var items = storeRepositoryFactory.Stores.OrderBy(x => x.Name).ToList();
                    items.Insert(0, new Store() { StoreId = null, Name = "Select store...".Localize() });
                    OnUIThread(() =>
                    {
                        AvailableStores = items;
                        OnPropertyChanged("AvailableStores");
                    });
                }
            }

        }

        public void UpdateCoupon()
        {
            if (HasCoupon)
            {
                InnerItem.Coupon = EntityFactory.CreateEntity<Coupon>();
                InnerItem.Coupon.Code = _couponCode;
                InnerItem.CouponId = InnerItem.Coupon.CouponId;
            }
        }

        #endregion
    }
}
