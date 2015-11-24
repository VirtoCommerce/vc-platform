using System;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;



namespace VirtoCommerce.Client.Model {

  /// <summary>
  /// 
  /// </summary>
  [DataContract]
  public class VirtoCommerceDomainCatalogModelSearchCriteria {
    
    /// <summary>
    /// Gets or Sets ResponseGroup
    /// </summary>
    [DataMember(Name="responseGroup", EmitDefaultValue=false)]
    public string ResponseGroup { get; set; }

    
    /// <summary>
    /// Gets or Sets Keyword
    /// </summary>
    [DataMember(Name="keyword", EmitDefaultValue=false)]
    public string Keyword { get; set; }

    
    /// <summary>
    /// Gets or Sets SearchInChildren
    /// </summary>
    [DataMember(Name="searchInChildren", EmitDefaultValue=false)]
    public bool? SearchInChildren { get; set; }

    
    /// <summary>
    /// Gets or Sets CategoryId
    /// </summary>
    [DataMember(Name="categoryId", EmitDefaultValue=false)]
    public string CategoryId { get; set; }

    
    /// <summary>
    /// Gets or Sets CategoriesIds
    /// </summary>
    [DataMember(Name="categoriesIds", EmitDefaultValue=false)]
    public List<string> CategoriesIds { get; set; }

    
    /// <summary>
    /// Gets or Sets CatalogId
    /// </summary>
    [DataMember(Name="catalogId", EmitDefaultValue=false)]
    public string CatalogId { get; set; }

    
    /// <summary>
    /// Gets or Sets CatalogsIds
    /// </summary>
    [DataMember(Name="catalogsIds", EmitDefaultValue=false)]
    public List<string> CatalogsIds { get; set; }

    
    /// <summary>
    /// Gets or Sets LanguageCode
    /// </summary>
    [DataMember(Name="languageCode", EmitDefaultValue=false)]
    public string LanguageCode { get; set; }

    
    /// <summary>
    /// Gets or Sets Currency
    /// </summary>
    [DataMember(Name="currency", EmitDefaultValue=false)]
    public string Currency { get; set; }

    
    /// <summary>
    /// Gets or Sets Code
    /// </summary>
    [DataMember(Name="code", EmitDefaultValue=false)]
    public string Code { get; set; }

    
    /// <summary>
    /// Gets or Sets Sort
    /// </summary>
    [DataMember(Name="sort", EmitDefaultValue=false)]
    public string Sort { get; set; }

    
    /// <summary>
    /// Gets or Sets Facets
    /// </summary>
    [DataMember(Name="facets", EmitDefaultValue=false)]
    public List<string> Facets { get; set; }

    
    /// <summary>
    /// Gets or Sets HideDirectLinkedCategories
    /// </summary>
    [DataMember(Name="hideDirectLinkedCategories", EmitDefaultValue=false)]
    public bool? HideDirectLinkedCategories { get; set; }

    
    /// <summary>
    /// Gets or Sets PropertyValues
    /// </summary>
    [DataMember(Name="propertyValues", EmitDefaultValue=false)]
    public List<VirtoCommerceDomainCatalogModelPropertyValue> PropertyValues { get; set; }

    
    /// <summary>
    /// Gets or Sets Start
    /// </summary>
    [DataMember(Name="start", EmitDefaultValue=false)]
    public int? Start { get; set; }

    
    /// <summary>
    /// Gets or Sets Count
    /// </summary>
    [DataMember(Name="count", EmitDefaultValue=false)]
    public int? Count { get; set; }

    
    /// <summary>
    /// Gets or Sets IndexDate
    /// </summary>
    [DataMember(Name="indexDate", EmitDefaultValue=false)]
    public DateTime? IndexDate { get; set; }

    

    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class VirtoCommerceDomainCatalogModelSearchCriteria {\n");
      
      sb.Append("  ResponseGroup: ").Append(ResponseGroup).Append("\n");
      
      sb.Append("  Keyword: ").Append(Keyword).Append("\n");
      
      sb.Append("  SearchInChildren: ").Append(SearchInChildren).Append("\n");
      
      sb.Append("  CategoryId: ").Append(CategoryId).Append("\n");
      
      sb.Append("  CategoriesIds: ").Append(CategoriesIds).Append("\n");
      
      sb.Append("  CatalogId: ").Append(CatalogId).Append("\n");
      
      sb.Append("  CatalogsIds: ").Append(CatalogsIds).Append("\n");
      
      sb.Append("  LanguageCode: ").Append(LanguageCode).Append("\n");
      
      sb.Append("  Currency: ").Append(Currency).Append("\n");
      
      sb.Append("  Code: ").Append(Code).Append("\n");
      
      sb.Append("  Sort: ").Append(Sort).Append("\n");
      
      sb.Append("  Facets: ").Append(Facets).Append("\n");
      
      sb.Append("  HideDirectLinkedCategories: ").Append(HideDirectLinkedCategories).Append("\n");
      
      sb.Append("  PropertyValues: ").Append(PropertyValues).Append("\n");
      
      sb.Append("  Start: ").Append(Start).Append("\n");
      
      sb.Append("  Count: ").Append(Count).Append("\n");
      
      sb.Append("  IndexDate: ").Append(IndexDate).Append("\n");
      
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
