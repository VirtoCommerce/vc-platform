using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;



namespace VirtoCommerce.SwaggerApiClient.Model {

  /// <summary>
  /// Property is metainformation record about what additional information merchandising item can be characterized. It&#39;s unheritable and can be defined in catalog, category, product or variation level.
  /// </summary>
  [DataContract]
  public class VirtoCommerceCatalogModuleWebModelProperty {
    
    /// <summary>
    /// Gets or sets a value indicating whether user can change property value.
    /// </summary>
    /// <value>Gets or sets a value indicating whether user can change property value.</value>
    [DataMember(Name="isReadOnly", EmitDefaultValue=false)]
    public bool? IsReadOnly { get; set; }

    
    /// <summary>
    /// Gets or sets a value indicating whether user can change property metadata or remove this property.
    /// </summary>
    /// <value>Gets or sets a value indicating whether user can change property metadata or remove this property.</value>
    [DataMember(Name="isManageable", EmitDefaultValue=false)]
    public bool? IsManageable { get; set; }

    
    /// <summary>
    /// Gets or sets a value indicating whether this instance is new. A new property should be created on server site instead of trying to update it.
    /// </summary>
    /// <value>Gets or sets a value indicating whether this instance is new. A new property should be created on server site instead of trying to update it.</value>
    [DataMember(Name="isNew", EmitDefaultValue=false)]
    public bool? IsNew { get; set; }

    
    /// <summary>
    /// Gets or Sets Id
    /// </summary>
    [DataMember(Name="id", EmitDefaultValue=false)]
    public string Id { get; set; }

    
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
    /// Gets or sets the name.
    /// </summary>
    /// <value>Gets or sets the name.</value>
    [DataMember(Name="name", EmitDefaultValue=false)]
    public string Name { get; set; }

    
    /// <summary>
    /// Gets or sets a value indicating whether this {VirtoCommerce.CatalogModule.Web.Model.Property} is required.
    /// </summary>
    /// <value>Gets or sets a value indicating whether this {VirtoCommerce.CatalogModule.Web.Model.Property} is required.</value>
    [DataMember(Name="required", EmitDefaultValue=false)]
    public bool? Required { get; set; }

    
    /// <summary>
    /// Gets or sets a value indicating whether this {VirtoCommerce.CatalogModule.Web.Model.Property} is dictionary.
    /// </summary>
    /// <value>Gets or sets a value indicating whether this {VirtoCommerce.CatalogModule.Web.Model.Property} is dictionary.</value>
    [DataMember(Name="dictionary", EmitDefaultValue=false)]
    public bool? Dictionary { get; set; }

    
    /// <summary>
    /// Gets or sets a value indicating whether this {VirtoCommerce.CatalogModule.Web.Model.Property} supports multiple values.
    /// </summary>
    /// <value>Gets or sets a value indicating whether this {VirtoCommerce.CatalogModule.Web.Model.Property} supports multiple values.</value>
    [DataMember(Name="multivalue", EmitDefaultValue=false)]
    public bool? Multivalue { get; set; }

    
    /// <summary>
    /// Gets or sets a value indicating whether this {VirtoCommerce.CatalogModule.Web.Model.Property} is multilingual.
    /// </summary>
    /// <value>Gets or sets a value indicating whether this {VirtoCommerce.CatalogModule.Web.Model.Property} is multilingual.</value>
    [DataMember(Name="multilanguage", EmitDefaultValue=false)]
    public bool? Multilanguage { get; set; }

    
    /// <summary>
    /// Gets or sets the type of the value.
    /// </summary>
    /// <value>Gets or sets the type of the value.</value>
    [DataMember(Name="valueType", EmitDefaultValue=false)]
    public string ValueType { get; set; }

    
    /// <summary>
    /// Gets or sets the type of object this property is applied to.
    /// </summary>
    /// <value>Gets or sets the type of object this property is applied to.</value>
    [DataMember(Name="type", EmitDefaultValue=false)]
    public string Type { get; set; }

    
    /// <summary>
    /// Gets or sets the current property value. Collection is used as a general placeholder to store both single and multi-value values.
    /// </summary>
    /// <value>Gets or sets the current property value. Collection is used as a general placeholder to store both single and multi-value values.</value>
    [DataMember(Name="values", EmitDefaultValue=false)]
    public List<VirtoCommerceCatalogModuleWebModelPropertyValue> Values { get; set; }

    
    /// <summary>
    /// Gets or sets the dictionary values.
    /// </summary>
    /// <value>Gets or sets the dictionary values.</value>
    [DataMember(Name="dictionaryValues", EmitDefaultValue=false)]
    public List<VirtoCommerceCatalogModuleWebModelPropertyDictionaryValue> DictionaryValues { get; set; }

    
    /// <summary>
    /// Gets or sets the attributes.
    /// </summary>
    /// <value>Gets or sets the attributes.</value>
    [DataMember(Name="attributes", EmitDefaultValue=false)]
    public List<VirtoCommerceCatalogModuleWebModelPropertyAttribute> Attributes { get; set; }

    
    /// <summary>
    /// Gets or sets the display names.
    /// </summary>
    /// <value>Gets or sets the display names.</value>
    [DataMember(Name="displayNames", EmitDefaultValue=false)]
    public List<VirtoCommerceDomainCatalogModelPropertyDisplayName> DisplayNames { get; set; }

    

    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class VirtoCommerceCatalogModuleWebModelProperty {\n");
      
      sb.Append("  IsReadOnly: ").Append(IsReadOnly).Append("\n");
      
      sb.Append("  IsManageable: ").Append(IsManageable).Append("\n");
      
      sb.Append("  IsNew: ").Append(IsNew).Append("\n");
      
      sb.Append("  Id: ").Append(Id).Append("\n");
      
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
      
      sb.Append("  Values: ").Append(Values).Append("\n");
      
      sb.Append("  DictionaryValues: ").Append(DictionaryValues).Append("\n");
      
      sb.Append("  Attributes: ").Append(Attributes).Append("\n");
      
      sb.Append("  DisplayNames: ").Append(DisplayNames).Append("\n");
      
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
