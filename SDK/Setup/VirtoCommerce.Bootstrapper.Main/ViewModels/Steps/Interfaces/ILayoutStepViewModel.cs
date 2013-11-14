using Microsoft.Practices.Prism.Commands;

using VirtoCommerce.ManagementClient.Core.Infrastructure.Wizard;

namespace VirtoCommerce.Bootstrapper.Main.ViewModels
{
    public interface ILayoutStepViewModel : IWizardStep
    {
        string LayoutDirectory { get; set; }

        DelegateCommand<object> BrowseCommand { get; }
    }
}