using VirtoCommerce.ManagementClient.Core.Infrastructure;
using VirtoCommerce.Foundation.Customers.Model;

namespace VirtoCommerce.ManagementClient.Customers.Dialogs.ViewModel.Interfaces
{
    public interface ICreateCustomerDialogViewModel : IViewModel
    {
        Contact InnerItem { get; }
        Email EmailForUserInput { get; }
        Phone PhoneForUserInput { get; }
    }
}
