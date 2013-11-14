using VirtoCommerce.Foundation.Catalogs.Model;
using VirtoCommerce.ManagementClient.Core.Infrastructure;

namespace VirtoCommerce.ManagementClient.Catalog.ViewModel.Catalog.Interfaces
{
	public interface ITreeVirtualCatalogViewModel : IViewModel
	{
		CatalogBase InnerItem { get; }

		void RefreshUI();
	}
}
