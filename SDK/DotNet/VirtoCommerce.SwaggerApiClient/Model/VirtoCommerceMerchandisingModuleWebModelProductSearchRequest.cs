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
  public class VirtoCommerceMerchandisingModuleWebModelProductSearchRequest {
    
    /// <summary>
    /// Store ID
    /// </summary>
    /// <value>Store ID</value>
    [DataMember(Name="store", EmitDefaultValue=false)]
    public string Store { get; set; }

    
    /// <summary>
    /// Array of pricelist IDs
    /// </summary>
    /// <value>Array of pricelist IDs</value>
    [DataMember(Name="pricelists", EmitDefaultValue=false)]
    public List<string> Pricelists { get; set; }

    
    /// <summary>
    /// Response detalization scale (default value is ItemMedium)
    /// </summary>
    /// <value>Response detalization scale (default value is ItemMedium)</value>
    [DataMember(Name="responseGroup", EmitDefaultValue=false)]
    public string ResponseGroup { get; set; }

    
    /// <summary>
    /// Product category outline
    /// </summary>
    /// <value>Product category outline</value>
    [DataMember(Name="outline", EmitDefaultValue=false)]
    public string Outline { get; set; }

    
    /// <summary>
    /// Culture name (default value is \"en-us\")
    /// </summary>
    /// <value>Culture name (default value is \"en-us\")</value>
    [DataMember(Name="language", EmitDefaultValue=false)]
    public string Language { get; set; }

    
    /// <summary>
    /// Currency (default value is \"USD\")
    /// </summary>
    /// <value>Currency (default value is \"USD\")</value>
    [DataMember(Name="currency", EmitDefaultValue=false)]
    public string Currency { get; set; }

    
    /// <summary>
    /// Gets or sets the search phrase
    /// </summary>
    /// <value>Gets or sets the search phrase</value>
    [DataMember(Name="searchPhrase", EmitDefaultValue=false)]
    public string SearchPhrase { get; set; }

    
    /// <summary>
    /// Gets or sets the sort
    /// </summary>
    /// <value>Gets or sets the sort</value>
    [DataMember(Name="sort", EmitDefaultValue=false)]
    public string Sort { get; set; }

    
    /// <summary>
    /// Gets or sets the sort order ascending or descending
    /// </summary>
    /// <value>Gets or sets the sort order ascending or descending</value>
    [DataMember(Name="sortOrder", EmitDefaultValue=false)]
    public string SortOrder { get; set; }

    
    /// <summary>
    /// Gets or sets the start date
    /// </summary>
    /// <value>Gets or sets the start date</value>
    [DataMember(Name="startDateFrom", EmitDefaultValue=false)]
    public DateTime? StartDateFrom { get; set; }

    
    /// <summary>
    /// Gets or sets the number of items to skip
    /// </summary>
    /// <value>Gets or sets the number of items to skip</value>
    [DataMember(Name="skip", EmitDefaultValue=false)]
    public int? Skip { get; set; }

    
    /// <summary>
    /// Gets or sets the number of items to return
    /// </summary>
    /// <value>Gets or sets the number of items to return</value>
    [DataMember(Name="take", EmitDefaultValue=false)]
    public int? Take { get; set; }

    
    /// <summary>
    /// Gets or sets search terms collection\r\n            Item format: name:value1,value2,value3
    /// </summary>
    /// <value>Gets or sets search terms collection\r\n            Item format: name:value1,value2,value3</value>
    [DataMember(Name="terms", EmitDefaultValue=false)]
    public List<string> Terms { get; set; }

    
    /// <summary>
    /// Gets or sets the facets collection\r\n            Item format: name:value1,value2,value3
    /// </summary>
    /// <value>Gets or sets the facets collection\r\n            Item format: name:value1,value2,value3</value>
    [DataMember(Name="facets", EmitDefaultValue=false)]
    public List<string> Facets { get; set; }

    

    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class VirtoCommerceMerchandisingModuleWebModelProductSearchRequest {\n");
      
      sb.Append("  Store: ").Append(Store).Append("\n");
      
      sb.Append("  Pricelists: ").Append(Pricelists).Append("\n");
      
      sb.Append("  ResponseGroup: ").Append(ResponseGroup).Append("\n");
      
      sb.Append("  Outline: ").Append(Outline).Append("\n");
      
      sb.Append("  Language: ").Append(Language).Append("\n");
      
      sb.Append("  Currency: ").Append(Currency).Append("\n");
      
      sb.Append("  SearchPhrase: ").Append(SearchPhrase).Append("\n");
      
      sb.Append("  Sort: ").Append(Sort).Append("\n");
      
      sb.Append("  SortOrder: ").Append(SortOrder).Append("\n");
      
      sb.Append("  StartDateFrom: ").Append(StartDateFrom).Append("\n");
      
      sb.Append("  Skip: ").Append(Skip).Append("\n");
      
      sb.Append("  Take: ").Append(Take).Append("\n");
      
      sb.Append("  Terms: ").Append(Terms).Append("\n");
      
      sb.Append("  Facets: ").Append(Facets).Append("\n");
      
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
