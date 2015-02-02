using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Foundation.Frameworks;

namespace VirtoCommerce.CartModule.Data.Model
{
	public class ShipmentEntity : Entity
	{
		public ShipmentEntity()
		{
			Items = new NullCollection<LineItemEntity>();
		}

		public string ShipmentMethodCode { get; set; }
		public string WarehouseLocation { get; set; }
		public AddressEntity DeliveryAddress { get; set; }
		public ICollection<LineItemEntity> Items { get; set; }
		public string Currency { get; set; }

		public string WeightUnit { get; set; }
		public decimal? WeightValue { get; set; }
		public decimal? VolumetricWeight { get; set; }

		public string DimensionUnit { get; set; }
		public decimal? DimensionHeight { get; set; }
		public decimal? DimensionLength { get; set; }
		public decimal? DimensionWidth { get; set; }

		public bool TaxIncluded { get; set; }

		public decimal ShippingPrice { get; set; }
		public decimal DiscountTotal { get; private set; }
		public decimal TaxTotal { get; set; }

		public ShoppingCartEntity ShoppingCart { get; set; }
		public string ShoppingCartId { get; set; }
		
	}
}
