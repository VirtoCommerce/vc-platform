using VirtoCommerce.Foundation.AppConfig.Model;
using VirtoCommerce.ManagementClient.Core.Infrastructure;

namespace VirtoCommerce.ManagementClient.AppConfig.ViewModel.EmailTemplates.Interfaces
{
    public interface IEmailTemplateAddLanguageViewModel : IViewModel
    {

        EmailTemplateLanguage InnerItem { get; }

    }
}
