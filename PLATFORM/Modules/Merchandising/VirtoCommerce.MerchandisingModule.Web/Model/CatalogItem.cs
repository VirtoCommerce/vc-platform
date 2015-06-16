using System;
using System.ComponentModel.DataAnnotations;

namespace VirtoCommerce.MerchandisingModule.Web.Model
{
    public class CatalogItem
    {
        #region Public Properties
		public string ManufacturerPartNumber { get; set; }
		/// <summary>
		/// Global Trade Item Number (GTIN). These identifiers include UPC (in North America), EAN (in Europe), JAN (in Japan), and ISBN (for books).
		/// </summary>
		public string Gtin { get; set; }

        public Association[] Associations { get; set; }

        [Required]
        public string CatalogId { get; set; }

        public string[] Categories { get; set; }

        [Required]
        public string Code { get; set; }

        public EditorialReview[] EditorialReviews { get; set; }

        [Required]
        public string Id { get; set; }

        public ItemImage[] Images { get; set; }
        public string MainProductId { get; set; }

        public bool? TrackInventory { get; set; }

        public bool? IsBuyable { get; set; }

        public bool? IsActive { get; set; }

		public int? MaxQuantity { get; set; }
		public int? MinQuantity { get; set; }

        public string Name { get; set; }

        public string Outline { get; set; }

        public PropertyDictionary Properties { get; set; }

		public PropertyDictionary VariationProperties { get; set; }

        public double Rating { get; set; }
        public int ReviewsTotal { get; set; }

        public SeoKeyword[] Seo { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime? EndDate { get; set; }

		/// <summary>
		/// Can be Physical, Digital or Subscription.
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

        #endregion
    }
}
