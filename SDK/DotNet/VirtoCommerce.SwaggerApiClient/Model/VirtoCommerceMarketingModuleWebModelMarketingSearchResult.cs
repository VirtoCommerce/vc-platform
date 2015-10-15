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
  public class VirtoCommerceMarketingModuleWebModelMarketingSearchResult {
    
    /// <summary>
    /// Gets or Sets TotalCount
    /// </summary>
    [DataMember(Name="totalCount", EmitDefaultValue=false)]
    public int? TotalCount { get; set; }

    
    /// <summary>
    /// Gets or Sets Promotions
    /// </summary>
    [DataMember(Name="promotions", EmitDefaultValue=false)]
    public List<VirtoCommerceMarketingModuleWebModelPromotion> Promotions { get; set; }

    
    /// <summary>
    /// Gets or Sets ContentPlaces
    /// </summary>
    [DataMember(Name="contentPlaces", EmitDefaultValue=false)]
    public List<VirtoCommerceMarketingModuleWebModelDynamicContentPlace> ContentPlaces { get; set; }

    
    /// <summary>
    /// Gets or Sets ContentItems
    /// </summary>
    [DataMember(Name="contentItems", EmitDefaultValue=false)]
    public List<VirtoCommerceMarketingModuleWebModelDynamicContentItem> ContentItems { get; set; }

    
    /// <summary>
    /// Gets or Sets ContentPublications
    /// </summary>
    [DataMember(Name="contentPublications", EmitDefaultValue=false)]
    public List<VirtoCommerceMarketingModuleWebModelDynamicContentPublication> ContentPublications { get; set; }

    
    /// <summary>
    /// Gets or Sets ContentFolders
    /// </summary>
    [DataMember(Name="contentFolders", EmitDefaultValue=false)]
    public List<VirtoCommerceMarketingModuleWebModelDynamicContentFolder> ContentFolders { get; set; }

    

    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class VirtoCommerceMarketingModuleWebModelMarketingSearchResult {\n");
      
      sb.Append("  TotalCount: ").Append(TotalCount).Append("\n");
      
      sb.Append("  Promotions: ").Append(Promotions).Append("\n");
      
      sb.Append("  ContentPlaces: ").Append(ContentPlaces).Append("\n");
      
      sb.Append("  ContentItems: ").Append(ContentItems).Append("\n");
      
      sb.Append("  ContentPublications: ").Append(ContentPublications).Append("\n");
      
      sb.Append("  ContentFolders: ").Append(ContentFolders).Append("\n");
      
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
