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
    public partial class VirtoCommerceCatalogModuleWebModelAggregationItem :  IEquatable<VirtoCommerceCatalogModuleWebModelAggregationItem>
    {
        /// <summary>
        /// Gets or sets the aggregation item value
        /// </summary>
        /// <value>Gets or sets the aggregation item value</value>
        [DataMember(Name="value", EmitDefaultValue=false)]
        public Object Value { get; set; }

        /// <summary>
        /// Gets or sets the aggregation item count
        /// </summary>
        /// <value>Gets or sets the aggregation item count</value>
        [DataMember(Name="count", EmitDefaultValue=false)]
        public int? Count { get; set; }

        /// <summary>
        /// Gets or sets the flag for aggregation item is applied
        /// </summary>
        /// <value>Gets or sets the flag for aggregation item is applied</value>
        [DataMember(Name="isApplied", EmitDefaultValue=false)]
        public bool? IsApplied { get; set; }

        /// <summary>
        /// Gets or sets the collection of aggregation item labels
        /// </summary>
        /// <value>Gets or sets the collection of aggregation item labels</value>
        [DataMember(Name="labels", EmitDefaultValue=false)]
        public List<VirtoCommerceCatalogModuleWebModelAggregationLabel> Labels { get; set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class VirtoCommerceCatalogModuleWebModelAggregationItem {\n");
            sb.Append("  Value: ").Append(Value).Append("\n");
            sb.Append("  Count: ").Append(Count).Append("\n");
            sb.Append("  IsApplied: ").Append(IsApplied).Append("\n");
            sb.Append("  Labels: ").Append(Labels).Append("\n");
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
            return this.Equals(obj as VirtoCommerceCatalogModuleWebModelAggregationItem);
        }

        /// <summary>
        /// Returns true if VirtoCommerceCatalogModuleWebModelAggregationItem instances are equal
        /// </summary>
        /// <param name="other">Instance of VirtoCommerceCatalogModuleWebModelAggregationItem to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(VirtoCommerceCatalogModuleWebModelAggregationItem other)
        {
            // credit: http://stackoverflow.com/a/10454552/677735
            if (other == null)
                return false;

            return 
                (
                    this.Value == other.Value ||
                    this.Value != null &&
                    this.Value.Equals(other.Value)
                ) && 
                (
                    this.Count == other.Count ||
                    this.Count != null &&
                    this.Count.Equals(other.Count)
                ) && 
                (
                    this.IsApplied == other.IsApplied ||
                    this.IsApplied != null &&
                    this.IsApplied.Equals(other.IsApplied)
                ) && 
                (
                    this.Labels == other.Labels ||
                    this.Labels != null &&
                    this.Labels.SequenceEqual(other.Labels)
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

                if (this.Value != null)
                    hash = hash * 59 + this.Value.GetHashCode();

                if (this.Count != null)
                    hash = hash * 59 + this.Count.GetHashCode();

                if (this.IsApplied != null)
                    hash = hash * 59 + this.IsApplied.GetHashCode();

                if (this.Labels != null)
                    hash = hash * 59 + this.Labels.GetHashCode();

                return hash;
            }
        }

    }
}
