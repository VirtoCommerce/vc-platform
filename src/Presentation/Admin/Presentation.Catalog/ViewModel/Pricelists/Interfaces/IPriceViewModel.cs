using VirtoCommerce.ManagementClient.Core.Infrastructure;
using VirtoCommerce.Foundation.Catalogs.Model;

namespace VirtoCommerce.ManagementClient.Catalog.ViewModel.Pricelists.Interfaces
{
    public interface IPriceViewModel : IViewModel
    {
        Price InnerItem { get; }

        bool Validate();
    }
}
