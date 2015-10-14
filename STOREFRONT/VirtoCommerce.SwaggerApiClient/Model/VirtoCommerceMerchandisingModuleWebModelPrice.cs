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
  public class VirtoCommerceMerchandisingModuleWebModelPrice {
    
    /// <summary>
    /// Gets or sets the value of original price
    /// </summary>
    /// <value>Gets or sets the value of original price</value>
    [DataMember(Name="list", EmitDefaultValue=false)]
    public double? List { get; set; }

    
    /// <summary>
    /// Gets or sets the value of minimum catalog item quantity for current price
    /// </summary>
    /// <value>Gets or sets the value of minimum catalog item quantity for current price</value>
    [DataMember(Name="minQuantity", EmitDefaultValue=false)]
    public int? MinQuantity { get; set; }

    
    /// <summary>
    /// Gets or sets the value of pricelist id
    /// </summary>
    /// <value>Gets or sets the value of pricelist id</value>
    [DataMember(Name="pricelistId", EmitDefaultValue=false)]
    public string PricelistId { get; set; }

    
    /// <summary>
    /// Gets or sets the value of catalog item id
    /// </summary>
    /// <value>Gets or sets the value of catalog item id</value>
    [DataMember(Name="productId", EmitDefaultValue=false)]
    public string ProductId { get; set; }

    
    /// <summary>
    /// Gets or sets the value for sale price (include static discount amount)
    /// </summary>
    /// <value>Gets or sets the value for sale price (include static discount amount)</value>
    [DataMember(Name="sale", EmitDefaultValue=false)]
    public double? Sale { get; set; }

    
    /// <summary>
    /// Gets or sets the value of price currency
    /// </summary>
    /// <value>Gets or sets the value of price currency</value>
    [DataMember(Name="currency", EmitDefaultValue=false)]
    public string Currency { get; set; }

    

    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class VirtoCommerceMerchandisingModuleWebModelPrice {\n");
      
      sb.Append("  List: ").Append(List).Append("\n");
      
      sb.Append("  MinQuantity: ").Append(MinQuantity).Append("\n");
      
      sb.Append("  PricelistId: ").Append(PricelistId).Append("\n");
      
      sb.Append("  ProductId: ").Append(ProductId).Append("\n");
      
      sb.Append("  Sale: ").Append(Sale).Append("\n");
      
      sb.Append("  Currency: ").Append(Currency).Append("\n");
      
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
