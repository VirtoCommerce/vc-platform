using System;
using System.Collections.Generic;
using System.Linq;
using VirtoCommerce.Domain.Catalog.Model;
using VirtoCommerce.Domain.Commerce.Model;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.CatalogModule.Web.Model
{
    /// <summary>
    /// Merchandising item.
    /// </summary>
    public class Product : AuditableEntity, ISeoSupport, IHasOutlines
    {
        /// <summary>
        /// Gets or sets the manufacturer part number for this product.
        /// </summary>
        /// <value>
        /// The manufacturer part number.
        /// </value>
		public string ManufacturerPartNumber { get; set; }
        /// <summary>
        /// Gets or sets the Global Trade Item Number.
        /// </summary>
        public string Gtin { get; set; }
        public string Code { get; set; }
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the catalog id that this product belongs to.
        /// </summary>
        /// <value>
        /// The catalog identifier.
        /// </value>
		public string CatalogId { get; set; }
        /// <summary>
        /// Gets or sets the catalog that this product belongs to.
        /// </summary>
        /// <value>
        /// The catalog.
        /// </value>
		public Catalog Catalog { get; set; }

        /// <summary>
        /// Gets or sets the category id that this product belongs to.
        /// </summary>
        /// <value>
        /// The category identifier.
        /// </value>
        public string CategoryId { get; set; }
        /// <summary>
        /// Gets or sets the category that this product belongs to.
        /// </summary>
        /// <value>
        /// The category.
        /// </value>
        public Category Category { get; set; }
        /// <summary>
        /// Gets or sets the all parent categories ids concatenated. E.g. (1;21;344)
        /// </summary>
        /// <value>
        /// The outline.
        /// </value>
		public string Outline { get; set; }
        /// <summary>
        /// All parents categories
        /// </summary>
        public ICollection<Category> Parents { get; set; }

        /// <summary>
        /// Gets or sets the date and time that this product was last indexed at.
        /// </summary>
        /// <value>
        /// The indexing date.
        /// </value>
		public DateTime? IndexingDate { get; set; }
        /// <summary>
        /// Gets or sets the titular item id for a variation.
        /// </summary>
        /// <value>
        /// The titular item identifier.
        /// </value>
        public string TitularItemId { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="Product"/> is buyable.
        /// </summary>
        public bool? IsBuyable { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="Product"/> is active.
        /// </summary>
        public bool? IsActive { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="Product"/> inventory is tracked.
        /// </summary>
        public bool? TrackInventory { get; set; }
        /// <summary>
        /// Gets or sets the maximum quantity of the product that a customer can buy.
        /// </summary>
        /// <value>
        /// The maximum quantity.
        /// </value>
		public int? MaxQuantity { get; set; }
        /// <summary>
        /// Gets or sets the minimum quantity of the product that a customer can buy.
        /// </summary>
        /// <value>
        /// The minimum quantity.
        /// </value>
		public int? MinQuantity { get; set; }

        /// <summary>
        /// Gets or sets the type of the product. (can be Physical, Digital or Subscription)
        /// </summary>
        /// <value>
        /// The type of the product.
        /// </value>
		public string ProductType { get; set; }
        /// <summary>
        /// Gets or sets the weight unit. (for physical product only)
        /// </summary>
        /// <value>
        /// The weight unit.
        /// </value>
		public string WeightUnit { get; set; }
        /// <summary>
        /// Gets or sets the weight. (for physical product only)
        /// </summary>
        /// <value>
        /// The weight.
        /// </value>
		public decimal? Weight { get; set; }

        /// <summary>
        /// Gets or sets the dimensions measure unit. (for physical product only)
        /// </summary>
        /// <value>
        /// The measure unit.
        /// </value>
		public string MeasureUnit { get; set; }
        /// <summary>
        /// Gets or sets the height. (for physical product only)
        /// </summary>
        /// <value>
        /// The height.
        /// </value>
		public decimal? Height { get; set; }
        /// <summary>
        /// Gets or sets the length. (for physical product only)
        /// </summary>
        /// <value>
        /// The length.
        /// </value>
		public decimal? Length { get; set; }
        /// <summary>
        /// Gets or sets the width. (for physical product only)
        /// </summary>
        /// <value>
        /// The width.
        /// </value>
		public decimal? Width { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="Product"/> can be reviewed in storefront.
        /// </summary>
		public bool? EnableReview { get; set; }

        /// <summary>
        /// Gets or sets the maximum number of download. (for digital product only)
        /// </summary>
        /// <value>
        /// The maximum number of download.
        /// </value>
		public int? MaxNumberOfDownload { get; set; }
        /// <summary>
        /// Gets or sets the download expiration. (for digital product only)
        /// </summary>
        /// <value>
        /// The download expiration.
        /// </value>
		public DateTime? DownloadExpiration { get; set; }
        /// <summary>
        /// Gets or sets the type of the download. (for digital product only)
        /// </summary>
        /// <value>
        /// The type of the download.
        /// </value>
		public string DownloadType { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="Product"/> has user agreement. (for digital product only)
        /// </summary>
		public bool? HasUserAgreement { get; set; }

        /// <summary>
        /// Gets or sets the type of the shipping.
        /// </summary>
        /// <value>
        /// The type of the shipping.
        /// </value>
		public string ShippingType { get; set; }
        /// <summary>
        /// Gets or sets the type of the tax.
        /// </summary>
        /// <value>
        /// The type of the tax.
        /// </value>
		public string TaxType { get; set; }

        /// <summary>
        /// Gets or sets the product vendor.
        /// </summary>
        /// <value>
        /// The vendor.
        /// </value>
		public string Vendor { get; set; }

        private string _imgSrc;
        /// <summary>
        /// Gets the default image for the product.
        /// </summary>
        /// <value>
        /// The image source URL.
        /// </value>
        public string ImgSrc
        {
            get
            {
                if (_imgSrc == null)
                {
                    if (Images != null && Images.Any())
                    {
                        _imgSrc = Images.First().Url;
                    }
                }
                return _imgSrc;
            }
        }

        /// <summary>
        /// Gets or sets the properties.
        /// </summary>
        /// <value>
        /// The properties.
        /// </value>
		public ICollection<Property> Properties { get; set; }
        /// <summary>
        /// Gets or sets the images.
        /// </summary>
        /// <value>
        /// The images.
        /// </value>
		public ICollection<Image> Images { get; set; }
        /// <summary>
        /// Gets or sets the assets.
        /// </summary>
        /// <value>
        /// The assets.
        /// </value>
		public ICollection<Asset> Assets { get; set; }
        /// <summary>
        /// Gets or sets the variations.
        /// </summary>
        /// <value>
        /// The variations.
        /// </value>
		public ICollection<Product> Variations { get; set; }
        /// <summary>
        /// Gets or sets the links.
        /// </summary>
        /// <value>
        /// The links.
        /// </value>
		public ICollection<CategoryLink> Links { get; set; }

        /// <summary>
        /// Gets or sets the reviews.
        /// </summary>
        /// <value>
        /// The reviews.
        /// </value>
        public ICollection<EditorialReview> Reviews { get; set; }
        /// <summary>
        /// Gets or sets the associations.
        /// </summary>
        /// <value>
        /// The associations.
        /// </value>
		public ICollection<ProductAssociation> Associations { get; set; }

        public string[] SecurityScopes { get; set; }

        #region ISeoSupport Members 
        public string SeoObjectType { get { return GetType().Name; } }
        /// <summary>
        /// Gets or sets the list of SEO information records.
        /// </summary>
        /// <value>
        /// The seo infos.
        /// </value>
        public ICollection<SeoInfo> SeoInfos { get; set; }
        #endregion

        #region Implementation of IHasOutlines

        public ICollection<Outline> Outlines { get; set; }

        #endregion
    }
}
