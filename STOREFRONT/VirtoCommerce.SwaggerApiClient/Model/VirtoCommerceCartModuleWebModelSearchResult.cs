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
  public class VirtoCommerceCartModuleWebModelSearchResult {
    
    /// <summary>
    /// Gets or sets the value of search result total shopping cart count
    /// </summary>
    /// <value>Gets or sets the value of search result total shopping cart count</value>
    [DataMember(Name="totalCount", EmitDefaultValue=false)]
    public int? TotalCount { get; set; }

    
    /// <summary>
    /// Gets or sets the collection of search result shopping carts
    /// </summary>
    /// <value>Gets or sets the collection of search result shopping carts</value>
    [DataMember(Name="shopingCarts", EmitDefaultValue=false)]
    public List<VirtoCommerceCartModuleWebModelShoppingCart> ShopingCarts { get; set; }

    

    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class VirtoCommerceCartModuleWebModelSearchResult {\n");
      
      sb.Append("  TotalCount: ").Append(TotalCount).Append("\n");
      
      sb.Append("  ShopingCarts: ").Append(ShopingCarts).Append("\n");
      
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
