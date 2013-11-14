using VirtoCommerce.ManagementClient.Core.Infrastructure;
using VirtoCommerce.Foundation.Catalogs.Model;

namespace VirtoCommerce.ManagementClient.Catalog.ViewModel.Catalog.Interfaces
{
    public interface IPropertyValueViewModel : IViewModel
    {
        PropertyValue InnerItem { get; }

        bool Validate();
    }
}
