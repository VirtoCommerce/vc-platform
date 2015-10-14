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
  public class VirtoCommerceDomainOrderModelSearchCriteria {
    
    /// <summary>
    /// Gets or Sets ResponseGroup
    /// </summary>
    [DataMember(Name="responseGroup", EmitDefaultValue=false)]
    public string ResponseGroup { get; set; }

    
    /// <summary>
    /// Gets or Sets Keyword
    /// </summary>
    [DataMember(Name="keyword", EmitDefaultValue=false)]
    public string Keyword { get; set; }

    
    /// <summary>
    /// Gets or Sets CustomerId
    /// </summary>
    [DataMember(Name="customerId", EmitDefaultValue=false)]
    public string CustomerId { get; set; }

    
    /// <summary>
    /// Gets or Sets EmployeeId
    /// </summary>
    [DataMember(Name="employeeId", EmitDefaultValue=false)]
    public string EmployeeId { get; set; }

    
    /// <summary>
    /// Gets or Sets StoreIds
    /// </summary>
    [DataMember(Name="storeIds", EmitDefaultValue=false)]
    public List<string> StoreIds { get; set; }

    
    /// <summary>
    /// Gets or Sets StartDate
    /// </summary>
    [DataMember(Name="startDate", EmitDefaultValue=false)]
    public DateTime? StartDate { get; set; }

    
    /// <summary>
    /// Gets or Sets EndDate
    /// </summary>
    [DataMember(Name="endDate", EmitDefaultValue=false)]
    public DateTime? EndDate { get; set; }

    
    /// <summary>
    /// Gets or Sets Start
    /// </summary>
    [DataMember(Name="start", EmitDefaultValue=false)]
    public int? Start { get; set; }

    
    /// <summary>
    /// Gets or Sets Count
    /// </summary>
    [DataMember(Name="count", EmitDefaultValue=false)]
    public int? Count { get; set; }

    

    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class VirtoCommerceDomainOrderModelSearchCriteria {\n");
      
      sb.Append("  ResponseGroup: ").Append(ResponseGroup).Append("\n");
      
      sb.Append("  Keyword: ").Append(Keyword).Append("\n");
      
      sb.Append("  CustomerId: ").Append(CustomerId).Append("\n");
      
      sb.Append("  EmployeeId: ").Append(EmployeeId).Append("\n");
      
      sb.Append("  StoreIds: ").Append(StoreIds).Append("\n");
      
      sb.Append("  StartDate: ").Append(StartDate).Append("\n");
      
      sb.Append("  EndDate: ").Append(EndDate).Append("\n");
      
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
