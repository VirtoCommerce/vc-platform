using VirtoCommerce.ManagementClient.Core.Infrastructure;
using VirtoCommerce.ManagementClient.Core.Infrastructure.Wizard;

namespace VirtoCommerce.ManagementClient.AppConfig.ViewModel.Wizard.Interfaces
{
    public interface ICreateSystemJobViewModel : IViewModel
    {
    }

	public interface ISystemJobOverviewStepViewModel : IWizardStep
	{
	}

	public interface ISystemJobParametersStepViewModel : IWizardStep
	{
	}

	//public interface ISystemJobScheduleStepViewModel : IWizardStep
	//{
	//	//void InitilizeStep();
	//	SystemJob InnerItem { get; }
	//}
}
