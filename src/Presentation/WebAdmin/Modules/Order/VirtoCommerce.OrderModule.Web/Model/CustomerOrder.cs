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
		
		public decimal  SubTotal 
		{
			get
			{
				decimal retVal = 0;
				if(Items != null)
				{
					retVal = Items.Sum(x=>x.Price * x.Quantity);
				}
				return retVal;
			}
		}

		public decimal TaxTotal 
		{
			get
			{
				decimal retVal = Tax;
				if(Items != null)
				{
					retVal = Items.Sum(x=>x.Tax);
				}
				if(Shipments != null)
				{
					retVal = Shipments.Sum(x=>x.Tax);
				}
				return retVal;
			}
		}
		
		public decimal  ShippingTotal 
		{
			get
			{
				decimal retVal = 0;
				if(Shipments != null)
				{
					retVal = Shipments.Sum(x=>x.Sum);
				}
				return retVal;
			}
		}
			
		public decimal  DiscountTotal 
		{
			get
			{
				decimal retVal = 0;
				retVal += Discount != null ? Discount.DiscountAmount ?? 0 : 0;
				if(Items != null)
				{
					retVal += Items.Where(x=>x.Discount != null).Sum(x=>x.Discount.DiscountAmount ?? 0);
				}
				if(Shipments != null)
				{
					retVal += Shipments.Where(x => x.Discount != null).Sum(x => x.Discount.DiscountAmount ?? 0);
				}
				return retVal;
			}
		}

		
		public ICollection<Address> Addresses { get; set; }
		public ICollection<PaymentIn> InPayments { get; set; }
		public ICollection<LineItem> Items { get; set; }
		public ICollection<Shipment> Shipments { get; set; }
		public Discount Discount { get; set; }

	}
}