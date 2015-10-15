using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;



namespace VirtoCommerce.SwaggerApiClient.Model {

  /// <summary>
  /// Information to define linking information from item or category to category.
  /// </summary>
  [DataContract]
  public class VirtoCommerceCatalogModuleWebModelListEntryLink {
    
    /// <summary>
    /// Gets or sets the list entry identifier.
    /// </summary>
    /// <value>Gets or sets the list entry identifier.</value>
    [DataMember(Name="listEntryId", EmitDefaultValue=false)]
    public string ListEntryId { get; set; }

    
    /// <summary>
    /// Gets or sets the type of the list entry. E.g. \"product\", \"category\"
    /// </summary>
    /// <value>Gets or sets the type of the list entry. E.g. \"product\", \"category\"</value>
    [DataMember(Name="listEntryType", EmitDefaultValue=false)]
    public string ListEntryType { get; set; }

    
    /// <summary>
    /// Gets or sets the target catalog identifier.
    /// </summary>
    /// <value>Gets or sets the target catalog identifier.</value>
    [DataMember(Name="catalogId", EmitDefaultValue=false)]
    public string CatalogId { get; set; }

    
    /// <summary>
    /// Gets or sets the target category identifier.
    /// </summary>
    /// <value>Gets or sets the target category identifier.</value>
    [DataMember(Name="categoryId", EmitDefaultValue=false)]
    public string CategoryId { get; set; }

    

    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class VirtoCommerceCatalogModuleWebModelListEntryLink {\n");
      
      sb.Append("  ListEntryId: ").Append(ListEntryId).Append("\n");
      
      sb.Append("  ListEntryType: ").Append(ListEntryType).Append("\n");
      
      sb.Append("  CatalogId: ").Append(CatalogId).Append("\n");
      
      sb.Append("  CategoryId: ").Append(CategoryId).Append("\n");
      
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
