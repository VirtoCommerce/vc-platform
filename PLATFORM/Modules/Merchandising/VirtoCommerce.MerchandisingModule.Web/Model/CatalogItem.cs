using System;
using System.ComponentModel.DataAnnotations;

namespace VirtoCommerce.MerchandisingModule.Web.Model
{
    public class CatalogItem
    {
        #region Public Properties
        /// <summary>
        /// Manufacturer part number
        /// </summary>
		public string ManufacturerPartNumber { get; set; }

		/// <summary>
		/// Global Trade Item Number (GTIN). These identifiers include UPC (in North America), EAN (in Europe), JAN (in Japan), and ISBN (for books).
		/// </summary>
		public string Gtin { get; set; }

        /// <summary>
        /// Collection of product associations
        /// </summary>
        public Association[] Associations { get; set; }

        /// <summary>
        /// Catalog id
        /// </summary>
        [Required]
        public string CatalogId { get; set; }

        /// <summary>
        /// Category id
        /// </summary>
        public string CategoryId { get; set; }

        /// <summary>
        /// Product code (used as SKU)
        /// </summary>
        [Required]
        public string Code { get; set; }

        /// <summary>
        /// Collection of editorial reviews
        /// </summary>
        public EditorialReview[] EditorialReviews { get; set; }

        /// <summary>
        /// Product id
        /// </summary>
        [Required]
        public string Id { get; set; }
		
        /// <summary>
        /// Product primary image
        /// </summary>
        public Image PrimaryImage { get; set; }

        /// <summary>
        /// Product image collection
        /// </summary>
        public Image[] Images { get; set; }
		
        /// <summary>
        /// Product assets collection
        /// </summary>
        public Asset[] Assets { get; set; }
        
        /// <summary>
        /// Main product id (if catalog item is a variation of product)
        /// </summary>
        public string MainProductId { get; set; }

        /// <summary>
        /// Enable tracking inventory for catalog item
        /// </summary>
        public bool? TrackInventory { get; set; }

        /// <summary>
        /// Catalog item can be purchased
        /// </summary>
        public bool? IsBuyable { get; set; }

        /// <summary>
        /// Catalog item is active
        /// </summary>
        public bool? IsActive { get; set; }

        /// <summary>
        /// Maximum inventory quantity
        /// </summary>
		public int? MaxQuantity { get; set; }

        /// <summary>
        /// Minimum inventory quantity
        /// </summary>
		public int? MinQuantity { get; set; }

        /// <summary>
        /// Catalog item name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Category outline
        /// </summary>
        public string Outline { get; set; }

        /// <summary>
        /// Catalog item properties collection with keys and values
        /// </summary>
        public PropertyDictionary Properties { get; set; }

        /// <summary>
        /// Variation properties collection with keys and values
        /// </summary>
		public PropertyDictionary VariationProperties { get; set; }

        /// <summary>
        /// Numeric catalog item rating
        /// </summary>
        public double Rating { get; set; }

        /// <summary>
        /// Total catalog item reviews count
        /// </summary>
        public int ReviewsTotal { get; set; }

        /// <summary>
        /// Collection of SEO parameters
        /// </summary>
        public SeoKeyword[] Seo { get; set; }

        /// <summary>
        /// Start selling date/time 
        /// </summary>
        public DateTime StartDate { get; set; }

        /// <summary>
        /// End selling date/time
        /// </summary>
        public DateTime? EndDate { get; set; }

		/// <summary>
		/// Product type (can be Physical, Digital or Subscription)
		/// </summary>
		public string ProductType { get; set; }

        /// <summary>
        /// Catalog item weight unit
        /// </summary>
		public string WeightUnit { get; set; }
		
        /// <summary>
        /// Catalog item weight
        /// </summary>
        public decimal? Weight { get; set; }

        /// <summary>
        /// Catalog item measurement unit
        /// </summary>
		public string MeasureUnit { get; set; }
		
        /// <summary>
        /// Catalog item height
        /// </summary>
        public decimal? Height { get; set; }
		
        /// <summary>
        /// Catalog item length
        /// </summary>
        public decimal? Length { get; set; }

        /// <summary>
        /// Catalog item width
        /// </summary>
		public decimal? Width { get; set; }

        /// <summary>
        /// Users can post reviews for catalog item
        /// </summary>
		public bool? EnableReview { get; set; }

        /// <summary>
        /// Catalog item inventory policy
        /// </summary>
        public Inventory Inventory { get; set; }

        /// <summary>
		/// Maximum download limit (for digital products)
		/// </summary>
		public int? MaxNumberOfDownload { get; set; }
		
        /// <summary>
        /// Download expiration date/time
        /// </summary>
        public DateTime? DownloadExpiration { get; set; }
		
        /// <summary>
		/// Download type: Standard Product, Software, Music
		/// </summary>
		public string DownloadType { get; set; }

        /// <summary>
        /// Product has end-user license agreement
        /// </summary>
		public bool? HasUserAgreement { get; set; }

        /// <summary>
        /// Shipping type
        /// </summary>
		public string ShippingType { get; set; }

        /// <summary>
        /// Tax type
        /// </summary>
		public string TaxType { get; set; }

        /// <summary>
        /// Vendor
        /// </summary>
		public string Vendor { get; set; }

        #endregion
    }
}
