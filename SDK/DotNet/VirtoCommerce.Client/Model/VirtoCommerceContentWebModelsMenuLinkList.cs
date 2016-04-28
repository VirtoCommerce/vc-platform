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
    public partial class VirtoCommerceContentWebModelsMenuLinkList :  IEquatable<VirtoCommerceContentWebModelsMenuLinkList>
    {
        /// <summary>
        /// Gets or Sets Id
        /// </summary>
        [DataMember(Name="id", EmitDefaultValue=false)]
        public string Id { get; set; }

        /// <summary>
        /// Name of menu link list, can be used as title of list in frontend
        /// </summary>
        /// <value>Name of menu link list, can be used as title of list in frontend</value>
        [DataMember(Name="name", EmitDefaultValue=false)]
        public string Name { get; set; }

        /// <summary>
        /// Store identifier, for which the list belongs
        /// </summary>
        /// <value>Store identifier, for which the list belongs</value>
        [DataMember(Name="storeId", EmitDefaultValue=false)]
        public string StoreId { get; set; }

        /// <summary>
        /// Locale of this menu link list
        /// </summary>
        /// <value>Locale of this menu link list</value>
        [DataMember(Name="language", EmitDefaultValue=false)]
        public string Language { get; set; }

        /// <summary>
        /// Gets or Sets MenuLinks
        /// </summary>
        [DataMember(Name="menuLinks", EmitDefaultValue=false)]
        public List<VirtoCommerceContentWebModelsMenuLink> MenuLinks { get; set; }

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
            sb.Append("class VirtoCommerceContentWebModelsMenuLinkList {\n");
            sb.Append("  Id: ").Append(Id).Append("\n");
            sb.Append("  Name: ").Append(Name).Append("\n");
            sb.Append("  StoreId: ").Append(StoreId).Append("\n");
            sb.Append("  Language: ").Append(Language).Append("\n");
            sb.Append("  MenuLinks: ").Append(MenuLinks).Append("\n");
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
            return this.Equals(obj as VirtoCommerceContentWebModelsMenuLinkList);
        }

        /// <summary>
        /// Returns true if VirtoCommerceContentWebModelsMenuLinkList instances are equal
        /// </summary>
        /// <param name="other">Instance of VirtoCommerceContentWebModelsMenuLinkList to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(VirtoCommerceContentWebModelsMenuLinkList other)
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
                    this.Name == other.Name ||
                    this.Name != null &&
                    this.Name.Equals(other.Name)
                ) && 
                (
                    this.StoreId == other.StoreId ||
                    this.StoreId != null &&
                    this.StoreId.Equals(other.StoreId)
                ) && 
                (
                    this.Language == other.Language ||
                    this.Language != null &&
                    this.Language.Equals(other.Language)
                ) && 
                (
                    this.MenuLinks == other.MenuLinks ||
                    this.MenuLinks != null &&
                    this.MenuLinks.SequenceEqual(other.MenuLinks)
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

                if (this.Name != null)
                    hash = hash * 59 + this.Name.GetHashCode();

                if (this.StoreId != null)
                    hash = hash * 59 + this.StoreId.GetHashCode();

                if (this.Language != null)
                    hash = hash * 59 + this.Language.GetHashCode();

                if (this.MenuLinks != null)
                    hash = hash * 59 + this.MenuLinks.GetHashCode();

                if (this.SecurityScopes != null)
                    hash = hash * 59 + this.SecurityScopes.GetHashCode();

                return hash;
            }
        }

    }
}
