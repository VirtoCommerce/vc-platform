using System;
using System.Linq;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;



namespace VirtoCommerce.Client.Model
{

    /// <summary>
    /// 
    /// </summary>
    [DataContract]
    public class VirtoCommerceMerchandisingModuleWebModelFacetValue : IEquatable<VirtoCommerceMerchandisingModuleWebModelFacetValue>
    {
        
        /// <summary>
        /// Gets or sets the facet value count
        /// </summary>
        /// <value>Gets or sets the facet value count</value>
        [DataMember(Name="count", EmitDefaultValue=false)]
        public int? Count { get; set; }
  
        
        /// <summary>
        /// Gets or sets the flag for facet value is applied
        /// </summary>
        /// <value>Gets or sets the flag for facet value is applied</value>
        [DataMember(Name="isApplied", EmitDefaultValue=false)]
        public bool? IsApplied { get; set; }
  
        
        /// <summary>
        /// Gets or sets the facet value label
        /// </summary>
        /// <value>Gets or sets the facet value label</value>
        [DataMember(Name="label", EmitDefaultValue=false)]
        public string Label { get; set; }
  
        
        /// <summary>
        /// Gets or sets the facet value
        /// </summary>
        /// <value>Gets or sets the facet value</value>
        [DataMember(Name="value", EmitDefaultValue=false)]
        public Object Value { get; set; }
  
        
  
        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class VirtoCommerceMerchandisingModuleWebModelFacetValue {\n");
            sb.Append("  Count: ").Append(Count).Append("\n");
            sb.Append("  IsApplied: ").Append(IsApplied).Append("\n");
            sb.Append("  Label: ").Append(Label).Append("\n");
            sb.Append("  Value: ").Append(Value).Append("\n");
            
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
            return this.Equals(obj as VirtoCommerceMerchandisingModuleWebModelFacetValue);
        }

        /// <summary>
        /// Returns true if VirtoCommerceMerchandisingModuleWebModelFacetValue instances are equal
        /// </summary>
        /// <param name="obj">Instance of VirtoCommerceMerchandisingModuleWebModelFacetValue to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(VirtoCommerceMerchandisingModuleWebModelFacetValue other)
        {
            // credit: http://stackoverflow.com/a/10454552/677735
            if (other == null)
                return false;

            return 
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
                    this.Label == other.Label ||
                    this.Label != null &&
                    this.Label.Equals(other.Label)
                ) && 
                (
                    this.Value == other.Value ||
                    this.Value != null &&
                    this.Value.Equals(other.Value)
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
                
                if (this.Count != null)
                    hash = hash * 57 + this.Count.GetHashCode();
                
                if (this.IsApplied != null)
                    hash = hash * 57 + this.IsApplied.GetHashCode();
                
                if (this.Label != null)
                    hash = hash * 57 + this.Label.GetHashCode();
                
                if (this.Value != null)
                    hash = hash * 57 + this.Value.GetHashCode();
                
                return hash;
            }
        }

    }


}
