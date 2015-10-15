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
  public class VirtoCommerceContentWebModelsGetPagesCriteria {
    
    /// <summary>
    /// Max value of last updated date, if it's null returns all pages for store
    /// </summary>
    /// <value>Max value of last updated date, if it's null returns all pages for store</value>
    [DataMember(Name="lastUpdateDate", EmitDefaultValue=false)]
    public DateTime? LastUpdateDate { get; set; }

    

    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class VirtoCommerceContentWebModelsGetPagesCriteria {\n");
      
      sb.Append("  LastUpdateDate: ").Append(LastUpdateDate).Append("\n");
      
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
