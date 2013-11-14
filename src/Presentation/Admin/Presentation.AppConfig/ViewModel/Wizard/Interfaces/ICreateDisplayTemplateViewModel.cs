using VirtoCommerce.ManagementClient.Core.Infrastructure;
using VirtoCommerce.ManagementClient.Core.Infrastructure.Wizard;
using VirtoCommerce.Foundation.AppConfig.Model;

namespace VirtoCommerce.ManagementClient.AppConfig.ViewModel.Wizard.Interfaces
{
    public interface ICreateDisplayTemplateViewModel : IViewModel
    {
//		DisplayTemplateMapping InnerItem { get; }
//		TypedExpressionElementBase ExpressionElementBlock { get; set; }
	}

	public interface IDisplayTemplateOverviewStepViewModel : IWizardStep
	{
	}

	public interface IDisplayTemplateConditionsStepViewModel : IWizardStep
	{
	}

}
