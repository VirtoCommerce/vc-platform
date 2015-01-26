using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtoCommerce.Domain.Order.Model
{
	public class Shipment : CommingOutOperation
	{
		public string CustomerOrderId { get; set; }
		public CustomerOrder CustomerOrder { get; set; }

		public ICollection<Position> Items { get; set; }
		public Address DeliveryAddress { get; set; }
		public Discount Discount { get; set; }
	}
}
