using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Interactivity.InteractionRequest;

namespace VirtoCommerce.ManagementClient.Core.Infrastructure
{
    public interface IHomeSettingsEditableViewModel<T>
    {
        InteractionRequest<Confirmation> CommonConfirmRequest { get; }
        InteractionRequest<Confirmation> CommonWizardDialogRequest { get; }

        DelegateCommand ItemAddCommand { get; }
        DelegateCommand<T> ItemEditCommand { get; }
        DelegateCommand<T> ItemDeleteCommand { get; }
    }
}
