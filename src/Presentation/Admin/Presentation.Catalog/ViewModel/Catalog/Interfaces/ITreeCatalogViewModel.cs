using VirtoCommerce.ManagementClient.Core.Infrastructure;
using catalogModel = VirtoCommerce.Foundation.Catalogs.Model;

namespace VirtoCommerce.ManagementClient.Catalog.ViewModel.Catalog.Interfaces
{
	public interface ITreeCatalogViewModel : IViewModel
	{
		catalogModel.Catalog InnerItem { get; }

		void RefreshUI();
	}
}
