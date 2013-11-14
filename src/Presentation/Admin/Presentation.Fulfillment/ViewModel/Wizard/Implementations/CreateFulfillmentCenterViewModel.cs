using System.Collections.Generic;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.Foundation.Orders.Repositories;
using VirtoCommerce.Foundation.Stores.Factories;
using VirtoCommerce.Foundation.Stores.Repositories;
using VirtoCommerce.ManagementClient.Core.Infrastructure;
using VirtoCommerce.ManagementClient.Core.Infrastructure.Wizard;
using VirtoCommerce.Foundation.Stores.Model;
using VirtoCommerce.ManagementClient.Fulfillment.ViewModel.FulfillmentCenters.Implementations;
using VirtoCommerce.ManagementClient.Fulfillment.ViewModel.Wizard.Interfaces;

namespace VirtoCommerce.ManagementClient.Fulfillment.ViewModel.Wizard.Implementations
{
	public class CreateFulfillmentCenterViewModel : WizardContainerStepsViewModel, ICreateFulfillmentCenterViewModel, ISupportWizardSave
    {
        public CreateFulfillmentCenterViewModel(IViewModelsFactory<IFulfillmentCenterOverviewStepViewModel> overviewVmFactory, IViewModelsFactory<IFulfillmentCenterAddressStepViewModel> addressVmFactory, FulfillmentCenter item)
        {

			RegisterStep(overviewVmFactory.GetViewModelInstance(new KeyValuePair<string, object>("item", item), new KeyValuePair<string, object>("isWizardMode", true)));
			RegisterStep(addressVmFactory.GetViewModelInstance(new KeyValuePair<string, object>("item", item), new KeyValuePair<string, object>("isWizardMode", true)));
        }

    }

    public class FulfillmentCenterOverviewStepViewModel : FulfillmentCenterViewModel, IFulfillmentCenterOverviewStepViewModel
    {
        public FulfillmentCenterOverviewStepViewModel(IRepositoryFactory<IFulfillmentCenterRepository> repositoryFactory, IRepositoryFactory<ICountryRepository> countryRepositoryFactory,
            IStoreEntityFactory entityFactory, FulfillmentCenter item)
            : base(repositoryFactory, countryRepositoryFactory,  entityFactory, item)
        {
        }
    }

    public class FulfillmentCenterAddressStepViewModel : FulfillmentCenterViewModel, IFulfillmentCenterAddressStepViewModel
    {
        public FulfillmentCenterAddressStepViewModel(IRepositoryFactory<IFulfillmentCenterRepository> repositoryFactory, IRepositoryFactory<ICountryRepository> countryRepositoryFactory,
            IStoreEntityFactory entityFactory, FulfillmentCenter item)
            : base(repositoryFactory, countryRepositoryFactory,  entityFactory, item)
        {
        }
    }
}
