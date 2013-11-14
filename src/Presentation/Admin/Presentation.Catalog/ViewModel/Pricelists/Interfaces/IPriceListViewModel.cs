using VirtoCommerce.Foundation.Catalogs.Model;
using VirtoCommerce.ManagementClient.Core.Infrastructure;

namespace VirtoCommerce.ManagementClient.Catalog.ViewModel.Pricelists.Interfaces
{
	public interface IPriceListViewModel : IViewModelDetailBase
	{
		Pricelist InnerItem { get; }
	}
}
