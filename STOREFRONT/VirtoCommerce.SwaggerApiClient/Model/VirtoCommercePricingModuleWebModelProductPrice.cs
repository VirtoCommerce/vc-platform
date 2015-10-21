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
  public class VirtoCommercePricingModuleWebModelProductPrice {
    
    /// <summary>
    /// Gets or Sets ProductId
    /// </summary>
    [DataMember(Name="productId", EmitDefaultValue=false)]
    public string ProductId { get; set; }

    
    /// <summary>
    /// Gets or Sets ProductName
    /// </summary>
    [DataMember(Name="productName", EmitDefaultValue=false)]
    public string ProductName { get; set; }

    
    /// <summary>
    /// List prices for the products. It includes tiered prices also. (Depending on the quantity, for example)
    /// </summary>
    /// <value>List prices for the products. It includes tiered prices also. (Depending on the quantity, for example)</value>
    [DataMember(Name="prices", EmitDefaultValue=false)]
    public List<VirtoCommercePricingModuleWebModelPrice> Prices { get; set; }

    

    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class VirtoCommercePricingModuleWebModelProductPrice {\n");
      
      sb.Append("  ProductId: ").Append(ProductId).Append("\n");
      
      sb.Append("  ProductName: ").Append(ProductName).Append("\n");
      
      sb.Append("  Prices: ").Append(Prices).Append("\n");
      
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
