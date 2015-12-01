using System;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;



namespace VirtoCommerce.Client.Model {

  /// <summary>
  /// Represent process payment request result
  /// </summary>
  [DataContract]
  public class VirtoCommerceOrderModuleWebModelProcessPaymentResult {
    
    /// <summary>
    /// Gets or Sets NewPaymentStatus
    /// </summary>
    [DataMember(Name="newPaymentStatus", EmitDefaultValue=false)]
    public string NewPaymentStatus { get; set; }

    
    /// <summary>
    /// Gets or Sets PaymentMethodType
    /// </summary>
    [DataMember(Name="paymentMethodType", EmitDefaultValue=false)]
    public string PaymentMethodType { get; set; }

    
    /// <summary>
    /// Redirect url used for OutSite payment processing
    /// </summary>
    /// <value>Redirect url used for OutSite payment processing</value>
    [DataMember(Name="redirectUrl", EmitDefaultValue=false)]
    public string RedirectUrl { get; set; }

    
    /// <summary>
    /// Gets or Sets IsSuccess
    /// </summary>
    [DataMember(Name="isSuccess", EmitDefaultValue=false)]
    public bool? IsSuccess { get; set; }

    
    /// <summary>
    /// Gets or Sets Error
    /// </summary>
    [DataMember(Name="error", EmitDefaultValue=false)]
    public string Error { get; set; }

    
    /// <summary>
    /// Generated Html form used for InSite payment processing
    /// </summary>
    /// <value>Generated Html form used for InSite payment processing</value>
    [DataMember(Name="htmlForm", EmitDefaultValue=false)]
    public string HtmlForm { get; set; }

    
    /// <summary>
    /// Gets or Sets OuterId
    /// </summary>
    [DataMember(Name="outerId", EmitDefaultValue=false)]
    public string OuterId { get; set; }

    

    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class VirtoCommerceOrderModuleWebModelProcessPaymentResult {\n");
      
      sb.Append("  NewPaymentStatus: ").Append(NewPaymentStatus).Append("\n");
      
      sb.Append("  PaymentMethodType: ").Append(PaymentMethodType).Append("\n");
      
      sb.Append("  RedirectUrl: ").Append(RedirectUrl).Append("\n");
      
      sb.Append("  IsSuccess: ").Append(IsSuccess).Append("\n");
      
      sb.Append("  Error: ").Append(Error).Append("\n");
      
      sb.Append("  HtmlForm: ").Append(HtmlForm).Append("\n");
      
      sb.Append("  OuterId: ").Append(OuterId).Append("\n");
      
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
