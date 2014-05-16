using System;
using System.Collections.Generic;
using System.Linq;
using VirtoCommerce.Foundation.AppConfig.Repositories;
using VirtoCommerce.Foundation.Catalogs.Repositories;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.Foundation.Marketing.Factories;
using VirtoCommerce.Foundation.Marketing.Model;
using VirtoCommerce.Foundation.Marketing.Repositories;
using VirtoCommerce.Foundation.Orders.Repositories;
using VirtoCommerce.ManagementClient.Catalog.ViewModel.Catalog.Interfaces;
using VirtoCommerce.ManagementClient.Core.Infrastructure;
using VirtoCommerce.ManagementClient.Core.Infrastructure.Wizard;
using VirtoCommerce.ManagementClient.Marketing.Model;
using VirtoCommerce.ManagementClient.Marketing.ViewModel.Implementations;
using VirtoCommerce.ManagementClient.Marketing.ViewModel.Wizard.Interfaces;

namespace VirtoCommerce.ManagementClient.Marketing.ViewModel.Wizard.Implementations
{
	public class CreateCatalogPromotionViewModel : WizardContainerStepsViewModel, ICreateCatalogPromotionViewModel
	{
		public CreateCatalogPromotionViewModel(
			IViewModelsFactory<ICatalogPromotionOverviewStepViewModel> overviewVmFactory,
			IViewModelsFactory<ICatalogPromotionExpressionStepViewModel> expressionVmFactory,
			Promotion item)
		{
			var parametr = new KeyValuePair<string, object>("item", item);

			RegisterStep(overviewVmFactory.GetViewModelInstance(parametr));
			RegisterStep(expressionVmFactory.GetViewModelInstance(parametr));
		}

	}

	public class CatalogPromotionOverviewStepViewModel : CatalogPromotionViewModel, ICatalogPromotionOverviewStepViewModel
	{
		public CatalogPromotionOverviewStepViewModel(
			IRepositoryFactory<IMarketingRepository> repositoryFactory,
			IRepositoryFactory<ICatalogRepository> catalogRepositoryFactory,
			IRepositoryFactory<IPricelistRepository> pricelistRepositoryFactory,
			IMarketingEntityFactory entityFactory, Promotion item)
			: base(null, repositoryFactory, catalogRepositoryFactory, pricelistRepositoryFactory, null, null, null, entityFactory, item)
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

				if (InnerItem.Errors.Any() || InnerItem.StartDate == DateTime.MinValue || (InnerItem as CatalogPromotion).CatalogId == null)
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
			InitializeAvailableCatalogs();
		}

		#endregion
	}

	public class CatalogPromotionExpressionStepViewModel : CatalogPromotionViewModel, ICatalogPromotionExpressionStepViewModel, ISupportWizardPrepare
	{
		public CatalogPromotionExpressionStepViewModel(
			IRepositoryFactory<IAppConfigRepository> appConfigRepositoryFactory,
			IRepositoryFactory<IMarketingRepository> repositoryFactory,
			IViewModelsFactory<ISearchCategoryViewModel> searchCategoryVmFactory,
			IViewModelsFactory<ISearchItemViewModel> searchItemVmFactory,
			IRepositoryFactory<IShippingRepository> shippingRepositoryFactory,
			IMarketingEntityFactory entityFactory, Promotion item)
			: base(appConfigRepositoryFactory, repositoryFactory, null, null, searchCategoryVmFactory, searchItemVmFactory, shippingRepositoryFactory, entityFactory, item)
		{
		}

		#region IWizardStep Members

		public override bool IsValid
		{
			get
			{
				var retVal = (ExpressionElementBlock is CatalogPromotionExpressionBlock) &&
							  (ExpressionElementBlock as CatalogPromotionExpressionBlock).GetPromotionRewards().Any() &&
							  (ExpressionElementBlock as CatalogPromotionExpressionBlock).ConditionBlock.Children.Any();
				return retVal;
			}
		}

		public override bool IsLast { get { return true; } }

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
}
