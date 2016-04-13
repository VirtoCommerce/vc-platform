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
    public partial class VirtoCommerceDomainCatalogModelProductAssociation :  IEquatable<VirtoCommerceDomainCatalogModelProductAssociation>
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
        /// Gets or Sets Priority
        /// </summary>
        [DataMember(Name="priority", EmitDefaultValue=false)]
        public int? Priority { get; set; }

        /// <summary>
        /// Gets or Sets Type
        /// </summary>
        [DataMember(Name="type", EmitDefaultValue=false)]
        public string Type { get; set; }

        /// <summary>
        /// Gets or Sets AssociatedProductId
        /// </summary>
        [DataMember(Name="associatedProductId", EmitDefaultValue=false)]
        public string AssociatedProductId { get; set; }

        /// <summary>
        /// Gets or Sets AssociatedProduct
        /// </summary>
        [DataMember(Name="associatedProduct", EmitDefaultValue=false)]
        public VirtoCommerceDomainCatalogModelCatalogProduct AssociatedProduct { get; set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class VirtoCommerceDomainCatalogModelProductAssociation {\n");
            sb.Append("  Name: ").Append(Name).Append("\n");
            sb.Append("  Description: ").Append(Description).Append("\n");
            sb.Append("  Priority: ").Append(Priority).Append("\n");
            sb.Append("  Type: ").Append(Type).Append("\n");
            sb.Append("  AssociatedProductId: ").Append(AssociatedProductId).Append("\n");
            sb.Append("  AssociatedProduct: ").Append(AssociatedProduct).Append("\n");
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
            return this.Equals(obj as VirtoCommerceDomainCatalogModelProductAssociation);
        }

        /// <summary>
        /// Returns true if VirtoCommerceDomainCatalogModelProductAssociation instances are equal
        /// </summary>
        /// <param name="other">Instance of VirtoCommerceDomainCatalogModelProductAssociation to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(VirtoCommerceDomainCatalogModelProductAssociation other)
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
                    this.Priority == other.Priority ||
                    this.Priority != null &&
                    this.Priority.Equals(other.Priority)
                ) && 
                (
                    this.Type == other.Type ||
                    this.Type != null &&
                    this.Type.Equals(other.Type)
                ) && 
                (
                    this.AssociatedProductId == other.AssociatedProductId ||
                    this.AssociatedProductId != null &&
                    this.AssociatedProductId.Equals(other.AssociatedProductId)
                ) && 
                (
                    this.AssociatedProduct == other.AssociatedProduct ||
                    this.AssociatedProduct != null &&
                    this.AssociatedProduct.Equals(other.AssociatedProduct)
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

                if (this.Priority != null)
                    hash = hash * 59 + this.Priority.GetHashCode();

                if (this.Type != null)
                    hash = hash * 59 + this.Type.GetHashCode();

                if (this.AssociatedProductId != null)
                    hash = hash * 59 + this.AssociatedProductId.GetHashCode();

                if (this.AssociatedProduct != null)
                    hash = hash * 59 + this.AssociatedProduct.GetHashCode();

                return hash;
            }
        }

    }
}
