using VirtoCommerce.Foundation.Orders.Model.ShippingMethod;
using VirtoCommerce.ManagementClient.Core.Infrastructure;

namespace VirtoCommerce.ManagementClient.Order.ViewModel.Settings.ShippingOptions.Interfaces
{
    public interface IShippingOptionAddShippingPackageViewModel: IViewModel
    {
        ShippingPackage InnerItem { get; }
    }
}
