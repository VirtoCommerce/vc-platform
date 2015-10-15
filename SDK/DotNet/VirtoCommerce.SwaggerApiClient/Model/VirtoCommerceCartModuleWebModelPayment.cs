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
  public class VirtoCommerceCartModuleWebModelPayment {
    
    /// <summary>
    /// Gets or sets the value of payment outer id
    /// </summary>
    /// <value>Gets or sets the value of payment outer id</value>
    [DataMember(Name="outerId", EmitDefaultValue=false)]
    public string OuterId { get; set; }

    
    /// <summary>
    /// Gets or sets the value of payment gateway code
    /// </summary>
    /// <value>Gets or sets the value of payment gateway code</value>
    [DataMember(Name="paymentGatewayCode", EmitDefaultValue=false)]
    public string PaymentGatewayCode { get; set; }

    
    /// <summary>
    /// Gets or sets the value of payment currency
    /// </summary>
    /// <value>Gets or sets the value of payment currency</value>
    [DataMember(Name="currency", EmitDefaultValue=false)]
    public string Currency { get; set; }

    
    /// <summary>
    /// Gets or sets the value of payment amount
    /// </summary>
    /// <value>Gets or sets the value of payment amount</value>
    [DataMember(Name="amount", EmitDefaultValue=false)]
    public double? Amount { get; set; }

    
    /// <summary>
    /// Gets or sets the billing address
    /// </summary>
    /// <value>Gets or sets the billing address</value>
    [DataMember(Name="billingAddress", EmitDefaultValue=false)]
    public VirtoCommerceCartModuleWebModelAddress BillingAddress { get; set; }

    
    /// <summary>
    /// Gets or Sets Id
    /// </summary>
    [DataMember(Name="id", EmitDefaultValue=false)]
    public string Id { get; set; }

    

    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class VirtoCommerceCartModuleWebModelPayment {\n");
      
      sb.Append("  OuterId: ").Append(OuterId).Append("\n");
      
      sb.Append("  PaymentGatewayCode: ").Append(PaymentGatewayCode).Append("\n");
      
      sb.Append("  Currency: ").Append(Currency).Append("\n");
      
      sb.Append("  Amount: ").Append(Amount).Append("\n");
      
      sb.Append("  BillingAddress: ").Append(BillingAddress).Append("\n");
      
      sb.Append("  Id: ").Append(Id).Append("\n");
      
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
