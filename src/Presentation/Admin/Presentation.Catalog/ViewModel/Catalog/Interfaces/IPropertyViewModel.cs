using VirtoCommerce.ManagementClient.Core.Infrastructure;
using VirtoCommerce.Foundation.Catalogs.Model;
using catalogModel = VirtoCommerce.Foundation.Catalogs.Model;

namespace VirtoCommerce.ManagementClient.Catalog.ViewModel.Catalog.Interfaces
{
    public interface IPropertyViewModel : IViewModel
    {
        Property InnerItem { get; }

        catalogModel.Catalog ParentCatalog { get; }

        bool Validate();
    }
}
