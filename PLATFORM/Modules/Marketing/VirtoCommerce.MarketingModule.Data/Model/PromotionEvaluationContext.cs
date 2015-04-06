using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Domain.Cart.Model;
using VirtoCommerce.Domain.Catalog.Model;
using VirtoCommerce.Domain.Marketing.Model;
using VirtoCommerce.Foundation.Money;

namespace VirtoCommerce.MarketingModule.Data
{
	public class PromotionEvaluationContext : IPromotionEvaluationContext
	{
		public PromotionEvaluationContext()
		{
			IsEveryone = true;
		}
		public string[] RefusedGiftIds { get; set; }

		public string StoreId { get; set; }

		public CurrencyCodes? Currency { get; set; }

		/// <summary>
		/// Customer id
		/// </summary>
		public string CustomerId { get; set; }

		public bool IsRegisteredUser { get; set; }

		/// <summary>
		/// Has user made any orders
		/// </summary>
		public bool IsFirstTimeBuyer { get; set; }

		public bool IsEveryone { get; set; }
		
		public decimal CartTotal { get; set; }

		/// <summary>
		/// Current shipment method
		/// </summary>
		public string ShipmentMethodCode { get; set; }
		public decimal ShipmentMethodPrice { get; set; }
		public string[] AvailableShipmentMethodCodes { get; set; }

		/// <summary>
		/// Entered coupon
		/// </summary>
		public string Coupon { get; set; }
		/// <summary>
		/// List of product promo information (should be populated in catalog browsing and cart detail places)
		/// </summary>
		public ProductPromoEntry[] ProductPromoEntries { get; set; }

		public Dictionary<string, string> Attributes { get; set; }

		
	}
	
}
