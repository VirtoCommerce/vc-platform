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
  public class VirtoCommerceDomainCatalogModelProperty {
    
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
    /// Gets or Sets Name
    /// </summary>
    [DataMember(Name="name", EmitDefaultValue=false)]
    public string Name { get; set; }

    
    /// <summary>
    /// Gets or Sets Required
    /// </summary>
    [DataMember(Name="required", EmitDefaultValue=false)]
    public bool? Required { get; set; }

    
    /// <summary>
    /// Gets or Sets Dictionary
    /// </summary>
    [DataMember(Name="dictionary", EmitDefaultValue=false)]
    public bool? Dictionary { get; set; }

    
    /// <summary>
    /// Gets or Sets Multivalue
    /// </summary>
    [DataMember(Name="multivalue", EmitDefaultValue=false)]
    public bool? Multivalue { get; set; }

    
    /// <summary>
    /// Gets or Sets Multilanguage
    /// </summary>
    [DataMember(Name="multilanguage", EmitDefaultValue=false)]
    public bool? Multilanguage { get; set; }

    
    /// <summary>
    /// Gets or Sets ValueType
    /// </summary>
    [DataMember(Name="valueType", EmitDefaultValue=false)]
    public string ValueType { get; set; }

    
    /// <summary>
    /// Gets or Sets Type
    /// </summary>
    [DataMember(Name="type", EmitDefaultValue=false)]
    public string Type { get; set; }

    
    /// <summary>
    /// Gets or Sets Attributes
    /// </summary>
    [DataMember(Name="attributes", EmitDefaultValue=false)]
    public List<VirtoCommerceDomainCatalogModelPropertyAttribute> Attributes { get; set; }

    
    /// <summary>
    /// Gets or Sets DictionaryValues
    /// </summary>
    [DataMember(Name="dictionaryValues", EmitDefaultValue=false)]
    public List<VirtoCommerceDomainCatalogModelPropertyDictionaryValue> DictionaryValues { get; set; }

    
    /// <summary>
    /// Gets or Sets DisplayNames
    /// </summary>
    [DataMember(Name="displayNames", EmitDefaultValue=false)]
    public List<VirtoCommerceDomainCatalogModelPropertyDisplayName> DisplayNames { get; set; }

    
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
      sb.Append("class VirtoCommerceDomainCatalogModelProperty {\n");
      
      sb.Append("  CatalogId: ").Append(CatalogId).Append("\n");
      
      sb.Append("  Catalog: ").Append(Catalog).Append("\n");
      
      sb.Append("  CategoryId: ").Append(CategoryId).Append("\n");
      
      sb.Append("  Category: ").Append(Category).Append("\n");
      
      sb.Append("  Name: ").Append(Name).Append("\n");
      
      sb.Append("  Required: ").Append(Required).Append("\n");
      
      sb.Append("  Dictionary: ").Append(Dictionary).Append("\n");
      
      sb.Append("  Multivalue: ").Append(Multivalue).Append("\n");
      
      sb.Append("  Multilanguage: ").Append(Multilanguage).Append("\n");
      
      sb.Append("  ValueType: ").Append(ValueType).Append("\n");
      
      sb.Append("  Type: ").Append(Type).Append("\n");
      
      sb.Append("  Attributes: ").Append(Attributes).Append("\n");
      
      sb.Append("  DictionaryValues: ").Append(DictionaryValues).Append("\n");
      
      sb.Append("  DisplayNames: ").Append(DisplayNames).Append("\n");
      
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
