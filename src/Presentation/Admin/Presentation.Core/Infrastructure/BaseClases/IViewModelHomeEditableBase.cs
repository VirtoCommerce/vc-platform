using System.Collections;
using System.ComponentModel;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Interactivity.InteractionRequest;

namespace VirtoCommerce.ManagementClient.Core.Infrastructure
{
    public interface IViewModelHomeEditableBase
    {
        DelegateCommand ItemAddCommand { get; }
        DelegateCommand<IList> ItemDeleteCommand { get; }
        
        InteractionRequest<Confirmation> CommonWizardDialogRequest { get; }
        InteractionRequest<Confirmation> CommonConfirmRequest { get; }
    }
}
