using VirtoCommerce.ManagementClient.Core.Infrastructure;
using VirtoCommerce.Foundation.Orders.Model.ShippingMethod;

namespace VirtoCommerce.ManagementClient.Order.ViewModel.Settings.ShippingPackages.Interfaces
{
    public interface IShippingPackageViewModel : IViewModel
    {
        ShippingPackage InnerItem { get; }
    }
}
