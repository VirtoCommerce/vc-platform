using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;



namespace VirtoCommerce.SwaggerApiClient.Model {

  /// <summary>
  /// Represents a price of a Product in depends on batch quantity.
  /// </summary>
  [DataContract]
  public class VirtoCommercePricingModuleWebModelPrice {
    
    /// <summary>
    /// Gets or Sets PricelistId
    /// </summary>
    [DataMember(Name="pricelistId", EmitDefaultValue=false)]
    public string PricelistId { get; set; }

    
    /// <summary>
    /// Gets or Sets Currency
    /// </summary>
    [DataMember(Name="currency", EmitDefaultValue=false)]
    public string Currency { get; set; }

    
    /// <summary>
    /// Gets or Sets ProductId
    /// </summary>
    [DataMember(Name="productId", EmitDefaultValue=false)]
    public string ProductId { get; set; }

    
    /// <summary>
    /// Sale price of a product. It can be null, then Sale price will be equal List price
    /// </summary>
    /// <value>Sale price of a product. It can be null, then Sale price will be equal List price</value>
    [DataMember(Name="sale", EmitDefaultValue=false)]
    public double? Sale { get; set; }

    
    /// <summary>
    /// Price of a product. It can be catalog price or purchase price
    /// </summary>
    /// <value>Price of a product. It can be catalog price or purchase price</value>
    [DataMember(Name="list", EmitDefaultValue=false)]
    public double? List { get; set; }

    
    /// <summary>
    /// It defines the minimum quantity of Products. Use it for creating tier prices.
    /// </summary>
    /// <value>It defines the minimum quantity of Products. Use it for creating tier prices.</value>
    [DataMember(Name="minQuantity", EmitDefaultValue=false)]
    public int? MinQuantity { get; set; }

    
    /// <summary>
    /// Gets or Sets CreatedDate
    /// </summary>
    [DataMember(Name="createdDate", EmitDefaultValue=false)]
    public DateTime? CreatedDate { get; set; }

    
    /// <summary>
    /// Gets or Sets ModifiedDate
    /// </summary>
    [DataMember(Name="modifiedDate", EmitDefaultValue=false)]
    public DateTime? ModifiedDate { get; set; }

    
    /// <summary>
    /// Gets or Sets CreatedBy
    /// </summary>
    [DataMember(Name="createdBy", EmitDefaultValue=false)]
    public string CreatedBy { get; set; }

    
    /// <summary>
    /// Gets or Sets ModifiedBy
    /// </summary>
    [DataMember(Name="modifiedBy", EmitDefaultValue=false)]
    public string ModifiedBy { get; set; }

    
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
      sb.Append("class VirtoCommercePricingModuleWebModelPrice {\n");
      
      sb.Append("  PricelistId: ").Append(PricelistId).Append("\n");
      
      sb.Append("  Currency: ").Append(Currency).Append("\n");
      
      sb.Append("  ProductId: ").Append(ProductId).Append("\n");
      
      sb.Append("  Sale: ").Append(Sale).Append("\n");
      
      sb.Append("  List: ").Append(List).Append("\n");
      
      sb.Append("  MinQuantity: ").Append(MinQuantity).Append("\n");
      
      sb.Append("  CreatedDate: ").Append(CreatedDate).Append("\n");
      
      sb.Append("  ModifiedDate: ").Append(ModifiedDate).Append("\n");
      
      sb.Append("  CreatedBy: ").Append(CreatedBy).Append("\n");
      
      sb.Append("  ModifiedBy: ").Append(ModifiedBy).Append("\n");
      
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
