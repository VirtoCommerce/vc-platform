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
  public class VirtoCommercePlatformCoreSecurityApplicationUserExtended {
    
    /// <summary>
    /// Gets or Sets Id
    /// </summary>
    [DataMember(Name="id", EmitDefaultValue=false)]
    public string Id { get; set; }

    
    /// <summary>
    /// Gets or Sets UserName
    /// </summary>
    [DataMember(Name="userName", EmitDefaultValue=false)]
    public string UserName { get; set; }

    
    /// <summary>
    /// Gets or Sets Email
    /// </summary>
    [DataMember(Name="email", EmitDefaultValue=false)]
    public string Email { get; set; }

    
    /// <summary>
    /// Gets or Sets PhoneNumber
    /// </summary>
    [DataMember(Name="phoneNumber", EmitDefaultValue=false)]
    public string PhoneNumber { get; set; }

    
    /// <summary>
    /// Gets or Sets StoreId
    /// </summary>
    [DataMember(Name="storeId", EmitDefaultValue=false)]
    public string StoreId { get; set; }

    
    /// <summary>
    /// Gets or Sets MemberId
    /// </summary>
    [DataMember(Name="memberId", EmitDefaultValue=false)]
    public string MemberId { get; set; }

    
    /// <summary>
    /// Gets or Sets Icon
    /// </summary>
    [DataMember(Name="icon", EmitDefaultValue=false)]
    public string Icon { get; set; }

    
    /// <summary>
    /// Gets or Sets IsAdministrator
    /// </summary>
    [DataMember(Name="isAdministrator", EmitDefaultValue=false)]
    public bool? IsAdministrator { get; set; }

    
    /// <summary>
    /// Gets or Sets UserType
    /// </summary>
    [DataMember(Name="userType", EmitDefaultValue=false)]
    public string UserType { get; set; }

    
    /// <summary>
    /// Gets or Sets UserState
    /// </summary>
    [DataMember(Name="userState", EmitDefaultValue=false)]
    public string UserState { get; set; }

    
    /// <summary>
    /// Gets or Sets Password
    /// </summary>
    [DataMember(Name="password", EmitDefaultValue=false)]
    public string Password { get; set; }

    
    /// <summary>
    /// Gets or Sets PasswordHash
    /// </summary>
    [DataMember(Name="passwordHash", EmitDefaultValue=false)]
    public string PasswordHash { get; set; }

    
    /// <summary>
    /// Gets or Sets SecurityStamp
    /// </summary>
    [DataMember(Name="securityStamp", EmitDefaultValue=false)]
    public string SecurityStamp { get; set; }

    
    /// <summary>
    /// Gets or Sets Logins
    /// </summary>
    [DataMember(Name="logins", EmitDefaultValue=false)]
    public List<VirtoCommercePlatformCoreSecurityApplicationUserLogin> Logins { get; set; }

    
    /// <summary>
    /// Gets or Sets Roles
    /// </summary>
    [DataMember(Name="roles", EmitDefaultValue=false)]
    public List<VirtoCommercePlatformCoreSecurityRole> Roles { get; set; }

    
    /// <summary>
    /// Gets or Sets Permissions
    /// </summary>
    [DataMember(Name="permissions", EmitDefaultValue=false)]
    public List<string> Permissions { get; set; }

    
    /// <summary>
    /// Gets or Sets ApiAccounts
    /// </summary>
    [DataMember(Name="apiAccounts", EmitDefaultValue=false)]
    public List<VirtoCommercePlatformCoreSecurityApiAccount> ApiAccounts { get; set; }

    

    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class VirtoCommercePlatformCoreSecurityApplicationUserExtended {\n");
      
      sb.Append("  Id: ").Append(Id).Append("\n");
      
      sb.Append("  UserName: ").Append(UserName).Append("\n");
      
      sb.Append("  Email: ").Append(Email).Append("\n");
      
      sb.Append("  PhoneNumber: ").Append(PhoneNumber).Append("\n");
      
      sb.Append("  StoreId: ").Append(StoreId).Append("\n");
      
      sb.Append("  MemberId: ").Append(MemberId).Append("\n");
      
      sb.Append("  Icon: ").Append(Icon).Append("\n");
      
      sb.Append("  IsAdministrator: ").Append(IsAdministrator).Append("\n");
      
      sb.Append("  UserType: ").Append(UserType).Append("\n");
      
      sb.Append("  UserState: ").Append(UserState).Append("\n");
      
      sb.Append("  Password: ").Append(Password).Append("\n");
      
      sb.Append("  PasswordHash: ").Append(PasswordHash).Append("\n");
      
      sb.Append("  SecurityStamp: ").Append(SecurityStamp).Append("\n");
      
      sb.Append("  Logins: ").Append(Logins).Append("\n");
      
      sb.Append("  Roles: ").Append(Roles).Append("\n");
      
      sb.Append("  Permissions: ").Append(Permissions).Append("\n");
      
      sb.Append("  ApiAccounts: ").Append(ApiAccounts).Append("\n");
      
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
