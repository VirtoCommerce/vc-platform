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
  public class VirtoCommerceCustomerModuleWebModelSearchResult {
    
    /// <summary>
    /// Total count of objects satisfied Search Criteria
    /// </summary>
    /// <value>Total count of objects satisfied Search Criteria</value>
    [DataMember(Name="totalCount", EmitDefaultValue=false)]
    public int? TotalCount { get; set; }

    
    /// <summary>
    /// Part of objects satisfied Search Criteria. See Skip and Count parameters of Search Criteria
    /// </summary>
    /// <value>Part of objects satisfied Search Criteria. See Skip and Count parameters of Search Criteria</value>
    [DataMember(Name="members", EmitDefaultValue=false)]
    public List<VirtoCommerceCustomerModuleWebModelMember> Members { get; set; }

    

    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class VirtoCommerceCustomerModuleWebModelSearchResult {\n");
      
      sb.Append("  TotalCount: ").Append(TotalCount).Append("\n");
      
      sb.Append("  Members: ").Append(Members).Append("\n");
      
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
