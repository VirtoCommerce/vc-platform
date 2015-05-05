using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace VirtoCommerce.OrderModule.Web.Model
{
	public class Shipment : Operation
	{
		public string Organization { get; set; }
		public string OrganizationId { get; set; }
		public string FulfillmentCenter { get; set; }
		public string FulfillmentCenterId { get; set; }
		public string Employee { get; set; }
		public string EmployeeId { get; set; }
		public decimal DiscountAmount { get; set; }
		public ICollection<LineItem> Items { get; set; }
		public ICollection<PaymentIn> InPayments { get; set; }
		public Address DeliveryAddress { get; set; }
		public Discount Discount { get; set; }
	}
}