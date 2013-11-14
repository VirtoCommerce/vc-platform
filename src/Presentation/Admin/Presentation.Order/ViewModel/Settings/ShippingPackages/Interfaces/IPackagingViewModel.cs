using VirtoCommerce.ManagementClient.Core.Infrastructure;
using VirtoCommerce.Foundation.Catalogs.Model;

namespace VirtoCommerce.ManagementClient.Order.ViewModel.Settings.ShippingPackages.Interfaces
{
    public interface IPackagingViewModel:IViewModel
    {
        Packaging InnerItem { get; }

	    void UpdateDimensions();
    }
}
