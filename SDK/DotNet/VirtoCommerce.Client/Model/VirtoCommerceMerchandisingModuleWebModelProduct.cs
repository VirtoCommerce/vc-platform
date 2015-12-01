using System;
using System.Linq;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;



namespace VirtoCommerce.Client.Model
{

    /// <summary>
    /// 
    /// </summary>
    [DataContract]
    public class VirtoCommerceMerchandisingModuleWebModelProduct : IEquatable<VirtoCommerceMerchandisingModuleWebModelProduct>
    {
        
        /// <summary>
        /// Gets or sets the collection of product variations
        /// </summary>
        /// <value>Gets or sets the collection of product variations</value>
        [DataMember(Name="variations", EmitDefaultValue=false)]
        public List<VirtoCommerceMerchandisingModuleWebModelProductVariation> Variations { get; set; }
  
        
        /// <summary>
        /// Gets or sets the value of catalog item manufacturer part number
        /// </summary>
        /// <value>Gets or sets the value of catalog item manufacturer part number</value>
        [DataMember(Name="manufacturerPartNumber", EmitDefaultValue=false)]
        public string ManufacturerPartNumber { get; set; }
  
        
        /// <summary>
        /// Gets or sets the value of catalog item Global Trade Item Number (GTIN).\r\n            These identifiers include UPC (in North America), EAN (in Europe), JAN (in Japan), and ISBN (for books).
        /// </summary>
        /// <value>Gets or sets the value of catalog item Global Trade Item Number (GTIN).\r\n            These identifiers include UPC (in North America), EAN (in Europe), JAN (in Japan), and ISBN (for books).</value>
        [DataMember(Name="gtin", EmitDefaultValue=false)]
        public string Gtin { get; set; }
  
        
        /// <summary>
        /// Gets or sets the collection of product associations
        /// </summary>
        /// <value>Gets or sets the collection of product associations</value>
        [DataMember(Name="associations", EmitDefaultValue=false)]
        public List<VirtoCommerceMerchandisingModuleWebModelAssociation> Associations { get; set; }
  
        
        /// <summary>
        /// Gets or sets the value of catalog id
        /// </summary>
        /// <value>Gets or sets the value of catalog id</value>
        [DataMember(Name="catalogId", EmitDefaultValue=false)]
        public string CatalogId { get; set; }
  
        
        /// <summary>
        /// Gets or sets the value of category id
        /// </summary>
        /// <value>Gets or sets the value of category id</value>
        [DataMember(Name="categoryId", EmitDefaultValue=false)]
        public string CategoryId { get; set; }
  
        
        /// <summary>
        /// Gets or sets the value of catalog item code
        /// </summary>
        /// <value>Gets or sets the value of catalog item code</value>
        [DataMember(Name="code", EmitDefaultValue=false)]
        public string Code { get; set; }
  
        
        /// <summary>
        /// Gets or sets the collection of catalog item editorial reviews
        /// </summary>
        /// <value>Gets or sets the collection of catalog item editorial reviews</value>
        [DataMember(Name="editorialReviews", EmitDefaultValue=false)]
        public List<VirtoCommerceMerchandisingModuleWebModelEditorialReview> EditorialReviews { get; set; }
  
        
        /// <summary>
        /// Gets or sets the value of catalog item id
        /// </summary>
        /// <value>Gets or sets the value of catalog item id</value>
        [DataMember(Name="id", EmitDefaultValue=false)]
        public string Id { get; set; }
  
        
        /// <summary>
        /// Gets or sets the value of catalog item primary image
        /// </summary>
        /// <value>Gets or sets the value of catalog item primary image</value>
        [DataMember(Name="primaryImage", EmitDefaultValue=false)]
        public VirtoCommerceMerchandisingModuleWebModelImage PrimaryImage { get; set; }
  
        
        /// <summary>
        /// Gets or sets the collection of catalog item images
        /// </summary>
        /// <value>Gets or sets the collection of catalog item images</value>
        [DataMember(Name="images", EmitDefaultValue=false)]
        public List<VirtoCommerceMerchandisingModuleWebModelImage> Images { get; set; }
  
        
        /// <summary>
        /// Gets or sets the collection of catalog item assets
        /// </summary>
        /// <value>Gets or sets the collection of catalog item assets</value>
        [DataMember(Name="assets", EmitDefaultValue=false)]
        public List<VirtoCommerceMerchandisingModuleWebModelAsset> Assets { get; set; }
  
        
        /// <summary>
        /// Gets or sets the value of catalog item main product id
        /// </summary>
        /// <value>Gets or sets the value of catalog item main product id</value>
        [DataMember(Name="mainProductId", EmitDefaultValue=false)]
        public string MainProductId { get; set; }
  
        
        /// <summary>
        /// Gets or sets the ability to perform inventory tracking for catalog item
        /// </summary>
        /// <value>Gets or sets the ability to perform inventory tracking for catalog item</value>
        [DataMember(Name="trackInventory", EmitDefaultValue=false)]
        public bool? TrackInventory { get; set; }
  
        
        /// <summary>
        /// Gets or sets the ability to buy catalog item
        /// </summary>
        /// <value>Gets or sets the ability to buy catalog item</value>
        [DataMember(Name="isBuyable", EmitDefaultValue=false)]
        public bool? IsBuyable { get; set; }
  
        
        /// <summary>
        /// Gets or sets the activity status of catalog item
        /// </summary>
        /// <value>Gets or sets the activity status of catalog item</value>
        [DataMember(Name="isActive", EmitDefaultValue=false)]
        public bool? IsActive { get; set; }
  
        
        /// <summary>
        /// Gets or sets the value of catalog item maximum inventory quantity
        /// </summary>
        /// <value>Gets or sets the value of catalog item maximum inventory quantity</value>
        [DataMember(Name="maxQuantity", EmitDefaultValue=false)]
        public int? MaxQuantity { get; set; }
  
        
        /// <summary>
        /// Gets or sets the value of catalog item minimum inventory quantity
        /// </summary>
        /// <value>Gets or sets the value of catalog item minimum inventory quantity</value>
        [DataMember(Name="minQuantity", EmitDefaultValue=false)]
        public int? MinQuantity { get; set; }
  
        
        /// <summary>
        /// Gets or sets the value of catalog item name
        /// </summary>
        /// <value>Gets or sets the value of catalog item name</value>
        [DataMember(Name="name", EmitDefaultValue=false)]
        public string Name { get; set; }
  
        
        /// <summary>
        /// Gets or sets the value of catalog item categories outline
        /// </summary>
        /// <value>Gets or sets the value of catalog item categories outline</value>
        [DataMember(Name="outline", EmitDefaultValue=false)]
        public string Outline { get; set; }
  
        
        /// <summary>
        /// Gets or sets the dictionary of catalog item properties
        /// </summary>
        /// <value>Gets or sets the dictionary of catalog item properties</value>
        [DataMember(Name="properties", EmitDefaultValue=false)]
        public Dictionary<string, Object> Properties { get; set; }
  
        
        /// <summary>
        /// Gets or sets the dictionary of catalog item variation properties
        /// </summary>
        /// <value>Gets or sets the dictionary of catalog item variation properties</value>
        [DataMember(Name="variationProperties", EmitDefaultValue=false)]
        public Dictionary<string, Object> VariationProperties { get; set; }
  
        
        /// <summary>
        /// Gets or sets the value of catalog item rating
        /// </summary>
        /// <value>Gets or sets the value of catalog item rating</value>
        [DataMember(Name="rating", EmitDefaultValue=false)]
        public double? Rating { get; set; }
  
        
        /// <summary>
        /// Gets or sets the value of catalog item total reviews quantity
        /// </summary>
        /// <value>Gets or sets the value of catalog item total reviews quantity</value>
        [DataMember(Name="reviewsTotal", EmitDefaultValue=false)]
        public int? ReviewsTotal { get; set; }
  
        
        /// <summary>
        /// Gets or sets the collection of catalog item SEO parameters
        /// </summary>
        /// <value>Gets or sets the collection of catalog item SEO parameters</value>
        [DataMember(Name="seo", EmitDefaultValue=false)]
        public List<VirtoCommerceMerchandisingModuleWebModelSeoKeyword> Seo { get; set; }
  
        
        /// <summary>
        /// Gets or sets the value of catalog item selling start date/time
        /// </summary>
        /// <value>Gets or sets the value of catalog item selling start date/time</value>
        [DataMember(Name="startDate", EmitDefaultValue=false)]
        public DateTime? StartDate { get; set; }
  
        
        /// <summary>
        /// Gets or sets the value of catalog item selling end date/time
        /// </summary>
        /// <value>Gets or sets the value of catalog item selling end date/time</value>
        [DataMember(Name="endDate", EmitDefaultValue=false)]
        public DateTime? EndDate { get; set; }
  
        
        /// <summary>
        /// Gets or sets the value of catalog item type
        /// </summary>
        /// <value>Gets or sets the value of catalog item type</value>
        [DataMember(Name="productType", EmitDefaultValue=false)]
        public string ProductType { get; set; }
  
        
        /// <summary>
        /// Gets or sets the value of catalog item weight unit
        /// </summary>
        /// <value>Gets or sets the value of catalog item weight unit</value>
        [DataMember(Name="weightUnit", EmitDefaultValue=false)]
        public string WeightUnit { get; set; }
  
        
        /// <summary>
        /// Gets or sets the value of catalog item weight
        /// </summary>
        /// <value>Gets or sets the value of catalog item weight</value>
        [DataMember(Name="weight", EmitDefaultValue=false)]
        public double? Weight { get; set; }
  
        
        /// <summary>
        /// Gets or sets the value of catalog item measurement unit
        /// </summary>
        /// <value>Gets or sets the value of catalog item measurement unit</value>
        [DataMember(Name="measureUnit", EmitDefaultValue=false)]
        public string MeasureUnit { get; set; }
  
        
        /// <summary>
        /// Gets or sets the value of catalog item height
        /// </summary>
        /// <value>Gets or sets the value of catalog item height</value>
        [DataMember(Name="height", EmitDefaultValue=false)]
        public double? Height { get; set; }
  
        
        /// <summary>
        /// Gets or sets the value of catalog item length
        /// </summary>
        /// <value>Gets or sets the value of catalog item length</value>
        [DataMember(Name="length", EmitDefaultValue=false)]
        public double? Length { get; set; }
  
        
        /// <summary>
        /// Gets or sets the value of catalog item width
        /// </summary>
        /// <value>Gets or sets the value of catalog item width</value>
        [DataMember(Name="width", EmitDefaultValue=false)]
        public double? Width { get; set; }
  
        
        /// <summary>
        /// Gets or sets the ability to add a review for catalog item
        /// </summary>
        /// <value>Gets or sets the ability to add a review for catalog item</value>
        [DataMember(Name="enableReview", EmitDefaultValue=false)]
        public bool? EnableReview { get; set; }
  
        
        /// <summary>
        /// Gets or sets catalog item inventory policy
        /// </summary>
        /// <value>Gets or sets catalog item inventory policy</value>
        [DataMember(Name="inventory", EmitDefaultValue=false)]
        public VirtoCommerceMerchandisingModuleWebModelInventory Inventory { get; set; }
  
        
        /// <summary>
        /// Gets or sets the value of catalog item maximum download limit (for digital products)
        /// </summary>
        /// <value>Gets or sets the value of catalog item maximum download limit (for digital products)</value>
        [DataMember(Name="maxNumberOfDownload", EmitDefaultValue=false)]
        public int? MaxNumberOfDownload { get; set; }
  
        
        /// <summary>
        /// Gets or sets the value of catalog item download expiration date/time (for digital products)
        /// </summary>
        /// <value>Gets or sets the value of catalog item download expiration date/time (for digital products)</value>
        [DataMember(Name="downloadExpiration", EmitDefaultValue=false)]
        public DateTime? DownloadExpiration { get; set; }
  
        
        /// <summary>
        /// Gets or sets the value of catalog item download type
        /// </summary>
        /// <value>Gets or sets the value of catalog item download type</value>
        [DataMember(Name="downloadType", EmitDefaultValue=false)]
        public string DownloadType { get; set; }
  
        
        /// <summary>
        /// Gets or sets the presence of catalog item end-user license agreement (for digital products)
        /// </summary>
        /// <value>Gets or sets the presence of catalog item end-user license agreement (for digital products)</value>
        [DataMember(Name="hasUserAgreement", EmitDefaultValue=false)]
        public bool? HasUserAgreement { get; set; }
  
        
        /// <summary>
        /// Gets or sets the value of catalog item shipping type
        /// </summary>
        /// <value>Gets or sets the value of catalog item shipping type</value>
        [DataMember(Name="shippingType", EmitDefaultValue=false)]
        public string ShippingType { get; set; }
  
        
        /// <summary>
        /// Gets or sets the value of catalog item tax type
        /// </summary>
        /// <value>Gets or sets the value of catalog item tax type</value>
        [DataMember(Name="taxType", EmitDefaultValue=false)]
        public string TaxType { get; set; }
  
        
        /// <summary>
        /// Gets or sets the value of catalog item vendor name
        /// </summary>
        /// <value>Gets or sets the value of catalog item vendor name</value>
        [DataMember(Name="vendor", EmitDefaultValue=false)]
        public string Vendor { get; set; }
  
        
  
        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class VirtoCommerceMerchandisingModuleWebModelProduct {\n");
            sb.Append("  Variations: ").Append(Variations).Append("\n");
            sb.Append("  ManufacturerPartNumber: ").Append(ManufacturerPartNumber).Append("\n");
            sb.Append("  Gtin: ").Append(Gtin).Append("\n");
            sb.Append("  Associations: ").Append(Associations).Append("\n");
            sb.Append("  CatalogId: ").Append(CatalogId).Append("\n");
            sb.Append("  CategoryId: ").Append(CategoryId).Append("\n");
            sb.Append("  Code: ").Append(Code).Append("\n");
            sb.Append("  EditorialReviews: ").Append(EditorialReviews).Append("\n");
            sb.Append("  Id: ").Append(Id).Append("\n");
            sb.Append("  PrimaryImage: ").Append(PrimaryImage).Append("\n");
            sb.Append("  Images: ").Append(Images).Append("\n");
            sb.Append("  Assets: ").Append(Assets).Append("\n");
            sb.Append("  MainProductId: ").Append(MainProductId).Append("\n");
            sb.Append("  TrackInventory: ").Append(TrackInventory).Append("\n");
            sb.Append("  IsBuyable: ").Append(IsBuyable).Append("\n");
            sb.Append("  IsActive: ").Append(IsActive).Append("\n");
            sb.Append("  MaxQuantity: ").Append(MaxQuantity).Append("\n");
            sb.Append("  MinQuantity: ").Append(MinQuantity).Append("\n");
            sb.Append("  Name: ").Append(Name).Append("\n");
            sb.Append("  Outline: ").Append(Outline).Append("\n");
            sb.Append("  Properties: ").Append(Properties).Append("\n");
            sb.Append("  VariationProperties: ").Append(VariationProperties).Append("\n");
            sb.Append("  Rating: ").Append(Rating).Append("\n");
            sb.Append("  ReviewsTotal: ").Append(ReviewsTotal).Append("\n");
            sb.Append("  Seo: ").Append(Seo).Append("\n");
            sb.Append("  StartDate: ").Append(StartDate).Append("\n");
            sb.Append("  EndDate: ").Append(EndDate).Append("\n");
            sb.Append("  ProductType: ").Append(ProductType).Append("\n");
            sb.Append("  WeightUnit: ").Append(WeightUnit).Append("\n");
            sb.Append("  Weight: ").Append(Weight).Append("\n");
            sb.Append("  MeasureUnit: ").Append(MeasureUnit).Append("\n");
            sb.Append("  Height: ").Append(Height).Append("\n");
            sb.Append("  Length: ").Append(Length).Append("\n");
            sb.Append("  Width: ").Append(Width).Append("\n");
            sb.Append("  EnableReview: ").Append(EnableReview).Append("\n");
            sb.Append("  Inventory: ").Append(Inventory).Append("\n");
            sb.Append("  MaxNumberOfDownload: ").Append(MaxNumberOfDownload).Append("\n");
            sb.Append("  DownloadExpiration: ").Append(DownloadExpiration).Append("\n");
            sb.Append("  DownloadType: ").Append(DownloadType).Append("\n");
            sb.Append("  HasUserAgreement: ").Append(HasUserAgreement).Append("\n");
            sb.Append("  ShippingType: ").Append(ShippingType).Append("\n");
            sb.Append("  TaxType: ").Append(TaxType).Append("\n");
            sb.Append("  Vendor: ").Append(Vendor).Append("\n");
            
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
            return this.Equals(obj as VirtoCommerceMerchandisingModuleWebModelProduct);
        }

        /// <summary>
        /// Returns true if VirtoCommerceMerchandisingModuleWebModelProduct instances are equal
        /// </summary>
        /// <param name="obj">Instance of VirtoCommerceMerchandisingModuleWebModelProduct to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(VirtoCommerceMerchandisingModuleWebModelProduct other)
        {
            // credit: http://stackoverflow.com/a/10454552/677735
            if (other == null)
                return false;

            return 
                (
                    this.Variations == other.Variations ||
                    this.Variations != null &&
                    this.Variations.SequenceEqual(other.Variations)
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
                    this.Associations == other.Associations ||
                    this.Associations != null &&
                    this.Associations.SequenceEqual(other.Associations)
                ) && 
                (
                    this.CatalogId == other.CatalogId ||
                    this.CatalogId != null &&
                    this.CatalogId.Equals(other.CatalogId)
                ) && 
                (
                    this.CategoryId == other.CategoryId ||
                    this.CategoryId != null &&
                    this.CategoryId.Equals(other.CategoryId)
                ) && 
                (
                    this.Code == other.Code ||
                    this.Code != null &&
                    this.Code.Equals(other.Code)
                ) && 
                (
                    this.EditorialReviews == other.EditorialReviews ||
                    this.EditorialReviews != null &&
                    this.EditorialReviews.SequenceEqual(other.EditorialReviews)
                ) && 
                (
                    this.Id == other.Id ||
                    this.Id != null &&
                    this.Id.Equals(other.Id)
                ) && 
                (
                    this.PrimaryImage == other.PrimaryImage ||
                    this.PrimaryImage != null &&
                    this.PrimaryImage.Equals(other.PrimaryImage)
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
                    this.MainProductId == other.MainProductId ||
                    this.MainProductId != null &&
                    this.MainProductId.Equals(other.MainProductId)
                ) && 
                (
                    this.TrackInventory == other.TrackInventory ||
                    this.TrackInventory != null &&
                    this.TrackInventory.Equals(other.TrackInventory)
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
                    this.Name == other.Name ||
                    this.Name != null &&
                    this.Name.Equals(other.Name)
                ) && 
                (
                    this.Outline == other.Outline ||
                    this.Outline != null &&
                    this.Outline.Equals(other.Outline)
                ) && 
                (
                    this.Properties == other.Properties ||
                    this.Properties != null &&
                    this.Properties.SequenceEqual(other.Properties)
                ) && 
                (
                    this.VariationProperties == other.VariationProperties ||
                    this.VariationProperties != null &&
                    this.VariationProperties.SequenceEqual(other.VariationProperties)
                ) && 
                (
                    this.Rating == other.Rating ||
                    this.Rating != null &&
                    this.Rating.Equals(other.Rating)
                ) && 
                (
                    this.ReviewsTotal == other.ReviewsTotal ||
                    this.ReviewsTotal != null &&
                    this.ReviewsTotal.Equals(other.ReviewsTotal)
                ) && 
                (
                    this.Seo == other.Seo ||
                    this.Seo != null &&
                    this.Seo.SequenceEqual(other.Seo)
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
                    this.Inventory == other.Inventory ||
                    this.Inventory != null &&
                    this.Inventory.Equals(other.Inventory)
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
                
                if (this.Variations != null)
                    hash = hash * 57 + this.Variations.GetHashCode();
                
                if (this.ManufacturerPartNumber != null)
                    hash = hash * 57 + this.ManufacturerPartNumber.GetHashCode();
                
                if (this.Gtin != null)
                    hash = hash * 57 + this.Gtin.GetHashCode();
                
                if (this.Associations != null)
                    hash = hash * 57 + this.Associations.GetHashCode();
                
                if (this.CatalogId != null)
                    hash = hash * 57 + this.CatalogId.GetHashCode();
                
                if (this.CategoryId != null)
                    hash = hash * 57 + this.CategoryId.GetHashCode();
                
                if (this.Code != null)
                    hash = hash * 57 + this.Code.GetHashCode();
                
                if (this.EditorialReviews != null)
                    hash = hash * 57 + this.EditorialReviews.GetHashCode();
                
                if (this.Id != null)
                    hash = hash * 57 + this.Id.GetHashCode();
                
                if (this.PrimaryImage != null)
                    hash = hash * 57 + this.PrimaryImage.GetHashCode();
                
                if (this.Images != null)
                    hash = hash * 57 + this.Images.GetHashCode();
                
                if (this.Assets != null)
                    hash = hash * 57 + this.Assets.GetHashCode();
                
                if (this.MainProductId != null)
                    hash = hash * 57 + this.MainProductId.GetHashCode();
                
                if (this.TrackInventory != null)
                    hash = hash * 57 + this.TrackInventory.GetHashCode();
                
                if (this.IsBuyable != null)
                    hash = hash * 57 + this.IsBuyable.GetHashCode();
                
                if (this.IsActive != null)
                    hash = hash * 57 + this.IsActive.GetHashCode();
                
                if (this.MaxQuantity != null)
                    hash = hash * 57 + this.MaxQuantity.GetHashCode();
                
                if (this.MinQuantity != null)
                    hash = hash * 57 + this.MinQuantity.GetHashCode();
                
                if (this.Name != null)
                    hash = hash * 57 + this.Name.GetHashCode();
                
                if (this.Outline != null)
                    hash = hash * 57 + this.Outline.GetHashCode();
                
                if (this.Properties != null)
                    hash = hash * 57 + this.Properties.GetHashCode();
                
                if (this.VariationProperties != null)
                    hash = hash * 57 + this.VariationProperties.GetHashCode();
                
                if (this.Rating != null)
                    hash = hash * 57 + this.Rating.GetHashCode();
                
                if (this.ReviewsTotal != null)
                    hash = hash * 57 + this.ReviewsTotal.GetHashCode();
                
                if (this.Seo != null)
                    hash = hash * 57 + this.Seo.GetHashCode();
                
                if (this.StartDate != null)
                    hash = hash * 57 + this.StartDate.GetHashCode();
                
                if (this.EndDate != null)
                    hash = hash * 57 + this.EndDate.GetHashCode();
                
                if (this.ProductType != null)
                    hash = hash * 57 + this.ProductType.GetHashCode();
                
                if (this.WeightUnit != null)
                    hash = hash * 57 + this.WeightUnit.GetHashCode();
                
                if (this.Weight != null)
                    hash = hash * 57 + this.Weight.GetHashCode();
                
                if (this.MeasureUnit != null)
                    hash = hash * 57 + this.MeasureUnit.GetHashCode();
                
                if (this.Height != null)
                    hash = hash * 57 + this.Height.GetHashCode();
                
                if (this.Length != null)
                    hash = hash * 57 + this.Length.GetHashCode();
                
                if (this.Width != null)
                    hash = hash * 57 + this.Width.GetHashCode();
                
                if (this.EnableReview != null)
                    hash = hash * 57 + this.EnableReview.GetHashCode();
                
                if (this.Inventory != null)
                    hash = hash * 57 + this.Inventory.GetHashCode();
                
                if (this.MaxNumberOfDownload != null)
                    hash = hash * 57 + this.MaxNumberOfDownload.GetHashCode();
                
                if (this.DownloadExpiration != null)
                    hash = hash * 57 + this.DownloadExpiration.GetHashCode();
                
                if (this.DownloadType != null)
                    hash = hash * 57 + this.DownloadType.GetHashCode();
                
                if (this.HasUserAgreement != null)
                    hash = hash * 57 + this.HasUserAgreement.GetHashCode();
                
                if (this.ShippingType != null)
                    hash = hash * 57 + this.ShippingType.GetHashCode();
                
                if (this.TaxType != null)
                    hash = hash * 57 + this.TaxType.GetHashCode();
                
                if (this.Vendor != null)
                    hash = hash * 57 + this.Vendor.GetHashCode();
                
                return hash;
            }
        }

    }


}
