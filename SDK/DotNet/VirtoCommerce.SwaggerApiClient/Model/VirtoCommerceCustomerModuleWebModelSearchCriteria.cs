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
  public class VirtoCommerceCustomerModuleWebModelSearchCriteria {
    
    /// <summary>
    /// Word, part of word or phrase to search
    /// </summary>
    /// <value>Word, part of word or phrase to search</value>
    [DataMember(Name="keyword", EmitDefaultValue=false)]
    public string Keyword { get; set; }

    
    /// <summary>
    /// It used to limit search within an organization
    /// </summary>
    /// <value>It used to limit search within an organization</value>
    [DataMember(Name="organizationId", EmitDefaultValue=false)]
    public string OrganizationId { get; set; }

    
    /// <summary>
    /// It used to skip some first search results
    /// </summary>
    /// <value>It used to skip some first search results</value>
    [DataMember(Name="start", EmitDefaultValue=false)]
    public int? Start { get; set; }

    
    /// <summary>
    /// It used to limit the number of search results
    /// </summary>
    /// <value>It used to limit the number of search results</value>
    [DataMember(Name="count", EmitDefaultValue=false)]
    public int? Count { get; set; }

    

    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class VirtoCommerceCustomerModuleWebModelSearchCriteria {\n");
      
      sb.Append("  Keyword: ").Append(Keyword).Append("\n");
      
      sb.Append("  OrganizationId: ").Append(OrganizationId).Append("\n");
      
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
