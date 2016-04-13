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
    public partial class VirtoCommerceSearchModuleWebModelFilterProperty :  IEquatable<VirtoCommerceSearchModuleWebModelFilterProperty>
    {
        /// <summary>
        /// Gets or Sets Name
        /// </summary>
        [DataMember(Name="name", EmitDefaultValue=false)]
        public string Name { get; set; }

        /// <summary>
        /// Gets or Sets IsSelected
        /// </summary>
        [DataMember(Name="isSelected", EmitDefaultValue=false)]
        public bool? IsSelected { get; set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class VirtoCommerceSearchModuleWebModelFilterProperty {\n");
            sb.Append("  Name: ").Append(Name).Append("\n");
            sb.Append("  IsSelected: ").Append(IsSelected).Append("\n");
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
            return this.Equals(obj as VirtoCommerceSearchModuleWebModelFilterProperty);
        }

        /// <summary>
        /// Returns true if VirtoCommerceSearchModuleWebModelFilterProperty instances are equal
        /// </summary>
        /// <param name="other">Instance of VirtoCommerceSearchModuleWebModelFilterProperty to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(VirtoCommerceSearchModuleWebModelFilterProperty other)
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
                    this.IsSelected == other.IsSelected ||
                    this.IsSelected != null &&
                    this.IsSelected.Equals(other.IsSelected)
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

                if (this.IsSelected != null)
                    hash = hash * 59 + this.IsSelected.GetHashCode();

                return hash;
            }
        }

    }
}
