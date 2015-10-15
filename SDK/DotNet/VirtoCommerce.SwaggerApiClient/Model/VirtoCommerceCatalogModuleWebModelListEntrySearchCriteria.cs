using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;



namespace VirtoCommerce.SwaggerApiClient.Model {

  /// <summary>
  /// Search criteria for categories and/or items.
  /// </summary>
  [DataContract]
  public class VirtoCommerceCatalogModuleWebModelListEntrySearchCriteria {
    
    /// <summary>
    /// Gets or sets the response group to define which types of entries to search for.
    /// </summary>
    /// <value>Gets or sets the response group to define which types of entries to search for.</value>
    [DataMember(Name="responseGroup", EmitDefaultValue=false)]
    public string ResponseGroup { get; set; }

    
    /// <summary>
    /// Gets or sets the keyword to search for.
    /// </summary>
    /// <value>Gets or sets the keyword to search for.</value>
    [DataMember(Name="keyword", EmitDefaultValue=false)]
    public string Keyword { get; set; }

    
    /// <summary>
    /// Gets or sets the category identifier.
    /// </summary>
    /// <value>Gets or sets the category identifier.</value>
    [DataMember(Name="categoryId", EmitDefaultValue=false)]
    public string CategoryId { get; set; }

    
    /// <summary>
    /// Gets or sets the catalog identifier.
    /// </summary>
    /// <value>Gets or sets the catalog identifier.</value>
    [DataMember(Name="catalogId", EmitDefaultValue=false)]
    public string CatalogId { get; set; }

    
    /// <summary>
    /// Gets or sets the start index of total results from which entries should be returned.
    /// </summary>
    /// <value>Gets or sets the start index of total results from which entries should be returned.</value>
    [DataMember(Name="start", EmitDefaultValue=false)]
    public int? Start { get; set; }

    
    /// <summary>
    /// Gets or sets the maximum count of results to return.
    /// </summary>
    /// <value>Gets or sets the maximum count of results to return.</value>
    [DataMember(Name="count", EmitDefaultValue=false)]
    public int? Count { get; set; }

    

    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class VirtoCommerceCatalogModuleWebModelListEntrySearchCriteria {\n");
      
      sb.Append("  ResponseGroup: ").Append(ResponseGroup).Append("\n");
      
      sb.Append("  Keyword: ").Append(Keyword).Append("\n");
      
      sb.Append("  CategoryId: ").Append(CategoryId).Append("\n");
      
      sb.Append("  CatalogId: ").Append(CatalogId).Append("\n");
      
      sb.Append("  Start: ").Append(Start).Append("\n");
      
      sb.Append("  Count: ").Append(Count).Append("\n");
      
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
