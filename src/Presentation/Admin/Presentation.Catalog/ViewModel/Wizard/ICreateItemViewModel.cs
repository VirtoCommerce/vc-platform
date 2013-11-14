using System.Collections.ObjectModel;
using VirtoCommerce.ManagementClient.Catalog.Model;
using VirtoCommerce.ManagementClient.Core.Infrastructure;
using VirtoCommerce.ManagementClient.Core.Infrastructure.Wizard;

namespace VirtoCommerce.ManagementClient.Catalog.ViewModel.Wizard
{
	public interface ICreateItemViewModel : IViewModel
	{
	}

	public interface IItemOverviewStepViewModel : IWizardStep
	{
	}

	public interface IItemPropertiesStepViewModel : IWizardStep
	{
		ObservableCollection<PropertyAndPropertyValueBase> PropertiesAndValues { get; }
	}

	public interface IItemPricingStepViewModel : IWizardStep
	{
		bool IsInitializingPricing { get; }
	}
}
