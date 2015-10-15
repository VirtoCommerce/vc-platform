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
  public class VirtoCommerceDomainPaymentModelBankCardInfo {
    
    /// <summary>
    /// Gets or Sets BankCardNumber
    /// </summary>
    [DataMember(Name="bankCardNumber", EmitDefaultValue=false)]
    public string BankCardNumber { get; set; }

    
    /// <summary>
    /// Gets or Sets BankCardType
    /// </summary>
    [DataMember(Name="bankCardType", EmitDefaultValue=false)]
    public string BankCardType { get; set; }

    
    /// <summary>
    /// Gets or Sets BankCardMonth
    /// </summary>
    [DataMember(Name="bankCardMonth", EmitDefaultValue=false)]
    public int? BankCardMonth { get; set; }

    
    /// <summary>
    /// Gets or Sets BankCardYear
    /// </summary>
    [DataMember(Name="bankCardYear", EmitDefaultValue=false)]
    public int? BankCardYear { get; set; }

    
    /// <summary>
    /// Gets or Sets BankCardCVV2
    /// </summary>
    [DataMember(Name="bankCardCVV2", EmitDefaultValue=false)]
    public string BankCardCVV2 { get; set; }

    

    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class VirtoCommerceDomainPaymentModelBankCardInfo {\n");
      
      sb.Append("  BankCardNumber: ").Append(BankCardNumber).Append("\n");
      
      sb.Append("  BankCardType: ").Append(BankCardType).Append("\n");
      
      sb.Append("  BankCardMonth: ").Append(BankCardMonth).Append("\n");
      
      sb.Append("  BankCardYear: ").Append(BankCardYear).Append("\n");
      
      sb.Append("  BankCardCVV2: ").Append(BankCardCVV2).Append("\n");
      
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
