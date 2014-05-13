using System.Collections.Generic;
using VirtoCommerce.Foundation.Catalogs.Factories;
using VirtoCommerce.Foundation.Catalogs.Repositories;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.Foundation.Orders.Repositories;
using VirtoCommerce.ManagementClient.Catalog.ViewModel.Pricelists.Implementations;
using VirtoCommerce.ManagementClient.Core.Infrastructure;
using VirtoCommerce.ManagementClient.Core.Infrastructure.Wizard;
using VirtoCommerce.Foundation.Catalogs.Model;

namespace VirtoCommerce.ManagementClient.Catalog.ViewModel.Wizard
{
	public class CreatePriceListAssignmentViewModel : WizardContainerStepsViewModel, ICreatePriceListAssignmentViewModel
	{

        public CreatePriceListAssignmentViewModel(
			IViewModelsFactory<IPriceListAssignmentOverviewStepViewModel> overviewVmFactory, 
			IViewModelsFactory<IPriceListAssignmentSetConditionsStepViewModel> conditionsVmFactory,  
			IViewModelsFactory<IPriceListAssignmentSetDatesStepViewModel> datesVmFactory, 
			PricelistAssignment item)
		{
            RegisterStep(overviewVmFactory.GetViewModelInstance(
                new KeyValuePair<string, object>("item", item)));
            RegisterStep(conditionsVmFactory.GetViewModelInstance(
                new KeyValuePair<string, object>("item", item)));
            RegisterStep(datesVmFactory.GetViewModelInstance(
                new KeyValuePair<string, object>("item", item)));
        }
    }

	public class PriceListAssignmentOverviewStepViewModel : PriceListAssignmentViewModel, IPriceListAssignmentOverviewStepViewModel
	{
        public PriceListAssignmentOverviewStepViewModel(
			IRepositoryFactory<IPricelistRepository> repositoryFactory, 
			IRepositoryFactory<ICatalogRepository> catalogRepositoryFactory, 
			ICatalogEntityFactory entityFactory, 
			IAuthenticationContext authContext, 
			PricelistAssignment item)
            : base(null, repositoryFactory, catalogRepositoryFactory, entityFactory, authContext, item)
		{
		}

		#region IWizardStep Members

		public override bool IsValid
		{
			get
			{
				var retval = true;
					bool doNotifyChanges = false;
					InnerItem.Validate(doNotifyChanges);
					retval = InnerItem.Errors.Count == 0;
					InnerItem.Errors.Clear();
				return retval;
			}
		}

		public override string Description { get { return "Enter price list assignment general information".Localize(); } }

		#endregion

		#region ViewModelDetailBase

		// Do custom initialize for step instead initialize all properties for detail ViewModel
		protected override void InitializePropertiesForViewing()
		{
			InitializeAvailablePriceLists();
		}

		#endregion
	}

	public class PriceListAssignmentSetConditionsStepViewModel : PriceListAssignmentViewModel, IPriceListAssignmentSetConditionsStepViewModel, ISupportWizardPrepare
	{
		public PriceListAssignmentSetConditionsStepViewModel(
			IRepositoryFactory<ICountryRepository> countryRepositoryFactory, 
			IRepositoryFactory<IPricelistRepository> repositoryFactory, 
			IRepositoryFactory<ICatalogRepository> catalogRepositoryFactory, 
			ICatalogEntityFactory entityFactory, 
			IAuthenticationContext authContext, 
			PricelistAssignment item)
            : base(countryRepositoryFactory, repositoryFactory, catalogRepositoryFactory, entityFactory, authContext, item)
		{
		}

		#region IWizardStep Members

		public override bool IsValid { get { return true; } }

		public override string Description { get { return "Set price list assignment availability conditions".Localize(); } }

		#endregion

		#region ViewModelDetailBase

		// Do custom initialize for step instead initialize all properties for detail ViewModel
		protected override void InitializePropertiesForViewing()
		{
			InitializeExpressionElementBlock();
		}

		#endregion

		#region ISupportWizardPrepare

		//Update InnerItem from the step
		public void Prepare()
		{
			UpdateFromExpressionElementBlock();
		}
	
		#endregion
	}

	public class PriceListAssignmentSetDatesStepViewModel : PriceListAssignmentViewModel, IPriceListAssignmentSetDatesStepViewModel
	{
        public PriceListAssignmentSetDatesStepViewModel(
			IRepositoryFactory<IPricelistRepository> repositoryFactory, 
			IRepositoryFactory<ICatalogRepository> catalogRepositoryFactory, 
			ICatalogEntityFactory entityFactory, 
			IAuthenticationContext authContext, 
			PricelistAssignment item)
            : base(null, repositoryFactory, catalogRepositoryFactory, entityFactory, authContext, item)
		{
		}

		#region IWizardStep Members

		public override bool IsValid { get { return true; } }

		public override bool IsLast { get { return true; } }

		public override string Description { get { return "Set price list assignment availability dates".Localize(); } }

		#endregion

		#region ViewModelDetailBase

		// Do custom initialize for step instead initialize all properties for detail ViewModel
		protected override void InitializePropertiesForViewing()
		{
		}

		#endregion
	}
}
