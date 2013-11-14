using System.Collections.ObjectModel;
using Microsoft.Practices.Prism.Commands;

namespace VirtoCommerce.ManagementClient.Core.Infrastructure.Wizard
{
    public interface IWizard : IViewModel
    {
        int TotalStepsCount { get; }

        int CurrentStepNumber { get; }

        IWizardStep CurrentStep { get; }

        ReadOnlyCollection<IWizardStep> AllRegisteredSteps { get; }

        DelegateCommand<object> MoveNextCommand { get; }

        DelegateCommand<object> MovePreviousCommand { get; }

	    void OnLoaded();
    }
}