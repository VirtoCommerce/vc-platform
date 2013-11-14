using Microsoft.Practices.Prism.Commands;
using VirtoCommerce.ManagementClient.Core.Infrastructure.Wizard;

namespace VirtoCommerce.ConfigurationUtility.Main.ViewModels
{
    public interface IConfigurationWizardViewModel : IWizard
    {
        DelegateCommand<object> CancelCommand { get; }
    }
}