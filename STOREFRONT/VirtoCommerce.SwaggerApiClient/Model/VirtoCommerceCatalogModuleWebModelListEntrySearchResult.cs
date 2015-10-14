using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;



namespace VirtoCommerce.SwaggerApiClient.Model {

  /// <summary>
  /// Class representing the result of ListEntries search.
  /// </summary>
  [DataContract]
  public class VirtoCommerceCatalogModuleWebModelListEntrySearchResult {
    
    /// <summary>
    /// Gets or sets the total entries count matching the search criteria.
    /// </summary>
    /// <value>Gets or sets the total entries count matching the search criteria.</value>
    [DataMember(Name="totalCount", EmitDefaultValue=false)]
    public int? TotalCount { get; set; }

    
    /// <summary>
    /// Gets or sets the list entries.
    /// </summary>
    /// <value>Gets or sets the list entries.</value>
    [DataMember(Name="listEntries", EmitDefaultValue=false)]
    public List<VirtoCommerceCatalogModuleWebModelListEntry> ListEntries { get; set; }

    

    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class VirtoCommerceCatalogModuleWebModelListEntrySearchResult {\n");
      
      sb.Append("  TotalCount: ").Append(TotalCount).Append("\n");
      
      sb.Append("  ListEntries: ").Append(ListEntries).Append("\n");
      
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
