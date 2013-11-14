using System.Collections.Generic;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.Foundation.Security.Repositories;
using VirtoCommerce.ManagementClient.Core.Infrastructure;
using VirtoCommerce.ManagementClient.Core.Infrastructure.Wizard;
using VirtoCommerce.Foundation.Security.Factories;
using VirtoCommerce.Foundation.Security.Model;
using VirtoCommerce.ManagementClient.Security.ViewModel.Implementations;
using VirtoCommerce.ManagementClient.Security.ViewModel.Wizard.Interfaces;

namespace VirtoCommerce.ManagementClient.Security.ViewModel.Wizard.Implementations
{
	public class CreateRoleViewModel : WizardContainerStepsViewModel, ICreateRoleViewModel, ISupportWizardSave
	{
		public CreateRoleViewModel(IViewModelsFactory<IRoleOverviewStepViewModel> overviewVmFactory, Role item)
		{
			var itemParameter = new KeyValuePair<string, object>("item", item);
			RegisterStep(overviewVmFactory.GetViewModelInstance(itemParameter));
		}

    }

    public class RoleOverviewStepViewModel : RoleViewModel, IRoleOverviewStepViewModel
	{
        public RoleOverviewStepViewModel(IRepositoryFactory<ISecurityRepository> repositoryFactory, ISecurityEntityFactory entityFactory, Role item)
            : base(repositoryFactory, entityFactory, item)
		{
		}
	}
}
