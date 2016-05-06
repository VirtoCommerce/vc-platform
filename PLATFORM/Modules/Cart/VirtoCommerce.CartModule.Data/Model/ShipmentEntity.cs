using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
            Items = new NullCollection<ShipmentItemEntity>();
            Discounts = new NullCollection<DiscountEntity>();
            Items = new NullCollection<ShipmentItemEntity>();
			Addresses = new NullCollection<AddressEntity>();
			TaxDetails = new NullCollection<TaxDetailEntity>();
		}

		[StringLength(64)]
		public string ShipmentMethodCode { get; set; }
        [StringLength(64)]
        public string ShipmentMethodOption { get; set; }

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
		public decimal DiscountTotal { get; set; }
		[Column(TypeName = "Money")]
		public decimal TaxTotal { get; set; }
        [Column(TypeName = "Money")]
        public decimal Total { get; set; }

        [StringLength(64)]
		public string TaxType { get; set; }

        public virtual ObservableCollection<ShipmentItemEntity> Items { get; set; }
        public virtual ObservableCollection<DiscountEntity> Discounts { get; set; }
        public virtual ObservableCollection<AddressEntity> Addresses { get; set; }
		public virtual ObservableCollection<TaxDetailEntity> TaxDetails { get; set; }
		public virtual ShoppingCartEntity ShoppingCart { get; set; }
		public string ShoppingCartId { get; set; }
		
	}
}
