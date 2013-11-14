using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Interactivity.InteractionRequest;
using VirtoCommerce.ManagementClient.Core.Infrastructure;
using VirtoCommerce.Foundation.Customers.Model;

namespace VirtoCommerce.ManagementClient.Customers.ViewModel.Settings.CasePropertySets.Interfaces
{
    public interface ICasePropertySetViewModel : IViewModel
    {
        CasePropertySet InnerItem { get; }

        InteractionRequest<Confirmation> CommonWizardDialogRequest { get; }

        DelegateCommand ItemAddCommand { get; }
        DelegateCommand<CaseProperty> ItemEditCommand { get; }
        DelegateCommand<CaseProperty> ItemDeleteCommand { get; }
    }
}
