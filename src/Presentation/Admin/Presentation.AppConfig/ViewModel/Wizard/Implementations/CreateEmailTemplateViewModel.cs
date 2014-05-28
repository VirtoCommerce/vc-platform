using System.Collections.Generic;
using VirtoCommerce.Foundation.AppConfig.Repositories;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.ManagementClient.AppConfig.ViewModel.EmailTemplates.Implementations;
using VirtoCommerce.ManagementClient.AppConfig.ViewModel.EmailTemplates.Interfaces;
using VirtoCommerce.ManagementClient.AppConfig.ViewModel.Wizard.Interfaces;
using VirtoCommerce.ManagementClient.Core.Infrastructure;
using VirtoCommerce.ManagementClient.Core.Infrastructure.Wizard;
using VirtoCommerce.Foundation.AppConfig.Factories;
using VirtoCommerce.Foundation.AppConfig.Model;

namespace VirtoCommerce.ManagementClient.AppConfig.ViewModel.Wizard.Implementations
{
	public class CreateEmailTemplateViewModel : WizardContainerStepsViewModel, ICreateEmailTemplateViewModel, ISupportWizardSave
	{
		public CreateEmailTemplateViewModel(IViewModelsFactory<IEmailTemplateOverviewStepViewModel> overviewVmFactory, IViewModelsFactory<IEmailTemplateLanguagesStepViewModel> languagesVmFactory, EmailTemplate item)
        {
            var itemParameter = new KeyValuePair<string, object>("item", item);
            RegisterStep(overviewVmFactory.GetViewModelInstance(itemParameter));
            RegisterStep(languagesVmFactory.GetViewModelInstance(itemParameter));
        }

    }


    public class EmailTemplateOverviewStepViewModel : EmailTemplateEditViewModel, IEmailTemplateOverviewStepViewModel
    {

        public EmailTemplateOverviewStepViewModel(IRepositoryFactory<IAppConfigRepository> repositoryFactory, IAppConfigEntityFactory entityFactory, IViewModelsFactory<IEmailTemplateAddLanguageViewModel> vmFactory, EmailTemplate item)
            : base(repositoryFactory, entityFactory, vmFactory, item)
        {

        }
    }

    public class EmailTemplateLanguagesStepViewModel:EmailTemplateEditViewModel, IEmailTemplateLanguagesStepViewModel
    {

        public EmailTemplateLanguagesStepViewModel(IRepositoryFactory<IAppConfigRepository> repositoryFactory, IAppConfigEntityFactory entityFactory, IViewModelsFactory<IEmailTemplateAddLanguageViewModel> vmFactory, EmailTemplate item)
            : base(repositoryFactory, entityFactory, vmFactory, item)
        {

        }

        public override bool IsLast
        {
            get
            {
                return true;
            }
        }

    }


}
