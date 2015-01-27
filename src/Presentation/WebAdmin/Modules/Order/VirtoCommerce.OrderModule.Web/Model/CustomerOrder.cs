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
	public class CustomerOrder : OperationTreeNode
	{
		public CustomerOrder()
		{
			OperationType = "CustomerOrder";
		}
		public ICollection<Address> BillingAddresses { get; set; }
		public ICollection<Address> ShippingAddresses { get; set; }
		public ICollection<PaymentIn> InPayments { get; set; }
		public ICollection<CustomerOrderItem> Items { get; set; }
		public ICollection<Shipment> Shipments { get; set; }
		public Discount Discount { get; set; }
	}
}