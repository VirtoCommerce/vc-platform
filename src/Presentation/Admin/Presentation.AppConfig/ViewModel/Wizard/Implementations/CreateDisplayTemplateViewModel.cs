using System.Collections.Generic;
using VirtoCommerce.Foundation.AppConfig.Factories;
using VirtoCommerce.Foundation.AppConfig.Repositories;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.Foundation.Stores.Repositories;
using VirtoCommerce.ManagementClient.AppConfig.ViewModel.DisplayTemplates.Implementations;
using VirtoCommerce.ManagementClient.AppConfig.ViewModel.Wizard.Interfaces;
using VirtoCommerce.ManagementClient.Catalog.ViewModel.Catalog.Interfaces;
using VirtoCommerce.ManagementClient.Core.Infrastructure;
using VirtoCommerce.ManagementClient.Core.Infrastructure.Wizard;
using VirtoCommerce.Foundation.AppConfig.Model;

namespace VirtoCommerce.ManagementClient.AppConfig.ViewModel.Wizard.Implementations
{
	public class CreateDisplayTemplateViewModel : WizardContainerStepsViewModel, ICreateDisplayTemplateViewModel
	{


		public CreateDisplayTemplateViewModel(IViewModelsFactory<IDisplayTemplateOverviewStepViewModel> overviewVmFactory, IViewModelsFactory<IDisplayTemplateConditionsStepViewModel> conditionsVmFactory, DisplayTemplateMapping item)
		{
            RegisterStep(overviewVmFactory.GetViewModelInstance(new KeyValuePair<string, object>("item", item)));
            RegisterStep(conditionsVmFactory.GetViewModelInstance(new KeyValuePair<string, object>("item", item)));
        }
    }

	public class DisplayTemplateOverviewStepViewModel : DisplayTemplateEditViewModel, IDisplayTemplateOverviewStepViewModel
	{
        public DisplayTemplateOverviewStepViewModel(
			IRepositoryFactory<IAppConfigRepository> repositoryFactory, 
			IAppConfigEntityFactory entityFactory, 
			DisplayTemplateMapping item)
            : base(null, repositoryFactory, entityFactory, null, null, item)
		{
		}

		#region IWizardStep Members

		public override bool IsValid
		{
			get
			{
				bool result = InnerItem != null && !string.IsNullOrEmpty(InnerItem.Name) && !string.IsNullOrEmpty(InnerItem.DisplayTemplate);
				return result;
			}
		}

		public override string Description
		{
			get
			{
				return "Enter Display template general information.".Localize();
			}
		}

		#endregion
		
	}

	public class DisplayTemplateConditionsStepViewModel : DisplayTemplateEditViewModel, IDisplayTemplateConditionsStepViewModel, ISupportWizardPrepare
	{
        public DisplayTemplateConditionsStepViewModel(
			IRepositoryFactory<IStoreRepository> storeRepositoryFactory,
			IRepositoryFactory<IAppConfigRepository> repositoryFactory, 
			IAppConfigEntityFactory entityFactory, 
			IViewModelsFactory<ISearchCategoryViewModel> categoryVmFactory, 
			IViewModelsFactory<ISearchItemViewModel> itemVmFactory, 
			DisplayTemplateMapping item)
			: base(storeRepositoryFactory, repositoryFactory, entityFactory, categoryVmFactory, itemVmFactory, item)
		{
		}

		#region IWizardStep Members
	
		public override bool IsValid
		{
			get
			{
				return true;
			}
		}

		public override string Description
		{
			get
			{
				return "Enter display template pick conditions.".Localize();
			}
		}

		#endregion

		#region ISupportWizardPrepare

		//Update InnerItem from the step
		public void Prepare()
		{
			UpdateFromExpressionElementBlock();
		}

		#endregion

	}
}
