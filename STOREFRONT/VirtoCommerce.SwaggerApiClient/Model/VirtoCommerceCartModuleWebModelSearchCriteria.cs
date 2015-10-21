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
  public class VirtoCommerceCartModuleWebModelSearchCriteria {
    
    /// <summary>
    /// Gets or sets the value of search criteria keyword
    /// </summary>
    /// <value>Gets or sets the value of search criteria keyword</value>
    [DataMember(Name="keyword", EmitDefaultValue=false)]
    public string Keyword { get; set; }

    
    /// <summary>
    /// Gets or sets the value of search criteria customer id
    /// </summary>
    /// <value>Gets or sets the value of search criteria customer id</value>
    [DataMember(Name="customerId", EmitDefaultValue=false)]
    public string CustomerId { get; set; }

    
    /// <summary>
    /// Gets or sets the value of search criteria store id
    /// </summary>
    /// <value>Gets or sets the value of search criteria store id</value>
    [DataMember(Name="storeId", EmitDefaultValue=false)]
    public string StoreId { get; set; }

    
    /// <summary>
    /// Gets or sets the value of search criteria skip records count
    /// </summary>
    /// <value>Gets or sets the value of search criteria skip records count</value>
    [DataMember(Name="start", EmitDefaultValue=false)]
    public int? Start { get; set; }

    
    /// <summary>
    /// Gets or sets the value of search criteria page size
    /// </summary>
    /// <value>Gets or sets the value of search criteria page size</value>
    [DataMember(Name="count", EmitDefaultValue=false)]
    public int? Count { get; set; }

    

    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class VirtoCommerceCartModuleWebModelSearchCriteria {\n");
      
      sb.Append("  Keyword: ").Append(Keyword).Append("\n");
      
      sb.Append("  CustomerId: ").Append(CustomerId).Append("\n");
      
      sb.Append("  StoreId: ").Append(StoreId).Append("\n");
      
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
