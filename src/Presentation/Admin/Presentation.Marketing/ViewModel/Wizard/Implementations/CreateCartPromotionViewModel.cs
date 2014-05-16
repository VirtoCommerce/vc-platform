using System;
using System.Collections.Generic;
using System.Linq;
using VirtoCommerce.Foundation.AppConfig.Repositories;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.Foundation.Marketing.Factories;
using VirtoCommerce.Foundation.Marketing.Model;
using VirtoCommerce.Foundation.Marketing.Repositories;
using VirtoCommerce.Foundation.Orders.Repositories;
using VirtoCommerce.Foundation.Stores.Repositories;
using VirtoCommerce.ManagementClient.Catalog.ViewModel.Catalog.Interfaces;
using VirtoCommerce.ManagementClient.Core.Infrastructure;
using VirtoCommerce.ManagementClient.Core.Infrastructure.Wizard;
using VirtoCommerce.ManagementClient.Marketing.Model;
using VirtoCommerce.ManagementClient.Marketing.ViewModel.Implementations;
using VirtoCommerce.ManagementClient.Marketing.ViewModel.Wizard.Interfaces;

namespace VirtoCommerce.ManagementClient.Marketing.ViewModel.Wizard.Implementations
{
	public class CreateCartPromotionViewModel : WizardContainerStepsViewModel, ICreateCartPromotionViewModel
	{

		public CreateCartPromotionViewModel(
			IViewModelsFactory<ICartPromotionOverviewStepViewModel> overviewVmFactory,
			IViewModelsFactory<ICartPromotionExpressionStepViewModel> expressionVmFactory,
			IViewModelsFactory<ICartPromotionCouponStepViewModel> couponVmFactory,
			Promotion item)
		{
			RegisterStep(overviewVmFactory.GetViewModelInstance(
				new KeyValuePair<string, object>("item", item)));
			RegisterStep(expressionVmFactory.GetViewModelInstance(
				new KeyValuePair<string, object>("item", item)));
			RegisterStep(couponVmFactory.GetViewModelInstance(
				new KeyValuePair<string, object>("item", item)));
		}

	}

	public class CartPromotionOverviewStepViewModel : CartPromotionViewModel, ICartPromotionOverviewStepViewModel
	{
		public CartPromotionOverviewStepViewModel(
			IRepositoryFactory<IMarketingRepository> repositoryFactory,
			IRepositoryFactory<IStoreRepository> storeRepositoryFactory,
			IMarketingEntityFactory entityFactory,
			Promotion item)
			: base(null, null, null, null, repositoryFactory, storeRepositoryFactory, entityFactory, item)
		{
		}

		#region IWizardStep Members

		public override bool IsValid
		{
			get
			{
				var retval = true;
				const bool doNotifyChanges = false;

				InnerItem.Validate(doNotifyChanges);

				if (InnerItem.Errors.Any() || InnerItem.StartDate == DateTime.MinValue || (InnerItem as CartPromotion).StoreId == null)
				{
					retval = false;
					InnerItem.Errors.Clear();
				}

				return retval;
			}
		}

		public override string Description { get { return "Enter promotion general information.".Localize(); } }

		public override string Comment { get { return "Fields marked as required must have values for the promotion to be created.".Localize(); } }

		#endregion

		#region ViewModelDetailBase

		protected override void InitializePropertiesForViewing()
		{
			InitializeAvailableStores();
		}

		#endregion
	}

	public class CartPromotionExpressionStepViewModel : CartPromotionViewModel, ICartPromotionExpressionStepViewModel, ISupportWizardPrepare
	{
		public CartPromotionExpressionStepViewModel(
			IRepositoryFactory<IAppConfigRepository> appConfigRepositoryFactory,
			IRepositoryFactory<IShippingRepository> shippingRepositoryFactory,
			IViewModelsFactory<ISearchCategoryViewModel> searchCategoryVmFactory,
			IViewModelsFactory<ISearchItemViewModel> searchItemVmFactory,
			IRepositoryFactory<IMarketingRepository> repositoryFactory,
			IRepositoryFactory<IStoreRepository> storeRepositoryFactory,
			IMarketingEntityFactory entityFactory, Promotion item)
			: base(appConfigRepositoryFactory, shippingRepositoryFactory, searchCategoryVmFactory, searchItemVmFactory, repositoryFactory, storeRepositoryFactory, entityFactory, item)
		{
		}

		#region IWizardStep Members

		public override bool IsValid
		{
			get
			{
				var retVal = (ExpressionElementBlock is CartPromotionExpressionBlock) &&
							  (ExpressionElementBlock as CartPromotionExpressionBlock).ConditionCutomerSegmentBlock.Children.Any() &&
							  (ExpressionElementBlock as CartPromotionExpressionBlock).GetPromotionRewards().Any();
				return retVal;
			}
		}

		public override string Description { get { return "Enter promotion details.".Localize(); } }

		public override string Comment { get { return "Add promotion conditions.".Localize(); } }

		#endregion

		#region ViewModelDetailBase

		protected override void InitializePropertiesForViewing()
		{
			InitializeExpressionElementBlock();
		}

		#endregion

		#region ISupportWizardPrepare

		public void Prepare()
		{
			UpdateFromExpressionElementBlock();
		}

		#endregion
	}

	public class CartPromotionCouponStepViewModel : CartPromotionViewModel, ICartPromotionCouponStepViewModel, ISupportWizardPrepare
	{
		public CartPromotionCouponStepViewModel(
			IRepositoryFactory<IMarketingRepository> repositoryFactory,
			IMarketingEntityFactory entityFactory,
			Promotion item)
			: base(null, null, null, null, repositoryFactory, null, entityFactory, item)
		{
		}

		#region IWizardStep Members

		public override bool IsValid
		{
			get
			{
				return !HasCoupon || !string.IsNullOrEmpty(CouponCodeDisplayed);
			}
		}

		public override bool IsLast { get { return true; } }

		public override string Description { get { return "Enter promotion coupons.".Localize(); } }

		public override string Comment { get { return "Select if the promotion available with or without coupon.".Localize(); } }

		#endregion

		#region ViewModelDetailBase

		protected override void InitializePropertiesForViewing()
		{
			//empty means - stop initialize for current step
		}

		#endregion

		#region ISupportWizardPrepare

		public void Prepare()
		{
			UpdateCoupon();
		}

		#endregion

	}
}
