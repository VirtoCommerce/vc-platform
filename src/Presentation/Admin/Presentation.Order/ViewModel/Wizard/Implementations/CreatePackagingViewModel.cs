using System.Collections.Generic;
using VirtoCommerce.Foundation.Catalogs.Factories;
using VirtoCommerce.Foundation.Catalogs.Repositories;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.ManagementClient.Core.Infrastructure;
using VirtoCommerce.ManagementClient.Core.Infrastructure.Wizard;
using VirtoCommerce.Foundation.Catalogs.Model;
using VirtoCommerce.ManagementClient.Order.ViewModel.Settings.ShippingPackages.Implementations;
using VirtoCommerce.ManagementClient.Order.ViewModel.Wizard.Interfaces;

namespace VirtoCommerce.ManagementClient.Order.ViewModel.Wizard.Implementations
{
	public class CreatePackagingViewModel : WizardContainerStepsViewModel, ICreatePackagingViewModel
    {

        public CreatePackagingViewModel(IViewModelsFactory<IPackagingOverviewStepViewModel> overviewVmFactory, Packaging item)
        {
			var itemParameter = new KeyValuePair<string, object>("item", item);
			RegisterStep(overviewVmFactory.GetViewModelInstance(itemParameter));
		}
	}

	public class PackagingOverviewStepViewModel : PackagingViewModel, IPackagingOverviewStepViewModel, ISupportWizardPrepare
	{
        public PackagingOverviewStepViewModel(IRepositoryFactory<ICatalogRepository> repositoryFactory, ICatalogEntityFactory entityFactory, Packaging item)
            : base(repositoryFactory, entityFactory, item)
		{

		}

		#region ISupportWizardPrepare

		//Update InnerItem from the step
		public void Prepare()
		{
			UpdateDimensions();
		}
	
		#endregion
	}

}
