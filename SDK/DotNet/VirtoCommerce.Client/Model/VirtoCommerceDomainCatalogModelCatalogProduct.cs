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
    /// 
    /// </summary>
    [DataContract]
    public partial class VirtoCommerceDomainCatalogModelCatalogProduct :  IEquatable<VirtoCommerceDomainCatalogModelCatalogProduct>
    {
        /// <summary>
        /// Gets or Sets Code
        /// </summary>
        [DataMember(Name="code", EmitDefaultValue=false)]
        public string Code { get; set; }

        /// <summary>
        /// Gets or Sets ManufacturerPartNumber
        /// </summary>
        [DataMember(Name="manufacturerPartNumber", EmitDefaultValue=false)]
        public string ManufacturerPartNumber { get; set; }

        /// <summary>
        /// Gets or Sets Gtin
        /// </summary>
        [DataMember(Name="gtin", EmitDefaultValue=false)]
        public string Gtin { get; set; }

        /// <summary>
        /// Gets or Sets Name
        /// </summary>
        [DataMember(Name="name", EmitDefaultValue=false)]
        public string Name { get; set; }

        /// <summary>
        /// Gets or Sets CatalogId
        /// </summary>
        [DataMember(Name="catalogId", EmitDefaultValue=false)]
        public string CatalogId { get; set; }

        /// <summary>
        /// Gets or Sets Catalog
        /// </summary>
        [DataMember(Name="catalog", EmitDefaultValue=false)]
        public VirtoCommerceDomainCatalogModelCatalog Catalog { get; set; }

        /// <summary>
        /// Gets or Sets CategoryId
        /// </summary>
        [DataMember(Name="categoryId", EmitDefaultValue=false)]
        public string CategoryId { get; set; }

        /// <summary>
        /// Gets or Sets Category
        /// </summary>
        [DataMember(Name="category", EmitDefaultValue=false)]
        public VirtoCommerceDomainCatalogModelCategory Category { get; set; }

        /// <summary>
        /// Gets or Sets MainProductId
        /// </summary>
        [DataMember(Name="mainProductId", EmitDefaultValue=false)]
        public string MainProductId { get; set; }

        /// <summary>
        /// Gets or Sets MainProduct
        /// </summary>
        [DataMember(Name="mainProduct", EmitDefaultValue=false)]
        public VirtoCommerceDomainCatalogModelCatalogProduct MainProduct { get; set; }

        /// <summary>
        /// Gets or Sets IsBuyable
        /// </summary>
        [DataMember(Name="isBuyable", EmitDefaultValue=false)]
        public bool? IsBuyable { get; set; }

        /// <summary>
        /// Gets or Sets IsActive
        /// </summary>
        [DataMember(Name="isActive", EmitDefaultValue=false)]
        public bool? IsActive { get; set; }

        /// <summary>
        /// Gets or Sets TrackInventory
        /// </summary>
        [DataMember(Name="trackInventory", EmitDefaultValue=false)]
        public bool? TrackInventory { get; set; }

        /// <summary>
        /// Gets or Sets IndexingDate
        /// </summary>
        [DataMember(Name="indexingDate", EmitDefaultValue=false)]
        public DateTime? IndexingDate { get; set; }

        /// <summary>
        /// Gets or Sets MaxQuantity
        /// </summary>
        [DataMember(Name="maxQuantity", EmitDefaultValue=false)]
        public int? MaxQuantity { get; set; }

        /// <summary>
        /// Gets or Sets MinQuantity
        /// </summary>
        [DataMember(Name="minQuantity", EmitDefaultValue=false)]
        public int? MinQuantity { get; set; }

        /// <summary>
        /// Gets or Sets ProductType
        /// </summary>
        [DataMember(Name="productType", EmitDefaultValue=false)]
        public string ProductType { get; set; }

        /// <summary>
        /// Gets or Sets WeightUnit
        /// </summary>
        [DataMember(Name="weightUnit", EmitDefaultValue=false)]
        public string WeightUnit { get; set; }

        /// <summary>
        /// Gets or Sets Weight
        /// </summary>
        [DataMember(Name="weight", EmitDefaultValue=false)]
        public double? Weight { get; set; }

        /// <summary>
        /// Gets or Sets MeasureUnit
        /// </summary>
        [DataMember(Name="measureUnit", EmitDefaultValue=false)]
        public string MeasureUnit { get; set; }

        /// <summary>
        /// Gets or Sets Height
        /// </summary>
        [DataMember(Name="height", EmitDefaultValue=false)]
        public double? Height { get; set; }

        /// <summary>
        /// Gets or Sets Length
        /// </summary>
        [DataMember(Name="length", EmitDefaultValue=false)]
        public double? Length { get; set; }

        /// <summary>
        /// Gets or Sets Width
        /// </summary>
        [DataMember(Name="width", EmitDefaultValue=false)]
        public double? Width { get; set; }

        /// <summary>
        /// Gets or Sets EnableReview
        /// </summary>
        [DataMember(Name="enableReview", EmitDefaultValue=false)]
        public bool? EnableReview { get; set; }

        /// <summary>
        /// Gets or Sets MaxNumberOfDownload
        /// </summary>
        [DataMember(Name="maxNumberOfDownload", EmitDefaultValue=false)]
        public int? MaxNumberOfDownload { get; set; }

        /// <summary>
        /// Gets or Sets DownloadExpiration
        /// </summary>
        [DataMember(Name="downloadExpiration", EmitDefaultValue=false)]
        public DateTime? DownloadExpiration { get; set; }

        /// <summary>
        /// Gets or Sets DownloadType
        /// </summary>
        [DataMember(Name="downloadType", EmitDefaultValue=false)]
        public string DownloadType { get; set; }

        /// <summary>
        /// Gets or Sets HasUserAgreement
        /// </summary>
        [DataMember(Name="hasUserAgreement", EmitDefaultValue=false)]
        public bool? HasUserAgreement { get; set; }

        /// <summary>
        /// Gets or Sets ShippingType
        /// </summary>
        [DataMember(Name="shippingType", EmitDefaultValue=false)]
        public string ShippingType { get; set; }

        /// <summary>
        /// Gets or Sets TaxType
        /// </summary>
        [DataMember(Name="taxType", EmitDefaultValue=false)]
        public string TaxType { get; set; }

        /// <summary>
        /// Gets or Sets Vendor
        /// </summary>
        [DataMember(Name="vendor", EmitDefaultValue=false)]
        public string Vendor { get; set; }

        /// <summary>
        /// Gets or Sets StartDate
        /// </summary>
        [DataMember(Name="startDate", EmitDefaultValue=false)]
        public DateTime? StartDate { get; set; }

        /// <summary>
        /// Gets or Sets EndDate
        /// </summary>
        [DataMember(Name="endDate", EmitDefaultValue=false)]
        public DateTime? EndDate { get; set; }

        /// <summary>
        /// Gets or Sets Properties
        /// </summary>
        [DataMember(Name="properties", EmitDefaultValue=false)]
        public List<VirtoCommerceDomainCatalogModelProperty> Properties { get; set; }

        /// <summary>
        /// Gets or Sets PropertyValues
        /// </summary>
        [DataMember(Name="propertyValues", EmitDefaultValue=false)]
        public List<VirtoCommerceDomainCatalogModelPropertyValue> PropertyValues { get; set; }

        /// <summary>
        /// Gets or Sets Images
        /// </summary>
        [DataMember(Name="images", EmitDefaultValue=false)]
        public List<VirtoCommerceDomainCatalogModelImage> Images { get; set; }

        /// <summary>
        /// Gets or Sets Assets
        /// </summary>
        [DataMember(Name="assets", EmitDefaultValue=false)]
        public List<VirtoCommerceDomainCatalogModelAsset> Assets { get; set; }

        /// <summary>
        /// Gets or Sets Links
        /// </summary>
        [DataMember(Name="links", EmitDefaultValue=false)]
        public List<VirtoCommerceDomainCatalogModelCategoryLink> Links { get; set; }

        /// <summary>
        /// Gets or Sets Variations
        /// </summary>
        [DataMember(Name="variations", EmitDefaultValue=false)]
        public List<VirtoCommerceDomainCatalogModelCatalogProduct> Variations { get; set; }

        /// <summary>
        /// Gets or Sets SeoObjectType
        /// </summary>
        [DataMember(Name="seoObjectType", EmitDefaultValue=false)]
        public string SeoObjectType { get; private set; }

        /// <summary>
        /// Gets or Sets SeoInfos
        /// </summary>
        [DataMember(Name="seoInfos", EmitDefaultValue=false)]
        public List<VirtoCommerceDomainCommerceModelSeoInfo> SeoInfos { get; set; }

        /// <summary>
        /// Gets or Sets Reviews
        /// </summary>
        [DataMember(Name="reviews", EmitDefaultValue=false)]
        public List<VirtoCommerceDomainCatalogModelEditorialReview> Reviews { get; set; }

        /// <summary>
        /// Gets or Sets Associations
        /// </summary>
        [DataMember(Name="associations", EmitDefaultValue=false)]
        public List<VirtoCommerceDomainCatalogModelProductAssociation> Associations { get; set; }

        /// <summary>
        /// Gets or Sets Prices
        /// </summary>
        [DataMember(Name="prices", EmitDefaultValue=false)]
        public List<VirtoCommerceDomainPricingModelPrice> Prices { get; set; }

        /// <summary>
        /// Gets or Sets Inventories
        /// </summary>
        [DataMember(Name="inventories", EmitDefaultValue=false)]
        public List<VirtoCommerceDomainInventoryModelInventoryInfo> Inventories { get; set; }

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
            sb.Append("class VirtoCommerceDomainCatalogModelCatalogProduct {\n");
            sb.Append("  Code: ").Append(Code).Append("\n");
            sb.Append("  ManufacturerPartNumber: ").Append(ManufacturerPartNumber).Append("\n");
            sb.Append("  Gtin: ").Append(Gtin).Append("\n");
            sb.Append("  Name: ").Append(Name).Append("\n");
            sb.Append("  CatalogId: ").Append(CatalogId).Append("\n");
            sb.Append("  Catalog: ").Append(Catalog).Append("\n");
            sb.Append("  CategoryId: ").Append(CategoryId).Append("\n");
            sb.Append("  Category: ").Append(Category).Append("\n");
            sb.Append("  MainProductId: ").Append(MainProductId).Append("\n");
            sb.Append("  MainProduct: ").Append(MainProduct).Append("\n");
            sb.Append("  IsBuyable: ").Append(IsBuyable).Append("\n");
            sb.Append("  IsActive: ").Append(IsActive).Append("\n");
            sb.Append("  TrackInventory: ").Append(TrackInventory).Append("\n");
            sb.Append("  IndexingDate: ").Append(IndexingDate).Append("\n");
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
            sb.Append("  StartDate: ").Append(StartDate).Append("\n");
            sb.Append("  EndDate: ").Append(EndDate).Append("\n");
            sb.Append("  Properties: ").Append(Properties).Append("\n");
            sb.Append("  PropertyValues: ").Append(PropertyValues).Append("\n");
            sb.Append("  Images: ").Append(Images).Append("\n");
            sb.Append("  Assets: ").Append(Assets).Append("\n");
            sb.Append("  Links: ").Append(Links).Append("\n");
            sb.Append("  Variations: ").Append(Variations).Append("\n");
            sb.Append("  SeoObjectType: ").Append(SeoObjectType).Append("\n");
            sb.Append("  SeoInfos: ").Append(SeoInfos).Append("\n");
            sb.Append("  Reviews: ").Append(Reviews).Append("\n");
            sb.Append("  Associations: ").Append(Associations).Append("\n");
            sb.Append("  Prices: ").Append(Prices).Append("\n");
            sb.Append("  Inventories: ").Append(Inventories).Append("\n");
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
            return this.Equals(obj as VirtoCommerceDomainCatalogModelCatalogProduct);
        }

        /// <summary>
        /// Returns true if VirtoCommerceDomainCatalogModelCatalogProduct instances are equal
        /// </summary>
        /// <param name="other">Instance of VirtoCommerceDomainCatalogModelCatalogProduct to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(VirtoCommerceDomainCatalogModelCatalogProduct other)
        {
            // credit: http://stackoverflow.com/a/10454552/677735
            if (other == null)
                return false;

            return 
                (
                    this.Code == other.Code ||
                    this.Code != null &&
                    this.Code.Equals(other.Code)
                ) && 
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
                    this.MainProductId == other.MainProductId ||
                    this.MainProductId != null &&
                    this.MainProductId.Equals(other.MainProductId)
                ) && 
                (
                    this.MainProduct == other.MainProduct ||
                    this.MainProduct != null &&
                    this.MainProduct.Equals(other.MainProduct)
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
                    this.IndexingDate == other.IndexingDate ||
                    this.IndexingDate != null &&
                    this.IndexingDate.Equals(other.IndexingDate)
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
                    this.StartDate == other.StartDate ||
                    this.StartDate != null &&
                    this.StartDate.Equals(other.StartDate)
                ) && 
                (
                    this.EndDate == other.EndDate ||
                    this.EndDate != null &&
                    this.EndDate.Equals(other.EndDate)
                ) && 
                (
                    this.Properties == other.Properties ||
                    this.Properties != null &&
                    this.Properties.SequenceEqual(other.Properties)
                ) && 
                (
                    this.PropertyValues == other.PropertyValues ||
                    this.PropertyValues != null &&
                    this.PropertyValues.SequenceEqual(other.PropertyValues)
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
                    this.Links == other.Links ||
                    this.Links != null &&
                    this.Links.SequenceEqual(other.Links)
                ) && 
                (
                    this.Variations == other.Variations ||
                    this.Variations != null &&
                    this.Variations.SequenceEqual(other.Variations)
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
                    this.Prices == other.Prices ||
                    this.Prices != null &&
                    this.Prices.SequenceEqual(other.Prices)
                ) && 
                (
                    this.Inventories == other.Inventories ||
                    this.Inventories != null &&
                    this.Inventories.SequenceEqual(other.Inventories)
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

                if (this.Code != null)
                    hash = hash * 59 + this.Code.GetHashCode();

                if (this.ManufacturerPartNumber != null)
                    hash = hash * 59 + this.ManufacturerPartNumber.GetHashCode();

                if (this.Gtin != null)
                    hash = hash * 59 + this.Gtin.GetHashCode();

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

                if (this.MainProductId != null)
                    hash = hash * 59 + this.MainProductId.GetHashCode();

                if (this.MainProduct != null)
                    hash = hash * 59 + this.MainProduct.GetHashCode();

                if (this.IsBuyable != null)
                    hash = hash * 59 + this.IsBuyable.GetHashCode();

                if (this.IsActive != null)
                    hash = hash * 59 + this.IsActive.GetHashCode();

                if (this.TrackInventory != null)
                    hash = hash * 59 + this.TrackInventory.GetHashCode();

                if (this.IndexingDate != null)
                    hash = hash * 59 + this.IndexingDate.GetHashCode();

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

                if (this.StartDate != null)
                    hash = hash * 59 + this.StartDate.GetHashCode();

                if (this.EndDate != null)
                    hash = hash * 59 + this.EndDate.GetHashCode();

                if (this.Properties != null)
                    hash = hash * 59 + this.Properties.GetHashCode();

                if (this.PropertyValues != null)
                    hash = hash * 59 + this.PropertyValues.GetHashCode();

                if (this.Images != null)
                    hash = hash * 59 + this.Images.GetHashCode();

                if (this.Assets != null)
                    hash = hash * 59 + this.Assets.GetHashCode();

                if (this.Links != null)
                    hash = hash * 59 + this.Links.GetHashCode();

                if (this.Variations != null)
                    hash = hash * 59 + this.Variations.GetHashCode();

                if (this.SeoObjectType != null)
                    hash = hash * 59 + this.SeoObjectType.GetHashCode();

                if (this.SeoInfos != null)
                    hash = hash * 59 + this.SeoInfos.GetHashCode();

                if (this.Reviews != null)
                    hash = hash * 59 + this.Reviews.GetHashCode();

                if (this.Associations != null)
                    hash = hash * 59 + this.Associations.GetHashCode();

                if (this.Prices != null)
                    hash = hash * 59 + this.Prices.GetHashCode();

                if (this.Inventories != null)
                    hash = hash * 59 + this.Inventories.GetHashCode();

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
