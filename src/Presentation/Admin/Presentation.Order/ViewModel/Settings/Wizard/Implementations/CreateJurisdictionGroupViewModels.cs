using System.Collections.Generic;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.Foundation.Orders.Factories;
using VirtoCommerce.Foundation.Orders.Repositories;
using VirtoCommerce.ManagementClient.Core.Infrastructure;
using VirtoCommerce.ManagementClient.Core.Infrastructure.Wizard;
using VirtoCommerce.Foundation.Orders.Model.Jurisdiction;
using VirtoCommerce.ManagementClient.Order.ViewModel.Settings.JurisdictionGroups.Implementations;
using VirtoCommerce.ManagementClient.Order.ViewModel.Settings.Wizard.Interfaces;

namespace VirtoCommerce.ManagementClient.Order.ViewModel.Settings.Wizard.Implementations
{
	public class CreateJurisdictionGroupViewModel : WizardContainerStepsViewModel, ICreateJurisdictionGroupViewModel
    {
        public CreateJurisdictionGroupViewModel(IViewModelsFactory<IJurisdictionGroupOverviewStepViewModel> vmFactory, JurisdictionTypes jurisdictionType, JurisdictionGroup item)
        {
            RegisterStep(vmFactory.GetViewModelInstance(
                new KeyValuePair<string, object>("item", item)
                , new KeyValuePair<string, object>("jurisdictionType", jurisdictionType)));
        }
     }

	public class JurisdictionGroupOverviewStepViewModel : JurisdictionGroupViewModel, IJurisdictionGroupOverviewStepViewModel, ISupportWizardPrepare
    {
        public JurisdictionGroupOverviewStepViewModel(IRepositoryFactory<IOrderRepository> repositoryFactory, IOrderEntityFactory entityFactory, JurisdictionTypes jurisdictionType, JurisdictionGroup item)
            : base(repositoryFactory, entityFactory, jurisdictionType, item)
        {
        }

		#region ISupportWizardPrepare

		//Update InnerItem from the step
		public void Prepare()
		{
			UptadeOfPaymentShipping();
		}

		#endregion
	}
}
