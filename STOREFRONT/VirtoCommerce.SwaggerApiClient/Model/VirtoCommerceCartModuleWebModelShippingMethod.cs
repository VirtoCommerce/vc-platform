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
  public class VirtoCommerceCartModuleWebModelShippingMethod {
    
    /// <summary>
    /// Gets or sets the value of shipping method code
    /// </summary>
    /// <value>Gets or sets the value of shipping method code</value>
    [DataMember(Name="shipmentMethodCode", EmitDefaultValue=false)]
    public string ShipmentMethodCode { get; set; }

    
    /// <summary>
    /// Gets or sets the value of shipping method name
    /// </summary>
    /// <value>Gets or sets the value of shipping method name</value>
    [DataMember(Name="name", EmitDefaultValue=false)]
    public string Name { get; set; }

    
    /// <summary>
    /// Gets or sets the value of shipping method option name
    /// </summary>
    /// <value>Gets or sets the value of shipping method option name</value>
    [DataMember(Name="optionName", EmitDefaultValue=false)]
    public string OptionName { get; set; }

    
    /// <summary>
    /// Gets or sets the value of shipping method option description
    /// </summary>
    /// <value>Gets or sets the value of shipping method option description</value>
    [DataMember(Name="optionDescription", EmitDefaultValue=false)]
    public string OptionDescription { get; set; }

    
    /// <summary>
    /// Gets or sets the value of shipping method logo absolute URL
    /// </summary>
    /// <value>Gets or sets the value of shipping method logo absolute URL</value>
    [DataMember(Name="logoUrl", EmitDefaultValue=false)]
    public string LogoUrl { get; set; }

    
    /// <summary>
    /// Gets or sets the value of shipping method tax type
    /// </summary>
    /// <value>Gets or sets the value of shipping method tax type</value>
    [DataMember(Name="taxType", EmitDefaultValue=false)]
    public string TaxType { get; set; }

    
    /// <summary>
    /// Gets or sets the value of shipping method currency
    /// </summary>
    /// <value>Gets or sets the value of shipping method currency</value>
    [DataMember(Name="currency", EmitDefaultValue=false)]
    public string Currency { get; set; }

    
    /// <summary>
    /// Gets or sets the value of shipping method price
    /// </summary>
    /// <value>Gets or sets the value of shipping method price</value>
    [DataMember(Name="price", EmitDefaultValue=false)]
    public double? Price { get; set; }

    
    /// <summary>
    /// Gets or sets the collection of shipping method discounts
    /// </summary>
    /// <value>Gets or sets the collection of shipping method discounts</value>
    [DataMember(Name="discounts", EmitDefaultValue=false)]
    public List<VirtoCommerceCartModuleWebModelDiscount> Discounts { get; set; }

    

    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class VirtoCommerceCartModuleWebModelShippingMethod {\n");
      
      sb.Append("  ShipmentMethodCode: ").Append(ShipmentMethodCode).Append("\n");
      
      sb.Append("  Name: ").Append(Name).Append("\n");
      
      sb.Append("  OptionName: ").Append(OptionName).Append("\n");
      
      sb.Append("  OptionDescription: ").Append(OptionDescription).Append("\n");
      
      sb.Append("  LogoUrl: ").Append(LogoUrl).Append("\n");
      
      sb.Append("  TaxType: ").Append(TaxType).Append("\n");
      
      sb.Append("  Currency: ").Append(Currency).Append("\n");
      
      sb.Append("  Price: ").Append(Price).Append("\n");
      
      sb.Append("  Discounts: ").Append(Discounts).Append("\n");
      
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
