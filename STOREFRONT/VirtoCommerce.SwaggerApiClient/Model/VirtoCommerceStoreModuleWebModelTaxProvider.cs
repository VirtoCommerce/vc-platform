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
  public class VirtoCommerceStoreModuleWebModelTaxProvider {
    
    /// <summary>
    /// Inner unique method code
    /// </summary>
    /// <value>Inner unique method code</value>
    [DataMember(Name="code", EmitDefaultValue=false)]
    public string Code { get; set; }

    
    /// <summary>
    /// Display name of shipping method
    /// </summary>
    /// <value>Display name of shipping method</value>
    [DataMember(Name="name", EmitDefaultValue=false)]
    public string Name { get; set; }

    
    /// <summary>
    /// Gets or Sets Description
    /// </summary>
    [DataMember(Name="description", EmitDefaultValue=false)]
    public string Description { get; set; }

    
    /// <summary>
    /// Absolute logo url of shipping method, can be used in UI
    /// </summary>
    /// <value>Absolute logo url of shipping method, can be used in UI</value>
    [DataMember(Name="logoUrl", EmitDefaultValue=false)]
    public string LogoUrl { get; set; }

    
    /// <summary>
    /// If true - method can be available on storefront
    /// </summary>
    /// <value>If true - method can be available on storefront</value>
    [DataMember(Name="isActive", EmitDefaultValue=false)]
    public bool? IsActive { get; set; }

    
    /// <summary>
    /// Gets or Sets Priority
    /// </summary>
    [DataMember(Name="priority", EmitDefaultValue=false)]
    public int? Priority { get; set; }

    
    /// <summary>
    /// Gets or Sets Settings
    /// </summary>
    [DataMember(Name="settings", EmitDefaultValue=false)]
    public List<VirtoCommerceStoreModuleWebModelSetting> Settings { get; set; }

    

    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class VirtoCommerceStoreModuleWebModelTaxProvider {\n");
      
      sb.Append("  Code: ").Append(Code).Append("\n");
      
      sb.Append("  Name: ").Append(Name).Append("\n");
      
      sb.Append("  Description: ").Append(Description).Append("\n");
      
      sb.Append("  LogoUrl: ").Append(LogoUrl).Append("\n");
      
      sb.Append("  IsActive: ").Append(IsActive).Append("\n");
      
      sb.Append("  Priority: ").Append(Priority).Append("\n");
      
      sb.Append("  Settings: ").Append(Settings).Append("\n");
      
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
