using VirtoCommerce.Foundation.Catalogs.Model;
using VirtoCommerce.ManagementClient.Core.Infrastructure;

namespace VirtoCommerce.ManagementClient.Catalog.ViewModel.Catalog.Interfaces
{
	public interface IPropertySetViewModel : IViewModel
	{
		bool Validate();
		ICollectionChange<PropertySetProperty> GetItemsCollection();
	}
}
