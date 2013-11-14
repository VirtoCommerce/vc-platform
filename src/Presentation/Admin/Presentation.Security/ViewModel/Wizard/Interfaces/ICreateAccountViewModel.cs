using VirtoCommerce.ManagementClient.Core.Infrastructure;
using VirtoCommerce.ManagementClient.Core.Infrastructure.Wizard;

namespace VirtoCommerce.ManagementClient.Security.ViewModel.Wizard.Interfaces
{
    public interface ICreateAccountViewModel : IViewModel
    {
        string Password { get; }
    }

    public interface IAccountOverviewStepViewModel : IWizardStep
    {
        string Password { get; set; }
    }

    public interface IAccountRolesStepViewModel : IWizardStep
	{
	}
}
