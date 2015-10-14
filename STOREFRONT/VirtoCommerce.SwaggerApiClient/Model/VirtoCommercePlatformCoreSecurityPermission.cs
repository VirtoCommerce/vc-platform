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
  public class VirtoCommercePlatformCoreSecurityPermission {
    
    /// <summary>
    /// Gets or Sets Name
    /// </summary>
    [DataMember(Name="name", EmitDefaultValue=false)]
    public string Name { get; set; }

    
    /// <summary>
    /// Gets or Sets Description
    /// </summary>
    [DataMember(Name="description", EmitDefaultValue=false)]
    public string Description { get; set; }

    
    /// <summary>
    /// Gets or Sets ModuleId
    /// </summary>
    [DataMember(Name="moduleId", EmitDefaultValue=false)]
    public string ModuleId { get; set; }

    
    /// <summary>
    /// Gets or Sets GroupName
    /// </summary>
    [DataMember(Name="groupName", EmitDefaultValue=false)]
    public string GroupName { get; set; }

    
    /// <summary>
    /// Gets or Sets AssignedScopes
    /// </summary>
    [DataMember(Name="assignedScopes", EmitDefaultValue=false)]
    public List<VirtoCommercePlatformCoreSecurityPermissionScope> AssignedScopes { get; set; }

    
    /// <summary>
    /// Gets or Sets AvailableScopes
    /// </summary>
    [DataMember(Name="availableScopes", EmitDefaultValue=false)]
    public List<VirtoCommercePlatformCoreSecurityPermissionScope> AvailableScopes { get; set; }

    
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
      sb.Append("class VirtoCommercePlatformCoreSecurityPermission {\n");
      
      sb.Append("  Name: ").Append(Name).Append("\n");
      
      sb.Append("  Description: ").Append(Description).Append("\n");
      
      sb.Append("  ModuleId: ").Append(ModuleId).Append("\n");
      
      sb.Append("  GroupName: ").Append(GroupName).Append("\n");
      
      sb.Append("  AssignedScopes: ").Append(AssignedScopes).Append("\n");
      
      sb.Append("  AvailableScopes: ").Append(AvailableScopes).Append("\n");
      
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
