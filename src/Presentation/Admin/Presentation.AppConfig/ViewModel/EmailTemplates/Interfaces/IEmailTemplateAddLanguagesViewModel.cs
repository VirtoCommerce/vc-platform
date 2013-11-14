using VirtoCommerce.ManagementClient.Core.Infrastructure;
using VirtoCommerce.Foundation.AppConfig.Model;

namespace VirtoCommerce.ManagementClient.AppConfig.ViewModel.EmailTemplates.Interfaces
{
    public interface IEmailTemplateAddLanguagesViewModel:IViewModel
    {

        EmailTemplateLanguage InnerItem { get; }

    }
}
