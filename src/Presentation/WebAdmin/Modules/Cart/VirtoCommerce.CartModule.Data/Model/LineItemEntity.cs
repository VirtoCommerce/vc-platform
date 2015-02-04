using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Foundation.Frameworks;

namespace VirtoCommerce.CartModule.Data.Model
{
	public class LineItemEntity : Entity
	{
		public string ProductId { get; set; }
		public string CatalogId { get; set; }
		public string CategoryId { get; set; }
	
		public string Name { get; set; }
		public int Quantity { get; set; }

		public string FulfilmentLocationCode { get; set; }
		public string ShipmentMethodCode { get; set; }
		public bool RequiredShipping { get; set; }
		public string ImageUrl { get; set; }

		public bool IsGift { get; set; }
		public string Currency { get; set; }

		public string LanguageCode { get; set; }

		public string Note { get; set; }

		public bool IsReccuring { get; set; }

		public bool TaxIncluded { get; set; }

		public string WeightUnit { get; set; }
		public decimal? WeightValue { get; set; }
		public decimal? VolumetricWeight { get; set; }

		public string DimensionUnit { get; set; }
		public decimal? DimensionHeight { get; set; }
		public decimal? DimensionLength { get; set; }
		public decimal? DimensionWidth { get; set; }
		
		public decimal ListPrice { get; set; }
		public decimal SalePrice { get; set; }
		public decimal PlacedPrice { get; set; }
		public decimal ExtendedPrice { get; private set; }

		public decimal DiscountTotal { get; private set; }
		public decimal TaxTotal { get; set; }

		public ShoppingCartEntity ShoppingCart { get; set; }
		public string ShoppingCartId { get; set; }

		public ShipmentEntity Shipment { get; set; }
		public string ShipmentId { get; set; }
	}
}
