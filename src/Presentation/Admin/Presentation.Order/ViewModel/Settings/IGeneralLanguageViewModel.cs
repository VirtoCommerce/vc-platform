using VirtoCommerce.ManagementClient.Core.Infrastructure;
using VirtoCommerce.ManagementClient.Order.Model.Settings;

namespace VirtoCommerce.ManagementClient.Order.ViewModel.Settings
{
    public interface IGeneralLanguageViewModel : IViewModel
    {
        GeneralLanguage InnerItem { get; }
    }
}
