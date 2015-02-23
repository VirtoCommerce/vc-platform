using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.Foundation.Money;

namespace VirtoCommerce.OrderModule.Web.Model
{
	public class CustomerOrder : Operation
	{
		public string Customer { get; set; }
		public string CustomerId { get; set; }
		public string ChannelId { get; set; }
		public string StoreId { get; set; }
		public string Organization { get; set; }
		public string OrganizationId { get; set; }
		public string Employee { get; set; }
		public string EmployeeId { get; set; }
		public decimal DiscountAmount { get; set; }	

		public ICollection<Address> Addresses { get; set; }
		public ICollection<PaymentIn> InPayments { get; set; }
		public ICollection<LineItem> Items { get; set; }
		public ICollection<Shipment> Shipments { get; set; }
		public Discount Discount { get; set; }

	}
}