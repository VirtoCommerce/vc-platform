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
  public class VirtoCommercePlatformCoreDynamicPropertiesDynamicObjectProperty {
    
    /// <summary>
    /// Gets or Sets ObjectId
    /// </summary>
    [DataMember(Name="objectId", EmitDefaultValue=false)]
    public string ObjectId { get; set; }

    
    /// <summary>
    /// Gets or Sets Values
    /// </summary>
    [DataMember(Name="values", EmitDefaultValue=false)]
    public List<VirtoCommercePlatformCoreDynamicPropertiesDynamicPropertyObjectValue> Values { get; set; }

    
    /// <summary>
    /// Gets or Sets Name
    /// </summary>
    [DataMember(Name="name", EmitDefaultValue=false)]
    public string Name { get; set; }

    
    /// <summary>
    /// Gets or Sets ObjectType
    /// </summary>
    [DataMember(Name="objectType", EmitDefaultValue=false)]
    public string ObjectType { get; set; }

    
    /// <summary>
    /// Gets or Sets IsArray
    /// </summary>
    [DataMember(Name="isArray", EmitDefaultValue=false)]
    public bool? IsArray { get; set; }

    
    /// <summary>
    /// Gets or Sets IsDictionary
    /// </summary>
    [DataMember(Name="isDictionary", EmitDefaultValue=false)]
    public bool? IsDictionary { get; set; }

    
    /// <summary>
    /// Gets or Sets IsMultilingual
    /// </summary>
    [DataMember(Name="isMultilingual", EmitDefaultValue=false)]
    public bool? IsMultilingual { get; set; }

    
    /// <summary>
    /// Gets or Sets IsRequired
    /// </summary>
    [DataMember(Name="isRequired", EmitDefaultValue=false)]
    public bool? IsRequired { get; set; }

    
    /// <summary>
    /// Gets or Sets ValueType
    /// </summary>
    [DataMember(Name="valueType", EmitDefaultValue=false)]
    public string ValueType { get; set; }

    
    /// <summary>
    /// Gets or Sets DisplayNames
    /// </summary>
    [DataMember(Name="displayNames", EmitDefaultValue=false)]
    public List<VirtoCommercePlatformCoreDynamicPropertiesDynamicPropertyName> DisplayNames { get; set; }

    
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
      sb.Append("class VirtoCommercePlatformCoreDynamicPropertiesDynamicObjectProperty {\n");
      
      sb.Append("  ObjectId: ").Append(ObjectId).Append("\n");
      
      sb.Append("  Values: ").Append(Values).Append("\n");
      
      sb.Append("  Name: ").Append(Name).Append("\n");
      
      sb.Append("  ObjectType: ").Append(ObjectType).Append("\n");
      
      sb.Append("  IsArray: ").Append(IsArray).Append("\n");
      
      sb.Append("  IsDictionary: ").Append(IsDictionary).Append("\n");
      
      sb.Append("  IsMultilingual: ").Append(IsMultilingual).Append("\n");
      
      sb.Append("  IsRequired: ").Append(IsRequired).Append("\n");
      
      sb.Append("  ValueType: ").Append(ValueType).Append("\n");
      
      sb.Append("  DisplayNames: ").Append(DisplayNames).Append("\n");
      
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
