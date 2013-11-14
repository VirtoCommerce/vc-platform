using VirtoCommerce.ManagementClient.Core.Infrastructure.Wizard;

namespace VirtoCommerce.Bootstrapper.Main.ViewModels
{
    public interface IModificationStepViewModel : IWizardStep
    {
        bool Repair { get; set; }

        bool Uninstall { get; set; }
    }
}