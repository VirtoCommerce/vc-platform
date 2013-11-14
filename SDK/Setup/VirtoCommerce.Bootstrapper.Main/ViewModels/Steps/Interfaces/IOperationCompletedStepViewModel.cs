using VirtoCommerce.ManagementClient.Core.Infrastructure.Wizard;

using VirtoCommerce.Bootstrapper.Main.Infrastructure;

namespace VirtoCommerce.Bootstrapper.Main.ViewModels
{
    public interface IOperationCompletedStepViewModel : IWizardStep
    {
        string Message { get; set; }

        OperationResult Result { get; set; }

        int ExitCode { get; set; }
    }
}