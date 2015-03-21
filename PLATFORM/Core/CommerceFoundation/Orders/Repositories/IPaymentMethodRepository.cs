using System.Linq;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.Foundation.Orders.Model.PaymentMethod;

namespace VirtoCommerce.Foundation.Orders.Repositories
{
	public interface IPaymentMethodRepository : IRepository
	{
		IQueryable<PaymentMethod> PaymentMethods { get; }

		IQueryable<PaymentGateway> PaymentGateways { get; }

		IQueryable<PaymentMethodShippingMethod> PaymentMethodShippingMethods { get; }

		IQueryable<PaymentMethodLanguage> PaymentMethodLanguages { get; }

		IQueryable<PaymentMethodPropertyValue> PaymentPropertyValues { get; }
	}
}
