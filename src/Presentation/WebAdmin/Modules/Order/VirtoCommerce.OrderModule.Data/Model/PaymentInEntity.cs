using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Foundation.Frameworks;

namespace VirtoCommerce.OrderModule.Data.Model
{
	public class PaymentInEntity : OperationEntity
	{
		public DateTime? IncomingDate { get; set; }
		public string OuterId { get; set; }
		public string Purpose { get; set; }
		public string GatewayCode { get; set; }
		public AddressEntity BillingAddress { get; set; }

		public string CustomerOrderId { get; set; }
		public CustomerOrderEntity CustomerOrder { get; set; }

		public string ShipmentId { get; set; }
		public ShipmentEntity Shipment { get; set; }
	}
}
