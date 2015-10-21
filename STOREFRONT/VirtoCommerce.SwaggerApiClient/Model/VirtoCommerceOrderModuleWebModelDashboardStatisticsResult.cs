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
  public class VirtoCommerceOrderModuleWebModelDashboardStatisticsResult {
    
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
    /// Gets or Sets Revenue
    /// </summary>
    [DataMember(Name="revenue", EmitDefaultValue=false)]
    public List<VirtoCommerceOrderModuleWebModelMoney> Revenue { get; set; }

    
    /// <summary>
    /// Gets or Sets RevenuePeriodDetails
    /// </summary>
    [DataMember(Name="revenuePeriodDetails", EmitDefaultValue=false)]
    public List<VirtoCommerceOrderModuleWebModelQuarterPeriodMoney> RevenuePeriodDetails { get; set; }

    
    /// <summary>
    /// Gets or Sets OrderCount
    /// </summary>
    [DataMember(Name="orderCount", EmitDefaultValue=false)]
    public int? OrderCount { get; set; }

    
    /// <summary>
    /// Gets or Sets CustomersCount
    /// </summary>
    [DataMember(Name="customersCount", EmitDefaultValue=false)]
    public int? CustomersCount { get; set; }

    
    /// <summary>
    /// Gets or Sets RevenuePerCustomer
    /// </summary>
    [DataMember(Name="revenuePerCustomer", EmitDefaultValue=false)]
    public List<VirtoCommerceOrderModuleWebModelMoney> RevenuePerCustomer { get; set; }

    
    /// <summary>
    /// Gets or Sets AvgOrderValue
    /// </summary>
    [DataMember(Name="avgOrderValue", EmitDefaultValue=false)]
    public List<VirtoCommerceOrderModuleWebModelMoney> AvgOrderValue { get; set; }

    
    /// <summary>
    /// Gets or Sets AvgOrderValuePeriodDetails
    /// </summary>
    [DataMember(Name="avgOrderValuePeriodDetails", EmitDefaultValue=false)]
    public List<VirtoCommerceOrderModuleWebModelQuarterPeriodMoney> AvgOrderValuePeriodDetails { get; set; }

    
    /// <summary>
    /// Gets or Sets ItemsPurchased
    /// </summary>
    [DataMember(Name="itemsPurchased", EmitDefaultValue=false)]
    public int? ItemsPurchased { get; set; }

    
    /// <summary>
    /// Gets or Sets LineitemsPerOrder
    /// </summary>
    [DataMember(Name="lineitemsPerOrder", EmitDefaultValue=false)]
    public double? LineitemsPerOrder { get; set; }

    

    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class VirtoCommerceOrderModuleWebModelDashboardStatisticsResult {\n");
      
      sb.Append("  StartDate: ").Append(StartDate).Append("\n");
      
      sb.Append("  EndDate: ").Append(EndDate).Append("\n");
      
      sb.Append("  Revenue: ").Append(Revenue).Append("\n");
      
      sb.Append("  RevenuePeriodDetails: ").Append(RevenuePeriodDetails).Append("\n");
      
      sb.Append("  OrderCount: ").Append(OrderCount).Append("\n");
      
      sb.Append("  CustomersCount: ").Append(CustomersCount).Append("\n");
      
      sb.Append("  RevenuePerCustomer: ").Append(RevenuePerCustomer).Append("\n");
      
      sb.Append("  AvgOrderValue: ").Append(AvgOrderValue).Append("\n");
      
      sb.Append("  AvgOrderValuePeriodDetails: ").Append(AvgOrderValuePeriodDetails).Append("\n");
      
      sb.Append("  ItemsPurchased: ").Append(ItemsPurchased).Append("\n");
      
      sb.Append("  LineitemsPerOrder: ").Append(LineitemsPerOrder).Append("\n");
      
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
