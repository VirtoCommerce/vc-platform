using System.Collections.Generic;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.Foundation.Orders.Factories;
using VirtoCommerce.Foundation.Orders.Repositories;
using VirtoCommerce.ManagementClient.Core.Infrastructure;
using VirtoCommerce.ManagementClient.Core.Infrastructure.Wizard;
using VirtoCommerce.Foundation.Orders.Model.Jurisdiction;
using VirtoCommerce.ManagementClient.Order.ViewModel.Settings.Jurisdictions.Implementations;
using VirtoCommerce.ManagementClient.Order.ViewModel.Settings.Wizard.Interfaces;

namespace VirtoCommerce.ManagementClient.Order.ViewModel.Settings.Wizard.Implementations
{
	public class CreateJurisdictionViewModel : WizardContainerStepsViewModel, ICreateJurisdictionViewModel, ISupportWizardSave
    {
        public CreateJurisdictionViewModel(IViewModelsFactory<IJurisdictionOverviewStepViewModel> vmFactory, JurisdictionTypes jurisdictionType, Jurisdiction item)
        {
            RegisterStep(vmFactory.GetViewModelInstance(
                new KeyValuePair<string, object>("item", item)
                , new KeyValuePair<string, object>("jurisdictionType", jurisdictionType)));
        }

    }

    public class JurisdictionOverviewStepViewModel : JurisdictionViewModel, IJurisdictionOverviewStepViewModel
    {
        public JurisdictionOverviewStepViewModel(IRepositoryFactory<IOrderRepository> repositoryFactory, IRepositoryFactory<ICountryRepository> countryRepositoryFactory,
            IOrderEntityFactory entityFactory, JurisdictionTypes jurisdictionType, Jurisdiction item)
            : base(repositoryFactory, countryRepositoryFactory,  entityFactory, jurisdictionType, item)
        {
        }
    }
}
