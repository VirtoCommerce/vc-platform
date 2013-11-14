using Microsoft.Practices.Prism.Commands;

using VirtoCommerce.ManagementClient.Core.Infrastructure.Wizard;

namespace VirtoCommerce.Bootstrapper.Main.ViewModels
{
    public interface IInstallationStepViewModel : IWizardStep
    {
        string LicenseUri { get; }

        bool IsLicenseAccepted { get; set; }

        string InstallFolder { get; set; }

        DelegateCommand<object> BrowseCommand { get; }
    }
}