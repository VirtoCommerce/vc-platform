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
  public class VirtoCommercePlatformWebModelSettingsSetting {
    
    /// <summary>
    /// Gets or Sets GroupName
    /// </summary>
    [DataMember(Name="groupName", EmitDefaultValue=false)]
    public string GroupName { get; set; }

    
    /// <summary>
    /// System name (ID) of the setting
    /// </summary>
    /// <value>System name (ID) of the setting</value>
    [DataMember(Name="name", EmitDefaultValue=false)]
    public string Name { get; set; }

    
    /// <summary>
    /// Current value for non-array setting
    /// </summary>
    /// <value>Current value for non-array setting</value>
    [DataMember(Name="value", EmitDefaultValue=false)]
    public string Value { get; set; }

    
    /// <summary>
    /// Gets or Sets ValueType
    /// </summary>
    [DataMember(Name="valueType", EmitDefaultValue=false)]
    public string ValueType { get; set; }

    
    /// <summary>
    /// Predefined set of allowed values for this setting
    /// </summary>
    /// <value>Predefined set of allowed values for this setting</value>
    [DataMember(Name="allowedValues", EmitDefaultValue=false)]
    public List<string> AllowedValues { get; set; }

    
    /// <summary>
    /// Gets or Sets DefaultValue
    /// </summary>
    [DataMember(Name="defaultValue", EmitDefaultValue=false)]
    public string DefaultValue { get; set; }

    
    /// <summary>
    /// Defines whether the setting can have multiple values
    /// </summary>
    /// <value>Defines whether the setting can have multiple values</value>
    [DataMember(Name="isArray", EmitDefaultValue=false)]
    public bool? IsArray { get; set; }

    
    /// <summary>
    /// Current values for array setting
    /// </summary>
    /// <value>Current values for array setting</value>
    [DataMember(Name="arrayValues", EmitDefaultValue=false)]
    public List<string> ArrayValues { get; set; }

    
    /// <summary>
    /// User-friendly name of the setting
    /// </summary>
    /// <value>User-friendly name of the setting</value>
    [DataMember(Name="title", EmitDefaultValue=false)]
    public string Title { get; set; }

    
    /// <summary>
    /// Gets or Sets Description
    /// </summary>
    [DataMember(Name="description", EmitDefaultValue=false)]
    public string Description { get; set; }

    

    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class VirtoCommercePlatformWebModelSettingsSetting {\n");
      
      sb.Append("  GroupName: ").Append(GroupName).Append("\n");
      
      sb.Append("  Name: ").Append(Name).Append("\n");
      
      sb.Append("  Value: ").Append(Value).Append("\n");
      
      sb.Append("  ValueType: ").Append(ValueType).Append("\n");
      
      sb.Append("  AllowedValues: ").Append(AllowedValues).Append("\n");
      
      sb.Append("  DefaultValue: ").Append(DefaultValue).Append("\n");
      
      sb.Append("  IsArray: ").Append(IsArray).Append("\n");
      
      sb.Append("  ArrayValues: ").Append(ArrayValues).Append("\n");
      
      sb.Append("  Title: ").Append(Title).Append("\n");
      
      sb.Append("  Description: ").Append(Description).Append("\n");
      
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
