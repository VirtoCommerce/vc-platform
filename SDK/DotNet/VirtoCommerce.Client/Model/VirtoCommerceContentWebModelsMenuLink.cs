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
    public partial class VirtoCommerceContentWebModelsMenuLink :  IEquatable<VirtoCommerceContentWebModelsMenuLink>
    {
        /// <summary>
        /// Gets or Sets Id
        /// </summary>
        [DataMember(Name="id", EmitDefaultValue=false)]
        public string Id { get; set; }

        /// <summary>
        /// Title of menu link element, displayed as link text or link title
        /// </summary>
        /// <value>Title of menu link element, displayed as link text or link title</value>
        [DataMember(Name="title", EmitDefaultValue=false)]
        public string Title { get; set; }

        /// <summary>
        /// Url of menu link element, inserts in href attribute of link
        /// </summary>
        /// <value>Url of menu link element, inserts in href attribute of link</value>
        [DataMember(Name="url", EmitDefaultValue=false)]
        public string Url { get; set; }

        /// <summary>
        /// Priority of menu link element, the higher the value, the higher in the list
        /// </summary>
        /// <value>Priority of menu link element, the higher the value, the higher in the list</value>
        [DataMember(Name="priority", EmitDefaultValue=false)]
        public int? Priority { get; set; }

        /// <summary>
        /// If true - will displayed in the list, if false - not
        /// </summary>
        /// <value>If true - will displayed in the list, if false - not</value>
        [DataMember(Name="isActive", EmitDefaultValue=false)]
        public bool? IsActive { get; set; }

        /// <summary>
        /// Gets or Sets MenuLinkListId
        /// </summary>
        [DataMember(Name="menuLinkListId", EmitDefaultValue=false)]
        public string MenuLinkListId { get; set; }

        /// <summary>
        /// Each link element can have an associated object like Product, Category, Promotion, etc.\r\n            Is a primary key of associated object
        /// </summary>
        /// <value>Each link element can have an associated object like Product, Category, Promotion, etc.\r\n            Is a primary key of associated object</value>
        [DataMember(Name="associatedObjectId", EmitDefaultValue=false)]
        public string AssociatedObjectId { get; set; }

        /// <summary>
        /// Display name for associated object
        /// </summary>
        /// <value>Display name for associated object</value>
        [DataMember(Name="associatedObjectName", EmitDefaultValue=false)]
        public string AssociatedObjectName { get; set; }

        /// <summary>
        /// Associated object type
        /// </summary>
        /// <value>Associated object type</value>
        [DataMember(Name="associatedObjectType", EmitDefaultValue=false)]
        public string AssociatedObjectType { get; set; }

        /// <summary>
        /// Gets or Sets SecurityScopes
        /// </summary>
        [DataMember(Name="securityScopes", EmitDefaultValue=false)]
        public List<string> SecurityScopes { get; set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class VirtoCommerceContentWebModelsMenuLink {\n");
            sb.Append("  Id: ").Append(Id).Append("\n");
            sb.Append("  Title: ").Append(Title).Append("\n");
            sb.Append("  Url: ").Append(Url).Append("\n");
            sb.Append("  Priority: ").Append(Priority).Append("\n");
            sb.Append("  IsActive: ").Append(IsActive).Append("\n");
            sb.Append("  MenuLinkListId: ").Append(MenuLinkListId).Append("\n");
            sb.Append("  AssociatedObjectId: ").Append(AssociatedObjectId).Append("\n");
            sb.Append("  AssociatedObjectName: ").Append(AssociatedObjectName).Append("\n");
            sb.Append("  AssociatedObjectType: ").Append(AssociatedObjectType).Append("\n");
            sb.Append("  SecurityScopes: ").Append(SecurityScopes).Append("\n");
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
            return this.Equals(obj as VirtoCommerceContentWebModelsMenuLink);
        }

        /// <summary>
        /// Returns true if VirtoCommerceContentWebModelsMenuLink instances are equal
        /// </summary>
        /// <param name="other">Instance of VirtoCommerceContentWebModelsMenuLink to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(VirtoCommerceContentWebModelsMenuLink other)
        {
            // credit: http://stackoverflow.com/a/10454552/677735
            if (other == null)
                return false;

            return 
                (
                    this.Id == other.Id ||
                    this.Id != null &&
                    this.Id.Equals(other.Id)
                ) && 
                (
                    this.Title == other.Title ||
                    this.Title != null &&
                    this.Title.Equals(other.Title)
                ) && 
                (
                    this.Url == other.Url ||
                    this.Url != null &&
                    this.Url.Equals(other.Url)
                ) && 
                (
                    this.Priority == other.Priority ||
                    this.Priority != null &&
                    this.Priority.Equals(other.Priority)
                ) && 
                (
                    this.IsActive == other.IsActive ||
                    this.IsActive != null &&
                    this.IsActive.Equals(other.IsActive)
                ) && 
                (
                    this.MenuLinkListId == other.MenuLinkListId ||
                    this.MenuLinkListId != null &&
                    this.MenuLinkListId.Equals(other.MenuLinkListId)
                ) && 
                (
                    this.AssociatedObjectId == other.AssociatedObjectId ||
                    this.AssociatedObjectId != null &&
                    this.AssociatedObjectId.Equals(other.AssociatedObjectId)
                ) && 
                (
                    this.AssociatedObjectName == other.AssociatedObjectName ||
                    this.AssociatedObjectName != null &&
                    this.AssociatedObjectName.Equals(other.AssociatedObjectName)
                ) && 
                (
                    this.AssociatedObjectType == other.AssociatedObjectType ||
                    this.AssociatedObjectType != null &&
                    this.AssociatedObjectType.Equals(other.AssociatedObjectType)
                ) && 
                (
                    this.SecurityScopes == other.SecurityScopes ||
                    this.SecurityScopes != null &&
                    this.SecurityScopes.SequenceEqual(other.SecurityScopes)
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

                if (this.Id != null)
                    hash = hash * 59 + this.Id.GetHashCode();

                if (this.Title != null)
                    hash = hash * 59 + this.Title.GetHashCode();

                if (this.Url != null)
                    hash = hash * 59 + this.Url.GetHashCode();

                if (this.Priority != null)
                    hash = hash * 59 + this.Priority.GetHashCode();

                if (this.IsActive != null)
                    hash = hash * 59 + this.IsActive.GetHashCode();

                if (this.MenuLinkListId != null)
                    hash = hash * 59 + this.MenuLinkListId.GetHashCode();

                if (this.AssociatedObjectId != null)
                    hash = hash * 59 + this.AssociatedObjectId.GetHashCode();

                if (this.AssociatedObjectName != null)
                    hash = hash * 59 + this.AssociatedObjectName.GetHashCode();

                if (this.AssociatedObjectType != null)
                    hash = hash * 59 + this.AssociatedObjectType.GetHashCode();

                if (this.SecurityScopes != null)
                    hash = hash * 59 + this.SecurityScopes.GetHashCode();

                return hash;
            }
        }

    }
}
