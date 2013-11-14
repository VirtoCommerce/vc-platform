using System.Collections.Generic;
using VirtoCommerce.ManagementClient.Core.Infrastructure;
using VirtoCommerce.ManagementClient.Core.Infrastructure.Wizard;
using VirtoCommerce.ManagementClient.Fulfillment.ViewModel.Wizard.Interfaces;

namespace VirtoCommerce.ManagementClient.Fulfillment.ViewModel.Wizard.Implementations
{
	public class CreatePicklistWizardViewModel : WizardContainerStepsViewModel, ICreatePicklistWizardViewModel
	{
		public CreatePicklistWizardViewModel(IViewModelsFactory<CreatePicklistStepViewModel> createPicklistVmFactory, Foundation.Orders.Model.Fulfillment.Picklist item)
		{
			RegisterStep(createPicklistVmFactory.GetViewModelInstance(new KeyValuePair<string, object>("item", item)));
		}
	}
}
