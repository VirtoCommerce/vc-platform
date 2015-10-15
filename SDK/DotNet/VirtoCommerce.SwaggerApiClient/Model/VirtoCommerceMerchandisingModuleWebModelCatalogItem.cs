using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;



namespace VirtoCommerce.SwaggerApiClient.Model {

  /// <summary>
  /// 
  /// </summary>
  [DataContract]
  public class VirtoCommerceMerchandisingModuleWebModelCatalogItem {
    
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
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class VirtoCommerceMerchandisingModuleWebModelCatalogItem {\n");
      
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
    /// Get the JSON string presentation of the object
    /// </summary>
    /// <returns>JSON string presentation of the object</returns>
    public string ToJson() {
      return JsonConvert.SerializeObject(this, Formatting.Indented);
    }

}


}
