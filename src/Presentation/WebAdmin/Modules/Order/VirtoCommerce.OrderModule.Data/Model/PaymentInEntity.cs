using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Domain.Order.Model;
using VirtoCommerce.Foundation.Frameworks;

namespace VirtoCommerce.OrderModule.Data.Model
{
	public class PaymentInEntity : OperationEntity, IFinanceInOperation
	{
		public PaymentInEntity()
		{
			Addresses = new NullCollection<AddressEntity>();
		}
		[StringLength(64)]
		public string OrganizationId { get; set; }
		[Required]
		[StringLength(64)]
		public string CustomerId { get; set; }

		public DateTime? IncomingDate { get; set; }
		[StringLength(128)]
		public string OuterId { get; set; }
		[StringLength(1024)]
		public string Purpose { get; set; }
		[StringLength(64)]
		public string GatewayCode { get; set; }
		public virtual ObservableCollection<AddressEntity> Addresses { get; set; }

		public string CustomerOrderId { get; set; }
		public virtual CustomerOrderEntity CustomerOrder { get; set; }

		public string ShipmentId { get; set; }
		public virtual ShipmentEntity Shipment { get; set; }
	}
}
