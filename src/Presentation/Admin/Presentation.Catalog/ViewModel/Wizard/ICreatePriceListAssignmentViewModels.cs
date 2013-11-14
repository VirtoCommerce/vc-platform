using VirtoCommerce.ManagementClient.Core.Infrastructure.Wizard;
using VirtoCommerce.ManagementClient.Core.Infrastructure;
using VirtoCommerce.ManagementClient.Catalog.Model;

namespace VirtoCommerce.ManagementClient.Catalog.ViewModel.Wizard
{
    public interface ICreatePriceListAssignmentViewModel : IViewModel
    {
    }

    public interface IPriceListAssignmentOverviewStepViewModel : IWizardStep
    {
    }

	public interface IPriceListAssignmentSetConditionsStepViewModel : IWizardStep
	{
	}

	public interface IPriceListAssignmentSetDatesStepViewModel : IWizardStep
	{
	}
}
