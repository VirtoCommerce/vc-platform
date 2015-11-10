using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Storefront.Model.Common;

namespace VirtoCommerce.Storefront.Model
{
    public class Product : Entity
    {
        /// <summary>
        /// Manufacturer part number for this product
        /// </summary>
        public string ManufacturerPartNumber { get; set; }

        /// <summary>
        /// Global trade item number
        /// </summary>
        public string Gtin { get; set; }

        /// <summary>
        /// Product code
        /// </summary>
        public string Sku { get; set; }

        /// <summary>
        /// Name of this product
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Product catalog id
        /// </summary>
        public string CatalogId { get; set; }

        /// <summary>
        /// Category id of this product
        /// </summary>
        public string CategoryId { get; set; }

        /// <summary>
        /// All parent categories ids concatenated with ";". E.g. (1;21;344)
        /// </summary>
        public string Outline { get; set; }

        /// <summary>
        /// Date of last indexing of product, if null - product never was indexed
        /// </summary>
        public DateTime? IndexingDate { get; set; }

        /// <summary>
        /// Titular item id for a variation
        /// </summary>
        public string TitularItemId { get; set; }

        /// <summary>
        /// Indicating whether this product is buyable
        /// </summary>
        public bool? IsBuyable { get; set; }

        /// <summary>
        /// Indicating whether this product is active
        /// </summary>
        public bool? IsActive { get; set; }

        /// <summary>
        /// Indicating whether this product inventory is tracked
        /// </summary>
        public bool? TrackInventory { get; set; }

        /// <summary>
        /// Maximum quantity of the product that a customer can buy
        /// </summary>
        public int? MaxQuantity { get; set; }

        /// <summary>
        /// Minimum quantity of the product that a customer can buy
        /// </summary>
        public int? MinQuantity { get; set; }

        /// <summary>
        /// Type of product (can be Physical, Digital or Subscription)
        /// </summary>
        public string ProductType { get; set; }

        /// <summary>
        /// Weight unit (for physical product only)
        /// </summary>
        public string WeightUnit { get; set; }

        /// <summary>
        /// Weight of product (for physical product only)
        /// </summary>
        public double? Weight { get; set; }

        /// <summary>
        /// Dimensions measure unit of size (for physical product only)
        /// </summary>
        public string MeasureUnit { get; set; }

        /// <summary>
        /// Height of product size (for physical product only)
        /// </summary>
        public double? Height { get; set; }

        /// <summary>
        /// Length of product size (for physical product only)
        /// </summary>
        public double? Length { get; set; }

        /// <summary>
        /// Width of product size (for physical product only)
        /// </summary>
        public double? Width { get; set; }

        /// <summary>
        /// Indicating whether this product can be reviewed in storefront
        /// </summary>
        public bool? EnableReview { get; set; }

        /// <summary>
        /// Maximum number of downloads of product (for digital product only)
        /// </summary>
        public int? MaxNumberOfDownload { get; set; }

        /// <summary>
        /// Download expiration date (for digital product only)
        /// </summary>
        public DateTime? DownloadExpiration { get; set; }

        /// <summary>
        /// Type of the download (for digital product only)
        /// </summary>
        public string DownloadType { get; set; }

        /// <summary>
        /// Indicating whether this product has user agreement (for digital product only)
        /// </summary>
        public bool? HasUserAgreement { get; set; }

        /// <summary>
        /// Type of product shipping
        /// </summary>
        public string ShippingType { get; set; }

        /// <summary>
        /// Type of product tax
        /// </summary>
        public string TaxType { get; set; }

        /// <summary>
        /// Product's vendor
        /// </summary>
        public string Vendor { get; set; }

        /// <summary>
        /// Default image of product
        /// </summary>
        public string ImgSrc { get; set; }

        /// <summary>
        /// List of product properties
        /// </summary>
        public List<Property> Properties { get; set; }

        /// <summary>
        /// List of product images
        /// </summary>
        public List<Image> Images { get; set; }

        /// <summary>
        /// List of product assets
        /// </summary>
        public List<Asset> Assets { get; set; }

        /// <summary>
        /// List of product variations
        /// </summary>
        public List<Product> Variations { get; set; }

        /// <summary>
        /// List of product categories links
        /// </summary>
        public List<CategoryLink> Links { get; set; }

        /// <summary>
        /// List of product SEO information records
        /// </summary>
        public List<SeoInfo> SeoInfos { get; set; }

        /// <summary>
        /// List of product reviews
        /// </summary>
        public List<EditorialReview> Reviews { get; set; }

        /// <summary>
        /// List of product associations
        /// </summary>
        public List<ProductAssociation> Associations { get; set; }

        public Price Price { get; set; }

        public Inventory Inventory { get; set; }
    }
}
