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
    public partial class VirtoCommerceDomainCommonConditionExpressionTree :  IEquatable<VirtoCommerceDomainCommonConditionExpressionTree>
    {
        /// <summary>
        /// Gets or Sets Id
        /// </summary>
        [DataMember(Name="id", EmitDefaultValue=false)]
        public string Id { get; set; }

        /// <summary>
        /// Gets or Sets AvailableChildren
        /// </summary>
        [DataMember(Name="availableChildren", EmitDefaultValue=false)]
        public List<VirtoCommerceDomainCommonDynamicExpression> AvailableChildren { get; set; }

        /// <summary>
        /// Gets or Sets Children
        /// </summary>
        [DataMember(Name="children", EmitDefaultValue=false)]
        public List<VirtoCommerceDomainCommonDynamicExpression> Children { get; set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class VirtoCommerceDomainCommonConditionExpressionTree {\n");
            sb.Append("  Id: ").Append(Id).Append("\n");
            sb.Append("  AvailableChildren: ").Append(AvailableChildren).Append("\n");
            sb.Append("  Children: ").Append(Children).Append("\n");
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
            return this.Equals(obj as VirtoCommerceDomainCommonConditionExpressionTree);
        }

        /// <summary>
        /// Returns true if VirtoCommerceDomainCommonConditionExpressionTree instances are equal
        /// </summary>
        /// <param name="other">Instance of VirtoCommerceDomainCommonConditionExpressionTree to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(VirtoCommerceDomainCommonConditionExpressionTree other)
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
                    this.AvailableChildren == other.AvailableChildren ||
                    this.AvailableChildren != null &&
                    this.AvailableChildren.SequenceEqual(other.AvailableChildren)
                ) && 
                (
                    this.Children == other.Children ||
                    this.Children != null &&
                    this.Children.SequenceEqual(other.Children)
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

                if (this.AvailableChildren != null)
                    hash = hash * 59 + this.AvailableChildren.GetHashCode();

                if (this.Children != null)
                    hash = hash * 59 + this.Children.GetHashCode();

                return hash;
            }
        }

    }
}
