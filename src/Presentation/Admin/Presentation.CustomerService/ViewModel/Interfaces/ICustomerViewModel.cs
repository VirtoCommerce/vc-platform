using VirtoCommerce.ManagementClient.Core.Infrastructure;
using VirtoCommerce.Foundation.Customers.Model;

namespace VirtoCommerce.ManagementClient.Customers.ViewModel.Interfaces
{
    public interface ICustomerViewModel : IViewModel
    {
        Contact InnerItem { get; }
    }
}
