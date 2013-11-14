using VirtoCommerce.ManagementClient.Core.Infrastructure;
using VirtoCommerce.ManagementClient.Core.Infrastructure.Wizard;
using VirtoCommerce.ManagementClient.Order.Infrastructure;
using VirtoCommerce.ManagementClient.Order.Model.Settings;

namespace VirtoCommerce.ManagementClient.Order.ViewModel.Settings.Wizard.Interfaces
{
    public interface IGeneralLanguagesStepViewModel : IWizardStep, ICollectionChange<GeneralLanguage>
    {
        bool IsLast_2 { set; }
    }
}