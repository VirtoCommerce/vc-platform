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
    public partial class VirtoCommercePlatformCoreSecurityPermission :  IEquatable<VirtoCommercePlatformCoreSecurityPermission>
    {
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
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
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
            return this.Equals(obj as VirtoCommercePlatformCoreSecurityPermission);
        }

        /// <summary>
        /// Returns true if VirtoCommercePlatformCoreSecurityPermission instances are equal
        /// </summary>
        /// <param name="other">Instance of VirtoCommercePlatformCoreSecurityPermission to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(VirtoCommercePlatformCoreSecurityPermission other)
        {
            // credit: http://stackoverflow.com/a/10454552/677735
            if (other == null)
                return false;

            return 
                (
                    this.Name == other.Name ||
                    this.Name != null &&
                    this.Name.Equals(other.Name)
                ) && 
                (
                    this.Description == other.Description ||
                    this.Description != null &&
                    this.Description.Equals(other.Description)
                ) && 
                (
                    this.ModuleId == other.ModuleId ||
                    this.ModuleId != null &&
                    this.ModuleId.Equals(other.ModuleId)
                ) && 
                (
                    this.GroupName == other.GroupName ||
                    this.GroupName != null &&
                    this.GroupName.Equals(other.GroupName)
                ) && 
                (
                    this.AssignedScopes == other.AssignedScopes ||
                    this.AssignedScopes != null &&
                    this.AssignedScopes.SequenceEqual(other.AssignedScopes)
                ) && 
                (
                    this.AvailableScopes == other.AvailableScopes ||
                    this.AvailableScopes != null &&
                    this.AvailableScopes.SequenceEqual(other.AvailableScopes)
                ) && 
                (
                    this.Id == other.Id ||
                    this.Id != null &&
                    this.Id.Equals(other.Id)
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

                if (this.Name != null)
                    hash = hash * 59 + this.Name.GetHashCode();

                if (this.Description != null)
                    hash = hash * 59 + this.Description.GetHashCode();

                if (this.ModuleId != null)
                    hash = hash * 59 + this.ModuleId.GetHashCode();

                if (this.GroupName != null)
                    hash = hash * 59 + this.GroupName.GetHashCode();

                if (this.AssignedScopes != null)
                    hash = hash * 59 + this.AssignedScopes.GetHashCode();

                if (this.AvailableScopes != null)
                    hash = hash * 59 + this.AvailableScopes.GetHashCode();

                if (this.Id != null)
                    hash = hash * 59 + this.Id.GetHashCode();

                return hash;
            }
        }

    }
}
