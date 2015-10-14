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
  public class VirtoCommercePlatformCoreSecurityApplicationUserLogin {
    
    /// <summary>
    /// Gets or Sets LoginProvider
    /// </summary>
    [DataMember(Name="loginProvider", EmitDefaultValue=false)]
    public string LoginProvider { get; set; }

    
    /// <summary>
    /// Gets or Sets ProviderKey
    /// </summary>
    [DataMember(Name="providerKey", EmitDefaultValue=false)]
    public string ProviderKey { get; set; }

    

    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class VirtoCommercePlatformCoreSecurityApplicationUserLogin {\n");
      
      sb.Append("  LoginProvider: ").Append(LoginProvider).Append("\n");
      
      sb.Append("  ProviderKey: ").Append(ProviderKey).Append("\n");
      
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
