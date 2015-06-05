using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Domain.Common;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.Domain.Catalog.Model
{
	public class CatalogProduct : AuditableEntity, ILinkSupport, ISeoSupport
	{
		/// <summary>
		/// SKU code
		/// </summary>
		public string Code { get; set; }
		public string ManufacturerPartNumber { get; set; }
		/// <summary>
		/// Global Trade Item Number (GTIN). These identifiers include UPC (in North America), EAN (in Europe), JAN (in Japan), and ISBN (for books).
		/// </summary>
		public string Gtin { get; set; }
		public string Name { get; set; }

		public string CatalogId { get; set; }
		public Catalog Catalog { get; set; }

		public string CategoryId { get; set; }
		public Category Category { get; set; }

		public string MainProductId { get; set; }
		public CatalogProduct MainProduct { get; set; }
        public bool? IsBuyable { get; set; }
        public bool? IsActive { get; set; }
        public bool? TrackInventory { get; set; }
		public DateTime? IndexingDate { get; set; }
		public int? MaxQuantity { get; set; }
		public int? MinQuantity { get; set; }

		/// <summary>
        /// Can be Shippable, Digital or Subscription.
		/// </summary>
		public string ProductType { get; set; }

		public string WeightUnit { get; set; }
		public decimal? Weight { get; set; }

		public string MeasureUnit { get; set; }
		public decimal? Height { get; set; }
		public decimal? Length { get; set; }
		public decimal? Width { get; set; }

		public bool? EnableReview { get; set; }

        /// <summary>
        /// re-downloads limit
        /// </summary>
		public int? MaxNumberOfDownload { get; set; }
		public DateTime? DownloadExpiration { get; set; }
        /// <summary>
        /// DownloadType: {Standard Product, Software, Music}
        /// </summary>
		public string DownloadType { get; set; }
		public bool? HasUserAgreement { get; set; }

		public string ShippingType { get; set; }
		public string TaxType { get; set; }

		public string Vendor { get; set; }
	    public DateTime StartDate { get; set; }
		public DateTime? EndDate { get; set; }

		public ICollection<PropertyValue> PropertyValues { get; set; }
		public ICollection<ItemAsset> Assets { get; set; }
		public ICollection<CategoryLink> Links { get; set; }
		public ICollection<CatalogProduct> Variations { get; set; }
		public ICollection<SeoInfo> SeoInfos { get; set; }
		public ICollection<EditorialReview> Reviews { get; set; }
		public ICollection<ProductAssociation> Associations { get; set; }
		public ICollection<Pricing.Model.Price> Prices { get; set; }
		public ICollection<Inventory.Model.InventoryInfo> Inventories { get; set; }

	}
}
