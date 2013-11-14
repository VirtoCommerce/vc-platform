using System.Collections.ObjectModel;
using VirtoCommerce.ManagementClient.Catalog.Model;
using VirtoCommerce.ManagementClient.Core.Infrastructure;
using VirtoCommerce.ManagementClient.Core.Infrastructure.Wizard;

namespace VirtoCommerce.ManagementClient.Catalog.ViewModel.Wizard
{
	public interface ICreateCategoryViewModel : IViewModel
	{
	}

	public interface ICategoryOverviewStepViewModel : IWizardStep
	{
	}

	public interface ICategoryPropertiesStepViewModel : IWizardStep
	{
		ObservableCollection<PropertyAndPropertyValueBase> PropertiesAndValues { get; }
	}
}
