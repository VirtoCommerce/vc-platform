using System.Collections.Generic;
using VirtoCommerce.Foundation.AppConfig.Factories;
using VirtoCommerce.Foundation.AppConfig.Repositories;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.ManagementClient.AppConfig.ViewModel.AppConfig.Implementations;
using VirtoCommerce.ManagementClient.AppConfig.ViewModel.Wizard.Interfaces;
using VirtoCommerce.ManagementClient.Core.Infrastructure;
using VirtoCommerce.ManagementClient.Core.Infrastructure.Wizard;
using VirtoCommerce.Foundation.AppConfig.Model;

namespace VirtoCommerce.ManagementClient.AppConfig.ViewModel.Wizard.Implementations
{
	public class CreateAppConfigSettingViewModel : WizardContainerStepsViewModel, ICreateAppConfigSettingViewModel
	{
		public CreateAppConfigSettingViewModel(IViewModelsFactory<IAppConfigSettingOverviewStepViewModel> vmFactory, Setting item)
		{
            RegisterStep(vmFactory.GetViewModelInstance(new KeyValuePair<string, object>("item", item)));
        }
    }

	public class AppConfigSettingOverviewStepViewModel : AppConfigSettingEditViewModel, IAppConfigSettingOverviewStepViewModel
	{
        public AppConfigSettingOverviewStepViewModel(IRepositoryFactory<IAppConfigRepository> repositoryFactory, IAppConfigEntityFactory entityFactory, Setting item)
            : base(repositoryFactory, entityFactory, item)
		{
		}
		
		public override bool IsValid
		{
			get
			{
				bool result = InnerItem != null && !string.IsNullOrEmpty(InnerItem.Name)
					&& !string.IsNullOrEmpty(InnerItem.SettingValueType);
				return result;
			}
		}

		public override string Description
		{
			get
			{
				return "Enter Setting details.".Localize();
			}
		}
	}
}
