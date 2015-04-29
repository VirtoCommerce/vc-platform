using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.CartModule.Data.Model
{
	public class ShipmentEntity : AuditableEntity
	{
		public ShipmentEntity()
		{
			Items = new NullCollection<LineItemEntity>();
			Addresses = new NullCollection<AddressEntity>();
		}

		[StringLength(64)]
		public string ShipmentMethodCode { get; set; }
		[StringLength(64)]
		public string FulfilmentCenterId { get; set; }
		[Required]
		[StringLength(3)]
		public string Currency { get; set; }

		[StringLength(16)]
		public string WeightUnit { get; set; }
		public decimal? WeightValue { get; set; }
		public decimal? VolumetricWeight { get; set; }

		[StringLength(16)]
		public string DimensionUnit { get; set; }
		public decimal? DimensionHeight { get; set; }
		public decimal? DimensionLength { get; set; }
		public decimal? DimensionWidth { get; set; }

		public bool TaxIncluded { get; set; }
		[Column(TypeName = "Money")]
		public decimal ShippingPrice { get; set; }
		[Column(TypeName = "Money")]
		public decimal DiscountTotal { get; private set; }
		[Column(TypeName = "Money")]
		public decimal TaxTotal { get; set; }

		public ICollection<AddressEntity> Addresses { get; set; }
		public ICollection<LineItemEntity> Items { get; set; }

		public virtual ShoppingCartEntity ShoppingCart { get; set; }
		public string ShoppingCartId { get; set; }
		
	}
}
