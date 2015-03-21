using System.Linq;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.Foundation.Orders.Model.PaymentMethod;
using VirtoCommerce.Foundation.Orders.Model.ShippingMethod;

namespace VirtoCommerce.Foundation.Orders.Repositories
{
	public interface IShippingRepository : IRepository
	{
		IQueryable<ShippingOption> ShippingOptions { get; }

		IQueryable<ShippingGateway> ShippingGateways { get; }

		IQueryable<ShippingMethod> ShippingMethods { get; }

		IQueryable<ShippingPackage> ShippingPackages { get; }

		IQueryable<ShippingMethodLanguage> ShippingMethodLanguages { get; }

		IQueryable<ShippingMethodJurisdictionGroup> ShippingMethodJurisdictionGroups { get; }
	}
}
