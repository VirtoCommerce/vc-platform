using VirtoCommerce.ManagementClient.Catalog.Model;
using VirtoCommerce.ManagementClient.Core.Infrastructure;

namespace VirtoCommerce.ManagementClient.Catalog.ViewModel.Catalog.Interfaces
{
    public interface IPropertyValueBaseViewModel : IViewModel
    {
		PropertyAndPropertyValueBase InnerItem { get; }
        bool Validate();
    }
}
