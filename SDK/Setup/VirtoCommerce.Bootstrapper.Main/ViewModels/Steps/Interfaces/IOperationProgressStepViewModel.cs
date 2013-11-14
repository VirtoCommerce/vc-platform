using VirtoCommerce.ManagementClient.Core.Infrastructure.Wizard;

namespace VirtoCommerce.Bootstrapper.Main.ViewModels
{
    public interface IOperationProgressStepViewModel : IWizardStep
    {
        string Message { get; set; }

        int Progress { get; }
    }
}