using VirtoCommerce.ManagementClient.Core.Infrastructure;
using VirtoCommerce.Foundation.Orders.Model;

namespace VirtoCommerce.ManagementClient.Order.ViewModel.Interfaces
{
    public interface IOrderAddressViewModel : IViewModel
    {
        OrderAddress AddressItem { get; }
    }

}
