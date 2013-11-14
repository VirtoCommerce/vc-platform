using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Interactivity.InteractionRequest;

namespace VirtoCommerce.ManagementClient.Core.Infrastructure
{
    public interface ISupportAcceptChanges
    {
        InteractionRequest<Confirmation> CancelConfirmRequest { get; }
        DelegateCommand<object> CancelCommand { get; }
        DelegateCommand<object> SaveChangesCommand { get; }
        bool IsModified { get; }
    }
}
