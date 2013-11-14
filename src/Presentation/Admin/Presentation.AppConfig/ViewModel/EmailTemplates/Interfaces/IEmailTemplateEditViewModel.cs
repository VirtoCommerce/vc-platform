using System.Collections.Generic;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Interactivity.InteractionRequest;
using VirtoCommerce.ManagementClient.Core.Infrastructure;
using VirtoCommerce.Foundation.AppConfig.Model;

namespace VirtoCommerce.ManagementClient.AppConfig.ViewModel.EmailTemplates.Interfaces
{
    public interface IEmailTemplateEditViewModel:IViewModel
    {
        EmailTemplate InnerItem { get; }
		List<string> LanguagesCodes { get; }
		EmailTemplateLanguage SelectedEmailTemplateLanguage { get; }
		EmailTemplateTypes SelectedEmailTemplateType  { get; }

		DelegateCommand AddEmailTemplateLanguageCommand { get; }
	    DelegateCommand<EmailTemplateLanguage> EditEmailTemplateLanguageCommand { get; }
	    DelegateCommand<EmailTemplateLanguage> RemoveEmailTemplateLanguageCommand { get; }
	    InteractionRequest<ConditionalConfirmation> AddEmailTemplateLanguageRequest { get; }
	    InteractionRequest<ConditionalConfirmation> RemoveEmailTemplateLanguageRequest { get; }
    }
}
