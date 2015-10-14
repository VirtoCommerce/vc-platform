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
  public class VirtoCommercePlatformWebModelSecurityChangePasswordInfo {
    
    /// <summary>
    /// Gets or Sets OldPassword
    /// </summary>
    [DataMember(Name="oldPassword", EmitDefaultValue=false)]
    public string OldPassword { get; set; }

    
    /// <summary>
    /// Gets or Sets NewPassword
    /// </summary>
    [DataMember(Name="newPassword", EmitDefaultValue=false)]
    public string NewPassword { get; set; }

    

    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class VirtoCommercePlatformWebModelSecurityChangePasswordInfo {\n");
      
      sb.Append("  OldPassword: ").Append(OldPassword).Append("\n");
      
      sb.Append("  NewPassword: ").Append(NewPassword).Append("\n");
      
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
