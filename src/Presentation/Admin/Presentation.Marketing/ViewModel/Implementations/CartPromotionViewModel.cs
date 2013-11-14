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
		private bool _isCouponCreated = true;

		#region Dependencies

		protected readonly IRepositoryFactory<IStoreRepository> _storeRepositoryFactory;

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

		private bool _hasCoupon;
		public bool HasCoupon
		{
			get { return _hasCoupon; }
			set
			{
				_hasCoupon = value;
				if (value && InnerItem.Coupon != null)
				{
					if (InnerItem.CouponId != InnerItem.Coupon.CouponId)
					{
						InnerItem.CouponId = InnerItem.Coupon.CouponId;
					}
				}
				else
				{
					InnerItem.CouponId = null;
					if (InnerItem.Coupon != null)
						InnerItem.Coupon.Code = null;
				}
				OnPropertyChanged();
			}
		}
		
		public List<Store> AvailableStores { get; private set; }

		#endregion

		#region override PromotionViewModelBase Methods

		protected override bool IsValidForSave()
		{
			return base.IsValidForSave() && (!HasCoupon
								|| !string.IsNullOrEmpty(InnerItem.Coupon.Code)) && (((!IsWizardMode) && ((ExpressionElementBlock is CartPromotionExpressionBlock) &&
							  (ExpressionElementBlock as CartPromotionExpressionBlock).ConditionCutomerSegmentBlock.Children.Any() &&
							  (ExpressionElementBlock as CartPromotionExpressionBlock).GetPromotionRewards().Any())) || IsWizardMode);
		}

		protected override void InitializePropertiesForViewing()
		{
			base.InitializePropertiesForViewing();
			InitializeAvailableStores();
		}

		protected override void LoadInnerItem()
		{
			base.LoadInnerItem();
			_isCouponCreated = InnerItem.Coupon == null;
			OnUIThread(() =>
			{
				if (_isCouponCreated)
				{
					InnerItem.Coupon = EntityFactory.CreateEntity<Coupon>();
					OnPropertyChanged("InnerItem");
				}
				HasCoupon = !_isCouponCreated;
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
			if (InnerItem.Coupon != null && !_isCouponCreated)
			{
				Repository.Attach(InnerItem.Coupon);
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
				if (_isCouponCreated)
					if (string.IsNullOrEmpty(InnerItem.CouponId))
						InnerItem.Coupon = null;
					else
						Repository.Add(InnerItem.Coupon);
				else
					if (string.IsNullOrEmpty(InnerItem.CouponId))
						Repository.Remove(InnerItem.Coupon);
					else
						Repository.Update(InnerItem.Coupon);
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
					items.Insert(0, new Store() { StoreId = null, Name = "Select store..."});
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
			if (string.IsNullOrEmpty(InnerItem.CouponId))
				InnerItem.Coupon = null;
		}

		#endregion
	}
}
