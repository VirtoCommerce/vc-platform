using VirtoCommerce.ManagementClient.Core.Infrastructure;
using VirtoCommerce.Foundation.Catalogs.Model;

namespace VirtoCommerce.ManagementClient.Catalog.ViewModel.Catalog.Interfaces
{
    public interface IItemAssetViewModel : IViewModel
    {
        ItemAsset InnerItem { get; }

        bool Validate();
    }
}
