using System;
using System.Linq;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace VirtoCommerce.Client.Model
{
    /// <summary>
    /// 
    /// </summary>
    [DataContract]
    public partial class VirtoCommerceCoreModuleWebModelStorefrontUser :  IEquatable<VirtoCommerceCoreModuleWebModelStorefrontUser>
    {
        /// <summary>
        /// List of stores which  user can sing in
        /// </summary>
        /// <value>List of stores which  user can sing in</value>
        [DataMember(Name="allowedStores", EmitDefaultValue=false)]
        public List<string> AllowedStores { get; set; }

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
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class VirtoCommerceCoreModuleWebModelStorefrontUser {\n");
            sb.Append("  AllowedStores: ").Append(AllowedStores).Append("\n");
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
        /// Returns the JSON string presentation of the object
        /// </summary>
        /// <returns>JSON string presentation of the object</returns>
        public string ToJson()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }

        /// <summary>
        /// Returns true if objects are equal
        /// </summary>
        /// <param name="obj">Object to be compared</param>
        /// <returns>Boolean</returns>
        public override bool Equals(object obj)
        {
            // credit: http://stackoverflow.com/a/10454552/677735
            return this.Equals(obj as VirtoCommerceCoreModuleWebModelStorefrontUser);
        }

        /// <summary>
        /// Returns true if VirtoCommerceCoreModuleWebModelStorefrontUser instances are equal
        /// </summary>
        /// <param name="other">Instance of VirtoCommerceCoreModuleWebModelStorefrontUser to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(VirtoCommerceCoreModuleWebModelStorefrontUser other)
        {
            // credit: http://stackoverflow.com/a/10454552/677735
            if (other == null)
                return false;

            return 
                (
                    this.AllowedStores == other.AllowedStores ||
                    this.AllowedStores != null &&
                    this.AllowedStores.SequenceEqual(other.AllowedStores)
                ) && 
                (
                    this.Id == other.Id ||
                    this.Id != null &&
                    this.Id.Equals(other.Id)
                ) && 
                (
                    this.UserName == other.UserName ||
                    this.UserName != null &&
                    this.UserName.Equals(other.UserName)
                ) && 
                (
                    this.Email == other.Email ||
                    this.Email != null &&
                    this.Email.Equals(other.Email)
                ) && 
                (
                    this.PhoneNumber == other.PhoneNumber ||
                    this.PhoneNumber != null &&
                    this.PhoneNumber.Equals(other.PhoneNumber)
                ) && 
                (
                    this.StoreId == other.StoreId ||
                    this.StoreId != null &&
                    this.StoreId.Equals(other.StoreId)
                ) && 
                (
                    this.MemberId == other.MemberId ||
                    this.MemberId != null &&
                    this.MemberId.Equals(other.MemberId)
                ) && 
                (
                    this.Icon == other.Icon ||
                    this.Icon != null &&
                    this.Icon.Equals(other.Icon)
                ) && 
                (
                    this.IsAdministrator == other.IsAdministrator ||
                    this.IsAdministrator != null &&
                    this.IsAdministrator.Equals(other.IsAdministrator)
                ) && 
                (
                    this.UserType == other.UserType ||
                    this.UserType != null &&
                    this.UserType.Equals(other.UserType)
                ) && 
                (
                    this.UserState == other.UserState ||
                    this.UserState != null &&
                    this.UserState.Equals(other.UserState)
                ) && 
                (
                    this.Password == other.Password ||
                    this.Password != null &&
                    this.Password.Equals(other.Password)
                ) && 
                (
                    this.PasswordHash == other.PasswordHash ||
                    this.PasswordHash != null &&
                    this.PasswordHash.Equals(other.PasswordHash)
                ) && 
                (
                    this.SecurityStamp == other.SecurityStamp ||
                    this.SecurityStamp != null &&
                    this.SecurityStamp.Equals(other.SecurityStamp)
                ) && 
                (
                    this.Logins == other.Logins ||
                    this.Logins != null &&
                    this.Logins.SequenceEqual(other.Logins)
                ) && 
                (
                    this.Roles == other.Roles ||
                    this.Roles != null &&
                    this.Roles.SequenceEqual(other.Roles)
                ) && 
                (
                    this.Permissions == other.Permissions ||
                    this.Permissions != null &&
                    this.Permissions.SequenceEqual(other.Permissions)
                ) && 
                (
                    this.ApiAccounts == other.ApiAccounts ||
                    this.ApiAccounts != null &&
                    this.ApiAccounts.SequenceEqual(other.ApiAccounts)
                );
        }

        /// <summary>
        /// Gets the hash code
        /// </summary>
        /// <returns>Hash code</returns>
        public override int GetHashCode()
        {
            // credit: http://stackoverflow.com/a/263416/677735
            unchecked // Overflow is fine, just wrap
            {
                int hash = 41;
                // Suitable nullity checks etc, of course :)

                if (this.AllowedStores != null)
                    hash = hash * 59 + this.AllowedStores.GetHashCode();

                if (this.Id != null)
                    hash = hash * 59 + this.Id.GetHashCode();

                if (this.UserName != null)
                    hash = hash * 59 + this.UserName.GetHashCode();

                if (this.Email != null)
                    hash = hash * 59 + this.Email.GetHashCode();

                if (this.PhoneNumber != null)
                    hash = hash * 59 + this.PhoneNumber.GetHashCode();

                if (this.StoreId != null)
                    hash = hash * 59 + this.StoreId.GetHashCode();

                if (this.MemberId != null)
                    hash = hash * 59 + this.MemberId.GetHashCode();

                if (this.Icon != null)
                    hash = hash * 59 + this.Icon.GetHashCode();

                if (this.IsAdministrator != null)
                    hash = hash * 59 + this.IsAdministrator.GetHashCode();

                if (this.UserType != null)
                    hash = hash * 59 + this.UserType.GetHashCode();

                if (this.UserState != null)
                    hash = hash * 59 + this.UserState.GetHashCode();

                if (this.Password != null)
                    hash = hash * 59 + this.Password.GetHashCode();

                if (this.PasswordHash != null)
                    hash = hash * 59 + this.PasswordHash.GetHashCode();

                if (this.SecurityStamp != null)
                    hash = hash * 59 + this.SecurityStamp.GetHashCode();

                if (this.Logins != null)
                    hash = hash * 59 + this.Logins.GetHashCode();

                if (this.Roles != null)
                    hash = hash * 59 + this.Roles.GetHashCode();

                if (this.Permissions != null)
                    hash = hash * 59 + this.Permissions.GetHashCode();

                if (this.ApiAccounts != null)
                    hash = hash * 59 + this.ApiAccounts.GetHashCode();

                return hash;
            }
        }

    }
}
