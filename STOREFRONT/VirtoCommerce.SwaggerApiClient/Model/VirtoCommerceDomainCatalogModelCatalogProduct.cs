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
  public class VirtoCommerceDomainCatalogModelCatalogProduct {
    
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
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
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
      
      sb.Append("  PropertyValues: ").Append(PropertyValues).Append("\n");
      
      sb.Append("  Images: ").Append(Images).Append("\n");
      
      sb.Append("  Assets: ").Append(Assets).Append("\n");
      
      sb.Append("  Links: ").Append(Links).Append("\n");
      
      sb.Append("  Variations: ").Append(Variations).Append("\n");
      
      sb.Append("  SeoInfos: ").Append(SeoInfos).Append("\n");
      
      sb.Append("  Reviews: ").Append(Reviews).Append("\n");
      
      sb.Append("  Associations: ").Append(Associations).Append("\n");
      
      sb.Append("  Prices: ").Append(Prices).Append("\n");
      
      sb.Append("  Inventories: ").Append(Inventories).Append("\n");
      
      sb.Append("  CreatedDate: ").Append(CreatedDate).Append("\n");
      
      sb.Append("  ModifiedDate: ").Append(ModifiedDate).Append("\n");
      
      sb.Append("  CreatedBy: ").Append(CreatedBy).Append("\n");
      
      sb.Append("  ModifiedBy: ").Append(ModifiedBy).Append("\n");
      
      sb.Append("  Id: ").Append(Id).Append("\n");
      
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
