using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Foundation.Money;

namespace VirtoCommerce.Domain.Order.Model
{
	public class CustomerOrder : Operation
	{
		public ICollection<PaymentIn> InPayments { get; set; }
		public ICollection<PaymentOut> OutPayments { get; set; }

		public ICollection<Position> Items { get; set; }
		public ICollection<Shipment> Shipments { get; set; }
		public Discount Discount { get; set; }
	}
}
