using System;
using System.Linq;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace VirtoCommerce.Client.Model
{
    /// <summary>
    /// Merchandising item.
    /// </summary>
    [DataContract]
    public partial class VirtoCommerceCatalogModuleWebModelProduct :  IEquatable<VirtoCommerceCatalogModuleWebModelProduct>
    {
        /// <summary>
        /// Gets or sets the manufacturer part number for this product.
        /// </summary>
        /// <value>Gets or sets the manufacturer part number for this product.</value>
        [DataMember(Name="manufacturerPartNumber", EmitDefaultValue=false)]
        public string ManufacturerPartNumber { get; set; }

        /// <summary>
        /// Gets or sets the Global Trade Item Number.
        /// </summary>
        /// <value>Gets or sets the Global Trade Item Number.</value>
        [DataMember(Name="gtin", EmitDefaultValue=false)]
        public string Gtin { get; set; }

        /// <summary>
        /// Gets or Sets Code
        /// </summary>
        [DataMember(Name="code", EmitDefaultValue=false)]
        public string Code { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>Gets or sets the name.</value>
        [DataMember(Name="name", EmitDefaultValue=false)]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the catalog id that this product belongs to.
        /// </summary>
        /// <value>Gets or sets the catalog id that this product belongs to.</value>
        [DataMember(Name="catalogId", EmitDefaultValue=false)]
        public string CatalogId { get; set; }

        /// <summary>
        /// Gets or sets the catalog that this product belongs to.
        /// </summary>
        /// <value>Gets or sets the catalog that this product belongs to.</value>
        [DataMember(Name="catalog", EmitDefaultValue=false)]
        public VirtoCommerceCatalogModuleWebModelCatalog Catalog { get; set; }

        /// <summary>
        /// Gets or sets the category id that this product belongs to.
        /// </summary>
        /// <value>Gets or sets the category id that this product belongs to.</value>
        [DataMember(Name="categoryId", EmitDefaultValue=false)]
        public string CategoryId { get; set; }

        /// <summary>
        /// Gets or sets the category that this product belongs to.
        /// </summary>
        /// <value>Gets or sets the category that this product belongs to.</value>
        [DataMember(Name="category", EmitDefaultValue=false)]
        public VirtoCommerceCatalogModuleWebModelCategory Category { get; set; }

        /// <summary>
        /// Gets or sets the all parent categories ids concatenated. E.g. (1;21;344)
        /// </summary>
        /// <value>Gets or sets the all parent categories ids concatenated. E.g. (1;21;344)</value>
        [DataMember(Name="outline", EmitDefaultValue=false)]
        public string Outline { get; set; }

        /// <summary>
        /// All parents categories
        /// </summary>
        /// <value>All parents categories</value>
        [DataMember(Name="parents", EmitDefaultValue=false)]
        public List<VirtoCommerceCatalogModuleWebModelCategory> Parents { get; set; }

        /// <summary>
        /// Gets or sets the date and time that this product was last indexed at.
        /// </summary>
        /// <value>Gets or sets the date and time that this product was last indexed at.</value>
        [DataMember(Name="indexingDate", EmitDefaultValue=false)]
        public DateTime? IndexingDate { get; set; }

        /// <summary>
        /// Gets or sets the titular item id for a variation.
        /// </summary>
        /// <value>Gets or sets the titular item id for a variation.</value>
        [DataMember(Name="titularItemId", EmitDefaultValue=false)]
        public string TitularItemId { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this {VirtoCommerce.CatalogModule.Web.Model.Product} is buyable.
        /// </summary>
        /// <value>Gets or sets a value indicating whether this {VirtoCommerce.CatalogModule.Web.Model.Product} is buyable.</value>
        [DataMember(Name="isBuyable", EmitDefaultValue=false)]
        public bool? IsBuyable { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this {VirtoCommerce.CatalogModule.Web.Model.Product} is active.
        /// </summary>
        /// <value>Gets or sets a value indicating whether this {VirtoCommerce.CatalogModule.Web.Model.Product} is active.</value>
        [DataMember(Name="isActive", EmitDefaultValue=false)]
        public bool? IsActive { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this {VirtoCommerce.CatalogModule.Web.Model.Product} inventory is tracked.
        /// </summary>
        /// <value>Gets or sets a value indicating whether this {VirtoCommerce.CatalogModule.Web.Model.Product} inventory is tracked.</value>
        [DataMember(Name="trackInventory", EmitDefaultValue=false)]
        public bool? TrackInventory { get; set; }

        /// <summary>
        /// Gets or sets the maximum quantity of the product that a customer can buy.
        /// </summary>
        /// <value>Gets or sets the maximum quantity of the product that a customer can buy.</value>
        [DataMember(Name="maxQuantity", EmitDefaultValue=false)]
        public int? MaxQuantity { get; set; }

        /// <summary>
        /// Gets or sets the minimum quantity of the product that a customer can buy.
        /// </summary>
        /// <value>Gets or sets the minimum quantity of the product that a customer can buy.</value>
        [DataMember(Name="minQuantity", EmitDefaultValue=false)]
        public int? MinQuantity { get; set; }

        /// <summary>
        /// Gets or sets the type of the product. (can be Physical, Digital or Subscription)
        /// </summary>
        /// <value>Gets or sets the type of the product. (can be Physical, Digital or Subscription)</value>
        [DataMember(Name="productType", EmitDefaultValue=false)]
        public string ProductType { get; set; }

        /// <summary>
        /// Gets or sets the weight unit. (for physical product only)
        /// </summary>
        /// <value>Gets or sets the weight unit. (for physical product only)</value>
        [DataMember(Name="weightUnit", EmitDefaultValue=false)]
        public string WeightUnit { get; set; }

        /// <summary>
        /// Gets or sets the weight. (for physical product only)
        /// </summary>
        /// <value>Gets or sets the weight. (for physical product only)</value>
        [DataMember(Name="weight", EmitDefaultValue=false)]
        public double? Weight { get; set; }

        /// <summary>
        /// Gets or sets the dimensions measure unit. (for physical product only)
        /// </summary>
        /// <value>Gets or sets the dimensions measure unit. (for physical product only)</value>
        [DataMember(Name="measureUnit", EmitDefaultValue=false)]
        public string MeasureUnit { get; set; }

        /// <summary>
        /// Gets or sets the height. (for physical product only)
        /// </summary>
        /// <value>Gets or sets the height. (for physical product only)</value>
        [DataMember(Name="height", EmitDefaultValue=false)]
        public double? Height { get; set; }

        /// <summary>
        /// Gets or sets the length. (for physical product only)
        /// </summary>
        /// <value>Gets or sets the length. (for physical product only)</value>
        [DataMember(Name="length", EmitDefaultValue=false)]
        public double? Length { get; set; }

        /// <summary>
        /// Gets or sets the width. (for physical product only)
        /// </summary>
        /// <value>Gets or sets the width. (for physical product only)</value>
        [DataMember(Name="width", EmitDefaultValue=false)]
        public double? Width { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this {VirtoCommerce.CatalogModule.Web.Model.Product} can be reviewed in storefront.
        /// </summary>
        /// <value>Gets or sets a value indicating whether this {VirtoCommerce.CatalogModule.Web.Model.Product} can be reviewed in storefront.</value>
        [DataMember(Name="enableReview", EmitDefaultValue=false)]
        public bool? EnableReview { get; set; }

        /// <summary>
        /// Gets or sets the maximum number of download. (for digital product only)
        /// </summary>
        /// <value>Gets or sets the maximum number of download. (for digital product only)</value>
        [DataMember(Name="maxNumberOfDownload", EmitDefaultValue=false)]
        public int? MaxNumberOfDownload { get; set; }

        /// <summary>
        /// Gets or sets the download expiration. (for digital product only)
        /// </summary>
        /// <value>Gets or sets the download expiration. (for digital product only)</value>
        [DataMember(Name="downloadExpiration", EmitDefaultValue=false)]
        public DateTime? DownloadExpiration { get; set; }

        /// <summary>
        /// Gets or sets the type of the download. (for digital product only)
        /// </summary>
        /// <value>Gets or sets the type of the download. (for digital product only)</value>
        [DataMember(Name="downloadType", EmitDefaultValue=false)]
        public string DownloadType { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this {VirtoCommerce.CatalogModule.Web.Model.Product} has user agreement. (for digital product only)
        /// </summary>
        /// <value>Gets or sets a value indicating whether this {VirtoCommerce.CatalogModule.Web.Model.Product} has user agreement. (for digital product only)</value>
        [DataMember(Name="hasUserAgreement", EmitDefaultValue=false)]
        public bool? HasUserAgreement { get; set; }

        /// <summary>
        /// Gets or sets the type of the shipping.
        /// </summary>
        /// <value>Gets or sets the type of the shipping.</value>
        [DataMember(Name="shippingType", EmitDefaultValue=false)]
        public string ShippingType { get; set; }

        /// <summary>
        /// Gets or sets the type of the tax.
        /// </summary>
        /// <value>Gets or sets the type of the tax.</value>
        [DataMember(Name="taxType", EmitDefaultValue=false)]
        public string TaxType { get; set; }

        /// <summary>
        /// Gets or sets the product vendor.
        /// </summary>
        /// <value>Gets or sets the product vendor.</value>
        [DataMember(Name="vendor", EmitDefaultValue=false)]
        public string Vendor { get; set; }

        /// <summary>
        /// Gets the default image for the product.
        /// </summary>
        /// <value>Gets the default image for the product.</value>
        [DataMember(Name="imgSrc", EmitDefaultValue=false)]
        public string ImgSrc { get; private set; }

        /// <summary>
        /// Gets or sets the properties.
        /// </summary>
        /// <value>Gets or sets the properties.</value>
        [DataMember(Name="properties", EmitDefaultValue=false)]
        public List<VirtoCommerceCatalogModuleWebModelProperty> Properties { get; set; }

        /// <summary>
        /// Gets or sets the images.
        /// </summary>
        /// <value>Gets or sets the images.</value>
        [DataMember(Name="images", EmitDefaultValue=false)]
        public List<VirtoCommerceCatalogModuleWebModelImage> Images { get; set; }

        /// <summary>
        /// Gets or sets the assets.
        /// </summary>
        /// <value>Gets or sets the assets.</value>
        [DataMember(Name="assets", EmitDefaultValue=false)]
        public List<VirtoCommerceCatalogModuleWebModelAsset> Assets { get; set; }

        /// <summary>
        /// Gets or sets the variations.
        /// </summary>
        /// <value>Gets or sets the variations.</value>
        [DataMember(Name="variations", EmitDefaultValue=false)]
        public List<VirtoCommerceCatalogModuleWebModelProduct> Variations { get; set; }

        /// <summary>
        /// Gets or sets the links.
        /// </summary>
        /// <value>Gets or sets the links.</value>
        [DataMember(Name="links", EmitDefaultValue=false)]
        public List<VirtoCommerceCatalogModuleWebModelCategoryLink> Links { get; set; }

        /// <summary>
        /// Gets or sets the reviews.
        /// </summary>
        /// <value>Gets or sets the reviews.</value>
        [DataMember(Name="reviews", EmitDefaultValue=false)]
        public List<VirtoCommerceCatalogModuleWebModelEditorialReview> Reviews { get; set; }

        /// <summary>
        /// Gets or sets the associations.
        /// </summary>
        /// <value>Gets or sets the associations.</value>
        [DataMember(Name="associations", EmitDefaultValue=false)]
        public List<VirtoCommerceCatalogModuleWebModelProductAssociation> Associations { get; set; }

        /// <summary>
        /// Gets or Sets SecurityScopes
        /// </summary>
        [DataMember(Name="securityScopes", EmitDefaultValue=false)]
        public List<string> SecurityScopes { get; set; }

        /// <summary>
        /// Gets or Sets SeoObjectType
        /// </summary>
        [DataMember(Name="seoObjectType", EmitDefaultValue=false)]
        public string SeoObjectType { get; private set; }

        /// <summary>
        /// Gets or sets the list of SEO information records.
        /// </summary>
        /// <value>Gets or sets the list of SEO information records.</value>
        [DataMember(Name="seoInfos", EmitDefaultValue=false)]
        public List<VirtoCommerceDomainCommerceModelSeoInfo> SeoInfos { get; set; }

        /// <summary>
        /// Gets or Sets Outlines
        /// </summary>
        [DataMember(Name="outlines", EmitDefaultValue=false)]
        public List<VirtoCommerceDomainCatalogModelOutline> Outlines { get; set; }

        /// <summary>
        /// Gets or Sets CreatedDate
        /// </summary>
        [DataMember(Name="createdDate", EmitDefaultValue=false)]
        public DateTime? CreatedDate { get; set; }

        /// <summary>
        /// Gets or Sets ModifiedDate
        /// </summary>
        [DataMember(Name="modifiedDate", EmitDefaultValue=false)]
        public DateTime? ModifiedDate { get; set; }

        /// <summary>
        /// Gets or Sets CreatedBy
        /// </summary>
        [DataMember(Name="createdBy", EmitDefaultValue=false)]
        public string CreatedBy { get; set; }

        /// <summary>
        /// Gets or Sets ModifiedBy
        /// </summary>
        [DataMember(Name="modifiedBy", EmitDefaultValue=false)]
        public string ModifiedBy { get; set; }

        /// <summary>
        /// Gets or Sets Id
        /// </summary>
        [DataMember(Name="id", EmitDefaultValue=false)]
        public string Id { get; set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class VirtoCommerceCatalogModuleWebModelProduct {\n");
            sb.Append("  ManufacturerPartNumber: ").Append(ManufacturerPartNumber).Append("\n");
            sb.Append("  Gtin: ").Append(Gtin).Append("\n");
            sb.Append("  Code: ").Append(Code).Append("\n");
            sb.Append("  Name: ").Append(Name).Append("\n");
            sb.Append("  CatalogId: ").Append(CatalogId).Append("\n");
            sb.Append("  Catalog: ").Append(Catalog).Append("\n");
            sb.Append("  CategoryId: ").Append(CategoryId).Append("\n");
            sb.Append("  Category: ").Append(Category).Append("\n");
            sb.Append("  Outline: ").Append(Outline).Append("\n");
            sb.Append("  Parents: ").Append(Parents).Append("\n");
            sb.Append("  IndexingDate: ").Append(IndexingDate).Append("\n");
            sb.Append("  TitularItemId: ").Append(TitularItemId).Append("\n");
            sb.Append("  IsBuyable: ").Append(IsBuyable).Append("\n");
            sb.Append("  IsActive: ").Append(IsActive).Append("\n");
            sb.Append("  TrackInventory: ").Append(TrackInventory).Append("\n");
            sb.Append("  MaxQuantity: ").Append(MaxQuantity).Append("\n");
            sb.Append("  MinQuantity: ").Append(MinQuantity).Append("\n");
            sb.Append("  ProductType: ").Append(ProductType).Append("\n");
            sb.Append("  WeightUnit: ").Append(WeightUnit).Append("\n");
            sb.Append("  Weight: ").Append(Weight).Append("\n");
            sb.Append("  MeasureUnit: ").Append(MeasureUnit).Append("\n");
            sb.Append("  Height: ").Append(Height).Append("\n");
            sb.Append("  Length: ").Append(Length).Append("\n");
            sb.Append("  Width: ").Append(Width).Append("\n");
            sb.Append("  EnableReview: ").Append(EnableReview).Append("\n");
            sb.Append("  MaxNumberOfDownload: ").Append(MaxNumberOfDownload).Append("\n");
            sb.Append("  DownloadExpiration: ").Append(DownloadExpiration).Append("\n");
            sb.Append("  DownloadType: ").Append(DownloadType).Append("\n");
            sb.Append("  HasUserAgreement: ").Append(HasUserAgreement).Append("\n");
            sb.Append("  ShippingType: ").Append(ShippingType).Append("\n");
            sb.Append("  TaxType: ").Append(TaxType).Append("\n");
            sb.Append("  Vendor: ").Append(Vendor).Append("\n");
            sb.Append("  ImgSrc: ").Append(ImgSrc).Append("\n");
            sb.Append("  Properties: ").Append(Properties).Append("\n");
            sb.Append("  Images: ").Append(Images).Append("\n");
            sb.Append("  Assets: ").Append(Assets).Append("\n");
            sb.Append("  Variations: ").Append(Variations).Append("\n");
            sb.Append("  Links: ").Append(Links).Append("\n");
            sb.Append("  Reviews: ").Append(Reviews).Append("\n");
            sb.Append("  Associations: ").Append(Associations).Append("\n");
            sb.Append("  SecurityScopes: ").Append(SecurityScopes).Append("\n");
            sb.Append("  SeoObjectType: ").Append(SeoObjectType).Append("\n");
            sb.Append("  SeoInfos: ").Append(SeoInfos).Append("\n");
            sb.Append("  Outlines: ").Append(Outlines).Append("\n");
            sb.Append("  CreatedDate: ").Append(CreatedDate).Append("\n");
            sb.Append("  ModifiedDate: ").Append(ModifiedDate).Append("\n");
            sb.Append("  CreatedBy: ").Append(CreatedBy).Append("\n");
            sb.Append("  ModifiedBy: ").Append(ModifiedBy).Append("\n");
            sb.Append("  Id: ").Append(Id).Append("\n");
            sb.Append("}\n");
            return sb.ToString();
        }
  
        /// <summary>
        /// Returns the JSON string presentation of the object
        /// </summary>
        /// <returns>JSON string presentation of the object</returns>
        public string ToJson()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }

        /// <summary>
        /// Returns true if objects are equal
        /// </summary>
        /// <param name="obj">Object to be compared</param>
        /// <returns>Boolean</returns>
        public override bool Equals(object obj)
        {
            // credit: http://stackoverflow.com/a/10454552/677735
            return this.Equals(obj as VirtoCommerceCatalogModuleWebModelProduct);
        }

        /// <summary>
        /// Returns true if VirtoCommerceCatalogModuleWebModelProduct instances are equal
        /// </summary>
        /// <param name="other">Instance of VirtoCommerceCatalogModuleWebModelProduct to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(VirtoCommerceCatalogModuleWebModelProduct other)
        {
            // credit: http://stackoverflow.com/a/10454552/677735
            if (other == null)
                return false;

            return 
                (
                    this.ManufacturerPartNumber == other.ManufacturerPartNumber ||
                    this.ManufacturerPartNumber != null &&
                    this.ManufacturerPartNumber.Equals(other.ManufacturerPartNumber)
                ) && 
                (
                    this.Gtin == other.Gtin ||
                    this.Gtin != null &&
                    this.Gtin.Equals(other.Gtin)
                ) && 
                (
                    this.Code == other.Code ||
                    this.Code != null &&
                    this.Code.Equals(other.Code)
                ) && 
                (
                    this.Name == other.Name ||
                    this.Name != null &&
                    this.Name.Equals(other.Name)
                ) && 
                (
                    this.CatalogId == other.CatalogId ||
                    this.CatalogId != null &&
                    this.CatalogId.Equals(other.CatalogId)
                ) && 
                (
                    this.Catalog == other.Catalog ||
                    this.Catalog != null &&
                    this.Catalog.Equals(other.Catalog)
                ) && 
                (
                    this.CategoryId == other.CategoryId ||
                    this.CategoryId != null &&
                    this.CategoryId.Equals(other.CategoryId)
                ) && 
                (
                    this.Category == other.Category ||
                    this.Category != null &&
                    this.Category.Equals(other.Category)
                ) && 
                (
                    this.Outline == other.Outline ||
                    this.Outline != null &&
                    this.Outline.Equals(other.Outline)
                ) && 
                (
                    this.Parents == other.Parents ||
                    this.Parents != null &&
                    this.Parents.SequenceEqual(other.Parents)
                ) && 
                (
                    this.IndexingDate == other.IndexingDate ||
                    this.IndexingDate != null &&
                    this.IndexingDate.Equals(other.IndexingDate)
                ) && 
                (
                    this.TitularItemId == other.TitularItemId ||
                    this.TitularItemId != null &&
                    this.TitularItemId.Equals(other.TitularItemId)
                ) && 
                (
                    this.IsBuyable == other.IsBuyable ||
                    this.IsBuyable != null &&
                    this.IsBuyable.Equals(other.IsBuyable)
                ) && 
                (
                    this.IsActive == other.IsActive ||
                    this.IsActive != null &&
                    this.IsActive.Equals(other.IsActive)
                ) && 
                (
                    this.TrackInventory == other.TrackInventory ||
                    this.TrackInventory != null &&
                    this.TrackInventory.Equals(other.TrackInventory)
                ) && 
                (
                    this.MaxQuantity == other.MaxQuantity ||
                    this.MaxQuantity != null &&
                    this.MaxQuantity.Equals(other.MaxQuantity)
                ) && 
                (
                    this.MinQuantity == other.MinQuantity ||
                    this.MinQuantity != null &&
                    this.MinQuantity.Equals(other.MinQuantity)
                ) && 
                (
                    this.ProductType == other.ProductType ||
                    this.ProductType != null &&
                    this.ProductType.Equals(other.ProductType)
                ) && 
                (
                    this.WeightUnit == other.WeightUnit ||
                    this.WeightUnit != null &&
                    this.WeightUnit.Equals(other.WeightUnit)
                ) && 
                (
                    this.Weight == other.Weight ||
                    this.Weight != null &&
                    this.Weight.Equals(other.Weight)
                ) && 
                (
                    this.MeasureUnit == other.MeasureUnit ||
                    this.MeasureUnit != null &&
                    this.MeasureUnit.Equals(other.MeasureUnit)
                ) && 
                (
                    this.Height == other.Height ||
                    this.Height != null &&
                    this.Height.Equals(other.Height)
                ) && 
                (
                    this.Length == other.Length ||
                    this.Length != null &&
                    this.Length.Equals(other.Length)
                ) && 
                (
                    this.Width == other.Width ||
                    this.Width != null &&
                    this.Width.Equals(other.Width)
                ) && 
                (
                    this.EnableReview == other.EnableReview ||
                    this.EnableReview != null &&
                    this.EnableReview.Equals(other.EnableReview)
                ) && 
                (
                    this.MaxNumberOfDownload == other.MaxNumberOfDownload ||
                    this.MaxNumberOfDownload != null &&
                    this.MaxNumberOfDownload.Equals(other.MaxNumberOfDownload)
                ) && 
                (
                    this.DownloadExpiration == other.DownloadExpiration ||
                    this.DownloadExpiration != null &&
                    this.DownloadExpiration.Equals(other.DownloadExpiration)
                ) && 
                (
                    this.DownloadType == other.DownloadType ||
                    this.DownloadType != null &&
                    this.DownloadType.Equals(other.DownloadType)
                ) && 
                (
                    this.HasUserAgreement == other.HasUserAgreement ||
                    this.HasUserAgreement != null &&
                    this.HasUserAgreement.Equals(other.HasUserAgreement)
                ) && 
                (
                    this.ShippingType == other.ShippingType ||
                    this.ShippingType != null &&
                    this.ShippingType.Equals(other.ShippingType)
                ) && 
                (
                    this.TaxType == other.TaxType ||
                    this.TaxType != null &&
                    this.TaxType.Equals(other.TaxType)
                ) && 
                (
                    this.Vendor == other.Vendor ||
                    this.Vendor != null &&
                    this.Vendor.Equals(other.Vendor)
                ) && 
                (
                    this.ImgSrc == other.ImgSrc ||
                    this.ImgSrc != null &&
                    this.ImgSrc.Equals(other.ImgSrc)
                ) && 
                (
                    this.Properties == other.Properties ||
                    this.Properties != null &&
                    this.Properties.SequenceEqual(other.Properties)
                ) && 
                (
                    this.Images == other.Images ||
                    this.Images != null &&
                    this.Images.SequenceEqual(other.Images)
                ) && 
                (
                    this.Assets == other.Assets ||
                    this.Assets != null &&
                    this.Assets.SequenceEqual(other.Assets)
                ) && 
                (
                    this.Variations == other.Variations ||
                    this.Variations != null &&
                    this.Variations.SequenceEqual(other.Variations)
                ) && 
                (
                    this.Links == other.Links ||
                    this.Links != null &&
                    this.Links.SequenceEqual(other.Links)
                ) && 
                (
                    this.Reviews == other.Reviews ||
                    this.Reviews != null &&
                    this.Reviews.SequenceEqual(other.Reviews)
                ) && 
                (
                    this.Associations == other.Associations ||
                    this.Associations != null &&
                    this.Associations.SequenceEqual(other.Associations)
                ) && 
                (
                    this.SecurityScopes == other.SecurityScopes ||
                    this.SecurityScopes != null &&
                    this.SecurityScopes.SequenceEqual(other.SecurityScopes)
                ) && 
                (
                    this.SeoObjectType == other.SeoObjectType ||
                    this.SeoObjectType != null &&
                    this.SeoObjectType.Equals(other.SeoObjectType)
                ) && 
                (
                    this.SeoInfos == other.SeoInfos ||
                    this.SeoInfos != null &&
                    this.SeoInfos.SequenceEqual(other.SeoInfos)
                ) && 
                (
                    this.Outlines == other.Outlines ||
                    this.Outlines != null &&
                    this.Outlines.SequenceEqual(other.Outlines)
                ) && 
                (
                    this.CreatedDate == other.CreatedDate ||
                    this.CreatedDate != null &&
                    this.CreatedDate.Equals(other.CreatedDate)
                ) && 
                (
                    this.ModifiedDate == other.ModifiedDate ||
                    this.ModifiedDate != null &&
                    this.ModifiedDate.Equals(other.ModifiedDate)
                ) && 
                (
                    this.CreatedBy == other.CreatedBy ||
                    this.CreatedBy != null &&
                    this.CreatedBy.Equals(other.CreatedBy)
                ) && 
                (
                    this.ModifiedBy == other.ModifiedBy ||
                    this.ModifiedBy != null &&
                    this.ModifiedBy.Equals(other.ModifiedBy)
                ) && 
                (
                    this.Id == other.Id ||
                    this.Id != null &&
                    this.Id.Equals(other.Id)
                );
        }

        /// <summary>
        /// Gets the hash code
        /// </summary>
        /// <returns>Hash code</returns>
        public override int GetHashCode()
        {
            // credit: http://stackoverflow.com/a/263416/677735
            unchecked // Overflow is fine, just wrap
            {
                int hash = 41;
                // Suitable nullity checks etc, of course :)

                if (this.ManufacturerPartNumber != null)
                    hash = hash * 59 + this.ManufacturerPartNumber.GetHashCode();

                if (this.Gtin != null)
                    hash = hash * 59 + this.Gtin.GetHashCode();

                if (this.Code != null)
                    hash = hash * 59 + this.Code.GetHashCode();

                if (this.Name != null)
                    hash = hash * 59 + this.Name.GetHashCode();

                if (this.CatalogId != null)
                    hash = hash * 59 + this.CatalogId.GetHashCode();

                if (this.Catalog != null)
                    hash = hash * 59 + this.Catalog.GetHashCode();

                if (this.CategoryId != null)
                    hash = hash * 59 + this.CategoryId.GetHashCode();

                if (this.Category != null)
                    hash = hash * 59 + this.Category.GetHashCode();

                if (this.Outline != null)
                    hash = hash * 59 + this.Outline.GetHashCode();

                if (this.Parents != null)
                    hash = hash * 59 + this.Parents.GetHashCode();

                if (this.IndexingDate != null)
                    hash = hash * 59 + this.IndexingDate.GetHashCode();

                if (this.TitularItemId != null)
                    hash = hash * 59 + this.TitularItemId.GetHashCode();

                if (this.IsBuyable != null)
                    hash = hash * 59 + this.IsBuyable.GetHashCode();

                if (this.IsActive != null)
                    hash = hash * 59 + this.IsActive.GetHashCode();

                if (this.TrackInventory != null)
                    hash = hash * 59 + this.TrackInventory.GetHashCode();

                if (this.MaxQuantity != null)
                    hash = hash * 59 + this.MaxQuantity.GetHashCode();

                if (this.MinQuantity != null)
                    hash = hash * 59 + this.MinQuantity.GetHashCode();

                if (this.ProductType != null)
                    hash = hash * 59 + this.ProductType.GetHashCode();

                if (this.WeightUnit != null)
                    hash = hash * 59 + this.WeightUnit.GetHashCode();

                if (this.Weight != null)
                    hash = hash * 59 + this.Weight.GetHashCode();

                if (this.MeasureUnit != null)
                    hash = hash * 59 + this.MeasureUnit.GetHashCode();

                if (this.Height != null)
                    hash = hash * 59 + this.Height.GetHashCode();

                if (this.Length != null)
                    hash = hash * 59 + this.Length.GetHashCode();

                if (this.Width != null)
                    hash = hash * 59 + this.Width.GetHashCode();

                if (this.EnableReview != null)
                    hash = hash * 59 + this.EnableReview.GetHashCode();

                if (this.MaxNumberOfDownload != null)
                    hash = hash * 59 + this.MaxNumberOfDownload.GetHashCode();

                if (this.DownloadExpiration != null)
                    hash = hash * 59 + this.DownloadExpiration.GetHashCode();

                if (this.DownloadType != null)
                    hash = hash * 59 + this.DownloadType.GetHashCode();

                if (this.HasUserAgreement != null)
                    hash = hash * 59 + this.HasUserAgreement.GetHashCode();

                if (this.ShippingType != null)
                    hash = hash * 59 + this.ShippingType.GetHashCode();

                if (this.TaxType != null)
                    hash = hash * 59 + this.TaxType.GetHashCode();

                if (this.Vendor != null)
                    hash = hash * 59 + this.Vendor.GetHashCode();

                if (this.ImgSrc != null)
                    hash = hash * 59 + this.ImgSrc.GetHashCode();

                if (this.Properties != null)
                    hash = hash * 59 + this.Properties.GetHashCode();

                if (this.Images != null)
                    hash = hash * 59 + this.Images.GetHashCode();

                if (this.Assets != null)
                    hash = hash * 59 + this.Assets.GetHashCode();

                if (this.Variations != null)
                    hash = hash * 59 + this.Variations.GetHashCode();

                if (this.Links != null)
                    hash = hash * 59 + this.Links.GetHashCode();

                if (this.Reviews != null)
                    hash = hash * 59 + this.Reviews.GetHashCode();

                if (this.Associations != null)
                    hash = hash * 59 + this.Associations.GetHashCode();

                if (this.SecurityScopes != null)
                    hash = hash * 59 + this.SecurityScopes.GetHashCode();

                if (this.SeoObjectType != null)
                    hash = hash * 59 + this.SeoObjectType.GetHashCode();

                if (this.SeoInfos != null)
                    hash = hash * 59 + this.SeoInfos.GetHashCode();

                if (this.Outlines != null)
                    hash = hash * 59 + this.Outlines.GetHashCode();

                if (this.CreatedDate != null)
                    hash = hash * 59 + this.CreatedDate.GetHashCode();

                if (this.ModifiedDate != null)
                    hash = hash * 59 + this.ModifiedDate.GetHashCode();

                if (this.CreatedBy != null)
                    hash = hash * 59 + this.CreatedBy.GetHashCode();

                if (this.ModifiedBy != null)
                    hash = hash * 59 + this.ModifiedBy.GetHashCode();

                if (this.Id != null)
                    hash = hash * 59 + this.Id.GetHashCode();

                return hash;
            }
        }

    }
}
