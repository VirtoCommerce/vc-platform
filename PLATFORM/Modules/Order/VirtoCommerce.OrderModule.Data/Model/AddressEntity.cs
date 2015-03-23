using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Domain.Order.Model;
using VirtoCommerce.Foundation.Frameworks;

namespace VirtoCommerce.OrderModule.Data.Model
{
	public class AddressEntity : Entity
	{
		[StringLength(32)]
		public string AddressType { get; set; }
		[StringLength(64)]
		public string Organization { get; set; }
		[StringLength(3)]
		public string CountryCode { get; set; }
		[Required]
		[StringLength(64)]
		public string CountryName { get; set; }
		[Required]
		[StringLength(128)]
		public string City { get; set; }
		[StringLength(64)]
		public string PostalCode { get; set; }
		[StringLength(2048)]
		public string Line1 { get; set; }
		[StringLength(2048)]
		public string Line2 { get; set; }
		[Required]
		[StringLength(64)]
		public string FirstName { get; set; }
		[Required]
		[StringLength(64)]
		public string LastName { get; set; }
		[StringLength(64)]
		public string Phone { get; set; }
		[StringLength(64)]
		public string Email { get; set; }

		public virtual CustomerOrderEntity CustomerOrder { get; set; }
		public string CustomerOrderId { get; set; }

		public virtual ShipmentEntity Shipment { get; set; }
		public string ShipmentId { get; set; }

		public virtual PaymentInEntity PaymentIn { get; set; }
		public string PaymentInId { get; set; }
	}
}
