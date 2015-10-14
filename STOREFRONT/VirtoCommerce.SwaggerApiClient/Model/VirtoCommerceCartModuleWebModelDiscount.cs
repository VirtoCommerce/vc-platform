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
  public class VirtoCommerceCartModuleWebModelDiscount {
    
    /// <summary>
    /// Gets or sets the value of promotion id
    /// </summary>
    /// <value>Gets or sets the value of promotion id</value>
    [DataMember(Name="promotionId", EmitDefaultValue=false)]
    public string PromotionId { get; set; }

    
    /// <summary>
    /// Gets or sets the value of currency
    /// </summary>
    /// <value>Gets or sets the value of currency</value>
    [DataMember(Name="currency", EmitDefaultValue=false)]
    public string Currency { get; set; }

    
    /// <summary>
    /// Gets or sets the value of discount amount
    /// </summary>
    /// <value>Gets or sets the value of discount amount</value>
    [DataMember(Name="discountAmount", EmitDefaultValue=false)]
    public double? DiscountAmount { get; set; }

    
    /// <summary>
    /// Gets or sets the value of discount description
    /// </summary>
    /// <value>Gets or sets the value of discount description</value>
    [DataMember(Name="description", EmitDefaultValue=false)]
    public string Description { get; set; }

    

    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class VirtoCommerceCartModuleWebModelDiscount {\n");
      
      sb.Append("  PromotionId: ").Append(PromotionId).Append("\n");
      
      sb.Append("  Currency: ").Append(Currency).Append("\n");
      
      sb.Append("  DiscountAmount: ").Append(DiscountAmount).Append("\n");
      
      sb.Append("  Description: ").Append(Description).Append("\n");
      
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
