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
  public class VirtoCommerceDomainQuoteModelQuoteRequestSearchResult {
    
    /// <summary>
    /// Gets or Sets TotalCount
    /// </summary>
    [DataMember(Name="totalCount", EmitDefaultValue=false)]
    public int? TotalCount { get; set; }

    
    /// <summary>
    /// Gets or Sets QuoteRequests
    /// </summary>
    [DataMember(Name="quoteRequests", EmitDefaultValue=false)]
    public List<VirtoCommerceDomainQuoteModelQuoteRequest> QuoteRequests { get; set; }

    

    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class VirtoCommerceDomainQuoteModelQuoteRequestSearchResult {\n");
      
      sb.Append("  TotalCount: ").Append(TotalCount).Append("\n");
      
      sb.Append("  QuoteRequests: ").Append(QuoteRequests).Append("\n");
      
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
