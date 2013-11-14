using VirtoCommerce.ManagementClient.Core.Infrastructure;
using VirtoCommerce.Foundation.Customers.Model;

namespace VirtoCommerce.ManagementClient.Customers.Dialogs.ViewModel.Interfaces
{
    public interface IEmailDialogViewModel:IViewModel
    {

        Email InnerItem { get; }

    }
}
