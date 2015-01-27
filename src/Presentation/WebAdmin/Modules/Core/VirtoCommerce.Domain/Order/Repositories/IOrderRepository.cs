using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Domain.Order.Model;
using VirtoCommerce.Foundation.Frameworks;

namespace VirtoCommerce.Domain.Order.Repositories
{
	public interface IOrderRepository : IRepository
	{
		IQueryable<CustomerOrder> CustomerOrders { get; }
		IQueryable<Shipment> Shipments { get; }
		IQueryable<PaymentIn> InPayments { get; }

		CustomerOrder GetCustomerOrderById(string id, ResponseGroup responseGroup);
		CustomerOrder GetShipmentById(string id, ResponseGroup responseGroup);
		CustomerOrder GetInPaymentById(string id, ResponseGroup responseGroup);
	}
}
