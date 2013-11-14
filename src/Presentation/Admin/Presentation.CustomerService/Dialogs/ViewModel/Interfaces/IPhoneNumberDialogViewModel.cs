using VirtoCommerce.ManagementClient.Core.Infrastructure;
using VirtoCommerce.Foundation.Customers.Model;

namespace VirtoCommerce.ManagementClient.Customers.Dialogs.ViewModel.Interfaces
{
    public interface IPhoneNumberDialogViewModel : IViewModel
    {
        Phone InnerItem { get; }
    }
}
