using System.Collections.Generic;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.ManagementClient.Core.Infrastructure;
using VirtoCommerce.ManagementClient.Core.Infrastructure.Wizard;
using VirtoCommerce.Foundation.Customers.Factories;
using VirtoCommerce.Foundation.Customers.Model;
using VirtoCommerce.Foundation.Customers.Repositories;
using VirtoCommerce.ManagementClient.Customers.ViewModel.Settings.CaseTemplates.Implementations;
using VirtoCommerce.ManagementClient.Customers.ViewModel.Settings.CaseTemplates.Interfaces;
using VirtoCommerce.ManagementClient.Customers.ViewModel.Settings.Wizard.Interfaces;

namespace VirtoCommerce.ManagementClient.Customers.ViewModel.Settings.Wizard.Implementations
{
	public class CreateCaseTemplateViewModel : WizardContainerStepsViewModel, ICreateCaseTemplateViewModel
    {
		
        public CreateCaseTemplateViewModel(IViewModelsFactory<ICaseTemplateOverviewStepViewModel> overviewVmFactory, CaseTemplate item)
        {
            var itemParameter = new KeyValuePair<string, object>("item", item);
            RegisterStep(overviewVmFactory.GetViewModelInstance(itemParameter));
        }
                
    }

    public class CaseTemplateOverviewStepViewModel : CaseTemplateViewModel, ICaseTemplateOverviewStepViewModel
    {
        public CaseTemplateOverviewStepViewModel(
			IViewModelsFactory<ICaseTemplatePropertyViewModel> templatePropertyVmFactory,
			IRepositoryFactory<ICustomerRepository> repositoryFactory, 
			ICustomerEntityFactory entityFactory, 
			CaseTemplate item)
            : base(templatePropertyVmFactory, repositoryFactory, entityFactory, item)
        {
        }

        #region WizardViewModelBase overrides
        
        public override bool IsValid
        {
            get { return true; }
        }
        
        #endregion
    }
}
