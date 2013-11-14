using VirtoCommerce.Foundation.Catalogs.Model;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.Foundation.Orders.Repositories;
using VirtoCommerce.ManagementClient.Core.Infrastructure;

namespace VirtoCommerce.ManagementClient.Catalog.ViewModel.Pricelists.Interfaces
{
	public interface IPriceListAssignmentViewModel : IViewModelDetailBase, IExpressionViewModel
    {
		IRepositoryFactory<ICountryRepository> CountryRepositoryFactory { get; }
        PricelistAssignment InnerItem { get; }

        Pricelist[] AvailablePriceLists { get; }
        CatalogBase[] AvailableCatalogs { get; }

		void UpdateFromExpressionElementBlock();
    }
}
