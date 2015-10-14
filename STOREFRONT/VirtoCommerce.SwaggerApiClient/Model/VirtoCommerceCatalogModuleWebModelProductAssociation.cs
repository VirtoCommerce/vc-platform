using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;



namespace VirtoCommerce.SwaggerApiClient.Model {

  /// <summary>
  /// Class containing associated product information like &#39;Accessory&#39;, &#39;Related Item&#39;, etc.
  /// </summary>
  [DataContract]
  public class VirtoCommerceCatalogModuleWebModelProductAssociation {
    
    /// <summary>
    /// Gets or sets the ProductAssociation name.
    /// </summary>
    /// <value>Gets or sets the ProductAssociation name.</value>
    [DataMember(Name="name", EmitDefaultValue=false)]
    public string Name { get; set; }

    
    /// <summary>
    /// Gets or Sets Description
    /// </summary>
    [DataMember(Name="description", EmitDefaultValue=false)]
    public string Description { get; set; }

    
    /// <summary>
    /// Gets or sets the order in which the associated product is displayed.
    /// </summary>
    /// <value>Gets or sets the order in which the associated product is displayed.</value>
    [DataMember(Name="priority", EmitDefaultValue=false)]
    public int? Priority { get; set; }

    
    /// <summary>
    /// Gets or sets the identifier of the associated product.
    /// </summary>
    /// <value>Gets or sets the identifier of the associated product.</value>
    [DataMember(Name="productId", EmitDefaultValue=false)]
    public string ProductId { get; set; }

    
    /// <summary>
    /// Gets or sets the name of the associated product.
    /// </summary>
    /// <value>Gets or sets the name of the associated product.</value>
    [DataMember(Name="productName", EmitDefaultValue=false)]
    public string ProductName { get; set; }

    
    /// <summary>
    /// Gets or sets the associated product code.
    /// </summary>
    /// <value>Gets or sets the associated product code.</value>
    [DataMember(Name="productCode", EmitDefaultValue=false)]
    public string ProductCode { get; set; }

    
    /// <summary>
    /// Gets or sets the associated product image.
    /// </summary>
    /// <value>Gets or sets the associated product image.</value>
    [DataMember(Name="productImg", EmitDefaultValue=false)]
    public string ProductImg { get; set; }

    

    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class VirtoCommerceCatalogModuleWebModelProductAssociation {\n");
      
      sb.Append("  Name: ").Append(Name).Append("\n");
      
      sb.Append("  Description: ").Append(Description).Append("\n");
      
      sb.Append("  Priority: ").Append(Priority).Append("\n");
      
      sb.Append("  ProductId: ").Append(ProductId).Append("\n");
      
      sb.Append("  ProductName: ").Append(ProductName).Append("\n");
      
      sb.Append("  ProductCode: ").Append(ProductCode).Append("\n");
      
      sb.Append("  ProductImg: ").Append(ProductImg).Append("\n");
      
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
