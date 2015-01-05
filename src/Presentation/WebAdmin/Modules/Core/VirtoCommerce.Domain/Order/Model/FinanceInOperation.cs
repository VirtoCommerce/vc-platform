using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtoCommerce.Domain.Order.Model
{
	public abstract class FinanceInOperation : FinanceOperation
	{
		public string CustomerOrderId { get; set; }
		public CustomerOrder CustomerOrder { get; set; }
		public string ShipmentId { get; set; }
		public Shipment Shipment { get; set; }
	}
}
