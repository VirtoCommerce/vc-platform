using VirtoCommerce.ManagementClient.Core.Infrastructure;
using VirtoCommerce.Foundation.Orders.Model.ShippingMethod;

namespace VirtoCommerce.ManagementClient.Order.ViewModel.Settings.ShippingOptions.Interfaces
{
    public interface IShippingOptionViewModel : IViewModel
    {
        ShippingOption InnerItem { get; }
    }
}
