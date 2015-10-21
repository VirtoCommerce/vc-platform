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
  public class VirtoCommerceCartModuleWebModelCoupon {
    
    /// <summary>
    /// Gets or sets the value of coupon code
    /// </summary>
    /// <value>Gets or sets the value of coupon code</value>
    [DataMember(Name="couponCode", EmitDefaultValue=false)]
    public string CouponCode { get; set; }

    
    /// <summary>
    /// Gets or sets the value of description of invalid operation with coupon
    /// </summary>
    /// <value>Gets or sets the value of description of invalid operation with coupon</value>
    [DataMember(Name="invalidDescription", EmitDefaultValue=false)]
    public string InvalidDescription { get; set; }

    

    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class VirtoCommerceCartModuleWebModelCoupon {\n");
      
      sb.Append("  CouponCode: ").Append(CouponCode).Append("\n");
      
      sb.Append("  InvalidDescription: ").Append(InvalidDescription).Append("\n");
      
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
