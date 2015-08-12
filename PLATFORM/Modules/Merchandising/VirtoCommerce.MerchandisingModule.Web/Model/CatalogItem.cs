using System;
using System.ComponentModel.DataAnnotations;

namespace VirtoCommerce.MerchandisingModule.Web.Model
{
    public class CatalogItem
    {
        /// <summary>
        /// Gets or sets the value of catalog item manufacturer part number
        /// </summary>
        public string ManufacturerPartNumber { get; set; }

        /// <summary>
        /// Gets or sets the value of catalog item Global Trade Item Number (GTIN).
        /// These identifiers include UPC (in North America), EAN (in Europe), JAN (in Japan), and ISBN (for books).
        /// </summary>
        public string Gtin { get; set; }

        /// <summary>
        /// Gets or sets the collection of product associations
        /// </summary>
        /// <value>
        /// Array of Association objects
        /// </value>
        public Association[] Associations { get; set; }

        /// <summary>
        /// Gets or sets the value of catalog id
        /// </summary>
        [Required]
        public string CatalogId { get; set; }

        /// <summary>
        /// Gets or sets the value of category id
        /// </summary>
        public string CategoryId { get; set; }

        /// <summary>
        /// Gets or sets the value of catalog item code
        /// </summary>
        [Required]
        public string Code { get; set; }

        /// <summary>
        /// Gets or sets the collection of catalog item editorial reviews
        /// </summary>
        /// <value>
        /// Array of EditorialReview objects
        /// </value>
        public EditorialReview[] EditorialReviews { get; set; }

        /// <summary>
        /// Gets or sets the value of catalog item id
        /// </summary>
        [Required]
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the value of catalog item primary image
        /// </summary>
        /// <value>
        /// Image object
        /// </value>
        public Image PrimaryImage { get; set; }

        /// <summary>
        /// Gets or sets the collection of catalog item images
        /// </summary>
        /// <value>
        /// Array of Image objects
        /// </value>
        public Image[] Images { get; set; }

        /// <summary>
        /// Gets or sets the collection of catalog item assets
        /// </summary>
        /// <value>
        /// Array of Asset objects
        /// </value>
        public Asset[] Assets { get; set; }
        
        /// <summary>
        /// Gets or sets the value of catalog item main product id
        /// </summary>
        public string MainProductId { get; set; }

        /// <summary>
        /// Gets or sets the ability to perform inventory tracking for catalog item
        /// </summary>
        public bool? TrackInventory { get; set; }

        /// <summary>
        /// Gets or sets the ability to buy catalog item
        /// </summary>
        public bool? IsBuyable { get; set; }

        /// <summary>
        /// Gets or sets the activity status of catalog item 
        /// </summary>
        public bool? IsActive { get; set; }

        /// <summary>
        /// Gets or sets the value of catalog item maximum inventory quantity
        /// </summary>
        public int? MaxQuantity { get; set; }

        /// <summary>
        /// Gets or sets the value of catalog item minimum inventory quantity
        /// </summary>
        public int? MinQuantity { get; set; }

        /// <summary>
        /// Gets or sets the value of catalog item name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the value of catalog item categories outline
        /// </summary>
        public string Outline { get; set; }

        /// <summary>
        /// Gets or sets the dictionary of catalog item properties
        /// </summary>
        public PropertyDictionary Properties { get; set; }

        /// <summary>
        /// Gets or sets the dictionary of catalog item variation properties
        /// </summary>
        public PropertyDictionary VariationProperties { get; set; }

        /// <summary>
        /// Gets or sets the value of catalog item rating
        /// </summary>
        public double Rating { get; set; }

        /// <summary>
        /// Gets or sets the value of catalog item total reviews quantity
        /// </summary>
        public int ReviewsTotal { get; set; }

        /// <summary>
        /// Gets or sets the collection of catalog item SEO parameters
        /// </summary>
        /// <value>
        /// Array of SeoKeyword objects
        /// </value>
        public SeoKeyword[] Seo { get; set; }

        /// <summary>
        /// Gets or sets the value of catalog item selling start date/time
        /// </summary>
        public DateTime StartDate { get; set; }

        /// <summary>
        /// Gets or sets the value of catalog item selling end date/time
        /// </summary>
        public DateTime? EndDate { get; set; }

        /// <summary>
        /// Gets or sets the value of catalog item type
        /// </summary>
        /// <value>
        /// "Physical", "Digital" or "Subscription"
        /// </value>
        public string ProductType { get; set; }

        /// <summary>
        /// Gets or sets the value of catalog item weight unit
        /// </summary>
        public string WeightUnit { get; set; }

        /// <summary>
        /// Gets or sets the value of catalog item weight
        /// </summary>
        public decimal? Weight { get; set; }

        /// <summary>
        /// Gets or sets the value of catalog item measurement unit
        /// </summary>
        public string MeasureUnit { get; set; }

        /// <summary>
        /// Gets or sets the value of catalog item height
        /// </summary>
        public decimal? Height { get; set; }

        /// <summary>
        /// Gets or sets the value of catalog item length
        /// </summary>
        public decimal? Length { get; set; }

        /// <summary>
        /// Gets or sets the value of catalog item width
        /// </summary>
        public decimal? Width { get; set; }

        /// <summary>
        /// Gets or sets the ability to add a review for catalog item
        /// </summary>
        public bool? EnableReview { get; set; }

        /// <summary>
        /// Gets or sets catalog item inventory policy
        /// </summary>
        /// <value>
        /// Inventory object
        /// </value>
        public Inventory Inventory { get; set; }

        /// <summary>
        /// Gets or sets the value of catalog item maximum download limit (for digital products)
        /// </summary>
        public int? MaxNumberOfDownload { get; set; }

        /// <summary>
        /// Gets or sets the value of catalog item download expiration date/time (for digital products)
        /// </summary>
        public DateTime? DownloadExpiration { get; set; }

        /// <summary>
        /// Gets or sets the value of catalog item download type
        /// </summary>
        /// <value>
        /// "Standard Product", "Software", "Music"
        /// </value>
        public string DownloadType { get; set; }

        /// <summary>
        /// Gets or sets the presence of catalog item end-user license agreement (for digital products)
        /// </summary>
        public bool? HasUserAgreement { get; set; }

        /// <summary>
        /// Gets or sets the value of catalog item shipping type
        /// </summary>
        public string ShippingType { get; set; }

        /// <summary>
        /// Gets or sets the value of catalog item tax type
        /// </summary>
        public string TaxType { get; set; }

        /// <summary>
        /// Gets or sets the value of catalog item vendor name
        /// </summary>
        public string Vendor { get; set; }
    }
}