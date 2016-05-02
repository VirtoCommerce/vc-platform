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
	public class LineItemEntity : AuditableEntity
	{
		public LineItemEntity()
		{
			TaxDetails = new NullCollection<TaxDetailEntity>();
            Discounts = new NullCollection<DiscountEntity>();
        }

		[Required]
		[StringLength(3)]
		public string Currency { get; set; }
		[Required]
		[StringLength(64)]
		public string ProductId { get; set; }
        [Required]
        [StringLength(64)]
        public string Sku { get; set; }
        [Required]
		[StringLength(64)]
		public string CatalogId { get; set; }

		[StringLength(64)]
		public string CategoryId { get; set; }

        [StringLength(64)]
        public string ProductType { get; set; }
        [Required]
		[StringLength(256)]
		public string Name { get; set; }

		public int Quantity { get; set; }

		[StringLength(64)]
		public string FulfilmentLocationCode { get; set; }

		[StringLength(64)]
		public string ShipmentMethodCode { get; set; }
		public bool RequiredShipping { get; set; }
		[StringLength(1028)]
		public string ImageUrl { get; set; }

		public bool IsGift { get; set; }

		[StringLength(16)]
		public string LanguageCode { get; set; }

		[StringLength(2048)]
		public string Comment { get; set; }

        [StringLength(64)]
        public string ValidationType { get; set; }

        public bool IsReccuring { get; set; }

		public bool TaxIncluded { get; set; }

		public decimal? VolumetricWeight { get; set; }

		[StringLength(32)]
		public string WeightUnit { get; set; }
		public decimal? Weight { get; set; }
		[StringLength(32)]
		public string MeasureUnit { get; set; }
		public decimal? Height { get; set; }
		public decimal? Length { get; set; }
		public decimal? Width { get; set; }

        [StringLength(128)]
        public string PriceId { get; set; }

        [Column(TypeName = "Money")]
		public decimal ListPrice { get; set; }
		[Column(TypeName = "Money")]
		public decimal SalePrice { get; set; }
		[Column(TypeName = "Money")]
		public decimal PlacedPrice { get; set; }
		[Column(TypeName = "Money")]
		public decimal ExtendedPrice { get; set; }
		[Column(TypeName = "Money")]
		public decimal DiscountTotal { get; set; }
		[Column(TypeName = "Money")]
		public decimal TaxTotal { get; set; }
		[StringLength(64)]
		public string TaxType { get; set; }

		public virtual ShoppingCartEntity ShoppingCart { get; set; }
		public string ShoppingCartId { get; set; }

		public virtual ObservableCollection<TaxDetailEntity> TaxDetails { get; set; }

        public virtual ObservableCollection<DiscountEntity> Discounts { get; set; }
    }
}
