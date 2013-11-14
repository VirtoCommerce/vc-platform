using System.Collections.ObjectModel;
using VirtoCommerce.ManagementClient.Core.Infrastructure;
using VirtoCommerce.ManagementClient.Core.Infrastructure.Wizard;
using VirtoCommerce.ManagementClient.DynamicContent.Model;

namespace VirtoCommerce.ManagementClient.DynamicContent.ViewModel.Wizard.Interfaces
{
    public interface ICreateContentPublishingItemViewModel : IViewModel
    {
    }

	public interface IContentPublishingOverviewStepViewModel : IWizardStep
	{
	}

	public interface IContentPublishingDynamicContentStepViewModel : IWizardStep
	{
	}

	public interface IContentPublishingContentPlacesStepViewModel : IWizardStep
	{
	}

	public interface IContentPublishingConditionsStepViewModel : IWizardStep
	{
	}
}
