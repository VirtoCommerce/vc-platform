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
  public class VirtoCommerceQuoteModuleWebModelShipmentMethod {
    
    /// <summary>
    /// Gets or Sets ShipmentMethodCode
    /// </summary>
    [DataMember(Name="shipmentMethodCode", EmitDefaultValue=false)]
    public string ShipmentMethodCode { get; set; }

    
    /// <summary>
    /// Gets or Sets OptionName
    /// </summary>
    [DataMember(Name="optionName", EmitDefaultValue=false)]
    public string OptionName { get; set; }

    
    /// <summary>
    /// Gets or Sets Name
    /// </summary>
    [DataMember(Name="name", EmitDefaultValue=false)]
    public string Name { get; set; }

    
    /// <summary>
    /// Gets or Sets LogoUrl
    /// </summary>
    [DataMember(Name="logoUrl", EmitDefaultValue=false)]
    public string LogoUrl { get; set; }

    
    /// <summary>
    /// Gets or Sets Currency
    /// </summary>
    [DataMember(Name="currency", EmitDefaultValue=false)]
    public string Currency { get; set; }

    
    /// <summary>
    /// Gets or Sets Price
    /// </summary>
    [DataMember(Name="price", EmitDefaultValue=false)]
    public double? Price { get; set; }

    

    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class VirtoCommerceQuoteModuleWebModelShipmentMethod {\n");
      
      sb.Append("  ShipmentMethodCode: ").Append(ShipmentMethodCode).Append("\n");
      
      sb.Append("  OptionName: ").Append(OptionName).Append("\n");
      
      sb.Append("  Name: ").Append(Name).Append("\n");
      
      sb.Append("  LogoUrl: ").Append(LogoUrl).Append("\n");
      
      sb.Append("  Currency: ").Append(Currency).Append("\n");
      
      sb.Append("  Price: ").Append(Price).Append("\n");
      
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
