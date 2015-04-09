using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Domain.Cart.Model;
using VirtoCommerce.Domain.Catalog.Model;
using VirtoCommerce.Domain.Marketing.Model;
using VirtoCommerce.Foundation.Money;

namespace VirtoCommerce.Domain.Marketing.Model
{
	public class PromotionEvaluationContext : IPromotionEvaluationContext
	{
		public PromotionEvaluationContext()
		{
			IsEveryone = true;
			CartPromoEntries = new List<ProductPromoEntry>();
			PromoEntries = new List<ProductPromoEntry>();
			Attributes = new Dictionary<string, string>();
		}


		public string[] RefusedGiftIds { get; set; }

		public string StoreId { get; set; }

		public string Currency { get; set; }

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
		/// List of product promo in cart
		/// </summary>
		public ICollection<ProductPromoEntry> CartPromoEntries { get; set; }
		/// <summary>
		/// List of products for promo evaluation
		/// </summary>
		public ICollection<ProductPromoEntry> PromoEntries { get; set; }
		/// <summary>
		/// Single catalog product promo entry 
		/// </summary>
		public ProductPromoEntry PromoEntry { get; set; }

		public Dictionary<string, string> Attributes { get; set; }

		
	}
	
}
