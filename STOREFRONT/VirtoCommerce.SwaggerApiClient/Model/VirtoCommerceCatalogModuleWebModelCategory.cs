using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;



namespace VirtoCommerce.SwaggerApiClient.Model {

  /// <summary>
  /// Merchandising Category
  /// </summary>
  [DataContract]
  public class VirtoCommerceCatalogModuleWebModelCategory {
    
    /// <summary>
    /// Gets or sets the parent category id.
    /// </summary>
    /// <value>Gets or sets the parent category id.</value>
    [DataMember(Name="parentId", EmitDefaultValue=false)]
    public string ParentId { get; set; }

    
    /// <summary>
    /// Gets or Sets Id
    /// </summary>
    [DataMember(Name="id", EmitDefaultValue=false)]
    public string Id { get; set; }

    
    /// <summary>
    /// Gets or sets a value indicating whether this {VirtoCommerce.CatalogModule.Web.Model.Category} is virtual or common.
    /// </summary>
    /// <value>Gets or sets a value indicating whether this {VirtoCommerce.CatalogModule.Web.Model.Category} is virtual or common.</value>
    [DataMember(Name="virtual", EmitDefaultValue=false)]
    public bool? Virtual { get; set; }

    
    /// <summary>
    /// Gets or sets the code.
    /// </summary>
    /// <value>Gets or sets the code.</value>
    [DataMember(Name="code", EmitDefaultValue=false)]
    public string Code { get; set; }

    
    /// <summary>
    /// Gets or sets the type of the tax.
    /// </summary>
    /// <value>Gets or sets the type of the tax.</value>
    [DataMember(Name="taxType", EmitDefaultValue=false)]
    public string TaxType { get; set; }

    
    /// <summary>
    /// Gets or sets the catalog that this category belongs to.
    /// </summary>
    /// <value>Gets or sets the catalog that this category belongs to.</value>
    [DataMember(Name="catalog", EmitDefaultValue=false)]
    public VirtoCommerceCatalogModuleWebModelCatalog Catalog { get; set; }

    
    /// <summary>
    /// Gets or sets the catalog id that this category belongs to.
    /// </summary>
    /// <value>Gets or sets the catalog id that this category belongs to.</value>
    [DataMember(Name="catalogId", EmitDefaultValue=false)]
    public string CatalogId { get; set; }

    
    /// <summary>
    /// Gets or sets the name.
    /// </summary>
    /// <value>Gets or sets the name.</value>
    [DataMember(Name="name", EmitDefaultValue=false)]
    public string Name { get; set; }

    
    /// <summary>
    /// Gets or Sets Path
    /// </summary>
    [DataMember(Name="path", EmitDefaultValue=false)]
    public string Path { get; set; }

    
    /// <summary>
    /// Gets or sets a value indicating whether this {VirtoCommerce.CatalogModule.Web.Model.Category} is active.
    /// </summary>
    /// <value>Gets or sets a value indicating whether this {VirtoCommerce.CatalogModule.Web.Model.Category} is active.</value>
    [DataMember(Name="isActive", EmitDefaultValue=false)]
    public bool? IsActive { get; set; }

    
    /// <summary>
    /// Gets or Sets Parents
    /// </summary>
    [DataMember(Name="parents", EmitDefaultValue=false)]
    public Dictionary<string, string> Parents { get; set; }

    
    /// <summary>
    /// Gets or sets the children categories.
    /// </summary>
    /// <value>Gets or sets the children categories.</value>
    [DataMember(Name="children", EmitDefaultValue=false)]
    public List<VirtoCommerceCatalogModuleWebModelCategory> Children { get; set; }

    
    /// <summary>
    /// Gets or sets the properties.
    /// </summary>
    /// <value>Gets or sets the properties.</value>
    [DataMember(Name="properties", EmitDefaultValue=false)]
    public List<VirtoCommerceCatalogModuleWebModelProperty> Properties { get; set; }

    
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
    /// Gets or sets the images.
    /// </summary>
    /// <value>Gets or sets the images.</value>
    [DataMember(Name="images", EmitDefaultValue=false)]
    public List<VirtoCommerceCatalogModuleWebModelImage> Images { get; set; }

    
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
      sb.Append("class VirtoCommerceCatalogModuleWebModelCategory {\n");
      
      sb.Append("  ParentId: ").Append(ParentId).Append("\n");
      
      sb.Append("  Id: ").Append(Id).Append("\n");
      
      sb.Append("  Virtual: ").Append(Virtual).Append("\n");
      
      sb.Append("  Code: ").Append(Code).Append("\n");
      
      sb.Append("  TaxType: ").Append(TaxType).Append("\n");
      
      sb.Append("  Catalog: ").Append(Catalog).Append("\n");
      
      sb.Append("  CatalogId: ").Append(CatalogId).Append("\n");
      
      sb.Append("  Name: ").Append(Name).Append("\n");
      
      sb.Append("  Path: ").Append(Path).Append("\n");
      
      sb.Append("  IsActive: ").Append(IsActive).Append("\n");
      
      sb.Append("  Parents: ").Append(Parents).Append("\n");
      
      sb.Append("  Children: ").Append(Children).Append("\n");
      
      sb.Append("  Properties: ").Append(Properties).Append("\n");
      
      sb.Append("  Links: ").Append(Links).Append("\n");
      
      sb.Append("  SeoInfos: ").Append(SeoInfos).Append("\n");
      
      sb.Append("  Images: ").Append(Images).Append("\n");
      
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
