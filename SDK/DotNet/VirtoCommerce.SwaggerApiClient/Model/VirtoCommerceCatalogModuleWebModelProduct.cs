using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;



namespace VirtoCommerce.SwaggerApiClient.Model {

  /// <summary>
  /// Merchandising item.
  /// </summary>
  [DataContract]
  public class VirtoCommerceCatalogModuleWebModelProduct {
    
    /// <summary>
    /// Gets or Sets Id
    /// </summary>
    [DataMember(Name="id", EmitDefaultValue=false)]
    public string Id { get; set; }

    
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
    public string ImgSrc { get; set; }

    
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
    /// Gets or sets the list of SEO information records.
    /// </summary>
    /// <value>Gets or sets the list of SEO information records.</value>
    [DataMember(Name="seoInfos", EmitDefaultValue=false)]
    public List<VirtoCommerceDomainCommerceModelSeoInfo> SeoInfos { get; set; }

    
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
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class VirtoCommerceCatalogModuleWebModelProduct {\n");
      
      sb.Append("  Id: ").Append(Id).Append("\n");
      
      sb.Append("  ManufacturerPartNumber: ").Append(ManufacturerPartNumber).Append("\n");
      
      sb.Append("  Gtin: ").Append(Gtin).Append("\n");
      
      sb.Append("  Code: ").Append(Code).Append("\n");
      
      sb.Append("  Name: ").Append(Name).Append("\n");
      
      sb.Append("  CatalogId: ").Append(CatalogId).Append("\n");
      
      sb.Append("  Catalog: ").Append(Catalog).Append("\n");
      
      sb.Append("  CategoryId: ").Append(CategoryId).Append("\n");
      
      sb.Append("  Category: ").Append(Category).Append("\n");
      
      sb.Append("  Outline: ").Append(Outline).Append("\n");
      
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
      
      sb.Append("  SeoInfos: ").Append(SeoInfos).Append("\n");
      
      sb.Append("  Reviews: ").Append(Reviews).Append("\n");
      
      sb.Append("  Associations: ").Append(Associations).Append("\n");
      
      sb.Append("  SecurityScopes: ").Append(SecurityScopes).Append("\n");
      
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
